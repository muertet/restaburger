using UnityEngine;
using System.Collections;

public class Seat : MonoBehaviour {

	public Client client = null;
	public ServeArea area;
	public GameObject screen;

	private bool rendered = false;
	

	void Update () {

		if (this.client == null) {return;}

		if (client.getOrder() != null && !this.rendered && screen != null) {
			MenuPlate order = client.getOrder();
			screen.renderer.material.mainTexture =  Resources.Load("Food/"+order.getImageName()) as Texture;
			screen.renderer.material.shader = Shader.Find("TextureDraw"); // make it shine!
			this.rendered = true;
		} else if (area.isServed ()) { // check if client has been served
			client.leaveRestaurant (true);
			area.restart ();
			this.releaseSeat ();
		} else if (client.isLeaving ()) { // looks like no one served him on time
			area.restart ();
			this.releaseSeat ();
		}
	}

	public void setClient (Client client) {
		this.client = client;
		gameObject.tag = "Seat";
	}

	public void releaseSeat () {
		this.client = null;
		gameObject.tag = "AvailableSeat";
		screen.renderer.material.mainTexture = null;
		screen.renderer.material.shader = Shader.Find("Diffuse");
		this.rendered = false;
	}
}
