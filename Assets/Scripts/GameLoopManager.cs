using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameLoopManager : MonoBehaviour
{
    [HideInInspector]
    [Tooltip("Dictionary of string and stack. The string= category name, stack contains some of the question ids of the category- depands on the game length")]
    private Dictionary<string, Stack<int>> _questions = new Dictionary<string, Stack<int>>();

    [HideInInspector]
    [Tooltip("The list of the category name. Those are the key values to find the stacks in the Dictionary")]
    private List<string> _categoryNames = new List<string>();

    //Connect other classes
    [SerializeField] private ReciveQuestion reciveQuestion;
    [SerializeField] private OrganizeQuestions organizeQuestions;
    [SerializeField] private Timer timer;
    [SerializeField] private PlayerData playerData;
    
    //UI
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject PlayerUICanvas;
    [SerializeField] private GameObject CategoryCanvas;
    [SerializeField] private GameObject QuestionCanvas;
    [SerializeField] private GameObject GameFinishedCanvas;
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private TMP_Text _playerScore;
    [SerializeField] private TMP_Text _finaleGameScore;
    [SerializeField] private TMP_Text _categoryCountdownText;
    [SerializeField] private TMP_Text CategoryText;

    private void Awake()
    {
        OrganizeQuestions.OnQuestionsReceived+= SetListOfQuestions;
        OrganizeQuestions.OnCategorysNamesReceived+= SetListOfCategoryNames;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void TurnOnEndGameCanvas()
    {
        playerData.SendData();
        _finaleGameScore.text = PlayerData.PlayerScore.ToString();
        PlayerUICanvas.gameObject.SetActive(false);
        QuestionCanvas.gameObject.SetActive(false);
        GameFinishedCanvas.gameObject.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        GameFinishedCanvas.gameObject.SetActive(false);
        MainMenuCanvas.gameObject.SetActive(true);
        _questions.Clear();
        _categoryNames.Clear();
    }


    public void GetGameLength(int lenghtID)
    {
        PlayerData.DifficultyLevel = lenghtID;
    }

    public void RunTrivia()
    {
        if (_playerName.text == null)
            return;
        PlayerData.PlayerName = _playerName.text;
        if (PlayerData.DifficultyLevel!=0 && PlayerData.PlayerName!=null && PlayerData.PlayerName != "​")
        StartCoroutine(PrepareGame());
    }

    public IEnumerator PrepareGame()
    {
        organizeQuestions.OrganizeList();
        bool gameReady = false;
        while (gameReady==false)
        {
            yield return null;
            if (_questions.Count == 0 && _categoryNames.Count == 0)
                continue;

            MainMenuCanvas.SetActive(false);
            PlayerUICanvas.SetActive(true);
            gameReady = true;
        }
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        for (int i = 0; i < _categoryNames.Count; i++)
        {
            StartCoroutine(DisplayCategory(_categoryNames[i]));
            while (CategoryCanvas.gameObject.activeInHierarchy)
            {
                yield return null;
            }
            Stack<int> questionStack;
            if (_questions.TryGetValue($"{_categoryNames[i]}", out questionStack))
            {
                while (questionStack.Count != 0)
                {
                    if (ReciveQuestion.RaciveNextQuestion)
                    {
                        int questionID = questionStack.Pop();
                        ReciveQuestion.RaciveNextQuestion = false;
                        reciveQuestion.GetQuestion(questionID);
                        while (ReciveQuestion.RaciveNextQuestion == false)
                        {
                            yield return null;
                        }
                        _playerScore.text = PlayerData.PlayerScore.ToString();
                    }
                    yield return null;
                }
            }
        }
        TurnOnEndGameCanvas();
    }

    private IEnumerator DisplayCategory(string categoryName)
    {
        QuestionCanvas.SetActive(false);
        CategoryCanvas.SetActive(true);
        CategoryText.text = categoryName;
        timer.StrtTimer(3);
        while (timer.timerActive)
        {
            yield return null;
        }
        QuestionCanvas.SetActive(true);
        CategoryCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        OrganizeQuestions.OnQuestionsReceived -= SetListOfQuestions;
        OrganizeQuestions.OnCategorysNamesReceived -= SetListOfCategoryNames;
    }

    private void SetListOfQuestions(Dictionary<string, Stack<int>> questions)
    {
        _questions = questions;
    }

    private void SetListOfCategoryNames(List<string> CategorysNames)
    {
        _categoryNames = CategorysNames;
    }


}
