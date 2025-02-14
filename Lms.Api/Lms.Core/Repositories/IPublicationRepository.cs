﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lms.API.Core.Entities;

namespace Lms.API.Core.Repositories
{
    public interface IPublicationRepository
    {
        Task<IEnumerable<Publication>> GetAllPublicationsAsync();
        Task<Publication> GetPublicationAsync(int? id);
        Task<IEnumerable<Publication>> GetPublicationByTitleAsync(string title);
        Task<IEnumerable<Publication>> GetPublicationBySearchAsync(string search);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
        void Remove(Publication removed);
    }
}
