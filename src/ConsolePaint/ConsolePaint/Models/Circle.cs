namespace ConsolePaint.Models
{
    public class Circle : Shape
    {
        public int Radius { get; set; }

        public override string ToText()
        {
            return $"{x},{y},{Radius},{BorderChar},{FillCharacter}";
        }

        public static Circle FromText(string data)
        {
            var parts = data.Split(',');
            if (parts.Length != 5) throw new FormatException("Invalid Circle data format");

            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int radius = int.Parse(parts[2]);
            char borderChar = parts[3][0];
            char fillCharacter = parts[4][0];

            return new Circle("LoadedCircle", radius, borderChar, x, y, fillCharacter);
        }

        public Circle(string name, int radius, char borderChar, int x, int y, char fillCharacter)
            : base(name, x, y, borderChar, fillCharacter)
        {
            Radius = radius;
        }

        public override void DrawOnCanvas(char[,] canvas, char fillChar)
        {
            int centerX = x;
            int centerY = y;

            for (int i = -Radius; i <= Radius; i++)
            {
                for (int j = -Radius; j <= Radius; j++)
                {
                    int distance = i * i + j * j;
                    int drawX = centerX + j;
                    int drawY = centerY + i;

                    if (drawX >= 0 && drawX < canvas.GetLength(1) && drawY >= 0 && drawY < canvas.GetLength(0))
                    {
                        if (distance <= Radius * Radius)
                        {
                            if (distance >= (Radius - 1) * (Radius - 1))
                            {
                                canvas[drawY, drawX] = BorderChar;
                            }
                            else
                            {
                                canvas[drawY, drawX] = fillChar;
                            }
                        }
                    }
                }
            }
        }

        public override bool IsInside(int testX, int testY)
        {
            int dx = testX - x;
            int dy = testY - y;
            return (dx * dx + dy * dy <= Radius * Radius);
        }

        public override object Clone()
        {
            return new Circle(Name, Radius, BorderChar, x, y, FillCharacter);
        }
    }
}
