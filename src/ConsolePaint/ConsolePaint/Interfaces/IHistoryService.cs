using ConsolePaint.Models;

namespace ConsolePaint.Interfaces;

public interface IHistoryService
{
    void SaveState(Dictionary<string, Shape> currentState);

    void Undo(ref Dictionary<string, object> shapes);

    void Redo(ref Dictionary<string, Shape> shapes);
}