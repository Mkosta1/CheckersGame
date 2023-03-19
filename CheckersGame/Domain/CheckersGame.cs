using System.ComponentModel.DataAnnotations;

namespace Domain;

public class CheckersGame
{
    public int Id { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.Now;
    public DateTime? GameOverAt{ get; set; }
    public string? GameWonByPlayer { get; set; }
    
    
    [MaxLength(128)] 
    public string Player1Name { get; set; } = default!;
    public EPlayerType Player1Type { get; set; }

    [MaxLength(128)] 
    public string Player2Name { get; set; } = default!;
    public EPlayerType Player2Type { get; set; }

    public int CheckersOptionId { get; set; }
    public CheckersOption? CheckersOption { get; set; }

    public ICollection<CheckersGameState>? CheckersGameState { get; set; }
}