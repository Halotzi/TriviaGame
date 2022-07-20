using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Player
{
    public string name;
}

public class InsertPlayer : MonoBehaviour
{
    void Start()
    {
        GetQuestion("Liad");
    }

    private string current_json;

    public void GetQuestion(string name)
    {
        StartCoroutine(GetRequest("https://localhost:44331/api/InsertPlayer?name=" + name));
    }

    IEnumerator GetRequest(string uri)
    {
        current_json = "";

        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
                current_json = webRequest.downloadHandler.text;

                if (current_json != null && current_json == "1")
                {
                    Debug.Log("Success");
                }
                else
                {
                    Debug.Log("Faild");
                }
            }
        }
    }
}
