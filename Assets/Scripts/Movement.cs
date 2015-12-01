using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
[RequireComponent (typeof (GunController))]
public class Movement : MonoBehaviour {

    Rigidbody rigid;
    private float h, v;
    public float maxSpeed;

	public float rotationSpeed = 450;
	public float walkSpeed = 2;
	public float runSpeed = 8;

	Animator anim;

	private Quaternion targetRotation;
	GunController gunController;
	
	private CharacterController controller;
	private Camera cam;

	// Use this for initialization
	void Start () {
        rigid = transform.GetComponent<Rigidbody>();
        //maxSpeed = 3.0F;
		gunController = GetComponent<GunController>();
		controller = GetComponent<CharacterController>();
		cam = Camera.main;

		anim = transform.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		ControlMouse();
		//ControlWASD();
		anim.SetFloat ("Forward", rigid.velocity.magnitude);
		//anim.SetFloat ("Turn", 
		
		// Old Gun controlls
		//		if (Input.GetButtonDown("Shoot")) {
		//			gun.Shoot();
		//		}
		//		else if (Input.GetButton("Shoot")) {
		//			gun.ShootContinuous();
		//		}
		

		// Weapon Input
		if (Input.GetMouseButton(0) || Input.GetKeyDown("space")) {
			gunController.OnTriggerHold();
		}
		
		if (Input.GetMouseButtonUp(0)) {
			gunController.OnTriggerRelease();
		}
	}
    void FixedUpdate()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        
        if (v > 0)
        {
            rigid.AddForce(GetIsoDir(Vector3.right), ForceMode.Impulse);
        } else if (v < 0)
        {
            rigid.AddForce(GetIsoDir(Vector3.right) * -1, ForceMode.Impulse);
        }

        if(h < 0)
        {
            rigid.AddForce(GetIsoDir(Vector3.forward), ForceMode.Impulse);
        } else if (h >   0)
        {
            rigid.AddForce(GetIsoDir(Vector3.forward) * -1, ForceMode.Impulse);
        }

        if (rigid.velocity.magnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }
    }
    public Vector3 GetIsoDir(Vector3 dir) {
        dir = Quaternion.Euler(0, -45, 0) * dir;
        return dir;
    }

	void ControlMouse() {
		
		Vector3 mousePos = Input.mousePosition;
		mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y,cam.transform.position.y - transform.position.y));
		targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x,0,transform.position.z));

		transform.eulerAngles = GetIsoDir(Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetRotation.eulerAngles.y,rotationSpeed * Time.deltaTime));
		
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical"));
		Vector3 motion = input;
		motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1)?.7f:1;
		//motion *= (Input.GetButton("Run"))?runSpeed:walkSpeed;
		motion += Vector3.up * -8;
		
		//controller.Move(motion * Time.deltaTime);

	}
}
