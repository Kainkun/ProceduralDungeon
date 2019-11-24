using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int direction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            SceneBuilder.scenebuilder.loadLevel(direction);
    }
}
