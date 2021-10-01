package com.project.solarsim;

public class NativeJNI {
    static {
      System.loadLibrary("cppDemo");
    }

    public native int Add(int a, int b);
}
