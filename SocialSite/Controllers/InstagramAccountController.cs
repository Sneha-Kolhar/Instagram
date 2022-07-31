using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SocialSite.Database;
using SocialSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SocialSite.Controllers
{
    public class InstagramAccountController : Controller
    {
      
        public readonly ApplicationDbContext db;
        private readonly IConfiguration configuration;
        public InstagramAccountController(ApplicationDbContext _db, IConfiguration _configuration)
        {
            db = _db;
            configuration = _configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Users users = new Users();
            return View(users);//byte default goes to index view as mentioned in the action os startup
        }
      
        [HttpPost]
        public IActionResult Register_User(string uname,string psw,string eid)
        {
            bool NameExits = db.Users.Any(x => x.Name == uname);
            bool PasswordExits = db.Users.Any(x => x.Password == psw);
            bool EmailIdExits = db.Users.Any(x => x.EmailId == eid);
            if (NameExits && PasswordExits && EmailIdExits)
            {
                ViewBag.EmailMessage = "This Email is already in use";
                return View("LoginUser");
            }

            int id_user = db.Users.OrderByDescending(a => a.Id).Select(z => z.Id).FirstOrDefault();
            Users users = new Users();
            users.EmailId = eid; users.Password = psw; users.Name = uname;
            Userdb dt = new Userdb(configuration);
            dt.saveRecord(id_user+1, uname, psw, eid);
            return RedirectToAction("SaveUserSession", users);
        }

        public IActionResult LoginUser()
        {
            return View();
        }

        public IActionResult Login(string uname, string psw)
        {
           int id =  db.Users.Where(a => a.Name == uname && a.Password == psw).Select(z=>z.Id).SingleOrDefault();
           string eid = db.Users.Where(a => a.Name == uname && a.Password == psw).Select(z => z.EmailId).SingleOrDefault();
            if(id == 0 || eid == null)
            {
                ViewBag.EmailMessage = "No Login ID found. Please Register";
                return View("Index");
            }
            Users users = new Users();
            users.EmailId = eid;users.Id = id;
            users.Password = psw; users.Name = uname;
            return RedirectToAction("SaveUserSession", users);
        }

        [HttpGet]
        public IActionResult UserDetails()
        {
            IEnumerable<Users> obj = db.Users;
            return View(obj);
        }

        public IActionResult Search()
        {
            return View();
        }

        public ActionResult ListUsers(string nsearch)
        {
            int UserId = int.Parse(HttpContext.Session.GetString("Id"));
            List<Users> users = db.Users.Where(n => n.Name.Contains(nsearch) || n.Id.ToString().Contains(nsearch) || n.EmailId.Contains(nsearch)).ToList();
            //if (users.Exists(r => r.UserId == UserId))
            //{
            //    var itemToRemove = users.Single(r => r.UserId == UserId);
            //    users.Remove(itemToRemove);
            //}
            //if (users.Count <= 0) ViewBag.error = "User not found";
            return PartialView("ListUsers", users);
        }

        public IActionResult SaveUserSession(Users u)
        {
            if (u != null)
            {
                HttpContext.Session.SetString("Id", u.Id.ToString());
                TempData["name"] = u.Name;
                TempData["email"] = u.EmailId;
                TempData["pass"] = u.Password;
            }
            return RedirectToAction("Search");
        }


    }
}









