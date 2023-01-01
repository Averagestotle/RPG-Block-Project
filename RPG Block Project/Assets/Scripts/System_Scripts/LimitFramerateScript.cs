using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFramerateScript : MonoBehaviour
{
    public int targetFrameRate = 0;
    // Start is called before the first frame update
    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled

        if (targetFrameRate == 0)
        {
            targetFrameRate = 60; // Default framerate if not chosen
        }
        Application.targetFrameRate = targetFrameRate;
    }
}
