using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

    public float distanceToStar;
    public float orbitSpeed;
    public float rotationSpeed;
    
    private Transform orbitAround;

	// Use this for initialization
	void Start () {
        orbitAround = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void FixedUpdate()
    {
        transform.RotateAround(orbitAround.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);

    }
}
