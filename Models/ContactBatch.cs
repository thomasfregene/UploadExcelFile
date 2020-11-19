using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadExcelFile.Models
{
    public class ContactBatch
    {
        public int BatchID { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string Status { get; set; }
        public string BatchName { get; set; }
    }
}