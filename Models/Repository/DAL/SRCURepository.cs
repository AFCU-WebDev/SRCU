using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRCUBagTracking.Models;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;

namespace SRCUBagTracking.Repository.DAL
{
    public class SRCURepository :ISRCURepository , IDisposable 
    {
        private SRCUDbContext srcuDb;
        public SRCURepository(SRCUDbContext srcuDbContext)
        {
            this.srcuDb = srcuDbContext;
        }

        // Add School 
        public void AddSchool(School school)
        {
            srcuDb.Schools.Add(school);
            srcuDb.SaveChanges();
        }

        //Add User
        public void AddUser(Login Logins)
        {
            srcuDb.Logins.Add(Logins);
            srcuDb.SaveChanges();
        }

        public void AddNewPDF(FileDetailsModel newPDF)
        {
            srcuDb.FileDetailsModels.Add(newPDF);
            srcuDb.SaveChanges();
        }

        //Delete Bag
        public void DeleteBag(int recId)
        {
            Bag bag = srcuDb.Bags.Find(recId);
            srcuDb.Bags.Remove(bag);
            srcuDb.SaveChanges();
        }

        // send emails using stored procedure SendEmail
        public int DeliverEmail(SendEmail email)
        {
            try
            {
                srcuDb.Database.Connection.Open();
                IDbCommand cmd = srcuDb.Database.Connection.CreateCommand();
                cmd.CommandText = "SendEmail";
                cmd.CommandType = CommandType.StoredProcedure;
                IDbDataParameter ReturnValue = cmd.CreateParameter();
                ReturnValue.ParameterName = "@return";
                ReturnValue.Direction = ParameterDirection.ReturnValue;
                ReturnValue.DbType = DbType.Int32;
                cmd.Parameters.Add(ReturnValue);
                cmd.Parameters.Add(new SqlParameter("MailServer", email.MailServer));
                cmd.Parameters.Add(new SqlParameter("From", email.From));
                cmd.Parameters.Add(new SqlParameter("To", email.To));
                cmd.Parameters.Add(new SqlParameter("CC", email.CC));
                cmd.Parameters.Add(new SqlParameter("Subject", email.Subject));
                cmd.Parameters.Add(new SqlParameter("Body", email.Body));

                cmd.ExecuteReader();
                cmd.Connection.Close();

                return Convert.ToInt32(ReturnValue.Value);
            }
            catch (Exception ex)
            {
                return Convert.ToInt32(ex);
            }
        }

        // bag CRUD
        public Bag GetBag(int bagId)
        {
            return srcuDb.Bags.Find(bagId);
        }

        public IEnumerable<Bag> GetBags(int schoolId)
        {
            var query = (from q in srcuDb.Bags.Where(q => q.school_ID == schoolId)
                         select q).OrderByDescending(q => q.dateSubmitted);

            return query.ToList();
        }

        // school CRUD
        public School GetSchool(int schoolId)
        {
            return srcuDb.Schools.Find(schoolId);
        }

        public IEnumerable<School> GetSchools()
        {
            return srcuDb.Schools.Where(q => q.active == true).ToList();
        }

        //Insert Bag
        public void InsertBag(Bag bag)
        {
            srcuDb.Bags.Add(bag);
            srcuDb.SaveChanges();
        }

        //Update Bag
        public void UpdateBag(Bag bag)
        {
            //srcuDb.Bags.Attach(bag);
            srcuDb.Entry(bag).State = System.Data.Entity.EntityState.Modified;
            srcuDb.SaveChanges();
        }

        //Update Login
        public void UpdateLogin(Login loginInfo) {
            srcuDb.Entry(loginInfo).State = System.Data.Entity.EntityState.Modified;
        }

        //Update School
        public void UpdateSchool(School school)
        {
            srcuDb.Entry(school).State = System.Data.Entity.EntityState.Modified;
        }

        public void Save()
        {
            srcuDb.SaveChanges();
        }

        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }


        
    }
}