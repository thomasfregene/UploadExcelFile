using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadExcelFile.Models;
using System.Data;
using System.Reflection;
using System.Web.Services;
using System.IO;

namespace UploadExcelFile.Controllers
{

    public class ContactBatchController : Controller
    {
        // GET: ContactBatch
        [HttpGet]
        public ActionResult Index()
        {
            return View(ContactBatchDB.GetAllBatches());
        }

        [HttpGet]
        public ActionResult GetFileById(int id)
        {
            return View(ContactDb.GetContactsByBatchId(id));
        }

        [HttpGet]
        public ActionResult EditById(int id)
        {
            List<ContactVM> contactVM = new List<ContactVM>();
            contactVM = ContactBatchDB.EditContactsByBatchId(id);
            Session["BatchId"] = ContactBatchDB.EditContactsByBatchId(id);
            return View(contactVM);
            //return View(ContactDb.GetContactsByBatchId(id));
        }

        //Edit
        [HttpGet]
        public ActionResult ReUploadBatch()
        {
            return View(new List<ContactVM>());
        }

        //Edit
        [HttpPost]
        [WebMethod(EnableSession = true)]
        public ActionResult ReUploadBatch(HttpPostedFileBase postedFile)
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
                            ContactVM contactVM = new ContactVM();

                            contactVM.FirstName = row.Split(',')[0];
                            contactVM.LastName = row.Split(',')[1];
                            contactVM.Email = row.Split(',')[2];
                            contactVM.Telephone = row.Split(',')[3];
                            contactVM.Mobile = row.Split(',')[4];
                            contactVM.CompanyID = Convert.ToInt32(row.Split(',')[5]);

                            //Checking then first Name field
                            if (contactVM.FirstName == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Message = "First Name field is required";
                            }

                            //checking the Last Name field
                            if (contactVM.LastName == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Message = "Last Name field is required";
                            }

                            //checking for email field
                            if (contactVM.Email == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Message = "Email field is required";
                            }

                            //checking for Mobile
                            if (contactVM.Mobile == string.Empty)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Message = "Mobile field is required";
                            }

                            //checking for Valid Company Id
                            if (contactVM.CompanyID != 7)
                            {
                                contactVM.Status = "Invalid";
                                contactVM.Message = "Invalid Company iD";
                            }

                            contact.Add(contactVM);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "No File Chosen";
                    TempData["Message"] = "Something went wrong " + ex.Message;
                }
            }
            //Seession object
            Session["ReUploadBatch"] = contact;

            return View(contact);
        }

        //Edit
        [HttpGet]
        [WebMethod(EnableSession = true)]
        public ActionResult UpdateByBatchId(ContactVM contactVM)
        {
            int batchID = 0;
            List<ContactVM> batchId = new List<ContactVM>();
            batchId = (List<ContactVM>)Session["BatchId"];
            foreach (ContactVM contactsBatch in batchId)
            {
                batchID = contactsBatch.BatchID;
            }

            //Session object
            List<ContactVM> contacts = new List<ContactVM>();
            contacts = (List<ContactVM>)Session["ReUploadBatch"];

            ContactBatchDB.UpdateContactByBatchId(contacts, batchID);

            return View();
        }

        //Delete
        [HttpGet]
        public ActionResult DeleteById(int id)
        {
            List<ContactVM> contactVM = new List<ContactVM>();
            contactVM = ContactBatchDB.DeleteContactByBatchId(id);
            return View(contactVM);
        }

        //[HttpGet]
        //public ActionResult DeletedBatchById()
        //{
        //    return View();
        //}

        [HttpGet]
        public ActionResult DeletedBatchById(int id)
        {
            ContactBatchDB.DeleteFileByBatchId(id);
            return View();
        }

        //[HttpGet]
        //public
    }

    
}