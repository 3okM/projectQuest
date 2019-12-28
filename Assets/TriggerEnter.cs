﻿using System.Collections;
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
        GetAnswer(taskNum);//TODO Read task from DB
            aTexture2DQuestion = new Texture2D(830, 200);
            aTexture2DQuestion.LoadImage(filedataQuestion);
            aTexture2DAnswer = new Texture2D(16, 16);
            aTexture2DAnswer.LoadImage(filedataAnswer);
            aTexture2DHint = new Texture2D(900, 800);
            aTexture2DHint.LoadImage(filedataHint);

        /* string filepathQuestion = @"C:\Users\KostyaO\MyFirstGame\q_1.png";
        if (File.Exists(filepathQuestion)) {
            filedataQuestion = File.ReadAllBytes(filepathQuestion);
        }
        string filepathAnswer = @"C:\Users\KostyaO\MyFirstGame\a_1.png";
        if (File.Exists(filepathAnswer)) {
            filedataAnswer = File.ReadAllBytes(filepathAnswer);
        } */
        Open();
        Hint();
    }
         // 200x300 px window will apear in the center of the screen.
     // Only show it if needed.
     private Rect windowRect = new Rect ((Screen.width - 1000)/2-500, (Screen.height - 800)/2, 1000, 800);
     private bool show = false;
     private bool showHint = false;
     public int questionHeight = 800;
     public int hintHeight = 100;
     public int answersHeight = 100;
     public Texture2D aTexture2DQuestion;
     public Texture2D aTexture2DAnswer;
     public Texture2D aTexture2DHint;
     byte[] filedataQuestion = new byte[5500000];
     byte[] filedataAnswer = new byte[5500000];
     byte[] filedataHint = new byte[5500000];
     public string userAnswer = "Enter Answer Here";
     public string rightAnswer;
     private string dbPath;
     public int taskNum; // TODO get from object settings 
     public GUISkin customSkin;
    void OnGUI () {
        GUI.skin = customSkin;
        if(show) {
        if(!showHint) {
            windowRect = new Rect ((Screen.width - 1000)/2-500, (Screen.height - 800)/2, 1000, 800);
        } else {
            windowRect = new Rect ((Screen.width - 1000)/2-500, (Screen.height - 800)/2, 1000+1000, 800);
        }
        windowRect = GUI.Window (0, windowRect, DialogWindow, "Solve it!!!");
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
         userAnswer = GUI.TextField(new Rect(5, y+answersHeight+questionHeight+10, 900-10, 20), userAnswer, 25);
/*         if(GUI.Button(new Rect(5,y+25+answersHeight+questionHeight, 1000/2 - 10, 20), "Restart"))
        {
            Application.LoadLevel (0);
            show = false;
        }
 */        if(GUI.Button(new Rect(900,y+answersHeight+questionHeight+10, 100 - 10, 20), "Answer"))
        {
           if(userAnswer != rightAnswer) {
               Application.Quit();
           } 
           else {
           show = false;
           userAnswer = "Enter Answer Here";
           }
        }
        if(GUI.Button(new Rect(5,y+answersHeight+questionHeight+55, 1000 - 10, 20), "Hint"))
        {
            showHint = !showHint;
        }

        if(GUI.Button(new Rect(5,y+answersHeight+questionHeight+95, 1000 - 10, 20), "Exit"))
        {
           Application.Quit();
           show = false;
        }

    }

    // To open the dialogue from outside of the script.
    public void Open()
    {
        show = true;
    }
    public void Hint()
    {
        showHint = false;
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

                var result = cmd.ExecuteNonQuery();
                Debug.Log("create schema: " + result);
            }
        }
    }

    public void GetAnswer(int taskNum) {
        using (var conn = new SqliteConnection(dbPath)) {
            conn.Open();
            using (var cmd = conn.CreateCommand()) {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM quest WHERE id = @taskNum;";

                cmd.Parameters.Add(new SqliteParameter {
                    ParameterName = "taskNum",
                    Value = taskNum
                });

                Debug.Log("answer (begin)");
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var id = reader.GetInt32(0);
                    Stream question = reader.GetStream(1);
                    Stream answers = reader.GetStream(2);
                    Stream solution = reader.GetStream(3);
                    Stream hint = reader.GetStream(4);
                    var answer = reader.GetString(5);
                    Debug.Log(id);
                    Debug.Log(answer);
                    rightAnswer = answer;
                    hint.Read(filedataHint, 0, (int )hint.Length);
                    question.Read(filedataQuestion, 0, (int )question.Length);
                    answers.Read(filedataAnswer, 0, (int )answers.Length);
                }
                Debug.Log("answer (end)");
            }
        }
    }

}