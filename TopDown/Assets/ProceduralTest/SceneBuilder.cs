using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneBuilder : MonoBehaviour
{
    [HideInInspector]
    public static SceneBuilder scenebuilder;

    [SerializeField]
    int minWidth, maxWidth, minHeight, maxHeight;

    int currentWidth, currentHeight;

    int[,] terrainMap;

    [SerializeField]
    Tilemap groundMap, colliderMap;

    [SerializeField]
    RuleTile[] groundTiles, colliderTiles;


    roomData currentRoom;

    [SerializeField]
    GameObject doorObject;
    Transform[] doors = new Transform[4];

    private void Awake()
    {
        scenebuilder = this;
    }

    void Start()
    {
        initDoors();

        currentRoom = new roomData(0, 0);


        buildScene(currentRoom);
        setPlayer();

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            loadLevel(0);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            loadLevel(1);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            loadLevel(2);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            loadLevel(-1);


    }

    public void loadLevel(int direction)
    {
        if(direction == 0)
            buildScene(goLeft());
        else if (direction == 1)
            buildScene(goMiddle());
        else if (direction == 2)
            buildScene(goRight());
        else
            buildScene(goBack());

        print(currentRoom.layerNumber + ", " + currentRoom.roomNumber);
        setPlayer();
    }

    void clearMap()
    {
        groundMap.ClearAllTiles();
        colliderMap.ClearAllTiles();

        terrainMap = null;
    }

    void setTiles()
    {
        terrainMap = new int[currentWidth, currentHeight];

        for (int y = 0; y < currentHeight; y++)
        {
            for (int x = 0; x < currentWidth; x++)
            {
                terrainMap[x, y] = Random.Range(0, 2);
            }
        }


        for (int y = 0; y < currentHeight; y++)
        {
            for (int x = 0; x < currentWidth; x++)
            {
                groundMap.SetTile(new Vector3Int(x - currentWidth / 2, y - 1, 0), groundTiles[0]);
            }
        }
    }

    void buildScene(roomData room)
    {
        Random.InitState(room.GetHashCode() + 1);

        currentWidth = Random.Range(minWidth, maxWidth + 1);
        currentHeight = Random.Range(minHeight, maxHeight + 1);

        clearMap();
        setTiles();
        buildDoors();
    }

    void initDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i] = Instantiate(doorObject, new Vector3(0, 5, 0), Quaternion.identity).transform;
            doors[i].GetComponent<Door>().direction = i;
        }
    }

    void buildDoors()
    {
        doors[0].position = new Vector3(-3, 3, 0);
        doors[1].position = new Vector3(0, 3, 0);
        doors[2].position = new Vector3(3, 3, 0);
        doors[3].position = new Vector3(0, -3, 0);

    }


    void setPlayer()
    {
        Player.player.transform.position = new Vector3(0, 0, 0);
    }





    roomData goLeft()
    {
        roomData tempRoom = currentRoom.goLeft(currentRoom);
        if (tempRoom == null)
            return currentRoom;

        currentRoom = tempRoom;
        return currentRoom;
    }
    roomData goMiddle()
    {
        roomData tempRoom = currentRoom.goMiddle(currentRoom);
        if (tempRoom == null)
            return currentRoom;

        currentRoom = tempRoom;
        return currentRoom;
    }
    roomData goRight()
    {
        roomData tempRoom = currentRoom.goRight(currentRoom);
        if (tempRoom == null)
            return currentRoom;

        currentRoom = tempRoom;
        return currentRoom;
    }
    roomData goBack()
    {
        roomData tempRoom = currentRoom.goBack(currentRoom);
        if (tempRoom == null)
            return currentRoom;

        currentRoom = tempRoom;
        return currentRoom;
    }
}
