using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace SRCUBagTracking.Models
{
    [Table("srcuSchoolsInfo")]
    public class School
    {
        [Key]
        public int? school_ID { get; set; }

        [Display(Name="School System*")]
        [Required(ErrorMessage = "School System is a required field.")]
        [DataType(DataType.Text, ErrorMessage="Please enter a valid School System!")]
        public string schoolSystem { get; set; }

        [Display(Name = "School Name*")]
        [Required(ErrorMessage = "School Name is a required field.")]
        public string schoolName { get; set; }

        [Display(Name = "School Type*")]
        [Required(ErrorMessage = "School Type is a required field.")]
        public string schoolType { get; set; }
        
        [Display(Name = "School Phone*")]
        [Required(ErrorMessage = "School Phone is a required field.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone number must be 10 digits!")]
        public string schoolPhone { get; set; }

        [Display(Name = "School Address1*")]
        [Required(ErrorMessage = "School Address1 is a required field.")]
        public string schoolAddress1 { get; set; }

        [Display(Name = "School Address2")]
        public string schoolAddress2 { get; set; }

        [Display(Name = "City*")]
        [Required(ErrorMessage = "City is a required field.")]
        public string city { get; set; }

        [Display(Name = "State*")]
        [Required(ErrorMessage = "State is a required field.")]
        public string state { get; set; }

        [Display(Name = "Zipcode*")]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zipcode")]
        [Required(ErrorMessage = "Zipcode is a required field.")]
        public string zip { get; set; }

        [Display(Name = "Contact Name*")]
        [Required(ErrorMessage = "Contact Name is a required field.")]
        public string contactName { get; set; }

        [Display(Name = "Contact Phone")]
        [Required(ErrorMessage = "Contact Phone is a required field.")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone number must be 10 digits!")]
        [DataType(DataType.PhoneNumber)]
        public string contactPhone { get; set; }

        [Display(Name = "Contact Email")]
        [DataType(DataType.EmailAddress)]
        public string contactEmail { get; set; }

        [Display(Name = "Contact Name 2")]
        public string contactName2 { get; set; }

      
        [Display(Name = "Contact Phone 2")]        
        [DataType(DataType.PhoneNumber)]
        public string contactPhone2 { get; set; }

        [Display(Name = "Contact Email 2")]
        [DataType(DataType.EmailAddress)]
        public string contactEmail2 { get; set; }

        [Display(Name = "Location In Building")]
        public string locationInBuilding { get; set; }

        [Display(Name = "Hours")]
        public string hours { get; set; }

        [Display(Name = "Courier ID*")]
        [Required(ErrorMessage = "Courier Company is a required field.")]
        public int? courier_ID { get; set; }

        //public string Courier { get; set; }

       // public IList<SelectListItem> Courier = new List<SelectListItem>{
        //        new SelectListItem{ Value="", Text="Please choose ...", Selected=true},
         //       new SelectListItem{ Value="1", Text="Morningside Courier"},
         //       new SelectListItem{ Value="2", Text="US Pack Logistics"}
       // };
        
        [Display(Name = "Active*")]
        [Required(ErrorMessage = "Active is a required field.")]
        public bool? active { get; set; }

        [Display(Name = "Branch ID*")]
        [Required(ErrorMessage = "Branch Name is a required field.")]
        public int? Branch_ID { get; set; }

        [Display(Name = "Branch Mascot")]
        public string BranchMascot { get; set; }

    }
}