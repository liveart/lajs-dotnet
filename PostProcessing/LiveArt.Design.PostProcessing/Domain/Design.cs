using System.Collections.Generic;
using Newtonsoft.Json;

namespace LiveArt.Design.PostProcessing.Domain
{
    internal class Design
    {
        public IEnumerable<Location> locations { get; set; }
        public Product product { get; set; }

        internal static Design GeDesignFromJsonStr(string jsonStr){
            var dataContainer = JsonConvert.DeserializeObject<DataContainer>(jsonStr);
             return dataContainer.data;
        }
    }
}
