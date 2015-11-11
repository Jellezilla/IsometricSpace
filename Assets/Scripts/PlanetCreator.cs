using UnityEngine;
using System.Collections;

public class PlanetCreator : MonoBehaviour {

	private GameStateHandler gsh;
	private TileHandler th;
	private int w, h;

	private Material testMat;
	// Use this for initialization
	void Start () {
		gsh = GameObject.Find ("GameStateHandler").GetComponent<GameStateHandler> ();
		th = GameObject.Find ("TileHandler").GetComponent<TileHandler> ();
		w = th.rows;
		h = th.columns;

		InitializeSurfaceMaterials ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void InitializeSurfaceMaterials() {	
		for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {
				int rand = Random.Range (1,3);

				if(rand == 1) {
					Debug.Log ("should'a made da sun mat now!");
					Tile t = th.GetTile (x,y);
						t.SetTileMat(gsh.GetCurrentMat());
				}
			}
		}
	}
}
