using CheckersBrain;
using CheckersGame2;
using ConsoleApp;
using DAL;
using DAL.Db;
using DAL.FileSystem;
using Domain;
using Microsoft.EntityFrameworkCore;

var gameOptions = new CheckersOption();


var dbOptions =
    new DbContextOptionsBuilder<AppDbContext>()
        .UseLoggerFactory(Helpers.MyLoggerFactory)
        .UseSqlite("Data Source=C:/Users/marku/RiderProjects/icd0008-2022f/CheckersGame/identifier.sqlite")
        .Options;

var ctx = new AppDbContext(dbOptions);

// apply any non-applied migrations
//ctx.Database.EnsureDeleted();
ctx.Database.Migrate();
// seed some data to db - if and when needed


IGameRepository repositorDb = new GameRepositoryDb(ctx);
IGameOptionsRepository repoFs = new GameOptionsRepositoryFileSystem();
IGameOptionsRepository repoDb = new GameOptionsRepositoryDb(ctx);
    
IGameOptionsRepository repo = repoDb;


Start();



void Start()
{
    Console.Title = ("Checkers - The game");
    RunMainMenu();

}

void RunMainMenu()
{   
    Console.WriteLine("Persistence engine: " + repo.Name);
    string prompt = "Main Menu";
    string[] options = { "Play", "Options", "Exit" };
    Menu mainMenu = new Menu(prompt, options);
    int selectedIndex = mainMenu.Run();
    switch (selectedIndex)
    {
        case 0:
            NewGame();
            break;
        case 1:
            DisplayOptionsInfo();
            break;
        case 2:
            ExitGame();
            break;
    }
}

void ExitGame()
{
    // WriteLine("\nPress any key to exit...");
    // ReadKey(true);
    Environment.Exit(0);
}

void DisplayOptionsInfo()
{
    string prompt = "Options Menu" +
                    "nothing here :)";
    string[] options =
    {
        "Delete", 
        "Back"
    };
    Menu displayOptions = new Menu(prompt, options);
    int selectedIndex = displayOptions.Run();
    switch (selectedIndex)
    {
        
        case 0:
            DeleteGame();
            break;
        case 1:
            RunMainMenu();
            break;
    }
        
        
}

void NewGame()
{
    string prompt = "New Game";
    string[] options = { "Select save", "List on saved game options", "Custom game", "Back" };
    Menu newGame = new Menu(prompt, options);
    int selectedIndex = newGame.Run();
    switch (selectedIndex)
    {
        case 0:
            LoadGameOptions();
            // LoadedGame();
            break;
        case 1:
            ListGameOptions();
            break;
        case 2:
            CustomGame(null);
            break;
        case 3:
            RunMainMenu();
            break;
    }
}


// void LoadedGame()
// {
//     Console.Clear();
//     CustomGame(null);
//     Console.WriteLine("\nPress any key to return Main menu...");
//     Console.ReadKey(true);
//     RunMainMenu();
// }

// void ChangeSave()
// {
//
//     Console.Clear();
//     if (repo == repoDb)
//     {
//         repo = repoFs;
//     }
//     else
//     {
//         repo = repoDb;
//     }
//     Console.WriteLine("Persistence engine: " + repo.Name);
//     Console.WriteLine("\nPress any key to return back...");
//     Console.ReadKey(true);
//     NewGame();
//             
//
//             
// }


// void ContinueGame()
// {
//     Console.WriteLine("\nPress any key to return Main menu...");
//     Console.ReadKey(true);
//     RunMainMenu();
// }

//Save current board
int sxaxis;
int syaxis;
int fxaxis;
int fyaxis;

void GameInput()
{
    Console.Write("Select x-axis: ");
    sxaxis = Convert.ToInt32(Console.ReadLine());
    Console.Write("Select y-axis: ");
    syaxis = Convert.ToInt32(Console.ReadLine());
    Console.Write("Select x-axis where to: ");
    fxaxis = Convert.ToInt32(Console.ReadLine());
    Console.Write("Select y-axis where to: ");
    fyaxis = Convert.ToInt32(Console.ReadLine());
}

void ReturnToMenu()
{
    Console.WriteLine("Press B to return back to menu");
    Console.WriteLine("##############################");
    Console.WriteLine("  Press any key to continue");
    ConsoleKey keyPressed;
    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
    keyPressed = keyInfo.Key;
    
    //Update selected index based on arrow keys.
    if (keyPressed == ConsoleKey.B)
    {
        
        RunMainMenu();
    }
}


void CustomGame(int? id)
{ 
    Console.Clear();
    CheckersBrainMain game;
    CheckersGame? gameFinal;
    
    var status = new CheckersGame();
    
    if (id != null)
    {
        //send the selected id game state and options to gamebrain to be handled aS the running game
        gameFinal = repositorDb.GetGame(id);
        game = new CheckersBrainMain(gameFinal!.CheckersOption!, gameFinal.CheckersGameState!.LastOrDefault());
        Console.Clear();
    }
    else
    {
        //creating new options
        Console.WriteLine("New option name: ");
        var name = Console.ReadLine();
        gameOptions.Name = name;
        Console.WriteLine("Enter width: ");
        gameOptions.Width = Convert.ToInt32(Console.ReadLine());
        
        Console.WriteLine("Enter Height: ");
        gameOptions.Height = Convert.ToInt32(Console.ReadLine());
        repoDb.SaveGameOptions(name!, gameOptions);

        //creating new game and using ^^ options with that 
        Console.WriteLine("Enter Player1 name: ");
        status.Player1Name = Console.ReadLine()!;
        Console.WriteLine("Enter number 1 for Ai");
        Console.WriteLine("Enter number 2 for Human");
        if (Console.ReadLine() == "1")
        {
            status.Player1Type = EPlayerType.Ai;  
        }else if (Console.ReadLine() == "2")
        {
            status.Player1Type = EPlayerType.Human;
        }

        Console.WriteLine("Enter Player2 name: ");
        status.Player2Name = Console.ReadLine()!;
        Console.WriteLine("Enter number 1 for Ai");
        Console.WriteLine("Enter number 2 for Human");
        if (Console.ReadLine() == "1")
        {
            status.Player2Type = EPlayerType.Ai;  
        }else if (Console.ReadLine() == "2")
        {
            status.Player2Type = EPlayerType.Human;
        }
        status.CheckersOptionId = repoDb.GetGameOptions(name!).Id;
        
        repositorDb.AddGame(status);
        
        gameFinal = repositorDb.GetGame(status.Id);
        
        game = new CheckersBrainMain(gameFinal!.CheckersOption!, null);
    }
    
    
    Ui.DrawGameBoard(game.GetBoard());
    var gameState = game.CheckGameState();
    bool whiteTurn = true;

    //game runs until there are oponent buttons
    while (gameState)
    {
        //when there is whites turn for the board
        if (whiteTurn)
        {
            //checks if there is take for the whites
            Console.WriteLine("White your turn: ");

            if (gameFinal.Player1Type == EPlayerType.Ai)
            {
                var move = game.AiMoveChooser();
                Ui.DrawGameBoard(game.NewWhiteState(
                    move[0], move[1],
                    move[2], move[3]));
            }
            else
            {
                if (game.Stepcheckerhecker())
                {
                    int startX;
                    int startY;
                    int middleX;
                    int middleY;
                    int finishX;
                    int finishY;
                    int choiceCounter = 1;
                    foreach (List<int> element in game.StepCheckerWhite())
                    {
                        startX = element[0];
                        startY = element[1];
                        middleX = element[2];
                        middleY = element[3];
                        finishX = element[4];
                        finishY = element[5];
                        Console.WriteLine(
                            "Choice " + choiceCounter + ": You have to take with button " + startX + " X " + startY +
                            " Y " + "button " + middleX + " X " + middleY + " Y " + "and move to " + finishX + " X " +
                            finishY + " Y ");
                        choiceCounter++;
                    }
                }

                //draws out new board for the whites
                GameInput();

                Ui.DrawGameBoard(game.NewWhiteState(sxaxis, syaxis, fxaxis, fyaxis));
            }

            gameState = game.CheckGameState();
                if (game.PreviousState)
                {
                    whiteTurn = true;
                    Console.WriteLine("Input correct fields!");
                }
                else
                {
                    whiteTurn = false;
                }
            
            
            
            //Save gamestate to database
            gameFinal.CheckersGameState!.Add(new CheckersGameState()
            {
                SerializedGameState = game.GetSerializedGameState(),
            });
                    
            repositorDb.SaveChanged();
            
            //Return back to menu after button insertion
            ReturnToMenu();
            
            
            
        }
        else
        {
            Console.WriteLine("Black your turn: ");
            if (gameFinal.Player2Type == EPlayerType.Ai)
            {
                var move = game.AiMoveChooser();
                Ui.DrawGameBoard(game.NewBlackState(
                    move[0], move[1],
                    move[2], move[3]));
            }
            else
            {
                if (game.Stepcheckerhecker())
                {
                    int startX;
                    int startY;
                    int middleX;
                    int middleY;
                    int finishX;
                    int finishY;
                    int choiceCounter = 1;
                    foreach (List<int> element in game.StepCheckerBlack())
                    {
                        startX = element[0];
                        startY = element[1];
                        middleX = element[2];
                        middleY = element[3];
                        finishX = element[4];
                        finishY = element[5];
                        Console.WriteLine(
                            "Choice " + choiceCounter + ": You have to take with button " + startX + " X " + startY +
                            " Y " + "button " + middleX + " X " + middleY + " Y " + "and move to " + finishX + " X " +
                            finishY + " Y ");
                        choiceCounter++;
                    }

                }

                GameInput();

                Ui.DrawGameBoard(game.NewBlackState(sxaxis, syaxis, fxaxis, fyaxis));
            }

            gameState = game.CheckGameState();
                if (game.PreviousState)
                {
                    Console.WriteLine("Input correct fields!");
                    whiteTurn = false;
                }
                else
                {
                    whiteTurn = true;
                }
            

            //Save gamestate to database
            gameFinal.CheckersGameState!.Add(new CheckersGameState()
            {
                SerializedGameState = game.GetSerializedGameState(),
            });
                    
            repositorDb.SaveChanged();
            
            //Return back to menu after button insertion
            ReturnToMenu();
        }
   
    }
    //if game is over then displays the winner
    if (game.GetWinner())
    {
        Console.WriteLine("\n White wins!");
        Console.WriteLine("\nPress any key to return Main menu...");
        gameFinal.GameOverAt = DateTime.Now;
        gameFinal.GameWonByPlayer = gameFinal.Player1Name;
        repositorDb.Update(gameFinal);
        
        Console.ReadKey(true);
        RunMainMenu();
    }
    else if (game.GetWinner() == false)
    {
        Console.WriteLine("\n Black wins!");
        Console.WriteLine("\nPress any key to return Main menu...");
        gameFinal.GameOverAt = DateTime.Now;
        gameFinal.GameWonByPlayer = gameFinal.Player1Name;
        repositorDb.Update(gameFinal);
        Console.ReadKey(true);
        RunMainMenu();
    }
}

// void SaveCurrentGame()
// {
//     if (repo == repoDb)
//     {
//         Console.Write("Save name: ");
//         var fileName = Console.ReadLine()??"noname";
//         repoDb.SaveGameOptions(fileName, gameOptions);
//         var loadedOptions = repoDb.GetGameOptions(fileName);
//         Console.WriteLine(loadedOptions);
//         var options = repoDb.GetGameOptionsList();
//         foreach (var option in options)
//         {
//             Console.WriteLine(option);
//         }
//         Console.ReadKey(true);
//         RunMainMenu();
//     }
//     
//     else if (repo == repoFs)
//     {
//         Console.Write("Save name: ");
//         var fileName = Console.ReadLine()??"noname";
//         repoFs.SaveGameOptions(fileName, gameOptions);
//         var loadedOptions = repoFs.GetGameOptions(fileName);
//         Console.WriteLine(loadedOptions);
//         var options = repoFs.GetGameOptionsList();
//         foreach (var option in options)
//         {
//             Console.WriteLine(option);
//         }
//         Console.ReadKey(true);
//         RunMainMenu();
//     }
//     
// }


void ListGameOptions()
{
    if (repo == repoDb)
    {
        Console.Clear();
        foreach (var name in repoDb.GetGameOptionsList())
        {
            Console.WriteLine(name);
        }
        Console.ReadKey(true);
        NewGame();
    }
    else
    {
        Console.Clear();
        foreach (var name in repoFs.GetGameOptionsList())
        {
            Console.WriteLine(name);
        }
        Console.ReadKey(true);
        NewGame();
    }
}

void DeleteGame()
{
    Console.Clear();
    foreach (var name in repositorDb.GetAll())
    {
        Console.WriteLine(name.Id);
    }
    Console.Write("Options name: ");
    var optionsName = Convert.ToInt32(Console.ReadLine());
    var selected = repositorDb.GetGame(optionsName);
    if (selected != null) repositorDb.Delete(selected);
    RunMainMenu();

}

    

void LoadGameOptions()
{
    Console.Clear();
    Console.Write("From db or fs: ");
    var fromWhere = Console.ReadLine();
    if (fromWhere == "db")
    {
        foreach (var name in repositorDb.GetAll())
        {
            
            Console.WriteLine(name.Id);
        }
        Console.Write("Options name: ");
        var optionsName = Convert.ToInt32(Console.ReadLine());
        if (true)
        {
            var selected = repositorDb.GetGame(optionsName);
            CustomGame(selected!.Id);
        }
    }
    if (fromWhere == "fs")
    {
        foreach (var name in repoFs.GetGameOptionsList())
        {
            Console.WriteLine(name);
        }
        Console.Write("Options name: ");
        var optionsName = Console.ReadLine();
        if (optionsName != null)
        {
            gameOptions = repoFs.GetGameOptions(optionsName);
            
        }
        RunMainMenu();
    }

}


// ILoggerFactory MyLoggerFactory =
//     LoggerFactory.Create(builder =>
//     {
//         builder
//             .AddFilter("Microsoft", LogLevel.Information)
//             .AddFilter("Microsoft", LogLevel.Information)
//             .AddConsole();
//     });

