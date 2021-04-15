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
        scrollBar.numberOfSteps = (int)(maxTime / Time.fixedDeltaTime);
        Camera.main.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, cameraSpeed);
        Physics.IgnoreLayerCollision(0, 0);
        ChangeSelectAndAddButtonsEnability(false);
    }

    private void Update()
    {
        if (paused && Input.GetKeyDown(KeyCode.Mouse0))
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
            scrollBar.value = currentTime / maxTime;
            timeText.text = $"Time: {currentTime}.";
        }
        else
        {
            Camera.main.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        }
        UpdateTime();
    }

    public void SetTime(float newTime)
    {
        currentTime = newTime;
        timeText.text = $"Time: {currentTime}.";
    }

    public void SetTimeFlow(bool on)
    {
        Time.timeScale = on ? 1 : 0;
    }

    private void UpdateTime()
    {
        if (timeShenanigans)
        {
            SetTimeFlow(!paused);
            timeShenanigans = false;
        }
    }
}
