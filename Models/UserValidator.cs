using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingCrasher.Models
{
    public class UserValidator
    {
        [Required]
        [MinLength(2)]
        public string FirstName {get;set;}

        [Required]
        [MinLength(2)]
        public string LastName {get;set;}

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm {get;set;}
        public UserValidator()
        {
            
        }
    }
}