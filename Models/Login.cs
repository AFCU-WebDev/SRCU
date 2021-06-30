using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SRCUBagTracking.Models
{
    [Table("srcuLogin")]
    public class Login
    {
        [Key]
        public int Login_ID { get; set; }
      
        [Required(ErrorMessage="Please enter your username.")]
        [Display(Name = "Username")]
        public string username { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        //[Required(ErrorMessage = "Please enter your password.")]
        //[DataType(DataType.Password)]
        //public string ConfirmPassword { get; set; }

        public string permission { get; set; }
        public int? Branch_ID { get; set; }
        public int? school_ID { get; set; }
       
    }
}