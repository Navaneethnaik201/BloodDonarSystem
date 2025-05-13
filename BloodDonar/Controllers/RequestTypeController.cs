﻿using BloodDonar.Models;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BloodDonar.Controllers
{
    public class RequestTypeController : Controller
    {
        BloodBankDBEntities DB = new BloodBankDBEntities();

        public ActionResult AllRequestType()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            var requesttypes = DB.RequestTypeTables.ToList();
            var listrequesttypes = new List<RequestTypeMV>();
            foreach (var requesttype in requesttypes)
            {
                var addrequesttype  = new RequestTypeMV();  
                addrequesttype.RequestTypeID = requesttype.RequestTypeID;   
                addrequesttype.RequestType   =  requesttype.RequestType;
                listrequesttypes.Add(addrequesttype);




            }
            return View(listrequesttypes);
        }

        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            var requestType = new RequestTypeMV();
            return View(requestType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequestTypeMV requestTypeMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            if (ModelState.IsValid)
            {
                var requestTypeTable = new RequestTypeTable();
                requestTypeTable.RequestTypeID = requestTypeMV.RequestTypeID;
                requestTypeTable.RequestType = requestTypeMV.RequestType;
                DB.RequestTypeTables.Add(requestTypeTable);
                DB.SaveChanges();
                return RedirectToAction("AllRequestType");
            }
            return View(requestTypeMV);
        }

        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }


            var requesttype = DB.RequestTypeTables.Find(id);
            if (requesttype == null)
            {

                return HttpNotFound();
            }
            var requesttypemv = new RequestTypeMV();
            requesttypemv.RequestTypeID = requesttype.RequestTypeID;    
            requesttypemv.RequestType = requesttype.RequestType;


            return View(requesttypemv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RequestTypeMV requestTypeMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            if (ModelState.IsValid)
            {
                var requestTypeTable = new RequestTypeTable();
                requestTypeTable.RequestTypeID = requestTypeMV.RequestTypeID;
                requestTypeTable.RequestType = requestTypeMV.RequestType;

                DB.Entry(requestTypeTable).State = EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("AllRequestType");
            }
            return View(requestTypeMV);
        }

        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var requesttype = DB.RequestTypeTables.Find(id);
            if (requesttype == null)
            {
                return HttpNotFound();
            }

            var requesttypemv = new RequestTypeMV();
            requesttypemv.RequestTypeID = requesttype.RequestTypeID;
            requesttypemv.RequestType = requesttype.RequestType;


            return View(requesttypemv);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var requesttype = DB.RequestTypeTables.Find(id);
            if (requesttype == null)
            {
                return HttpNotFound();
            }

            DB.RequestTypeTables.Remove(requesttype);
            DB.SaveChanges();
            return RedirectToAction("AllRequestType");
        }
    }
}