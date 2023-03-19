namespace DAL;

public interface IBaseRepository
{
    string Name { get; }
    void SaveChanged();
}