using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    float cameraSpeed = 1;

    Transform player;
    Vector3 camTarget;

    void Start()
    {
        player = Player.player.transform;
    }

    Vector3 camGoTo;
    void Update()
    {
        camTarget = player.position;

        camGoTo = Vector3.Lerp(transform.position, camTarget, Time.deltaTime * cameraSpeed);
        camGoTo.z = -10;
        transform.position = camGoTo;
    }
}
