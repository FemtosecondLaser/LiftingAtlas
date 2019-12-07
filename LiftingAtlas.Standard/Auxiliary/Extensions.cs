using System;
using System.Collections;
using System.Text;

namespace LiftingAtlas.Standard
{
    public static class Extensions
    {
        public static bool ListEquals(this IList thisList, IList otherList)
        {
            if (thisList is null)
                return otherList is null;

            if (otherList is null)
                return false;

            if (ReferenceEquals(thisList, otherList))
                return true;

            if (thisList.Count != otherList.Count)
                return false;

            if (thisList.Count == 0)
                return true;

            for (int i = 0; i < thisList.Count; i++)
                if (!thisList[i].Equals(otherList[i]))
                    return false;

            return true;
        }

        public static char DecimalDigitToSuperscriptEquivalent(this char decimalDigit)
        {
            if (!char.IsDigit(decimalDigit))
                throw new ArgumentException(
                    "Character is not a decimal digit.",
                    nameof(decimalDigit)
                    );

            switch (decimalDigit)
            {
                case '0':
                    return '⁰';

                case '1':
                    return '¹';

                case '2':
                    return '²';

                case '3':
                    return '³';

                case '4':
                    return '⁴';

                case '5':
                    return '⁵';

                case '6':
                    return '⁶';

                case '7':
                    return '⁷';

                case '8':
                    return '⁸';

                case '9':
                    return '⁹';

                default:
                    throw new NotImplementedException(
                        "Unexpected decimal digit. No superscript equivalent defined."
                        );
            }
        }

        public static string DecimalDigitStringToSuperscriptEquivalent(this string decimalDigitString)
        {
            if (decimalDigitString == null)
                throw new ArgumentNullException(nameof(decimalDigitString));

            foreach (char character in decimalDigitString)
                if (!char.IsDigit(character))
                    throw new ArgumentException(
                        "String does not consist only of decimal digits.",
                        nameof(decimalDigitString)
                        );

            StringBuilder decimalDigitStringSuperscriptEquivalentBuilder =
                new StringBuilder(decimalDigitString.Length);

            foreach (char character in decimalDigitString)
                decimalDigitStringSuperscriptEquivalentBuilder.Append(
                    character.DecimalDigitToSuperscriptEquivalent()
                    );

            return decimalDigitStringSuperscriptEquivalentBuilder.ToString();
        }
    }
}
