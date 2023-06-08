using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HightScore : MonoBehaviour
{
    public GameObject time;
    public GameObject name;
    public GameObject rank;

    public void SetScore(string name, float time, string rank)
    {
        this.rank.GetComponent<Text>().text = rank;
        this.name.GetComponent<Text>().text = name;
        this.time.GetComponent<Text>().text = time.ToString();

    }
}
