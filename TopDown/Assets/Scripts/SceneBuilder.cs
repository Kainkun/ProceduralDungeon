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

    static int currentWidth, currentHeight;

    [HideInInspector]
    public static int[,] terrainMap;

    [SerializeField]
    Tilemap groundMap, colliderMap;

    [SerializeField]
    RuleTile[] groundTiles, colliderTiles;
    [SerializeField]
    Tile[] wallTiles;


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

        //checkForHashClones(13);
        //checkForClonesGames(10000, 19);
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

        //print(currentRoom.layerNumber + ", " + currentRoom.roomNumber);
        setPlayer();
    }

    void clearMap()
    {
        groundMap.ClearAllTiles();
        colliderMap.ClearAllTiles();

        terrainMap = null;
    }

    void makeRectangle(int block, int xpos, int ypos, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(terrainMap.GetLength(0) - 1 > x + xpos && terrainMap.GetLength(1) - 1 > y + ypos && x + xpos > 0 && y + ypos > 0)
                    terrainMap[x + xpos, y + ypos] = block;
            }
        }
    }


    void randomWalkTiles(float fillPercent, float walkerSpawnChance, int maxWalkers)
    {
        terrainMap = new int[currentWidth, currentHeight];

        int currentFill = 0;
        int maxFill = (int)(currentWidth*currentHeight*fillPercent);

        List<Walker> walkers = new List<Walker>();
        walkers.Add(new Walker(currentWidth/2, currentHeight/2));

        for (int y = 0; y < currentHeight; y++)
        {
            for (int x = 0; x < currentWidth; x++)
            {
                terrainMap[x, y] = 1;
            }
        }

        int steps = 0;
        while (currentFill < maxFill || steps < 100)
        {
            steps++;
            for (int i = 0; i < walkers.Count; i++)
            {
                walkers[i].walk(0.2f);
                if (terrainMap[walkers[i].x, walkers[i].y] != 0)
                {
                    terrainMap[walkers[i].x, walkers[i].y] = 0;
                    currentFill++;

                    if(Random.Range(0f,1f) > walkerSpawnChance && walkers.Count < maxWalkers)
                    {
                        walkers.Add(new Walker(walkers[i].x, walkers[i].y));
                    }
                }
            }

        }

    }
    class Walker
    {

        public int x;
        public int y;

        public int lastdirection;

        public Walker(int setX, int setY)
        {
            x = setX;
            y = setY;
            lastdirection = Random.Range(1, 5);
        }
        public void walk(float straightBias)
        {
            int moveDirection;

            if (straightBias > Random.Range(0f, 1f))
                moveDirection = lastdirection;
            else
            {
                moveDirection = Random.Range(1, 5);
                lastdirection = moveDirection;
            }

            if (moveDirection == 3)
            {
                if (y > 1)
                    y--;
                else
                    y++;

                return;
            }

            if (moveDirection == 2)
            {
                if (x < currentWidth - 2)
                    x++;
                else
                    x--;

                return;
            }

            if (moveDirection == 4)
            {
                if (x > 1)
                    x--;
                else
                    x++;

                return;
            }

            if (moveDirection == 1)
            {
                if (y < currentHeight - 2)
                    y++;
                else
                    y--;

                return;
            }

            Debug.LogError("walker stuck");
        }
    }

    void removeSoloBlocks()
    {
        for (int y = 1; y < currentHeight - 1; y++)
        {
            for (int x = 1; x < currentWidth - 1; x++)
            {
                if (terrainMap[x+1, y] == 0 && terrainMap[x - 1, y] == 0 && terrainMap[x, y + 1] == 0 && terrainMap[x, y - 1] == 0)
                    terrainMap[x, y] = 0;
            }
        }
    }

    void setTiles()
    {
        for (int y = 0; y < currentHeight; y++)
        {
            for (int x = 0; x < currentWidth; x++)
            {
                if (terrainMap[x, y] == 1)
                    colliderMap.SetTile(new Vector3Int(x, y, 0), wallTiles[Random.Range(0, wallTiles.Length)]);
                else
                    groundMap.SetTile(new Vector3Int(x, y, 0), groundTiles[0]);
            }
        }
    }

    void buildScene(roomData room)
    {
        Random.InitState(room.GetHashCode() + 1);

        currentWidth = Random.Range(minWidth, maxWidth + 1);
        currentHeight = Random.Range(minHeight, maxHeight + 1);

        clearMap();
        randomWalkTiles(0.2f, 0.02f, 5);

        setPlayer();
        removeSoloBlocks();
        buildDoors();

        setTiles();
    }

    void initDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i] = Instantiate(doorObject, new Vector3(0, 5, 0), Quaternion.identity).transform;
            doors[i].GetComponent<Door>().direction = i;
        }

        doors[3].GetComponent<Door>().close();
    }

    void buildDoors()
    {
        bool found = false;
        while(!found)
        {
            int door0yPos = Random.Range((int)(currentHeight * 0.2f), (int)(currentHeight * 0.8f));

            for (int x = 4; x < currentWidth / 2 - 2; x++)
            {
                if (terrainMap[x, door0yPos] == 0)
                {
                    makeRectangle(0, x - 2, door0yPos, 3, 2);
                    doors[0].position = new Vector3(x - 2 + 0.5f, door0yPos + 0.5f, 0);
                    found = true;
                    break;
                }
            }
        }

        found = false;
        while (!found)
        {
            int door1xPos = Random.Range((int)(currentWidth * 0.2f), (int)(currentWidth * 0.8f));

            for (int y = currentHeight - 6; y > currentHeight / 2 - 2; y--)
            {
                if (terrainMap[door1xPos, y] == 0)
                {
                    makeRectangle(0, door1xPos, y + 1, 2, 3);
                    doors[1].position = new Vector3(door1xPos + 0.5f, y + 2 + 0.5f, 0);
                    found = true;
                    break;
                }
            }
        }

        found = false;
        while (!found)
        {
            int door0yPos = Random.Range((int)(currentHeight * 0.2f), (int)(currentHeight * 0.8f));

            for (int x = currentWidth - 6; x > currentWidth / 2 + 2; x--)
            {
                if (terrainMap[x, door0yPos] == 0)
                {
                    makeRectangle(0, x + 1, door0yPos, 3, 2);
                    doors[2].position = new Vector3(x + 2 + 0.5f, door0yPos + 0.5f, 0);
                    found = true;
                    break;
                }
            }
        }


        int door3xPos = (int)Player.player.transform.position.x;
        for (int y = 3; y < currentHeight - 2; y++)
        {
            if (terrainMap[door3xPos, y] == 0)
            {
                makeRectangle(0, door3xPos, y - 2, 2, 4);
                doors[3].position = new Vector3(door3xPos + 0.5f, y + 0.5f - 2, 0);
                break;
            }
        }

    }


    void setPlayer()
    {
        int closestXfromCenter = 10000;
        int currentY = 0;
        for (int y = 4; y < currentHeight - 1; y++)
        {
            for (int x = 1; x < currentWidth - 1; x++)
            {
                if (terrainMap[x, y] == 0 && Mathf.Abs((currentWidth / 2) - x) < Mathf.Abs(currentWidth / 2 - closestXfromCenter))
                    closestXfromCenter = x;
            }

            if (closestXfromCenter < Mathf.Abs( currentWidth / 2 - 10000))
            {
                currentY = y;
                break;
            }
        }

        //print(closestXfromCenter + ", " + currentY);
        Player.player.transform.position = new Vector3(closestXfromCenter, currentY, 0);
        Camera.main.transform.position = new Vector3(closestXfromCenter, currentY, -10);
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

    void checkForClonesGames(int games, int layers)
    {
        int clonecount = 0;
        Dictionary<int, roomData> hash = new Dictionary<int, roomData>();
        for (int g = 0; g < games; g++)
        {
            print("game " + g);
            roomData currentRoom = new roomData(0, 0);
            for (int r = 0; r <= layers; r++)
            {
                Random.InitState((int)System.DateTime.Now.Ticks);
                int rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    currentRoom = currentRoom.goLeft(currentRoom);
                }
                if (rand == 1)
                {
                    currentRoom = currentRoom.goMiddle(currentRoom);
                }
                if (rand == 2)
                {
                    currentRoom = currentRoom.goRight(currentRoom);
                }
                int chash = currentRoom.GetHashCode();

                if (hash.ContainsKey(chash) && hash[chash].layerNumber != currentRoom.layerNumber)
                {
                    print("1# " + hash[chash].layerNumber + ", " + hash[chash].roomNumber + ": " + hash[chash].GetHashCode());
                    print("2# " + currentRoom.layerNumber + ", " + currentRoom.roomNumber + ": " + currentRoom.GetHashCode());
                    clonecount++;
                }
                else
                {
                    if(!hash.ContainsKey(chash))
                        hash.Add(chash, currentRoom);
                }

            }
        }
        print("clone count: " + clonecount);
    }

    void checkForHashClones(int layers)
    {
        int clonecount = 0;
        Dictionary<int, roomData> hash = new Dictionary<int, roomData>();
        for (int i = 0; i <= layers; i++)
        {
            for (int p = 0; p <= Mathf.Pow(3, i); p++)
            {
                roomData room = new roomData((uint)i, (uint)p);
                int num = room.GetHashCode();
                if (hash.ContainsKey(num))
                {
                    print("1# " + hash[num].layerNumber + ", " + hash[num].roomNumber + ": " + hash[num].GetHashCode());
                    print("2# " + room.layerNumber + ", " + room.roomNumber + ": " + room.GetHashCode());
                    clonecount++;
                }
                else
                    hash.Add(num, room);
            }
        }
        print("clone count: " + clonecount);
    }
}
