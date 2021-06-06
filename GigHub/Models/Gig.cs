using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; set; }

        public bool IsCanceled { get; private set; }

        [Required]
        public string ArtistId { get; set; }

        public ApplicationUser Artist { get; set; }

        [Required]
        [MaxLength(255)]
        public string Venue { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public byte GenreId { get; set; }

        public Genre Genre { get; set; }
        public ICollection<Attendence> Attendences { get; private set; }

        public Gig()
        {
            Attendences = new Collection<Attendence>();
        }

        internal void Create()
        {
            var notification = Notification.GigCreated(this);

            foreach (var attendee in Attendences.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }

        internal void Modify(DateTime dateTime, string venue, byte genreId)
        {
            var notification = Notification.GigUpdated(this, DateTime, Venue);

            DateTime = dateTime;
            Venue = venue;
            GenreId = genreId;



            foreach (var attendee in Attendences.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }

        }

        internal void Cancel()
        {
            IsCanceled = true;

            var notification = Notification.GigCanceled(this);

            foreach (var attendee in Attendences.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }

        
    }
}