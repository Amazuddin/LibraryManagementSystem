using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using LibraryManagementSystem.Context;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        //
        // GET: /Authentication/

        private readonly LibraryContext db = new LibraryContext();

        public static string EncodePassword(string password)
        {
            byte[] originalBytes;
            byte[] encodedBytes;
            MD5 a;
            a = new MD5CryptoServiceProvider();
            originalBytes = Encoding.Default.GetBytes(password);
            encodedBytes = a.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }

        public ActionResult Register()
        {
            ViewBag.Register = "active";
            if (Session["CurrentUserId"] != null) return RedirectToAction("Index", "Primary");
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            ViewBag.Register = "active";
            var emailExist = db.Users.FirstOrDefault(i => i.Email == user.Email);
            if (emailExist == null)
            {
                user.Password = EncodePassword(user.Password);
                db.Users.Add(user);
                db.SaveChanges();
            }
            else
            {
                ViewBag.Error = "Another User is Registered with this Email";
                return View();
            }

            return RedirectToAction("Login");
        }

        public ActionResult Login(string id)
        {
            ViewBag.Login = "active";
            if (Session["CurrentUserId"] != null) return RedirectToAction("Index", "Primary");
            ViewBag.Error = id;
            return View();
        }

        public ActionResult LoginUser(string email, string password)
        {
            var userPassword = EncodePassword(password);
            var hs = db.Users.FirstOrDefault(r => r.Email == email && r.Password == userPassword);
            if (hs != null)
            {
                Session["CurrentUserId"] = hs.Id;
                Session["LoginUserName"] = hs.Email;
                return RedirectToAction("Index", "Primary");
            }

            return RedirectToAction("Login", "Authentication", new { id = "Error" });
        }

        public ActionResult Logout()
        {
            Session["CurrentUserId"] = null;
            return RedirectToAction("Login");
        }
    }
}