﻿using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {


	public Animator anim;

	void Start () 
	{
		anim = GetComponent < Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown ("1")) 
		{
			anim.Play ("hip_hop_dancing",-1,0f);
		}
		if (Input.GetMouseButtonDown (0)) 
		{
			anim.Play ("shooting",-1,0f);
		}
		if (Input.GetKeyDown ("3")) 
		{
			anim.Play ("flying_back_death",-1,0f);
		}
	}
}