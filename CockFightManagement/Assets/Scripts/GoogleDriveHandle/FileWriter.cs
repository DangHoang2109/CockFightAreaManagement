using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileWriter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            WriteFile();
        }
    }
    private void WriteFile()
    {
        string filePath = AppDefine.FilePath + "/myCSVFile.csv";

        // write the data to the file using the using statement
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // write the header row to the file
            writer.WriteLine("Name, Age, Score");

            // write some sample data to the file
            writer.WriteLine("John Smith, 30, 75");
            writer.WriteLine("Jane Doe, 25, 90");
        }
    }
}
public static class AppDefine
{
    public const string FilePath =
#if UNITY_EDITOR
        "Assets/FolderResult";
#else
    Application.persistentDataPath + "/";
#endif
}
