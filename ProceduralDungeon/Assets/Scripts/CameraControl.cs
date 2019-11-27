using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    float mouseCamControl = 1;

    [SerializeField]
    float cameraSpeed = 1;

    Transform player;
    Vector3 camTarget;

    Vector3 mousePos;

    void Start()
    {
        player = Player.player.transform;
    }

    Vector3 camGoTo;
    private void FixedUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        camTarget = player.position;

        camTarget += (mousePos - camTarget).normalized * mouseCamControl;

        camGoTo = Vector3.Lerp(transform.position, camTarget, Time.deltaTime * cameraSpeed);
        camGoTo.z = -10;
        transform.position = camGoTo;

    }
}
