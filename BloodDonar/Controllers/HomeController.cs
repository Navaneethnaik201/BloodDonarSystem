using BloodDonar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using System.ComponentModel.DataAnnotations;

namespace BloodDonar.Controllers
{
    public class HomeController : Controller
    {
        BloodBankDBEntities DB = new BloodBankDBEntities();


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MainHome()
        {
            var message = ViewData["Message"] == null ? "Welcome to Blood Donation" : ViewData["Message"];
            ViewData["Message"] = message;
            var registration = new RegisterationMV();
            ViewBag.UserTypeID = new SelectList(DB.UserTypeTables.Where(ut => ut.UserTypeID > 1).ToList(), "UserTypeID", "UserType", "0");
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", "0");
            return View(registration);

        }
        public ActionResult Login()
        {
            var usermv = new UserMV();
            return View(usermv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login(UserMV userMV)
        {
            if (ModelState.IsValid)
            {
                var user = DB.UserTables.Where(u => u.Password == userMV.Password && u.UserName == userMV.UserName).FirstOrDefault();
                if (user != null)
                {
                    if (user.AccountStatusID == 1002)
                    {
                        ModelState.AddModelError(String.Empty, "please wait,your Account is under Review");

                    }
                    else if (user.AccountStatusID == 1003)
                    {
                        ModelState.AddModelError(String.Empty, "Your Account is rejected!");

                    }
                    else if (user.AccountStatusID == 2)
                    {
                        ModelState.AddModelError(String.Empty, "Your Account is Suspended!");

                    }
                    else if (user.AccountStatusID == 1004)
                    {
                        Session["UserID"] = user.UserID;

                        Session["UserName"] = user.UserName;
                        Session["Password"] = user.Password;
                        Session["EmailAddress"] = user.EmailAddress;
                        Session["AccountStatusID"] = user.AccountStatusID;

                        Session["AccountStatus"] = user.AccountStatusTable.AccountStatus;
                        Session["UserTypeID"] = user.UserTypeID;

                        Session["UserType"] = user.UserTypeTable.UserType;

                        Session["Description"] = user.Description;
                        



                        if (user.UserTypeID == 1) //Admin
                        {
                            return RedirectToAction("AllNewUserRequests","Accounts");

                        }
                        else if (user.UserTypeID == 2)
                        {
                            var donar = DB.DonarTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                            if (donar != null)
                            {


                                Session["DonorID"] = donar.DonorID;
                                Session["FullName"] = donar.FullName;


                                Session["LastDonationdate"] = donar.LastDonationdate;
                                Session["ContactNo"] = donar.ContactNo;
                                Session["CNIC"] = donar.CNIC;
                                Session["Location"] = donar.Location;
                                Session["CityID"] = donar.CityID;
                                Session["BloodGroupID"] = donar.BloodGroupID;

                                Session["BloodGroup"] = donar.BloodGroupsTable.BloodGroup;

                                Session["City"] = donar.CityTable.City;
                                return RedirectToAction("Donor","Dashboard");

                            }
                            else
                            {
                                

                                ModelState.AddModelError(String.Empty, "Username and password is Incorrect!");

                            }
                        }
                        else if (user.UserTypeID == 4)
                        {
                            var hospital = DB.HospitalTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                            if (hospital != null)
                            {

                                Session["HospitalID"] = hospital.HospitalID;

                                Session["FullName"] = hospital.FullName;

                                Session["Address"] = hospital.Address;

                                Session["PhoneNo"] = hospital.PhoneNo;

                                Session["Website"] = hospital.Website;

                                Session["Email"] = hospital.Email;

                                Session["Loaction"] = hospital.Loaction;

                                Session["CityID"] = hospital.CityID;

                                Session["City"] = hospital.CityTable.City;
                                return RedirectToAction("Hospital", "Dashboard");

                            }
                            else
                            {
                                ModelState.AddModelError(String.Empty, "Username and password is Incorrect!");

                            }
                        }
                        else if (user.UserTypeID == 5)  //bloodbank
                        {
                            var bloodbank = DB.BloodBankTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                            if (bloodbank != null)
                            {

                                Session["BloodbankID"] = bloodbank.BloodbankID;
                                Session["BloodBankName"] = bloodbank.BloodBankName;
                                Session["Address"] = bloodbank.Address;
                                Session["PhoneNo"] = bloodbank.PhoneNo;
                                Session["Location"] = bloodbank.Location;
                                Session["WebSite"] = bloodbank.WebSite;
                                Session["Email"] = bloodbank.Email;
                                Session["CityID"] = bloodbank.CityID;
                                Session["City"] = bloodbank.CityTable.City;
                                return RedirectToAction("BloodBank", "Dashboard");
                            }
                            else
                            {
                                ModelState.AddModelError(String.Empty, "Username and password is Incorrect!");

                            }
                        }
                        else
                        {
                            ModelState.AddModelError(String.Empty, "Username and password is Incorrect!");

                        }

                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Username and password is Incorrect!");

                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "please provide username and password!");
                }
                
                

            }
            
            ClearSession();
            return View(userMV);



        }

        

        public void ClearSession()
        {
            Session["UserID"] = string.Empty;

            Session["UserName"] = string.Empty;
            Session["Password"] = string.Empty; ;
            Session["EmailAddress"] = string.Empty;
            Session["AccountStatusID"] = string.Empty;

            Session["AccountStatus"] = string.Empty;
            Session["UserTypeID"] = string.Empty;

            Session["UserType"] = string.Empty;

            Session["Description"] = string.Empty;


        }
        public ActionResult Logout()
        {
            ClearSession();
            return RedirectToAction("MainHome");
        }

        public ActionResult AboutUs()
        {
            return View();
        }
    }


}