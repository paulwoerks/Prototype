using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera[] cams;
    public void Switch()
    {
        int cam0priority = cams[0].Priority;

        cams[0].Priority = cams[1].Priority;

        cams[1].Priority = cam0priority;
    }
}
