using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingCrasher.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId {get;set;}

        [ForeignKey("User")]
        public int UserId {get;set;}

        public User User {get;set;}

        [Required]
        [MinLength(3)]
        public string WedderOne {get;set;}

        [Required]
        [MinLength(3)]
        public string WedderTwo {get;set;}

        [Required]
        public DateTime WeddingDate {get;set;}

        [Required]
        public string WeddingAddress {get;set;}
        public List<Rsvp> Guests {get;set;}

        public Wedding()
        {
            Guests = new List<Rsvp>();
        }

    }

}