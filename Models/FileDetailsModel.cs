using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SRCUBagTracking.Models
{
   [Table("srcuFileDetails")]
    public class FileDetailsModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Uploaded File")]
        public String fileName { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Select File")]
        [NotMapped]
        public HttpPostedFileBase files { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Select Thumbnail Image")]
        [NotMapped]
        public HttpPostedFileBase thumbnail { get; set; }

        [Display(Name = "File Title")]
        public String fileTitle { get; set; }

        [Display(Name = "File Category")]
        public String fileCategory { get; set; }

        [Display(Name = "PDF  Content")]
        public byte[] fileContent { get; set; }        

        [DataType(DataType.DateTime)]
        [Display(Name="Upload Date")]
        public DateTime dateUploaded { get; set; }

        public int Order_appearance { get; set; }

        public bool Active { get; set; }



    }
    }
