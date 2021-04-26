using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Core.Entites
{
    class User 
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }


        // nav prop

        public ICollection<Document> Documents { get; set; }

    }
}
