using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRCUBagTracking.Models
{
    [Table("srcuBagReport")]
    public class Bag
        {
            [Key]
            public int rec_ID { get; set; }

            public int? school_ID { get; set; }
            
            [Display(Name="Bag Number")]
            public String bagNumber { get; set; }

            //[Required]
            [Display(Name = "Deposits")]
            public int? deposits { get; set; }

            [Display(Name = "Member Apps")]
            public int? memberApps { get; set; }

            [Display(Name = "Dollar Amount")]
            [DataType(DataType.Currency, ErrorMessage = "Please enter a valid number!")]
            //[RegularExpression(@"^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$", ErrorMessage = "Please enter a valid number!")]
            public decimal? dollarAmount { get; set; }

            [Display(Name = "Completed By Teller")]
            public int? completedByTeller { get; set; }

            [Display(Name = "Date Submitted")]
            [DataType(DataType.DateTime, ErrorMessage="Please enter a valid date!")]        
            public DateTime? dateSubmitted { get ; set; }

            [Display(Name = "Date Completed Deposits")]
            [DataType(DataType.DateTime)]
            public DateTime? dateCompletedDeposits { get; set; }

            [Display(Name = "Date Completed Apps")]
            [DataType(DataType.DateTime)]
            public DateTime? dateCompletedApps { get; set; }

            [Display(Name = "Liaison Comment")]
            public String liasonComment { get; set; }

            [Display(Name = "Liaison Comment")]
            public String branchComment { get; set; }

            [Display(Name = "Hand Delivered?")]
            public bool? handDeliver { get; set; }

            [Display(Name = "Application Faxed?")]
            public bool? AppFaxed { get; set; }
            
        }
    }
