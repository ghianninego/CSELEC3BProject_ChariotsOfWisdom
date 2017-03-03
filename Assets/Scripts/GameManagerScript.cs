using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public static GameManagerScript Instance = null;

	public bool isBlackPlayer = false;
	public GameObject pieceClicked = null;
	public GameObject[] spaces;
	public LayerMask layerMask;

	private Grid grid ;

	// Use this for initialization
	void Start () {
		if (Instance == null) {
			Instance = this;
		}	
		grid = new Grid ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray,out hit, 100)){
				if (pieceClicked == null) {
					//if clicked is a chess piece
					if (hit.transform.gameObject.tag != "Untagged" && hit.transform.gameObject.tag != "BoardSquare") {

						//if black player clicked black piece

						if (isBlackPlayer && hit.transform.gameObject.GetComponent<Piece> ().isBlack) {
							grid.disableHighlights ();
							pieceClicked = hit.transform.gameObject;
							highlightPlaceableCells ();


						} //if white player clicked white piece 
						else if (!isBlackPlayer && !hit.transform.gameObject.GetComponent<Piece> ().isBlack) {
							grid.disableHighlights ();
							pieceClicked = hit.transform.gameObject;
							highlightPlaceableCells ();
							//Debug.Log (pieceClicked.name);
						}

					} else {
						pieceClicked = null;
						grid.disableHighlights ();
					}
				} else {
					//check if clicked cell is placeable
					int index;
					if(hit.transform.tag == "BoardSquare")
						index = getIndexOfCell (hit.transform.gameObject);
					else 
						index = getIndexOfCell (getBoardSquare(hit.transform.gameObject));
						int row = index / grid.gridSize;
						int col = index % grid.gridSize;
						if (grid.canPlaceInCell (row, col)) {
							GameObject cell = getBoardSquare (pieceClicked);
							int rowOrig = getIndexOfCell (cell) / grid.gridSize;
							int colOrig = getIndexOfCell (cell) % grid.gridSize;
							grid.updateGrid (rowOrig, colOrig, row, col, pieceClicked.GetComponent<Piece> ().isBlack, pieceClicked.tag);

						if (hit.transform.gameObject.tag != "BoardSquare") {
							Destroy (hit.transform.gameObject);
						}

							//move gameobject
							pieceClicked.transform.position = new Vector3(hit.transform.position.x,hit.transform.position.y+0.1f,hit.transform.position.z);
							grid.disableHighlights ();
							pieceClicked = null;
							isBlackPlayer = !isBlackPlayer;

						
					}
				}

					
			}
		}
	}


	//get square/cell piece g is in
	GameObject getBoardSquare(GameObject g){
		RaycastHit[] allHits;
		Ray r = new Ray (g.transform.position, Vector3.down);
		Debug.DrawRay (g.transform.position, Vector3.down,Color.white,20f);

		allHits = Physics.RaycastAll(r,200f,layerMask);
		Debug.Log (allHits.Length);
		for (int i = 0; i < allHits.Length ; i++) {
			if (allHits [i].transform.gameObject.tag == "BoardSquare") {
				
				return allHits [i].transform.gameObject;
				break;
			}
		}
		return null;
	}

	//get index of the gameobject g in array spaces
	int getIndexOfCell(GameObject g){
		return System.Array.IndexOf (spaces, g);
	}

	//highlight all clickable cells of clicked piece
	void highlightPlaceableCells(){
		if (pieceClicked != null) {
			GameObject cell = getBoardSquare (pieceClicked);
			if (cell != null) {
				int row = getIndexOfCell (cell) / grid.gridSize;
				int col = getIndexOfCell (cell) % grid.gridSize;
				grid.setCanPlay (pieceClicked.gameObject.tag, row, col);
			} else {
				pieceClicked = null;
			}

		}
	}

	//enable/disable highlight of grid[i,j]
	public void highlight(int row, int col,bool b){
		int index = row * grid.gridSize + col;
		spaces [index].GetComponent<MeshRenderer> ().enabled = b;
	}
		
}
