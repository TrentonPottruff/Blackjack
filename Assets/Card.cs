using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
public class Card : MonoBehaviour {
	public bool up = true;
	public Suit suit;
	public int number;

	private Image image;
	private bool prevUp;
	private Suit prevSuit;
	private int prevNum;

	private void Awake() {
		image = GetComponent<Image>();
	}

	private void Start() {
		UpdateSprite();
	}

	private void Update() {
		if ((suit != prevSuit) || (number != prevNum) || (up != prevUp)) {
			UpdateSprite();

			//Update previous details
			prevSuit = suit;
			prevNum = number;
			prevUp = up;
		}
	}

	private void UpdateSprite() {
		//Display new card details
		string location = (up) ? $"Playing Cards/Image/PlayingCards/{suit.ToString()}{number.ToString("00")}" : "Playing Cards/Image/PlayingCards/BackColor_Black";
		Debug.Log(location);
		Sprite sprite = Resources.Load<Sprite>(location);
		image.sprite = sprite;
	}
}

public enum Suit {
	Club, Diamond, Heart, Spade
}