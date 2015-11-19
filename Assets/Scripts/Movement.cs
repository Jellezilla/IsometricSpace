using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    Rigidbody rigid;
    private float h, v;
    public float maxSpeed;


	// Use this for initialization
	void Start () {
        rigid = transform.GetComponent<Rigidbody>();
        //maxSpeed = 3.0F;
	}
	
	// Update is called once per frame
	void Update () {

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
}
