using NUnit.Framework;
using System;

namespace LiftingAtlas.Standard.Tests
{
    [TestFixture]
    public class HexFirstCharLetterGuidFormatterMust
    {
        [Test]
        public void FormatGuidInSuchWaySoThatTheFirstCharacterOfHexadecimalRepresentationIsALetter()
        {
            HexFirstCharLetterGuidFormatter hexFirstCharLetterGuidFormatter = new HexFirstCharLetterGuidFormatter();

            Assert.That(
                Char.IsLetter(hexFirstCharLetterGuidFormatter.FormatGuid(Guid.Empty).ToString("N")[0]),
                Is.True,
                "First character of formatted guid must be a letter."
                );
        }
    }
}
