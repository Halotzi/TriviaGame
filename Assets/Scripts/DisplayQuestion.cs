using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayQuestion : MonoBehaviour
{
    [SerializeField] private TMP_Text _question;
    [SerializeField] private TMP_Text _ans1;
    [SerializeField] private TMP_Text _ans2;
    [SerializeField] private TMP_Text _ans3;
    [SerializeField] private TMP_Text _ans4;
    [SerializeField] private Timer timer;
    [SerializeField]
    Button[] answers = new Button[4];

    private int currectAnswer;
    private int playerAnswer=0;
    
    
    public void ShowQuestion(ShowQuestion showQuestion)
    {
        _question.text = showQuestion.question_text;
        _ans1.text = showQuestion.ans1_text;
        _ans2.text = showQuestion.ans2_text;
        _ans3.text = showQuestion.ans3_text;
        _ans4.text = showQuestion.ans4_text;

        currectAnswer = showQuestion.correct_ans_id;
        StartCoroutine(Timer(10));
    }

    private IEnumerator Timer(int time)
    {
        //First timer to mark your question
        timer.StrtTimer(time);
        while (timer.timerActive)
        {
            yield return null;
        }

        //Check if you are right
        if (playerAnswer == currectAnswer)
        {
            PlayerData.PlayerScore++;
        }

        //If you are wrong
        else
        {
            if (!(playerAnswer - 1 < 0))
                answers[playerAnswer - 1].image.color = Color.red;
        }
            
        //Mark the right answer
        answers[currectAnswer-1].image.color = Color.green;

        //All the buttons are interactables
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].interactable = false;
        }

        //Timer to see the right answer
        timer.StrtTimer(5);
        while (timer.timerActive)
        {
            yield return null;
        }

        //Reset buttons
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].image.color = Color.white;
            answers[i].interactable = true;
        }
        ReciveQuestion.RaciveNextQuestion = true;
    }

    public void GetPlayerAnswer(int buttonNum)
    {
        if (!(playerAnswer-1<0))
        answers[playerAnswer - 1].image.color = Color.white;
        playerAnswer = buttonNum;
        answers[playerAnswer - 1].image.color = Color.cyan;
    }
}
