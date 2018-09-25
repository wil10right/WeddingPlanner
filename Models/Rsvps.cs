using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingCrasher.Models
{
    public class Rsvp
    {
        [Key]
        public int RsvpId {get;set;}

        [ForeignKey("User")]
        public int UserId {get;set;}

        public User User {get;set;}

        [ForeignKey("Wedding")]
        public int WeddingId {get;set;}

        public Wedding Wedding {get;set;}


        public Rsvp()
        {
            
        }

    }
}