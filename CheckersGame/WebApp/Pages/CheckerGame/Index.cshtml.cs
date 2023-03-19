using DAL;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain;

namespace WebApp.Pages_CheckerGame
{
    public class IndexModel : PageModel
    {
        private readonly IGameRepository _repo;

        public IndexModel(IGameRepository repo)
        {
            _repo = repo;
        }

        public IList<CheckersGame> CheckersGame { get;set; } = default!;

        public Task OnGetAsync()
        {
            CheckersGame = _repo.GetAll();
            return Task.CompletedTask;
        }
    }
}
