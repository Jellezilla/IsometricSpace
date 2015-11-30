using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum MissionType{ Intel, Elimination };
public enum MissionRewardType{ Money };

public class Mission : MonoBehaviour {

	public MissionType missionType;
	public MissionRewardType rewardType;
	public bool completed {get; protected set;}
	protected bool rewardGiven = false;
	protected PlayerAttributes playerAttributes;
	public int rewardValue = 50;
	private Transform target;



	//protected Player

	// Use this for initialization
	public void Awake () {
		target = GameObject.FindGameObjectWithTag("MissionTarget").transform;
		playerAttributes = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
		playerAttributes.activeMissions.Add(this);
		playerAttributes.currentMission = this;
	}
	
	// Update is called once per frame
	public void Update () {

		CheckCompletion();
	}

	void CheckCompletion(){

	
		switch (missionType){

		case MissionType.Intel: 
			if (target.gameObject.GetComponent<Intel>() == null){
				return;
			}
			if (target.gameObject.GetComponent<Intel>().collected){
				completed = true;
				if (!rewardGiven)
					HandleReward();
			}

			break;
		case MissionType.Elimination:
			if (!target.GetComponent<EnemyAttributes>().alive){
				completed = true;
				if (!rewardGiven)
					HandleReward();
			}
			break;
		}
	}

	protected void HandleReward(){
		playerAttributes.currentMission = null;
		playerAttributes.completedMissions.Add(this);
		playerAttributes.activeMissions.Remove(this);
		rewardGiven = true;
		switch(rewardType){
		case MissionRewardType.Money:
			playerAttributes.cash += rewardValue;
			break;
		}
	}




}
