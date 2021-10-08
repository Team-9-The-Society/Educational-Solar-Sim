// CppLibrary.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "CppLibrary.h"
#include <iostream>

bool testConnection() { 
	return true; 
}

int getRandom() { 
	return rand(); 
}

int displaySum(int a, int b) { 
	return a+b; 
}