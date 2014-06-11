using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour 
{
	public bool bDebug = true;
	public float Radius = 2.0f;
	public Vector3 [] pointA;

	// Returns the length and size of the waypoint array
	public float Length
	{
		get
		{
			return pointA.Length;
		}
	}

	// Returns the Vector3 position of a particular waypoint at a specified index in the array
	public Vector3 GetPoint (int index)
	{
		return pointA [index];
	}

	// Draws the path in the editor environment
	void OnDrawGizmos ()
	{
		if (!bDebug)
			return;
		for (int i = 0; i < pointA.Length; i++)
		{
			if (i + 1 < pointA.Length)
			{
				Debug.DrawLine (pointA [i], pointA [i + 1], Color.red);
			}
		}
	}

	// UNUSED 
	/*
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	*/
}
