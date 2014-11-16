using UnityEngine;
using System.Collections;

/*
	Thanks to DimasTheDriver for providing the initial script to work with
	http://www.41post.com/4545/programming/unity-how-to-create-a-speech-balloon
*/

[ExecuteInEditMode]
public class SpeechBubble : MonoBehaviour
{
	//this game object's transform
	private Transform goTransform;
	//the game object's position on the screen, in pixels
	private Vector3 goScreenPos;
	//the game objects position on the screen
	private Vector3 goViewportPos;
	
	//the width of the speech bubble
	public int bubbleWidth = 200;
	//the height of the speech bubble
	public int bubbleHeight = 100;
	
	//an offset, to better position the bubble
	public float offsetX = 0;
	public float offsetY = 150;
	
	//an offset to center the bubble
	private int centerOffsetX;
	private int centerOffsetY;
	
	//a material to render the triangular part of the speech balloon
	public Material mat;

	private GUIStyle currentStyle = null;
	private bool render = false;
	private float renderTime = 0;
	const int BUBBLE_DURATION = 15; // secs
	
	//use this for early initialization
	void Awake ()
	{
		//get this game object's transform
		goTransform = this.GetComponent<Transform>();
	}
	
	//use this for initialization
	void Start()
	{
		//if the material hasn't been found
		if (!mat)
		{
			Debug.LogError("Please assign a material on the Inspector.");
			return;
		}
		
		//Calculate the X and Y offsets to center the speech balloon exactly on the center of the game object
		centerOffsetX = bubbleWidth/2;
		centerOffsetY = bubbleHeight/2;
	}
	
	//Called once per frame, after the update
	void LateUpdate()
	{
		if (this.render) {
			//find out the position on the screen of this game object
			goScreenPos = Camera.main.WorldToScreenPoint(goTransform.position); 
			
			//Could have used the following line, instead of lines 70 and 71
			//goViewportPos = Camera.main.WorldToViewportPoint(goTransform.position);
			goViewportPos.x = goScreenPos.x/(float)Screen.width;
			goViewportPos.y = goScreenPos.y/(float)Screen.height;
			
			if ((Time.time - this.renderTime) >= BUBBLE_DURATION ) {
				this.render = false;
			}
		}
	}

	public void show () {
		this.render = true;
		this.renderTime = Time.time;
	}

	public void hide () {
		this.render = false;
	}

	//Draw GUIs
	void OnGUI()
	{
		if (this.render) {
			//Begin the GUI group centering the speech bubble at the same position of this game object. After that, apply the offset
			GUI.BeginGroup (new Rect (goScreenPos.x - centerOffsetX - offsetX, Screen.height - goScreenPos.y - centerOffsetY - offsetY, bubbleWidth, bubbleHeight));

			InitStyles ();
			GUI.contentColor = Color.black;

			//Render the round part of the bubble
			GUI.Label (new Rect (0, 0, 200, 100), "", currentStyle);

			Client client = gameObject.GetComponent<Client>();
			MenuPlate order = client.getOrder();

			if (client.isInLoop()) {
				GUI.Label(new Rect(10,25,190,50), "I don't give a fuck about your restaurant.");
			} else if (client.isLeaving()) {
				if (client.isHappy()) {
					GUI.Label(new Rect(10,25,190,50), "¡Excellent food!");
				} else {
					GUI.Label(new Rect(10,25,190,50), "This is the worst service i've never seen");
				}

			} else if (order == null) {
				GUI.Label(new Rect(10,25,190,50), "Let me seat before ordering please.");
			} else {
				GUI.DrawTexture(new Rect(50, 10, 90, 80), Resources.Load("Food/"+order.getImageName()) as Texture);
			}


			GUI.EndGroup ();
		}
	}
	
	//Called after camera has finished rendering the scene
	void OnRenderObject()
	{
		if (this.render) {
			//push current matrix into the matrix stack
			GL.PushMatrix ();
			//set material pass
			mat.SetPass (0);
			//load orthogonal projection matrix
			GL.LoadOrtho ();
			//a triangle primitive is going to be rendered
			GL.Begin (GL.TRIANGLES);

			//set the color
			GL.Color (Color.white);

			//Define the triangle vetices
			GL.Vertex3 (goViewportPos.x, goViewportPos.y + (offsetY / 3) / Screen.height, 0.1f);
			GL.Vertex3 (goViewportPos.x - (bubbleWidth / 3) / (float)Screen.width, goViewportPos.y + offsetY / Screen.height, 0.1f);
			GL.Vertex3 (goViewportPos.x - (bubbleWidth / 8) / (float)Screen.width, goViewportPos.y + offsetY / Screen.height, 0.1f);

			GL.End ();
			//pop the orthogonal matrix from the stack
			GL.PopMatrix ();
		}
	}
	private void InitStyles()
	{
		if( currentStyle == null )
		{
			currentStyle = new GUIStyle( GUI.skin.box );
			//currentStyle.normal.background = MakeTex( 2, 2, new Color( 255f, 255f, 255f, 1f ) );
			currentStyle.normal.background = Resources.Load("Textures/bubble") as Texture2D;
		}
	}
	
	private Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}
}