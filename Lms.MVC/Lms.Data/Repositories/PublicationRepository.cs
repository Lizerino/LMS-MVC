using Lms.API.Core.Entities;
using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.MVC.Data.Repositories
{
    public class PublicationRepository:IPublicationRepository
    {
        public Author CreateAuthor(string firstName, string lastName, DateTime birthday)
        {
            var author = new Author()
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthday
            };
            return author;
        }

        public Subject CreateSubject(string subjectTitle)
        {
            var subject = new Subject()
            {
                Title = subjectTitle
            };
            return subject;
        }

        public IEnumerable<Subject> GetSubjects()
        {
            var subjects = new List<Subject>()
            {
                new Subject() { Title = "Bil"},
                new Subject() { Title = "Reference"},
                new Subject() { Title = "Textbook"},
                new Subject() { Title = "Byxor"},
                new Subject() { Title = "Hat"}
            };

            return subjects;
        }
    }
}
