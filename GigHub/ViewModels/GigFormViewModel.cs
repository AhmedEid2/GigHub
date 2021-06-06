using GigHub.Controllers;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace GigHub.ViewModels
{
    public class GigFormViewModel
    {

        public int Id { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public byte GenreId { get; set; }

        public string Heading { get; set; }
        public string Action
        {
            get
            {
                //this for extracting the method name dynamicly instead of write a magic string like "Update" or "Create"

                //So the first step is generate an expression from controller and actions in it 
                Expression<Func<GigsController, ActionResult>> update = (c => c.Update(this));
                Expression<Func<GigsController, ActionResult>> create = (c => c.Create(this));

                var action = (Id != 0) ? update : create;

                //Then convert this expression to method call expression and extract the name of the method from it
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }
        public IEnumerable<Genre> Genres { get; set; }
        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("{0},{1}", Date, Time));   
        }

    }
}