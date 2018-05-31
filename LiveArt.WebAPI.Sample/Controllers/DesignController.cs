using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using LiveArt.Data.Json.Design;
using LiveArt.WebAPI.Sample.Repositories;
using LiveArt.WebAPI.Sample.Utils;

namespace LiveArt.WebAPI.Sample.Controllers
{
    public class DesignController : ApiController
    {
        private IDesignRepository Repository;

        public DesignController(){
           // Repository=new DesignRepositorySimple(); // store all designs into the same folder
            Repository = new DesignRepositoryByDate(); // store designs to date-based sub-folders

            //note: if you change
        }

        /* In production code dependency-injection will be better (Unity,Ninject,Autofac e.t.c)
         * see http://www.asp.net/web-api/overview/advanced/dependency-injection
         *    
        public DesignController(IDesignRepository repository){
            this.Repository=repository;
        }
         */


        [HttpGet]
        public DesignsListResponse List(string email)
        {
            var allDesigns = this.Repository.GetDesigns();
            var designsForThisEmail = allDesigns.Where(d => d.Email == email);
            return new DesignsListResponse(designsForThisEmail);
        }

        [HttpGet]
        public RawJsonResponse Get(string id) 
        {
            if(this.Repository.DesignExists(id)){
                var designJsonStr = this.Repository.GetJson(id);
                return new RawJsonResponse(designJsonStr);
            } else throw new DesignNotFoundException(id);

        }

        [HttpPost]
        public DesignCreatedResponse Save(SaveDesignRequest saveRequest) 
        {

            var newDesign = new DesignDescriptor()
            {
                ID = Guid.NewGuid().ToString(),
                Title = saveRequest.Title,
                Date = DateTime.Now.ToString(),
                Type = saveRequest.Type,
                Email = saveRequest.Email
            };

            this.Repository.AddDesign(newDesign, saveRequest.Data);
            return new DesignCreatedResponse
            {
                Design = newDesign
            };
        }

        #region DTO objects
            public class SaveDesignRequest{
              //  public  JObject data{get;set;} // design json
                public string Title { get; set; }
                public string Data { get; set; } // DesignJSON
                public string Type { get; set; }
                public string Email { get; set; }
            }

            public class DesignCreatedResponse
            {
                public DesignDescriptor Design{get;set;}
            }
        #endregion

        #region Exceptions
        public class DesignNotFoundException:LiveArtException{
            public string DesignID;
            public DesignNotFoundException(string designID):base(string.Format("Design with ID='{0}' not found",designID),HttpStatusCode.NotFound)
            {
                this.DesignID=designID;
            }
        }
        #endregion


    }
}
