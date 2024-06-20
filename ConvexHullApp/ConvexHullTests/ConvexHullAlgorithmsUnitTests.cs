namespace ConvexHullApp.UnitTests
{
    [TestFixture]
    public class ConvexHullAlgorithmsUnitTests
    {
        [Test]
        public void TestOrientationCollinear()
        {
            var p = new Point(0, 0);
            var q = new Point (1, 1);
            var r = new Point (2, 2);
            var result = ConvexHullAlgorithms.Orientation(p, q, r);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void TestOrientationClockwise()
        {
            var p = new Point (0, 0);
            var q = new Point (4, 4);
            var r = new Point (1, 2);
            var result = ConvexHullAlgorithms.Orientation(p, q, r);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void TestOrientationAntiClockwise()
        {
            var p = new Point (0, 0);
            var q = new Point (1, 1);
            var r = new Point (2, 0);
            var result = ConvexHullAlgorithms.Orientation(p, q, r);
            Assert.That(result, Is.EqualTo(-1));
        }

        [Test]
        public void TestJarvisHullAlgorithmWithSimpleInput()
        {
            var points = new[]
            {
                new Point (0, 0),
                new Point (1, 1),
                new Point (2, 0)
            };

            var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);
            Assert.Multiple(() =>
            {
                Assert.That(result.Points, Has.Length.EqualTo(3));
                Assert.That(result.Shape, Is.EqualTo("Triangle"));
            });
        }

        [Test]
        public void TestJarvisHullAlgorithmWithFunctionSkipInput()
        {
            var points = new[]
            {
                new Point (0, 0),
                new Point (1, 1)     };

            var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);
            Assert.Multiple(() =>
            {
                Assert.That(result.Points, Has.Length.EqualTo(2));
                Assert.That(result.Shape, Is.EqualTo("Line Segment - Convex Hull invalid"));
            });
        }

        [Test]
        public void TestJarvisHullAlgorithmWithComplexInput()
        {
            var points = new[]
            {
                new Point (0, 3),
                new Point (2, 2),
                new Point (1, 1),
                new Point (2, 1),
                new Point (3, 0),
                new Point (0, 0),
                new Point (3, 3)
            };

            var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);
            Assert.Multiple(() =>
            {
                Assert.That(result.Points, Has.Length.EqualTo(4));
                Assert.That(result.Shape, Is.EqualTo("Quadrilateral"));
                Assert.That(result.Points, Is.EquivalentTo(new[]
                {
                    new Point (0, 3),
                    new Point (3, 3),
                    new Point (3, 0),
                    new Point (0, 0)
                }));
            });
        }

        [Test]
        public void TestJarvisHullAlgorithmWithDuplicatePoints()
        {
            var points = new[]
            {
                new Point (0, 0),
                new Point (0, 0),
                new Point (1, 1),
                new Point (1, 1),
                new Point (2, 0),
                new Point (2, 0)
            };

            var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);

            Assert.Multiple(() =>
            {
                Assert.That(result.Points, Has.Length.EqualTo(3));
                Assert.That(result.Shape, Is.EqualTo("Triangle"));
                Assert.That(result.Points, Is.EquivalentTo(new[]
                {
                    new Point (0, 0),
                    new Point (2, 0),
                    new Point (1, 1)
                }));
            });
        }

        [Test]
        public void TestJarvisHullAlgorithmWithSinglePoint()
        {
            var points = new[]
            {
                new Point (0, 0)
            };

            var result = ConvexHullAlgorithms.JarvisHullAlgorithm(points);

            Assert.Multiple(() =>
            {
                Assert.That(result.Points, Has.Length.EqualTo(1));
                Assert.That(result.Shape, Is.EqualTo("Point - Convex Hull invalid"));
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
                Assert.That(result.Shape, Is.EqualTo("Blank - Convex Hull invalid"));
            });
        }

        [Test]
        public void TestRemoveDuplicates()
        {
            var points = new[]
{
                new Point (0, 0),
                new Point (0, 0),
                new Point (1, 1),
                new Point (1, 1),
                new Point (2, 0),
                new Point (2, 0),
                new Point (2, 1),
                new Point (0, 0)
            };

            var result = ConvexHullAlgorithms.RemoveDuplicates(points);

            Assert.Multiple(
                () =>
                {
                    Assert.That(result, Has.Length.EqualTo(4));
                    Assert.That(result, Is.EquivalentTo(new[]
                    {
                        new Point (0, 0),
                        new Point (2, 0),
                        new Point (1, 1),
                        new Point (2, 1),
                    })
                    );
                }
            );
        }
    }
}
