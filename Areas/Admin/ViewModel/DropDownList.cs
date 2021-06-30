using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;


// WE DO NOT USE THIS VIEWMODEL AT THIS TIME(06/16/16)
namespace SRCUBagTracking.Areas.Admin.ViewModel
{   
    public class DropDownList
    {
        
        // Branches name for dropdown
        public IList<SelectListItem> Branches
        {
            get
            {
                var listOfBranches = new List<SelectListItem>{
                    new SelectListItem { Value = "1", Text = "Ashburn" },
                    new SelectListItem { Value = "2", Text = "Burke/Fairfax Station" },                    
                    new SelectListItem { Value = "3", Text = "Fairfax" },                    
                    new SelectListItem { Value = "4", Text = "Gainesville" },                    
                    new SelectListItem { Value = "5", Text = "Manassas" },                    
                    new SelectListItem { Value = "6", Text = "Mount Vernon" },                    
                    new SelectListItem { Value = "7", Text = "Springfield" },                    
                    new SelectListItem { Value = "8", Text = "Stafford" },                    
                    new SelectListItem { Value = "9", Text = "Sterling" },                    
                    new SelectListItem { Value = "10", Text = "Sudley Manor" },     
                    new SelectListItem { Value = "11", Text = "Winchester Medical Center" },     
                    new SelectListItem { Value = "12", Text = "Woodbridge" }    
                };
                return listOfBranches;
            }
        }


        // Couries name for dropdown
        public IList<SelectListItem> Couriers
        {
            get
            {
                var listOfCouries = new List<SelectListItem>{
                    new SelectListItem { Value = "0", Text = "No Courier" },
                    new SelectListItem { Value = "1", Text = "Morningside Courier" },
                    new SelectListItem { Value = "2", Text = "US Pack Logistics" }                    
                };
                return listOfCouries;
            }
        }            
    }
}