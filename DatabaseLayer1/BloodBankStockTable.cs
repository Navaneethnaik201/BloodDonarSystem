//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseLayer1
{
    using System;
    using System.Collections.Generic;
    
    public partial class BloodBankStockTable
    {
        public int BloodBankStockID { get; set; }
        public int BloodGroupID { get; set; }
        public bool Status { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int BloodbankID { get; set; }
    
        public virtual BloodBankTable BloodBankTable { get; set; }
        public virtual BloodGroupsTable BloodGroupsTable { get; set; }
    }
}
