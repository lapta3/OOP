using ConsolePaint.Interfaces;
using ConsolePaint.Models;

namespace ConsolePaint.Services
{
    public class FileService : IFileService
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "canvas.txt");

        public static Shape? FromText(string type, string data)
        {
            return type switch
            {
                "Circle" => Circle.FromText(data),
                "Rectangle" => Rectangle.FromText(data),
                _ => null
            };
        }

        // Save shapes to a file
        public void SaveShapes(Dictionary<string, Shape> shapes)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_filePath))
                {
                    foreach (var shape in shapes)
                    {
                        // Serialize the shape to text
                        string shapeData = shape.Value.ToText();
                        writer.WriteLine($"{shape.Key}|{shape.Value.GetType().Name}|{shapeData}");
                    }
                }
                Console.WriteLine($"Фигуры сохранены в {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }
        }

        // Load shapes from a file
        public Dictionary<string, Shape> LoadShapes()
        {
            var shapes = new Dictionary<string, Shape>();
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("Файл не найден.");
                return shapes;
            }

            try
            {
                using (StreamReader reader = new StreamReader(_filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine($"Чтение строки: {line}");
                        var parts = line.Split('|');
                        if (parts.Length < 3) continue;

                        string name = parts[0];
                        string type = parts[1];
                        string data = parts[2];

                        // Deserialize the shape from text
                        Shape shape = FromText(type, data);
                        if (shape != null)
                        {
                            Console.WriteLine($"Загружена фигура: {name} ({type})");
                            shapes[name] = shape;
                        }
                        else
                        {
                            Console.WriteLine($"Не удалось десериализовать фигуру: {type} с данными: {data}");
                        }
                    }
                }
                Console.WriteLine("Фигуры успешно загружены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке: {ex.Message}");
            }
            return shapes;
        }
    }
}
