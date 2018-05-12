using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Stack: MonoBehaviour {
//    public GameObject[] children;
//    public int index;

//    public Stack(int children){
        
//        for (var i = 0; i < transform.childCount; i++){
//            children[i] = transform.GetChild(i).gameObject;
//        }
//        index = children - 1;
        
//    }
//}

public class TheStack : MonoBehaviour {

	private const float BOUNDS_SIZE = 3f;

	public GameObject camera;

	private GameObject[] theStack;
    private GameObject currentTile;
	private int stackIndex;
	private int scoreCount = 0;

	private float tileTransition = 0.0f;
	private float tileSpeed = 3.0f;

	private float secondaryPosition = 0;
	private bool isMovingOnX = true;

	void Start () {
        createStack();

	}

    void createStack(){
        theStack = new GameObject[transform.childCount];
        for (var i = 0; i < transform.childCount; i++)
        {
            theStack[i] = transform.GetChild(i).gameObject;
        }
        stackIndex = transform.childCount - 1;
        currentTile = theStack[stackIndex];
    }

	void Update () {

		if( Input.GetMouseButtonDown(0) )
		{
			if ( PlaceTile() ) {
				updateCamera();
				SpawnTile();
                currentTile = theStack[stackIndex];
				scoreCount++;
			} else {
				EndGame();
			}

		}
		MoveTile ();

	}


	private void updateCamera()
	{
		float cameraX = camera.transform.localPosition.x;
		float cameraY = camera.transform.localPosition.y;
		float cameraZ = camera.transform.localPosition.z;

        camera.transform.localPosition = new Vector3(cameraX, cameraY+1, cameraZ);
	}

	private void MoveTile(){
        float pos1;
        float pos2;
        char axis;

        if (isMovingOnX){
            pos1 = scoreCount;
            pos2 = secondaryPosition;
            axis = 'x';
        } else {
            pos1 = secondaryPosition;
            pos2 = scoreCount;
            axis = 'z';
        }

        sineAnimation(
            gameObj: currentTile,
            bounds: BOUNDS_SIZE,
            speed: tileSpeed,
            axis: axis,
            primaryPos: pos1,
            secondaryPos: pos2);

	}

    float transition = 0;
    private void sineAnimation(GameObject gameObj, float bounds, float speed, char axis, float primaryPos = 0, float secondaryPos = 0 ) {
        transition += Time.deltaTime * speed;
        float sine = Mathf.Sin(transition) * bounds;

        if (axis == 'x')
            gameObj.transform.localPosition = new Vector3(sine, primaryPos, secondaryPos);
        else if (axis == 'y')
            gameObj.transform.localPosition = new Vector3(primaryPos, sine, secondaryPos);
        else
            gameObj.transform.localPosition = new Vector3(primaryPos, secondaryPos, sine);

    }

	private void SpawnTile(){
		stackIndex--;
		if (stackIndex < 0)
			stackIndex = transform.childCount-1;

		theStack[stackIndex].transform.localPosition = new Vector3 (0, scoreCount, 0);
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
