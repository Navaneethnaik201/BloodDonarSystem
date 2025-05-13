using BloodDonar.Models;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;

namespace BloodDonar.Controllers
{

    public class RegistrationController : Controller
    {
        BloodBankDBEntities DB = new BloodBankDBEntities();

        static RegisterationMV registerationmv;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectUser(RegisterationMV registerationMV)
        {
            registerationmv = registerationMV;
            if (registerationMV.UserTypeID == 2)
            {
                return RedirectToAction("DonarUser");
            } else if (registerationMV.UserTypeID == 4)
            {
                return RedirectToAction("HopitalUser");

            }
            else if (registerationMV.UserTypeID == 5)
            {
                return RedirectToAction("BloodBankUser");

            }
            else
            {
                return RedirectToAction("MainHome", "Home");
            }




        }
        public ActionResult HopitalUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", registerationmv.CityID);
            return View(registerationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HopitalUser(RegisterationMV registerationMV) {

            if (ModelState.IsValid)
            {
                var checktitle = DB.HospitalTables.Where(h => h.FullName == registerationMV.Hospital.FullName.Trim()).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {

                        try
                        {
                            var user = new UserTable();
                            user.UserName = registerationMV.User.UserName;
                            user.Password = registerationMV.User.Password;
                            user.EmailAddress = registerationMV.User.EmailAddress;
                            user.AccountStatusID = 1002;
                            user.UserTypeID = registerationMV.UserTypeID;
                            user.Description = registerationMV.User.Description;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            
                            
                            var hospital = new HospitalTable(); 
                            hospital.FullName= registerationMV.Hospital.FullName;
                            hospital.Address= registerationMV.Hospital.Address;
                            hospital.PhoneNo= registerationMV.Hospital.PhoneNo;
                            hospital.Website= registerationMV.Hospital.Website;
                            hospital.Email= registerationMV.Hospital.Email;
                            hospital.Loaction= registerationMV.Hospital.Address;
                            hospital.CityID = registerationMV.CityID;
                            hospital.UserID = user.UserID;
                            DB.HospitalTables.Add(hospital);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank you  for Rgeistration,your query will be review shortly!";
                            return RedirectToAction("MainHome","Home");
                        }
                        catch   
                        {
                            ModelState.AddModelError(string.Empty, "Please provide Correct Information!");

                            transaction.Rollback();
                        }

                    }





                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Hospital already Registered");


                }


            }
        

            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", registerationMV.CityID);
            return View(registerationMV);
        }




        public ActionResult DonarUser()
        {

            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", registerationmv.CityID);
            ViewBag. BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            return View(registerationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]      
        public ActionResult DonarUser(RegisterationMV registerationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.DonarTables.Where(h => h.FullName == registerationMV.Donar.FullName.Trim() && h.CNIC == registerationMV.Donar.CNIC).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {

                        try
                        {
                            var user = new UserTable();
                            user.UserName = registerationMV.User.UserName;
                            user.Password = registerationMV.User.Password;
                            user.EmailAddress = registerationMV.User.EmailAddress;
                            user.AccountStatusID = 1002;
                            user.UserTypeID = registerationMV.UserTypeID;
                            user.Description = registerationMV.User.Description;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var donar = new DonarTable();
                            
                            donar.FullName = registerationMV.Donar.FullName;
                            donar.BloodGroupID = registerationMV.BloodGroupID;
                            donar.Location = registerationMV.Donar.Location;
                            donar.ContactNo = registerationMV.Donar.ContactNo;
                            donar.LastDonationdate = registerationMV.Donar.LastDonationdate;
                            donar.CNIC = registerationMV.Donar.CNIC;
                            donar.CityID = registerationMV.CityID;
                            donar.UserID = user.UserID;
                            DB.DonarTables.Add(donar);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank you  for Rgeistration,your query will be review shortly!";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Please provide Correct Information!");
                            transaction.Rollback();
                        }

                    }





                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Donar already Registered");


                }


            }

            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", registerationMV.BloodGroupID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", registerationMV.CityID);
      
            return View(registerationMV);

        }


        public ActionResult BloodBankUser()
        {

            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", registerationmv.CityID);
            return View(registerationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BloodBankUser(RegisterationMV registerationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.BloodBankTables.Where(h => h.BloodBankName == registerationMV.BloodBank.BloodBankName.Trim() && h.PhoneNo == registerationMV.BloodBank.PhoneNo).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {

                        try
                        {
                            var user = new UserTable();
                            user.UserName = registerationMV.User.UserName;
                            user.Password = registerationMV.User.Password;
                            user.EmailAddress = registerationMV.User.EmailAddress;
                            user.AccountStatusID = 1002;
                            user.UserTypeID = registerationMV.UserTypeID;
                            user.Description = registerationMV.User.Description;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var bloodBank = new BloodBankTable();
                             bloodBank.BloodBankName = registerationMV.BloodBank.BloodBankName;
                             bloodBank.Address = registerationMV.BloodBank.Location;
                             bloodBank.Location = registerationMV.BloodBank.Location;
                             bloodBank.PhoneNo = registerationMV.BloodBank.PhoneNo;
                             bloodBank.WebSite = registerationMV.BloodBank.WebSite;
                             bloodBank.CityID = registerationMV.CityID;
                             bloodBank.UserID = user.UserID;
                             bloodBank.Email = registerationMV.BloodBank.Email;
                             DB.BloodBankTables.Add(bloodBank);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank you  for Rgeistration,your query will be review shortly!";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Please provide Correct Information!");

                            transaction.Rollback();
                        }

                    }





                }
                else
                {
                    ModelState.AddModelError(string.Empty, "blood bank already Registered");


                }


            }

          
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", registerationMV.CityID);

            return View(registerationMV);
        }


    }
}