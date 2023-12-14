using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using System.Net.Sockets;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class FireBaseService : IFireBaseService
{
    public async Task<string> GetLinkAsync(string fileName, string folder)
    {
        var config = new FirebaseAuthConfig
        {
            ApiKey = UploadConfig.API_KEY,
            AuthDomain = UploadConfig.AuthDomain,
            Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                },
        };
        var auth = new FirebaseAuthClient(config);
        var loginInfo = await auth.SignInWithEmailAndPasswordAsync(UploadConfig.AuthEmail, UploadConfig.AuthPassword);
        var storage = new FirebaseStorage(UploadConfig.Bucket, new FirebaseStorageOptions
        {
            AuthTokenAsyncFactory = async () => await Task.FromResult(await loginInfo.User.GetIdTokenAsync()),
            ThrowOnCancel = true
        });

        var starRef = storage.Child(folder).Child(fileName);
        var dowloadURL = await starRef.GetDownloadUrlAsync();
        return dowloadURL;
    }

    public async Task<bool> RemoveFileAsync(string fileName, string folder)
    {
        var config = new FirebaseAuthConfig
        {
            ApiKey = UploadConfig.API_KEY,
            AuthDomain = UploadConfig.AuthDomain,
            Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                },
        };
        var auth = new FirebaseAuthClient(config);
        var loginInfo = await auth.SignInWithEmailAndPasswordAsync(UploadConfig.AuthEmail, UploadConfig.AuthPassword);
        var storage = new FirebaseStorage(UploadConfig.Bucket, new FirebaseStorageOptions
        {
            AuthTokenAsyncFactory = async () => await Task.FromResult(await loginInfo.User.GetIdTokenAsync()),
            ThrowOnCancel = true
        });
        await storage.Child(folder).Child(fileName).DeleteAsync();
        return true;
    }

    public async Task<FireBaseFile> UploadFileAsync(IFormFile fileUpload, string folder)
    {
        FileInfo fileInfo = new FileInfo(fileUpload.FileName);

        if (fileUpload.Length > 0)
        {
            var fs = fileUpload.OpenReadStream();
            var config = new FirebaseAuthConfig
            {
                ApiKey = UploadConfig.API_KEY,
                AuthDomain = UploadConfig.AuthDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                },
            };
            var auth = new FirebaseAuthClient(config);
            var loginInfo = await auth.SignInWithEmailAndPasswordAsync(UploadConfig.AuthEmail, UploadConfig.AuthPassword);

            var cancellation = new FirebaseStorage(
                UploadConfig.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = async () => await Task.FromResult(await loginInfo.User.GetIdTokenAsync()),
                    ThrowOnCancel = true
                }
                ).Child(folder).Child(fileUpload.FileName)
                .PutAsync(fs, CancellationToken.None);
            try
            {
                var result = await cancellation;

                return new FireBaseFile
                {
                    FileName = fileUpload.FileName,
                    URL = result,
                    Extension = fileInfo.Extension
                };
            }
            catch (Exception ex)
            {
                throw new InvalidParameterException(ex.Message);

            }

        }
        else throw new InvalidParameterException("File is not existed!");
    }
}
