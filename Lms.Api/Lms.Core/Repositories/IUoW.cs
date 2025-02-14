﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.API.Core.Repositories
{
    public interface IUoW
    {
        IPublicationRepository PublicationRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        Task CompleteAsync();
    }
}
