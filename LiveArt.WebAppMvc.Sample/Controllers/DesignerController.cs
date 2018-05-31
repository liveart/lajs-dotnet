using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiveArt.WebAppMvc.Sample.Controllers
{
    public class DesignerController:Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult IFrameIndex()
        {
            return View();
        }

    }
}