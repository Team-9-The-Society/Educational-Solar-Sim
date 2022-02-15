using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HashItUp
{
    /// <summary>
    /// This is a reusable hash algorithm. Keep track of YOUR OWN LOAD AND LoadBalance if you are using it!
    /// </summary>
    /// <param name="tablesize"> size of your table/array</param>
    /// <param name="coprime">double hash base</param>
    /// <param name="key">the current element</param>
    /// <param name="loadBalance">% of the table/array that is currently taken up</param>
    /// <param name="collisionArray">array to mark what is and isnt taken up.</param>
    /// <returns></returns>

    private int myTableSize;
    private int myCoprime;
    private int myKey;
    private float highLoadBalance;
    private int[] myCollisionArray;
    private float load;
    private int invKey; 
    public HashItUp(int tablesize, int coprime, int key, float loadBalance, int[] collisionArray)
    {
        myTableSize = tablesize;
        myCoprime = coprime;
        myKey = key;
        highLoadBalance = loadBalance;
        myCollisionArray = collisionArray;
        load = 0;
        InverseKey();
    }

    private void InverseKey()
    {
        invKey = Math.Abs(myCollisionArray.Length - 1 - myKey);
    }

    public void ManualIncrementLoad(int increment)
    {
        load += increment;
    }
    public void ManualIncrementAndLoadBalance(int increment)
    {
        load += increment;
        highLoadBalance = load / ((float)myCollisionArray.Length - 3);
    }
    public float getLoadBalance()
    {
        return highLoadBalance;
    }
    public float getLoad()
    {
        return load;
    }

    private int SecondHash(int key, int coprime)
    {
        return coprime - (key % coprime);
    }

    private int HashIt(int mode)
    {
        int key;
        if (mode == 0) 
        {
            key = myKey;
        }
        else
        {
            key = invKey;
        }

        int apple = 0;
        int num = 0;
        do
        {
            num = ((key + apple * (SecondHash(key, myCoprime))) % (myTableSize - 1));
            apple++;
            num = Math.Abs(num % (myCollisionArray.Length - 1));
            if (num > myCollisionArray.Length - 1 || num < 0)
            {
                LoadReset();
                num = 1;
            }

        } while (myCollisionArray[num] == 50);
        myCollisionArray[num] = 50;

        return num;
    }

    private void LoadReset()
    {
        load = 0;
        highLoadBalance = 0;

        for (int i = 0; i < myCollisionArray.Length; i++)
        {
            myCollisionArray[i] = 0;
        }
        Debug.Log("LOAD RESET");

    }

    public void ChangeKey(int newKey)
    {
        myKey = newKey;
        InverseKey();
    }

    public void PublicLoadReset()
    {
        LoadReset();
    }

    public int HashItOut(int mode)
    {
        return HashIt(mode);
    }

}
