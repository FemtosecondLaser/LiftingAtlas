using NUnit.Framework;
using System;

namespace LiftingAtlas.Standard.Tests
{
    [TestFixture]
    public class HexFirstCharLetterGuidFormatterMust
    {
        /// <summary>
        /// Tests ability of <see cref="HexFirstCharLetterGuidFormatter"/>
        /// to format guid in such way so that
        /// the first character of hexadecimal representation is a letter.
        /// </summary>
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
