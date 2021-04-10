using UnityEngine;

public partial class MainManager : MonoBehaviour
{
    public float cameraSpeed = 4.0f;
    public Camera camera2;

    public void UpdateCameraPosition()
    {
        if (manualScrollBarValueChange)
        {
            Camera.main.transform.position = new Vector3(0f, 0f, currentTime * cameraSpeed - 10f);
        }
    }
}
