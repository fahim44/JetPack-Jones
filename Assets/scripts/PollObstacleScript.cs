using UnityEngine;
using System.Collections;

public class PollObstacleScript : MonoBehaviour {
    public GameObject explotion_effect;

	private GameControllerScript gameControllerScript;
	// Use this for initialization
	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameControllerScript = gameControllerObject.GetComponent<GameControllerScript> ();
		}
	}
		
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
            Vector3 v3 = other.gameObject.transform.position;
            v3.z = -5.0f;
            Instantiate(explotion_effect, v3, other.gameObject.transform.rotation);
            GetComponent<AudioSource>().Play();
            Destroy (other.gameObject);
			gameControllerScript.stop_game ();
		}
	}
}
