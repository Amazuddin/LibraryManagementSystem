using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.Context;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class BookInfoesController : Controller
    {
        private LibraryContext db = new LibraryContext();

        // GET: BookInfoes
        public ActionResult Index()
        {
            ViewBag.BookInfo = "active";
            return View(db.BookInfos.ToList());
        }

        // GET: BookInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInfo bookInfo = db.BookInfos.Find(id);
            if (bookInfo == null)
            {
                return HttpNotFound();
            }
            return View(bookInfo);
        }

        // GET: BookInfoes/Create
        public ActionResult Create()
        {
            BookInfo book = new BookInfo();
            return View(book);
        }

        // POST: BookInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BookName,AuthorName,Status")] BookInfo bookInfo)
        {
            if (ModelState.IsValid)
            {
                db.BookInfos.Add(bookInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookInfo);
        }

        // GET: BookInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInfo bookInfo = db.BookInfos.Find(id);
            if (bookInfo == null)
            {
                return HttpNotFound();
            }
            return View(bookInfo);
        }

        // POST: BookInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookName,AuthorName,Status")] BookInfo bookInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookInfo);
        }

        // GET: BookInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookInfo bookInfo = db.BookInfos.Find(id);
            if (bookInfo == null)
            {
                return HttpNotFound();
            }
            return View(bookInfo);
        }

        // POST: BookInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookInfo bookInfo = db.BookInfos.Find(id);
            var borrowInfo = db.BorrowInfos.FirstOrDefault(i => i.BookInfoId == bookInfo.Id);
            if (borrowInfo != null)
            {
                ViewBag.Error = "This Book Already Used. So, Can't Delete";
                return RedirectToAction("Delete", new { message = "This Book Already Used. So, Can't Delete" });
            }
            db.BookInfos.Remove(bookInfo);
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
