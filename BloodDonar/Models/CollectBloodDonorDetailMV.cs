using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BloodDonar.Models
{
    public class CollectBloodDonorDetailMV
    {
        public int DonorID { get; set; }
        public string FullName { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime LastDonationdate { get; set; }
        public string ContactNo { get; set; }
        public string CNIC { get; set; }
        public string Location { get; set; }
        public int CityID { get; set; }
        public int BloodGroupID { get; set; }

        public string BloodGroup { get; set; }

        public string City { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string AccountStatusID { get; set; }

        public string UserTypeID { get; set; }

        public string Description { get; set; }
    }
}