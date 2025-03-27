
using ConsolePaint.Models;

namespace ConsolePaint.Interfaces;

public interface IFileService
{
    void SaveShapes(Dictionary<string, Shape> shapes);
    
}