using UnityEngine;
using UnityEngine.SceneManagement;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.IO;

public class Finish : MonoBehaviour
{
    private AudioSource finishSound;

    private bool completed = false;
    private float timeLevel = 0;
    private int strawberries = 0;
    private static string connectionString;
    private static string DBPath;
    private const string fileName = "recordsDB.db";

    private static string GetDatabasePath()
    {
        string filePath = Path.Combine(Application.dataPath, fileName);
        if (!File.Exists(filePath))
            UnpackDatabase(filePath);
        return filePath;
    }

    private void Start()
    {
        finishSound = GetComponent<AudioSource>();
        DBPath = GetDatabasePath();
        connectionString = "URI=file:" + DBPath;
    }
    private static void UnpackDatabase(string toPath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

        WWW reader = new WWW(fromPath);

        while (!reader.isDone) { }

        File.WriteAllBytes(toPath, reader.bytes);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !completed)
        {
            finishSound.Play();
            completed = true;
            Invoke("CompleteLevel", 2f);
        }
    }

    private void CompleteLevel()
    {
        timeLevel = Time.timeSinceLevelLoad;
        strawberries = ItemCollector.countStrawberries;
        InsertScore(strawberries, timeLevel);
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            SceneManager.LoadScene("Level 2");
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            SceneManager.LoadScene("Level 3");
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            SceneManager.LoadScene("End Screne");
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void InsertScore(int points, float time)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string sqlQuery = "";
                if (SceneManager.GetActiveScene().name == "Level 1")
                {
                    sqlQuery = String.Format("INSERT INTO level_one_records(points, time) VALUES(\"{0}\",\"{1}\")",
                                    points,
                                    time);
                }
                else if (SceneManager.GetActiveScene().name == "Level 2")
                {
                    sqlQuery = String.Format("INSERT INTO level_two_records(points, time) VALUES(\"{0}\",\"{1}\")",
                                    points,
                                    time);
                }
                else if (SceneManager.GetActiveScene().name == "Level 3")
                {
                    sqlQuery = String.Format("INSERT INTO level_three_records(points, time) VALUES(\"{0}\",\"{1}\")",
                                    points,
                                    time);
                }

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }
}
