using UnityEngine;
using System.Collections;

public class GameStateHandler : MonoBehaviour {

	private Texture2D GameLogo;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform);
		GameLogo = Resources.Load("textLogo") as Texture2D;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.G)) {
			if(Application.loadedLevel == 1)
			{
				Application.LoadLevel (2);
			} else {
				Application.LoadLevel (1);
			}
		}
	}
	void OnGUI() {
		if (Application.loadedLevel == 0) {

			GUI.DrawTexture (new Rect(Screen.width/2-GameLogo.width/2, Screen.height/2-200, GameLogo.width, GameLogo.height), GameLogo);

			if (GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-50, 200, 100), "Explore a new galaxy"))
				Application.LoadLevel(1);

			if(GUI.Button (new Rect(Screen.width/2-100, Screen.height/2+60, 200, 50), "Options"))
				Debug.Log ("you ain't got no motherfucking options bitch!");	

		}
	}
}
