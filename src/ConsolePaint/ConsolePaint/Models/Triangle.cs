namespace ConsolePaint.Models
{
    public class Triangle : Shape
    {
        public int Height { get; set; }
        public int Width { get; set; }

        public override string ToText()
        {
            return $"{x},{y},{Width},{Height},{BorderChar},{FillCharacter}";
        }

        public static Triangle FromText(string data)
        {
            var parts = data.Split(',');
            if (parts.Length != 6) throw new FormatException("Invalid Triangle data format");

            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int width = int.Parse(parts[2]);
            int height = int.Parse(parts[3]);
            char borderChar = parts[4][0];
            char fillCharacter = parts[5][0];

            return new Triangle(height, borderChar, x, y, width, "LoadedTriangle", fillCharacter);
        }

        public Triangle(int height, char borderChar, int x, int y, int width, string name, char fillCharacter)
            : base(name, x, y, borderChar, fillCharacter) // Вызов конструктора базового класса
        {
            Height = height;
            Width = width;
        }

        public override void DrawOnCanvas(char[,] canvas, char fillChar)
        {
            int centerX = x;
            int startY = y;

            for (int i = 0; i < Height; i++)
            {
                int startX = centerX - i;
                int endX = centerX + i;

                for (int j = startX; j <= endX; j++)
                {
                    if (i == Height - 1 || j == startX || j == endX)  // Рисуем границу
                    {
                        if (j >= 0 && j < canvas.GetLength(1) && (startY + i) >= 0 &&
                            (startY + i) < canvas.GetLength(0))
                        {
                            canvas[startY + i, j] = BorderChar;
                        }
                    }
                    else  // Заполняем внутреннюю область
                    {
                        if (j >= 0 && j < canvas.GetLength(1) && (startY + i) >= 0 &&
                            (startY + i) < canvas.GetLength(0))
                        {
                            canvas[startY + i, j] = fillChar;
                        }
                    }
                }
            }
        }

        public override bool IsInside(int testX, int testY)
        {
            int ax = x, ay = y;
            int bx = x - Width / 2, by = y + Height;
            int cx = x + Width / 2, cy = y + Height;

            int areaABC = Math.Abs((bx - ax) * (cy - ay) - (by - ay) * (cx - ax));

            int areaPAB = Math.Abs((bx - testX) * (ay - testY) - (by - testY) * (ax - testX));
            int areaPBC = Math.Abs((cx - testX) * (by - testY) - (cy - testY) * (bx - testX));
            int areaPCA = Math.Abs((ax - testX) * (cy - testY) - (ay - testY) * (cx - testX));

            return (areaABC == areaPAB + areaPBC + areaPCA);
        }

        public override object Clone()
        {
            return new Triangle(Height, BorderChar, x, y, Width, Name, FillCharacter);
        }
    }
}
