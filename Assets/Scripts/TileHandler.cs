using UnityEngine;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TileHandler : MonoBehaviour {

    public int rows, columns;
    private int a, b;
    public Tile[,] TileMap;
	private int[,] map;
	private int[,] tmpMap;
	private Material wallMat;

	//private Transform tmp;

    public GameObject TilePrefab;    
	public GameObject WallPrefab;
	private GameStateHandler gsh;


    // Use this for initialization
    void Start () {
		gsh = GameObject.Find ("GameStateHandler").GetComponent<GameStateHandler> ();
		Debug.Log (gsh.GetCurrentPlanetType ().ToString ());
		SpawnTiles();
		for (int i = 0; i < 11; i++) {
			CellularAutomata();
		
		}
		SetMaterials ();
		//StartCoroutine (wait (2.2f));
		SetColliderOnBlockedTiles ();
		//TilePrefab.transform.GetComponent<Tile> ().type = Tile.TileType.unassigned;
		//SpawnTileGroup ();
		//SpawnMap (0);
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);

		}
		if (Input.GetKeyDown (KeyCode.T)) {
			CellularAutomata ();
			SetMaterials ();
		}
		if (Input.GetKeyDown (KeyCode.U)) {
			AddResources ();
			
		}
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
			TileMap[i, j].type = Tile.TileType.unassigned;
        
		}

				CreateIntMap();
		// SetRandomTileMats();
		AddWalls ();
		//CellularAutomata ();
    }

	#region integar cell map
	void CreateIntMap() {
		map = new int[rows,columns];
		tmpMap = new int[rows,columns];
 
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < rows; j++) {
				int rand = Random.Range(1,5);
				map[i,j] = rand;
				tmpMap[i,j] = map[i,j];
			}
		}
	}
	List<int> GetIntAdjacent(int x, int y)
	{
		List<int> tmp = new List<int> ();

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
							tmp.Add(map[x + i, y + j]);
						}
					}
				}
		catch (System.IndexOutOfRangeException e)
		{
			Debug.Log(e);
			Debug.Log("a: " + a + ". b: " + b);
		}

		return tmp;


	}
	// reemake the get neighbour methods to int map
	void CellularAutomata() {
		Debug.Log ("cell!");
		for (int x = 0; x < rows; x++) {
			for (int y = 0; y < columns; y++) {
				List<int> tmpList = GetIntAdjacent (x,y);

				var most = (from i in tmpList
				            group i by i into grp
				            orderby grp.Count() descending
				            select grp.Key).First();

//				UnityEngine.Debug.Log ("most: "+most);
				tmpMap[x,y] = most;
	
			}
		}
		for (int i = 0; i < rows; i++) {
			for(int j = 0; j < columns; j++) {
			map[i,j] = tmpMap[i,j];
			}
		}
	}
	// apply cellular automata to it. 
	// set tiles property to int map and set mats. 
	// no need for tmp map ***

	#endregion



	void SetMaterials() {
		// if (planet.cold || planet.habitable || planet.warm) 
		// set materials for ground1, ground2, ground3, liquid and walls (maybe even gas)
		Material ground1 = new Material(Resources.Load ("Materials/smoke", typeof(Material)) as Material);
		Material ground2 = new Material(Resources.Load ("Materials/smoke", typeof(Material)) as Material);
		Material ground3 = new Material(Resources.Load ("Materials/smoke", typeof(Material)) as Material);
		Material liquid  = new Material(Resources.Load ("Materials/smoke", typeof(Material)) as Material);
 		
		switch (gsh.GetCurrentPlanetType()) {
			case GameStateHandler.PlanetType.Cold:
				ground1 =	new Material(Resources.Load ("Materials/TileMats/habit_ground1", typeof(Material)) as Material);
				ground2 =	new Material(Resources.Load ("Materials/TileMats/cold_ground2", typeof(Material)) as Material);
				ground3 =	new Material(Resources.Load ("Materials/TileMats/cold_ground3", typeof(Material)) as Material);
				liquid =  	new Material(Resources.Load ("Materials/TileMats/cold_liquid", typeof(Material)) as Material);
				wallMat = 	new Material(Resources.Load ("Materials/TileMats/cold_wall", typeof(Material)) as Material);
			break;
			case GameStateHandler.PlanetType.Habitable:
				ground1 =	new Material(Resources.Load ("Materials/TileMats/habit_ground1", typeof(Material)) as Material);
				ground2 =	new Material(Resources.Load ("Materials/TileMats/habit_ground2", typeof(Material)) as Material);
				ground3 =	new Material(Resources.Load ("Materials/TileMats/habit_ground3", typeof(Material)) as Material);
				liquid =  	new Material(Resources.Load ("Materials/TileMats/habit_liquid", typeof(Material)) as Material);
				wallMat = 	new Material(Resources.Load ("Materials/TileMats/habit_wall", typeof(Material)) as Material);
			break;
			case GameStateHandler.PlanetType.Warm:
				ground1 =	new Material(Resources.Load ("Materials/TileMats/warm_ground1", typeof(Material)) as Material);
				ground2 =	new Material(Resources.Load ("Materials/TileMats/warm_ground2", typeof(Material)) as Material);
				ground3 =	new Material(Resources.Load ("Materials/TileMats/warm_ground3", typeof(Material)) as Material);
				liquid =  	new Material(Resources.Load ("Materials/TileMats/warm_liquid", typeof(Material)) as Material);
				wallMat = 	new Material(Resources.Load ("Materials/TileMats/warm_wall", typeof(Material)) as Material);
			break;

		
		}
		for(int x = 0; x < rows; x++) {
			for(int y = 0; y < columns; y++) {
				if(tmpMap[x,y] == 1) {
					TileMap[x,y].type = Tile.TileType.ground1;
					TileMap[x,y].gameObject.layer = 9; // Grass
					TileMap[x,y].SetTileMat(ground1);
				} else if (tmpMap[x,y] == 2) {
					TileMap[x,y].type = Tile.TileType.ground2;
					TileMap[x,y].gameObject.layer = 10; // Sand
					TileMap[x,y].SetTileMat(ground2);
				} else if (tmpMap[x,y] == 3) {
					TileMap[x,y].type = Tile.TileType.ground3;
					TileMap[x,y].gameObject.layer = 11; // Mud
					TileMap[x,y].SetTileMat(ground3);
				} else if (tmpMap[x,y] == 4){
					TileMap[x,y].type = Tile.TileType.liquid;
					TileMap[x,y].gameObject.layer = 10; // Unwalkable
					TileMap[x,y].SetTileMat(liquid);
					TileMap[x,y].blocked = true;
				}

			// not implemented yet
	
		   
				   
			}
			// if (planet.cold || planet.habitable || planet.warm) 
			// set materials for ground1, ground2, ground3, liquid and walls (maybe even gas)
		}
 	}
	private void AddResources() {
		for (int x = 0; x < rows; x++) {
			for (int y = 0; y < columns; y++) {
				if(!TileMap[x,y].blocked) {
					int rand = Random.Range (1,1025);
					if(rand == 1) {
						Instantiate(Resources.Load ("RedGasTile"),TileMap[x,y].transform.position,Quaternion.identity);
						Debug.Log ("red gas!");
					}
				}
			}
		}



	}
	private void SetColliderOnBlockedTiles (){
		for(int x = 0; x < rows; x++) {
			for(int y = 0; y < columns; y++) {
				if(TileMap[x,y].blocked && isNeighbourWalkable(x,y)) {
					TileMap[x,y].GetComponent<BoxCollider>().size = new Vector3(1.0f, 15.0f, 1.0f);
					Debug.Log ("fede!");
				}
			}
		}
	}
	public bool isNeighbourWalkable(int x, int y) {
		List<Tile> tmpList = new List<Tile> ();
		try {
			if(!TileMap[x-1,y].blocked ||  !TileMap[x,y-1].blocked || !TileMap[x+1,y].blocked || !TileMap[x,y+1].blocked )
			{
				return true;
			} else {
				return false;
			}
		} catch(System.IndexOutOfRangeException) {
			return false;
		}

	}
	#region helpers & software rot
 	private void AddWalls() {


		if (gsh.GetCurrentPlanetType () == GameStateHandler.PlanetType.Warm) {
			WallPrefab.transform.GetComponent<Renderer>().material = Resources.Load("Materials/TileMats/warm_wall", typeof(Material)) as Material;
		} else if (gsh.GetCurrentPlanetType () == GameStateHandler.PlanetType.Habitable) {
			WallPrefab.transform.GetComponent<Renderer>().material = Resources.Load("Materials/TileMats/habit_wall", typeof(Material)) as Material;
		} else if (gsh.GetCurrentPlanetType () == GameStateHandler.PlanetType.Cold) {
			WallPrefab.transform.GetComponent<Renderer>().material = Resources.Load("Materials/TileMats/cold_wall", typeof(Material)) as Material;
		}
		//WallPrefab.transform.GetComponent<Renderer>().material = Resources.Load("Materials/PlanetMats/moon", typeof(Material)) as Material;
		WallPrefab.transform.GetComponent<Tile> ().blocked = true;
		
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {

				try{ GetTile(i+1,j); } catch(System.IndexOutOfRangeException) {
					(Instantiate(WallPrefab, new Vector3(i+1, 0.5F, j), Quaternion.identity) as GameObject).transform.parent = transform;
				}
				try{ GetTile(i-1,j); } catch(System.IndexOutOfRangeException) {
					(Instantiate(WallPrefab, new Vector3(i-1, 0.5F, j), Quaternion.identity)as GameObject).transform.parent = transform;
				}
				try{ GetTile(i,j+1); } catch(System.IndexOutOfRangeException) {
					(Instantiate(WallPrefab, new Vector3(i, 0.5F, j+1), Quaternion.identity)as GameObject).transform.parent = transform;
				}
				try{ GetTile(i,j-1); } catch(System.IndexOutOfRangeException) {
					(Instantiate(WallPrefab, new Vector3(i, 0.5F, j-1), Quaternion.identity)as GameObject).transform.parent = transform;
				}
				try{ GetTile(i+1,j+1); } catch(System.IndexOutOfRangeException) {
					(Instantiate(WallPrefab, new Vector3(i+1, 0.5F, j+1), Quaternion.identity)as GameObject).transform.parent = transform;
				}
				try{ GetTile(i+1,j-1); } catch(System.IndexOutOfRangeException) {
					(Instantiate(WallPrefab, new Vector3(i+1, 0.5F, j-1), Quaternion.identity)as GameObject).transform.parent = transform;
				}
				try{ GetTile(i-1,j+1); } catch(System.IndexOutOfRangeException) {
					(Instantiate(WallPrefab, new Vector3(i-1, 0.5F, j+1), Quaternion.identity)as GameObject).transform.parent = transform;
				}
				try{ GetTile(i-1,j-1); } catch(System.IndexOutOfRangeException) {
					(Instantiate(WallPrefab, new Vector3(i-1, 0.5F, j-1), Quaternion.identity)as GameObject).transform.parent = transform;
				}
			}
		}


	}	

    public Tile GetTile(int x, int y)
    {
        return TileMap[x, y];
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
					UnityEngine.Debug.Log(e);
					UnityEngine.Debug.Log("a: " + a + ". b: " + b);
                }
        //foreach(Node n in tmpList) {
        //	n.transform.GetComponent<MeshRenderer>().material.color = Color.blue;
        //}
        return tmpList;
    }
	public List<Tile> GetNeighbours(Tile tile) {
		return GetNeighbours((int)tile.transform.position.x, (int)tile.transform.position.z);
	}
	public List<Tile> GetNeighbours(int x, int y) {
		List<Tile> tmpList = new List<Tile> ();
		try {
			tmpList.Add (TileMap[x-1,y]);
			tmpList.Add (TileMap[x,y-1]);
			tmpList.Add (TileMap[x+1,y]);
			tmpList.Add (TileMap[x,y+1]);
			tmpList.Add (TileMap[x,y]);
		} catch(System.IndexOutOfRangeException) {
		}
		return tmpList;
	}
	IEnumerator wait(float delay)
	{
		yield return new WaitForSeconds (delay);

	}
}
#endregion