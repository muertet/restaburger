using UnityEngine;
using System.Collections;

public class ClientSpawner : Photon.MonoBehaviour {

	public const int CLIENTS_LIMIT = 5;
	public const int INTERVAL_BETWEEN_SPAWN = 20; // secs
	
	public static int currentClients = 0;
	public static float lastSpawn = 0;
	public static int availableSkins = 2;

	// Update is called once per frame
	void Update () {
		if (PhotonNetwork.isMasterClient) {
			// check if we have to spawn any client
			if (currentClients < CLIENTS_LIMIT && (lastSpawn == 0 || (Time.time - lastSpawn) >= INTERVAL_BETWEEN_SPAWN )) {
				Transform spawn = GameObject.FindWithTag("ClientSpawn").transform;

				// choose skin
				int skin = Random.Range(1, availableSkins+1);
				bool canSeat = false;
				int seat = 0;

				// decide where will he seat
				GameObject[] seats = GameObject.FindGameObjectsWithTag ("AvailableSeat");

				if (seats.Length > 0) {
					seat = Random.Range(0, seats.Length);
					canSeat = true;
				}

				GameObject client = PhotonNetwork.Instantiate ("Client", new Vector3 (spawn.position.x, spawn.position.y + 0.20f, spawn.position.z), Quaternion.identity, 0);
				client.GetComponent<PhotonView>().RPC ("setup", PhotonTargets.AllBuffered, skin, canSeat, seat);

				currentClients++;
				lastSpawn = Time.time;
			}
		}
	}
}
