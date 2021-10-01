// Write C++ code here.
//
// Do not forget to dynamically load the C++ library into your application.
//
// For instance,
//
// In MainActivity.java:
//    static {
//       System.loadLibrary("cppDemo");
//    }
//
// Or, in MainActivity.kt:
//    companion object {
//      init {
//         System.loadLibrary("cppDemo")
//      }
//    }
#include <jni.h>
#include <iosfwd>


extern "C" {
    JNIEXPORT jint JNICALL Java_com_project_solarsim_NativeJNI_Add(JNIEnv *env, jobject thiz, jint first, jint second)
    {
        return first + second;
    }
}
