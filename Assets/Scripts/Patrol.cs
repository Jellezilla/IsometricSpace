using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour {

	private enum PatrolBehaviorState { MovingToNextPatrolPoint = 0, WaitingForNextMove = 1 }
	
	public Transform[] patrolPoints;
	public float patrolWalkSpeed = 2.0f;
	public float delayAtPatrolPointMin = 1.0f;
	public float delayAtPatrolPointMax = 5.0f;
	
	private PatrolBehaviorState patrolBehaviorState;
	private int patrolPointIndex;
	private float timeToWaitBeforeNextMove;
	
	Vector3 directionVector;	
	Transform destination;
	
	void Start()
	{
		patrolPointIndex = 0;
		patrolBehaviorState = PatrolBehaviorState.MovingToNextPatrolPoint;
		timeToWaitBeforeNextMove = -1.0f; 
		destination = patrolPoints [patrolPointIndex+1];
		directionVector = destination.position - transform.position;
	}
	
	Vector3 GetCurrentDestination()
	{
		return patrolPoints[patrolPointIndex].position;
	}
	
	void ChooseNextDestination()
	{
		patrolPointIndex++;
		if (patrolPointIndex >= patrolPoints.GetLength(0))
		{
			patrolPointIndex = 0;
		}
	}
	
	
	void UpdateMovingToNextPatrolPoint()
	{
		Vector3 currentDestination = GetCurrentDestination();
		transform.forward = currentDestination - transform.position;
		transform.Translate(Vector3.forward * patrolWalkSpeed * Time.deltaTime);
		
		if ((GetCurrentDestination() - transform.position).magnitude < 0.3f)
		{
			timeToWaitBeforeNextMove = Random.Range(delayAtPatrolPointMin, delayAtPatrolPointMax);
			patrolBehaviorState = PatrolBehaviorState.WaitingForNextMove;
		}
	}
	
	void UpdateWaitingForNextMove()
	{
		timeToWaitBeforeNextMove -= Time.deltaTime;
		Quaternion newRot = Quaternion.LookRotation(destination.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * 10f);
		if (timeToWaitBeforeNextMove < 0.0f)
		{
			ChooseNextDestination();
			patrolBehaviorState = PatrolBehaviorState.MovingToNextPatrolPoint;
			
			if (patrolPointIndex == patrolPoints.GetLength(0)-1){
				destination = patrolPoints[0];
			}
			else if (patrolPointIndex == patrolPoints.GetLength(0)){
				destination = patrolPoints[1];
				
			}
			else{
				destination = patrolPoints[patrolPointIndex+1]; 
			}
		}
	}
	
	void Update()
	{
		
		if (patrolBehaviorState == PatrolBehaviorState.MovingToNextPatrolPoint)
		{
			UpdateMovingToNextPatrolPoint();
		}
		else if (patrolBehaviorState == PatrolBehaviorState.WaitingForNextMove)
		{
			UpdateWaitingForNextMove();
			
		}
	}
	
	void LateUpdate()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
}
