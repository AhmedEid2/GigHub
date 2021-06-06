using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.ViewModels;


namespace GigHub.Controllers
{
    public class FollowingsController : Controller
    {
        private ApplicationDbContext _context;
        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Followings
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var followees = _context.Followings
                .Where(f => f.FollowerId == userId)
                .Select(f => f.Followee)
                .ToList();
            var viewModel = new FollowingViewModel()
            {
                FollowerId = userId,
                Followees = followees
            };
            return View(viewModel);
        }
    }
}