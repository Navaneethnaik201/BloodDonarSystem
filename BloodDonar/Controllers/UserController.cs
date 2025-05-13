using BloodDonar.Models;
using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodDonar.Controllers
{
    public class UserController : Controller
    {
        BloodBankDBEntities DB = new BloodBankDBEntities();
        // GET: User
        public ActionResult UserProfile(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            var user = DB.UserTables.Find(id);
            return View(user);
        }

        public ActionResult EditUserProfile(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
           
            var userprofile = new RegisterationMV();
            var user = DB.UserTables.Find(id);
            userprofile.UserTypeID = user.UserTypeID;

            userprofile.User.UserID = user.UserID;
            userprofile.User.UserName = user.UserName;
            userprofile.User.EmailAddress = user.EmailAddress;
            userprofile.User.AccountStatusID = user.AccountStatusID;
            userprofile.User.UserTypeID = user.UserTypeID;
            userprofile.User.Description = user.Description;

            if(user.HospitalTables.Count > 0)
            {
                var hospital = user.HospitalTables.FirstOrDefault();
                userprofile.Hospital.HospitalID = hospital.HospitalID;
                userprofile.Hospital.FullName = hospital.FullName;
                userprofile.Hospital.Address = hospital.Address;
                userprofile.Hospital.PhoneNo = hospital.PhoneNo;
                userprofile.Hospital.Website = hospital.Website;
                userprofile.Hospital.Email = hospital.Email;
                userprofile.Hospital.Loaction = hospital.Loaction;
                userprofile.Hospital.CityID = hospital.CityID;
                userprofile.Hospital.UserID = hospital.UserID;

                userprofile.ContactNo = hospital.PhoneNo;
                userprofile.CityID = hospital.CityID;
                
            }
            else if(user.BloodBankTables.Count > 0)
            {
                var bloodbank = user.BloodBankTables.FirstOrDefault();

                userprofile.BloodBank.BloodbankID = bloodbank.BloodbankID;
                userprofile.BloodBank.BloodBankName = bloodbank.BloodBankName;
                userprofile.BloodBank.Address = bloodbank.Address;
                userprofile.BloodBank.PhoneNo = bloodbank.PhoneNo;
                userprofile.BloodBank.Location = bloodbank.Location;
                userprofile.BloodBank.WebSite = bloodbank.WebSite;
                userprofile.BloodBank.Email = bloodbank.Email;
                userprofile.BloodBank.CityID = bloodbank.CityID;
                
                userprofile.BloodBank.UserID = bloodbank.UserID;
                userprofile.ContactNo = bloodbank.PhoneNo;
                userprofile.CityID = bloodbank.CityID;




            }
            else if (user.DonarTables.Count > 0)
            {
                var donor = user.DonarTables.FirstOrDefault();

                userprofile.Donar.DonorID = donor.DonorID;
                userprofile.Donar.FullName = donor.FullName;
                userprofile.Donar.LastDonationdate = donor.LastDonationdate;
                userprofile.Donar.ContactNo = donor.ContactNo;
                userprofile.Donar.CNIC = donor.CNIC;
                userprofile.Donar.Location = donor.Location;
                userprofile.Donar.CityID = donor.CityID;
                userprofile.Donar.BloodGroupID = donor.BloodGroupID;
                userprofile.Donar.UserID = donor.UserID;
                userprofile.ContactNo = donor.ContactNo;
                userprofile.CityID = donor.CityID;
                userprofile.BloodGroupID = donor.BloodGroupID;



            }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", userprofile.CityID);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", userprofile.BloodGroupID);
            return View(userprofile);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        public ActionResult EditUserProfile(RegisterationMV userprofile)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            if (ModelState.IsValid)
            {
                var Checkuseremail = DB.UserTables.Where(u => u.EmailAddress == userprofile.User.EmailAddress && u.UserID != userprofile.User.UserID).FirstOrDefault();
                if (Checkuseremail != null)
                {
                    try
                    {
                        var user = DB.UserTables.Find(userprofile.User.UserID);
                        user.EmailAddress = userprofile.User.EmailAddress;
                        DB.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        DB.SaveChanges();

                        if (userprofile.Donar.DonorID != 0)
                        {
                            var donor = DB.DonarTables.Find(userprofile.Donar.DonorID);
                            donor.FullName = userprofile.Donar.FullName;
                            donor.BloodGroupID = userprofile.Donar.BloodGroupID;
                            donor.ContactNo = userprofile.Donar.ContactNo;
                            donor.CNIC = userprofile.Donar.CNIC;
                            donor.CityID = userprofile.Donar.CityID;
                            donor.Location = userprofile.Donar.Location;
                            DB.Entry(donor).State = System.Data.Entity.EntityState.Modified;
                            DB.SaveChanges();
                        }
                        else if (userprofile.BloodBank.BloodbankID != 0)
                        {
                            var bloodbank = DB.BloodBankTables.Find(userprofile.BloodBank.BloodbankID);
                            bloodbank.BloodBankName = userprofile.BloodBank.BloodBankName;
                            bloodbank.PhoneNo = userprofile.BloodBank.PhoneNo;
                            bloodbank.Email = userprofile.BloodBank.Email;
                            bloodbank.WebSite = userprofile.BloodBank.WebSite;
                            bloodbank.CityID = userprofile.BloodBank.CityID;
                            bloodbank.Address = userprofile.BloodBank.Address;
                            DB.Entry(bloodbank).State = System.Data.Entity.EntityState.Modified;
                            DB.SaveChanges();
                        }
                        else if (userprofile.Hospital.HospitalID != 0)
                        {
                            var hospital = DB.HospitalTables.Find(userprofile.Hospital.HospitalID);
                            hospital.FullName = userprofile.Hospital.FullName;
                            hospital.PhoneNo = userprofile.Hospital.PhoneNo;
                            hospital.Email = userprofile.Hospital.Email;
                            hospital.Website = userprofile.Hospital.Website;
                            hospital.Address = userprofile.Hospital.Address;
                            hospital.CityID = userprofile.Hospital.CityID;
                            DB.Entry(hospital).State = System.Data.Entity.EntityState.Modified;
                            DB.SaveChanges();
                        }
                        return RedirectToAction("UserProfile","User",new {id = userprofile.User.UserID});
                    }
                    catch
                    {

                        ModelState.AddModelError(string.Empty, "Data is Incorrect");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User email already Exists");
                }

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Data is Incorrect");
            }

            //var user = DB.UserTables.Find(id);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", userprofile.CityID);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", userprofile.BloodGroupID);
            return View(userprofile);

        }
    }
}