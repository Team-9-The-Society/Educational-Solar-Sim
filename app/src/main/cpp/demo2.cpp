// Write C++ code here.
//
// Do not forget to dynamically load the C++ library into your application.
//
// For instance,
//
// In MainActivity.java:
//    static {
//       System.loadLibrary("demo2");
//    }
//
// Or, in MainActivity.kt:
//    companion object {
//      init {
//         System.loadLibrary("demo2")
//      }
//    }
#include <jni.h>


extern "C" {
JNIEXPORT jint JNICALL
Java_com_project_demo2_NativeJNI_Add(JNIEnv *env, jobject thiz, jint first, jint second)
{
    return first + second;
}
}
