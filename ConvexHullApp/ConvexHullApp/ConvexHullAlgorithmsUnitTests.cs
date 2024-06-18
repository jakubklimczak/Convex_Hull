using NUnit.Framework;

namespace ConvexHullApp
{
    [TestFixture]
    public class ConvexHullAlgorithmsUnitTests
    {
        [Test]
        public void TestOrientationCollinear()
        {
            var p = new Point { X = 0, Y = 0 };
            var q = new Point { X = 1, Y = 1 };
            var r = new Point { X = 2, Y = 2 };
            var result = ConvexHullAlgorithms.Orientation(p, q, r);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void TestOrientationClockwise()
        {
            var p = new Point { X = 0, Y = 0 };
            var q = new Point { X = 4, Y = 4 };
            var r = new Point { X = 1, Y = 2 };
            var result = ConvexHullAlgorithms.Orientation(p, q, r);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void TestOrientationAntiClockwise()
        {
            var p = new Point { X = 0, Y = 0 };
            var q = new Point { X = 1, Y = 1 };
            var r = new Point { X = 2, Y = 0 };
            var result = ConvexHullAlgorithms.Orientation(p, q, r);
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void TestDistance()
        {
            // 3-4-5 triangle, distance is 5
            var p = new Point { X = 0, Y = 0 };
            var q = new Point { X = 3, Y = 4 };
            var result = ConvexHullAlgorithms.Distance(p, q);
            Assert.That(result, Is.EqualTo(5.0));
        }

        [Test]
        public void TestJarvisHullAlgorithmWithSimpleInput()
        {
            var points = new[]
            {
                new Point { X = 0, Y = 0 },
                new Point { X = 1, Y = 1 },
                new Point { X = 2, Y = 0 }
            };

            var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);
            Assert.Multiple(() =>
            {
                Assert.That(result.Points, Has.Length.EqualTo(3));
                Assert.That(result.Shape, Is.EqualTo(GeometricalShape.Triangle));
            });
        }

        [Test]
        public void TestJarvisHullAlgorithmWithFunctionSkipInput()
        {
            var points = new[]
            {
                new Point { X = 0, Y = 0 },
                new Point { X = 1, Y = 1 },
            };

            var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);
            Assert.Multiple(() =>
            {
                Assert.That(result.Points, Has.Length.EqualTo(2));
                Assert.That(result.Shape, Is.EqualTo(GeometricalShape.Line));
            });            
        }

        [Test]
        public void TestJarvisHullAlgorithmWithComplexInput()
        {
            var points = new[]
            {
                new Point { X = 0, Y = 3 },
                new Point { X = 2, Y = 2 },
                new Point { X = 1, Y = 1 },
                new Point { X = 2, Y = 1 },
                new Point { X = 3, Y = 0 },
                new Point { X = 0, Y = 0 },
                new Point { X = 3, Y = 3 }
            };

            var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);
            Assert.Multiple(() =>
            {
                Assert.That(result.Points, Has.Length.EqualTo(4));
                Assert.That(result.Shape, Is.EqualTo(GeometricalShape.Quadrilateral));
                Assert.That(result.Points, Is.EquivalentTo(new[]
                {
                    new Point { X = 0, Y = 3 },
                    new Point { X = 3, Y = 3 },
                    new Point { X = 3, Y = 0 },
                    new Point { X = 0, Y = 0 }
                }));
            });
        }

        [Test]
        public void TestJarvisHullAlgorithmWithDuplicatePoints()
        {
            var points = new[]
            {
                new Point { X = 0, Y = 0 },
                new Point { X = 0, Y = 0 },
                new Point { X = 1, Y = 1 },
                new Point { X = 1, Y = 1 },
                new Point { X = 2, Y = 0 },
                new Point { X = 2, Y = 0 }
            };

            var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);

            Assert.Multiple(() =>
            {
                Assert.That(result.Points, Has.Length.EqualTo(3));
                Assert.That(result.Shape, Is.EqualTo(GeometricalShape.Triangle));
                Assert.That(result.Points, Is.EquivalentTo(new[]
                {
                    new Point { X = 0, Y = 0 },
                    new Point { X = 2, Y = 0 },
                    new Point { X = 1, Y = 1 }
                }));
            });
        }

        [Test]
        public void TestJarvisHullAlgorithmWithSinglePoint()
        {
            var points = new[]
            {
                new Point { X = 0, Y = 0 }
            };

            var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);

            Assert.Multiple(() =>
            {
                Assert.That(result.Points, Has.Length.EqualTo(1));
                Assert.That(result.Shape, Is.EqualTo(GeometricalShape.Point));
            });
        }

        [Test]
        public void TestJarvisHullAlgorithmWithNoPoints()
        {
            var points = Array.Empty<Point>();

            var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);

            Assert.Multiple(() =>
            {
                Assert.That(result.Points, Has.Length.EqualTo(0));
                Assert.That(result.Shape, Is.EqualTo(GeometricalShape.BlankSpace));
            });
        }
    }
}
