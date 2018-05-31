using System;
using System.Collections.Generic;
using LiveArt.Design.PostProcessing.Domain;
using LiveArt.Design.PostProcessing.Logging;

namespace LiveArt.Design.PostProcessing.Packer
{
    //Common context for all Svg,Images e.t.c process
    internal class PackingContext
    {


        public IWorkingFolder WorkingFolder{get;set;}// target folder
        
        // origin url/prefixed
        public string SourceImagesBaseUrl{get;set;} // for convert urls, like "someFolder/sample.png" to "http://somedomain/appFolder/someFolder/sample.png"

        public string TargetImagesFolder{get;set;}

        internal LoggerInMemory Logger = new LoggerInMemory();
        
        public Domain.Design CurrentDesign;

        public Location CurrentLocation;

        public Config Config;

        public DesignPack Result;
        public Func<string, string> ResolvePathToUrlCallback;
        public IEnumerable<Func<string, string>> ResolveUrlToRelativePathCallbacks;

        public string ResolveToUrl(string localPath)
        {
            if (localPath == null) return null;
            return ResolvePathToUrlCallback != null ? ResolvePathToUrlCallback(localPath) : localPath;
        }

        

    }
}
