using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvexHullApp
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Result
    {
        public Point[] Points;

        public Result(Point[] input)
        {
            Points = input;
        }


    }

    internal class ConvexHullAlgorithms
    {
        public ConvexHullAlgorithms() { }

        /*
         * Function that performs Jarvis-Hull algorithm on an array of 2d geometrical points (X, Y)
         * Returns an ordered array of points
         */
        public Result JarvisHullAlgorithm(Point[] inputPointsArray)
        {
            if (inputPointsArray.Length == 0 || inputPointsArray.Length == 1 || inputPointsArray.Length == 2)
                { return new Result(inputPointsArray); }


        }
    }
}
