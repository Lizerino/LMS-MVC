using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Core.Entities
{
    public class User 
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }


        // nav prop

        public ICollection<Document> Documents { get; set; }

    }
}
