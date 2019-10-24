using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;


public class HandleScore : MonoBehaviour {

    public GameObject leaderBoard_gameObject;
    public GameObject highScore_gameObject;
    public GameObject creditBoard_gameObject;

    public Text scoreItem_1_name, scoreItem_1_score, scoreItem_2_name, scoreItem_2_score, scoreItem_3_name, scoreItem_3_score;
    public InputField user_name;
   
    private int position = 4;
    private static string url = "baseUrl/index.php";
    private GameControllerScript gameControllerScript;

    // Use this for initialization
    void Start () {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameControllerScript = gameControllerObject.GetComponent<GameControllerScript>();
        }
        
       
    }

    private void OnEnable()
    {
        StartCoroutine(get_leaderboardDataFromServer());
    }


    IEnumerator get_leaderboardDataFromServer()
    {
        string st = "{\"game_id\" : \"180101\"}";
        UnityWebRequest www = GetWebRequest("leaderboard", st);
        yield return www.Send();
        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(www.responseCode);
            leaderBoard_gameObject.SetActive(true);
        }
        else
        {
            Handle_leaderboardData(www.downloadHandler.text);
            //Debug.Log(www.downloadHandler.text);
           // var n = JSON.Parse(www.downloadHandler.text);
            //var arr = JSONArray.Parse(n["data"].Value);
            //Debug.Log(arr[0]["score"].Value);
        }
    }

    private void Handle_leaderboardData(string jsonString)
    {
        var jsonObj = JSON.Parse(jsonString);
        int resultCode = jsonObj["result"].AsInt;
        if(resultCode == 0)
        {
            position = 1;
            ActiveHighScoreGameObject();
        }
        else
        {
            JSONNode arr = JSONArray.Parse(jsonObj["data"].Value);
            UpdateLeaderBoardItems(arr);
            position = DeterminePosition(arr, gameControllerScript.getScore());
            if (position < 4)
                ActiveHighScoreGameObject();
            else
                leaderBoard_gameObject.SetActive(true);
        }
    }

    private void ActiveHighScoreGameObject()
    {
        user_name.text = PlayerPrefs.GetString("name", "");
        highScore_gameObject.SetActive(true);
    }

    private int DeterminePosition(JSONNode arr, int score)
    {
        if (score <= 0)
            return 4;

        int pos = arr.Count+1;
        for(int i = arr.Count - 1; i >= 0; i--)
        {
            if (score > arr[i]["score"].AsInt)
            {
                pos = i + 1;
            }
            else
                break;
        }
        return pos;
    }

    private void UpdateLeaderBoardItems(JSONNode arr)
    {
        if (arr.Count >= 1)
        {
            UpdateScoreItem_1(arr[0]["user_name"].Value, arr[0]["score"].Value);
        }
        if (arr.Count >= 2)
        {
            UpdateScoreItem_2(arr[1]["user_name"].Value, arr[1]["score"].Value);
        }
        if (arr.Count >= 3)
        {
            UpdateScoreItem_3(arr[2]["user_name"].Value, arr[2]["score"].Value);
        }
    }

    private void UpdateScoreItem_1(string name, string score)
    {
        scoreItem_1_name.text = name;
        scoreItem_1_score.text = score;
    }

    private void UpdateScoreItem_2(string name, string score)
    {
        scoreItem_2_name.text = name;
        scoreItem_2_score.text = score;
    }

    private void UpdateScoreItem_3(string name, string score)
    {
        scoreItem_3_name.text = name;
        scoreItem_3_score.text = score;
    }
    private UnityWebRequest GetWebRequest(string optCode, string bodyJSONString)
    {
        UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] body = new System.Text.UTF8Encoding().GetBytes(bodyJSONString);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(body);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("optcode", optCode);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("User-Agent", "dummyAgent");
        www.SetRequestHeader("Cookie", string.Format("dummyCookie"));
        www.chunkedTransfer = false;
        return www;
    }

    private void Update_ScoreItem_after_scoreSubmission()
    {
        if (position == 1)
        {
            UpdateScoreItem_3(scoreItem_2_name.text, scoreItem_2_score.text);
            UpdateScoreItem_2(scoreItem_1_name.text, scoreItem_1_score.text);
            UpdateScoreItem_1(PlayerPrefs.GetString("name", ""), gameControllerScript.getScore().ToString());
        }
        else if (position == 2)
        {
            UpdateScoreItem_3(scoreItem_2_name.text, scoreItem_2_score.text);
            UpdateScoreItem_2(PlayerPrefs.GetString("name", ""), gameControllerScript.getScore().ToString());
        }
        else if (position == 3)
            UpdateScoreItem_3(PlayerPrefs.GetString("name", ""), gameControllerScript.getScore().ToString());
    }

    IEnumerator submit_result_to_server()
    {

        string st = "{\"user_name\":" + "\"" + PlayerPrefs.GetString("name", "") + "\","
            + "\"game_id\":\"180101\","
            + "\"score\":" + "\"" + gameControllerScript.getScore().ToString() + "\""
            +"}";
        UnityWebRequest www = GetWebRequest("submitscore", st);
        yield return www.Send();      
    }

    public void restart_game()
    {
        gameControllerScript.RestartGame();
    }

    public void submit_result()
    {
        if(user_name.text != "")
        {
            PlayerPrefs.SetString("name", user_name.text);
            Update_ScoreItem_after_scoreSubmission();
            StartCoroutine(submit_result_to_server());
            highScore_gameObject.SetActive(false);
            leaderBoard_gameObject.SetActive(true);
        }
    }

    public void show_creditView()
    {
        leaderBoard_gameObject.SetActive(false);
        creditBoard_gameObject.SetActive(true);
    }
}
