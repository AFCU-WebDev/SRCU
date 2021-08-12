using System.Linq;
using System.Web.Mvc;
using SRCUBagTracking.Models;
using SRCUBagTracking.Repository.DAL;
using Afcu.Model.AppleNet;
using Afcu.DataAccess.AppleNet;
using System.Security.Principal;
using System;

namespace SRCUBagTracking.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private SRCUDbContext srcuDb = new SRCUDbContext();
        private ISRCURepository _srcuRepositoryInterface;

        public AccountController()
        {
            this._srcuRepositoryInterface = new SRCURepository(new SRCUDbContext());
        }


        // GET: /Account/Login
        //[AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            
            // ********** Form Anthentication **********//********* This section needed for both form auth and windows auth ***** 09/01/2016 *****//
            return View();

        }

        // ********** Form Anthentication **********//********** 09/01/2016 *****//
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                //FormsAuthentication.SetAuthCookie("Username", false);
                try
                {
                    //if (authProvider.Authenticate(model.username, model.password))
                    //{
                    if (srcuDb.Logins.Any(q => q.username == model.username && q.password == model.password))
                    {
                        // ---- Make First Letter Uppercase
                        var usernameUpperFirstLetter = model.username.Substring(0, 1).ToUpper() + model.username.Substring(1);
                        // --- Get username  ----------------------------------------------------------------------------
                        Session["user"] = new Login { username = usernameUpperFirstLetter };

                        // --- Get schoolID from Db  ---------------------------------------------------------------------
                        var schoolId = (from q in srcuDb.Logins.Where(q => q.username == model.username && q.password == model.password)
                                        select q.school_ID).FirstOrDefault();
                        Session["schoolID"] = schoolId;

                        // --- Get Permission  ---------------------------------------------------------------------------                                            
                        var permission = (from q in srcuDb.Logins.Where(q => q.username == model.username && q.password == model.password)
                                          select q.permission).FirstOrDefault();

                        // Get Branch_ID
                        var BranchId = (from q in srcuDb.Logins.Where(q => q.username == model.username && q.password == model.password)
                                        select q.Branch_ID).FirstOrDefault();
                        Session["BranchId"] = BranchId;
                        //Liaison login
                        if (permission == "3" && BranchId != null)
                        {
                            // --- Branch Info  ----------------------------------------------------------------------
                            var branchSchool = from branch in srcuDb.Branches
                                               join schools in srcuDb.Schools on branch.Branch_ID equals schools.Branch_ID
                                               where schools.school_ID == schoolId
                                               select new { schoolName = schools.schoolName, branchName = branch.BranchName, branchManager = branch.BranchManager, branchPhone = branch.BranchPhone, branchEmail = branch.BranchEmail, schoolLocation = schools.locationInBuilding, schoolHours = schools.hours, BranchMascot = schools.BranchMascot };

                            Session["schoolName"] = branchSchool.Select(q => q.schoolName).First();
                            Session["schoolLocation"] = branchSchool.Select(q => q.schoolLocation).First();
                            Session["schoolHours"] = branchSchool.Select(q => q.schoolHours).First();
                            Session["branchMascot"] = branchSchool.Select(q => q.BranchMascot).First();
                            Session["branchName"] = branchSchool.Select(q => q.branchName).First();
                            Session["branchManager"] = branchSchool.Select(q => q.branchManager).First();
                            Session["branchPhone"] = branchSchool.Select(q => q.branchPhone).First();
                            Session["branchEmail"] = branchSchool.Select(q => q.branchEmail).First();
                            //Roles.AddUserToRole("Rippon", "school");
                            return RedirectToAction("BagLibrary", "Home", new { area = "" });
                        }

                        ////// Branch login
                        //else if (permission == "2" && BranchId != null)
                        //{
                        //    var branchName = from branch in srcuDb.Branches
                        //                     where branch.Branch_ID == BranchId
                        //                     select new { branchName = branch.BranchName };

                        //    Session["branchName"] = branchName.Select(q => q.branchName).First();

                        //    // BranchId used in BranchController, Index function
                        //    Session["BranchId"] = BranchId;

                        //    return RedirectToAction("Index", "Branch", new { area = "Branch", BranchId = Session["BranchId"] });
                        //}

                        //Admin login
                        else if (permission == "1" && BranchId != null)
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        }

                        else
                        {
                            TempData["permissionError"] = "You don't have a sufficient permission to access this site!";
                        }
                    }

                    else
                    {
                        TempData["loginError"] = "Your username or password is not correct. Please try again!";
                    }

                }
                catch
                //(System.Data.SqlClient.SqlException ex)
                {
                    return PartialView("_ErrorHandling");
                    //ModelState.AddModelError("ERROR", ex.Errors.ToString());
                    //return View();
                }
                return RedirectToLocal(returnUrl);
            }
            return View();
        }

        //********** Form Anthentication **********//********** 09/01/2016 *****//
        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {

                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("BagLibrary", "Home");
            }
        }

        // ********** End Form Anthentication **********//********** 09/01/2016 *****//

    }
}