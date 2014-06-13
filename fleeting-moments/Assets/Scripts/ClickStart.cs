using UnityEngine;
using System.Collections;

public class ClickStart : MonoBehaviour {
	public string levelToLoad = "01_scene";
	// Use this for initialization
	void Start () {
		StartCoroutine("splashScreen");
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetMouseButtonDown(0))
		{
			Application.LoadLevel (levelToLoad);
		}
	}
}
