using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class QuizModel 
{
    //https://www.google.com/search?rlz=1C1CHBF_esMX879MX879&biw=1366&bih=657&tbm=isch&sxsrf=ACYBGNTLT_i0zPFWhnyULZans8rr7a_UuA%3A1579305342867&sa=1&ei=fkkiXv3CNM7YtAWgmrj4Cg&q=minimalist+monsters&oq=minimalist+monsters&gs_l=img.3..0i19l6j0i8i30i19.18583.19707..19831...0.0..0.109.907.8j3......0....1..gws-wiz-img.......35i39j0i7i30j0i8i30j0i5i30j0j0i30._wCjQDTLV-A&ved=0ahUKEwj9-c_v6ovnAhVOLK0KHSANDq8Q4dUDCAc&uact=5#imgdii=GOhhCyvZnp_NpM:&imgrc=1je0Q23ozvnAZM:
    private static List<QuestionData> questionsDataBase = new List<QuestionData>() {
            new QuestionData(){
                questionId="1000",
                questionDifficulty=1,
                questionCategoryId=QuestionCategoryId.Anime,
                questionText="how many is 2 +2 ?",
                answers=new List<AnswerData>(){
                    new AnswerData(){
                        answerText="answer 1",
                        correctAnswer=true
                    },
                     new AnswerData(){
                         answerText="answer 1",
                        correctAnswer=false
                    }
                     ,
                     new AnswerData(){
                       answerText="answer 1",
                        correctAnswer=false
                    }
                }
            },
            new QuestionData(){
                questionId="1001",
                questionDifficulty=2,
                questionCategoryId=QuestionCategoryId.Anime,
                questionText="who are you",
                answers=new List<AnswerData>(){
                    new AnswerData(){
                        answerText="answer 1",
                        correctAnswer=true
                    },
                     new AnswerData(){
                        answerText="answer 1",
                        correctAnswer=false
                    }
                     ,
                     new AnswerData(){
                       answerText="answer 1",
                        correctAnswer=false
                    }
                }
            }

        };

    public QuestionData GetAQuestion() {
        QuestionData questionData = new QuestionData();
        Debug.Log("leveldifficuytl " + GameManager.instance.currentLevelData.levelDifficulty);
        List<QuestionData> questions = questionsDataBase.FindAll(x => x.questionDifficulty <= GameManager.instance.currentLevelData.levelDifficulty);

        questionData = questions[UnityEngine.Random.Range(0, questions.Count)];
        Debug.Log("questionData " + JsonUtility.ToJson(questionData));


        return questionData;


    }

   
   
}
