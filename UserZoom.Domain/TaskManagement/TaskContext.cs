using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Domain;

namespace UserZoom.Domain.TaskManagement
{
    public sealed class UZTaskContext : DbContext
    {
        public UZTaskContext() 
            : base(ConfigurationManager.AppSettings["uz:data:connectionString:name"])
        {
        }

        public DbSet<UZTask> Tasks { get; set; }
    }
}
