#include <stdio.h>
#include <stdlib.h>
#include "pch.h"
#include "framework.h"
#include "ComputationEngineLibrary.h"
#include <math.h>
#include <iostream>

namespace ComputationEngineLibrary
{
	using namespace std;
	/*
	Parameters: Array pointers are given of values corresponding to objects by index
	Method Purpose: Calculates force and update the array
	Output: 1 for succes, 0 for failure. the "real" output is the modified memory at the location of the force pointer
	*/
	int updateForce(double *xPos, double *yPos, double *zPos, double **force, double *mass, int numBodies)
	{
		//Defines the gravitational constant in m^3kg^-1s^-2
		double gravitationalConstant = 6.67408 * pow(10, -11);
		//Defines distance in astronomical units in meters (the distance from the Earth to the Sun)
		double astronomicalUnit = 149597870700;
		double distance = 0;

		for(int i = 0; i < numBodies; i++)
		{
			double accel[3] = {0, 0, 0};
			for(int j = 0; j < numBodies; j++)
			{
				if(i == j)
				{
					continue;
				}
				//X coordinates
				distance = (xPos[i] - xPos[j])*astronomicalUnit;
				if(distance > 0)
				{
					force[j][0] += -1 * (gravitationalConstant * mass[i] * mass[j])/pow(distance, 2);
				}
				else if(distance < 0)
				{
					force[j][0] += (gravitationalConstant * mass[i] * mass[j])/pow(distance, 2);
				}
				
				//Y coordinates
				distance = (yPos[i] - yPos[j])*astronomicalUnit;
				if(distance > 0)
				{
					force[j][1] += -1 * (gravitationalConstant * mass[i] * mass[j])/pow(distance, 2);
				}
				else if(distance < 0)
				{
					force[j][1] += (gravitationalConstant * mass[i] * mass[j])/pow(distance, 2);
				}
				
				//Z coordinates
				distance = (zPos[i] - zPos[j])*astronomicalUnit;
				if(distance > 0)
				{
					force[j][2] += -1 * (gravitationalConstant * mass[i] * mass[j])/pow(distance, 2);
				}
				else if(distance < 0)
				{
					force[j][2] += (gravitationalConstant * mass[i] * mass[j])/pow(distance, 2);
				}			
			}
		}
		
		return 1;	
	}
}