using UnityEngine;
using System.Collections;

public class GameLoader : MonoBehaviour {
	public GameObject manager;
	// Use this for initialization
	void Awake () {
		if (GameManager.instance == null) {
			Instantiate(manager, new Vector3(0,0,0), Quaternion.identity); 
		}
	}
}
