using UnityEngine;
using System.Collections;

public static class Utils {
	public static int CardNumberToValue(int cardNumber) {
		if (cardNumber > 10) {
			return 10;
		} else if (cardNumber < 2) {
			return 11;
		} else {
			return cardNumber;
		}
	}
}