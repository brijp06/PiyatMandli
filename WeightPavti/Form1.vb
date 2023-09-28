Imports Google.Apis.Auth
Imports Google.Apis.Download
Imports Google.Apis.Drive.v3
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Services
Imports System.Threading
Imports Google.Apis.Drive.v3.Data
Imports Google.Apis.Upload
Public Class Form1
    'Private Service As DriveService = New DriveService
    Dim clid As String
    Dim authid As String
    Dim path As String
    Private moService As DriveService = New DriveService
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' readini()
        CreateService()
    End Sub
    'Private Service As DriveService = New DriveService

    Private Sub CreateService()
        ' If Not BeGreen Then
        Dim ClientId = "744206872529-useqh5d9sf0v412864jne6vj0fdi6e4b.apps.googleusercontent.com" 'clid
        Dim ClientSecret = "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" 'authid
        Dim MyUserCredential As UserCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(New ClientSecrets() With {.ClientId = ClientId, .ClientSecret = ClientSecret}, {DriveService.Scope.Drive}, "user", CancellationToken.None).Result
        moService = New DriveService(New BaseClientService.Initializer() With {.HttpClientInitializer = MyUserCredential, .ApplicationName = "Backup"})
        path = Application.StartupPath & "\Backup\Mandli"
        'End If
    End Sub
    Public Sub readini()
        FileOpen(1, Application.StartupPath & "\autobackup.ini", OpenMode.Input)
        Input(1, clid)
        Input(1, authid)
        Input(1, path)
    End Sub
    Private Sub UploadFile(FilePath As String)
        'Me.Cursor = Cursors.WaitCursor
        If moService.ApplicationName <> "Backup" Then CreateService()

        Dim oGDriveFile As New File()

        oGDriveFile.Name = “Backup” ' Set your File Name here.
        oGDriveFile.Description = “Datebase Back Up” ' Set a meaningful description, I had set the same name as my project name
        oGDriveFile.MimeType = “application/zip” ' You must set your MIME type carefully. Refer to the MIME table above

        Dim bArrByteArray As Byte() = System.IO.File.ReadAllBytes(FilePath) ' Your File Path from where you would want to upload from.
        Dim oStream As New System.IO.MemoryStream(bArrByteArray)

        Dim oUploadRequest As FilesResource.CreateMediaUpload

        oUploadRequest = moService.Files.Create(oGDriveFile, oStream, oGDriveFile.MimeType)

        oUploadRequest.Fields = “id”
        oUploadRequest.Alt = FilesResource.CreateMediaUpload.AltEnum.Json
        ' Me.PictureBox1.Visible = True
        lbplease.Visible = True

        oUploadRequest.Upload() ' <- The file upload happens here.

        Dim oFile As File = oUploadRequest.ResponseBody
        '  Me.PictureBox1.Visible = False
        lbplease.Visible = False

        MsgBox("Upload Finished")

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)


    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        path = path & Now.Day & Now.Month & Now.Year & ".Zip"
        UploadFile(path)
        'Me.PictureBox1.Visible = True
        lbplease.Visible = True

    End Sub


End Class
