using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public bool slowMotion = false;
	
	void Update () {
		Time.timeScale = slowMotion ? 0.3f : 1.0f;
	}
}
