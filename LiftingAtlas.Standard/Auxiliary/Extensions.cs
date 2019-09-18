using System;
using System.Collections;
using System.Text;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Determines equality of <see cref="IList"/>s.
        /// </summary>
        /// <param name="thisList">This list.</param>
        /// <param name="otherList">Other list.</param>
        /// <returns>True if operands are equal;
        /// otherwise, false.</returns>
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

        /// <summary>
        /// Returns superscript equivalent of <paramref name="decimalDigit"/>.
        /// </summary>
        /// <param name="decimalDigit">Decimal digit,
        /// to return superscript equivalent of. Must be a decimal digit.</param>
        /// <returns>Superscript equivalent of <paramref name="decimalDigit"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="decimalDigit"/>
        /// is not a decimal digit.</exception
        /// <exception cref="NotImplementedException"><paramref name="decimalDigit"/>
        /// is unexpected decimal digit with no superscript equivalent defined.</exception>
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

        /// <summary>
        /// Returns superscript equivalent of <paramref name="decimalDigitString"/>.
        /// </summary>
        /// <param name="decimalDigitString">Decimal digit string,
        /// to return superscript equivalent of. Must consist only of decimal digits.</param>
        /// <returns>Superscript equivalent of <paramref name="decimalDigitString"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="decimalDigitString"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="decimalDigitString"/>
        /// does not consist only of decimal digits.</exception>
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

        /// <summary>
        /// Creates an instance of <see cref="PlannedSet"/>
        /// based on an instance of <see cref="PlannedCycleDataset"/>.
        /// </summary>
        /// <param name="plannedCycleDataset">Planned cycle dataset. Must not be null.</param>
        /// <returns>Planned set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="plannedCycleDataset"/> is null.</exception>
        public static PlannedSet ToPlannedSet(this PlannedCycleDataset plannedCycleDataset)
        {
            if (plannedCycleDataset == null)
                throw new ArgumentNullException(nameof(plannedCycleDataset));

            int number;
            NonNegativeI32Range plannedPercentageOfReferencePoint;
            NonNegativeI32Range plannedRepetitions;
            NonNegativeDBLRange plannedWeight;
            (int liftedRepetitions, double liftedWeight)? liftedValues;
            double? weightAdjustmentConstant;
            string note;

            number = plannedCycleDataset.setNumber;

            if (plannedCycleDataset.plannedPercentageOfReferencePointLowerBound == null ||
                plannedCycleDataset.plannedPercentageOfReferencePointUpperBound == null)
                plannedPercentageOfReferencePoint = null;
            else
                plannedPercentageOfReferencePoint =
                    new NonNegativeI32Range(
                        plannedCycleDataset.plannedPercentageOfReferencePointLowerBound.Value,
                        plannedCycleDataset.plannedPercentageOfReferencePointUpperBound.Value
                        );

            if (plannedCycleDataset.plannedRepetitionsLowerBound == null ||
                plannedCycleDataset.plannedRepetitionsUpperBound == null)
                plannedRepetitions = null;
            else
                plannedRepetitions =
                    new NonNegativeI32Range(
                        plannedCycleDataset.plannedRepetitionsLowerBound.Value,
                        plannedCycleDataset.plannedRepetitionsUpperBound.Value
                        );

            if (plannedCycleDataset.plannedWeightLowerBound == null ||
                plannedCycleDataset.plannedWeightUpperBound == null)
                plannedWeight = null;
            else
                plannedWeight =
                    new NonNegativeDBLRange(
                        plannedCycleDataset.plannedWeightLowerBound.Value,
                        plannedCycleDataset.plannedWeightUpperBound.Value
                        );

            if (plannedCycleDataset.liftedRepetitions == null ||
                plannedCycleDataset.liftedWeight == null)
                liftedValues = null;
            else
                liftedValues = (
                    plannedCycleDataset.liftedRepetitions.Value,
                    plannedCycleDataset.liftedWeight.Value
                    );

            weightAdjustmentConstant = plannedCycleDataset.weightAdjustmentConstant;

            note = plannedCycleDataset.note;

            return new PlannedSet(
                number,
                plannedPercentageOfReferencePoint,
                plannedRepetitions,
                plannedWeight,
                liftedValues,
                weightAdjustmentConstant,
                note
                );
        }
    }
}
