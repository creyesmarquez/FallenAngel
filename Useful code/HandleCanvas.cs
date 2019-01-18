using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleCanvas : MonoBehaviour
{
    private CanvasScaler scaler;

	void Start ()
    {
        // scales on every aspect ratio
        scaler = GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
	}
}
