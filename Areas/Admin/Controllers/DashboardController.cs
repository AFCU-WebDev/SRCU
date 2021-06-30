using SRCUBagTracking.Controllers;
using SRCUBagTracking.Repository.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRCUBagTracking.Areas.Admin.Controllers
{
    [CheckSessionOutAttribute]
    public class DashboardController : Controller
    {
        private SRCUDbContext srcuDb = new SRCUDbContext();
        private ISRCURepository _srcuRepositoryInterface;

        public DashboardController()
        {
            this._srcuRepositoryInterface = new SRCURepository(new SRCUDbContext());
        }
 
        // dashboard page which represent the today and yesterday date with the number of bags and applications
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            //--------------------------------DATES-------------------------------

            var TodayDate = DateTime.Today;                 //TODAY

            var YesterdayDate = DateTime.Today.AddDays(-1);   //YESTEREDAY

            var TomorrowDate = DateTime.Today.AddDays(1);     //TOMORROW

            //----------------------TODAY  TODAY   TODAY   TODAY------------------
            //TODAY Submitted Bag
            var TodayTotalBag = (from q in srcuDb.Bags.Where(q => q.dateSubmitted >= TodayDate)
                           select q.bagNumber).Count();
            if (TodayTotalBag == 0)
            {
                TodayTotalBag = 0;
            }
            ViewBag.TodayBag = TodayTotalBag;
            ViewData["TodayBag"] = TodayTotalBag;

            // TODAY Total Deposit
            var TodayTotalDeposit = (from q in srcuDb.Bags.Where(q => q.dateSubmitted >= TodayDate)
                            select q.deposits).Sum();
            if (TodayTotalDeposit == null)
            {
                TodayTotalDeposit = 0;
            }
            ViewBag.TodayDeposits = TodayTotalDeposit;


            // TODAY Total Dollar Amount
            var TodayTotalDollar = (from q in srcuDb.Bags.Where(q => q.dateSubmitted >= TodayDate )
                                    select q.dollarAmount).Sum();

            if (TodayTotalDollar == null)
            {
                TodayTotalDollar = 0;
            }
            ViewBag.TodayDollarAmt = TodayTotalDollar;


            // TODAY Total Apps
            var TodayTotalApps = (from q in srcuDb.Bags.Where(q => q.dateSubmitted >= TodayDate)
                                select q.memberApps).Sum();
            if (TodayTotalApps == null)
            {
                TodayTotalApps = 0;
            }
            ViewBag.TodayApps = TodayTotalApps;

            //---------------YESTERDAY  YESTERDAY   YESTERDAY   YESTERDAY---------------
            //Day Of Week             
            var DayOfWeek = TodayDate.DayOfWeek;
            if (DayOfWeek == DayOfWeek.Monday){
                YesterdayDate = DateTime.Today.AddDays(-3);
            }


            //YESTERDAY Submitted Bag
            var YesterdayTotalBag = (from q in srcuDb.Bags.Where(q => q.dateSubmitted >= YesterdayDate && q.dateSubmitted < TodayDate)
                                     select q.bagNumber).Count();
            if (YesterdayTotalBag == 0)
            {
                YesterdayTotalBag = 0;
            }
            ViewBag.YesterdayBag = YesterdayTotalBag;


            //YESTERDAY Total Deposits
            var YesterdayTotalDeposits = (from q in srcuDb.Bags.Where(q => q.dateSubmitted >= YesterdayDate && q.dateSubmitted < TodayDate)
                                     select q.deposits).Sum();
            if (YesterdayTotalDeposits == null)
            {
                YesterdayTotalDeposits = 0;
            }
            ViewBag.YesterdayDeposits = YesterdayTotalDeposits;

            //YESTERDAY Total Dollar Amount
            var YesterdayTotalDollar = (from q in srcuDb.Bags.Where(q => q.dateSubmitted >= YesterdayDate && q.dateSubmitted < TodayDate)
                                          select q.dollarAmount).Sum();
            if (YesterdayTotalDollar == null)
            {
                YesterdayTotalDollar = 0;
            }
            ViewBag.YesterdayDollarAmt = YesterdayTotalDollar;


            //YESTERDAY Total Apps
            var YesterdayTotalApps = (from q in srcuDb.Bags.Where(q => q.dateSubmitted >= YesterdayDate && q.dateSubmitted < TodayDate)
                                          select q.memberApps).Sum();
            if (YesterdayTotalApps == null)
            {
                YesterdayTotalApps = 0;
            }
            ViewBag.YesterdayApps = YesterdayTotalApps;


            return View();
        }

    }
}