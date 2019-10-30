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

        public static PlannedSet ToPlannedSet(this PlannedCycleDataset plannedCycleDataset)
        {
            if (plannedCycleDataset == null)
                throw new ArgumentNullException(nameof(plannedCycleDataset));

            SetNumber number;
            PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint;
            PlannedRepetitions plannedRepetitions;
            PlannedWeight plannedWeight;
            LiftedValues liftedValues;
            WeightAdjustmentConstant weightAdjustmentConstant;
            string note;

            number =  new SetNumber(plannedCycleDataset.setNumber);

            if (plannedCycleDataset.plannedPercentageOfReferencePointLowerBound == null ||
                plannedCycleDataset.plannedPercentageOfReferencePointUpperBound == null)
                plannedPercentageOfReferencePoint = null;
            else
                plannedPercentageOfReferencePoint =
                    new PlannedPercentageOfReferencePoint(
                        plannedCycleDataset.plannedPercentageOfReferencePointLowerBound.Value,
                        plannedCycleDataset.plannedPercentageOfReferencePointUpperBound.Value
                        );

            if (plannedCycleDataset.plannedRepetitionsLowerBound == null ||
                plannedCycleDataset.plannedRepetitionsUpperBound == null)
                plannedRepetitions = null;
            else
                plannedRepetitions =
                    new PlannedRepetitions(
                        new Repetitions(plannedCycleDataset.plannedRepetitionsLowerBound.Value),
                        new Repetitions(plannedCycleDataset.plannedRepetitionsUpperBound.Value)
                        );

            if (plannedCycleDataset.plannedWeightLowerBound == null ||
                plannedCycleDataset.plannedWeightUpperBound == null)
                plannedWeight = null;
            else
                plannedWeight =
                    new PlannedWeight(
                        new Weight(plannedCycleDataset.plannedWeightLowerBound.Value),
                        new Weight(plannedCycleDataset.plannedWeightUpperBound.Value)
                        );

            if (plannedCycleDataset.liftedRepetitions == null ||
                plannedCycleDataset.liftedWeight == null)
                liftedValues = null;
            else
                liftedValues =
                    new LiftedValues(
                        new Repetitions(plannedCycleDataset.liftedRepetitions.Value),
                        new Weight(plannedCycleDataset.liftedWeight.Value)
                        );

            if (plannedCycleDataset.weightAdjustmentConstant == null)
                weightAdjustmentConstant = null;
            else
                weightAdjustmentConstant =
                    new WeightAdjustmentConstant(
                        plannedCycleDataset.weightAdjustmentConstant.Value
                        );

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
