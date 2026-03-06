using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.App
{
    public static class QuantityWeightUtility
    {
        public static bool DemonstrateWeightEquality(Weight w1, Weight w2)
        {
            return w1.Equals(w2);
        }

        public static Weight DemonstrateWeightConversion(double value, WeightUnit from, WeightUnit to)
        {
            Weight weight = new Weight(value, from);
            return weight.ConvertTo(to);
        }

        public static Weight DemonstrateWeightAddition(Weight w1, Weight w2)
        {
            return w1.Add(w2);
        }

        public static Weight DemonstrateWeightAddition(Weight w1, Weight w2, WeightUnit target)
        {
            return w1.Add(w2, target);
        }
    }
}