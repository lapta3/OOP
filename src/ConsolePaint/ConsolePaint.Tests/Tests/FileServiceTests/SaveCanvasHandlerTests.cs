using ConsolePaint.Models;
using Xunit;
using ConsolePaint.Services;
using System.IO;

namespace ConsolePaint.Tests.Tests.FileServiceTests
{
    public class SaveCanvasHandlerTests
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "canvas.txt");

        [Fact]
        public void SaveCanvas_ShouldCreateFile()
        {
            var fileService = new FileService();
    
            // Исправленный вызов конструктора Circle
            var shapes = new Dictionary<string, Shape>
            {
                { "circle1", new Circle("newname", '5', 'x', 6, 5, 'd') },  // Добавлено имя фигуры
                { "rectangle1", new Rectangle(5, 6, 'x', 2, 3, "Rectangle1", ' ') }  
            };

            if (File.Exists(_filePath))
                File.Delete(_filePath);

            fileService.SaveShapes(shapes);

            Assert.True(File.Exists(_filePath));
        }

    }
}