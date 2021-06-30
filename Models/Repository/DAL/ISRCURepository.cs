using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRCUBagTracking.Models;

namespace SRCUBagTracking.Repository.DAL
{
    public interface ISRCURepository : IDisposable
    {
        void AddSchool(School school);
        void AddUser(Login Logins);
        void AddNewPDF(FileDetailsModel newPDF);
        void DeleteBag(int recId);
        int DeliverEmail(SendEmail email);
        Bag GetBag(int bagId);
        IEnumerable<Bag> GetBags(int schoolId);
        School GetSchool(int schoolId);
        IEnumerable<School> GetSchools();
        void InsertBag(Bag bag);
        void UpdateBag(Bag bag);
        void UpdateLogin(Login loginInfo);
        void UpdateSchool(School school);
        void Save();
    }
}