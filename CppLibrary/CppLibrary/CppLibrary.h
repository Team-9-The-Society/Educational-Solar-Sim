// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the CPPLIBRARY_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// CPPLIBRARY_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef CPPLIBRARY_EXPORTS
#define CPPLIBRARY_API __declspec(dllexport)
#else
#define CPPLIBRARY_API __declspec(dllimport)
#endif

// Declare functions here
extern "C" { 
	CPPLIBRARY_API bool testConnection();
	CPPLIBRARY_API int getRandom();
	CPPLIBRARY_API int displaySum(int a, int b);
}