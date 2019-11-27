using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;

    [SerializeField]
    Sprite openDoor, closedDoor;

    [SerializeField]
    SpriteRenderer sr;

    public int direction;


    public void close()
    {
        sr.sprite = closedDoor;
        isOpen = false;
    }

    public void open()
    {
        sr.sprite = openDoor;
        isOpen = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && isOpen)
            SceneBuilder.scenebuilder.loadLevel(direction);
    }
}
