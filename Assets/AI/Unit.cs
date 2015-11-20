using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	
	private Transform target;
	public float speed = 2;
	Vector3[] path;
	int targetIndex;
	public bool destinationReached = false;
	public bool start = false;

	void Start(){
	
	}
	
	public void MakeRequest(Vector3 target){
		PathRequestManager.RequestPath(transform.position, target, OnPathFound);
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccesful){
		
		if (pathSuccesful){
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}
	
	public void StopPathFollowing(){
		StopCoroutine("FollowPath");
		
	}

	IEnumerator FollowPath(){
		if (path.Length > 0){
			Vector3 currentWaypoint = path[0];
			Vector3 lastWaypoint = path[path.Length-1];
			targetIndex = 0;
			while (true){
				if (transform.position == currentWaypoint){
					targetIndex++;
					if (targetIndex >= path.Length){
						yield break;
					}
					currentWaypoint = path[targetIndex];
				}
				transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
				
				if (transform.position == lastWaypoint ){
					destinationReached = true;
				}
				yield return null; 
			}
			yield return null;
		}
		
	}

	public void OnDrawGizmos(){
		
		if (path != null){
			
			for (int i = targetIndex; i < path.Length; i++){
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one/3);
				
				if (i == targetIndex){
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1], path[i]);
				}
			}
		}
	}
}