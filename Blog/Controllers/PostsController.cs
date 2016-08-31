using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog.Extensions;
using Blog.Models;
using Microsoft.AspNet.Identity;
using File = Blog.Models.File;

namespace Blog.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        

        // GET: Posts
        [Authorize(Roles = "Administrators")]
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }
        [AllowAnonymous]
        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(p => p.Files).Include(p => p.PostTags).SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var tags = db.Tags.ToList();
            var postTags = (from tag in post.PostTags from t in tags where tag.TagId == t.Id select t.Name).ToList();
            ViewBag.Tags = postTags;
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            
            ViewBag.Tags = TagsByIdAndName();
            ViewBag.Date = $"{dt:yyyy-MM-dd}";
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Body,AuthorId,Files,TagIds")]
            Post post, HttpPostedFileBase upload)
        {
            // Receives ready-to-use Data from Tags/Tags controller/action
            // containing the ids of the selected tags to add to the post
            int[] tagIds = TempData["tagIds"] as int[];

            ViewBag.Tags = TagsByIdAndName();

            if (!ModelState.IsValid)
            {
                this.AddNotification("Post creation unsuccessful", NotificationType.ERROR);
                return View(post);
            }

            // image handling
            var defImagePath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "/images/default.jpg";
            // default image
            var image = new File
            {
                FileName = System.IO.Path.GetFileName(defImagePath),
                FileType = FileType.Image,
                ContentType = "image/jpeg",
                Content = System.IO.File.ReadAllBytes(defImagePath)
            };
            // user image
            if (upload != null && upload.ContentLength > 0)
            {
                image = new File
                {
                    FileName = System.IO.Path.GetFileName(upload.FileName),
                    FileType = FileType.Image ,
                    ContentType = upload.ContentType
                };

                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    image.Content = reader.ReadBytes(upload.ContentLength);
                }

            }

            post.Files = new List<File> { image };

            if (tagIds != null && tagIds.Length > 0)
            {
                foreach (var tagId in tagIds)
                {
                    var postTag = new PostTag
                    {
                        Post = post,
                        PostId = post.Id,
                        TagId = tagId
                    };
                    db.PostTags.Add(postTag);
                }
            }
            
            post.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            db.Posts.Add(post);
            db.SaveChanges();
            bool success = upload != null;
            return RedirectToAction("Index", "Home", new { success });
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(p => p.Files).SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                return HttpNotFound();
            }
       
            var currentUserId = User.Identity.GetUserId();
            if ((post.AuthorId == null || post.AuthorId != currentUserId) && !User.IsInRole("Administrators"))
            {
                this.AddNotification("Insufficient rights", NotificationType.WARNING);
                return RedirectToAction("Login", "Account");
            }
            
            var authors = db.Users.ToList().OrderByDescending(u => u.Id == post.AuthorId);

            ViewBag.Tags = TagsByIdAndName();
            ViewBag.PostTags = TagsOfPost(post);
            ViewBag.Authors = authors;
            ViewBag.Author = post.Author;
            ViewBag.Date = $"{dt:yyyy-MM-dd}";

            return View(post);
            
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Body,Date,AuthorId,Files,PostTags")] Post post, HttpPostedFileBase upload)
        {
            ViewBag.Tags = TagsByIdAndName();

            int[] tagIds = TempData["tagIds"] as int[];

            string[] validImageTypes =
                    {
                        "image/gif" ,
                        "image/jpeg" ,
                        "image/pjpeg" ,
                        "image/png"
                    };

            var postToUpdate = db.Posts.Include(f => f.Files).Include(p => p.PostTags).SingleOrDefault(p => p.Id == post.Id);

            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    if (!validImageTypes.Contains(upload.ContentType))
                    {
                        ModelState.AddModelError("Files", "Please choose either a GIF, JPG or PNG image.");
                        return View(postToUpdate);
                    }

                    if (postToUpdate.Files != null && postToUpdate.Files.Any(f => f.FileType == FileType.Image))
                    {
                        db.Files.Remove(postToUpdate.Files.First(f => f.FileType == FileType.Image));
                    }

                    var image = new File
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName) ,
                        FileType = FileType.Image ,
                        ContentType = upload.ContentType
                    };


                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        image.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    postToUpdate.Files = new List<File> {image};
                }
                /* EDIT TAGS - HAS DEFECT Saves tags to post correctly, but
                 * also makes a new post that is a duplicate of the edited one
                 *
                if (tagIds != null && tagIds.Length > 0)
                {
                    RemoveTagsOfPost(postToUpdate);
                    postToUpdate.PostTags.Clear();
                    foreach (var tagId in tagIds)
                    {
                        var postTag = new PostTag
                        {
                            Post = post,
                            PostId = post.Id,
                            TagId = tagId
                        };

                        db.PostTags.Add(postTag);
                        postToUpdate.PostTags.Add(postTag);
                    }
                }
                */
                this.AddNotification("Post Edited successfully", NotificationType.SUCCESS);
                post = postToUpdate;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Posts", new { post.Id });
            }
            this.AddNotification("Post Edit unsuccessful", NotificationType.ERROR);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var currentUserId = User.Identity.GetUserId();
            if ((post.AuthorId == null || post.AuthorId != currentUserId) && !User.IsInRole("Administrators"))
            {
                this.AddNotification("Insufficient rights", NotificationType.WARNING);
                return RedirectToAction("Login", "Account");
            }
            this.AddNotification("Post Deleted successfully", NotificationType.SUCCESS);
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region HELPERS

        private ApplicationDbContext db = new ApplicationDbContext();
        private DateTime dt = DateTime.Now.Date;

        private Dictionary<int , string> TagsByIdAndName()
        {
            return db.Tags.Select(t => new { t.Id, t.Name }).OrderBy(x => x.Name).ToDictionary(t => t.Id, t => t.Name);
        }

        private List<int> TagsOfPost(Post post)
        {
            var list = db.PostTags.Where(t => t.PostId == post.Id).Select(t => t.TagId).ToList();
            return list;
        }

        private void RemoveTagsOfPost(Post post)
        {
            var tags = db.PostTags.Where(t => t.PostId == post.Id);
            foreach (var postTag in tags)
            {
                db.PostTags.Remove(postTag);
            }
            db.SaveChanges();
        }

        #endregion

    }
}
