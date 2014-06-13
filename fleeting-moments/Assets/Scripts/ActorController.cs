using UnityEngine;
using System.Collections;

public class ActorController : MonoBehaviour 
{
	public Transform [] controlPath;

	public Transform actor;
	public enum Direction {Forward, Reverse};
	
	private float pathPosition = 0;
	private RaycastHit hit;
	public float speed = 0.2f;
	private float rayLength = 5;
	private Direction characterDirection;
	private Vector3 floorPosition;	
	private float lookAheadAmount = 0.01f;
	private float ySpeed = 0;
	private float gravity = 0.5f;
	private float jumpForce = 0.12f;
	private uint jumpState = 0; // 0 = grounded 1 = jumping

	public bool goRight = false;
	public bool goLeft = false;

	void OnDrawGizmos ()
	{
		iTween.DrawPath (controlPath, Color.red);	
	}	
	
	// Use this for initialization
	void Start ()
	{
		// Plop the character pieces in the "Ignore Raycast" layer so we don't have false raycast data:	
		foreach (Transform child in actor) 
		{
			child.gameObject.layer = 2;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		DetectDirection ();
		FindFloorAndRotation ();
		MoveActor ();
		// MoveCamera ();
	}
	
	
	void DetectDirection ()
	{
		// Forward path movement:
		if (goRight)
		{
			characterDirection = Direction.Forward;	
		}
		if (goRight) 
		{
			pathPosition += Time.deltaTime * speed;
		}
		
		// Reverse path movement:
		if (goLeft)
		{
			characterDirection = Direction.Reverse;
		}
		if (goLeft) 
		{
			// Handle path loop around since we can't interpolate a path percentage that's negative(well duh):
			float temp = pathPosition - (Time.deltaTime * speed);
			if (temp < 0)
			{
				pathPosition = 1;	
			}
			else
			{
				pathPosition -= (Time.deltaTime * speed);
			}
		}	
		
		// Jump:
		/*
		if (Input.GetKeyDown ("space") && jumpState == 0) {
			ySpeed -= jumpForce;
			jumpState = 1;
		}
		*/
	}
	
	
	void FindFloorAndRotation ()
	{
		float pathPercent = pathPosition % 1;
		Vector3 coordinateOnPath = iTween.PointOnPath (controlPath,pathPercent);
		Vector3 lookTarget;
		
		// Calculate look data if we aren't going to be looking beyond the extents of the path:
		if(pathPercent - lookAheadAmount >= 0 && pathPercent + lookAheadAmount <= 1)
		{
			
			// Leading or trailing point so we can have something to look at:
			if (characterDirection == Direction.Forward)
			{
				lookTarget = iTween.PointOnPath (controlPath, pathPercent + lookAheadAmount);
			}else{
				lookTarget = iTween.PointOnPath (controlPath, pathPercent - lookAheadAmount);
			}
			
			// Look:
			actor.LookAt(lookTarget);
			
			// Nullify all rotations but y since we just want to look where we are going:
			float yRot = actor.eulerAngles.y;
			actor.eulerAngles = new Vector3(0, yRot, 0);
		}
		
		if (Physics.Raycast (coordinateOnPath, -Vector3.up, out hit, rayLength))
		{
			Debug.DrawRay (coordinateOnPath, -Vector3.up * hit.distance);
			floorPosition = hit.point;
		}
	}
	
	
	void MoveActor ()
	{
		// Add gravity:
		ySpeed += gravity * Time.deltaTime;
		
		// Apply gravity:
		actor.position = new Vector3 (floorPosition.x, actor.position.y - ySpeed, floorPosition.z);
		
		// Floor checking:
		if (actor.position.y < floorPosition.y){
			ySpeed = 0;
			jumpState = 0;
			actor.position = new Vector3 (floorPosition.x, floorPosition.y, floorPosition.z);
		}		
	}
	
	
	void MoveCamera ()
	{
		iTween.MoveUpdate (Camera.main.gameObject, new Vector3 (actor.position.x, 2.7f, actor.position.z - 5f), 0.9f);	
	}
}
