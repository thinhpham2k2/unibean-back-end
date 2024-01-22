using Microsoft.AspNetCore.Http;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services.Interfaces;

public interface IFireBaseService
{
    Task<FireBaseFile> UploadFileAsync(IFormFile fileUpload, string folder);

    string PushNotificationToTopic(string topic);

    Task<bool> RemoveFileAsync(string fileName, string folder);

    Task<string> GetLinkAsync(string fileName, string folder);
}
