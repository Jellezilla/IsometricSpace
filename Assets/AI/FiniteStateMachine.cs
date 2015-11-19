using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Patrol))]
[RequireComponent(typeof (Unit))]


public class FiniteStateMachine : MonoBehaviour {

	public enum State{Patrol, Chase, Attack, Resign};

	Unit unit;
	Patrol patrol;
	public State state;
	Transform player;
	float chaseDistance = 10f;
	Vector3 lastKnownPlayerPos;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Target").transform;
		state = State.Patrol;
		unit = GetComponent<Unit>();
		patrol = GetComponent<Patrol>();

	}
	
	// Update is called once per frame
	void Update () {
		float distanceToPlayer = Vector3.Distance(transform.position, player.position);
		if (distanceToPlayer <= chaseDistance){

			RaycastHit hit;
			if(Physics.Raycast(transform.position, (player.position-transform.position).normalized, out hit, chaseDistance)){

				Debug.DrawRay(transform.position, (player.position-transform.position).normalized*chaseDistance, Color.green);

			}
			state = State.Chase;

		}
		else {
			if (state == State.Chase)
				state = State.Resign;
		}

		switch(state){

		case State.Patrol:
			StartCoroutine(setPatrol());
			break;
		case State.Resign:
			patrol.shouldPatrol = true;
			patrol.ResumePatrolling();
			break;
		case State.Chase:
			unit.StopPathFollowing();
			patrol.shouldPatrol = false;
			transform.forward = player.position - transform.position;
			transform.Translate(Vector3.forward * 10 * Time.deltaTime);
			break;
		}
	}

	IEnumerator setPatrol(){
		yield return new WaitForSeconds(.5f);
		patrol.shouldPatrol = true;
	}
}
