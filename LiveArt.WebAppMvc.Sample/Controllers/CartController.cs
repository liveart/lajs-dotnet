using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using LiveArt.Design.PostProcessing.Packer;
using LiveArt.Design.PostProcessing.PackerExtentions;
using LiveArt.WebAppMvc.Sample.Models;
using LiveArt.WebAPI.Sample.Controllers;
using LiveArt.WebAPI.Sample.Repositories;
using JDesign=LiveArt.Data.Json.Design;

namespace LiveArt.WebAppMvc.Sample.Controllers
{
    public class CartController:Controller
    {
        private IDesignRepository Repository;
        /* In production code dependency-injection will be better (Unity,Ninject,Autofac e.t.c)
      * see http://www.asp.net/web-api/overview/advanced/dependency-injection
      *    
     public DesignController(IDesignRepository repository){
         this.Repository=repository;
     }
      */

        public CartController()
        {
            // Repository=new DesignRepositorySimple(); // store all designs into the same folder
            // use the same repository as in LiveArt.WebAPI.Controllers.DesignController
            Repository = new DesignRepositoryByDate(); // store designs to date-based sub-folders
            
        }

        #region ShowDetails
        public ActionResult ShowDesignDetails(string designId)
        {
            if (!this.Repository.DesignExists(designId)) throw new DesignController.DesignNotFoundException(designId);


            // in real project, get detail via DesignService (not repository), map it to DesignViewModel and use it in view
            var designJson = this.Repository.GetJson(designId);
            var design = JDesign.Design.Parse(designJson);

            var viewModel = ToViewModel(designId,design);
            viewModel.DesignId = designId;

            //pack to .zip
            var packedResult = PrepareDesignPack(designId, designJson);
            viewModel.ZipFileUrl = packedResult.ZipFile.Url;

            return View(viewModel);
        }

        #region MappingToViewModel 
        // in real project we recomend use AutoMapper for such tasks
        private DesignDetailsViewModel ToViewModel(string designId,JDesign.Design design)
        {
            
            return new DesignDetailsViewModel()
            {
                Product = ToViewModel(design.Product),
                Locations=new List<LocationViewModel>(design.Locations.Select((loc)=>ToViewModel(designId,loc))),
                Prices = new List<PriceRowViewModel>(design.Prices.Select(ToViewModel))
            };
                
        }

        private ProductViewModel ToViewModel(JDesign.Product product)
        {
            Func<ColorViewModel> getColorViewModel = () =>
            {
                if (string.IsNullOrEmpty(product.ColorName)) return null;
                return new ColorViewModel()
                {
                    Name = product.ColorName,
                    HtmlHexValue = product.ColorValue.ToHtmlHexString()
                };
            };


            return new ProductViewModel()
            {
                Name = product.Name,
                Color = getColorViewModel()
            };
        }

        private LocationViewModel ToViewModel(string designId,JDesign.Location location)
        {
            return new LocationViewModel()
            {
                Name = location.Name,
                SvgPreviewUrl = GetSvgPreviewUrl(designId,location,false),
                SvgPreviewDownloadUrl = GetSvgPreviewUrl(designId,location,true)
            };
        }

        private PriceRowViewModel ToViewModel(JDesign.Price price)
        {
            return new PriceRowViewModel()
            {
                Label = price.Label,
                Value = price.Value,
                IsTotal = price.IsTotal
            };
        }

        private string GetSvgPreviewUrl(string designId,JDesign.Location location,bool forceDownload = false)
        {
            var locationRouteName = LiveArt.WebAPI.Sample.WebApiConfig.LIVEART_LOCATIONS_ROUTE_NAME;
            var previewUrl = Url.RouteUrl(locationRouteName, routeValues: new
            {
                httpRoute = true,
                controller = "Preview",
                action = "Svg",
                locationName = location.Name,
                designID = designId,
                forceDownload = forceDownload
            });
            return previewUrl;
        }




        #endregion MappingToViewModel

        #endregion ShowDetails

        #region PackDesignToZip
        public ActionResult PackDesign(string id, bool showLog = false)
        {
            var designId = id;

            var designJson = this.Repository.GetJson(designId);
            
            //Do Design Packing
            var packedDesign = PrepareDesignPack(designId,designJson);

            //Show result to UI
            ViewBag.DesignId = designId;
            ViewBag.ShowLog = showLog;

            return View(packedDesign);
        }

        private DesignPack PrepareDesignPack(string designId, string designJson)
        {
            var packerPaths = BuildPackerPath(designId);

            //Do Design Packing
            var designPacker = BuildDesignPacker(packerPaths, designId, designJson);
            var packedDesign = designPacker.PackTo(packerPaths.DesignFolderFullPath);
            return packedDesign;
        }

        private IDesignPacker BuildDesignPacker(PackerPaths packerPaths,string designId,string designJson)
        {
            return DesignPacker
                //configure action on PackTo 
                .FromJsonStr(designJson)
                //alternative: .FromJsonFile(soruceDesignFilePath)

                .DownloadImages(packerPaths.BaseUrlForRelativeImages,
                    (imageUrl) => // url to relative file name resolver
                    {
                        return null; // null means "can't extract file path, use default name generation 
                    })

                .UpdateViewBox()
                .SaveSvg() //default fileNamePath: "[safeLocationName].svg"
                .GeneratePng(AppSettings.InkscapeExePath)

                .LoadConfig(packerPaths.ConfigFilePath)
                .UpdateUnits()
                .SaveSvg(safeLocationName => $"{safeLocationName}-UNITS.svg")
                .GeneratePdf(AppSettings.InkscapeExePath)

                .ZipTo($"design.{designId}.zip")

                .ResolveUrls(localFileName => Url.Content(Path.Combine(packerPaths.DesignFolderRelativePath, localFileName)));
        }

        private PackerPaths BuildPackerPath(string designId)
        {
            var result = new PackerPaths();
            result.DesignFolderRelativePath = Path.Combine("~/files/packedDesigns", designId);// in real app "id" should be safe for injection
            result.DesignFolderFullPath = MapPath(result.DesignFolderRelativePath);
            result.ConfigFilePath = MapPath("~/_LiveArtJS/config/config.json");
           
            //base url for images like src="gallery/someimg.png" in SOURCE design
            //"https://localhost/_LiveArtJS/";
            result.BaseUrlForRelativeImages = new Uri(Request.Url, Url.Content("~/_LiveArtJS")).AbsoluteUri;
            return result;

        }

        private class PackerPaths
        {
            public string DesignFolderRelativePath { get; set; }
            public string DesignFolderFullPath { get; set; }
            public string ConfigFilePath { get; set; }
            public string BaseUrlForRelativeImages { get; set; }

        }
        #endregion PackDesignToZip

        #region Utils
        private string MapPath(string path)
        {// short alias
            return HostingEnvironment.MapPath(path);
        }

        #endregion

    }
}