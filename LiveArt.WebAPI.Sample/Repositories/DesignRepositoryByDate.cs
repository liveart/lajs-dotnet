using System;
using System.IO;
using System.Runtime.Caching;

namespace LiveArt.WebAPI.Sample.Repositories
{
    public class DesignRepositoryByDate:DesignRepositorySimple
    {
        private const string CACHE_PREFIX= "store_path.";
        private const string PREDEFINED_SUBFOLDER = "_PredefinedDesigns";
        private const string GROUPED_DESIGNS_FOLDER = "DesignsByDate";


        protected override string GetStorePath(string id){

            string storePath = GetStorePathFromCache(id);
            if (storePath == null)
            {
                storePath = Path.Combine(BaseStorePath, PREDEFINED_SUBFOLDER); // try find design in predefined folder
                if(!Directory.Exists(Path.Combine(storePath,id))){ // if not, generate path from design date
                    storePath = GetDateBasedStorePath(id);    
                }

                SaveStorePatchToCahce(id,storePath);
            }
            return storePath;
        }

        private string GetDateBasedStorePath(string id){
            var designDesc = this.GetDescriptor(id);
            var dt = designDesc != null ? DateTime.Parse(designDesc.Date) : DateTime.Now; // try get design date from repository, otherwise (new design) use today date
            string subFolderName = dt.ToString("yyyy-MM-dd");
            return Path.Combine(BaseStorePath, GROUPED_DESIGNS_FOLDER,subFolderName);
        }

        private string GetStorePathFromCache(string id){
            ObjectCache cache = MemoryCache.Default;

            return (string)cache.Get(CACHE_PREFIX+id);
        }

        private void SaveStorePatchToCahce(string id,string storePath)
        {
            ObjectCache cache = MemoryCache.Default; // you can use NamedCache to configure memory limits via web.config
            CacheItemPolicy policy = new CacheItemPolicy();
            cache.Set(CACHE_PREFIX+id, storePath, policy);
        }

    }
}