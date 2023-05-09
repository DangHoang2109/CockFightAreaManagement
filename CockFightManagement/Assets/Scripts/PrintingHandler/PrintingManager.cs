using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sfs2X.Entities.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Diagnostics;

public class PrintingManager : MonoSingleton<PrintingManager>
{
    string path = null;
    
    void Start()
    {
        path = Application.dataPath + "/Ticket.pdf";  
       
    }

    public void GenerateAndPrintDocument(List<string> fileContent)
    {
        if (File.Exists(path))
            File.Delete(path);

        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            var document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            var writer = PdfWriter.GetInstance(document, fileStream);

            document.Open();

            document.NewPage();

            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            foreach (string line in fileContent)
            {
                document.Add( WriteLine(line, Element.ALIGN_LEFT));
            }

            document.Close();
            writer.Close();
        }

        PrintFiles();

        static Paragraph WriteLine(string lineContent, int align = Element.ALIGN_CENTER)
        {
            Paragraph p = new Paragraph(lineContent);
            p.Alignment = align;
            return p;
        }
    }

    void PrintFiles()
    {
        UnityEngine.Debug.Log(path);
        if (path == null)
            return;

        if (File.Exists(path))
        {
            UnityEngine.Debug.Log("file found");
        }
        else
        {
            UnityEngine.Debug.Log("file not found");
            return;
        }

        ProcessStartInfo info = new ProcessStartInfo(path);
        info.Verb = "print";
        info.CreateNoWindow = true;
        info.WindowStyle = ProcessWindowStyle.Hidden;

        Process p = new Process();
        p.StartInfo = info;
        p.Start();
        p.WaitForExit();

        Process.Start(path);
    }
}
