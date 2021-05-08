using Lms.API.Core.Entities;
using Lms.MVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Core.Repositories
{
    public interface IPublicationRepository
    {
        public Author CreateAuthor(string firstName, string lastName);
        public Subject CreateSubject(string subjectTitle);
        public IEnumerable<Subject> GetSubjects();

    }
}
