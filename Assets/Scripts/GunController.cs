using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour {

	public Transform weaponHold;
	//public Gun startingGun;

	// Next line is from older tutorial. And everything that's commented out is stuff from new tutorial.
	public Gun[] guns;
	//public Gun[] purchasedGuns;
	public List<Gun> purchasedGuns = null;


	//public Rect guiAreaRect = new Rect(0,0,0,0);

	Gun equippedGun;

	public void BuyRevolver() {
		purchasedGuns.CopyTo(0, guns, 0, 1);
	}

	public void BuyRifle() {
		purchasedGuns.CopyTo(1, guns, 1, 1);
	}

	public void BuyShotgun() {
		purchasedGuns.CopyTo(2, guns, 2, 1);
	}

	void Start() {
		//purchasedGuns.CopyTo(0, guns, 0, 1);
		/*if (startingGun != null) {*/
			EquipGun(/*startingGun*/ 0);
		/*}*/
	}


	void Update() {

		for (int i = 0; i < guns.Length; i++) {
			if (Input.GetKeyDown((i+1) + "") /*|| Input.GetKeyDown("{" + (i+1) + "}")*/) {
				EquipGun(i);
				break;
			}
		}
	}

	public void EquipGun(int i /*Gun gunToEquip*/) {
		if (equippedGun != null) {
			Destroy(equippedGun.gameObject);
		}

		// The gun will be instantiated on the "Weapon Hold" game object.
		equippedGun = Instantiate(/*gunToEquip*/ guns[i], weaponHold.position, weaponHold.rotation) as Gun;

		// Making the gun a child of "Weapon Hold", so it moves with the player.
		equippedGun.transform.parent = weaponHold;
	}


	public void OnTriggerHold() {
		// Checking if there's a weapon currently equipped.
		if (equippedGun != null) {
			equippedGun.OnTriggerHold();
		}
	}

	public void OnTriggerRelease() {
		// Checking if there's a weapon currently equipped.
		if (equippedGun != null) {
			equippedGun.OnTriggerRelease();
		}
	}
}
