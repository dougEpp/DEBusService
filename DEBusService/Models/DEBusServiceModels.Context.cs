﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DEBusService.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BusServiceContext : DbContext
    {
        public BusServiceContext()
            : base("name=BusServiceContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<bus> buses { get; set; }
        public virtual DbSet<busRoute> busRoutes { get; set; }
        public virtual DbSet<busStop> busStops { get; set; }
        public virtual DbSet<driver> drivers { get; set; }
        public virtual DbSet<routeSchedule> routeSchedules { get; set; }
        public virtual DbSet<routeStop> routeStops { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<trip> trips { get; set; }
        public virtual DbSet<tripStop> tripStops { get; set; }
    }
}
