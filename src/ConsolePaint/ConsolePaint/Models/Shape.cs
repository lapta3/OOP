namespace ConsolePaint.Models
{
    public abstract class Shape : ICloneable
    {
        public char BorderChar { get; set; }
        public string Name { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public char FillCharacter { get; set; }

        public Shape(string name, int x, int y, char borderChar, char fillChar)
        {
            Name = name;
            x = x;
            y = y;
            BorderChar = borderChar;
            FillCharacter = fillChar;
        }

        public static Shape? FromText(string type, string data)
        {
            return type switch
            {
                "Circle" => Circle.FromText(data),
                "Rectangle" => Rectangle.FromText(data),
                "Triangle" => Triangle.FromText(data),
                _ => null
            };
        }

        public abstract void DrawOnCanvas(char[,] canvas, char fillChar);
        public abstract string ToText();

        public void Move(int newX, int newY)
        {
            x = newX;
            y = newY;
        }

        public void FloodFill(char[,] canvas, int x, int y, char fillChar)
        {
            int height = canvas.GetLength(0);
            int width = canvas.GetLength(1);
            var queue = new Queue<(int, int)>();

            if (x < 0 || x >= width || y < 0 || y >= height || canvas[y, x] != ' ')
                return;

            queue.Enqueue((x, y));

            while (queue.Count > 0)
            {
                (int cx, int cy) = queue.Dequeue();
                if (cx < 0 || cx >= width || cy < 0 || cy >= height || canvas[cy, cx] != ' ')
                    continue;

                canvas[cy, cx] = fillChar;

                queue.Enqueue((cx + 1, cy));
                queue.Enqueue((cx - 1, cy));
                queue.Enqueue((cx, cy + 1));
                queue.Enqueue((cx, cy - 1));
            }
        }

        public virtual bool IsInside(int testX, int testY)
        {
            return false;
        }

        public abstract object Clone();
    }
}
