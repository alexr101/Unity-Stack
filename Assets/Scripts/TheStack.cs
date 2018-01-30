using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack : MonoBehaviour {

	private GameObject[] theStack;
	private int stackIndex;
	private int scoreIndex = 0;

	void Start () {
		theStack = new GameObject[transform.childCount];

		for (var i = 0; i < transform.childCount; i++) 
		{
			theStack [i] = transform.GetChild (i).gameObject;
		}

		stackIndex = transform.childCount - 1;


	}

	void Update () {
		if( Input.GetKey("down") )
		{
			SpawnTile ();
			scoreIndex++;
		}
	}

	private void SpawnTile(){

		theStack [stackIndex].transform.localPosition = new Vector3 (0, scoreIndex, 0);

		stackIndex--;
		if (stackIndex < 0)
			stackIndex = transform.childCount-1;
		


	}

	private void PlaceTile(){


	}
}
