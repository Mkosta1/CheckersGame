using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Db;
using Domain;

namespace WebApp.Pages_CheckersGameStates
{
    public class CreateModel : PageModel
    {
        private readonly DAL.Db.AppDbContext _context;

        public CreateModel(DAL.Db.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CheckersGameId"] = new SelectList(_context.CheckerGame, "Id", "Player1Name");
            return Page();
        }

        [BindProperty]
        public CheckersGameState CheckersGameState { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.CheckersGameStates == null || CheckersGameState == null)
            {
                return Page();
            }

            _context.CheckersGameStates.Add(CheckersGameState);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
