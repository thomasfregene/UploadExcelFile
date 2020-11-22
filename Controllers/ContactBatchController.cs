using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadExcelFile.Models;
using System.Data;
using System.Reflection;

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
            return View(ContactDb.GetContactsByBatchId(id));
        }

        public ActionResult DeleteById(int id)
        {
            List<ContactVM> contactVM = new List<ContactVM>();
            contactVM = ContactBatchDB.DeleteContactByBatchId(id);
            return View(contactVM);
        }

        [HttpGet]
        public ActionResult DeletedBatchById(int id)
        {
            ContactBatchDB.DeleteFileByBatchId(id);
            return View();
        }
    }

    
}