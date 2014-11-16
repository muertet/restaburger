using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Truck : MonoBehaviour {

	private List<Vector3> waypoint = new List<Vector3>();

	private float speed = 3;
	private float pauseDuration = 60;
	private float curTime;
	private int currentWaypoint = 0;
	private bool cratesSpawned = false;

	// Use this for initialization
	void Start () {
		waypoint.Add(new Vector3(-18.99263f, 0.5f, 6.423897f));
		waypoint.Add(new Vector3(-44.28593f, 0.5f, 6.423894f));
	}
	
	// Update is called once per frame
	void Update () {
		if (currentWaypoint < waypoint.Count) {
			followPoint ();
		} else if(PhotonNetwork.isMasterClient) {
			PhotonNetwork.Destroy(this.gameObject);
		}
	}

	void followPoint () {
		
		Vector3 target = waypoint[currentWaypoint];
		target.y = transform.position.y; // Keep waypoint at character's height
		Vector3 moveDirection = target - transform.position;
		
		if (moveDirection.magnitude < 0.5f) {
			if (curTime == 0) {
				curTime = Time.time; // Pause over the Waypoint

				if (currentWaypoint == 0 && !cratesSpawned && PhotonNetwork.isMasterClient) { // spawn the crates inside vehicle
					PhotonNetwork.Instantiate ("Crate", new Vector3 (-12.3801f, 1.188603f, 3.291701f), Quaternion.identity, 0);
					PhotonNetwork.Instantiate ("Crate", new Vector3 (-12.00868f, 1.188603f, 4.092537f), Quaternion.identity, 0);
					PhotonNetwork.Instantiate ("Crate", new Vector3 (-11.11167f, 1.188603f, 4.230117f), Quaternion.identity, 0);
					PhotonNetwork.Instantiate ("Crate", new Vector3 (-10.31522f, 1.188603f, 4.230117f), Quaternion.identity, 0);
					cratesSpawned = true;
				}
			}

			if ((Time.time - curTime) >= pauseDuration || currentWaypoint == 1) {
				currentWaypoint++;
				curTime = 0;
			}
		} else {
			transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
		}    
	}
}
