using UnityEngine;
using System.Collections;

public class IsoGrid : MonoBehaviour {

    public Transform Tile;
    public int size;
    

    private Vector3[,] spawnGrid = new Vector3[3, 4];

    // Use this for initialization
    void Start () {
        SpawnGrid();
    }
	
    void SpawnGrid()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Instantiate(Tile, new Vector3(x, 0, y), Quaternion.identity);
                            
            }
        }
    }



	// Update is called once per frame
	void Update () {
	  if(Input.GetKeyDown(KeyCode.O))
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Tile"))
                Destroy(go);

            SpawnGrid();
        }
	}
}
