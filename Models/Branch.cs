using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SRCUBagTracking.Models
{
    [Table("srcuBranchInfo")]
    public class Branch
    {

        [Key]
        public int? Branch_ID { get; set; }

        [Display(Name = "Branch Name")]
        [Required(ErrorMessage = "Branch Name is a required field.")]
        public string BranchName { get; set; }

        [Display(Name = "Branch Manager")]
        public string BranchManager { get; set; }

        [Display(Name = "Branch Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone number must be 10 digits!")]
        public string BranchPhone { get; set; }

        [Display(Name = "Branch Email")]
        [DataType(DataType.EmailAddress)]
        public string BranchEmail { get; set; }
    }
}