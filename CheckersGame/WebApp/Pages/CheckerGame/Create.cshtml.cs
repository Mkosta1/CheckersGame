using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Db;
using Domain;

namespace WebApp.Pages_CheckerGame
{
    public class CreateModel : PageModel
    {
        private readonly DAL.Db.AppDbContext _context;
        private readonly IGameRepository _repo;

        public CreateModel(DAL.Db.AppDbContext context, IGameRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        public IActionResult OnGet()
        {
            OptionsSelectList = new SelectList(_context.CheckersOptions, "Id", "Name");
            return Page();
        }

        [BindProperty] public CheckersGame CheckersGame { get; set; } = default!;

        public SelectList OptionsSelectList { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.CheckerGame == null || CheckersGame == null)
            {
                return Task.FromResult<IActionResult>(Page());
            }

            _repo.AddGame(CheckersGame);

            return Task.FromResult<IActionResult>(RedirectToPage("./LaunchGame", new {id = CheckersGame.Id}));
        }
    }
}