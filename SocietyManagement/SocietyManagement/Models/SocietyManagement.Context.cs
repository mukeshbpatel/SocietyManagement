﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocietyManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SocietyManagementEntities : DbContext
    {
        public SocietyManagementEntities()
            : base("name=SocietyManagementEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<BuildingUnit> BuildingUnits { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<ComplaintComment> ComplaintComments { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<Due> Dues { get; set; }
        public virtual DbSet<ForumComment> ForumComments { get; set; }
        public virtual DbSet<Forum> Forums { get; set; }
        public virtual DbSet<KeyValue> KeyValues { get; set; }
        public virtual DbSet<NoticeBoard> NoticeBoards { get; set; }
        public virtual DbSet<PollOption> PollOptions { get; set; }
        public virtual DbSet<Poll> Polls { get; set; }
        public virtual DbSet<PollVote> PollVotes { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<UserDefineField> UserDefineFields { get; set; }
        public virtual DbSet<RecurringDue> RecurringDues { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Penalty> Penalties { get; set; }
        public virtual DbSet<BuildingUnitMedia> BuildingUnitMedias { get; set; }
        public virtual DbSet<CollectionMedia> CollectionMedias { get; set; }
        public virtual DbSet<ExpenseMedia> ExpenseMedias { get; set; }
        public virtual DbSet<FinancialYear> FinancialYears { get; set; }
        public virtual DbSet<ForumMedia> ForumMedias { get; set; }
        public virtual DbSet<NoticeBoardMedia> NoticeBoardMedias { get; set; }
        public virtual DbSet<SystemSetting> SystemSettings { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
    }
}
