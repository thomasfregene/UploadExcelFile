using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.SessionState;
using UploadExcelFile.Models;

namespace UploadExcelFile.Controllers
{
    [SessionState(SessionStateBehavior.Required)]
    public class ContactsController : Controller
    {
        // GET: Contacts
        [HttpGet]
        public ActionResult Index()
        {
            return View(new List<ContactVM>());
        }

        [HttpPost]
        [WebMethod(EnableSession = true)]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            List<ContactVM> contact = new List<ContactVM>();
            string filePath = string.Empty;
            if (postedFile != null)
            {
                try
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    //Read the contents of CSV file.
                    string csvData = System.IO.File.ReadAllText(filePath);

                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            ContactVM contactVM = new ContactVM()
                            {
                                FirstName = row.Split(',')[0],
                                LastName = row.Split(',')[1],
                                Email = row.Split(',')[2],
                                Telephone = row.Split(',')[3],
                                Mobile = row.Split(',')[4],
                                CompanyID = Convert.ToInt32(row.Split(',')[5]),
                            };

                            //Checking then first Name field
                            if (contactVM.FirstName == string.Empty)
                            {
                                contactVM.Status = "Inavlid";
                                contactVM.Message = "First Name Field is required ";
                            }



                            //checking the Last Name field
                            if (contactVM.LastName == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Message = "Last Name Field is required ";
                            }



                            //checking for email field
                            if (contactVM.Email == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Message = "Email Field is required ";
                            }

                            //checking for Telephone
                            if (contactVM.Telephone == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Message = "Telephone Field is required ";
                            }

                            //checking for Mobile
                            if (contactVM.Mobile == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Message = "Mobile Field is required ";
                            }


                            //checking for Valid Company Id
                            //needs improvement
                            if (contactVM.CompanyID != 7 && contactVM.CompanyID == null /*|| contactVM.CompanyID > 0*/)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Message = "CompanyID Field Must be same for All Entries";
                            }


                            contact.Add(contactVM);
                        }

                    }


                }
                catch (Exception ex)
                {
                    //throw;
                    TempData["Message"] = "No File Chosen";
                    TempData["Message"] = "Something went wrong " + ex.Message;
                }
            }

            Session.Clear();
            Session["Upload"] = contact;
            return View(contact);
        }

        private string getBatchName()
        {
            string batchName;
            var rand = new Random();
            var newRand = rand.Next(100);
            ContactBatch batch = new ContactBatch();
            batchName = batch.BatchName = "Upload - " + newRand;

            return batchName;
        }

        [HttpGet]
        [WebMethod(EnableSession = true)]
        public ActionResult CreateContact()
        {
            //DateTime myDateTime = DateTime.Now;
            //string sqlformattedDate = myDateTime.ToString("yyyy-MM-dd hh:mm:ss.fff");

            var batchName = getBatchName();

            ContactBatch batch = new ContactBatch
            {
                BatchName = batchName,
                CreatedBy = "System",
                DateCreated = DateTime.Now
                //DateCreated = Convert.ToDateTime(sqlformattedDate)
            };
            int batchId = ContactBatchDB.GetBatchID(batch);
            List<ContactVM> contacts = new List<ContactVM>();
            contacts = (List<ContactVM>)Session["Upload"];
            ContactDb.PostToDatabase(contacts, batchId);
            return View(contacts);
        }


    }
}