using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LibraryManagementSystem.Common;
using LibraryManagementSystem.Context;
using LibraryManagementSystem.Models;
using System.Data.SqlClient;

namespace LibraryManagementSystem.Controllers
{
    public class PrimaryController : Controller
    {
        //
        // GET: /Primary/
        private readonly LibraryContext db = new LibraryContext();

        public ActionResult Index()
        {
            if (Session["CurrentUserId"] == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            ViewBag.Borrow = "active";
  
            var bookInfos = db.BookInfos
                .Where(book => book.Status == CommonEnums.Status.Available)
                .ToList();

            ViewBag.bookInfos = bookInfos;
            return View();
        }


        [HttpPost]
        public ActionResult Index(BorrowInfo[] borrow)
        {

                int currentUser = Convert.ToInt32(Session["CurrentUserId"]);

                if (borrow == null)
                {
                    return Json("No Book Is Selected For Borrowing", JsonRequestBehavior.AllowGet);
                }

                if (borrow.Length > 3)
                {
                    return Json("Can not borrow more than 3 books", JsonRequestBehavior.AllowGet);
                }

                var borrowInfos = db.BorrowInfos
                    .Count(item => item.UserId == currentUser && item.IsReturn == CommonEnums.BooleanState.No);

                if (borrowInfos == 3)
                {
                    return Json("You already borrowed 3 books", JsonRequestBehavior.AllowGet);
                }

                if (borrow.Length > (3 - borrowInfos))
                {
                    return Json("You can borrow " + (3 - borrowInfos) + " book" , JsonRequestBehavior.AllowGet);
                }


                foreach (var item in borrow)
                {
                    BorrowInfo borrowInfo = new BorrowInfo
                    {
                        BookInfoId = item.BookInfoId,
                        UserId = currentUser
                    };
                    db.BorrowInfos.Add(borrowInfo);
                    db.SaveChanges();

                    var book = db.BookInfos.FirstOrDefault(bk => bk.Id == item.BookInfoId);
                    book.Status = CommonEnums.Status.NotAvailable;
                    db.SaveChanges();
                }

                return Json("Success! Borrow Is Complete!", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReturnBook()
        {
            if (Session["CurrentUserId"] == null)
                return RedirectToAction("Login", "Authentication");

            ViewBag.ReturnBook = "active";

            int currentUser = Convert.ToInt32(Session["CurrentUserId"]);

            var borrowList = db.Database.SqlQuery<ViewBorrowInfo>($"EXECUTE dbo.GetBorrowedBookData {currentUser}").ToList();

            //List<ViewBorrowInfo> borrowList = new List<ViewBorrowInfo>();

            //var query = (from book in db.BookInfos
            //             join borrow in db.BorrowInfos on book.Id equals borrow.BookInfoId
            //             where borrow.UserId == currentUser && borrow.IsReturn == CommonEnums.BooleanState.No
            //             select new
            //             {
            //                 bookInfoId = book.Id,
            //                 borrowInfoId = borrow.Id,
            //                 bookName = book.BookName,
            //                 authorName = book.AuthorName
            //             });

            //foreach (var item in query)
            //{
            //    ViewBorrowInfo borrowInfo = new ViewBorrowInfo()
            //    {
            //        AuthorName = item.authorName,
            //        BookName = item.bookName,
            //        BookInfoId = item.bookInfoId,
            //        BorrowInfoId = item.borrowInfoId
            //    };
            //    borrowList.Add(borrowInfo);
            //}

            ViewBag.borrowInfos = borrowList;
            return View();
        }


        public ActionResult ChangeStatus(long id)
        {
            if (Session["CurrentUserId"] == null)
                return RedirectToAction("Login", "Authentication");

            var borrowInfo = db.BorrowInfos.FirstOrDefault(item => item.Id == id);
            if (borrowInfo != null)
            {
                var book = db.BookInfos.FirstOrDefault(bk => bk.Id == borrowInfo.BookInfoId);
                borrowInfo.IsReturn = CommonEnums.BooleanState.Yes;
                book.Status = CommonEnums.Status.Available;
                db.SaveChanges();
            }

            return Json("Success! Return Complete!", JsonRequestBehavior.AllowGet);
        }
    }
}