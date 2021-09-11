package com.society.demo2;

public class NativeJNI {
    static {
      System.loadLibrary("demo2");
    }

    public native int Add(int a, int b);
}
