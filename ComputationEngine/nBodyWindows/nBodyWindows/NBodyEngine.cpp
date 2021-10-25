#include <stdio.h>
#include <stdlib.h>

#include "pch.h"
#include "framework.h"
#include "NBodyEngine.h"

#include <math.h>


	//Defines the gravitational constant in m^3kg^-1s^-2
	
	/*
	Parameters: array of x positions, y positions, z positions, double array of forces (nested array is x,y,z), array of masses, int of number of bodies.
	Method Purpose: calculate the force being enacted on each body and modify the array that is passed.
	Output: 1 if successful, 0 if unsuccessful. the real output is the modified double array of forces that is passed in and out.
	*/

	int updateForce(double* xPos, double* yPos, double* zPos, double** force, double* mass, int numBodies)
	{	
		double gravitationalConstant = 6.67408 * pow(10, -11);
		//Defines distance in astronomical units in meters (the distance from the Earth to the Sun)
		double astronomicalUnit = 149597870700;

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
				distance = (xPos[i] - xPos[j]) * astronomicalUnit;
				if (distance > 0)
				{
					accel[0] += -1 * (gravitationalConstant * mass[j]) / pow(distance, 2);
				}
				else if (distance < 0)
				{
					accel[0] += (gravitationalConstant * mass[j]) / pow(distance, 2);
				}

				//Y coordinates
				distance = (yPos[i] - yPos[j]) * astronomicalUnit;
				if (distance > 0)
				{
					accel[1] += -1 * (gravitationalConstant * mass[j]) / pow(distance, 2);
				}
				else if (distance < 0)
				{
					accel[1] += (gravitationalConstant * mass[j]) / pow(distance, 2);
				}

				//Z coordinates
				distance = (zPos[i] - zPos[j]) * astronomicalUnit;
				if (distance > 0)
				{
					accel[2] += -1 * (gravitationalConstant * mass[j]) / pow(distance, 2);
				}
				else if (distance < 0)
				{
					accel[2] += (gravitationalConstant * mass[j]) / pow(distance, 2);
				}
			}
			//Convert the acceleration into forces, populate the force array
			for (int j = 0; j < numBodies; j++)
			{
				force[j][0] = accel[0] * mass[j];
				force[j][1] = accel[1] * mass[j];
				force[j][2] = accel[2] * mass[j];
			}
		}

		return 1;
	}
