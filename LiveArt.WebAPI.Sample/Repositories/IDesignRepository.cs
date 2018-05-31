using System.Collections.Generic;
using LiveArt.Data.Json.Design;

namespace LiveArt.WebAPI.Sample.Repositories
{
    public interface IDesignRepository
    {
        string GetJson(string id);
        bool DesignExists(string id);
        DesignDescriptor GetDescriptor(string id);

        void AddDesign(DesignDescriptor designDescriptor, string designJson);
        IEnumerable<DesignDescriptor> GetDesigns();
    }
}
