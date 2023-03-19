using System.Text.Json;
using Domain;

namespace DAL.FileSystem;

public class GameOptionsRepositoryFileSystem : IGameOptionsRepository
{
    private const string FileExtension = "Json";
    private readonly string _optionsDirectory = "." + System.IO.Path.DirectorySeparatorChar + "options";

    public string Name { get; } = "FileSystem";
    
    public void SaveChanged()
    {
        throw new NotImplementedException("File system is updated immediately!");
    }


    public List<string> GetGameOptionsList()
    {
        CheckOrCreateDirectory();
        
        var res = new List<string>();

        foreach (var fileName in  Directory.GetFileSystemEntries(_optionsDirectory, "*." + FileExtension))
        {
            res.Add(System.IO.Path.GetFileNameWithoutExtension(fileName));
        }

        return res;
    }

    public CheckersOption GetGameOptions(string id)
    {
        var fileContent = System.IO.File.ReadAllText(GetFileName(id));
        var options = JsonSerializer.Deserialize<CheckersOption>(fileContent);
        if (options == null)
        {
            throw new NullReferenceException($"Could not deserialize: {fileContent}");
        }
        return options;
    }

    public void SaveGameOptions(string id, CheckersOption option)
    {
        CheckOrCreateDirectory();
        
        var fileContent = JsonSerializer.Serialize(option);
        File.WriteAllText(GetFileName(id), fileContent);

    }

    public void DeleteGameOptions(string id)
    {
        System.IO.File.Delete(GetFileName(id));
    }

    private string GetFileName(string id)
    {
        return _optionsDirectory + 
               System.IO.Path.DirectorySeparatorChar + 
               id + "." + 
               FileExtension;
    }

    private void CheckOrCreateDirectory()
    {
        if (!System.IO.Directory.Exists(_optionsDirectory))
        {
            System.IO.Directory.CreateDirectory(_optionsDirectory);
        }
    }
    
}