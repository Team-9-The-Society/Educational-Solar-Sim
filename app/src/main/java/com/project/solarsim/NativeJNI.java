package com.project.solarsim;

public class NativeJNI {
    static {
      System.loadLibrary("demo2");
    }

    public native int Add(int a, int b);
}
