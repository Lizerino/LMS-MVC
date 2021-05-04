using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Core.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        public ICollection<AuthorLiterature> Bibliography { get; set; }
    }
}
