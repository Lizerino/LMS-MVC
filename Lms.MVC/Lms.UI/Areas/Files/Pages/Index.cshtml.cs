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
        // todo: unit of work
        private readonly ApplicationDbContext db;       

        public IndexModel(ApplicationDbContext context)
        {
            db = context;           
        }

        public IList<ApplicationFile> DatabaseFiles { get; private set; }        

        public async Task OnGetAsync()
        {
            DatabaseFiles = await db.DbFile.AsNoTracking().ToListAsync();            
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
    }
}