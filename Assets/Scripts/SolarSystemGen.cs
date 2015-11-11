using UnityEngine;
using System.Collections;
using UnityEditor;

public class SolarSystemGen : MonoBehaviour {


	// Use this for initialization
	void Start () {
		GenerateSolarSystem (8);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown (0)) {
			Debug.Log ("hej fede!");
			Ray ray  = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;



			if(Physics.Raycast (ray, out hit)) {
				if(hit.transform.tag == "Planet") {
					//Material tmpMat = hit.transform.GetComponent<Renderer>().material; // as Material;
					GameObject.Find ("GameStateHandler").GetComponent<GameStateHandler>().SetCurrentMat(hit.transform.GetComponent<Renderer>().material);
					Debug.Log ("iniating warp drive!");
					StartCoroutine(ChangeLevel (Application.loadedLevel+1));
//					Application.LoadLevel (Application.loadedLevel+1);
				}
			}
			Debug.Log ("planet not found!");
		}
	}

	IEnumerator ChangeLevel(int level) {
		float fadeTime = GameObject.Find ("SceneFader").GetComponent<Fading>().BegindFade(1);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel (level);
	}

	public void GenerateSolarSystem(int planetCount) {
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = new Vector3(15.0F, 2.5F, 15.0F);
		sphere.transform.SetParent (GameObject.FindWithTag("God").transform);
		sphere.transform.name = "Sun";
		Material newMat = Resources.Load("Materials/PlanetMats/sun", typeof(Material)) as Material;
		sphere.transform.localScale += new Vector3(2.0F, 2.0F, 2.0F);
		sphere.GetComponent<Renderer> ().material = newMat;

		for (int i = 1; i <= planetCount; i++) {
			// Instantiate planet (sphere)
			GameObject planet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			// Set the initial position of the new planet
			planet.transform.position = new Vector3(sphere.transform.position.x, sphere.transform.position.y, sphere.transform.position.z);

			// Set name and parent of new planet
			planet.name = ("planet"+i.ToString());
			planet.transform.SetParent (sphere.transform);
			planet.tag = "Planet";
			// Add the orbit script to the new planet
			Orbit orb = planet.AddComponent<Orbit>();

			// set distance to star
			orb.distanceToStar = 6.5F*i + Random.Range (5.0F, 8.0F); // the 6.5f value should be changed to the orb.distanceToStar of the previous planet

			// set the orbitSpeed of the new planet
			orb.orbitSpeed = (planetCount * 6.5F) - (Random.Range (0.7F, 0.9F) * orb.distanceToStar); 

			// set the size of the new planet
			//			planet.transform.localScale.Set(2,2,2);

			// Add trail to orbiting planet
			TrailRenderer tr = planet.AddComponent<TrailRenderer>();
			tr.time = orb.distanceToStar;

			tr.startWidth = 0.5F;
			tr.endWidth = 0.1F;
			tr.material = Resources.Load ("Materials/PlanetMats/TrailMat", typeof(Material)) as Material;


			// Set rotational speed of the new planet

			// Assign a new material to the new planet
			orb.rotationSpeed = 365;

			// get random material:
			planet.GetComponent<Renderer>().material = GetRandPlanetMat(orb.distanceToStar);

			// Add moons? 
		}
		
	}

	private Material GetRandPlanetMat(float dist) {
		Material _mat;
		int rand = Random.Range (1,3);
		_mat = Resources.Load ("Materials/PlanetMats/01LavaWorld", typeof(Material)) as Material;

		if (dist < 20.0f) {
			if(rand == 1) _mat = Resources.Load ("Materials/PlanetMats/01LavaWorld", typeof(Material)) as Material;
			else if(rand == 2) _mat = Resources.Load ("Materials/PlanetMats/Mercury", typeof(Material)) as Material;
			else if(rand == 3) _mat = Resources.Load ("Materials/PlanetMats/Venus", typeof(Material)) as Material;
		} else if (dist > 20.0f && dist < 30.0f) {
			if(rand == 1) _mat = Resources.Load ("Materials/PlanetMats/Earth", typeof(Material)) as Material;
			else if(rand == 2) _mat = Resources.Load ("Materials/PlanetMats/Earth1", typeof(Material)) as Material;
			else if(rand == 3) _mat = Resources.Load ("Materials/PlanetMats/Earth2", typeof(Material)) as Material;
		} else if (dist > 30.0f && dist < 45.0f) {
			if(rand == 1) _mat = Resources.Load ("Materials/PlanetMats/Mars", typeof(Material)) as Material;
			else if(rand == 2) _mat = Resources.Load ("Materials/PlanetMats/Jupiter", typeof(Material)) as Material;
			else if(rand == 3) _mat = Resources.Load ("Materials/PlanetMats/Saturn", typeof(Material)) as Material;
		} else if (dist > 45.0f) {
			if(rand == 1) _mat = Resources.Load ("Materials/PlanetMats/Uranus", typeof(Material)) as Material;
			else if(rand == 2) _mat = Resources.Load ("Materials/PlanetMats/Neptune", typeof(Material)) as Material;
			else if(rand == 3) _mat = Resources.Load ("Materials/PlanetMats/Moon", typeof(Material)) as Material;
		}

		return _mat;
	}




	[CustomEditor(typeof(SolarSystemGen))]
	class SunSystemBuilder : Editor {
		SolarSystemGen ssg = new SolarSystemGen();
		public int planetCount = 8;
		public override void OnInspectorGUI() {
			if (GUILayout.Button ("I am God!")) {
				ssg.GenerateSolarSystem(planetCount);
				Debug.Log("It's alive: " + target.name);
			}

		}
	}
}
