﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserZoom.Domain;

namespace UserZoom.Shared.Test.EntityFramework
{
    public sealed class TaskContext : DbContext
    {
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //var entityConfig = modelBuilder.Entity<UZTask>();

        //    //entityConfig.HasKey(t => t.Id);

        //    base.OnModelCreating(modelBuilder);
        //}

        public TaskContext() 
            : base(ConfigurationManager.AppSettings["uz:data:connectionString:name"])
        {

        }

        public DbSet<UZTask> Tasks { get; set; }
    }
}
