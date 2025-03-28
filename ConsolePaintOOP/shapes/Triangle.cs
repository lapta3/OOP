namespace ConsolePaintOOP.Shapes
{
    public class Triangle : Shape
    {
        private readonly int a, b, c;
        private readonly int x, y;

        public Triangle() : base() { }

        public Triangle(int x, int y, int a, int b, int c, char symbol, ConsoleColor color)
            : base(symbol, color)
        {
            this.x = x; // Базовая точка (левый угол)
            this.y = y;
            this.a = a;
            this.b = b;
            this.c = c;
            CalculatePixels();
        }

        protected override void CalculatePixels()
        {
            OuterPixels.Clear();
            InnerPixels.Clear();

            // Вычисление координат вершин треугольника
            var (x1, y1, x2, y2, x3, y3) = CalculateTriangleCoordinates();

            // Рисуем контур треугольника (три стороны)
            DrawLine(x1, y1, x2, y2);
            DrawLine(x2, y2, x3, y3);
            DrawLine(x3, y3, x1, y1);

            // Заполняем внутреннюю область (скан-линия)
            FillTriangle(x1, y1, x2, y2, x3, y3);
        }

        /// <summary>
        /// Вычисляет координаты трех вершин треугольника на основе сторон a, b, c.
        /// </summary>
        private (int x1, int y1, int x2, int y2, int x3, int y3) CalculateTriangleCoordinates()
        {
            int x1 = x, y1 = y;             // Левая нижняя вершина
            int x2 = x1 + a, y2 = y1;       // Правая нижняя вершина

            // Вычисляем высоту треугольника по формуле Герона
            double s = (a + b + c) / 2.0;  // Полупериметр
            double area = Math.Sqrt(s * (s - a) * (s - b) * (s - c)); // Площадь
            int height = (int)Math.Round(2 * area / a); // Высота

            int x3 = x1 + (a / 2); // Вершина треугольника
            int y3 = y1 - height;

            return (x1, y1, x2, y2, x3, y3);
        }

        /// <summary>
        /// Заполняет внутреннюю область треугольника.
        /// </summary>
        private void FillTriangle(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            int minY = Math.Min(y1, Math.Min(y2, y3));
            int maxY = Math.Max(y1, Math.Max(y2, y3));

            for (int y = minY + 1; y < maxY; y++)
            {
                List<double> nodeX = [];

                // Вычисляем пересечения скан-линии с ребрами треугольника
                ComputeIntersection(x1, y1, x2, y2, y, nodeX);
                ComputeIntersection(x2, y2, x3, y3, y, nodeX);
                ComputeIntersection(x3, y3, x1, y1, y, nodeX);

                if (nodeX.Count >= 2)
                {
                    nodeX.Sort();

                    for (int i = 0; i < nodeX.Count; i += 2)
                    {
                        if (i + 1 >= nodeX.Count) break;
                        int startX = (int)Math.Ceiling(nodeX[i]);
                        int endX = (int)Math.Floor(nodeX[i + 1]);

                        for (int x = startX; x <= endX; x++)
                        {
                            if (!OuterPixels.Any(p => p.X == x && p.Y == y))
                            {
                                InnerPixels.Add(new Pixel(x, y, ' ', Color));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Рисует линию между двумя точками.
        /// </summary>
        private void DrawLine(int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;
            int err = dx - dy;

            int cx = x1, cy = y1;
            while (true)
            {
                OuterPixels.Add(new Pixel(cx, cy, Symbol, Color));

                if (cx == x2 && cy == y2)
                    break;

                int e2 = err * 2;
                if (e2 > -dy)
                {
                    err -= dy;
                    cx += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    cy += sy;
                }
            }
        }

        /// <summary>
        /// Вычисляет точку пересечения скан-линии с ребром треугольника.
        /// </summary>
        private static void ComputeIntersection(int x1, int y1, int x2, int y2, int scanlineY, List<double> nodeX)
        {
            if ((y1 < scanlineY && y2 >= scanlineY) || (y2 < scanlineY && y1 >= scanlineY))
            {
                double xIntersection = x1 + (scanlineY - y1) * (x2 - x1) / (double)(y2 - y1);
                nodeX.Add(xIntersection);
            }
        }
    }
}
