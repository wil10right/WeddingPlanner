using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingCrasher.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WeddingCrasher.Controllers
{
    public class HomeController : Controller
    {
        private WeddingContext _context;
        public HomeController(WeddingContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            ViewBag.LoginAgain = TempData["SessionExp"];
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(UserValidator data)
        {
            if(ModelState.IsValid)
            {
                User DbUser = _context.users.SingleOrDefault(u=>u.Email == data.Email);
                if(DbUser != null)
                {
                    ViewBag.EmailTaken = "The Email address you have provided is already registered!";
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User RealUser = new User();
                RealUser.Password = data.Password;
                RealUser.Password = Hasher.HashPassword(RealUser, RealUser.Password);
                RealUser.FirstName = data.FirstName;
                RealUser.LastName = data.LastName;
                RealUser.Email = data.Email;
                _context.Add(RealUser);
                _context.SaveChanges();
                User NewUser = _context.users.SingleOrDefault(u=>u.Email == RealUser.Email);
                HttpContext.Session.SetInt32("CurrentUser",NewUser.UserId);
                HttpContext.Session.SetInt32("LoggedIn",1);
                return RedirectToAction("Dashboard");
            }else{
                return View("Index");
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string LogEmail, string LogPass)
        {
            User DbUser = _context.users.SingleOrDefault(u=>u.Email == LogEmail);
            if(DbUser != null && LogPass != null)
            {
                HttpContext.Session.SetInt32("CurrentUser",DbUser.UserId);
                HttpContext.Session.SetInt32("LoggedIn",1);
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(DbUser, DbUser.Password, LogPass))
                {
                    ViewBag.User = DbUser;
                    return RedirectToAction("Dashboard");
                }
            }
            ViewBag.LoginError = "Invalid Login!";
            return View("Index");
        }

        [HttpGet]
        [Route("dashy")]
        public IActionResult Dashboard()
        {
            int CurrentUserId = HttpContext.Session.GetInt32("CurrentUser").GetValueOrDefault();
            ViewBag.CurrentUser = CurrentUserId;
            // List<Wedding> AllWeddings = _context.weddings.Where(w=>w.WeddingId>0).ToList();
            List<Wedding> AllWeddings = _context.weddings.Include(g=>g.Guests).ThenInclude(u=>u.User).ToList();
            ViewBag.Weddings = AllWeddings;
            //query for all the weddings
            return View("Dashboard");
        }

        [HttpGet]
        [Route("new")]
        public IActionResult NewWedding()
        {
            int CurrentUserId = HttpContext.Session.GetInt32("CurrentUser").GetValueOrDefault();
            ViewBag.CurrentUser = CurrentUserId;
            ViewBag.TimeTravel = TempData["timetravel"];
            return View("Plan");
        }

        [HttpPost]
        [Route("MakeWedding")]
        public IActionResult MakeWedding(Wedding data)
        {
            int? CheckSesh = HttpContext.Session.GetInt32("CurrentUser").GetValueOrDefault();
            if(data.UserId == 0)
            {
                TempData["SessionExp"] = "Session expired. Please log back in again.";
                return RedirectToAction("Index");
            }
            if(ModelState.IsValid)
            {
                if(data.WeddingDate < DateTime.Today)
                {
                    // ViewBag.TimeTravel = "Date of Wedding must be AFTER today.";
                    TempData["timetravel"] = "Date of Wedding must be AFTER today.";
                    return RedirectToAction("NewWedding");
                }
                _context.weddings.Add(data);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }else{
                return View("Plan");
            }
        }

        [HttpGet]
        [Route("wedding/{wid}")]
        public IActionResult WeddingInfo(int wid)
        {
            Wedding WedInfo = _context.weddings.Include(g=>g.Guests).ThenInclude(u=>u.User).SingleOrDefault(w=>w.WeddingId == wid);
            ViewBag.ThisWedding = WedInfo;
            ViewBag.Date = WedInfo.WeddingDate;
            ViewBag.GuestList = WedInfo.Guests;
            //info for a single wedding queried by the id passed through the url
            //query for the guest list
            return View("Wedding");
        }

        [HttpGet]
        [Route("delete/{wid}")]
        public IActionResult DeleteWedding(int wid)
        {
            // var AllWeddings = _context.weddings.Where(x=>x.WeddingId == wid).Include(g=>g.Guests).ToList();
            Wedding DeleteThis = _context.weddings.SingleOrDefault(w=>w.WeddingId == wid);
            Wedding parent = _context.weddings.Include(g=>g.Guests).SingleOrDefault(x=>x.WeddingId == wid);
            List<Rsvp> DeleteThese = parent.Guests;
            // foreach(var rsvp in parent.Guests.ToList())
            foreach(var rsvp in DeleteThese)
            {
                _context.rsvps.Remove(rsvp);
            }
            _context.weddings.Remove(DeleteThis);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("rsvp/{uid}/{wid}")]
        public IActionResult Rsvp(int uid, int wid)
        {
            Rsvp NewRsvp = new Rsvp();
            NewRsvp.UserId = uid;
            NewRsvp.WeddingId = wid;
            _context.Add(NewRsvp);
            _context.SaveChanges();
            return RedirectToAction("WeddingInfo", new{wid = wid});
        }

        [HttpGet]
        [Route("unrsvp/{uid}/{wid}")]
        public IActionResult UnRsvp(int uid, int wid)
        {
            Rsvp DeleteThis = _context.rsvps.SingleOrDefault(r=>r.WeddingId==wid && r.UserId ==uid);
            _context.rsvps.Remove(DeleteThis);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

    }
}
