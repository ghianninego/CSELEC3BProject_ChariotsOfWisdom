using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

	public static GameManagerScript Instance = null;

	public bool isBlackPlayer = false;
	public GameObject pieceClicked = null;
	public GameObject[] spaces;
	public LayerMask layerMask;
	public Camera[] cameras;
	public Camera topCam;
	public Text winnerLabel;
	public GameObject panel;

	public Text blackScore;
	public Text whiteScore;
	public Text playAgainText;
	public Text gameovertext;

	private Grid grid ;
	private bool isPaused = false;

	// Use this for initialization
	void Start () {
		Instance = this;
		grid = new Grid ();
	}
	
	// Update is called once per frame
	void Update () {
		blackScore.text = Settings.blackPoints+"";
		whiteScore.text = Settings.whitePoints+"";
		if (Settings.camSetting == 0) {
			topCam.gameObject.SetActive (false);
			if (isBlackPlayer) {
				cameras [0].gameObject.SetActive (false);
				cameras [1].gameObject.SetActive (true);
			} else {
				cameras [0].gameObject.SetActive (true);
				cameras [1].gameObject.SetActive (false);
			}
		} else {
			topCam.gameObject.SetActive (true);
			cameras [0].gameObject.SetActive (false);
			cameras [1].gameObject.SetActive (false);
		}

		if (Input.GetMouseButtonDown (0) && !isPaused) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			RaycastHit hit;

			if(Physics.Raycast(ray,out hit, 100)){
				if (pieceClicked == null) {
					//if clicked is a chess piece
					if (hit.transform.gameObject.tag != "Untagged" && hit.transform.gameObject.tag != "BoardSquare") {

						clickPiece (hit.transform.gameObject);

					} else {
						pieceClicked = null;
						grid.disableHighlights ();
					}
				} else {
					//check if clicked cell is placeable
					int index = 0;
					if (hit.transform.tag == "BoardSquare")
						index = getIndexOfCell (hit.transform.gameObject);
					else {
						if (!clickPiece (hit.transform.gameObject)) {
							index = getIndexOfCell (getBoardSquare (hit.transform.gameObject));
						}

					}
						int row = index / grid.gridSize;
						int col = index % grid.gridSize;
						if (grid.canPlaceInCell (row, col)) {
							GameObject cell = getBoardSquare (pieceClicked);
							int rowOrig = getIndexOfCell (cell) / grid.gridSize;
							int colOrig = getIndexOfCell (cell) % grid.gridSize;

						if (hit.transform.gameObject.tag != "BoardSquare") {
							Debug.Log(hit.transform.gameObject.tag);
							if (hit.transform.gameObject.tag == "Cross") {
								Debug.Log ("KO");
								gameOver ();
							}
							Destroy (hit.transform.gameObject);
						}
							grid.updateGrid (rowOrig, colOrig, row, col, pieceClicked.GetComponent<Piece> ().isBlack, pieceClicked.tag);



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
	//return true if player was able to click piece
	bool clickPiece(GameObject piece){
		//if black player clicked black piece

		if (isBlackPlayer && piece.GetComponent<Piece> ().isBlack) {
			grid.disableHighlights ();
			pieceClicked = piece;
			if (!highlightPlaceableCells ()) {
				pieceClicked = null;
				return false;
			}
			return true;

		} //if white player clicked white piece 
		else if (!isBlackPlayer && !piece.GetComponent<Piece> ().isBlack) {
			grid.disableHighlights ();
			pieceClicked = piece;
			if (!highlightPlaceableCells ()) {
				pieceClicked = null;
				return false;
			}
			return true;
		}
		return false;
	}


	//get square/cell piece g is in
	GameObject getBoardSquare(GameObject g){
		RaycastHit[] allHits;
		Ray r = new Ray (g.transform.position, Vector3.down);
		Debug.DrawRay (g.transform.position, Vector3.down,Color.white,20f);

		allHits = Physics.RaycastAll(r,200f,layerMask);
		for (int i = 0; i < allHits.Length ; i++) {
			if (allHits [i].transform.gameObject.tag == "BoardSquare") {
				return allHits [i].transform.gameObject;
				break;
			}
		}
		return null;
	}

	//get square/cell piece g is in
	GameObject getPiece(GameObject boardCell){
		RaycastHit[] allHits;
		Vector3 startPos = new Vector3 (boardCell.transform.position.x,boardCell.transform.position.y-0.5f,boardCell.transform.position.z);
		Ray r = new Ray (startPos, Vector3.down);

		allHits = Physics.RaycastAll(r,200f,layerMask);
		for (int i = 0; i < allHits.Length ; i++) {


			if (allHits [i].transform.gameObject.tag != "BoardSquare") {

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
	bool highlightPlaceableCells(){
		bool isMoveable = false;
		if (pieceClicked != null) {
			GameObject cell = getBoardSquare (pieceClicked);
			if (cell != null) {
				int row = getIndexOfCell (cell) / grid.gridSize;
				int col = getIndexOfCell (cell) % grid.gridSize;
				isMoveable = grid.setCanPlay (pieceClicked.gameObject.tag, row, col);
			} else {
				pieceClicked = null;
			}

		}
		return isMoveable;
	}

	//enable/disable highlight of grid[i,j]
	public void highlight(int row, int col,bool b){
		int index = row * grid.gridSize + col;
		spaces [index].GetComponent<MeshRenderer> ().enabled = b;
	}


	public void gameOver(){
		isPaused = true;
		if (isBlackPlayer) {
			winnerLabel.text = "Black player wins!";
			Settings.blackPoints++;
		} else {
			winnerLabel.text = "White player wins!";
			Settings.whitePoints++;
		}
		if (Settings.blackPoints < Settings.numOfGames && Settings.whitePoints < Settings.numOfGames) {
			gameovertext.text = "Round Over";
			playAgainText.text = "Next Round";

		} else {
			gameovertext.text = "GAME OVER";
			if (Settings.blackPoints == Settings.numOfGames) {
				winnerLabel.text = "Black player wins!";

			}
			else {
				winnerLabel.text = "White player wins!";
			}
			playAgainText.text = "Play Again";
		}
		panel.GetComponent<CanvasRenderer> ().SetAlpha (200);
		panel.SetActive (true);
	}

	public void reloadScene(){
		if (Settings.blackPoints >= Settings.numOfGames || Settings.whitePoints >= Settings.numOfGames) {
			Settings.blackPoints = 0;
			Settings.whitePoints = 0;
		}
		SceneManager.LoadScene ("GameScene");
	}
		
}
