//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class LaboratoryTest
    {
        public int Id_labTest { get; set; }
        public string Code { get; set; }
        public Nullable<int> LabManager { get; set; }
        public Nullable<int> LabTech { get; set; }
        public Nullable<int> Id_app { get; set; }
        public string DoctorsNote { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string Result { get; set; }
        public Nullable<System.DateTime> DateTaken { get; set; }
        public string ManagersNote { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public string Status { get; set; }
    
        public virtual Appointment Appointment { get; set; }
        public virtual Logging Logging { get; set; }
        public virtual Logging Logging1 { get; set; }
        public virtual MedicalGlossary MedicalGlossary { get; set; }
    }
}