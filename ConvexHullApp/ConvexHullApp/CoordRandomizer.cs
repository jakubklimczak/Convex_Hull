namespace ConvexHullApp
{
    public static class CoordRandomizer
    {
        private static readonly Random random = new();
        public static Point GetRandomCoordinates(double min_x, double max_x, double min_y, double max_y) 
        {
            var coord_x = random.NextDouble() * (max_x - min_x) + min_x;
            var coord_y = random.NextDouble() * (max_y - min_y) + min_y;

            return new ConvexHullApp.Point(coord_x, coord_y);
        }
    }
}
