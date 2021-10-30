using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CppBackend : MonoBehaviour
{

   

    // Start is called before the first frame update
    unsafe void Start()
    {

        int count = 2;


        double[] X = { 1, 2 };
        double[] Y = { 1, 2 };
        double[] Z = { 1, 2 };
        double[] M = { 1, 1 };
        double[][] F = new double[count,3];

        updateForce(in X, in Y, in Z, out F, in M, in count);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [DllImport("nBodyWindows", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
    static unsafe extern int updateForce(in double* xPos, in double* yPos, in double* zPos, out double** force, in double* mass, in int numBodies);
}
