using Risarc.Enterprise.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risarc.Enterprise.Repository
{
    public class EnterpriseContext : DbContext
    {
        public EnterpriseContext()
            : base(ConfigurationManager.ConnectionStrings["EnterpriseConnection"].ToString())
        {
        }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<JobSchedule> JobSchedules { get; set; }

        public DbSet<FileImportingJob> FileImportingJobs { get; set; }

        public DbSet<Server> Servers { get; set; }

        public DbSet<JobLog> JobLogs { get; set; }

        public DbSet<DependantJob> DependantJobs { get; set; }

        public DbSet<JobCategory> JobCategories { get; set; }

        public List<TEntity> GetEntityBySp<TEntity>(string storedProcedure)
        {
            return ((IEnumerable<TEntity>)this.Database.SqlQuery<TEntity>(storedProcedure, new object[0])).ToList<TEntity>();
        }

        public List<TEntity> GetEntityBySp<TEntity>(string storedProcedure, object[] paramValues)
        {
            string str1 = "CALL " + storedProcedure + "(";
            foreach (object paramValue in paramValues)
            {
                string empty = string.Empty;
                string str2 = paramValue.GetType().Equals(typeof(int)) ? paramValue.ToString() : "'" + paramValue.ToString() + "'";
                str1 = str1 + str2 + ",";
            }
            return ((IEnumerable<TEntity>)this.Database.SqlQuery<TEntity>(str1.Remove(str1.LastIndexOf(",")) + ")", new object[0])).ToList<TEntity>();
        }

        public void Attach<T>(T entity, DbSet<T> dbSet) where T : class
        {
            dbSet.Attach(entity);
            this.Entry(entity).State = EntityState.Modified;
        }
    }
}
