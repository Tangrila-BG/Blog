using System.Linq;
using System.Web.Mvc;
using Blog.Models;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var authors = db.Users;
            ViewBag.Authors = authors;
            return View(db.Posts.ToList());
        }
    }
}