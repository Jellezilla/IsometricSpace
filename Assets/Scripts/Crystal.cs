using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour {


	private int amount;

	public int GetAmount() {
		return amount;
	}

	public void SetAmount(int adj) {
		amount += adj;
		if (amount >= 0) {
			Destroy(gameObject);
		}
	}

}
