using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using Microsoft.AspNet.Identity;

namespace Blog.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = new ApplicationUser();

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
            Post post = db.Posts.Include(p => p.Image).SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            DateTime dt = DateTime.Now.Date;
            ViewBag.Date = $"{dt:yyyy-MM-dd}";
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Body,Date,AuthorId,Images")] Post post, HttpPostedFileBase upload)
        {
            if (!ModelState.IsValid) return View(post);

            if (upload != null && upload.ContentLength > 0)
            {
                var picture = new File
                {
                    FileName = System.IO.Path.GetFileName(upload.FileName),
                    FileType = FileType.Picture,
                    ContentType = upload.ContentType
                };
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    picture.Content = reader.ReadBytes(upload.ContentLength);
                }
                post.Image = new List<File> { picture };
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
            Post post = db.Posts.Include(p => p.Image).SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                return HttpNotFound();
            }

            var authors = db.Users.ToList().OrderByDescending(u => u.Id == post.AuthorId);
            DateTime dt = DateTime.Now.Date;

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
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Body,Date,AuthorId,Images")] Post post, HttpPostedFileBase upload)
        {
            if (!ModelState.IsValid) return View(post);
            var postToUpdate = db.Posts.FirstOrDefault(p => p.Id == post.Id);

            if (upload != null && upload.ContentLength > 0)
            {
                if (postToUpdate != null && postToUpdate.Image.Any(f => f.FileType == FileType.Picture))
                {
                    db.Files.Remove(postToUpdate.Image.First(f => f.FileType == FileType.Picture));
                }

                var picture = new File
                {
                    FileName = System.IO.Path.GetFileName(upload.FileName),
                    FileType = FileType.Picture,
                    ContentType = upload.ContentType
                };

                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    picture.Content = reader.ReadBytes(upload.ContentLength);
                }
                postToUpdate.Image = new List<File> { picture };
            }

            post = postToUpdate;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
        
    }
}
