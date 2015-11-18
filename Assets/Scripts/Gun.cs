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

	public Transform shell;
	public Transform shellEjection;
	MuzzleFlash muzzleflash;
	float nextShotTime;

	bool triggerReleasedSinceLastShot;
	// How many shots we've fired in our current burst.
	int shotsRemainingInBurst;

	void Start() {
		muzzleflash = GetComponent<MuzzleFlash>();
		shotsRemainingInBurst = burstCount;
	}


	void Shoot() {

		if (Time.time > nextShotTime) {

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

			GetComponent<AudioSource>().Play();

			// Iinstantiate new shell when we shoot.
			Instantiate(shell, shellEjection.position, shellEjection.rotation);
			
			// Activate muzzleflash. "Activate" method declared in "MuzzleFlash" script.
			muzzleflash.Activate();
		}
	}

	public void OnTriggerHold() {
		Shoot();
		triggerReleasedSinceLastShot = false;
	}

	public void OnTriggerRelease() {
		triggerReleasedSinceLastShot = true;
		shotsRemainingInBurst = burstCount;
	}
}
