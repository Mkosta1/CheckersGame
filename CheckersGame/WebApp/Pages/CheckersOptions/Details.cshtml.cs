using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Db;
using Domain;

namespace WebApp.Pages_CheckersOptions
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.Db.AppDbContext _context;

        public DetailsModel(DAL.Db.AppDbContext context)
        {
            _context = context;
        }

      public CheckersOption CheckersOption { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CheckersOptions == null)
            {
                return NotFound();
            }

            var checkersoption = await _context.CheckersOptions.FirstOrDefaultAsync(m => m.Id == id);
            if (checkersoption == null)
            {
                return NotFound();
            }
            else 
            {
                CheckersOption = checkersoption;
            }
            return Page();
        }
    }
}
