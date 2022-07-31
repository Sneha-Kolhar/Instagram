using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using SocialSite.Models;

namespace SocialSite.Controllers
{
    public class AlbumController : Controller
    {
        public readonly ApplicationDbContext db;
        public readonly IHostingEnvironment host;

        public AlbumController(ApplicationDbContext _db,
             IHostingEnvironment ihostingEnvironment)
        {
            db = _db;
            host = ihostingEnvironment;
        }
        public IActionResult AddNewImage()
        {
            Gallery imageModel = new Gallery();
            return View("Image", imageModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewImage([Bind("ImageId,Title,ImageFile")] Gallery imageModel)
        {
            if (ModelState.IsValid)
            {
                string folder = "/Gallery/";
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string path = Path.Combine(host.WebRootPath + folder, fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageModel.ImageFile.CopyToAsync(fileStream);
                }
                db.Add(imageModel);
                await db.SaveChangesAsync();
            }
            return View(imageModel);
        }
    }
}
