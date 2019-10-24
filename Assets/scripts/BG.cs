using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour {

    public float speed;

	// Update is called once per frame
	void Update () {
        Vector2 v = new Vector2(Time.time * speed, 0.0f);
        GetComponent<Renderer>().material.mainTextureOffset = v;
	}
}
