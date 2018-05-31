using System.Collections.Generic;

namespace LiveArt.Data.Json.Design
{

    public class DesignsListResponse
    {
        public IEnumerable<DesignDescriptor> Designs { get; set; }

        public DesignsListResponse(IEnumerable<DesignDescriptor> design)
        {
            this.Designs = design;
        }
    }
}