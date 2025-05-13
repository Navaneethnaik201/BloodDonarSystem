using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BloodDonar.Models
{
    public class BloodBankStockMV
    {
        public int BloodBankStockID { get; set; }
        public int BloodGroupID { get; set; }
        [Display(Name="Blood Bank")]

        public string BloodBank { get; set; }

        [Display(Name ="Blood Group")]
        public string BloodGrouup { get; set; }
        [Display(Name="Is Ready")]
        public string Status { get; set; }
        [Display(Name ="Blood Details")]
        public double Quantity { get; set; }
        public string Description { get; set; }
        public int BloodbankID { get; set; }
    }
}