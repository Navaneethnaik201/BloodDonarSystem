using BloodDonar.Models;
using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Documents;

namespace BloodDonar.Controllers
{
    public class BloodBankController : Controller
    {
        BloodBankDBEntities DB = new BloodBankDBEntities();
        // GET: BloodBank
        public ActionResult BloodBankStock()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");


            }

            int bloodbankID = 0;
            string bloodbankid = Convert.ToString(Session["BloodBankID"]);
            int.TryParse(bloodbankid, out bloodbankID);
            

            
            var list = new List<BloodBankStockMV>();
           
            var stocklist = DB.BloodBankStockTables.Where(b => b.BloodbankID == bloodbankID);
            foreach(var stock in stocklist)
            {
                string bloodbank = stock.BloodBankTable.BloodBankName;
                string bloodgroup = stock.BloodGroupsTable.BloodGroup;
                var bloodBankStockmv = new BloodBankStockMV();

                bloodBankStockmv.BloodBankStockID = stock.BloodBankStockID;
                bloodBankStockmv.BloodGroupID = stock.BloodGroupID;


                bloodBankStockmv.BloodBank = bloodbank;

                bloodBankStockmv.BloodGrouup = bloodgroup;
                bloodBankStockmv.Status = stock.Status==true ? "Ready for use" : "Not Ready";

                bloodBankStockmv.Quantity = stock.Quantity;
                bloodBankStockmv.Description = stock.Description;
                bloodBankStockmv.BloodbankID = stock.BloodbankID;
                list.Add(bloodBankStockmv);
            }
            return View(list);
        }
    }
}