using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.IO;
using UnityEngine.UI;

public class ResultsTableController : MonoBehaviour
{
    public GameObject scorePrefab;
    public Transform scoreParent;

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

    void Start()
    {
        DBPath = GetDatabasePath();
        connectionString = "URI=file:" + DBPath;
        ShowScores();
    }
    private static void UnpackDatabase(string toPath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, fileName);

        WWW reader = new WWW(fromPath);

        while (!reader.isDone) { }

        File.WriteAllBytes(toPath, reader.bytes);

    }

    public void GetScores()
    {
        classRecords.Clear();

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM final_records ORDER BY time DESC";
                dbCommand.CommandText = sqlQuery;

                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        classRecords.Add(new ClassRecords(reader.GetInt32(2),
                                                            reader.GetString(0),
                                                                Convert.ToSingle(reader.GetValue(1))));
                    }

                    dbConnection.Close();
                    reader.Close();
                }
            }
        }

    }

    private void ShowScores()
    {
        GetScores();
        for (int i = 0; i < classRecords.Count; i++)
        {
            GameObject tmpObject = Instantiate(scorePrefab);

            ClassRecords tmpScore = classRecords[i];

            tmpObject.GetComponent<HightScore>().SetScore(tmpScore.name, tmpScore.time, "#" + (i + 1).ToString());

            tmpObject.transform.SetParent(scoreParent);
        }
    }
}
