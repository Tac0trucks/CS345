using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mamdani_Fuzzy_Inference_System
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("  Fuzzy Logic Controller: Mamdani vs. Sugeno (.NET Framework)  ");
            Console.WriteLine("===============================================================");

            // 1. CRISP INPUTS
            double temperature = 32.5; // °C
            double humidity = 80.0;    // %
            Console.WriteLine($"\n[Inputs] Temperature: {temperature}°C | Humidity: {humidity}%");

            // 2. FUZZIFICATION (Shared Input Memberships)
            double tempLow = TriangularMembership(temperature, 10, 15, 20);
            double tempMed = TriangularMembership(temperature, 15, 25, 35);
            double tempHigh = TriangularMembership(temperature, 30, 35, 40);

            double humLow = TriangularMembership(humidity, 0, 25, 50);
            double humHigh = TriangularMembership(humidity, 40, 75, 100);

            Console.WriteLine("\n--- 1. Fuzzification Results ---");
            Console.WriteLine($"  Temp      -> Low: {tempLow:F2} | Med: {tempMed:F2} | High: {tempHigh:F2}");
            Console.WriteLine($"  Humidity  -> Low: {humLow:F2} | High: {humHigh:F2}");

            // 3. RULE EVALUATION (Shared Rule Firing Strengths)
            // Rule 1: IF Temp is High OR Humidity is High  -> Fan is Fast
            // Rule 2: IF Temp is Medium AND Humidity is Low -> Fan is Medium
            // Rule 3: IF Temp is Low                        -> Fan is Slow
            double rule1_strength = Math.Max(tempHigh, humHigh); // Fast
            double rule2_strength = Math.Min(tempMed, humLow);   // Medium
            double rule3_strength = tempLow;                    // Slow

            Console.WriteLine("\n--- 2. Rule Firing Strengths ---");
            Console.WriteLine($"  Rule 1 (Fast Fan)   : {rule1_strength:F2}");
            Console.WriteLine($"  Rule 2 (Medium Fan) : {rule2_strength:F2}");
            Console.WriteLine($"  Rule 3 (Slow Fan)   : {rule3_strength:F2}");

            // 4. MAMDANI DEFUZZIFICATION (Continuous Integration / Centroid)
            double mamdaniOutput = ComputeMamdaniCentroid(rule1_strength, rule2_strength, rule3_strength);

            // 5. SUGENO DEFUZZIFICATION (Zero-Order Singleton / Weighted Average)
            double sugenoOutput = ComputeSugenoWeightedAverage(rule1_strength, rule2_strength, rule3_strength);

            // 6. COMPARISON SUMMARY
            Console.WriteLine("\n===============================================================");
            Console.WriteLine("                     FINAL RESULTS SUMMARY                     ");
            Console.WriteLine("===============================================================");
            Console.WriteLine($"  Mamdani (True Centroid / CoG) : {mamdaniOutput:F2}% Fan Speed");
            Console.WriteLine($"  Sugeno  (Weighted Singleton)  : {sugenoOutput:F2}% Fan Speed");
            Console.WriteLine($"  Variance                      : {Math.Abs(mamdaniOutput - sugenoOutput):F2}%");
            Console.WriteLine("===============================================================");

            Console.WriteLine("\nPress any key to exit...");
            Console.Read();
        }

        // --- MAMDANI CENTROID METHOD ---
        static double ComputeMamdaniCentroid(double r1Fast, double r2Med, double r3Slow)
        {
            double sumNumerator = 0.0;
            double sumDenominator = 0.0;
            double step = 0.5; // Discretization step

            for (double y = 0.0; y <= 100.0; y += step)
            {
                // Consequent fuzzy sets
                double outSlow = TriangularMembership(y, 0.0, 0.0, 50.0);
                double outMed = TriangularMembership(y, 20.0, 50.0, 80.0);
                double outFast = TriangularMembership(y, 50.0, 100.0, 100.0);

                // Min Implication (Clipping)
                double clippedSlow = Math.Min(r3Slow, outSlow);
                double clippedMed = Math.Min(r2Med, outMed);
                double clippedFast = Math.Min(r1Fast, outFast);

                // Max Aggregation
                double aggregatedY = Math.Max(clippedSlow, Math.Max(clippedMed, clippedFast));

                // Numerical integration: Integral(y * mu(y)) / Integral(mu(y))
                sumNumerator += y * aggregatedY * step;
                sumDenominator += aggregatedY * step;
            }

            return sumDenominator > 0.0 ? (sumNumerator / sumDenominator) : 0.0;
        }

        // --- SUGENO WEIGHTED AVERAGE METHOD ---
        static double ComputeSugenoWeightedAverage(double r1Fast, double r2Med, double r3Slow)
        {
            // Zero-order Sugeno singletons (crisp target constants)
            double cSlow = 20.0;
            double cMed = 60.0;
            double cFast = 100.0;

            double numerator = (r3Slow * cSlow) + (r2Med * cMed) + (r1Fast * cFast);
            double denominator = r3Slow + r2Med + r1Fast;

            return denominator > 0.0 ? (numerator / denominator) : 0.0;
        }

        // --- MEMBERSHIP FUNCTION ---
        static double TriangularMembership(double x, double a, double b, double c)
        {
            if (x <= a || x >= c)
                return 0.0;

            if (x == b)
                return 1.0;

            if (x > a && x < b)
                return (x - a) / (b - a);

            return (c - x) / (c - b);
        }
    
    }
}
