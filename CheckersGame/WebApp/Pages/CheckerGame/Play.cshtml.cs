using CheckersBrain;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.CheckerGame;

public class Play : PageModel
{
    private readonly IGameRepository _repo;

    public Play(IGameRepository repo)
    {
        _repo = repo;
    }

    public CheckersBrainMain Brain { get; set; } = default!;

    public CheckersGame CheckerGame { get; set; } = default!;

    public EGamePiece?[][] GameBoard { get; set; } = default!;

    public bool StartMove { get; set; } = true;
    public int? XStart { get; set; }
    public int? YStart { get; set; }
    public int? XFinish { get; set; }
    public int? YFinish { get; set; }

    public int PlayerNo { get; set; }


    public Task<IActionResult> OnGet(int? id, int? playerNo, int? xStart, int? yStart, int? xFinish, int? yFinish, bool? checkAi)
    {
        if (id == null)
        {
            return Task.FromResult<IActionResult>(RedirectToPage("/Index", new { error = "No game Id!" }));
        }

        if (playerNo == null || playerNo.Value < 0 || playerNo.Value > 1)

        {
            return Task.FromResult<IActionResult>(RedirectToPage("/Index", new { error = "No player number, or wrong number!" }));
        }


        PlayerNo = playerNo.Value;

        //playerNo 0 - first - player is white
        //playerNo 1 - second - player is black


        var game = _repo.GetGame(id);


        if (game == null || game.CheckersOption == null)
        {
            return Task.FromResult<IActionResult>(NotFound());
        }

        CheckerGame = game;

        Brain = new CheckersBrainMain(game.CheckersOption, game.CheckersGameState?.LastOrDefault());

        XStart = xStart;
        YStart = yStart;
        XFinish = xFinish;
        YFinish = yFinish;

        if (Brain.CheckGameState() == false)
        {
            if (Brain.GetWinner())
            {
                game.GameOverAt = DateTime.Now;
                game.GameWonByPlayer = game.Player1Name;
                _repo.Update(game);
            }
            else
            {
                game.GameOverAt = DateTime.Now;
                game.GameWonByPlayer = game.Player2Name;
                _repo.Update(game);
            }
        }
        
        else if (Brain.State.NextMoveByBlack == false)
        {
            if (checkAi.HasValue && CheckerGame.Player1Type == EPlayerType.Ai)
            {
                var move = Brain.AiMoveChooser();
                GameBoard = Brain.NewWhiteState(
                    move[0], move[1],
                    move[2], move[3]);
                StartMove = true;
                game.CheckersGameState!.Add(new CheckersGameState()
                {
                    SerializedGameState = Brain.GetSerializedGameState(),
                });

                _repo.SaveChanged();
            }

            if (xStart.HasValue && yStart.HasValue)
            {
                StartMove = false;

                if (xFinish.HasValue && yFinish.HasValue)
                {
                    GameBoard = Brain.NewWhiteState(xStart.Value, yStart.Value, xFinish.Value, yFinish.Value);
                    StartMove = true;
                    if (Brain.PreviousState == false)
                    {
                        game.CheckersGameState!.Add(new CheckersGameState()
                        {
                            SerializedGameState = Brain.GetSerializedGameState(),
                        });

                        _repo.SaveChanged();
                    }
                    else
                    {
                        return Task.FromResult<IActionResult>(Page());
                    }
                }
            }
        }
        else if (Brain.State.NextMoveByBlack)
        {
            if (checkAi.HasValue && CheckerGame.Player2Type == EPlayerType.Ai)
            {
                var move = Brain.AiMoveChooser();
                GameBoard = Brain.NewBlackState(
                    move[0], move[1],
                    move[2], move[3]);

                StartMove = true;
                game.CheckersGameState!.Add(new CheckersGameState()
                {
                    SerializedGameState = Brain.GetSerializedGameState(),
                });

                _repo.SaveChanged();
            }

            XStart = xStart;
            YStart = yStart;
            XFinish = xFinish;
            YFinish = yFinish;
            if (xStart.HasValue && yStart.HasValue)
            {
                StartMove = false;
                if (xFinish.HasValue && yFinish.HasValue)
                {
                    GameBoard = Brain.NewBlackState(xStart.Value, yStart.Value, xFinish.Value, yFinish.Value);
                    StartMove = true;
                    if (Brain.PreviousState == false)
                    {
                        game.CheckersGameState!.Add(new CheckersGameState()
                        {
                            SerializedGameState = Brain.GetSerializedGameState(),
                        });

                        _repo.SaveChanged();
                    }
                    else
                    {
                        return Task.FromResult<IActionResult>(Page());
                    }
                }
            }
        }

        return Task.FromResult<IActionResult>(Page());
    }
}