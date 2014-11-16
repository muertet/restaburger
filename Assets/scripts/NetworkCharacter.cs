using UnityEngine;
using System;

public class NetworkCharacter : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
	private GameObject pickedUpObject;

	Animator anim;

	bool gotFirstUpdate = false;
	
	void Start () {
		anim = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine) {

			// show / hide chat
			if (Input.GetKey(KeyCode.Return)) {
				GameLogic.showChat = true;
			}

			// ESC menu
			if (Input.GetKey(KeyCode.Escape)) {

				// maybe user just wants to hide the chat
				if (GameLogic.showChat) {
					GameLogic.showChat = false;
				} else {
					if (GameLogic.CURRENT_VIEW == GameLogic.ESC_MENU) {
						GameLogic.CURRENT_VIEW = GameLogic.GAME;
					} else {
						GameLogic.CURRENT_VIEW = GameLogic.ESC_MENU;
					}
				}
			}

			// hold object / request more ingredients
			if (Input.GetKey("e") || Input.GetKey(KeyCode.Mouse0)) {

				// keep streaming object position
				if (pickedUpObject) {
					object[] args = new object[] { Camera.main.transform.position, Camera.main.transform.rotation, NetworkManager.myPhotonView.viewID };
					pickedUpObject.GetComponent<PhotonView>().RPC("setHold", PhotonTargets.AllBuffered, args);
					return;
				}
				
				Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

				Transform hitTransform;
				Vector3 hitPoint;
				bool isObject = false;
				GameObject newObject = null;

				
				hitTransform = FindClosestHitObject(ray, 100, out hitPoint);

				if (hitTransform == null) { return;}

				// hold objects
				if (Input.GetKey(KeyCode.Mouse0)) {
					// we hit an stacked object
					if (hitTransform.collider.gameObject.tag == "Stacked") {
						newObject = hitTransform.collider.gameObject.transform.parent.gameObject;
						isObject = true;
					} else if (hitTransform.collider.gameObject.tag == "DynamicObject") {
						newObject = hitTransform.collider.gameObject;
						isObject = true;
					}

					if (isObject) {
						if (newObject == null) {Debug.LogError ("Player tried to hold a non existent object");}

						// check if player is already holding an object
						if (pickedUpObject != null && pickedUpObject.GetInstanceID() != newObject.GetInstanceID()) {
							return;
						}
						pickedUpObject = newObject;
											
						object[] args = new object[] { Camera.main.transform.position, Camera.main.transform.rotation, NetworkManager.myPhotonView.viewID };
						pickedUpObject.GetComponent<PhotonView>().RPC("setHold", PhotonTargets.AllBuffered, args);

					} else if (hitTransform.collider.gameObject.name == "RequestButton") {
						RequestButton.requestFood();
					}
				} else { // button e got pressed

					switch (hitTransform.collider.gameObject.name) {
						case "RequestButton":
							RequestButton.requestFood();
						break;
						case "Crate(Clone)":
						case "Crate":
							Crate crate = hitTransform.gameObject.GetComponent<Crate>();
							crate.open();
						break;
					}
				}

			} else if (pickedUpObject != null) { //if player is not holding E but was picking up an object last frame
				unHoldItem();
			}
		} else {
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }

	public void unHoldItem () {
		pickedUpObject.GetComponent<PhotonView>().RPC("unHold", PhotonTargets.AllBuffered);
		pickedUpObject = null; //unset pickedObject
	}

	[RPC]
	private void SetPlayerTexture(int playerID, string textureName)
	{
		PhotonView.Find(playerID).transform.renderer.material = Resources.Load("Skins/Materials/" + textureName) as Material;
	}


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
			stream.SendNext(anim.GetFloat("Speed"));
			stream.SendNext(anim.GetBool("Jumping"));
        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			anim.SetFloat("Speed", (float)stream.ReceiveNext());
			anim.SetBool("Jumping", (bool)stream.ReceiveNext());

			if(gotFirstUpdate == false) {
				transform.position = this.correctPlayerPos;
				transform.rotation = this.correctPlayerRot;
				gotFirstUpdate = true;
			}

        }
    }

	Transform FindClosestHitObject (Ray ray, float distance, out Vector3 hitPoint) {
		RaycastHit[] hits = Physics.RaycastAll(ray);
		
		Transform closestHit = null;
		hitPoint = Vector3.zero;
		
		foreach (RaycastHit hit in hits) {
			if (hit.transform != this.transform && (closestHit == null || hit.distance < distance) ) {
				closestHit = hit.transform;
				distance = hit.distance;
				hitPoint = hit.point;
			}
		}
		return closestHit;
	}
}
