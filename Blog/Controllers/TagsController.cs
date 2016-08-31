using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Blog.Models;

namespace Blog.Controllers
{
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // POST: Tags
        // Receives data from the client and sends it to
        // Posts/Create controller/action to use
        [HttpPost]
        public ActionResult Tags(Test model)
        {
            // Data comes in format: "[\"37\",\"46\"]"
            // Separates each number into element of array
            // eg. e0: "[\"37\", e1: 
            var temp = model.Ids[0].Split(',');

            // gets the url that requested this controller/action
            var requester = Request.UrlReferrer;
            
            // From each string takes only the number and parses it as such
            int[] ids = temp.Select(s => int.Parse(Regex.Match(s , @"\d+").Value)).ToArray();

            TempData["tagIds"] = ids;
            
            // depending on which view requested this controller action
            // sends the data to the view's corresponding controller/action
            var path = requester.Segments[1] + requester.Segments[2];
            if (path.ToLower() == "posts/edit/")
                return RedirectToAction("Edit", "Posts");

            return RedirectToAction("Create", "Posts");
        }
    }
}