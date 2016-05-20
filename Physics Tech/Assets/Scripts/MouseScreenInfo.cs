 using UnityEngine;
using System.Collections;

public class MouseScreenInfo : MonoBehaviour 
{
	public static Vector3 mouseWorldPos;
	public static Vector3 screenDim;

	// Use this for initialization
	void Awake () 
	{
		mouseWorldPos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane - 1));
		screenDim = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, Camera.main.farClipPlane - 1));
	}
	
	// Update is called once per frame
	void Update () 
	{
		mouseWorldPos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane - 1) );
		screenDim = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height, Camera.main.farClipPlane - 1));
	}
}
