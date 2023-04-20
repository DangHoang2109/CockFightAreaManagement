using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Drive.v3;
//using Google.Apis.Services;
//using Google.Apis.Upload;
public class DriveFileUploader : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UploadFile();
        }

    }
    private void UploadFile()
    {
        string filePath = Application.persistentDataPath + "/myCSVFile.csv";

        // set the ID of the Google Drive folder to upload the file to
        string folderId = "INSERT_FOLDER_ID_HERE";

        //// authenticate with the Google Drive API
        //UserCredential credential;
        //using (var stream = new FileStream("INSERT_PATH_TO_CLIENT_SECRET_JSON_FILE_HERE", FileMode.Open, FileAccess.Read))
        //{
        //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //        GoogleClientSecrets.Load(stream).Secrets,
        //        new[] { DriveService.Scope.Drive },
        //        "user",
        //        System.Threading.CancellationToken.None).Result;
        //}

        //// create the Drive API service
        //var service = new DriveService(new BaseClientService.Initializer()
        //{
        //    HttpClientInitializer = credential,
        //    ApplicationName = "MyApp"
        //});

        //// create the file metadata
        //var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        //{
        //    Name = "myCSVFile.csv",
        //    Parents = new[] { folderId },
        //    MimeType = "text/csv"
        //};

        //// create the file content
        //var streamContent = new System.IO.StreamContent(new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read));
        //var uploadRequest = service.Files.Create(fileMetadata, streamContent, "text/csv");
        //uploadRequest.Fields = "id";
        //uploadRequest.Upload();

        //// get the ID of the uploaded file
        //var file = uploadRequest.ResponseBody;
        //Debug.Log("File ID: " + file.Id);
    }

}
