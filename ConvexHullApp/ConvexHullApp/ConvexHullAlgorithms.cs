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

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Result(Point[] points, GeometricalShape shape)
    {
        public Point[] Points = points;
        public GeometricalShape Shape = shape;
    }

    public static class ConvexHullAlgorithms
    {

        /*
         *  Helper function allowing to find the orientation of three points.
         *  Returns 0 if points are collinear, 1 if they are oriented clockwise, and 2 if they are oriented anticlockwise
         */
        public static int Orientation(Point p, Point q, Point r)
        {
            int val = (q.Y - p.Y) * (r.X - q.X) -
                      (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0;  // collinear
            return (val > 0) ? 1 : 2;
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
            List<Point> collinearPointList = [];

            // detection of leftmost point in the array - it has to be a part of convex hull
            Point startingPoint = inputPointsArray[0];
            foreach (var itPoint in inputPointsArray)
            {
                if (itPoint.X < startingPoint.X)
                {
                    startingPoint = itPoint;
                }
            }

            Point currentPoint = startingPoint;

            // Traverse the points counterclockwise to find the convex hull
            do
            {
                pointList.Add(currentPoint);
                Point nextPoint = inputPointsArray[0];

                foreach (var itPoint in inputPointsArray)
                {
                    // skip over current point 
                    if (itPoint == currentPoint) continue;

                    int direction = Orientation(currentPoint, nextPoint, itPoint);

                    // If next is still current or the point forms a counterclockwise angle, update next to be this point and clear the collinear points list
                    if (nextPoint == currentPoint || direction == 2)
                    {
                        nextPoint = itPoint;
                        collinearPointList.Clear();
                    }

                    // Handle collinear points
                    else if (direction == 0)
                    {
                        if (Distance(currentPoint, itPoint) > Distance(currentPoint, nextPoint))
                        {
                            // If the new point is farther, add the current next to collinear points and update next
                            collinearPointList.Add(nextPoint);
                            nextPoint = itPoint;
                        }
                        else
                        {
                            collinearPointList.Add(itPoint);
                        }
                    }
                }

                foreach (var p in collinearPointList)
                {
                    pointList.Add(p);
                }

                currentPoint = nextPoint;

            } while (currentPoint != startingPoint);

            // Determine the shape based on the number of points in the hull
            GeometricalShape shape = (GeometricalShape)Math.Min(pointList.Count, Enum.GetValues(typeof(GeometricalShape)).Length - 1);

            return new Result(points: [.. pointList], shape: shape);
        }
    }
}
