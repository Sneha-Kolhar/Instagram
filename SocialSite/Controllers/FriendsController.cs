using Microsoft.AspNetCore.Mvc;
using SocialSite.Database;
using SocialSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SocialSite.Controllers
{
    public class FriendsController : Controller
    {
        public readonly ApplicationDbContext db;
        
        public FriendsController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public ActionResult sendFriendRequest(string User_Id)
        {
            int c_userid = int.Parse(User_Id);
            int p_userid = int.Parse(HttpContext.Session.GetString("Id"));
            Friends login_p = db.Friends.Where(a => a.UserId == p_userid).SingleOrDefault();
            Friends login_c = db.Friends.Where(a => a.UserId == c_userid).SingleOrDefault();
            if (login_p == null && login_c == null)
            {
                Friends frd = new Friends() ;
                frd.UserId = p_userid; frd.Requests = c_userid.ToString(); frd.Friend = ""; frd.Id = 1;
                db.Friends.Add(frd);
                db.SaveChanges();
            }
            else if (login_p != null && login_p.Requests.Contains(c_userid.ToString()))
            {
                ViewBag.error = "You already sent a follow request";
            }
            else if (login_c != null && login_c.Requests.Contains(p_userid.ToString()))
            {
                string old_val = (login_c.Requests.Contains(p_userid.ToString())  == true) ? p_userid.ToString() : "+" + p_userid;
               login_c.Requests =  login_c.Requests.Replace(old_val, "");
                string val = "+" + p_userid.ToString();
                login_c.Friend += val;

                if(login_p == null)
                {
                    Friends frd = new Friends();
                    frd.UserId = p_userid; frd.Requests = ""; frd.Friend = ""; frd.Id = 1;
                    db.Friends.Add(frd);
                }
                else
                {
                    login_p.Friend += "+" + c_userid.ToString();
                }
                db.SaveChanges();
                //go to view of gallery to frd and access of frd gallery
            }
            else if (login_c == null)
            {
                string val = "+" + c_userid.ToString();
                login_p.Requests += val;
                db.SaveChanges();
            }
            else
            {
                string val = "+" + p_userid.ToString();
                login_c.Requests += val;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "InstagramAccount");
        }

        public ActionResult deleteFriendRequest(string User_Id)
        {
            int c_userid = int.Parse(User_Id);
            int p_userid = int.Parse(HttpContext.Session.GetString("Id"));
            Friends login_p = db.Friends.Where(a => a.UserId == p_userid).SingleOrDefault();
            Friends login_c = db.Friends.Where(a => a.UserId == c_userid).SingleOrDefault();
            string old_val = (login_p.Requests.Contains(c_userid.ToString()) == true) ? c_userid.ToString() : "+" + c_userid;
            login_p.Friend = login_p.Friend.Replace(old_val, "");
            if (login_c != null && (login_c.Friend.Contains(p_userid.ToString()) || login_c.Friend.Contains("+"+ p_userid)))
            {
                old_val = (login_c.Friend.Contains(p_userid.ToString()) == true) ?  ((login_c.Friend.Contains("+" + p_userid) == true)? "+" + p_userid : p_userid.ToString()) : "";
                login_c.Friend = login_c.Friend.Replace(old_val, "");
            }
            db.SaveChanges();
            return RedirectToAction("Index", "InstagramAccount");
        }

        public ActionResult viewFriendRequest()
        {
            List<Users> users = new List<Users>();
            int p_userid = int.Parse(HttpContext.Session.GetString("Id"));
            Friends login_p = db.Friends.Where(a => a.UserId == p_userid).SingleOrDefault();
            string frds =  db.Friends.Where(a => a.UserId == p_userid).Select(x => x.Friend).SingleOrDefault();
            if(frds == null)
            {
                ViewBag.error = "You are alone";
                return View();
            }
           
            string[] Friends_Ids = frds.Split('+', StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in Friends_Ids)
            {
                int Friend_Id = Convert.ToInt32(id);
                Users u = db.Users.Where(n => n.Id == Friend_Id).FirstOrDefault();
                users.Add(u);
            }
            return View("viewFriends", users);
        }
     
    }
}

