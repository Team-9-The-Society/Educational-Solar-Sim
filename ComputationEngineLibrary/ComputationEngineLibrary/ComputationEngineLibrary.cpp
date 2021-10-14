#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include "pch.h"
#include "framework.h"
#include "ComputationEngineLibrary.h"
#include <math.h>
#include <vector>
#include <iostream>

namespace ComputationEngineLibrary
{
	using namespace std;
	//Defines the gravitational constant in m^3kg^-1s^-2
	double gravitationalConstant = 6.67408 * pow(10, -11);
	//Defines distance in astronomical units in meters (the distance from the Earth to the Sun)
	double astronomicalUnit = 149597870700;
	/*
	Parameters:
	Method Purpose:
	Output:
	*/
	int updateForce(double* xPosPassed, double* yPosPassed, double* zPosPassed, double* massPassed, int numBodiesPassed)
	{
		const int numBodies = numBodiesPassed;
		vector<double> xPos;
		vector<double> yPos;
		vector<double> zPos;
		vector<double> mass;

		for (int i = 0; i < numBodies; i++)
		{
			xPos[i] = xPosPassed[i];
			yPos[i] = yPosPassed[i];
			zPos[i] = zPosPassed[i];
		}

		vector<vector<double>> force;
		double distance = 0;

		for (int i = 0; i < numBodies; i++)
		{
			double accel[3] = { 0, 0, 0 };
			for (int j = 0; j < numBodies; j++)
			{
				if (i == j)
				{
					continue;
				}
				//X coordinates
				distance = (xPos[i] - xPos[i]);
				if (distance > 0)
				{
					accel[0] += -1 * (gravitationalConstant * mass[j]) / pow(distance, 2);
				}
				else if (distance < 0)
				{
					accel[0] += (gravitationalConstant * mass[j]) / pow(distance, 2);
				}

				//Y coordinates
				distance = (yPos[i] - yPos[j]);
				if (distance > 0)
				{
					accel[1] += -1 * (gravitationalConstant * mass[j]) / pow(distance, 2);
				}
				else if (distance < 0)
				{
					accel[1] += (gravitationalConstant * mass[j]) / pow(distance, 2);
				}

				//Z coordinates
				distance = (zPos[i] - zPos[i]);
				if (distance > 0)
				{
					accel[2] += -1 * (gravitationalConstant * mass[j]) / pow(distance, 2);
				}
				else if (distance < 0)
				{
					accel[2] += (gravitationalConstant * mass[j]) / pow(distance, 2);
				}
			}
			for (int j = 0; j < numBodies; j++)
			{
				force[j][0] = accel[0] * mass[j];
				force[j][1] = accel[1] * mass[j];
				force[j][2] = accel[2] * mass[j];
			}
			//free(accel);
		}

		for (int i = 0; i < numBodies; i++)
		{
			printf("Body %d forces (x, y, z): %f, %f, %f\n", i, force[i][0], force[i][1], force[i][2]);
		}

		return 0;
	}
}