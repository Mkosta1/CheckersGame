using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Db;
using Domain;

namespace WebApp.Pages_CheckersGameStates
{
    public class IndexModel : PageModel
    {
        private readonly DAL.Db.AppDbContext _context;

        public IndexModel(DAL.Db.AppDbContext context)
        {
            _context = context;
        }

        public IList<CheckersGameState> CheckersGameState { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.CheckersGameStates != null)
            {
                CheckersGameState = await _context.CheckersGameStates
                .Include(c => c.CheckersGame).ToListAsync();
            }
        }
    }
}
