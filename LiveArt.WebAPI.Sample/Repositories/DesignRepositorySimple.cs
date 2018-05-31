using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveArt.Data.Json.Design;
using Newtonsoft.Json;

namespace LiveArt.WebAPI.Sample.Repositories
{
    /// <summary>
    ///   this implementation of IDesignRepository, save all designs into one folder, without any grouping e.t.c
    /// </summary>
    public class DesignRepositorySimple:IDesignRepository
    {
        const string DefaultStorePath = "~/files";
        const string DesignFileName = "design.json";
        const string LIST_FILE_NAME = "allDesigns.json";
        protected string BaseStorePath;

        public DesignRepositorySimple(string storePath=null)
        {
            this.BaseStorePath=(storePath!=null?storePath: System.Web.Hosting.HostingEnvironment.MapPath(DefaultStorePath));
        }

        protected virtual string GetStorePath(string designId){
            return this.BaseStorePath; // in Simple implementation all designs stored into the same folder, designID ignored
        }

#region implement IDesignRepository interface
        public string GetJson(string id)
        {
            var jsonFilePath = GetDesignFilePath(id);
            var jsonContent = File.ReadAllText(jsonFilePath);
            return jsonContent;
        }

        public DesignDescriptor GetDescriptor(string id)
        {
            return this.GetDesigns().FirstOrDefault(d => d.ID == id);
        }

        public void AddDesign(DesignDescriptor designDescriptor, string designJson)
        {
            SaveJsonToFile(designDescriptor.ID, designJson);

            // add descriptor to designs lists
            var designs = new List<DesignDescriptor>(this.LoadDesignsFromFile());
            designs.Add(designDescriptor);

            this.SaveDesignsToFile(designs);
        }

        public bool DesignExists(string id)
        {
            string designFilePath = GetDesignFilePath(id);
            return File.Exists(designFilePath);
        }

        public IEnumerable<DesignDescriptor> GetDesigns()
        {
            return this.LoadDesignsFromFile();
        }

#endregion

        

        #region Design JSON 

                protected virtual string GetDesignFilePath(string id) // TODO: rename it to ResolveDesignJsonPath
                {
                    var designFolder = Path.Combine(this.GetStorePath(id), id); // use designID as folder name
                    var designFilePath = Path.Combine(designFolder, DesignFileName);
                    return designFilePath;
                }


                private void SaveJsonToFile(string designID,string designJSON){
                    var designFilePath = GetDesignFilePath(designID);
                    CreateFolderFolderForPath(designFilePath);
                    File.WriteAllText(designFilePath,designJSON);
                }
         
        #endregion

        #region Design Descriptors

                private string GetListFilePath()
                {
                    return Path.Combine(this.BaseStorePath, LIST_FILE_NAME);
                }

                private IEnumerable<DesignDescriptor> LoadDesignsFromFile()
                {
                    var listFilePath = this.GetListFilePath();
                    IEnumerable<DesignDescriptor> allDesigns;

                    if (File.Exists(listFilePath))
                    {
                        var allDesignsJson = File.ReadAllText(listFilePath);
                        allDesigns = JsonConvert.DeserializeObject<IEnumerable<DesignDescriptor>>(allDesignsJson);
                    }
                    else allDesigns = new List<DesignDescriptor>();

                    return allDesigns;
                }

                private void SaveDesignsToFile(IEnumerable<DesignDescriptor> designs)
                {
                    var listFilePath = this.GetListFilePath();
                    var jsonString = JsonConvert.SerializeObject(designs);
                    File.WriteAllText(listFilePath, jsonString);
                }

                
            
        #endregion


        #region Utils
            private static void CreateFolderFolderForPath(string path)
            {
                string folderPath = Path.GetDirectoryName(path);
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            }

            

        #endregion

    }
}