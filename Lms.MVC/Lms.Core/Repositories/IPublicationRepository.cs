using System;
using System.Collections.Generic;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.Core.Repositories
{
    public interface IPublicationRepository
    {
        public Author CreateAuthor(string firstName, string lastName, DateTime dateOfBirth);

        public Subject CreateSubject(string subjectTitle);

        public IEnumerable<Subject> GetSubjects();
    }
}