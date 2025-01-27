﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private ApplicationDbContext _context;
        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetUserNotifications()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _context.NotificationUsers
                .Where(nu => nu.UserId == userId && !nu.IsRead)
                .Select(n => n.Notification)
                .Include(a => a.Gig.Artist)
                .ToList();


            return notifications.Select(Mapper.Map<Notification,NotificationDto>);
        }


        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _context.NotificationUsers.Where(nu => nu.UserId == userId && !nu.IsRead).ToList();

            notifications.ForEach(n => n.Read());
            _context.SaveChanges();

            return Ok();
            
        }
    }
}
