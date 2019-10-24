using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

	public float thrust;
	public Rigidbody2D rb;
    public GameObject fire_particle;

	private GameControllerScript gameControllerScript;

	void Start() {
		//rb = GetComponent<Rigidbody2D>();

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameControllerScript = gameControllerObject.GetComponent<GameControllerScript> ();
		}
	}

	void FixedUpdate() {
		if ((Input.GetButton ("Jump") || Input.touchCount>0 ) && gameControllerScript.is_game_running()) {
			rb.AddForce (transform.up * thrust);
            fire_particle.SetActive(true);
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
		}
        else
        {
            fire_particle.SetActive(false);
            if (GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Stop();
            }
        }
	}
}
