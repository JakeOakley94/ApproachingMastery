using DatabaseInteraction.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApproachingMastery.Controllers
{
    public class ImageController : Controller
    {
        public ActionResult Show(Guid imageID)
        {
            Picture picture = Picture.GetImageFromDatabase(imageID);

            string extension = new FileInfo(picture.ImageName).Extension.Substring(1);
            return File(picture.Image, $"image/{extension}");
        }
    }
}