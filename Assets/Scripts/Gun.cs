using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public enum FireMode {Auto, Burst, Single};
	public FireMode fireMode;

	public Texture gunLogo = null;

	// The spawn position of the projectile. Array, so we can shoot multiple projectiles at once.
	public Transform[] projectileSpawn;
	// Which projectiles are we shooting.
	public Projectile projectile;
	// Rate of fire of the weapon.
	public float msBetweenShots = 100;
	// The speed at which the projectile will leave the gun.
	public float muzzleVelocity = 35;
	public int burstCount;

	[Header("Effects")]
	public Transform shell;
	public Transform shellEjection;
	MuzzleFlash muzzleflash;
	float nextShotTime;

	bool triggerReleasedSinceLastShot;
	// How many shots we've fired in our current burst.
	int shotsRemainingInBurst;

	[Header("Recoil")]
	public Vector2 kickMinMax = new Vector2(.05f, .2f);
	public Vector2 recoilAngleMinMax = new Vector2(3, 5);
	public float recoilMoveSettleTime = .1f;
	public float recoilRotationSettleTime = .1f;

	Vector3 recoilSmoothDampVelocity;
	float recoilRotSmoothDampVelocity;
	// How much the gun rotates upwards when firing.
	float recoilAngle;

	public float currentAmmo;
	public float currentClip;
	public float maxAmmo;
	public float maxClip;

	//From tutorial
//	public int totalAmmo = 40;
//	public int ammoPerMag = 10;
//	private int currentAmmoInMag;
//	private bool reloading;

//	[HideInInspector]
//	public AmmoGUI gui;

	void Start() {
		muzzleflash = GetComponent<MuzzleFlash>();
		shotsRemainingInBurst = burstCount;
		Restock();

//		currentAmmoInMag = ammoPerMag;

//		if (gui) {
//			gui.SetAmmoInfo(totalAmmo, currentAmmoInMag);
//		}
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.R))
		{

			if (currentAmmo > 0) {
				Reload();
			}

		}
	}

	void LateUpdate() {
		// Animate recoil
		transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
		// Reducing the recoil angle back to zero over time.
		//recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);

		//transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;
	}


	void Shoot() {



		if (Time.time > nextShotTime && currentClip != 0) {

			// Burst fire logic
			if (fireMode == FireMode.Burst) {
				// Can't shoot if no shots are remaining in burst.
				if (shotsRemainingInBurst == 0) {
					return;
				}
				// If there are shots remaining in burst, then decrement "shotsRemainingInBurst".
				shotsRemainingInBurst--;
			}

			// Single fire logic
			else if (fireMode == FireMode.Single) {
				// Can't shoot if trigger hasn't been released since last shot.
				if (!triggerReleasedSinceLastShot) {
					return;
				}
			}

			for (int i = 0; i < projectileSpawn.Length; i++) {

				// Making sure we can't shoot a projectile every frame (creating delay).
				nextShotTime = Time.time + msBetweenShots / 1000;
		
				// Instantiate new projectile when we shoot.
				Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
				newProjectile.SetSpeed(muzzleVelocity);
			}

//			if (gui) {
//				gui.SetAmmoInfo(totalAmmo, currentAmmoInMag);
//			}

			//Debug.Log(currentAmmoInMag + "       " + totalAmmo);

			GetComponent<AudioSource>().Play();

			// Iinstantiate new shell when we shoot.
			Instantiate(shell, shellEjection.position, shellEjection.rotation);
			
			// Activate muzzleflash. "Activate" method declared in "MuzzleFlash" script.
			muzzleflash.Activate();

			transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
			//recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
			//recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);

			//currentAmmoInMag -= 1;
		
			currentClip -= 1;
			currentAmmo -= 1;

			Debug.Log(currentClip);
			Debug.Log(currentAmmo);
		}
		
	}

	void Reload() {      
		
		StartCoroutine(ReloadWait());

		if (currentClip < maxClip)  // is clip full?
		{
			if(currentAmmo >= maxClip) // Reload full clip
			{
				
				//GetComponent<AudioSource>().PlayOneShot(reloadSound);
				//gunAnim.animation.CrossFade("ReloadGun");
				
				StartCoroutine(ReloadWait());
				
			} else if(currentAmmo < maxClip) // reload not completely full clip
			{
				//GetComponent<AudioSource>().PlayOneShot (reloadSound);
				
				StartCoroutine(ReloadWait());
				
				if(currentAmmo + currentClip >= maxClip)
				{
					StartCoroutine(ReloadWait());
				} else {
					currentClip += currentAmmo;
					currentAmmo = 0;
				}
			}
		}

	}


	public void Restock()
	{
		currentClip = 30;
		maxClip = 30;
		currentAmmo = 90;
		maxAmmo = maxClip + currentAmmo;
	}
	
	IEnumerator ReloadWait()
	{
		Debug.Log("Reload Now");
		yield return new WaitForSeconds(3.0F); 
		currentAmmo += currentClip; // add the remaining bullets in the clip to the total amount of bullets.
		currentClip = maxClip;      // put new bullets in current clip.
		currentAmmo -= maxClip;     // subtract the bullets just put in the clip, from the total amount of bullets.
	}

	//************************************************ From Tutorial ************************************************

//	public bool Reload() {
//		// If we have ammo left and our current mag isn't full, then allow the reload. Otherwise don't.
//		if (totalAmmo != 0 && currentAmmoInMag != ammoPerMag) {
//			reloading = true;
//			return true;
//		}
//			return false;
//	}
//
//	public void FinishReload() {
//		reloading = false;
//		currentAmmoInMag = ammoPerMag;
//		totalAmmo -= ammoPerMag;
//
//		if (totalAmmo < 0) {
//			currentAmmoInMag += totalAmmo;
//			totalAmmo = 0; 
//		}
//
//		if (gui) {
//			gui.SetAmmoInfo(totalAmmo, currentAmmoInMag);
//		}
//	}


	public void OnTriggerHold() {

		if(currentClip > 0)
		{
			Shoot();
		}

		triggerReleasedSinceLastShot = false;
	}

	public void OnTriggerRelease() {
		triggerReleasedSinceLastShot = true;
		shotsRemainingInBurst = burstCount;
	}
}
