using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour {

	public int xVal;
	public int yVal;
	public bool isBomb = false;
	public Sprite[] sprites;
	public Color[] colors;
	public Color currentColor;
	public Color bombColor;
	public bool chosen = false;

	private bool marked = false;
	private int slotValue;
	private SpriteRenderer sr;
	private Sprite originalSprite;
	private Color originalColor = Color.white;

	public static bool gameOver = false;

	void OnAwake(){
		sr = GetComponent<SpriteRenderer> ();
		originalSprite = sprites [0];
	}

	public void Numbered (int xV, int yV) {
		xVal = xV;
		yVal = yV;
	}

	public void Bomb (bool bombCheck){
		if (bombCheck) {
			isBomb = true;
			sr = GetComponent<SpriteRenderer> ();
			originalSprite = sprites [3];
			sr.sprite = sprites [0];
			originalColor = bombColor;
			sr.color = Color.white;
		}
	}

	public void AddValue (int myValue){

		slotValue = myValue;
		sr = GetComponent<SpriteRenderer> ();

		if (!isBomb) {
			originalSprite = sprites [myValue + 4];
			sr.sprite = sprites [0];
			originalColor = colors [myValue];
			sr.color = Color.white;
		}
	}

	void OnMouseOver (){
		if (gameOver == false) {
			if (!marked)
				sr.color = currentColor;

			if (!chosen) {
				if (Input.GetButtonDown ("Fire2") && !marked) {
					marked = true;
					sr.sprite = sprites [2];
				} else if (Input.GetMouseButtonDown (1) && marked) {
					marked = false;
					sr.sprite = sprites [0];
				}
			}
		} 
	}

	void OnMouseDown(){
		if (!marked && !chosen && !gameOver) {
			if (Input.GetMouseButtonDown (0)) {
				Reveal ();
			} 

			if (GameManager.pointsLeft <= 0) {
				GameObject gm = GameObject.Find ("GM");
				GameManager gmScript = gm.GetComponent<GameManager> ();
				gmScript.EndGame (true);
				gameOver = true;
			}
		}
	}

	void OnMouseExit(){
		if (!gameOver) {
			if (!marked && !chosen) {
				sr.color = Color.white;
			} else if (marked) {
				sr.color = Color.white;
			} else if (chosen) {
				sr.color = originalColor;
			}
		}
	}

	public void Reveal(){
		sr.sprite = originalSprite;
		sr.color = originalColor;
		chosen = true;

		if (isBomb) {
			GameObject gm = GameObject.Find ("GM");
			GameManager gmScript = gm.GetComponent<GameManager> ();
			gmScript.EndGame (false);
			gameOver = true;
		} else {
			GameManager.pointsLeft--;
			if (slotValue == 0) {
				GameObject gm = GameObject.Find ("GM");
				GameManager gmScript = gm.GetComponent<GameManager> ();
				gmScript.RevealInRange (xVal, yVal);
			}
		}
	}
}
