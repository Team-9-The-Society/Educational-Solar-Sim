// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the COMPUTATIONENGINELIBRARY_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// COMPUTATIONENGINELIBRARY_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef COMPUTATIONENGINELIBRARY_EXPORTS
#define COMPUTATIONENGINELIBRARY_API __declspec(dllexport)
#else
#define COMPUTATIONENGINELIBRARY_API __declspec(dllimport)
#endif

// Export Methods Here
extern "C" {
	COMPUTATIONENGINELIBRARY_API int updateForce(double* xPosPassed, double* yPosPassed, double* zPosPassed, double* massPassed, int numBodiesPassed);
}
