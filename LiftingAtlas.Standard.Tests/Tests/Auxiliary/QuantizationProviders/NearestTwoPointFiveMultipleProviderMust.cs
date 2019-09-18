using NUnit.Framework;

namespace LiftingAtlas.Standard.Tests
{
    [TestFixture]
    public class NearestTwoPointFiveMultipleProviderMust
    {
        /// <summary>
        /// Nearest two point five multiple provider.
        /// </summary>
        NearestTwoPointFiveMultipleProvider nearestTwoPointFiveMultipleProvider;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            nearestTwoPointFiveMultipleProvider = new NearestTwoPointFiveMultipleProvider();
        }

        /// <summary>
        /// Tests ability of <see cref="NearestTwoPointFiveMultipleProvider"/>
        /// to round to nearest multiple of 2.5.
        /// </summary>
        [Test]
        [TestCase(3.97, 5.00)]
        [TestCase(-3.97, -5.00)]
        [TestCase(3.00, 2.5)]
        [TestCase(-3.00, -2.5)]
        [TestCase(5.00, 5.00)]
        [TestCase(-5.00, -5.00)]
        [TestCase(0.00, 0.00)]
        public void RoundToNearestTwoPointFiveMultiple(double input, double expectedResult)
        {
            Assert.That(
                nearestTwoPointFiveMultipleProvider.Quantize(input),
                Is.EqualTo(expectedResult),
                "Must round to nearest multiple of 2.5."
                );
        }

        /// <summary>
        /// Tests ability of <see cref="NearestTwoPointFiveMultipleProvider"/>
        /// to round input half away from zero.
        /// </summary>
        [Test]
        [TestCase(1.25, 2.5)]
        [TestCase(-1.25, -2.5)]
        public void RoundHalfAwayFromZero(double input, double expectedResult)
        {
            Assert.That(
                nearestTwoPointFiveMultipleProvider.Quantize(input),
                Is.EqualTo(expectedResult),
                "Must round half away from zero."
                );
        }
    }
}
