using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileHandler : MonoBehaviour {

    public int rows, columns;
    private int a, b;
    public Tile[,] TileMap;
    private Transform tmp;
    public GameObject TilePrefab;    

    // Use this for initialization
    void Start () {
        SpawnTiles();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SpawnTiles()
    {
        TileMap = new Tile[rows, columns];
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 pos = new Vector3(i, 0.0F, j);
                (Instantiate(TilePrefab, pos, transform.rotation) as GameObject).transform.parent = transform;
                // Instantiate(TilePrefab, pos, transform.rotation);
            }
        }

        //populate map
        foreach (Transform a in transform)
        {
            int i = (int)a.position.x;
            int j = (int)a.position.z;
            TileMap[i, j] = a.transform.GetComponent<Tile>();
            
        }


    }

    public Transform GetTile(int x, int y)
    {
        return TileMap[x, y].transform;
    }
    public List<Tile> GetAdjacent(Tile tile)
    {
        return GetAdjacent((int)tile.transform.position.x, (int)tile.transform.position.z);
    }

    public List<Tile> GetAdjacent(int x, int y)
    {
        List<Tile> tmpList = new List<Tile>();
        for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
                try
                {
                    if (i == 0 && j == 0) // exclude middle point (self)
                    {
                    }
                    else
                    {
                        a = x + i;
                        b = y + j;
                        if (a >= 0 && a < rows && b >= 0 && b < columns)
                        {
                            tmpList.Add(TileMap[x + i, y + j]);
                        }
                    }
                }
                catch (System.IndexOutOfRangeException e)
                {
                    Debug.Log(e);
                    Debug.Log("a: " + a + ". b: " + b);
                }
        //foreach(Node n in tmpList) {
        //	n.transform.GetComponent<MeshRenderer>().material.color = Color.blue;
        //}
        return tmpList;
    }
}
