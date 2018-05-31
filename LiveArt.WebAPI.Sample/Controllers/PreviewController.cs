using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http;
using LiveArt.Data.Json.Design;
using LiveArt.WebAPI.Sample.Repositories;
using LiveArt.WebAPI.Sample.Utils;

namespace LiveArt.WebAPI.Sample.Controllers
{
    public class PreviewController : ApiController
    {
        private IDesignRepository Repository;

        public PreviewController() : this(new DesignRepositoryByDate())
        {
        }
        public PreviewController(IDesignRepository repository )
        {
            this.Repository=repository;
        }

        [HttpGet]
        public RawSvgResponse Svg(string designID, string locationName,bool forceDownload=false)
        {
            if (!this.Repository.DesignExists(designID)) throw new DesignController.DesignNotFoundException(designID);

            var designJson = this.Repository.GetJson(designID);
            var design = Design.Parse(designJson);
            var location = design.Locations.FirstOrDefault(l=>string.Compare(l.Name,locationName,true)==0);
            if (location == null) throw new LocationNotFoundException(designID, locationName);
            string svgFileName = string.Format("{0}.{1}.svg", designID, locationName);

            foreach (char c in System.IO.Path.GetInvalidFileNameChars()) svgFileName = svgFileName.Replace(c, '-');

            var locationSvg = location.SVG;
            locationSvg = FixRelativeImagesUrls(locationSvg);
            return new RawSvgResponse(locationSvg, svgFileName, forceDownload);

        }
        #region Fix urls
        public static Regex hrefRegex = new Regex("\\s(xlink:)?href\\s?=\\s?\"(?<url>.*?)\"", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        private string FixRelativeImagesUrls(string svgStr)
        {
            return hrefRegex.Replace(svgStr,(m) =>
            {
                
                var origUrl = m.Groups["url"].Value;
                var newUrl = origUrl
                   .Replace("../_LiveArtJS/", Url.Content("~/_LiveArtJS/"))
                    .Replace("../../", Url.Content("~/_LiveArtJS/"));
                string absoluteUrl = new Uri(Request.RequestUri, newUrl).AbsoluteUri;

                return m.Value.Replace(origUrl, absoluteUrl);
            });
        }

      



        #endregion


        #region Exceptions
        public class LocationNotFoundException : LiveArtException
        {
            public string DesignID;
            public string LocationName;
            public LocationNotFoundException(string designID,string locationName)
                : base(string.Format("Location '{1}' not found in designID='{0}' not found", designID, locationName), HttpStatusCode.NotFound)
            {
                this.DesignID = designID;
                this.LocationName = locationName;
            }
        }
        #endregion
    }
}