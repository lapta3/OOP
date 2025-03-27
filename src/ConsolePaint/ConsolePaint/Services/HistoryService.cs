using ConsolePaint.Interfaces;
using ConsolePaint.Models;

namespace ConsolePaint.Services
{
    public class HistoryService 
    {
        private Stack<Dictionary<string, Shape>> historyStack = new();
        private Stack<Dictionary<string, Shape>> redoStack = new();

        public void SaveState(Dictionary<string, Shape> currentState)
        {
            // Make a deep copy of all Shape objects
            var copiedState = currentState.ToDictionary(
                kvp => kvp.Key,
                kvp => (Shape)kvp.Value.Clone() // Call Clone on Shape
            );
            historyStack.Push(copiedState);
            redoStack.Clear();
        }

        public void Undo(ref Dictionary<string, Shape> shapes)
        {
            if (historyStack.Count > 0)
            {
                var previousState = historyStack.Pop();
                redoStack.Push(new Dictionary<string, Shape>(shapes.ToDictionary(kvp => kvp.Key, kvp => (Shape)kvp.Value.Clone())));
                shapes = new Dictionary<string, Shape>(previousState.ToDictionary(kvp => kvp.Key, kvp => (Shape)kvp.Value.Clone()));
            }
        }

        public void Redo(ref Dictionary<string, Shape> shapes)
        {
            if (redoStack.Count > 0)
            {
                var nextState = redoStack.Pop();
                historyStack.Push(new Dictionary<string, Shape>(shapes.ToDictionary(kvp => kvp.Key, kvp => (Shape)kvp.Value.Clone())));
                shapes = new Dictionary<string, Shape>(nextState.ToDictionary(kvp => kvp.Key, kvp => (Shape)kvp.Value.Clone()));
            }
        }
    }
}