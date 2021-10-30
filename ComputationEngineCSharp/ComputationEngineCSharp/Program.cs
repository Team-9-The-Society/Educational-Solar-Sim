using System;

namespace ComputationEngineCSharp
{
    class Program
    {
        static void Main()
        {
            NBody nBody = new NBody();

            // Mass 1 (Sun) at (0, 0, 0) is 1.989E30 kg
            // Mass 2 (Earth) at (1 AU, 0, 0) is 5.972E24 kg
            double[,] PosPass = new double[2, 3] { { 0, 0, 0 }, { 1, 0, 0 } };
            double[] massPass = new double[2] { 1.989 * Math.Pow(10, 30), 5.972 * Math.Pow(10, 24) };

            nBody.UpdateForce(PosPass, massPass, 2);
        }
    }
}
