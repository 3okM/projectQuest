using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using System;

public class TriggerEnter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        dbPath = "URI=file:" + Application.persistentDataPath + "/exampleDatabase.db";
        Debug.Log("database file at " + dbPath);
        CreateSchema();
        //InsertScore("GG Meade", 3701);
        //InsertScore("US Grant", 4242);
        //InsertScore("GB McClellan", 107);
        //GetHighScores(10);
    }
    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerEnter2D(Collider2D col){
        OnTriggerEnter(null);
    }
    void OnTriggerEnter(Collider col) {
        Debug.Log("OnTriggerEnter (start)");
         //if(col.tag == "Player")
        System.Random rnd = new System.Random();
        //taskNum = rnd.Next(1, 5); 
        GetAnswer(taskKit, taskNum);//TODO Read task from DB
            aTexture2DQuestion = new Texture2D(830, 200);
            aTexture2DQuestion.LoadImage(filedataQuestion);
            /* aTexture2DAnswer = new Texture2D(16, 16);
            aTexture2DAnswer.LoadImage(filedataAnswer); */
            aTexture2DHint = new Texture2D(900, 800);
            aTexture2DHint.LoadImage(filedataHint);
            PlayerController.MOVEMENT_BASE_SPEED = 0;

        /* string filepathQuestion = @"C:\Users\KostyaO\MyFirstGame\q_1.png";
        if (File.Exists(filepathQuestion)) {
            filedataQuestion = File.ReadAllBytes(filepathQuestion);
        }
        string filepathAnswer = @"C:\Users\KostyaO\MyFirstGame\a_1.png";
        if (File.Exists(filepathAnswer)) {
            filedataAnswer = File.ReadAllBytes(filepathAnswer);
        } */
        Open();
    }
         // 200x300 px window will apear in the center of the screen.
     // Only show it if needed.
     private Rect windowRect = new Rect ((Screen.width - 1000)/2-500, (Screen.height - 800)/2, 1000, 800);
     private bool show = false;
     private float PRIMARY_MOVEMENT_BASE_SPEED = PlayerController.MOVEMENT_BASE_SPEED;
     public bool showHint = true;
     public bool showHintGlobalChange = true;
     public int questionHeight = 800;
     public int hintHeight = 100;
     public int answersHeight = 100;
     public Texture2D aTexture2DQuestion;
     public Texture2D aTexture2DAnswer;
     public Texture2D aTexture2DHint;
     byte[] filedataQuestion = new byte[5500000];
     byte[] filedataAnswer = new byte[5500000];
     byte[] filedataHint = new byte[5500000];
     public string userAnswer = "Введите ответ";
     public string rightAnswer;
     private string dbPath;
     public int taskNum;
     public string taskKit; 
     //public GUISkin customSkin;
     public GUIStyle answerGuiStyle;
     public GUIStyle hintEnabledGuiStyle;
     public GUIStyle hintDisabledGuiStyle;
     public GUIStyle exitGuiStyle;
     public GUIStyle textGuiStile;
     public GUIStyle windowGuiStyleBig;
     public GUIStyle windowGuiStyleSmall;
    void OnGUI () {
        //GUI.skin = customSkin;
        if(show) {
        if(!showHint) {
            //windowRect = new Rect ((Screen.width - 500)/2-250, (Screen.height - 800)/2, 1000, 800);
            windowRect = new Rect ((Screen.width - 1000)/2-500, (Screen.height - 800)/2, 1000, 800);
                    windowRect = GUI.Window (0, windowRect, DialogWindow, "", windowGuiStyleSmall);
        } else {
            windowRect = new Rect ((Screen.width - 1000)/2-500, (Screen.height - 800)/2, 1000+1000, 800);
        windowRect = GUI.Window (0, windowRect, DialogWindow, "", windowGuiStyleBig);  
        }
//        windowRect = GUI.Window (0, windowRect, DialogWindow, "", windowGuiStyle);//"Solve it!!!"
        }
    }
    // This is the actual window.
    void DialogWindow (int windowID)
    {
     
        float y = 20;
        //GUI.Label(new Rect(5,y, windowRect.width, 20), "Again?");

        if (!aTexture2DAnswer && !aTexture2DQuestion && !aTexture2DHint)  {
            Debug.LogError("Textures are not initialased at all");
            return;
        }
       GUI.DrawTexture(new Rect(5,y, 1000-10, questionHeight), aTexture2DQuestion, ScaleMode.ScaleToFit);
       /* GUI.DrawTexture(new Rect(5,y+questionHeight, 1000-10, answersHeight), aTexture2DAnswer, ScaleMode.ScaleToFit); */
  /* 
        
        
        Sprite sprite = Sprite.Create(aTexture2DAnswer, new Rect(5, y, 160, 160), new Vector2(0.5f, 0.5f));
        // create gameobject
        GameObject newSprite = new GameObject();
        newSprite.AddComponent<SpriteRenderer>();
        SpriteRenderer SR = newSprite.GetComponent<SpriteRenderer>();
        SR.sprite = sprite;
                 
        
        
        */

        if (showHint) {
            GUI.DrawTexture(new Rect(5+1000,y, 1000-10, hintHeight), aTexture2DHint, ScaleMode.ScaleToFit);    
        }
         userAnswer = GUI.TextField(new Rect(5, y+answersHeight+questionHeight+10, 900-10, 20), userAnswer, 25, textGuiStile);
/*         if(GUI.Button(new Rect(5,y+25+answersHeight+questionHeight, 1000/2 - 10, 20), "Restart"))
        {
            Application.LoadLevel (0);
            show = false;
        }
 */        if(GUI.Button(new Rect(900,y+answersHeight+questionHeight+10, 100 - 10, 20), "", answerGuiStyle)) //answer
        {
           if(userAnswer != rightAnswer) {
               Application.Quit();
           } 
           else {
           show = false;
           ScoreScript.scoreValue += 1;
           PlayerController.MOVEMENT_BASE_SPEED = PRIMARY_MOVEMENT_BASE_SPEED;
           userAnswer = "Введите ответ";
           }
        }
        if(showHintGlobalChange){
        if(GUI.Button(new Rect(5+450,y+answersHeight+questionHeight+55, 450 - 10, 20), "", hintEnabledGuiStyle)) //hint
        {
             showHint = !showHint;
        }
        }
        else{
        if(GUI.Button(new Rect(5+450,y+answersHeight+questionHeight+55, 450 - 10, 20), "", hintDisabledGuiStyle)){} //hint    
        }

        if(GUI.Button(new Rect(5,y+answersHeight+questionHeight+55, 450 - 10, 20), "Выход")) //exit
        {
           Application.Quit();
           show = false;
           PlayerController.MOVEMENT_BASE_SPEED = PRIMARY_MOVEMENT_BASE_SPEED;
        }

    }

    // To open the dialogue from outside of the script.
    public void Open()
    {
        show = true;
    }
 

    public void CreateSchema() {
        using (var conn = new SqliteConnection(dbPath)) {
            conn.Open();
            Debug.Log("CreateSchema(start)");
            using (var cmd = conn.CreateCommand()) {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'quest' ( " +
                                    "  'id' INTEGER PRIMARY KEY, " +
                                    "  'question' BLOB NOT NULL, " +
                                    "  'answers' BLOB NOT NULL," +
                                    "  'hint' BLOB NOT NULL," +
                                    "  'solution' BLOB NOT NULL," +
                                    "  'answer' TEXT NOT NULL" +									  
                                    ");";
/*CREATE TABLE quest (
    kit      VARCHAR,
    id       INTEGER,
    question BLOB    NOT NULL,
    solution BLOB    NOT NULL,
    hint     BLOB    NOT NULL,
    answer   TEXT    NOT NULL,
    PRIMARY KEY (kit, id)
); */
                var result = cmd.ExecuteNonQuery();
                Debug.Log("create schema: " + result);
            }
        }
    }

    public void GetAnswer(string taskKit, int taskNum) {
        using (var conn = new SqliteConnection(dbPath)) {
            conn.Open();
            using (var cmd = conn.CreateCommand()) {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM quest WHERE kit = @taskKit and id = @taskNum;";
                cmd.Parameters.Add(new SqliteParameter {
                    ParameterName = "taskKit",
                    Value = taskKit
                });

                cmd.Parameters.Add(new SqliteParameter {
                    ParameterName = "taskNum",
                    Value = taskNum
                });

                Debug.Log("answer (begin)");
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var kit = reader.GetString(0);
                    var id = reader.GetInt32(1);
                    Stream question = reader.GetStream(2);
                    Stream solution = reader.GetStream(3);
                    Stream hint = reader.GetStream(4);
                    var answer = reader.GetString(5);
                    Debug.Log(id);
                    Debug.Log(answer);
                    rightAnswer = answer;
                    hint.Read(filedataHint, 0, (int )hint.Length);
                    question.Read(filedataQuestion, 0, (int )question.Length);
                    //answers.Read(filedataAnswer, 0, (int )answers.Length);
                }
                Debug.Log("answer (end)");
            }
        }
    }

}
