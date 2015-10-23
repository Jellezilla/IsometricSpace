using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {


    public Tile prev;
   
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Tile getPrev()
    {
        return prev;
    }
    public void setPrev(Tile _prev)
    {
        prev = _prev;
    }
}
