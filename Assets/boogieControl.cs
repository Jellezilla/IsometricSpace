using UnityEngine;
using System.Collections;

public class boogieControl : MonoBehaviour {


	public Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown ("b"))
		{
			anim.Play("boogie",-1,0f);
		}
			
	}
}
