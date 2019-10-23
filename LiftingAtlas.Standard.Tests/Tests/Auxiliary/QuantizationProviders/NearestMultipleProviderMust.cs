using NUnit.Framework;

namespace LiftingAtlas.Standard.Tests
{
    [TestFixture]
    public class NearestMultipleProviderMust
    {
        /// <summary>
        /// Nearest multiple provider.
        /// </summary>
        NearestMultipleProvider nearestMultipleProvider;

        /// <summary>
        /// Tests ability of <see cref="NearestMultipleProvider"/>
        /// to round to nearest multiple.
        /// </summary>
        [Test]
        [TestCase(3.97, 2.50, 5.00)]
        [TestCase(-3.97, 2.50, -5.00)]
        [TestCase(3.00, 2.50, 2.50)]
        [TestCase(-3.00, 2.50, -2.50)]
        [TestCase(5.00, 2.50, 5.00)]
        [TestCase(-5.00, 2.50, -5.00)]
        [TestCase(0.00, 2.50, 0.00)]
        [TestCase(8.33, 1.00, 8.00)]
        [TestCase(-8.33, 1.00, -8.00)]
        [TestCase(9.67, 1.00, 10.00)]
        [TestCase(-9.67, 1.00, -10.00)]
        [TestCase(33.00, 1.00, 33.00)]
        [TestCase(-33.00, 1.00, -33.00)]
        public void RoundToNearestMultiple(double value, double uniformQuantizationInterval, double expectedResult)
        {
            nearestMultipleProvider =
                new NearestMultipleProvider(
                    new UniformQuantizationInterval(uniformQuantizationInterval)
                    );

            Assert.That(
                nearestMultipleProvider.Quantize(value),
                Is.EqualTo(expectedResult),
                "Must round to nearest multiple."
                );
        }

        /// <summary>
        /// Tests ability of <see cref="NearestMultipleProvider"/>
        /// to round half away from zero.
        /// </summary>
        [Test]
        [TestCase(1.25, 2.50, 2.50)]
        [TestCase(-1.25, 2.50, -2.50)]
        public void RoundHalfAwayFromZero(double value, double uniformQuantizationInterval, double expectedResult)
        {
            nearestMultipleProvider =
                new NearestMultipleProvider(
                    new UniformQuantizationInterval(uniformQuantizationInterval)
                    );

            Assert.That(
                nearestMultipleProvider.Quantize(value),
                Is.EqualTo(expectedResult),
                "Must round half away from zero."
                );
        }
    }
}
