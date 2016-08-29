using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using Microsoft.Ajax.Utilities;
using File = Blog.Models.File;

namespace Blog.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

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

            if (!ModelState.IsValid) return View(post);
           
            // image handling
            if (upload != null && upload.ContentLength > 0)
            {
                var image = new File
                {
                    FileName = System.IO.Path.GetFileName(upload.FileName),
                    FileType = FileType.Image,
                    ContentType = upload.ContentType
                };
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    image.Content = reader.ReadBytes(upload.ContentLength);
                }
                post.Files = new List<File> { image };
            }

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
            return RedirectToAction("Index");
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

            var authors = db.Users.ToList().OrderByDescending(u => u.Id == post.AuthorId);

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
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Body,Date,AuthorId,Files")] Post post, HttpPostedFileBase upload)
        {
            string[] validImageTypes =
                    {
                        "image/gif" ,
                        "image/jpeg" ,
                        "image/pjpeg" ,
                        "image/png"
                    };
            //if (post.Files == null || post.Files.Count == 0)
            //    ModelState.AddModelError("Files", "This field is required");
            

            if (ModelState.IsValid)
            {
                var postToUpdate = db.Posts.Include(f => f.Files).SingleOrDefault(p => p.Id == post.Id);
               
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
                post = postToUpdate;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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

        // HELPERS

        private ApplicationDbContext db = new ApplicationDbContext();
        private DateTime dt = DateTime.Now.Date;

        private Dictionary<int , string> TagsByIdAndName()
        {
            return db.Tags.Select(t => new { t.Id, t.Name }).OrderBy(x => x.Name).ToDictionary(t => t.Id, t => t.Name);
        }

    }
}
