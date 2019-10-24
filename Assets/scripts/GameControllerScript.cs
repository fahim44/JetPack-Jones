using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {

	public GameObject poll;
	public Vector3 spawnValues;
	public float spawnWait;
	public float startWait;
	public Rigidbody2D player_rb;
	public Text startText;
	public Text scoreText;

    public GameObject handleScoreObject;

	private int score;
	private bool game_started,game_is_running;

    private AdmobScript admobScript;

	// Use this for initialization
	void Start () {
		game_started = false;
		game_is_running = false;
		player_rb.gravityScale = 0;
		score = -1;
		add_score ();
		startText.text = "Tap to Play";

        Cursor.visible = false;

        GameObject admobObject = GameObject.FindWithTag("admob");
        if (admobObject != null)
        {
            admobScript = admobObject.GetComponent<AdmobScript>();
        }

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        else if ((Input.GetButton("Jump") || Input.touchCount > 0) && !game_started ) {
			game_started = true;
			game_is_running = true;
			player_rb.gravityScale = 1;
			startText.text = "";
			StartCoroutine (SpawnPolls ());
		} /*else if ((Input.GetButton("Jump") || Input.touchCount > 0) && game_started && !game_is_running) {
			SceneManager.LoadScene ("flappy_box");
		}*/
	}

	IEnumerator SpawnPolls (){
		yield return new WaitForSeconds (startWait);

		while (game_is_running) {
			Vector3 spawnPosition = new Vector3 (spawnValues.x, Random.Range (-spawnValues.y, spawnValues.y), spawnValues.z);
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (poll, spawnPosition, spawnRotation);
			yield return new WaitForSeconds (spawnWait);
		}
	}

	public void stop_game(){
		game_is_running = false;
        Cursor.visible = true;
        handleScoreObject.SetActive(true);
        if (admobScript != null)
            admobScript.showBanner();
    }

	public bool is_game_running(){
		return game_is_running;
	}

	public void add_score(){
		score++;
		scoreText.text = "Score : " + score;
	}

    public int getScore()
    {
        return score;
    }

    public void RestartGame()
    {
        if (admobScript != null)
            admobScript.hideBanner();
        SceneManager.LoadScene("flappy_box");
    }
}
