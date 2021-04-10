using System.Collections.Generic;
using UnityEngine;
using System;

public partial class MainManager : MonoBehaviour
{
    public int maxTime = 60;
    private float currentTime = 0;
    private bool paused = false;
    private bool scrolledInPause = false;
    private List<DirectionalPointStruct> points = new List<DirectionalPointStruct>();
    private bool timeShenanigans = false;

    private void Start()
    {
        scrollBar.numberOfSteps = maxTime;
        Camera.main.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, cameraSpeed);
        Physics.IgnoreLayerCollision(0, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!pointerOnUIElement)
            { 
                OnMouseClick();
            }
        }
        UpdateAllCubes();
        UpdateCameraPosition();
    }

    private void FixedUpdate()
    {
        if (currentTime <= maxTime)
        {
            currentTime += Time.fixedDeltaTime;
            scrollBar.value = currentTime / scrollBar.numberOfSteps;
        }
        UpdateTime();
    }

    public void SetTime(int newTime)
    {
        currentTime = (float)Convert.ToDouble(newTime);
    }

    public void SetTimeFlow(bool on)
    {
        Time.timeScale = on ? 1 : 0;
    }

    private void UpdateTime()
    {
        if (timeShenanigans)
        {
            SetTimeFlow(false);
            timeShenanigans = false;
        }
    }
}
