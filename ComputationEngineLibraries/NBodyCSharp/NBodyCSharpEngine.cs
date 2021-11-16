﻿using System;
using System.Diagnostics;

namespace ComputationEngineCSharp
{
    public class NBody
    {
        //Defines the gravitational constant in m^3/(kg*s^2)
        public double GravitationalConstant { get; } = 6.67408 * Math.Pow(10, -11);

		public double[,] UpdateForce(double[,] Pos, double[] mass, int numBodies, string mode = "Meters")
		{
			for (int i = 0; i < numBodies; i++)
            {
				if (mass[i] <= 0)
                {
					throw new ArgumentException("Mass can not be 0 or negative");
				}
            }

			//Pos is stored as x, y, z
			switch (mode)
            {
				case "Meters":
					break;
				case "AU":
                    for (int i = 0; i < numBodies; i++)
                    {
						for (int j = 0; j < 3; j++)
                        {
							Pos[i, j] = AuToMeters(Pos[i, j]);
                        }
                    }
					break;
            }

			double[,] force = new double[numBodies, 3];

			for (int i = 0; i < numBodies; i++)
			{
				for (int j = 0; j < numBodies/2 && i!=j; j++)
				{
					double distanceSquared = 0;
					for (int k=0; k<3; k++)
                    {
						distanceSquared += Math.Pow(Pos[i, k] - Pos[j, k], 2);
					}
					double distance = Math.Sqrt(distanceSquared);
					Debug.WriteLine("\n");
					Debug.WriteLine($"Distance between body {i} at ({Pos[i, 0]},{Pos[i, 1]},{Pos[i, 2]}) and body {j} at ({Pos[j, 0]},{Pos[j, 1]},{Pos[j, 2]}) is {distance}");

					if (distance == 0)
                    {
						for (int k = 0; k < 3; k++)
                        {
							force[i, k] = force[j, k] = 0;
						}
                    }
                    else
                    {
						double totalForce = GravitationalConstant * (mass[i] * mass[j]) / Math.Pow(distance, 2);
						Debug.WriteLine("\n");
						Debug.WriteLine($"Magnitude of the total force between body {i} at ({Pos[i, 0]},{Pos[i, 1]},{Pos[i, 2]}) and body {j} at ({Pos[j, 0]},{Pos[j, 1]},{Pos[j, 2]}) is {totalForce}");

						int iDir;
						_ = distance > 0 ? iDir = -1 : iDir = 1;

						for (int k = 0; k < 3; k++)
						{
							double diffrence = Pos[i, k] - Pos[j, k];

							force[i, k] += RoundToSignificantDigits(iDir * totalForce * (diffrence/distance), 15);
							force[j, k] += -1 * force[i, k];
						}
					}
				}
			}

			Debug.WriteLine("\n");
			for (int i = 0; i < numBodies; i++)
			{
				Debug.WriteLine($"Body {i} at ({Pos[i, 0]}, {Pos[i, 1]}, {Pos[i, 2]}) forces (x, y, z): {force[i, 0]}, {force[i, 1]}, {force[i, 2]}");
			}

			return force;
		}

		private double AuToMeters(double Au)
        {
			return Au * 149597870691;
		}

		public double RoundToSignificantDigits(double d, int digits)
		{
			if (d == 0)
				return 0;

			double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
			return scale * Math.Round(d / scale, digits);
		}
	}
}
