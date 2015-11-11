using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {


    public Tile prev;
	private Material mat;

    // Use this for initialization
    void Start () {
		mat = transform.GetComponent<Renderer> ().material;
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
	public Material GetTileMat() {
		return mat;
	}	

	public void SetTileMat(Material _mat) {
		mat = _mat;
	}
}
