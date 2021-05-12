using System;
using System.ComponentModel.DataAnnotations;

namespace Lms.MVC.Core.Entities
{
    public class DbFile
    {
        public int Id { get; set; }

        public byte[] Content { get; set; }

        
        public string UntrustedName { get; set; }

       
        public string Note { get; set; }

       
        public long Size { get; set; }

        
        public DateTime UploadDT { get; set; }
    }
}
