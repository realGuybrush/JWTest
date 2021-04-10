using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class MainManager : MonoBehaviour
{
    bool display1 = true;
    bool pointerOnUIElement = false;
    bool manualValueChange = false;
    public Scrollbar scrollBar;
    public Button addSelectButton;
    public GameObject changePanel;
    public List<InputField> inputFields;
    public void OnMouseClick()
    {
        Vector3 mos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        if (display1)
        {
            if (addOrSelect)
            {
                AddPoint(mos);
            }
            else
            {
                SelectPoint(mos);
            }
        }
    }
    public void OnScrollBarHandlePress()
    {
        SetTimeFlow(false);
        manualValueChange = true;
    }
    public void OnScrollBarHandleRelease()
    {
        if (!paused)
        {
            SetTimeFlow(true);
        }
        manualValueChange = false;
    }
    public void OnScrollBarValueChange()
    {
        if (manualValueChange)
        {
            SetTime((int)(scrollBar.value * scrollBar.numberOfSteps));
            scrolledInPause = true;
        }
    }
    public void OnPlayButtonPress()
    {
        SetTimeFlow(true);
        paused = false;
    }
    public void OnPauseButtonPress()
    {
        SetTimeFlow(false);
        paused = true;
    }
    public void OnTopViewPress()
    {
        display1 = !display1;
        addSelectButton.gameObject.SetActive(display1);
        changePanel.SetActive(display1&&!addOrSelect);
        Camera.main.targetDisplay = display1 ? 0 : 1;
        camera2.targetDisplay = display1 ? 1 : 0;
    }
    public void OnAddSelectPress()
    {
        addOrSelect = !addOrSelect;
        addSelectButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = addOrSelect ? "Add/Se.." : "A/Select";
        changePanel.SetActive(!addOrSelect);
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
    public void UpdateInputFields()
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
    public void ClearInputFields()
    {
        for (int i = 0; i < 8; i++)
        {
            inputFields[i].text = "";
        }
    }
}
