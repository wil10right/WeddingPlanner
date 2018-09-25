using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingCrasher.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt {get;set;}

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt {get;set;}

        public List<Wedding> PlannedWeddings {get;set;}
        public List<Rsvp> AttendingWeddings {get;set;}

        public User()
        {
            PlannedWeddings = new List<Wedding>();
            AttendingWeddings = new List<Rsvp>();
        }
    }
}
