using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FindWorldSpaceCamera : MonoBehaviour
{
    /*
     * This script will iterate though all child canvases,
     * check if world space and camera is null, then
     * set it to use the camera tagged MainCamera
     */
    void Awake()
    {
        var canvas = GetComponentsInChildren<Canvas>();
        for (int i = 0; i < canvas.Length; i++)
        {
            if (canvas[i].renderMode == RenderMode.WorldSpace &&
                canvas[i].worldCamera == null)
            {
                canvas[i].worldCamera = Camera.main;
            }
        }

        var eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogWarning("EventSystem not found in scene.  Player controls will not work");
        }
    }
}
