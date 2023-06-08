using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;
using System.IO;

public class RecordsManager : MonoBehaviour
{
 
    private string name;
    private float finalTime;
    private int idLevelOne;
    private int idLevelTwo;

    private List<ClassRecords> classRecords = new List<ClassRecords>();

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

    private void DeleteLastScore()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM level_one_records ORDER BY id LIMIT 1";
                dbCommand.CommandText = sqlQuery;

                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idLevelOne = reader.GetInt32(0);
                    }

                    reader.Close();
                }

            }
        }

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM level_two_records ORDER BY id LIMIT 1";
                dbCommand.CommandText = sqlQuery;

                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idLevelTwo = reader.GetInt32(0);
                    }

                    reader.Close();
                }
            }
        }

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("DELETE FROM level_one_records WHERE id=\"{0}\"",
                                    idLevelOne);

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();

                sqlQuery = String.Format("DELETE FROM level_one_records WHERE id=\"{0}\"",
                                    idLevelTwo);

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();

                dbConnection.Close();
            }
        }
    }

    private void DeleteScore(int finalId)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("DELETE FROM final_records WHERE id=\"{0}\"",
                                    finalId);

                dbCommand.CommandText = sqlQuery;
                dbCommand.ExecuteScalar();

                dbConnection.Close();
            }
        }
    }
}
