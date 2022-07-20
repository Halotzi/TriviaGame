using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class ShowQuestion
{
    public string question_text;
    public int id;
    public string ans1_text;
    public string ans2_text;
    public string ans3_text;
    public string ans4_text;
    public int correct_ans_id;
}

public class ReciveQuestion : MonoBehaviour
{
    [SerializeField] DisplayQuestion displayQuestion;
    [SerializeField] Timer timer;
    [SerializeField] private ServerManager _serverManager;
    
    private ShowQuestion current_question;
    
    static public bool RaciveNextQuestion=true;

    public void GetQuestion(int QID)
    {
        StartCoroutine(_serverManager.GetRequest($"https://localhost:44331/api/Question?questionID={QID}", ConvertQuestion));
    }

    public void ConvertQuestion(string json)
    {
        if (json != null && json.Length > 0)
        {
            current_question = JsonUtility.FromJson<ShowQuestion>(json);
            displayQuestion.ShowQuestion(current_question);
        }
    }
}
