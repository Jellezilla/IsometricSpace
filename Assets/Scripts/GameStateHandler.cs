using UnityEngine;
using System.Collections;

public class GameStateHandler : MonoBehaviour {

	private Texture2D GameLogo;
	private Material currentMat;

	public Material GetCurrentMat() {
		return currentMat;
	}
	public void SetCurrentMat(Material mat) {
		currentMat = mat;
	}
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
				StartCoroutine(ChangeLevel(2));

			} else {
				StartCoroutine(ChangeLevel(1));
			}
		}
	}
	IEnumerator ChangeLevel(int level) {
		float fadeTime = GameObject.Find ("SceneFader").GetComponent<Fading>().BegindFade(1);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel (level);
	}
	void OnGUI() {
		if (Application.loadedLevel == 0) {

			GUI.DrawTexture (new Rect(Screen.width/2-GameLogo.width/2, Screen.height/2-200, GameLogo.width, GameLogo.height), GameLogo);

			if (GUI.Button(new Rect(Screen.width/2-100, Screen.height/2-50, 200, 100), "Explore a new galaxy"))
				StartCoroutine(ChangeLevel(1));

			if(GUI.Button (new Rect(Screen.width/2-100, Screen.height/2+60, 200, 50), "Options"))
				Debug.Log ("you ain't got no motherfucking options bitch!");	

		}
	}
}
