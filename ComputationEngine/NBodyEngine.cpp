#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <vector>
#include <iostream>

using namespace std;
//Defines the gravitational constant in m^3kg^-1s^-2
double gravitationalConstant = 6.67408 * pow(10, -11);
//Defines distance in astronomical units in meters (the distance from the Earth to the Sun)
double astronomicalUnit = 149597870700;
class BodyInfo
{
	private:
		int id;
		double mass;
		double radius;
		double position[3];
		double velocity[3];
		double acceleration[3];
		double force[3];
	public:
		BodyInfo(int passedId, double passedMass, double passedRadius, double passedPosition[3], double passedVelocity[3], double passedAcceleration[3], double passedForce[3])
		{
			id = passedId;
			mass = passedMass;
			radius = passedRadius;
			for(int i = 0; i < 3; i++)
			{
				position[i] = passedPosition[i] * astronomicalUnit;
				velocity[i] = passedVelocity[i];
				acceleration[i] = passedAcceleration[i];
				force[i] = passedForce[i];
			}
		}
		int getId()
		{
			return id;
		}
		void setId(int passedId)
		{
			id = passedId;
		}
		double getMass()
		{
			return mass;
		}
		void setMass(double passedMass)
		{
			mass = passedMass;
		}
		double getRadius()
		{
			return radius;
		}
		void setRadius(double passedRadius)
		{
			radius = passedRadius;
		}
		double* getPosition()
		{
			return position;
		}
		void setPosition(double passedPosition[3])
		{
			for(int i = 0; i<3; i++)
			{
				position[i] = passedPosition[i];
			}
		}
		double* getVelocity()
		{
			return velocity;
		}
		void setVelocity(double passedVelocity[3])
		{
			for(int i =0; i<3; i++)
			{
				velocity[i] = passedVelocity[i];
			}
		}
		double* getAcceleration()
		{
			return acceleration;
		}
		void setAcceleration(double passedAcceleration[3])
		{
			for(int i=0; i<3; i++)
			{
				acceleration[i] = passedAcceleration[i];
			}
		}
		double* getForce()
		{
			return force;
		}
		void setForce(double passedForce[3])
		{
			for(int i=0; i<3; i++)
			{
				force[i] = passedForce[i];
			}
		}
		void toString()
		{
			/*
			printf("ID: %d\n", id);
			printf("Mass: %e\n", mass);
			printf("Radius: %e\n", radius);
			printf("Position: ");
			for(int i=0; i<3; i++)
			{
				printf("%e ", position[i]);
			}
			printf("\nVelocity: ");
			for(int i=0; i<3; i++)
			{
				printf("%e ", velocity[i]);
			}
			printf("\nAcceleration: ");
			for(int i=0; i<3; i++)
			{
				printf("%e ", acceleration[i]);
			}
			*/
			printf("\nForce:");
			for(int i=0; i<3; i++)
			{
				printf("%e ", (force[i]));
			}
			printf("\n");
		}	
};

vector<BodyInfo> globalBodies; 
vector<double> xPos;
vector<double> yPos;
vector<double> zPos;
vector<double> mass;
vector<vector<double>> force;
/*
Parameters: None required, it will just update the global vector of bodies
Method Purpose: When called, the method will update all the bodies acceleration in each cordinate plane based only on positions 
Output: The changes in the acceleration will be updated directly to the bodies acceleration component 
*/
void updateAccel()
{
	for(int i = 0; i < globalBodies.size(); i++)
	{
		double accel[3] = {0, 0, 0};
		for(int j = 0; j < globalBodies.size(); j++)
		{
			if(i == j)
			{
				continue;
			}
			double distance = 0;
			for(int k = 0; k < 3; k++)
			{
				distance = (globalBodies.at(i).getPosition()[k] - globalBodies.at(j).getPosition()[k]);
				if(distance > 0)
				{
					accel[k] += -1 * (gravitationalConstant * globalBodies.at(j).getMass())/pow(distance, 2);
				}
				else if(distance < 0)
				{
					accel[k] += (gravitationalConstant * globalBodies.at(j).getMass())/pow(distance, 2);
				}
				else
				{
					continue;
				}
			}
		}
		globalBodies.at(i).setAcceleration(accel);
	}
}
void updateAccel2()
{
	double distance = 0;
	for(int i = 0; i < mass.size(); i++)
	{
		double accel[3] = {0, 0, 0};
		for(int j = 0; j < mass.size(); j++)
		{
			if(i == j)
			{
				continue;
			}
			//X coordinates
			distance = (xPos.at(i) - xPos.at(j));
			if(distance > 0)
			{
				accel[0] += -1 * (gravitationalConstant * mass.at(j))/pow(distance, 2);
			}
			else if(distance < 0)
			{
				accel[0] += (gravitationalConstant * mass.at(j))/pow(distance, 2);
			}
			
			//Y coordinates
			distance = (yPos.at(i) - yPos.at(j));
			if(distance > 0)
			{
				accel[1] += -1 * (gravitationalConstant * mass.at(j))/pow(distance, 2);
			}
			else if(distance < 0)
			{
				accel[1] += (gravitationalConstant * mass.at(j))/pow(distance, 2);
			}
			
			//Z coordinates
			distance = (zPos.at(i) - zPos.at(j));
			if(distance > 0)
			{
				accel[2] += -1 * (gravitationalConstant * mass.at(j))/pow(distance, 2);
			}
			else if(distance < 0)
			{
				accel[2] += (gravitationalConstant * mass.at(j))/pow(distance, 2);
			}			
		}
		for(int j = 0; j < mass.size(); j++){
			force[j][0] = xPos.at(j) * mass.at(j);
			force[j][1] = yPos.at(j) * mass.at(j);
			force[j][2] = zPos.at(j) * mass.at(j);
		}
		//free(accel);
	}
	
}
/*
Parameters: None required, it will just update the global vector of bodies
Method Purpose: When called, the method will update all the bodies velocity in each cordinate plane based on the updated aacceleration and change in time 
Output: The changes in the velocity will be updated directly to the bodies velocity component 
*/
void updateVelc(double deltaTime)
{
	for(int i=0; i<globalBodies.size(); i++)
	{
		double newVelocity[3] = {0,0,0};
		for(int j=0; j<3; j++)
		{
			newVelocity[j] = globalBodies.at(i).getVelocity()[j] + globalBodies.at(i).getAcceleration()[j] * deltaTime;
		}
		globalBodies.at(i).setVelocity(newVelocity);
	}
}
/*
Parameters: None required, it will just update the global vector of bodies
Method Purpose: When called, the method will update all the bodies velocity in each cordinate plane based on the updated velocity and change in time 
Output: The changes in the positions will be updated directly to the bodies postions component 
*/
void updatePos(double deltaTime)
{
	for(int i=0; i<globalBodies.size(); i++)
	{
		double newPosition[3] = {0,0,0};
		for(int j=0; j<3; j++)
		{
			double initPos = globalBodies.at(i).getPosition()[j];
			double vel = globalBodies.at(i).getVelocity()[j];
			double accel = globalBodies.at(i).getAcceleration()[j];
			newPosition[j] = initPos + vel*deltaTime + 0.5*accel*pow(deltaTime, 2);
		}
		globalBodies.at(i).setPosition(newPosition);
	}
}
/*
Parameters: None required, it will just multiply acceleration by mass, which are already calculated
Method Purpose: When called, the method iterates through each body and updates their forces, only run this after all other calculations for the bodies have been ran
Output: The changes in the forces will be updated directly to the bodies forces component
*/
void updateForce()
{
	for(int i=0; i<globalBodies.size(); i++)
	{
		double force[3] = {0,0,0};
		for(int j=0; j<3; j++){
			force[j] = globalBodies.at(i).getMass() * globalBodies.at(i).getAcceleration()[j];
		}
		globalBodies.at(i).setForce(force);
	}
}

//Main method is only for testing purposes 
int main(int argc, char **argv)
{
	
	double pos1[3] = {0,0,0};
	double pos2[3] = {1,0,0};
	double vel1[3] = {0,0,0};
	double vel2[3] = {0,29780,0};
	double acl1[3] = {2000,500000,0};
	double acl2[3] = {300,5000,0};
	double frc1[3] = {0, 0, 0};
	BodyInfo arg(1,(1.9885 * pow(10,30)),50.2,pos1,vel1,acl1,frc1);
	BodyInfo arg1(2,(5.97237 * pow(10,24)),3.2,pos2,vel2,acl2,frc1);
	globalBodies.push_back(arg);
	globalBodies.push_back(arg1);
	
	int time = 2.828 * pow(10,6);
	for(int j =0; j<time; j+=3600)
	{
		updateAccel();
		//updateVelc(3600);
		//updatePos(3600);
		updateForce();
	}
	for(int i=0; i<globalBodies.size(); ++i)
		{
			globalBodies.at(i).toString();
		}
	printf("---------------------------------------\n");
	
	char *eptr;
	int numBodies = strtol(argv[5], &eptr, 10);
	
	printf("%f\n", argv[1][1]);
	printf("%f\n", argv[1][2]);
	printf("%f\n", argv[2][1]);
	printf("%f\n", argv[2][2]);
	
	for (int i = 0; i < numBodies; i++){
		xPos.push_back(argv[1][i]);
		yPos.push_back(argv[2][i]);
		zPos.push_back(argv[3][i]);
		mass.push_back(argv[4][i]);
	}
	updateAccel2;
	for(int i = 0; i < numBodies; i++){
		printf("Body %d forces (x, y, z): %f, %f, %f\n", i, force[i][0], force[i][1], force[i][2]);
	}
	
	
	return 0;
}
