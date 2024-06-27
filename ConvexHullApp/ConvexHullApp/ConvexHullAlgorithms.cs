namespace ConvexHullApp
{
    public class Point(double x, double y)
    {
        public double X { get; set; } = x;
        public double Y { get; set; } = y;
        private const double Epsilon = 1e-9;

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is Point other)
            {
                return Math.Abs(X - other.X) < Epsilon && Math.Abs(Y - other.Y) < Epsilon;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Math.Round(X / Epsilon), Math.Round(Y / Epsilon));
        }
    }

    public class Result(Point[] points, string shape)
    {
        public Point[] Points { get; set; } = points;
        public string Shape { get; set; } = shape;
    }

    public static class ConvexHullAlgorithms
    {
        /*
         * Helper function for returning the correct name of the geometrical figure
         */
        private static string GetFigureName(int numberOfPoints)
        {
            return numberOfPoints switch
            {
                0 => "Blank - Convex Hull invalid",
                1 => "Point - Convex Hull invalid",
                2 => "Line Segment - Convex Hull invalid",
                3 => "Triangle",
                4 => "Quadrilateral",
                5 => "Pentagon",
                6 => "Hexagon",
                7 => "Heptagon",
                8 => "Octagon",
                _ => numberOfPoints + "-gon",
            };
        }

        /*
         * Helper function for clearing duplicates from the point array
         */
        public static Point[] RemoveDuplicates(Point[] points)
        {
            HashSet<Point> uniquePoints = new(points);
            return new List<Point>(uniquePoints).ToArray();
        }

        /*
         *  Helper function allowing to find the orientation of three points.
         *  Returns 0 if points are collinear, -1 if they are oriented anticlockwise, and 1 if they are oriented clockwise
         */
        public static int Orientation(Point p, Point q, Point r)
        {
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (Math.Abs(val) > -0.0000001 && Math.Abs(val) < 0.0000001)
            {
                return 0; // collinear
            }

            return (val > 0) ? -1 : 1; // clock or anticlock wise
        }
        /*
         * Helper function allowing to get second element from the top of stack
         */
        private static Point TwoPointsBack(Stack<Point> stack)
        {
            Point top = stack.Pop();
            Point result = stack.Peek();
            stack.Push(top);
            return result;
        }

        /*
         * Function that performs Jarvis-Hull algorithm on an array of 2d geometrical points (X, Y)
         * Returns an ordered array of points
         */
        public static Result JarvisHullAlgorithm(Point[] inputPointsArray)
        {
            // Handle the cases where there are 0, 1, or 2 points directly, skipping the computationally intensive part of the function
            if (inputPointsArray.Length == 0 || inputPointsArray.Length == 1 || inputPointsArray.Length == 2)
            { 
                return new Result(points: inputPointsArray, shape: GetFigureName(inputPointsArray.Length)); 
            }

            inputPointsArray = RemoveDuplicates(inputPointsArray);

            List<Point> pointList = [];

            // Detection of leftmost point in the array - it has to be a part of convex hull
            int leftmostIndex = 0;
            for (int i = 1; i < inputPointsArray.Length; i++)
            {
                if (inputPointsArray[i].X < inputPointsArray[leftmostIndex].X)
                {
                    leftmostIndex = i;
                }

                else if (inputPointsArray[i].X.Equals(inputPointsArray[leftmostIndex].X) && inputPointsArray[i].Y < inputPointsArray[leftmostIndex].Y)
                {
                    leftmostIndex = i;
                }
            }
            int currentPointIndex = leftmostIndex, nextPointIndex;

            // Traverse the points anticlockwise to find the convex hull
            do
            {
                pointList.Add(inputPointsArray[currentPointIndex]);
                nextPointIndex = (currentPointIndex + 1) % inputPointsArray.Length;

                for (int i = 0; i < inputPointsArray.Length; i++)
                {
                    if (Orientation(inputPointsArray[currentPointIndex], inputPointsArray[i], inputPointsArray[nextPointIndex]) == 1)
                    {
                        nextPointIndex = i;
                    }
                }

                currentPointIndex = nextPointIndex;
            } while (currentPointIndex != leftmostIndex);

            string figureName = GetFigureName(pointList.Count);
            return new Result([.. pointList], figureName);
        }

        /*
         * Function that performs Graham-Hull algorithm on an array of 2d geometrical points (X, Y)
         * Returns an ordered array of points
         */
        public static Result GrahamScan(Point[] inputPointsArray)
        {
            // Handle the cases where there are 0, 1, or 2 points directly, skipping the computationally intensive part of the function
            if (inputPointsArray.Length == 0 || inputPointsArray.Length == 1 || inputPointsArray.Length == 2)
            {
                return new Result(points: inputPointsArray, shape: GetFigureName(inputPointsArray.Length));
            }

            inputPointsArray = RemoveDuplicates(inputPointsArray);

            // Find the point with the lowest y-coordinate, break ties by the lowest x-coordinate
            Point start = inputPointsArray.OrderBy(p => p.Y).ThenBy(p => p.X).First();

            // Sort the points by the polar angle with the start point
            var sortedPoints = inputPointsArray.OrderBy(p => Math.Atan2(p.Y - start.Y, p.X - start.X)).ToArray();

            Stack<Point> hull = new();
            hull.Push(start);

            foreach (var point in sortedPoints)
            {
                while (hull.Count > 1 && Orientation(TwoPointsBack(hull), hull.Peek(), point) < 0)
                {
                    hull.Pop();
                }
                hull.Push(point);
            }

 
            List<Point> pointList = [.. hull];
            if (pointList.Count > 0)
            {
                pointList.RemoveAt(pointList.Count - 1);
            }
            string figureName = GetFigureName(pointList.Count);
            return new Result([.. pointList], figureName);
        }
    }
}
