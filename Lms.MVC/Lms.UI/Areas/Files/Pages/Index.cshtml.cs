using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

using Lms.MVC.Core.Entities;
using Lms.MVC.Data.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Lms.MVC.UI.Areas.Files
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext db;

        private readonly IFileProvider _fileProvider;

        public IndexModel(ApplicationDbContext context, IFileProvider fileProvider)
        {
            db = context;
            _fileProvider = fileProvider;
        }

        public IList<DbFile> DatabaseFiles { get; private set; }

        public IDirectoryContents PhysicalFiles { get; private set; }

        public async Task OnGetAsync()
        {
            DatabaseFiles = await db.DbFile.AsNoTracking().ToListAsync();
            PhysicalFiles = _fileProvider.GetDirectoryContents(string.Empty);
        }

        public async Task<IActionResult> OnGetDownloadDbAsync(int? id)
        {
            if (id == null)
            {
                return Page();
            }
            var requestFile = await db.DbFile.SingleOrDefaultAsync(m => m.Id == id);
            if (requestFile == null)
            {
                return Page();
            }

            // Don't display the untrusted file name in the UI. HTML-encode the value.
            return File(requestFile.Content, MediaTypeNames.Application.Octet, WebUtility.HtmlEncode(requestFile.UntrustedName));
        }

        public IActionResult OnGetDownloadPhysical(string fileName)
        {
            var downloadFile = _fileProvider.GetFileInfo(fileName);

            return PhysicalFile(downloadFile.PhysicalPath, MediaTypeNames.Application.Octet, fileName);
        }
    }
}