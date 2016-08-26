using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            Post post = db.Posts.Include(p => p.Files).SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.Date = $"{dt:yyyy-MM-dd}";
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Body,AuthorId,Files")] Post post, HttpPostedFileBase upload)
        {
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
    }
}
