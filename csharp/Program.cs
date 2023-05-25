using RngLab.Rng.Generators;
using System;

namespace CulumiCs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("[i] Running Test (Reproduction)...");
            Culumi.Test.RunReproduction();
            Console.WriteLine("[i] Running Test (Characteristic)... This may take some time. Please wait.");
            Culumi.Test.RunCharacteristic();
            Console.WriteLine("[i] All tests were passed.");
        }
    }
}
