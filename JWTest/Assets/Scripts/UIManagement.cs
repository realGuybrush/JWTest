using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum PointerMode { Select, Add }
public enum ViewMode { Top, FirstPerson }

public partial class MainManager : MonoBehaviour
{
    private ViewMode viewMode = ViewMode.FirstPerson;
    private bool pointerOnUIElement = false;
    private bool manualScrollBarValueChange = false;
    private PointerMode pointerMode = PointerMode.Select;
    public Scrollbar scrollBar;
    public Button selectButton;
    public Button addButton;
    public Button playPauseButton;
    public GameObject changePanel;
    public List<InputField> inputFields;
    public Text modeText;
    public Text timeText;

    public void OnMouseClick()
    {
        if (viewMode== ViewMode.FirstPerson)
        {
            if (pointerMode == PointerMode.Add)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                AddPoint(mousePos);
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100f));
                SelectPoint(ray);
            }
        }
    }

    public void OnScrollBarHandlePress()
    {
        SetTimeFlow(false);
        Camera.main.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, cameraSpeed);
        manualScrollBarValueChange = true;
    }

    public void OnScrollBarHandleRelease()
    {
        if (!paused)
        {
            SetTimeFlow(true);
        }
        manualScrollBarValueChange = false;
    }

    public void OnScrollBarValueChange()
    {
        if (manualScrollBarValueChange)
        {
            SetTime(scrollBar.value * maxTime);
            scrolledInPause = true;
        }
    }

    public void OnPlayPauseButtonPress()
    {
        paused = !paused;
        SetTimeFlow(!paused);
        playPauseButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = paused ? ">" : "II";
        ChangeSelectAndAddButtonsEnability(paused);
        changePanel.SetActive(paused && pointerMode == PointerMode.Select);
    }

    public void OnSelectPress()
    {
        pointerMode = PointerMode.Select;
        changePanel.SetActive(true);
        modeText.text = "Mode: select.";
    }

    public void OnAddPress()
    {
        pointerMode = PointerMode.Add;
        changePanel.SetActive(false);
        modeText.text = "Mode: add.";
    }

    public void OnPointerEnterElement()
    {
        pointerOnUIElement = true;
    }

    public void OnPointerExitElement()
    {
        pointerOnUIElement = false;
    }

    public void OnPositionValueChanged()
    {
        if (selectedPoint > -1)
        {
            float x, y, z;
            try
            {
                x = (float)Convert.ToDouble(inputFields[0].text);
            }
            catch
            {
                x = points[selectedPoint].position.x;
            }
            try
            {
                y = (float)Convert.ToDouble(inputFields[1].text);
            }
            catch
            {
                y = points[selectedPoint].position.y;
            }
            try
            {
                z = (float)Convert.ToDouble(inputFields[2].text);
            }
            catch
            {
                z = points[selectedPoint].position.z;
            }
            points[selectedPoint].position = new Vector3(x, y, z);
            points[selectedPoint].point.transform.position = points[selectedPoint].position;
            UpdateCube(selectedPoint);
        }
    }

    public void OnRotationValueChanged()
    {
        if (selectedPoint > -1)
        {
            float x, y, z;
            try
            {
                x = (float)Convert.ToDouble(inputFields[3].text);
            }
            catch
            {
                x = points[selectedPoint].rotation.x;
            }
            try
            {
                y = (float)Convert.ToDouble(inputFields[4].text);
            }
            catch
            {
                y = points[selectedPoint].rotation.y;
            }
            try
            {
                z = (float)Convert.ToDouble(inputFields[5].text);
            }
            catch
            {
                z = points[selectedPoint].rotation.z;
            }
            points[selectedPoint].rotation = new Vector3(x, y, z);
            points[selectedPoint].point.transform.localEulerAngles = points[selectedPoint].rotation;
            UpdateCube(selectedPoint);
        }
    }

    public void OnCreationTimeChanged()
    {
        if (selectedPoint > -1)
        {
            int crTime;
            try
            {
                crTime = Convert.ToInt32(inputFields[6].text);
            }
            catch
            {
                crTime = points[selectedPoint].creationTime;
            }
            points[selectedPoint].creationTime = crTime;
            UpdateCube(selectedPoint);
        }
    }

    public void OnDestinationTimeChanged()
    {
        if (selectedPoint > -1)
        {
            int dstTime;
            try
            {
                dstTime = Convert.ToInt32(inputFields[7].text);
            }
            catch
            {
                dstTime = points[selectedPoint].destinationTime;
            }
            points[selectedPoint].destinationTime = dstTime;
            UpdateCube(selectedPoint);
        }
    }

    public void OnDeleteButtonPress()
    {
#if UNITY_EDITOR
        if (selectedPoint > -1)
        {
            if (EditorUtility.DisplayDialog("Deleting point", "Are you sure\nyou want to Delete selected Point?", "Yes", "No"))
            {
                GameObject.Destroy(points[selectedPoint].point);
                points.RemoveAt(selectedPoint);
                selectedPoint = -1;
                UpdateInputFields();
            }
        }
#endif
    }

    private void UpdateInputFields()
    {
        if (selectedPoint > -1)
        {
            inputFields[0].text = points[selectedPoint].position.x.ToString();
            inputFields[1].text = points[selectedPoint].position.y.ToString();
            inputFields[2].text = points[selectedPoint].position.z.ToString();
            inputFields[3].text = points[selectedPoint].rotation.x.ToString();
            inputFields[4].text = points[selectedPoint].rotation.y.ToString();
            inputFields[5].text = points[selectedPoint].rotation.z.ToString();
            inputFields[6].text = points[selectedPoint].creationTime.ToString();
            inputFields[7].text = points[selectedPoint].destinationTime.ToString();
        }
        else
        {
            ClearInputFields();
        }
    }

    private void ClearInputFields()
    {
        for (int i = 0; i < 8; i++)
        {
            inputFields[i].text = "";
        }
    }

    private void ChangeSelectAndAddButtonsEnability(bool enabled)
    {
        addButton.enabled = paused;
        SwapButtonColors(addButton);
        selectButton.enabled = paused;
        SwapButtonColors(selectButton);
        if (!enabled)
        {
            modeText.text = "Mode: disabled.";
        }
        else
        {
            if (pointerMode == PointerMode.Add)
            {
                modeText.text = "Mode: add.";
            }
            else
            {
                modeText.text = "Mode: select.";
            }
        }
    }

    private void SwapButtonColors(Button button)
    {
        ColorBlock block = button.colors;
        Color color = block.disabledColor;
        block.disabledColor = block.normalColor;
        block.normalColor = color;
        button.colors = block;
    }
}
