using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsNullCheckScript : MonoBehaviour
{
    public bool IsGameObjectEmpty(GameObject gameObject)
    {
        if (gameObject != null)
        {
            return true;
        }
        else
        {
            Debug.LogError("A null object was passed.");
            return false;
        }
    }
}
