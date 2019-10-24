using UnityEngine;
using System.Collections;

public class PollPassingScript : MonoBehaviour {

	private GameControllerScript gameControllerScript;
	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameControllerScript = gameControllerObject.GetComponent<GameControllerScript> ();
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			gameControllerScript.add_score ();
            GetComponent<AudioSource>().Play();
		}
	}
}
