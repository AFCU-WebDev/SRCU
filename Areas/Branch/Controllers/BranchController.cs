using Afcu.DataAccess.AppleNet;
using Afcu.Model.AppleNet;
using SRCUBagTracking.Controllers;
using SRCUBagTracking.Models;
using SRCUBagTracking.Repository.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace SRCUBagTracking.Areas.Branch.Controllers
{
    //[Authorize(Roles = "2")]
    //[CheckSessionOutAttribute]
    public class BranchController : Controller
    {
        private SRCUDbContext srcuDb = new SRCUDbContext();
        private ISRCURepository _srcuRepositoryInterface;

        public BranchController()
        {
            this._srcuRepositoryInterface = new SRCURepository(new SRCUDbContext());
        }

            // GET: Branch/Branch
            public ActionResult Index()
        {
            var BranchId = Convert.ToInt32(Session["BranchId"]);

            var SchoolName = (from q in srcuDb.Schools.Where(q => q.schoolName != null && q.active == true && q.Branch_ID == BranchId)
                                  select q).OrderBy(q => q.schoolName).ToList();
                ViewBag.schoolName = SchoolName;

                return View(SchoolName);
            }

            //Branch SchoolBag
            public ActionResult BranchSchoolBag(int schoolId)
            {
                //adde the date to remove all the records before August 1st for the new school year
                var currentSchoolYear = DateTime.Parse("2021-08-01");
                var bag = from Bag in _srcuRepositoryInterface.GetBags(schoolId).Where(q => q.dateSubmitted >= currentSchoolYear)
                          select Bag;
                    ViewBag.SchoolId = schoolId;

                    // Pass schoolName for page title
                    var SchoolName = (from q in srcuDb.Schools.Where(q => q.schoolName != null && q.active == true && q.school_ID == schoolId)
                                      select q.schoolName).FirstOrDefault();
                    ViewBag.schoolName = SchoolName;
                    return View(bag);
            }

            //GET:
            public ActionResult BranchWork(int recId)
            {
                Bag bag = _srcuRepositoryInterface.GetBag(recId);

                ViewBag.recId = recId;
                ViewBag.BagNumber = bag.bagNumber;
                ViewBag.Apps = bag.memberApps;
                ViewBag.Deposits = bag.deposits;
                ViewBag.handDeliver = bag.handDeliver;
                ViewBag.SubmittedDate = bag.dateSubmitted;
                ViewBag.liaisonComment = bag.liasonComment;
                ViewBag.branchComment = bag.branchComment;
                ViewBag.dateCompletedDeposits = bag.dateCompletedDeposits;
                ViewBag.dateCompletedApps = bag.dateCompletedApps;

                return View(bag);
            }

            //POST
            [HttpPost]
            public ActionResult BranchWork(Bag bag)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Get the Teller ID from Active Directory
                        AppleNetDbContext db = new AppleNetDbContext();

                        var currentUser = System.Web.HttpContext.Current.User.Identity;
                        var login = currentUser.Name.Replace("AFCU\\", "").ToLower();
                        Teller teller = new Teller();

                        //teller.Branch.Name

                        if (!string.IsNullOrEmpty(login))
                        {
                            // begins AppleNet windows Authentication
                            var windowsUser = WindowsIdentity.GetCurrent();
                            // Ends AppleNet windows Authentication
                            teller = db.Tellers.Include("Branch").Include("Position").FirstOrDefault(t => t.Logon.Equals(login) && t.StatusId > 0 && t.StatusId < 4);
                        }

                        bag.completedByTeller = teller.Number;
                       
                        _srcuRepositoryInterface.UpdateBag(bag);
                        _srcuRepositoryInterface.Save();

                        /// Send an email Notification if there is a comment from the Branch
                        //if (bag.branchComment != null)
                        //{
                        //    var emailTo =  "fnamdarian@applefcu.org";
                        //    var emailSubject = "There is a New Comment.";
                        //    var emailBody = "<html> <head> New Comment</head><body> Please check your comment box.  </body> </html>";
                        //    var emailCC = "";

                        //    var email = new SendEmail();
                        //    email.DeliverEmail(emailTo, emailCC, emailSubject, emailBody);  
                        //}

                        return RedirectToAction("BranchSchoolBag", "Branch", new { area = "Branch", recId = bag.rec_ID, schoolId = bag.school_ID });
                    }
                    catch (DataException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, " + "and if the problem persists see your system administrator.");
                    }
                }
                return RedirectToAction("BranchSchoolBag", new { schoolId = bag.school_ID });
            }

            //GET: 
            public ActionResult BranchComments(int recId, int schoolId)
            {
                Bag bag = _srcuRepositoryInterface.GetBag(recId);
                ViewBag.liaisonComment = bag.liasonComment;
                ViewBag.branchComment = bag.branchComment;
                ViewBag.submittedDate = bag.dateSubmitted;

            return View(bag);
            }
        }
    }
