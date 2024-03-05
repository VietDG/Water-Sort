using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionClass
{

}

public static class IntExtensions
{
    public static void Intcrement(this int number)
        => number++;

    // Take note of the extra ref keyword here
    public static void RefIntcrement(this ref int number)
        => number++;

    public static void RefIntCrementWithAmount(this ref int number, int amount)
    {
        number += amount;
    }

    public static void RefIntReset(this ref int number) => number = 0;
}