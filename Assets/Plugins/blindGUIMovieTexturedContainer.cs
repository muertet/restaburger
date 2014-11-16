// TODO: Add Header
using UnityEngine;
using System.Collections;

/// <summary>
/// This class used as container for objects
/// </summary>
public class blindGUIMovieTexturedContainer : blindGUITexturedContainer {
	
	/// <summary>
	/// Background texture for container
	/// </summary>
	public bool m_autoPlay = false;
	public bool m_loop = false;
	
	/// <summary>
	/// Initialization
	/// </summary>
	public override void Start () {
		base.Start();
		if (m_autoPlay && m_backgroundTexture && (m_backgroundTexture.GetType() == typeof(MovieTexture))) {
			MovieTexture m_bacgkroundMovieTexture = m_backgroundTexture as MovieTexture;
			m_bacgkroundMovieTexture.Play();
			m_bacgkroundMovieTexture.loop = m_loop;
		}
	}
	
	/// <summary>
	/// Plays texture
	/// </summary>
	public void Play() {
		if (m_backgroundTexture && (m_backgroundTexture.GetType() == typeof(MovieTexture))) {
			MovieTexture m_bacgkroundMovieTexture = m_backgroundTexture as MovieTexture;
			m_bacgkroundMovieTexture.Play();
		}
	}
	
	/// <summary>
	/// Pauses texture
	/// </summary>
	public void Pause() {
		if (m_backgroundTexture && (m_backgroundTexture.GetType() == typeof(MovieTexture))) {
			MovieTexture m_bacgkroundMovieTexture = m_backgroundTexture as MovieTexture;
			m_bacgkroundMovieTexture.Pause();
		}
	}
	
	/// <summary>
	/// Stops play
	/// </summary>
	public void Stop() {
		if (m_backgroundTexture && (m_backgroundTexture.GetType() == typeof(MovieTexture))) {
			MovieTexture m_bacgkroundMovieTexture = m_backgroundTexture as MovieTexture;
			m_bacgkroundMovieTexture.Stop();
		}
	}
}
