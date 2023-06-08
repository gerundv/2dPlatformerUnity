using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class SaveResult : MonoBehaviour
{
    private float timeLevelOne;
    private float timeLevelTwo;
    private float timeLevelThree;

    [SerializeField] private Text inputText;

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

    public void Save()
    {

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM level_one_records ORDER BY id DESC LIMIT 1";
                dbCommand.CommandText = sqlQuery;

                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        timeLevelOne = Convert.ToSingle(reader.GetValue(2));
                    }
                    dbConnection.Close();
                    reader.Close();
                }

            }
        }

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM level_two_records ORDER BY id DESC LIMIT 1";
                dbCommand.CommandText = sqlQuery;

                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        timeLevelTwo = Convert.ToSingle(reader.GetValue(2));
                    }
                    dbConnection.Close();

                    reader.Close();
                }
            }
        }

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM level_three_records ORDER BY id DESC LIMIT 1";
                dbCommand.CommandText = sqlQuery;

                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        timeLevelThree = Convert.ToSingle(reader.GetValue(2));
                    }
                    dbConnection.Close();

                    reader.Close();
                }
            }
        }

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string name = inputText.text;
                string sqlQuery = String.Format("INSERT INTO final_records(name, time) VALUES(\"{0}\",\"{1}\")",
                                    name,
                                    timeLevelOne + timeLevelTwo +  timeLevelThree);
                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();
                dbConnection.Close();
            }
        }


        SceneManager.LoadScene("Start screen");
    }
}
