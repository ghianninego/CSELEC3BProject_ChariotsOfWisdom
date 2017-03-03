using UnityEngine;
using System.Collections;

public class Grid {

	public int gridSize = 8;
	string[,] grid;
	bool[,] canPlace;

	//constructor
	public Grid () {
		grid = new string[gridSize, gridSize];
		canPlace = new bool[gridSize, gridSize];
		for (int i = 0; i < gridSize; i++) {
			for (int j = 0; j < gridSize; j++) {
				grid [i, j] = "empty"; //initialize grid with empty cells
				canPlace[i,j] = false;
			}
		}
		//fill
		for (int i = 1; i < 7; i++) {
			grid [2, i] = "bCircle";
		}

		for (int i = 1; i < 7; i++) {
			grid [5, i] = "wCircle";
		}

		grid [1, 1] = "bSquare";
		grid [1, 6] = "bSquare";
		grid [1, 2] = "bTriangle";
		grid [1, 5] = "bTriangle";
		grid [1, 3] = "bOctagon";
		grid [1, 4] = "bCross";

		grid [6, 1] = "wSquare";
		grid [6, 6] = "wSquare";
		grid [6, 2] = "wTriangle";
		grid [6, 5] = "wTriangle";
		grid [6, 3] = "wOctagon";
		grid [6, 4] = "wCross";
	}
		
	public void setCanPlay(string pieceType, int row, int col){
		if (pieceType == "Square") {
			setCanPlaySquare (row, col);
		} else if (pieceType == "Triangle") {
			setCanPlayTriangle (row, col);
		} else if (pieceType == "Circle") {
			setCanPlayCircle (row, col);
		} else if (pieceType == "Cross") {
			setCanPlayCross (row, col);
		} else if (pieceType == "Octagon") {
			setCanPlaySquare (row, col);
			setCanPlayTriangle (row, col);
		}
	}
	bool sameColor(int row, int col){
		bool isBlack = (grid[row,col][0] == 'b')?true:false;
		bool isWhite = (grid[row,col][0] == 'w')?true:false;

		if (GameManagerScript.Instance.pieceClicked.GetComponent<Piece> ().isBlack && isBlack
		    || !GameManagerScript.Instance.pieceClicked.GetComponent<Piece> ().isBlack && isWhite)
			return true;
		else
			return false;
	}
	void setCanPlaySquare(int row, int col){
		for (int i = row + 1; i < gridSize; i++) {
			if (sameColor(i,col)) {
				break;
			}
			canPlace [i, col] = true;
			GameManagerScript.Instance.highlight (i, col,true);
			if (grid [i, col] != "empty") {
				break;
			}
		}
		for (int i = row - 1; i > -1; i--) {
			if (sameColor(i,col)) {
				break;
			}
			canPlace [i, col] = true;
			GameManagerScript.Instance.highlight (i, col,true);
			if (grid [i, col] != "empty") {
				break;
			}
		}

		for (int i = col + 1; i < gridSize; i++) {
			if (sameColor(row,i)) {
				break;
			}
			canPlace [row, i] = true;
			GameManagerScript.Instance.highlight (row,i,true);
			if (grid [row, i] != "empty") {
				break;
			}
		}
		for (int i = col - 1; i > -1; i--) {
			if (sameColor(row,i)) {
				break;
			}
			canPlace [row, i] = true;
			GameManagerScript.Instance.highlight (row,i,true);
			if (grid [row, i] != "empty") {
				break;
			}
		}
	}

	void setCanPlayTriangle(int row, int col){
		//upright
		int colDist = 1;
		for(int i = row-1; i>-1; i--){
			
			if (col + colDist < gridSize) {
				if (!sameColor (i, col + colDist)) {
					canPlace [i, col + colDist] = true;
					GameManagerScript.Instance.highlight (i, col + colDist, true);
				} else
					break;

			}

			colDist++;
		}
		//upleft
		colDist = 1;
		for (int i = row - 1; i > -1; i--) {
			if (col - colDist > -1) {
				if (!sameColor (i, col - colDist)) {
					canPlace [i, col - colDist] = true;
					GameManagerScript.Instance.highlight (i, col - colDist, true);
				} else
					break;
			}
			colDist++;
		}

		//down
		colDist = 1;
		for(int i = row+1; i<gridSize; i++){
			if (col + colDist < gridSize) {
				if (!sameColor (i, col + colDist)) {
					canPlace [i, col + colDist] = true;
					GameManagerScript.Instance.highlight (i, col + colDist, true);
				} else
					break;
			}
			colDist++;
		}

		colDist = 1;
		for (int i = row + 1; i < gridSize; i++) {
			if (col - colDist > -1) {
				if (!sameColor (i, col - colDist)) {
					canPlace [i, col - colDist] = true;
					GameManagerScript.Instance.highlight (i, col - colDist, true);
				}else
					break;
			}
			colDist++;
		}
	}

	void setCanPlayCircle(int row, int col){
		
		bool isBlack = GameManagerScript.Instance.pieceClicked.GetComponent<Piece> ().isBlack;

		//if black move downwards
		if (isBlack) {
			if(!sameColor(row + 1, col)){
				canPlace [row + 1, col] = true;
				GameManagerScript.Instance.highlight (row + 1, col,true);
			}
			if (!sameColor (row + 1, col - 1)) {
				canPlace [row + 1, col - 1] = true;
				GameManagerScript.Instance.highlight (row + 1, col - 1, true);
			}
			if (!sameColor (row + 1, col + 1)) {
				canPlace [row + 1, col + 1] = true;
				GameManagerScript.Instance.highlight (row + 1, col + 1, true);
			}
		} else {
			if (!sameColor (row - 1, col)) {
				canPlace [row - 1, col] = true;
				GameManagerScript.Instance.highlight (row - 1, col, true);
			}
			if (!sameColor (row - 1, col - 1)) {
				canPlace [row - 1, col - 1] = true;
				GameManagerScript.Instance.highlight (row - 1, col - 1, true);
			}
			if (!sameColor (row - 1, col + 1)) {
				canPlace [row - 1, col + 1] = true;
				GameManagerScript.Instance.highlight (row - 1, col + 1, true);
			}
		}

	}

	void setCanPlayCross(int row, int col){

		if(!sameColor(row + 1, col)){
			canPlace [row + 1, col] = true;
			GameManagerScript.Instance.highlight (row + 1, col,true);
		}
		if (!sameColor (row + 1, col - 1)) {
			canPlace [row + 1, col - 1] = true;
			GameManagerScript.Instance.highlight (row + 1, col - 1, true);
		}
		if (!sameColor (row + 1, col + 1)) {
			canPlace [row + 1, col + 1] = true;
			GameManagerScript.Instance.highlight (row + 1, col + 1, true);
		}
		if (!sameColor (row - 1, col)) {
			canPlace [row - 1, col] = true;
			GameManagerScript.Instance.highlight (row - 1, col, true);
		}
		if (!sameColor (row - 1, col - 1)) {
			canPlace [row - 1, col - 1] = true;
			GameManagerScript.Instance.highlight (row - 1, col - 1, true);
		}
		if (!sameColor (row - 1, col + 1)) {
			canPlace [row - 1, col + 1] = true;
			GameManagerScript.Instance.highlight (row - 1, col + 1, true);
		}
		if (!sameColor (row, col - 1)) {
			canPlace [row, col-1] = true;
			GameManagerScript.Instance.highlight (row, col-1,true);
		}
		if (!sameColor (row, col + 1)) {
			canPlace [row, col+1] = true;
			GameManagerScript.Instance.highlight (row, col+1,true);
		}
			
			


	}

	public void disableHighlights(){
		for (int i = 0; i < gridSize; i++) {
			for (int j = 0; j < gridSize; j++) {
				canPlace [i, j] = false;
				GameManagerScript.Instance.highlight (i, j,false);
			}
		}
	}

	public bool canPlaceInCell(int row, int col){
		return canPlace [row, col];
	}


	public void updateGrid(int origRow, int origCol, int destRow, int destCol, bool isBlack, string piece){
		grid [origRow, origCol] = "empty";
		grid [destRow, destCol] = ((isBlack) ? "b" : "w") + piece;
	}
}
