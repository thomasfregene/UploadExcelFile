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
            return View(ContactDb.GetAllBatches());
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
    }

    
}