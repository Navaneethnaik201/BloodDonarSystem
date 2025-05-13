using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BloodDonar.Models
{
    public class UserTypeMV
    {
        public int UserTypeID { get; set; }
        [Required(ErrorMessage="Required*")]
        [Display(Name="User Type")]
        public string UserType { get; set; }
    }
}