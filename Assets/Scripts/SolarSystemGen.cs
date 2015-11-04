using UnityEngine;
using System.Collections;
using UnityEditor;

public class SolarSystemGen : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GenerateSolarSystem(int planetCount) {
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = new Vector3(15.0F, 2.5F, 15.0F);
		sphere.transform.SetParent (GameObject.FindWithTag("God").transform);
		sphere.transform.name = "Sun";
		Material newMat = Resources.Load("Materials/sun", typeof(Material)) as Material;
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

			// Add the orbit script to the new planet
			Orbit orb = planet.AddComponent<Orbit>();

			// set distance to star
			orb.distanceToStar = 6.5F*i + Random.Range (5.0F, 8.0F);

			// set the orbitSpeed of the new planet
			orb.orbitSpeed = (planetCount * 6.5F) - (Random.Range (0.7F, 0.9F) * orb.distanceToStar); 

			// set the size of the new planet
			//			planet.transform.localScale.Set(2,2,2);

			// Add trail to orbiting planet
			TrailRenderer tr = planet.AddComponent<TrailRenderer>();
			tr.time = orb.distanceToStar;

			tr.startWidth = 0.5F;
			tr.endWidth = 0.1F;
			tr.material = Resources.Load ("Materials/TrailMat", typeof(Material)) as Material;


			// Set rotational speed of the new planet

			// Assign a new material to the new planet
			orb.rotationSpeed = 365;
			planet.GetComponent<Renderer>().material = Resources.Load ("Materials/Earth", typeof(Material)) as Material;

			// Add moons? 
		}
		
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
