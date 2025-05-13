using BloodDonar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using System.Data.Entity.Hierarchy;

namespace BloodDonar.Controllers
{
    public class AccountsController : Controller
    {
        BloodBankDBEntities DB = new BloodBankDBEntities();
        public ActionResult AllNewUserRequests()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login","Home");


            }
            
            var users = DB.UserTables.Where(u =>u.AccountStatusID == 1002).ToList();   

            return View(users);

        }

        public ActionResult UserDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            var user = DB.UserTables.Find(id);
            return View(user);
        }
        public ActionResult UserApproved(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            var user = DB.UserTables.Find(id);
            user.AccountStatusID = 1004;
            DB.Entry(user).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllNewUserRequests");

        }

        public ActionResult UserRejected(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            var user = DB.UserTables.Find(id);
            user.AccountStatusID = 1003;
            DB.Entry(user).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllNewUserRequests");



        }

        public ActionResult AddNewDonorByBloodBank()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            var collectBloodMV  = new CollectBloodMV();
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", "0");
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult AddNewDonorByBloodBank(CollectBloodMV collectBloodMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }


            int bloodbankID = 0;
            string bloodbankid = Convert.ToString(Session["BloodBankID"]);
            int.TryParse(bloodbankid, out bloodbankID);
            var currentdate =DateTime.Now.Date;
            if (ModelState.IsValid)
            {
                using (var transaction = DB.Database.BeginTransaction())
                {

                    try
                    {
                        var checkdonor = DB.DonarTables.Where(d => d.CNIC.Trim().Replace("-", "") == collectBloodMV.DonorDetails.CNIC.Trim().Replace("-", "")).FirstOrDefault();
                        if (checkdonor == null)
                        {
                            var user = new UserTable();
                            user.UserName = collectBloodMV.DonorDetails.FullName.Trim();
                            user.Password = "12345";
                            user.EmailAddress = collectBloodMV.DonorDetails.EmailAddress;
                            user.AccountStatusID = 1004;
                            user.UserTypeID = 2;
                            user.Description = "Add By Blood Bank";
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            int maxDonorID = DB.DonarTables.Max(d => d.DonorID);
                            var donar = new DonarTable();

                            donar.FullName = collectBloodMV.DonorDetails.FullName;
                 
                            donar.BloodGroupID = collectBloodMV.BloodGroupID;
                            donar.Location = collectBloodMV.DonorDetails.Location;
                            donar.ContactNo = collectBloodMV.DonorDetails.ContactNo;
                            donar.LastDonationdate = DateTime.Now;
                            donar.CNIC = collectBloodMV.DonorDetails.CNIC;
                            donar.CityID = collectBloodMV.CityID;
                            donar.UserID = user.UserID;
                            DB.DonarTables.Add(donar);
                            DB.SaveChanges();
                            checkdonor = DB.DonarTables.Where(d => d.CNIC.Trim().Replace("-", "") == collectBloodMV.DonorDetails.CNIC.Trim().Replace("-", "")).FirstOrDefault();
                        }

                        var checkbloodgroupstock = DB.BloodBankStockTables.Where(s => s.BloodbankID == bloodbankID && s.BloodGroupID == collectBloodMV.BloodGroupID).FirstOrDefault();
                        if (checkbloodgroupstock == null)
                        {

                            var bloodbankstock = new BloodBankStockTable();
                            bloodbankstock.BloodGroupID = collectBloodMV.BloodGroupID;
                            bloodbankstock.Status = true;
                            bloodbankstock.Quantity = 0;
                            bloodbankstock.Description = "";
                            bloodbankstock.BloodbankID = bloodbankID;
                            DB.BloodBankStockTables.Add(bloodbankstock);
                            DB.SaveChanges();

                            checkbloodgroupstock = DB.BloodBankStockTables.Where(s => s.BloodbankID == bloodbankID && s.BloodGroupID == collectBloodMV.BloodGroupID).FirstOrDefault();

                        }
                        checkbloodgroupstock.Quantity += collectBloodMV.Quantity;
                        DB.Entry(checkbloodgroupstock).State = System.Data.Entity.EntityState.Modified;
                        DB.SaveChanges();

                        var collectblooddetail = new BloodBankStockDetailTable();

                        collectblooddetail.BloodBankStockID = checkbloodgroupstock.BloodBankStockID;
                        collectblooddetail.BloodGroupID = collectBloodMV.BloodGroupID;
                        collectblooddetail.Quantity = collectBloodMV.Quantity;

                        collectblooddetail.DonorID = checkdonor.DonorID;
                        collectblooddetail.DonateDateTime = DateTime.Now;
                        DB.BloodBankStockDetailTables.Add(collectblooddetail);
                        DB.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("BloodBankStock", "BloodBank");
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
                ModelState.AddModelError(string.Empty, "Please provide Donor full  Information!");

            }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", collectBloodMV.CityID);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", collectBloodMV.BloodGroupID);
            return View(collectBloodMV);

        }
    }
}