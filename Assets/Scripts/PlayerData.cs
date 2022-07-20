using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : MonoBehaviour
{
    [SerializeField] ServerManager _serverManager;

    public static string PlayerName=null;
    public static int DifficultyLevel=0;
    public static int PlayerScore=0;

    public void SendData()
    {
        StartCoroutine(_serverManager.GetRequest($"https://localhost:44331/api/InsertPlayer?name={PlayerName}&Difficulty={DifficultyLevel}&Score={PlayerScore}", ClearData));
    }

    public void ClearData(string json)
    {
        int log=1;
        if (log== int.Parse(json))
        {
            PlayerName = null;
            DifficultyLevel = 0;
            PlayerScore = 0;
        }
    }
}
