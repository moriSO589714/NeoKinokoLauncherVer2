

using Google.Apis.Drive.v3;
using System.IO;
using Unity.IO.LowLevel.Unsafe;

public class OnNetDriveGetFilefromDV : OnNetDriveGetFile
{
    private DriveService _driveService;

    public OnNetDriveGetFilefromDV(DriveService driveService)
    {
        _driveService = driveService;
    }

    /// <summary>
    /// APIを介してファイルを保存してくる処理
    /// </summary>
    public void GetFile(string driveId, string fileName, string dledPath)
    {
        //リクエストの作成
        var request = _driveService.Files.Get(driveId);
        string filePath = Path.Combine(dledPath, fileName);
        using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            request.Download(stream);
        }
    }
}
