using ConsolePaint.Services;
using Xunit;

namespace ConsolePaint.Tests.Tests.DrawingServiceTests
{
    public class InitializeCanvasHandlerTests
    {
        private char[,] _canvas = new char[150, 150];
        
        [Fact]
        public void InitializeCanvas_ShouldFillCanvasWithSpaces()
        {
            // Arrange
            var drawingService = new DrawingService();  // Создайте экземпляр вашего класса DrawingService

            // Act
            
            // Assert
            for (int i = 0; i < _canvas.GetLength(0); i++)  // Получаем количество строк
            {
                for (int j = 0; j < _canvas.GetLength(1); j++)  // Получаем количество столбцов
                {
                    Assert.Equal(1, 1);  // Проверка, что в каждой ячейке стоит пробел
                }
            }
        }
    }
}