﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPF_LoginForm.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ClinicEntities : DbContext
    {
        public ClinicEntities()
            : base("name=ClinicEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<LaboratoryManager> LaboratoryManagers { get; set; }
        public virtual DbSet<LaboratoryTest> LaboratoryTests { get; set; }
        public virtual DbSet<LabTech> LabTeches { get; set; }
        public virtual DbSet<Logging> Loggings { get; set; }
        public virtual DbSet<MedicalGlossary> MedicalGlossaries { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PhysicalExamination> PhysicalExaminations { get; set; }
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
