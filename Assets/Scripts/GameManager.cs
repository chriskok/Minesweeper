using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

	private GameObject[,] board;
	private bool[,] bombs;
	private int xBomb = 0;
	private int yBomb = 0;
	private float counter = 0f;

	public GameObject cell;
	public int xLength = 6;
	public int yLength = 10;
	public int bombAmount = 10;

	public Text cellCount;
	public Text timeTaken;
	public Text winText;
	public Button resetButton;

	public static int pointsLeft;

	void Start () {
		AddCells ();
		PlantMines ();
		AddValues ();
		pointsLeft = (xLength * yLength) - bombAmount;
		winText.text = "";
		resetButton.gameObject.SetActive(false);
	}

	void Update (){
		cellCount.text = "Cells Left: " + pointsLeft;
		counter += Time.deltaTime;
		timeTaken.text = "Time Taken: " + Mathf.RoundToInt(counter);
	}

	void AddCells(){
		board = new GameObject[xLength +2,yLength +2];
		for(int i = 0; i< xLength; i++){
			for(int j = 0; j< yLength; j++){
				board [i, j] = (GameObject)Instantiate (cell, new Vector3 (i, j, 0), Quaternion.identity);
				Slot slotScript = board [i, j].GetComponent<Slot> ();
				slotScript.Numbered (i, j);
			}
		}
	}

	void PlantMines(){
		bombs = new bool[xLength +1, yLength +1];

		for(int i = 0; i< bombAmount; i++){
			xBomb = Mathf.RoundToInt (Random.Range (0, xLength));
			yBomb = Mathf.RoundToInt (Random.Range (0, yLength));
			Slot slotScript = board [xBomb, yBomb].GetComponent<Slot> ();

			if (slotScript.isBomb == true) {
				i--;
				continue;
			} else {
				slotScript.Bomb (true);
				bombs [xBomb, yBomb] = true;
			}
		}
	}

	void AddValues(){
		for(int i = 0; i< xLength; i++){
			for(int j = 0; j< yLength; j++){
				int thisValue = 0;
				Slot slotScript = board [i, j].GetComponent<Slot> ();

				//Chunk of code checking the bombs around each cell
				if (slotScript.isBomb == false) {
					if (j >= 1 && (bombs [i, j - 1] == true))
						thisValue++;
					if (j <= yLength && (bombs [i, j+1] == true))
						thisValue++;
					if (i >= 1 && (bombs [i -1, j] == true))
						thisValue++;
					if (i <= xLength && (bombs [i + 1, j] == true))
						thisValue++;
					if (i >= 1 && j >= 1 && (bombs [i -1, j-1] == true))
						thisValue++;
					if (i <= xLength && j <= yLength && (bombs [i + 1, j+1] == true))
						thisValue++;
					if (i >= 1 && j <= yLength && (bombs [i -1, j +1] == true))
						thisValue++;
					if (i <= xLength && j >= 1 && (bombs [i + 1, j -1] == true))
						thisValue++;
				} 

				slotScript.AddValue (thisValue);
			}
		}
	}

	public void RevealInRange(int i, int j){

		if (j >= 1) {
			Slot slotScript = board [i, j - 1].GetComponent<Slot> ();
			if (slotScript.chosen == false) {
				slotScript.Reveal();
			}
		}
		if (j <= yLength) {
			if (board [i, j + 1] != null) {
				Slot slotScript = board [i, j + 1].GetComponent<Slot> ();
				if (slotScript.chosen == false) {
					slotScript.Reveal ();
				}
			}
		}
		if (i >= 1) {
			Slot slotScript = board [i - 1, j].GetComponent<Slot> ();
			if (slotScript.chosen == false) {
				slotScript.Reveal();
			}
		}
		if (i <= xLength){
			if (board [i + 1, j] != null) {
				Slot slotScript = board [i + 1, j].GetComponent<Slot> ();
				if (slotScript.chosen == false) {
					slotScript.Reveal ();
				}
			}
		}
		if (i >= 1 && j >= 1 ){
			Slot slotScript = board [i - 1, j - 1].GetComponent<Slot> ();
			if (slotScript.chosen == false) {
				slotScript.Reveal();
			}
		}
		if (i <= xLength && j <= yLength){
			if (board [i + 1, j + 1] != null) {
				Slot slotScript = board [i + 1, j + 1].GetComponent<Slot> ();
				if (slotScript.chosen == false) {
					slotScript.Reveal ();
				}
			}
		}
		if (i >= 1 && j <= yLength){
			if (board [i - 1, j + 1] != null) {
				Slot slotScript = board [i - 1, j + 1].GetComponent<Slot> ();
				if (slotScript.chosen == false) {
					slotScript.Reveal ();
				}
			}
		}
		if (i <= xLength && j >= 1){
			if (board [i + 1, j - 1] != null) {
				Slot slotScript = board [i + 1, j - 1].GetComponent<Slot> ();
				if (slotScript.chosen == false) {
					slotScript.Reveal ();
				}
			}
		}
	}

	public void EndGame(bool won){
		cellCount.gameObject.SetActive (false);
		timeTaken.gameObject.SetActive (false);
		resetButton.gameObject.SetActive(true);

		if (won) {
			winText.text = "Congrats! \nYou won in " + Mathf.RoundToInt (counter) + "seconds";
		} else if (!won) {
			winText.text = "GAME OVER! \nYou had just " + pointsLeft + " cells left";
		}
	}

	public void ResetGame(){
		SceneManager.LoadScene (0);
		Slot.gameOver = false;
	}

	public void ExitGame(){
		Application.Quit();
	}
}
