using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassRecords
{
    public int id { get; set; }
    public string name { get; set; }
    public float time { get; set; }
    
    public ClassRecords(int id, string name, float time)
    {
        this.id = id;
        this.name = name;
        this.time = time;
    }
}
