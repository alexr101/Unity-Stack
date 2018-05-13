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
    private const float STACK_ANIMATION_SPEED = 5f;

	//public GameObject camera;
    private GameObject[] theStack;
    private Vector2 stackBounds = new Vector2(BOUNDS_SIZE, BOUNDS_SIZE);
    private GameObject previousTile;
    private GameObject currentTile;
	private int stackIndex;
	private int scoreCount = 0;

	private float tileTransition = 0.0f;
	private float tileSpeed = 3.0f;

	private float secondaryPosition = 0;
	private bool isMovingOnX = true;
    private Vector3 newPos;

	void Start () {
        createStack();

	}

    void createStack(){
        newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        theStack = new GameObject[transform.childCount];
        for (var i = 0; i < transform.childCount; i++) {
            theStack[i] = transform.GetChild(i).gameObject;
        }
        stackIndex = 0; 
        currentTile = theStack[stackIndex];
    }

	void Update () {

		if( Input.GetMouseButtonDown(0) )
		{
			if ( ShouldPlaceTile() ) {
				//updateCamera();
                newPos = Vector3.down * (scoreCount + 1);
                SpawnTile();
                currentTile = theStack[stackIndex];
				scoreCount++;
			} else {
				EndGame();
			}

		}

		MoveTile();
        transform.position = Vector3.Lerp(this.transform.position, newPos, STACK_ANIMATION_SPEED*Time.deltaTime);
	}

    //private void updateStackPosition(){
    //    newPos = Vector3.down * scoreCount;
    //}

	//private void updateCamera(){
	//	float cameraX = camera.transform.localPosition.x;
	//	float cameraY = camera.transform.localPosition.y;
	//	float cameraZ = camera.transform.localPosition.z;

    //  camera.transform.localPosition = new Vector3(cameraX, cameraY+1, cameraZ);
	//}

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
			stackIndex = transform.childCount-1; // -2 because stack moves down

		theStack[stackIndex].transform.localPosition = new Vector3 (0, scoreCount, 0);
        theStack[stackIndex].transform.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

	}

    private bool ShouldPlaceTile(){

        //Transform t = theStack[stackIndex].transform;

        float delta;

        int previousIndex = (stackIndex == transform.childCount-1) ? transform.childCount - 1 : stackIndex + 1;
                                
        previousTile = theStack[previousIndex];
        //currentTile = theStack[stackIndex];
        if(isMovingOnX) {
            delta =  previousTile.transform.position.x - currentTile.transform.position.x;
            delta = Mathf.Abs(delta);
            if (delta >= stackBounds.x) return false;
            stackBounds.x -= delta;

        } else {
            delta = previousTile.transform.position.z - currentTile.transform.position.z;
            delta = Mathf.Abs(delta);
            if (delta >= stackBounds.y) return false;
            stackBounds.y -= delta;

        }
        // rescale tile below (to simulate the rest of it, spawn a new one for the remainder of the size)
        currentTile.transform.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

 

        float newPosX = (currentTile.transform.localPosition.x + previousTile.transform.localPosition.x) / 2;
        float newPosZ = (currentTile.transform.localPosition.z + previousTile.transform.localPosition.z) / 2;

        if(isMovingOnX) {
            secondaryPosition = newPosX;
            currentTile.transform.position = new Vector3(newPosX, currentTile.transform.position.y, currentTile.transform.position.z);
        } else {
            secondaryPosition = newPosZ;
            currentTile.transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y, newPosZ);
        }

        
		isMovingOnX = !isMovingOnX;			
		return true;
	}

	private void EndGame(){

	}
}
