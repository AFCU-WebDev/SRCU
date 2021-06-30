using SRCUBagTracking.Controllers;
using SRCUBagTracking.Models;
using SRCUBagTracking.Repository.DAL;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//using XsPDF.Pdf;

namespace SRCUBagTracking.Areas.Admin.Controllers
{
    [CheckSessionOutAttribute]
    public class AdminController : Controller
    {
        // define the repository
        private readonly SRCUDbContext srcuDb = new SRCUDbContext();
        private readonly ISRCURepository _srcuRepositoryInterface;

        public AdminController()
        {
            this._srcuRepositoryInterface = new SRCURepository(new SRCUDbContext());
        }

        // ---GET:
        // Generating the Add Bag form
        public ActionResult AddBag(int schoolId)
        {

            ViewBag.SchoolId = schoolId;
            return View(new Bag());
        }

        //POST :
        //Submit all the Add Bag form values in "srcuBagReport" SQL table
        [HttpPost]
        public ActionResult AddBag(Bag model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Add bag with all submitted values
                    _srcuRepositoryInterface.InsertBag(model);
                    _srcuRepositoryInterface.Save();

                    //return to school bag list page after submitting the "Add Bag" form
                    return RedirectToAction("SchoolBag", "Admin", new { area = "Admin", schoolId = model.school_ID });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View();
        }


        //GET :
        //Generating the "Add New School" form
        public ActionResult AddSchool()
        {
            return View(new School());
        }

        //POST :
        //Submit all the "Add New School" form values in "srcuSchoolsInfo" SQL table
        [HttpPost]
        public ActionResult AddSchool(School school)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _srcuRepositoryInterface.AddSchool(school);

                    // Creating Username for New school
                    var schoolTypeInitial = school.schoolType[0].ToString() + "S";
                    var newUsername = (school.schoolName + schoolTypeInitial);
                    newUsername = newUsername.Replace(" ", "");

                    //Creating Password for New School
                    var newPassword = Membership.GeneratePassword(10, 2);

                    Login logins = new Login();
                    logins.username = newUsername;
                    logins.password = newPassword;
                    logins.permission = "3";
                    logins.Branch_ID = school.Branch_ID;
                    logins.school_ID = school.school_ID;

                    _srcuRepositoryInterface.AddUser(logins);

                    _srcuRepositoryInterface.Save();

                    //return to schools home after submitting to add the school
                    return RedirectToAction("Schools", "Admin", new { area = "Admin" });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(school);
        }

        //GET: 
        //Admin can see and read the branches and liaisons comments
        public ActionResult AdminComments(int recId, int schoolId)
        {
            Bag bag = _srcuRepositoryInterface.GetBag(recId);

            //Liaison comments
            ViewBag.liaisonComment = bag.liasonComment;

            //Branch comments
            ViewBag.branchComment = bag.branchComment;

            //date comment has been submitted
            ViewBag.submittedDate = bag.dateSubmitted;

            return View(bag);
        }


        public ActionResult AdminMarketing()
        {
            //Letter Category from srcuFileDetails db - The code check if there is any letters in sql table and list it in table
            var LetterCategoryFiles = (from q in srcuDb.FileDetailsModels.Where(q => q.fileCategory == "Letters")
                                       select q).ToList();
           
            //saved the Letter categoty file in viewbag
            ViewBag.LetterCategoryFiles = LetterCategoryFiles;


            //Flyers Category from srcuFileDetails db - The code check if there is any Flyers in sql table and list it in table
            var FlyersCategoryFiles = (from q in srcuDb.FileDetailsModels.Where(q => q.fileCategory == "Flyers")
                                       select q).OrderBy(q=> q.Order_appearance).ToList();
            //saved the Flyers Category Files in viewbag
            ViewBag.FlyersCategoryFiles = FlyersCategoryFiles;

            return View(new FileDetailsModel());
        }

        [HttpPost]
        public ActionResult AdminMarketing(FileDetailsModel model, HttpPostedFileBase files, HttpPostedFileBase thumbnail)
        {
            String FileExt = Path.GetExtension(files.FileName).ToUpper();
            String imageExt = Path.GetExtension(thumbnail.FileName).ToUpper();

            if (FileExt == ".PDF" && (imageExt == ".PNG" || imageExt ==".JPG" ))
            {
                // get the file name and save it in the file system
                string _FileName = Path.GetFileName(files.FileName);
                string _imageName = Path.GetFileName(thumbnail.FileName);

                String fileFullPath = Path.GetFullPath(_FileName);
                String imageFullPath = Path.GetFullPath(_imageName);
                //Add "/SRCUBagTracker" to the document and image path for windows authentication/ AppleNet 
                string _path = Path.Combine(Server.MapPath("~/Content/docs/Marketing/UploadedFiles"), _FileName);
                string thumbnailPath = Path.Combine(Server.MapPath("~/Content/img/pics/Marketing"), _imageName);
                files.SaveAs(_path);
                thumbnail.SaveAs(thumbnailPath);

                // Read the file from the file system and convert it to binary
                FileStream str = new FileStream(_path, FileMode.Open);
                BinaryReader Br = new BinaryReader(str);
                //Stream str = files.InputStream;
                //BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                //save the data in DB
                FileDetailsModel Fd = new FileDetailsModel();
                model.fileName = Path.GetFileNameWithoutExtension(files.FileName).ToString();
                Fd.fileName = model.fileName;
                model.fileContent = FileDet;
                Fd.fileContent = model.fileContent;
                Fd.fileCategory = model.fileCategory;
                Fd.dateUploaded = DateTime.Now;
                Fd.fileTitle = model.fileTitle;

               _srcuRepositoryInterface.AddNewPDF(model);
                return RedirectToAction("AdminMarketing", "Admin");
            }
            else
            {
                ViewBag.FileStatus = "Invalid file format.";
                return View();
            }
        }


        // GET : 
        //Pop-up the Delete Alert confirmation window
        public ActionResult DeleteBag()
        {
            return View();
        }


        //Delete the bag from  "srcuBagReport" SQL table
        [HttpPost]
        public ActionResult DeleteBag(int recId, int schoolId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Delete bag with related Id
                    _srcuRepositoryInterface.DeleteBag(recId);
                    _srcuRepositoryInterface.Save();
                    //Return to school bag page after deleting the bag
                    return RedirectToAction("SchoolBag", "Admin", new { area = "Admin", schoolId = schoolId });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View();
        }





        //GET: SCHOOL LIST
        //Getting all the Active schools from [srcuSchoolsInfo] SQL table 
        public ActionResult Schools()
        {
            var SchoolName = (from q in srcuDb.Schools.Where(q => q.schoolName != null && q.active == true)
                              select q).OrderByDescending(q => q.schoolName).ToList();
            ViewBag.schoolName = SchoolName;
            return View(SchoolName);
        }

        //POST : 
        [HttpPost]
        public ActionResult Schools(School model)
        {
            return View();
        }

        //Get:       
        //Getting School Bag page with related school ID
        public ActionResult SchoolBag(int schoolId)
        {

            //Getting all the Bags with related school Id from [srcuBagReport] SQL table 
            var bag = from Bag in _srcuRepositoryInterface.GetBags(schoolId)
                      select Bag;
            ViewBag.SchoolId = schoolId;

            //Getting all the Active schools name [srcuSchoolsInfo] SQL table 
            var SchoolName = (from q in srcuDb.Schools.Where(q => q.schoolName != null && q.active == true && q.school_ID == schoolId)
                              select q.schoolName).FirstOrDefault();
            ViewBag.schoolName = SchoolName;
            return View(bag);
        }

        // GET: Admin/Admin
        public ActionResult SchoolsDetail()
        {
            var school = from School in _srcuRepositoryInterface.GetSchools()
                         select School;
            return View(school);
        }

        //GET://
        public ActionResult SchoolProfile(int schoolId)
        {

            // Get the login Info from Login table 
            var loginInfo = (from logins in srcuDb.Logins.Where(q => q.school_ID == schoolId)
                             select logins).FirstOrDefault();
            return View(loginInfo);
        }

        //POST://
        [HttpPost]
        public ActionResult SchoolProfile(Login updatedLoginInfo)
        {
            _srcuRepositoryInterface.UpdateLogin(updatedLoginInfo);
            _srcuRepositoryInterface.Save();
            return RedirectToAction("SchoolBag", "Admin", new { area = "Admin", schoolId = updatedLoginInfo.school_ID });
        }

        //GET:
        // Getting "Update Bag" form with existing values from [srcuBagReport] SQL table after clicking on "Edit" in the Schools Bag page
        public ActionResult UpdateBag(int recId)
        {
            Bag bag = _srcuRepositoryInterface.GetBag(recId);
            return View(bag);

        }

        //POST:
        //Post all the updated values from "Update Bag" form to the [srcuBagReport] SQL table 
        [HttpPost]
        public ActionResult UpdateBag(Bag bag)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _srcuRepositoryInterface.UpdateBag(bag);
                    _srcuRepositoryInterface.Save();
                    return RedirectToAction("SchoolBag", "Admin", new { area = "Admin", schoolId = bag.school_ID });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, " + "and if the problem persists see your system administrator.");
            }
            return View(bag);
        }

        //GET :
        // Getting "Update School" form with existing values from [srcuSchoolsInfo] SQL table after clicking on "Edit" in the Schools Details page
        public ActionResult UpdateSchool(int schoolId)
        {
            //var schoolId = Convert.ToInt32(Session["schoolID"]);
            School school = _srcuRepositoryInterface.GetSchool(schoolId);
            return View(school);
        }

        //POST:
        //Post all the updated values from "Update School" form to the [srcuSchoolsInfo] SQL table 
        [HttpPost]
        public ActionResult UpdateSchool(School school)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _srcuRepositoryInterface.UpdateSchool(school);
                    _srcuRepositoryInterface.Save();
                    //return to the "Schools Details" page after submitting the Updates
                    return RedirectToAction("SchoolsDetail", "Admin", new { area = "Admin" });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, " + "and if the problem persists see your system administrator.");
            }
            return View(school);
        }

        //public ActionResult UploadMarketing()
        //{
        //    return View();
        //}

        }  
    }
