using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;



public class ExportExcelData : MonoBehaviour
{
    private int time = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        WriteInfo("hello.txt", "wow!");

    }

    // Update is called once per frame
    void Update()
    {
    }

    void WriteInfo(string filePath, string info)
    {
        var records = new List<Foo2>
        {
            new Foo2 { time = time, numCells = 1 },
            new Foo2 { time = time, numCells = 2 },
        };
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }
}


public class Foo2
{
    public int time { get; set; }
    public int numCells { get; set; }
}