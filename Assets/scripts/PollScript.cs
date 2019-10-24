using UnityEngine;
using System.Collections;

public class PollScript : MonoBehaviour {
	public float movement_speed;

	// Use this for initialization

	void Update() {
		transform.Translate(Vector3.left * movement_speed * Time.deltaTime);

		//transform.Translate(Vector3.left * movement_speed * Time.deltaTime, Space.World);
	}
}
