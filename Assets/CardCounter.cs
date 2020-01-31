using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CardCounter : MonoBehaviour {
	[SerializeField]
	private Transform hand;
	private int prevCards;
	public int count;
	private TextMeshProUGUI counter;

	private void Start() {
		counter = GetComponent<TextMeshProUGUI>();
	}

	private void Update() {
		var numCards = hand.childCount;
		if (numCards != prevCards) {
			Count();
			prevCards = numCards;
		}
	}

	public void Count() {
		count = 0;
		foreach (Card card in hand.GetComponentsInChildren<Card>()) {
			count += Utils.CardNumberToValue(card.number);
			counter.text = $"{count}";
		}
	}

	public void SetVisible(bool visible) {
		counter.enabled = visible;
	}
}