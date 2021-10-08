using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class CppIntegration : MonoBehaviour
{
    // Import and expose native c++ functions
    [DllImport("CPPLIBRARY", EntryPoint = "testConnection")]
    public static extern bool testConnection();
    [DllImport("CPPLIBRARY", EntryPoint = "getRandom")]
    public static extern int getRandom();
    [DllImport("CPPLIBRARY", EntryPoint = "displaySum")]
    public static extern int displaySum(int a, int b);

    // GameObject of text to toggle on UI
    public GameObject connectionText;

    void Start()
    {
        connectionText = GameObject.Find("CppConnectionText");
        connectionText.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        // Display output of functions in console
        bool connection = testConnection();
        print("C++ testConnection(): " + connection);  
        print("C++ getRandom(): " + getRandom());
        print("C++ displaySum(1, 2): " + displaySum(1, 2));

        //Toggle text on UI to show connection
        connectionText.gameObject.SetActive(!connectionText.activeSelf);
    }
}