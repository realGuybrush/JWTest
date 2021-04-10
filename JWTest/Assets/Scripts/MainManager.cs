using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public partial class MainManager : MonoBehaviour
{
    public int maxTime = 60;
    float currentTime = 0;
    bool paused = false;
    bool scrolledInPause = false;
    List<DirectionalPointStruct> points = new List<DirectionalPointStruct>();
    void Start()
    {
        scrollBar.numberOfSteps = maxTime;
        Camera.main.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, cameraSpeed);
        Physics.IgnoreLayerCollision(0, 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!pointerOnUIElement)
            { 
                OnMouseClick();
            }
        }
        UpdateAllCubes();
        UpdateCameraPosition();
    }

    void FixedUpdate()
    {
        if (currentTime <= maxTime)
        {
            currentTime += Time.fixedDeltaTime;
            scrollBar.value = currentTime / scrollBar.numberOfSteps;
        }
    }

    public void SetTime(int newTime)
    {
        currentTime = (float)Convert.ToDouble(newTime);
    }

    public void SetTimeFlow(bool go)
    {
        Time.timeScale = go?1:0;
    }
}
