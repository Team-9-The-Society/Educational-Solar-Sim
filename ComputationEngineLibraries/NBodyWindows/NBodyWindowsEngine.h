#ifdef COMPUTATIONENGINELIBRARY_EXPORTS
#	define COMPUTATIONENGINELIBRARY_API __declspec(dllexport)
#else
#	define COMPUTATIONENGINELIBRARY_API __declspec(dllimport)
#endif

// Export Methods Here
extern "C" {
	COMPUTATIONENGINELIBRARY_API int updateForce(double* xPos, double* yPos, double* zPos, double** force, double* mass, int numBodies);
}