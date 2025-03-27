using ConsolePaint.Interfaces;
using ConsolePaint.Models;

namespace ConsolePaint.Services;

public class DrawingService : IDrawingService
{
    private static DrawingService _instance;
    private Dictionary<string, Shape> shapes = new();
    private HistoryService historyService = new();
    private const int CanvasWidth = 150;
    private const int CanvasHeight = 150;
    private char[,] _canvas = new char[CanvasHeight, CanvasWidth];

    public DrawingService()
    {
        InitializeCanvas();
    }

    public static DrawingService Instance => _instance ??= new DrawingService();

    private void InitializeCanvas()
    {
        for (int i = 0; i < CanvasHeight; i++)
            for (int j = 0; j < CanvasWidth; j++)
                _canvas[i, j] = ' ';
    }

    public void DrawCanvas(bool saveToFile = false)
    {
        Console.Clear();
        Console.WriteLine("+" + new string('-', CanvasWidth) + "+");
        for (int i = 0; i < CanvasHeight; i++)
        {
            Console.Write("|");
            for (int j = 0; j < CanvasWidth; j++)
                Console.Write(_canvas[i, j]);
            Console.WriteLine("|");
        }
        Console.WriteLine("+" + new string('-', CanvasWidth) + "+");
    }

    public void AddShape()
    {
        Console.Write("Введите имя фигуры: ");
        string name = Console.ReadLine();
        if (string.IsNullOrEmpty(name)) return;

        Console.Write("Введите тип фигуры (rectangle, circle, triangle): ");
        string type = Console.ReadLine()?.ToLower();
        if (string.IsNullOrEmpty(type)) return;

        int x = GetCoordinate("Введите x: ");
        int y = GetCoordinate("Введите y: ");

        Console.Write("Введите символ заливки: ");
        char fillChar = Console.ReadKey().KeyChar;
        Console.WriteLine();

        Shape shape;

        switch (type)
        {
            case "rectangle":
                int width = GetCoordinate("Введите ширину: ");
                int height = GetCoordinate("Введите высоту: ");
                Console.Write("Введите символ границы: ");
                char rectBorder = Console.ReadKey().KeyChar;
                Console.WriteLine();
                shape = new Rectangle(width, height, rectBorder, x, y, name, fillChar);
                break;
            case "circle":
                int radius = GetCoordinate("Введите радиус: ");
                Console.Write("Введите символ границы: ");
                char circBorder = Console.ReadKey().KeyChar;
                Console.WriteLine();
                shape = new Circle(name, radius, circBorder, x, y, fillChar);
                break;
            case "triangle":
                int triHeight = GetCoordinate("Введите высоту: ");
                int triWidth = GetCoordinate("Введите ширину: ");
                Console.Write("Введите символ границы: ");
                char triBorder = Console.ReadKey().KeyChar;
                Console.WriteLine();
                shape = new Triangle(triHeight, triBorder, x, y, triWidth, name, fillChar);
                break;
            default:
                Console.WriteLine("Несуществующий тип фигуры");
                return;
        }

        historyService.SaveState(new Dictionary<string, Shape>(shapes));
        shapes[name] = shape;
        RedrawCanvas(true);
    }

    public void FillShape(string shapeName, char fillChar)
    {
        if (string.IsNullOrEmpty(shapeName) || !shapes.ContainsKey(shapeName))
        {
            Console.WriteLine("Фигура не найдена.");
            return;
        }

        Shape shape = shapes[shapeName];
        shape.FillCharacter = fillChar;

        RedrawCanvas();
    }

    public void RemoveShape(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Некорректное имя фигуры");
            return;
        }

        if (shapes.Remove(name))
        {
            RedrawCanvas();
        }
        else
        {
            Console.WriteLine("Фигура не найдена");
        }
    }

    public void MoveShape()
    {
        Console.Write("Введите название фигуры для перемещения: ");
        string name = Console.ReadLine();
        if (string.IsNullOrEmpty(name) || !shapes.ContainsKey(name))
        {
            Console.WriteLine("Фигура не найдена");
            return;
        }

        int newX = GetCoordinate("Введите новую x: ");
        int newY = GetCoordinate("Введите новую y: ");

        historyService.SaveState(new Dictionary<string, Shape>(shapes));

        Shape shape = shapes[name];
        shape.Move(newX, newY);

        historyService.SaveState(new Dictionary<string, Shape>(shapes));
        RedrawCanvas();
    }

    public void LoadFromFile()
    {
        FileService fileService = new FileService();
        shapes = fileService.LoadShapes();
        Console.WriteLine($"Загружено фигур: {shapes.Count}");

        InitializeCanvas();

        foreach (var shape in shapes.Values)
        {
            char fillChar = shape.FillCharacter;

            shape.DrawOnCanvas(_canvas, fillChar);
        }

        RedrawCanvas();
    }

    public void Undo()
    {
        historyService.Undo(ref shapes);
        RedrawCanvas();
    }

    public void Redo()
    {
        historyService.Redo(ref shapes);
        RedrawCanvas();
    }

    public void Save()
    {
        FileService fileService = new FileService();
        fileService.SaveShapes(shapes);
    }

    public void RedrawCanvas(bool saveToFile = true)
    {
        InitializeCanvas();
        foreach (var shape in shapes.Values)
        {
            shape.DrawOnCanvas(_canvas, shape.FillCharacter);
        }
        DrawCanvas(saveToFile);
    }

    private int GetCoordinate(string prompt)
    {
        int coordinate;
        do
        {
            Console.Write(prompt);
        }
        while (!int.TryParse(Console.ReadLine(), out coordinate) || coordinate < 0 || coordinate >= CanvasWidth);

        return coordinate;
    }
}
