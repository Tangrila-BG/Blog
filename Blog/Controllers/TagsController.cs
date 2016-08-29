using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Blog.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Blog.Controllers
{
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Post Tags
        [HttpGet]
        public ActionResult PostTags(Post post)
        {
            var postTags = db.PostTags.Where(tag => tag.PostId == post.Id).ToList();
            return Json(postTags);
        }

        // POST: Tags
        // Receives data from the client and sends it to
        // Posts/Create controller/action to use
        [HttpPost]
        public ActionResult Tags(Test model)
        {
            // Data comes in format: "[\"37\",\"46\"]"
            // Separates each number into element of array
            // eg. e0: "[\"37\", e1: 
            var test = model.Ids[0].Split(',');

            // From each string takes only the number and parses it as such
            int[] ids = test.Select(s => int.Parse(Regex.Match(s , @"\d+").Value)).ToArray();
            TempData["tagIds"] = ids;
            return RedirectToAction("Create" , "Posts");
        }
    }
}