﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace totalhr.data.TimeRecordingSystem.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TimeRecordingEntities : DbContext
    {
        public TimeRecordingEntities()
            : base("name=TimeRecordingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ScheduledNotification> ScheduledNotifications { get; set; }
        public virtual DbSet<TaskScheduler> TaskSchedulers { get; set; }
        public virtual DbSet<TimeRecording> TimeRecordings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<TimeRecordingType> TimeRecordingTypes { get; set; }
    }
}