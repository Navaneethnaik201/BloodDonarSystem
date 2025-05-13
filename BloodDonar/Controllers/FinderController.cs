using BloodDonar.Models;
using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace BloodDonar.Controllers
{
    public class FinderController : Controller
    {
        BloodBankDBEntities DB = new BloodBankDBEntities();
        // GET: Finder
        public ActionResult FinderDonors()
        {
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", "0");
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", "0");
            return View(new FinderMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult FinderDonors(FinderMV finderMV)
        {
            int userid = 0;

            int.TryParse(Convert.ToString(Session["UserID"]), out userid);


            var setdate = DateTime.Now.AddDays(-120);
            var donors = DB.DonarTables.Where(d => d.BloodGroupID == finderMV.BloodGroupID && d.CityID == finderMV.CityID && d.LastDonationdate > setdate).ToList();
            foreach (var donor in donors)
            {
                var user = DB.UserTables.Find(donor.UserID);
                if (userid != user.UserID)
                {


                    if (user.AccountStatusID == 1004)
                    {
                        var adddonor = new FinderSearchResultMV();
                        adddonor.UserID = user.UserID;
                        adddonor.BloodGroup = donor.BloodGroupsTable.BloodGroup;
                        adddonor.BloodGroupID = donor.BloodGroupID;
                        adddonor.ContactNo = donor.ContactNo;
                        adddonor.DonorID = donor.DonorID;
                        adddonor.FullName = donor.FullName;
                        adddonor.Address = donor.Location;
                        adddonor.UserType = "Person";
                        adddonor.UserTypeID = user.UserTypeID;
                        finderMV.SearchResult.Add(adddonor);


                    }

                }
            }

                var bloodbanks = DB.BloodBankStockTables.Where(d => d.BloodGroupID == finderMV.BloodGroupID && d.Quantity > 0).ToList();
                foreach (var bloodbank in bloodbanks)
                {
                    var getbloodbank = DB.BloodBankTables.Find(bloodbank.BloodbankID);
                    var user = DB.UserTables.Find(getbloodbank.UserID);
                if (userid != user.UserID)
                {
                    if (user.AccountStatusID == 1004)
                    {


                        var adddonor = new FinderSearchResultMV();
                        adddonor.UserID = user.UserID;

                            

                        adddonor.BloodGroup = bloodbank.BloodGroupsTable.BloodGroup;
                        adddonor.BloodGroupID = bloodbank.BloodGroupID;
                        adddonor.ContactNo = bloodbank.BloodBankTable.PhoneNo;
                        adddonor.Address = bloodbank.BloodBankTable.Address;
                        adddonor.DonorID = bloodbank.BloodbankID;
                        adddonor.FullName = bloodbank.BloodBankTable.BloodBankName;
                        adddonor.UserType = "BloodBank";
                        adddonor.UserTypeID = user.UserTypeID;

                        finderMV.SearchResult.Add(adddonor);
                    }
                }


                }
                ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupID", "BloodGroup", finderMV.BloodGroupID);
                ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "city", finderMV.CityID);
                return View(finderMV);
        }

        public ActionResult RequestForBlood(int? donorid, int usertypeid, int bloodgroupid)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("/MainHome", "Home");
            }


            var request = new RequestMV();
            request.AcceptedID = (int)donorid;

            if (usertypeid == 2)
            {
                request.AcceptedTypeID = 1;
            }
            else if (usertypeid == 5)
            {
                request.AcceptedTypeID = 2;
            }
            request.RequiredBloodGroup = (int)bloodgroupid;


            return View();


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestForBlood(RequestMV requestMV)
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {

                return RedirectToAction("/MainHome", "Home");

            }

            int UserTypeID = 0;
            int RequestTypeID = 0;
            int RequestByID = 0;

            int.TryParse(Convert.ToString(Session["UserTypeID"]),out UserTypeID);

            if (UserTypeID == 2) //donor
            { 
                    int.TryParse(Convert.ToString(Session["DonorID"]),out RequestByID);
            }
            else if (UserTypeID == 4) // hospital
            {
                RequestTypeID = 2002;
                int.TryParse(Convert.ToString(Session["HospitalID"]), out RequestByID);

            }
            else if (UserTypeID == 5)  //bloodbank
            {
                RequestTypeID = 2003;
                int.TryParse(Convert.ToString(Session["BloodbankID"]), out RequestByID);
             
            }

            if (ModelState.IsValid)
            {

                var request = new RequestTable();
                request.RequestDate = DateTime.Now;
                request.AcceptedTypeID = requestMV.AcceptedTypeID;
                request.AcceptedID = requestMV.AcceptedID;
                request.RequiredBloodGroup = requestMV.RequiredBloodGroup;
                request.ExpectedDate = requestMV.ExpectedDate;
                request.RequestDetails = requestMV.RequestDetails;
                request.RequestStatusID = 1;
                request.RequestByID = RequestByID;
                request.RequestTypeID = RequestTypeID;
                DB.RequestTables.Add(request);
                DB.SaveChanges();
                return RedirectToAction("/MainHome", "Home");
            }
            
        
            
       
            return View(requestMV);


        }
        public ActionResult ShowAllRequests()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }

            int UserTypeID = 0;
            int RequestTypeID = 0;
            int RequestByID = 0;

            int.TryParse(Convert.ToString(Session["UserTypeID"]), out UserTypeID);

            if (UserTypeID == 2) //donor
            {
                int.TryParse(Convert.ToString(Session["DonorID"]), out RequestByID);
            }
            else if (UserTypeID == 4) // hospital
            {
                RequestTypeID = 2002;
                int.TryParse(Convert.ToString(Session["HospitalID"]), out RequestByID);

            }
            else if (UserTypeID == 5)  //bloodbank
            {
                RequestTypeID = 2003;
                int.TryParse(Convert.ToString(Session["BloodbankID"]), out RequestByID);

            }
            var requests = DB.RequestTables.Where(r => r.RequestByID == RequestByID && r.RequestTypeID == RequestTypeID).ToList();
          var list = new List<RequestListMV>();
          foreach (var request in requests)
          {
              var addrequest = new RequestListMV();
  
              addrequest.RequestID = request.RequestID;
              addrequest.RequestDate = request.RequestDate;
              addrequest.RequestByID = request.RequestByID;
              addrequest.AcceptedID = request.AcceptedID;

             if (request.AcceptedTypeID == 1)
             {
                    var getdonor = DB.DonarTables.Find(request.AcceptedID);
                 addrequest.AcceptedFullName = getdonor.FullName;
                 addrequest.ContactNo = getdonor.ContactNo;
                 addrequest.Address = getdonor.Location;
             }else if (request.AcceptedTypeID == 2)
             { 
                var getbloodbank = DB.BloodBankTables.Find(request.AcceptedID);
                 addrequest.AcceptedFullName = getbloodbank.BloodBankName;
                 addrequest.ContactNo = getbloodbank.PhoneNo;
                 addrequest.Address = getbloodbank.Address;


             }
              addrequest.AcceptedTypeID = request.AcceptedTypeID;
              addrequest.AcceptedType = request.AcceptedTypeTable.AcceptedType;

              addrequest.RequiredBloodGroup = request.RequiredBloodGroup;
              var bloodgroup = DB.BloodGroupsTables.Find(addrequest.RequiredBloodGroup);
              addrequest.BloodGroup = bloodgroup.BloodGroup;
              addrequest.RequestTypeID = request.RequestID;
              addrequest.RequestType = request.RequestTypeTable.RequestType;
              addrequest.RequestStatus = request.RequestStatusTable.RequestStatus;
              addrequest.RequestStatusID = request.RequestStatusID;
              addrequest.ExpectedDate = request.ExpectedDate;
              addrequest.RequestDetails = request.RequestDetails;
              list.Add(addrequest);
          }
            return View(list);
        }
        public ActionResult CancelRequest(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            var request  = DB.RequestTables.Find(id);
            request.RequestStatusID = 4;
            DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("ShowAllRequests");


        }

        public ActionResult AcceptRequest(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            var request = DB.RequestTables.Find(id);
            request.RequestStatusID = 2;
            DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("ShowAllRequests");


        }

        public ActionResult DonorRequests()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }

            int UserTypeID = 0;
            int AcceptedTypeID = 0;
            int AcceptedByID = 0;

            int.TryParse(Convert.ToString(Session["UserTypeID"]), out UserTypeID);

            if (UserTypeID == 2) //donor
            {
                AcceptedTypeID = 1;
                int.TryParse(Convert.ToString(Session["DonorID"]), out AcceptedByID);
            }
            else if (UserTypeID == 4) // hospital
            {
                int.TryParse(Convert.ToString(Session["HospitalID"]), out AcceptedByID );

            }
            else if (UserTypeID == 5)  //bloodbank
            {
                AcceptedTypeID = 2;
                int.TryParse(Convert.ToString(Session["BloodbankID"]), out AcceptedByID);

            }
            var requests = DB.RequestTables.Where(r => r.AcceptedID == AcceptedByID && r.AcceptedTypeID == AcceptedTypeID).ToList();
            var list = new List<RequestListMV>();
            foreach (var request in requests)
            {
                var addrequest = new RequestListMV();

                addrequest.RequestID = request.RequestID;
                addrequest.RequestDate = request.RequestDate;
                addrequest.RequestByID = request.RequestByID;
                addrequest.AcceptedID = request.AcceptedID;

                if (request.RequestTypeID == 2002)
                {
                    var getdonor = DB.HospitalTables.Find(request.RequestByID);
                    addrequest.AcceptedFullName = getdonor.FullName;
                    addrequest.ContactNo = getdonor.PhoneNo;
                    addrequest.Address = getdonor.Address;
                }
                else if (request.RequestTypeID == 2003)
                {
                    var getbloodbank = DB.BloodBankTables.Find(request.RequestByID);
                    addrequest.AcceptedFullName = getbloodbank.BloodBankName;
                    addrequest.ContactNo = getbloodbank.PhoneNo;
                    addrequest.Address = getbloodbank.Address;


                }
                addrequest.AcceptedTypeID = request.AcceptedTypeID;
                addrequest.AcceptedType = request.AcceptedTypeTable.AcceptedType;

                addrequest.RequiredBloodGroup = request.RequiredBloodGroup;
                var bloodgroup = DB.BloodGroupsTables.Find(addrequest.RequiredBloodGroup);
                addrequest.BloodGroup = bloodgroup.BloodGroup;
                addrequest.RequestTypeID = request.RequestID;
                addrequest.RequestType = request.RequestTypeTable.RequestType;
                addrequest.RequestStatus = request.RequestStatusTable.RequestStatus;
                addrequest.RequestStatusID = request.RequestStatusID;
                addrequest.ExpectedDate = request.ExpectedDate;
                addrequest.RequestDetails = request.RequestDetails;
                list.Add(addrequest);
            }
            return View(list);
        }

        public ActionResult CompleteResult(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            var request = DB.RequestTables.Find(id);
           if(request.AcceptedTypeID == 1)
            {
                var donor = DB.DonarTables.Find(request.AcceptedTypeID);
                donor.LastDonationdate = DateTime.Now;
                DB.Entry(donor).State = System.Data.Entity.EntityState.Modified;

            }
            request.RequestStatusID = 3;
            DB.Entry(request).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();

            return RedirectToAction("ShowAllRequests");
        }
    }
}