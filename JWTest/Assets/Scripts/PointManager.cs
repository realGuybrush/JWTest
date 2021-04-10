using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainManager : MonoBehaviour
{
    bool addOrSelect = true;
    int selectedPoint = -1;
    public GameObject pointPrefab;
    void AddPoint(Vector3 pos)
    {
        points.Add(new DirectionalPointStruct
        {
            position = pos,
            rotation = new Vector3(0f, 0f, 0f),
            creationTime = (int)currentTime,
            destinationTime = (int)currentTime + 1,
            point = Instantiate(pointPrefab)
        });
        points[points.Count - 1].point.transform.position = pos;
        points[points.Count - 1].point.transform.eulerAngles = points[points.Count - 1].rotation;
        int index = points.Count - 1;
        SetCubeValues(points.Count - 1, true, Color.white, 
            -10f / (points[index].destinationTime - points[index].creationTime), 
            true, new Vector3(0f, 0f, 10f * (points[index].destinationTime - currentTime) / (points[index].destinationTime - points[index].creationTime)));
    }
    public void SelectPoint(Vector3 start)
    {
        RaycastHit[] hits = Physics.BoxCastAll(start, new Vector3(0.5f, 0.5f, 0.5f), Camera.main.transform.forward, new Quaternion(), 10f);
        if (hits.Length > 0)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (hits[0].collider.gameObject.transform.position == points[i].point.transform.position)
                {
                    selectedPoint = i;
                    UpdateInputFields();
                    return;
                }
            }
            ClearInputFields();
        }
    }

    void UpdateAllCubes()
    {
        for (int i = 0; i < points.Count; i++)
        {
            UpdateCube(i);
        }
        if (scrolledInPause)
            scrolledInPause = false;
    }
    void UpdateCube(int index)
    {
        if (!paused)
        {
            float timeDelta = Time.fixedDeltaTime * 2.0f;
            if ((points[index].creationTime > currentTime - timeDelta) && (points[index].creationTime < currentTime + timeDelta))
            {
                SetCubeValues(index, true, Color.white, -10f / (points[index].destinationTime - points[index].creationTime), true, new Vector3(0f, 0f, 10f));
            }
            if ((points[index].destinationTime > currentTime - timeDelta) && (points[index].destinationTime < currentTime + timeDelta))
            {
                SetCubeValues(index, true, Color.red, 0f, true, new Vector3());
            }
        }
        if ((manualValueChange)||!(manualValueChange||paused))
        {
            if (points[index].creationTime > currentTime)
            {
                SetCubeValues(index, false, Color.white, 0f, true, new Vector3(0f, 0f, 10f));
            }
            if ((points[index].creationTime <= currentTime) && (points[index].destinationTime > currentTime))
            {
                SetCubeValues(index, true, Color.white,
                    -10f / (points[index].destinationTime - points[index].creationTime),
                    true, new Vector3(0f, 0f, 10f * (points[index].destinationTime - currentTime) / (points[index].destinationTime - points[index].creationTime)));
            }
            if (points[index].destinationTime <= currentTime)
            {
                SetCubeValues(index, true, Color.red, 0f, true, new Vector3());
            }
        }
    }
    void SetCubeValues(int index, bool active, Color color, float velocity, bool updatePos, Vector3 newLocalPosition)
    {
        GameObject cube = points[index].point.transform.GetChild(0).gameObject;
        cube.SetActive(active);
        cube.GetComponent<MeshRenderer>()?.material.SetColor("_Color", color);
        cube.GetComponent<Rigidbody>().velocity = velocity* cube.transform.forward;
        if (updatePos)
        {
            cube.transform.localPosition = newLocalPosition;
        }
    }
}
