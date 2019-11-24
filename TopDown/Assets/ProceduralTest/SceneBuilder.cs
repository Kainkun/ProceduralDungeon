using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneBuilder : MonoBehaviour
{
    [SerializeField]
    int width, height;

    int[,] terrainMap;

    [SerializeField]
    Tilemap groundMap, colliderMap;

    [SerializeField]
    RuleTile[] groundTiles, colliderTiles;

    void Start()
    {
        buildScene();
    }


    void Update()
    {
        
    }

    void buildScene()
    {
        clearMap();
        terrainMap = new int[width, height];
        initPos();
        setTiles();


    }

    void setTiles()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < height; x++)
            {
                groundMap.SetTile(new Vector3Int(x, y, 0), groundTiles[0]);
            }
        }
    }

    void initPos()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < height; x++)
            {
                terrainMap[x, y] = Random.Range(0, 2);
            }
        }
    }

    void clearMap()
    {
        groundMap.ClearAllTiles();
        colliderMap.ClearAllTiles();

        terrainMap = null;
    }
}
