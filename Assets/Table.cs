using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table : MonoBehaviour {
	private Stack<DeckCard> deck = new Stack<DeckCard>();
	[SerializeField]
	private TablePhase phase = TablePhase.Draw;
	[SerializeField]
	private Transform playerHand, dealerHand;
	[SerializeField]
	private GameObject cardPrefab, actionButtons;
	private int cardsToBeDealt = 0;
	[SerializeField]
	private CardCounter playerCounter, dealerCounter;
	private bool busted;

	private void Start() {
		ShuffleShoe();
		StartCoroutine(DrawPhase(4, 0));
	}

	private void Update() {
		switch (phase) {
			case TablePhase.Player:
				PlayerPhase();
				break;
		}

		actionButtons.SetActive(phase == TablePhase.Player && !busted);
	}

	private IEnumerator DrawPhase(int cards, float delay) {
		yield return new WaitForSeconds(delay);
		phase = TablePhase.Draw;
		ClearTable();
		cardsToBeDealt = cards;
		dealerCounter.SetVisible(false);
		busted = false;
		while (cardsToBeDealt > 0) {
			yield return new WaitForSeconds(0.5f);
			DeckCard drawn = deck.Pop();
			Transform hand = (cardsToBeDealt % 2 > 0) ? dealerHand : playerHand;
			GameObject go = Instantiate<GameObject>(cardPrefab, hand);
			Card card = go.GetComponent<Card>();
			card.suit = drawn.suit;
			card.number = drawn.number;
			card.up = (cardsToBeDealt != 1);
			cardsToBeDealt--;
		}
		phase = TablePhase.Player;
	}

	private void PlayerPhase() {
		if (playerCounter.count > 21 && !busted) {
			busted = true;
			Debug.LogError("YOU BUSTED BIIIIAAAAATCH");
			StartCoroutine(DrawPhase(4, 1));
		}
	}

	private IEnumerator DealerPhase() {
		phase = TablePhase.Dealer;
		foreach (Card card in dealerHand.GetComponentsInChildren<Card>()) {
			card.up = true;
		}
		dealerCounter.SetVisible(true);
		yield return new WaitForSeconds(0.5f);
		while (dealerCounter.count < 17) {
			DeckCard drawn = deck.Pop();
			GameObject go = Instantiate<GameObject>(cardPrefab, dealerHand);
			Card card = go.GetComponent<Card>();
			card.suit = drawn.suit;
			card.number = drawn.number;
			dealerCounter.Count();
			yield return new WaitForSeconds(0.5f);
		}
		StartCoroutine(DrawPhase(4, 2.5f));
	}

	public void Hit() {
		if (phase == TablePhase.Player) {
			DeckCard drawn = deck.Pop();
			GameObject go = Instantiate<GameObject>(cardPrefab, playerHand);
			Card card = go.GetComponent<Card>();
			card.suit = drawn.suit;
			card.number = drawn.number;
		}
	}

	public void Stand() {
		if (phase == TablePhase.Player) {
			StartCoroutine(DealerPhase());
		}
	}

	public void ClearTable() {
		foreach (GameObject child in GameObject.FindGameObjectsWithTag("Card")) {
			Destroy(child);
		}
	}

	public void ShuffleShoe() {
		//Reset shoe
		deck.Clear();
		for (int d = 0; d < 6; d++) {
			for (int suit = 0; suit < 4; suit++) {
				for (int num = 1; num < 14; num++) {
					deck.Push(new DeckCard() {
						suit = (Suit)suit,
						number = num
					});
				}
			}
		}
		//Shuffle shoe
		var values = deck.ToArray();
		deck.Clear();
		System.Random rand = new System.Random();
		foreach (var value in values.OrderBy(x => rand.Next()))
			deck.Push(value);
	}
}

public enum TablePhase {
	Draw, Player, Dealer
}