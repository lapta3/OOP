namespace ConsolePaint.Models
{
    public class Rectangle : Shape
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public override string ToText()
        {
            return $"{x},{y},{Width},{Height},{BorderChar},{FillCharacter}";
        }

        public static Rectangle FromText(string data)
        {
            var parts = data.Split(',');
            if (parts.Length != 6) throw new FormatException("Invalid Rectangle data format");

            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int width = int.Parse(parts[2]);
            int height = int.Parse(parts[3]);
            char borderChar = parts[4][0];
            char fillCharacter = parts[5][0];
            return new Rectangle(width, height, borderChar, x, y, "LoadedRectangle", fillCharacter);
        }

        public Rectangle(int width, int height, char borderChar, int x, int y, string name, char fillCharacter)
            : base(name, x, y, borderChar, fillCharacter)
        {
            Width = width;
            Height = height;
            BorderChar = borderChar;
            this.x = x;
            this.y = y;
            Name = name;
            FillCharacter = fillCharacter;
        }

        public override void DrawOnCanvas(char[,] canvas, char fillChar)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    int canvasX = x + j;
                    int canvasY = y + i;

                    if (canvasX >= 0 && canvasX < canvas.GetLength(1) && canvasY >= 0 && canvasY < canvas.GetLength(0))
                    {
                        if (i == 0 || i == Height - 1 || j == 0 || j == Width - 1)  // Рисуем границу
                        {
                            canvas[canvasY, canvasX] = BorderChar;
                        }
                        else  // Заполняем внутреннюю область
                        {
                            canvas[canvasY, canvasX] = fillChar;
                        }
                    }
                }
            }
        }

        public override bool IsInside(int testX, int testY)
        {
            return testX >= x && testX < x + Width && testY >= y && testY < y + Height;
        }

        public override object Clone()
        {
            return new Rectangle(Width, Height, BorderChar, x, y, Name, FillCharacter);
        }
    }
}
