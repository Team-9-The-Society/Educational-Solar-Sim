#include "pch.h"
#include "CppUnitTest.h"
#include "../ComputationEngineLibrary/ComputationEngineLibrary.cpp"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace ComputationEngineLibraryTests
{
	TEST_CLASS(UnitTests)
	{
	public:
		TEST_METHOD(updateForceTest)
		{
			double* xPosPassed = { 0 };
			double* yPosPassed = { 0 };
			double* zPosPassed = { 0 };
			double* massPassed = { 0 };
			int numBodiesPassed = 1;
			Assert::AreEqual(0, ComputationEngineLibrary::updateForce(xPosPassed, yPosPassed, zPosPassed, massPassed, numBodiesPassed));
		}
	};
}
