using UnityEngine;
using System.Collections;

public class IsoGrid : MonoBehaviour {

    public Transform Tile;
	// Use this for initialization
	void Start () {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                Instantiate(Tile, new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	  
	}
}
