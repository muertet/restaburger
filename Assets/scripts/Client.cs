using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Client : Photon.MonoBehaviour {


	private List<Vector3> waypoint = new List<Vector3>();
	public float speed = 3;        // walking speed between Waypoints
	public bool loop = false;            
	public float dampingLook = 6.0f;      // How slowly to turn
	private float pauseDuration = 0;      // How long to pause at a Waypoint
	
	private float curTime;
	private int currentWaypoint = 0;

	private MenuPlate order = null;
	private float orderTime = 0;
	private bool destroyOnFinish = false;
	private bool happy = true;
	private bool ready = false;

	private const int MAX_WAITING_TIME = 120;

	void OnDestroy () {
		ClientSpawner.currentClients--;
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Untagged") { return;}

		SpeechBubble bubble = gameObject.GetComponent<SpeechBubble> ();
		bubble.show();
	}

	[RPC]
	public void setup(int skin, bool canSeat, int seatNum) {
		transform.renderer.material = Resources.Load("Client/Materials/"+ skin) as Material;

		if (!canSeat) { // ups no free space let him in a loop.
			this.loop = true;
			// waypoints to look like a walking citizen
			waypoint.Add(new Vector3(-29.33569f, 0.5f, 0.9746358f));
			waypoint.Add(new Vector3(-6.996582f, 0.5f, 0.9746358f));
			this.ready = true;
			return;
		}
		
		// waypoints to join the restaurant
		waypoint.Add(new Vector3(-17.24557f, 0.5f, -0.09645343f));
		waypoint.Add(new Vector3(-17.24557f, 0.5f, -3.070881f));
		waypoint.Add(new Vector3(-14.98184f, 0.5f, -3.070881f));

		GameObject[] seats = GameObject.FindGameObjectsWithTag ("AvailableSeat");
		GameObject seat = seats[seatNum];
		
		waypoint.Add(new Vector3(seat.transform.position.x, 0.5f, seat.transform.position.z));
		seat.GetComponent<Seat> ().setClient(gameObject.GetComponent<Client>());
		this.ready = true;
	}

	void  Update () {
		if (!this.ready) { return;}

		if (PhotonNetwork.isMasterClient) {
			if (currentWaypoint < waypoint.Count) {
					followPoint ();
			} else if (destroyOnFinish) {
					PhotonNetwork.Destroy (gameObject);
			} else if (loop) {
					currentWaypoint = 0;
			} else if (order == null) {
					orderFood ();
			} else {
					checkDeliveryTime ();
			}
		} else {
			if (currentWaypoint < waypoint.Count) {
				followPoint ();
			} else if (loop) {
				currentWaypoint = 0;
			}
		}
	}

	public MenuPlate getOrder () {
		return this.order;
	}

	public bool isInLoop () {
		return this.loop;
	}

	[RPC]
	public void leaveRestaurant (bool happy) {
		this.happy = happy;
		waypoint.Add(new Vector3(-14.98184f, 0.5f, -3.070881f));
		waypoint.Add(new Vector3(-17.24557f, 0.5f, -3.070881f));
		waypoint.Add(new Vector3(-17.24557f, 0.5f, -0.09645343f));
		waypoint.Add(new Vector3(-29.33569f, 0.5f, 0.9746358f));
		destroyOnFinish = true;
	}

	public bool isHappy () {
		return this.happy;
	}

	void checkDeliveryTime () {
		if ((Time.time - this.orderTime) >= MAX_WAITING_TIME) {
			orderLeaveRestaurant(false);
		}
	}
	public void orderLeaveRestaurant (bool happy) {
		GetComponent<PhotonView>().RPC ("leaveRestaurant", PhotonTargets.AllBuffered, happy);
	}

	public bool isLeaving () {
		if (this.order == null) { // calm down! he hasnt ordered yet!
			return false;
		} else if ((Time.time - this.orderTime) >= MAX_WAITING_TIME || this.destroyOnFinish) {
			return true;
		}
		return false;

	}

	void orderFood () {
		List<MenuPlate> menu = FoodHelper.getMenu();
		int num = Random.Range (0, menu.Count);

		GetComponent<PhotonView>().RPC ("serOrder", PhotonTargets.AllBuffered, num, Time.time);
	}

	[RPC]
	void serOrder (int num, float time) {
		List<MenuPlate> menu = FoodHelper.getMenu();
		this.order = menu[num];
		this.orderTime = Time.time;
	}
	
	void followPoint () {

		Vector3 target = waypoint[currentWaypoint];
		target.y = transform.position.y; // Keep waypoint at character's height
		Vector3 moveDirection = target - transform.position;

		if (moveDirection.magnitude < 0.5f) {
			if (curTime == 0) {
				curTime = Time.time; // Pause over the Waypoint
			}
			if ((Time.time - curTime) >= pauseDuration) {
				currentWaypoint++;
				curTime = 0;
			}
		} else {        
			//Quaternion rotation = Quaternion.LookRotation(target);
			//transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);transform.LookAt(target);
			
			transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
		}    
	}
}
