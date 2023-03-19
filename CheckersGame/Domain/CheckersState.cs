using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Domain;

public class CheckersState
{
    public EGamePiece?[][] GameBoard { get; set; } = default!;
    public bool NextMoveByBlack { get; set; }
}