using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SRCUBagTracking.Models
{
    public class ContactUs
    {
        public int ID { get; set; }

        public string schoolName { get; set; }

        public string subject { get; set; }

        public string typeOfRequest { get; set; }

        public string marketingSupply { get; set; }

        public string operationalSupply  { get; set; }

        public string maintenanceRepairs  { get; set; }

        public string othersMisc { get; set; }

        public string message { get; set; }

        [Display(Name = "Date Submitted")]
        [DataType(DataType.DateTime, ErrorMessage = "Please enter a valid date!")]
        public DateTime? dateSubmitted { get; set; }
    }
}