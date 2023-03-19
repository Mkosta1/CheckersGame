using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.CheckerGame;

public class LaunchGame : PageModel
{
    
    private readonly IGameRepository _repo;

    public LaunchGame(IGameRepository repo)
    {
        _repo = repo;
    }

    public int GameId { get; set; }
    
    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return RedirectToPage("/CheckerGame/Index", new {error="No id!"});
        }
        var game = _repo.GetGame(id);
        
        if (game == null)
        {
            return RedirectToPage("/CheckerGame/Index", new {error="No game found!"});
        }
        
        //2 player game 
        if (game.Player1Type == EPlayerType.Human && game.Player2Type == EPlayerType.Human)
        {
            GameId = game.Id;
            return Page();
        }

        if (game.Player1Type == EPlayerType.Ai && game.Player2Type == EPlayerType.Human)
        {
            return RedirectToPage("Play", new { id = game.Id, playerNo = 1 });
        }
        //single player game
        //playerNo = 0 is White
        
        return RedirectToPage("Play", new { id = game.Id, playerNo = 0 });
        
    }
}