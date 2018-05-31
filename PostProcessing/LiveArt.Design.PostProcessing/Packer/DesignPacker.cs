using System;
using System.Collections.Generic;
using System.Linq;
using LiveArt.Design.PostProcessing.Logging;

namespace LiveArt.Design.PostProcessing.Packer
{
    public interface IDesignPacker
    {
        IDesignPacker ResolveUrls(Func<string,string> fileToUrlResolver);
        DesignPack PackTo(string workingFolderFullPath);
    }

    internal interface IDesignPackerWithActions : IDesignPacker // for moq
    {
        IDesignPacker RegBeforePackAction(Action<PackingContext> beforePackAction);
    }

    internal static class IDesignPackerExtentions //helper for xxxDesignPackerExtentions
    {
        internal static IDesignPacker RegBeforePackAction(this IDesignPacker packer,Action<PackingContext> beforePackAction)
        {
            if (packer is IDesignPackerWithActions) ((IDesignPackerWithActions)packer).RegBeforePackAction(beforePackAction); // for moq
            else ((DesignPacker)packer).RegBeforePackAction(beforePackAction); // for moq
            
            return packer;
        }

        internal static IDesignPacker PackTo(this IDesignPacker packer, IWorkingFolder workingFolder)
        {
            ((DesignPacker)packer).PackTo(workingFolder);
            return packer;
        }
        
    }

    public class DesignPacker : IDesignPacker
    {

        private IList<Action<PackingContext>> OnPackActions;
        internal PackingContext PackContext;
        
        public DesignPacker()
        {
            this.OnPackActions = new List<Action<PackingContext>>();
        }

        
        internal IDesignPacker FromDesign(Domain.Design savedDesign){
            this.PackContext = new PackingContext()
            {
                CurrentDesign = savedDesign
            };
            return this;
        }

        internal IDesignPacker RegBeforePackAction(Action<PackingContext> beforePackAction)
        {
            this.OnPackActions.Add(beforePackAction);
            return this;
        }

        public IDesignPacker  ResolveUrls(Func<string,string> fileToUrlResolver){
            this.PackContext.ResolvePathToUrlCallback = fileToUrlResolver;
            return this;
        }

        public DesignPack PackTo(string workingFolderFullPath)
        {

            var workingFolder = new WorkingFolder(workingFolderFullPath);
            return PackTo(workingFolder);

        }

        internal DesignPack PackTo(IWorkingFolder workingFolder)
        {

            workingFolder.EnsureExists();
            this.PackContext.WorkingFolder = workingFolder;
            PreparePackResultBeforeActions();

              this.DoActionWithLog("PackTo", workingFolder.GetFullPath(string.Empty), () =>
            {
                // execute pended actions
                foreach (var action in this.OnPackActions)
                {
                    action(this.PackContext);
                }
            });

              PreparePackResultAfterActions();
              return this.PackContext.Result;

        }



        private void PreparePackResultBeforeActions()
        {
            var result=new DesignPack();
            this.PackContext.Result = result;

            result.Locations = (from inLocation in this.PackContext.CurrentDesign.locations // todo:remove it from here
                                select new DesignPack.Location()
                               {
                                   Name = inLocation.name
                               }).ToArray();
            
        }
        private void PreparePackResultAfterActions()
        {
            
            var result = this.PackContext.Result;
            result.LogLines = this.PackContext.Logger.GetLog();
            
        }

        private string GetFullPath(string localPath)
        {
            return this.PackContext.WorkingFolder.GetFullPath(localPath);
        }


        private void DoActionWithLog(string actionName, string targetName, Action action)
        {
           this.PackContext.Logger.DoActionWithLog(actionName, targetName, action);
        }

      

        #region Static Builder
        public static IDesignPacker FromJsonStr(string designJsonStr) // build packer with all default implementations
        {

            var packer = new DesignPacker();

            return packer.FromDesign(Domain.Design.GeDesignFromJsonStr(designJsonStr));
        }

        public static IDesignPacker FromJsonFile(string designJsonFileName)
        {
            var designJson = System.IO.File.ReadAllText(designJsonFileName);
            return DesignPacker.FromJsonStr(designJson );
                
        }
        #endregion

      

        
    }
}
