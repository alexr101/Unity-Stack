using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour {

	private const float BOUNDS_SIZE = 3f;

	public GameObject camera;

	private GameObject[] theStack;
	private int stackIndex;
	private int scoreCount = 0;

	private float tileTransition = 0.0f;
	private float tileSpeed = 3.0f;

	private float secondaryPosition = 0;

	private bool isMovingOnX = true;

	void Start () {
		theStack = new GameObject[transform.childCount];

		for (var i = 0; i < transform.childCount; i++) 
		{
			theStack [i] = transform.GetChild (i).gameObject;
		}

		stackIndex = transform.childCount - 1;


	}

	void Update () {

		if( Input.GetMouseButtonDown(0) )
		{
			if (PlaceTile ()) {
				updateCamera ();
				SpawnTile ();
				scoreCount++;
			} else {
				EndGame ();
			}

		}
		MoveTile ();

	}

	private void updateCamera()
	{
		float cameraX = camera.transform.localPosition.x;
		float cameraY = camera.transform.localPosition.y;
		float cameraZ = camera.transform.localPosition.z;

		camera.transform.localPosition = new Vector3 (cameraX, cameraY+1, cameraZ);

	}

	private void MoveTile(){
		tileTransition += Time.deltaTime * tileSpeed;

		if(isMovingOnX)
			theStack [stackIndex].transform.localPosition = new Vector3 (Mathf.Sin( tileTransition) * BOUNDS_SIZE, scoreCount, secondaryPosition);
		else 
			theStack [stackIndex].transform.localPosition = new Vector3 (secondaryPosition, scoreCount, Mathf.Sin( tileTransition) * BOUNDS_SIZE );

	}

	private void SpawnTile(){


		stackIndex--;
		if (stackIndex < 0)
			stackIndex = transform.childCount-1;

		theStack [stackIndex].transform.localPosition = new Vector3 (0, scoreCount, 0);




	}

	private bool PlaceTile(){

		Transform t = theStack [stackIndex].transform;

		secondaryPosition = (isMovingOnX) 
			? t.localPosition.x
			: t.localPosition.z;

		isMovingOnX = !isMovingOnX;			
		return true;
	}

	private void EndGame(){

	}
}
