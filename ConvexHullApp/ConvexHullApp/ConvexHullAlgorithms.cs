namespace ConvexHullApp
{
    public enum GeometricalShape
    {
        BlankSpace,
        Point,
        Line,
        Triangle,
        Quadrilateral
    }

    #pragma warning disable CS8765
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Point p = (Point)obj;
            return X == p.X && Y == p.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
    #pragma warning restore CS8765

    public class Result(Point[] points, GeometricalShape shape)
    {
        public Point[] Points = points;
        public GeometricalShape Shape = shape;
    }

    public static class ConvexHullAlgorithms
    {

        /*
         *  Helper function allowing to find the orientation of three points.
         *  Returns 0 if points are collinear, 1 if they are oriented anticlockwise, and 2 if they are oriented clockwise
         */
        public static int Orientation(Point p, Point q, Point r)
        {
            int val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0;  // collinear
            return val > 0 ? 1 : 2;
        }

        /*
         *  Helper function allowing to find the distance between 2 points.
         */
        public static double Distance(Point p, Point q)
        {
            return Math.Sqrt((q.X - p.X) * (q.X - p.X) + (q.Y - p.Y) * (q.Y - p.Y));
        }

        /*
         * Function that performs Jarvis-Hull algorithm on an array of 2d geometrical points (X, Y)
         * Returns an ordered array of points
         */
        public static Result JarvisHullAlgorithm(Point[] inputPointsArray)
        {
            // Handle the cases where there are 0, 1, or 2 points directly, skipping the computationally intensive part of the function
            if (inputPointsArray.Length == 0 || inputPointsArray.Length == 1 || inputPointsArray.Length == 2)
            { return new Result(points: inputPointsArray, shape: (GeometricalShape)inputPointsArray.Length); }

            List<Point> pointList = [];

            // Detection of leftmost point in the array - it has to be a part of convex hull
            Point startingPoint = inputPointsArray[0];
            foreach (var itPoint in inputPointsArray)
            {
                if (itPoint.X < startingPoint.X)
                {
                    startingPoint = itPoint;
                }
            }

            Point currentPoint = startingPoint;

            // Traverse the points anticlockwise to find the convex hull
            do
            {
                pointList.Add(currentPoint);
                Point nextPoint = inputPointsArray[0];

                foreach (var itPoint in inputPointsArray)
                {
                    // Skip over current point 
                    if (itPoint == currentPoint) continue;

                    int direction = Orientation(currentPoint, nextPoint, itPoint);

                    // If next is still current or the point forms a anticlockwise angle, update next to be this point and clear the collinear points list
                    if (nextPoint == currentPoint || direction == 1)
                    {
                        nextPoint = itPoint;
                    }

                    // Handle collinear points
                    else if (direction == 0 && Distance(currentPoint, itPoint) > Distance(currentPoint, nextPoint))
                    {
                        nextPoint = itPoint;
                    }
                }

                currentPoint = nextPoint;

            } while (currentPoint != startingPoint);

            // Remove duplicates in the hull
            pointList = pointList.Distinct().ToList();

            // Determine the shape based on the number of points in the hull
            GeometricalShape shape = (GeometricalShape)Math.Min(pointList.Count, Enum.GetValues(typeof(GeometricalShape)).Length - 1);

            return new Result(points: [.. pointList], shape: shape);
        }
    }
}
