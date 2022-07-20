using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour
{
    public IEnumerator GetRequest(string uri, Action<string> OnCompleteRequest)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (OnCompleteRequest != null)
                {
                    Debug.Log("Received: " + webRequest.downloadHandler.text);
                    OnCompleteRequest.Invoke(webRequest.downloadHandler.text);
                }
            }
        }
    }

    internal string GetRequest(string v)
    {
        throw new NotImplementedException();
    }

    public IEnumerator GetRequest(string uri, Action<string, int> OnCompleteRequest, int currentCategoryID)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (OnCompleteRequest != null)
                {
                    Debug.Log("Received: " + webRequest.downloadHandler.text);
                    OnCompleteRequest.Invoke(webRequest.downloadHandler.text, currentCategoryID);
                }
            }
        }
    }
}

