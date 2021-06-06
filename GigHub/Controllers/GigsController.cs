using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private ApplicationDbContext _context;
        public GigsController()
        {
            _context = new ApplicationDbContext();
        } 

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Gigs.
                Where(g => g.ArtistId == userId && g.DateTime > DateTime.Now && !g.IsCanceled)
                .Include(g => g.Genre)
                .ToList();
            return View(gigs);
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendences
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            var viewModel = new GigsViewModel()
            {
                UpComingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Going"
            };
            return View("Gigs",viewModel);
        }


        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();

            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId); 
            var gigModel = new GigFormViewModel()
            {
                Heading = "Edit a Gig",
                Id = id,
                Genres = _context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                GenreId = gig.GenreId,
                Venue = gig.Venue
            };
            return View("GigForm",gigModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            var gigModel = new GigFormViewModel()
            {
                Heading = "Add a Gig",
                Genres = _context.Genres.ToList()
            };
            return View("GigForm",gigModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel gigViewModel)
        {
            if (!ModelState.IsValid)
            {
                gigViewModel.Genres = _context.Genres.ToList();
                return View("GigForm",gigViewModel);
            }
            var gig = new Gig()
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = gigViewModel.GetDateTime(),
                GenreId = gigViewModel.GenreId,
                Venue = gigViewModel.Venue
            };
            _context.Gigs.Add(gig);
            _context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }


        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home",new { query = viewModel.SearchTerm});
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel gigViewModel)
        {
            var userId = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                gigViewModel.Genres = _context.Genres.ToList();
                return View("GigForm",gigViewModel);
            }

            var gig = _context.Gigs
                .Include(g => g.Attendences.Select(a => a.Attendee))
                .Single(a => a.Id == gigViewModel.Id && a.ArtistId == userId);


            gig.Venue = gigViewModel.Venue;
            gig.DateTime = gigViewModel.GetDateTime();
            gig.GenreId = gigViewModel.GenreId;

            gig.Modify(gigViewModel.GetDateTime(), gigViewModel.Venue, gigViewModel.GenreId);

            _context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }
    }
}