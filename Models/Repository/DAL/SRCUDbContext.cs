using SRCUBagTracking.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SRCUBagTracking.Repository.DAL
{
    public class SRCUDbContext : DbContext
    {
        public SRCUDbContext()
            : base("name=PublicWebEntities")
        {
        }
        public DbSet<Bag> Bags { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<School> Schools { get; set; }     
        public DbSet<SendEmail> SendEmails { get; set; }
        public DbSet<FileDetailsModel> FileDetailsModels { get; set; }
    }
}