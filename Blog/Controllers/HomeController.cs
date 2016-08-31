using System.Linq;
using System.Web.Mvc;
using Blog.Extensions;
using Blog.Models;

namespace Blog.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(bool success)
        {
            this.AddNotification(
               success
                   ? "Post created successfully"
                   : "Post created successfully, but since you did not upload an image we did it for you",
               NotificationType.SUCCESS);
            var authors = db.Users;
            ViewBag.Authors = authors;
            return View(db.Posts.ToList());
        }
    }
}