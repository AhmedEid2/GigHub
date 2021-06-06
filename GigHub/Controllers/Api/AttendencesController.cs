using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using GigHub.Dtos;

namespace GigHub.Controllers
{
    [Authorize]
    public class AttendencesController : ApiController
    {
        private ApplicationDbContext _context;
        public AttendencesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        /*when we use a scalable data type we need to add [FromBody] before it and this has another 
          implementation in handling tha data in the UI  so we need to complex type like an object */
        public IHttpActionResult Attende(/*[FromBody]int gigId*/ AttendenceDto dto) 
        {
            var userId = User.Identity.GetUserId();

            var exists = _context.Attendences.Any(a => a.AttendeeId == userId && a.GigId == dto.GigId);
            if (exists)
            {
                return BadRequest("This Gig is already exists");
            }
            var attendence = new Attendence()
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };
            _context.Attendences.Add(attendence);
            _context.SaveChanges();


            return Ok();

        } 
    }
}
