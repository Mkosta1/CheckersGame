using Domain;
using static System.Console;

namespace CheckersBrain;

public static class Ui

{
    public static void DrawGameBoard(EGamePiece?[][] board)
    {   
        
        var cols = board.GetLength(0);
        var rows = board[0].GetLength(0);
        //rows ylevalt alla, cols vasakult paremale
        var n = 0;
        for (int i = 0; i < rows; i++)
        {
            var m = 0;
            for (int j = 0; j < cols; j++)
            {
                if (i == 0)
                {
                    if (j == 0)
                    {
                        Console.Write(" +-");
                    }
                    else
                    {
                        Console.Write("+-");
                    }
                    Console.Write(m + "-");
                    m++;
                }
                else
                {
                    if (j == 0)
                    {
                        Console.Write(" +---");
                    }
                    else
                    {
                        Console.Write("+---");
                    }
                }
            }
            Console.WriteLine("+");
            
            Console.Write(n);
            n++;

            for (int j = 0; j < cols; j++)
            {
                Console.Write("| ");
                var pieceStr1 = board[j][i] == null ? " " : board[j][i] == EGamePiece.Black ? "X" : "O";
                if (board[j][i] == EGamePiece.BlackQueen)
                {
                    pieceStr1 = "K";
                }
                if (board[j][i] == EGamePiece.WhiteQueen)
                {
                    pieceStr1 = "Q";
                }
                Console.Write(pieceStr1);
                Console.Write(" ");
            }
            
            Console.WriteLine("|");
            
        }

        for (int j = 0; j < cols; j++)
        { 
            if (j == 0)
            {
                Console.Write(" +---");
            }
            else
            {
                Console.Write("+---"); 
            }
        }
    
        Console.WriteLine("+");
    
    
    }
}