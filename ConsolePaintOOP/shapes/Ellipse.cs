namespace ConsolePaintOOP.Shapes
{
    public class Ellipse : Shape
    {
        private readonly int centerX, centerY, radiusX, radiusY;

        public Ellipse() : base() { }

        public Ellipse(int centerX, int centerY, int radiusX, int radiusY, char symbol, ConsoleColor color)
            : base(symbol, color)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.radiusX = radiusX;
            this.radiusY = radiusY;
            CalculatePixels();
        }

        protected override void CalculatePixels()
        {
            OuterPixels.Clear();
            InnerPixels.Clear();

            if (radiusX == radiusY)
            {
                // Если радиусы равны, рисуем круг
                DrawCircle(centerX, centerY, radiusX);
            }
            else
            {
                // Если радиусы разные, рисуем эллипс
                DrawEllipse();
            }
        }

        /// <summary>
        /// Рисует эллипс по уравнению (x - centerX)^2 / radiusX^2 + (y - centerY)^2 / radiusY^2 = 1
        /// </summary>
        private void DrawEllipse()
        {
            for (int y = centerY - radiusY; y <= centerY + radiusY; y++)
            {
                for (int x = centerX - radiusX; x <= centerX + radiusX; x++)
                {
                    double distance = Math.Pow((x - centerX) / (double)radiusX, 2) +
                                      Math.Pow((y - centerY) / (double)radiusY, 2);
                    if (distance <= 1)
                    {
                        if (Math.Abs(distance - 1) < 0.05)
                            OuterPixels.Add(new Pixel(x, y, Symbol, Color));
                        else
                            InnerPixels.Add(new Pixel(x, y, ' ', Color));
                    }
                }
            }
        }

        /// <summary>
        /// Рисует круг с помощью алгоритма Брезенхэма
        /// </summary>
        private void DrawCircle(int centerX, int centerY, int radius)
        {
            int x = 0;
            int y = radius;
            int d = 3 - 2 * radius; // Начальное значение ошибки

            while (x <= y)
            {
                // Добавляем пиксели для всех восьми симметричных точек
                PlotCirclePoints(centerX, centerY, x, y);
                x++;

                if (d > 0)
                {
                    y--;
                    d = d + 4 * (x - y) + 10;
                }
                else
                {
                    d = d + 4 * x + 6;
                }
            }

            // Заполняем внутреннюю область
            FillCircle(centerX, centerY, radius);
        }

        /// <summary>
        /// Добавляет точки окружности в 8 симметричных направлениях
        /// </summary>
        private void PlotCirclePoints(int cx, int cy, int x, int y)
        {
            OuterPixels.Add(new Pixel(cx + x, cy + y, Symbol, Color));
            OuterPixels.Add(new Pixel(cx - x, cy + y, Symbol, Color));
            OuterPixels.Add(new Pixel(cx + x, cy - y, Symbol, Color));
            OuterPixels.Add(new Pixel(cx - x, cy - y, Symbol, Color));
            OuterPixels.Add(new Pixel(cx + y, cy + x, Symbol, Color));
            OuterPixels.Add(new Pixel(cx - y, cy + x, Symbol, Color));
            OuterPixels.Add(new Pixel(cx + y, cy - x, Symbol, Color));
            OuterPixels.Add(new Pixel(cx - y, cy - x, Symbol, Color));
        }

        /// <summary>
        /// Заполняет круг пикселями
        /// </summary>
        private void FillCircle(int cx, int cy, int radius)
        {
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    if (x * x + y * y <= radius * radius)
                    {
                        if (!OuterPixels.Any(p => p.X == cx + x && p.Y == cy + y))
                        {
                            InnerPixels.Add(new Pixel(cx + x, cy + y, ' ', Color));
                        }
                    }
                }
            }
        }
    }
}
