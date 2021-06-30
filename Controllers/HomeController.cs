using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SRCUBagTracking.Models;
using SRCUBagTracking.Repository.DAL;
using System.Collections.Generic;
using System.Web;


namespace SRCUBagTracking.Controllers
{
    //[Authorize]
    [CheckSessionOutAttribute]
    public class HomeController : Controller
    {
        private SRCUDbContext srcuDb = new SRCUDbContext();
        private ISRCURepository _srcuRepositoryInterface;

        public HomeController()
        {
            this._srcuRepositoryInterface = new SRCURepository(new SRCUDbContext());
        }

        // GET : Liaison submitted bag
        public ActionResult BagLibrary()
        {
            var schoolId = Convert.ToInt32(Session["schoolID"]);
            //added the date to remove all the records before August 1st for the new school year for Liaison
            var Date2019 = DateTime.Parse("2019-08-01");
            var LiaisonBag = from Bag in _srcuRepositoryInterface.GetBags(schoolId).Where(q => q.dateSubmitted >= Date2019)
                             select Bag;
            return View(LiaisonBag);
        }

        //GET : BagWizard form

        public ActionResult BagWizard()
        {
            Bag model = new Bag();
            model.dateSubmitted = DateTime.Now;
            return View(model);
        }

        //POST: bagWizard form data to DB and send email based on workflow
        [HttpPost]
        public ActionResult BagWizard(Bag model, bool handDeliverCheckBox = false)
        {
            //SRCU TEAM        
           // const string emailCC = "fnamdarian@applefcu.org";
            const string emailCC = "SRCU@applefcu.org";
            string emailSubject = "";
            string emailBody = "";
            //string emailTo = "f.namdarian@yahoo.com";
            // on 9/4/2019 applecourier@mcsicourier.com replaced with pia@mcsicourier.com on S.Lopez request
            //string courier1 = "Pvales4800@gmail.com";
            // string courier1 = "namdarian.f@gmail.com";
           string courier1 = "pia@mcsicourier.com";
            string courier2 = "infoVA@gouspack.com";
            model.handDeliver = handDeliverCheckBox;

            try
            {
                if (ModelState.IsValid)
                {
                    var schoolId = Convert.ToInt32(Session["schoolID"]);
                    model.school_ID = schoolId;

                    // GET  School Name
                    var schoolName = (from q in srcuDb.Schools.Where(q => q.school_ID == schoolId)
                                      select q.schoolName).FirstOrDefault();

                    // GET  Courier ID
                    var courier = (from q in srcuDb.Schools.Where(x => x.school_ID == schoolId && x.courier_ID != null)
                                   select q.courier_ID).FirstOrDefault();

                    // Branch Email    
                   // var emailTo = "fnamdarian@applefcu.org";
                    var emailTo = (from branch in srcuDb.Branches
                                   join schools in srcuDb.Schools on branch.Branch_ID equals schools.Branch_ID
                                   where schools.school_ID == schoolId
                                   select branch.BranchEmail).FirstOrDefault();

                    //Scenario 1 Workflow: No work today
                    if ((model.deposits == null || model.deposits == 0) && (model.memberApps == null || model.memberApps == 0))
                    {
                        // if school register for Courier
                        // morningside
                        if (courier == 1)
                        {
                            emailTo = courier1 + ";" + emailTo;
                            //emailTo = emailTo + "pia@mcsicourier.com";
                        }
                        //US Pack
                        else if (courier == 2)
                        {
                            emailTo = courier2 + ";" + emailTo;
                            //emailTo = emailTo + "infoVA@gouspack.com";
                        }
                        else
                        {
                            emailTo = emailTo + "";
                        }
                        emailSubject = "NO ACTION - SRCU Bag Cancelled";
                        emailBody = "<html> <head> Apple’s " + schoolName + " Student Run Credit Union branch does NOT require a deposit bag pickup today.   </body> </html>";

                    }


                    //Scenario 2 Workflow: deposits and applications OR deposits only - But Hand delivered
                    else if (((model.deposits != 0 && model.memberApps != 0) || (model.deposits != 0 && model.memberApps == 0)) && (handDeliverCheckBox == true))
                    {
                        // if school register for Courier
                        // morningside
                        if (courier == 1)
                        {
                            //emailTo = emailTo + "pia@mcsicourier.com";
                            emailTo = emailTo + ";" + courier1;
                        }
                        //US Pack
                        else if (courier == 2)
                        {
                            //emailTo = emailTo + "infoVA@gouspack.com";
                            emailTo = emailTo + ";" + courier2;
                        }
                        else
                        {
                            emailTo = emailTo + "";
                        }

                        emailSubject = "NO BAG PICKUP - SRCU Bag will be hand delivered";
                        emailBody = "<html> <head> Apple’s " + schoolName + " Student Run Credit Union branch does NOT require a deposit bag pickup today. It will be hand delivered by liaison.  </body> </html>";
                    }

                    //Scenario 3 Workflow: applications only - fax
                    else if ((model.deposits == null || model.deposits == 0) && (model.dollarAmount == null || model.dollarAmount == 0) && (model.memberApps != null || model.memberApps != 0))
                    {
                        if (courier == 1)
                        {
                            //emailTo = emailTo + "pia@mcsicourier.com";
                            emailTo = emailTo + ";" + courier1;
                        }
                        //US Pack
                        else if (courier == 2)
                        {
                            //emailTo = emailTo + "infoVA@gouspack.com";
                            emailTo = emailTo + ";" + courier2;
                        }
                        else
                        {
                            emailTo = emailTo + "";
                        }

                        emailSubject = "NO BAG PICKUP - SRCU Applications Submitted";
                        emailBody = "<html> <head> Apple’s " + schoolName + " Student Run Credit Union branch does NOT require a deposit bag pickup today. Applications will be submitted electronically. </body> </html>";
                    }
                    ////Scenario 4 Workflow: deposits and applications OR deposits only                 
                    else if ((model.deposits != 0) && (handDeliverCheckBox == false))
                    {
                        emailTo = emailTo + "";

                        emailSubject = "BAG PICKUP - SRCU Bag Submitted";
                        emailBody = "<html> <head> Apple’s " + schoolName + " Student Run Credit Union branch has requested a deposit bag pickup today.   </body> </html>";
                    }


                    var email = new SendEmail();
                    email.DeliverEmail(emailTo, emailCC, emailSubject, emailBody);


                    _srcuRepositoryInterface.InsertBag(model);
                    _srcuRepositoryInterface.Save();
                    return RedirectToAction("BagLibrary");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists see your system administrator.");
            }

            return View(model);

        }

        //GET
        public ActionResult ContactUs()
        {
            ContactUs model = new ContactUs();
            model.dateSubmitted = DateTime.Now;
            return View(model);
        }


        //POST Contac Us
        [HttpPost]
        public ActionResult ContactUs(ContactUs model)
        {
            //SRCU TEAM                    
            const string emailTo = "SRCU@applefcu.org";
            //const string emailTo = "fnamdarian@applefcu.org";
            const string emailCC = "";
            string emailSubject = "";
            string emailBody = "";

            try
            {
                if (ModelState.IsValid)
                {
                    var schoolId = Convert.ToInt32(Session["schoolID"]);
                    //model.school_ID = schoolId;

                    // GET  School Name
                    var schoolName = (from q in srcuDb.Schools.Where(q => q.school_ID == schoolId)
                                      select q.schoolName).FirstOrDefault();

                    emailSubject = "New SRCU Request";
                    emailBody = "<html><head><style>body { font-family:Arial, Helvetica, sans-serif;} #SMtable{ width:75%; } #SMtitle{ font-weight: 600; } #SMtable, .SMth, .SMtd{ border: 1px solid black; border-collapse: collapse; }.SMth, .SMtd { padding: 5px; }.SMth{ background-color:#B12029; color: white; } .SMtrCol{ background-color: #b6d0dc; } }</style> </head>";
                    emailBody += "<body><p>An Apple Student Run Credit Union request has been received for <strong>" + schoolName + "</strong> at " + model.dateSubmitted;
                    emailBody += "<p id='SMtitle'>The SRCU form details:</p>";
                    emailBody += "<table id='SMtable'><tr class='SMtr'><th class='SMth'>Data Fields</th><th class='SMth'>Data</th></tr>";
                    emailBody += "<tr class='SMtr'><td class='SMtd'>School Name</td><td class='SMtd'>" + schoolName + "</td></tr>";
                    emailBody += "<tr class='SMtrCol'><td class='SMtd'>Subject</td><td class='SMtd'>" + model.subject + "</td></tr>";
                    emailBody += "<tr class='SMtr'><td class='SMtd'>Marketing Supply Request</td><td class='SMtd'>" + model.marketingSupply + "</td></tr>";
                    emailBody += "<tr class='SMtrCol'><td class='SMtd'>Operational Supply Request</td><td  class='SMtd'>" + model.operationalSupply + "</td></tr>";
                    emailBody += "<tr class='SMtr'><td class='SMtd'>Maintenance and Repairs</td><td class='SMtd'>" + model.maintenanceRepairs + "</td></tr>";
                    emailBody += "<tr class='SMtrCol'><td class='SMtd'>Other/Misc</td><td class='SMtd'>" + model.othersMisc + "</td></tr>";
                    emailBody += "<tr class='SMtr'><td class='SMtd'>Message</td><td class='SMtd'>" + model.message + "</td></tr></table></body></html>";                                       

                    var email = new SendEmail();
                    email.DeliverEmail(emailTo, emailCC, emailSubject, emailBody);

                    return RedirectToAction("BagLibrary");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists see your system administrator.");
            }

            return View(model);
        }



        //GET
        public ActionResult Error()
        {
            return View();
        }

        //GET: 
        public ActionResult LiaisonComments(int recId, int schoolId)
        {
            Bag bag = _srcuRepositoryInterface.GetBag(recId);
            ViewBag.liaisonComment = bag.liasonComment;
            ViewBag.branchComment = bag.branchComment;
            ViewBag.submittedDate = bag.dateSubmitted;

            return View(bag);
        }

        //GET
        public ActionResult Marketing()
        {
            //Letter Category from srcuFileDetails db
            var LetterCategoryFiles = (from q in srcuDb.FileDetailsModels.Where(q => q.fileCategory == "Letters")
                                       select q).ToList();

            //var  LetterCategoryFiles_NoExtention[LetterCategoryFiles] = Path.GetFileNameWithoutExtension(LetterCategoryFiles_NoExtention[LetterCategoryFiles]);

            ViewBag.LetterCategoryFiles = LetterCategoryFiles;


            //Flyers Category from srcuFileDetails db - The code check if there is any Flyers in sql table and list it in table
            var FlyersCategoryFiles = (from q in srcuDb.FileDetailsModels.Where(q => q.fileCategory == "Flyers")
                                       select q).OrderBy(q => q.Order_appearance).ToList();
            ViewBag.FlyersCategoryFiles = FlyersCategoryFiles;
            return View();
        }

        public ActionResult srcuDocuments()
        {
            return View();
        }

        public ActionResult Training()
        {
            return View();
        }
    }
}