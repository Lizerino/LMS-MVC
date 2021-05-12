using System.Threading.Tasks;

using Lms.MVC.Core.Entities;
using Lms.MVC.Data.Data;
using Lms.MVC.UI.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace Lms.MVC.UI.Areas.Files
{
    public class DeleteDbFileModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public DeleteDbFileModel(ApplicationDbContext context)
        {
            db = context;
        }

        [BindProperty]
        public DbFile RemoveFile { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Index");
            }

           
           RemoveFile = await db.DbFile.SingleOrDefaultAsync(m => m.Id == id);

            if (RemoveFile == null)
            {
                return RedirectToPage("/index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Index");
            }

            
           RemoveFile = await db.DbFile.FindAsync(id);

            if (RemoveFile != null)
            {
                db.DbFile.Remove(RemoveFile);
                await db.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}
