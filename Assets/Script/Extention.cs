using System;
using UnityEngine;

public static class Extention
{
    public static void Log(this Exception ex)
    {
        Debug.LogError("message: " + ex.Message);
        Debug.LogError("stack trace: ");
        Debug.LogError(ex.StackTrace);
    }
}

