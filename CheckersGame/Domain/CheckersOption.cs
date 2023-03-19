namespace Domain;


public class CheckersOption
{

    //pk in database
    
    public int Id { get; set; }
    
    public string? Name { get; set; } = default!;
    public int Width { get; set; }
    public int Height { get; set; }
    public int RandomMoves { get; set; } = 0;
    public bool WhiteStart { get; set; } = true;
        
    //ICollection - no foo[];
    public ICollection<CheckersGame>? CheckerGames { get; set; }

    public override string ToString()
    {
        return $"{Width}x{Height}x Random: {RandomMoves} WhiteStart: {WhiteStart}";
    }
}