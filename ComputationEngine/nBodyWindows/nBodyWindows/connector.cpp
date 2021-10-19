#include <vector>
#include "pch.h"
#include "NBodyEngine.cpp"

using namespace engine;

#define EXPORT extern "C" __declspec(dllexport)

typedef intptr_t ItemListHandle;

EXPORT bool timeStep(ItemListHandle* hItems, double* xpos, double* ypos, double* zpos, double** force,double* mass, int n)
{
    
    updateForce(xpos,ypos,zpos,force,mass,n);

    return true;
}
