using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class OrganizeQuestions : MonoBehaviour
{
    //All the game lenght options
    [SerializeField] private Button Short;
    [SerializeField] private Button Medium;
    [SerializeField] private Button Long;
    [SerializeField] private ServerManager _serverManager;
    [SerializeField] private GameLoopManager questionManager;

    [Tooltip("Dictionary of string and stack. The string= category name, stack contains some of the question ids of the category- depands on the game length")]
    public Dictionary<string, Stack<int>> questions = new Dictionary<string, Stack<int>>();

    [Tooltip("List category name, to help you find indexs in the dictionary")]
    public List<string> CategorysNames = new List<string>();

    [Tooltip("To follow my current Category")]
    private int currentCategory;

    [Tooltip("Help me to transfer data of max question of each category beetween methods")]
    private int MaxQuestionsPerCategory;

    //Actions
    public static Action<Dictionary<string, Stack<int>>> OnQuestionsReceived;
    public static Action<List<string>> OnCategorysNamesReceived;

    [Tooltip("The max question you gonna add to each stack, depends on your game's length choice")]
    public int MaxQuestionsFromDifficulty;

    //Counting categorys
    private int CountedCategorys;


    public void OrganizeList()
    {
        MaxQuestionsFromDifficulty = PlayerData.DifficultyLevel;
        StartCoroutine(_serverManager.GetRequest("https://localhost:44331/api/CountCategorys", GetAmountOfCategorys));
    }

    private void GetAmountOfCategorys(string json)
    {
        if (json != null && json.Length > 0)
        {
            CountedCategorys = int.Parse(json);
            
            for (int i = 1; i <= CountedCategorys; i++)
            {
               StartCoroutine(_serverManager.GetRequest("https://localhost:44331/api/CountQuestions?categoryID=" + i, SetMaxQuestionsPerCategory, i));
            }
        }
    }

    private void SetMaxQuestionsPerCategory(string json, int currentCategoryID)
    {
        if (json != null && json.Length > 0)
        {
            currentCategory = currentCategoryID;
            MaxQuestionsPerCategory = int.Parse(json);
            StartCoroutine(_serverManager.GetRequest("https://localhost:44331/api/GetQuestionsIdsOfCategory?categoryID=" + currentCategory, SetAmountOfQuestionsFromCategorys));
        }
    }

    private void SetAmountOfQuestionsFromCategorys(string json)
    {
        QuestionsIds questionsId = JsonUtility.FromJson<QuestionsIds>(json);
        Stack<int> questionStack = new Stack<int>();
        if (MaxQuestionsFromDifficulty > MaxQuestionsPerCategory)
            MaxQuestionsFromDifficulty = MaxQuestionsPerCategory;
        for (int i = 0; i < MaxQuestionsFromDifficulty; i++)
        {
            int idLocation = UnityEngine.Random.Range(0, questionsId.questionsIds.Count);
            questionStack.Push(questionsId.questionsIds[idLocation]);
            questionsId.questionsIds.Remove(questionsId.questionsIds[idLocation]);
        }
        CategorysNames.Add(questionsId.CategoryName);
        questions.Add(questionsId.CategoryName, questionStack);
        MaxQuestionsFromDifficulty=PlayerData.DifficultyLevel;
        if (questions.Count == CountedCategorys)
        {
            if (OnQuestionsReceived != null)
            {
                OnQuestionsReceived.Invoke(questions);
            }

            if (OnCategorysNamesReceived != null)
            {
                OnCategorysNamesReceived.Invoke(CategorysNames);
            }
        }
    }
}

[Serializable]
public class QuestionsIds
{
    public string CategoryName;
    public List<int> questionsIds = new List<int>();
}
