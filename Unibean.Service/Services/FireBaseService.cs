﻿using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class FireBaseService : IFireBaseService
{
    public async Task<string> GetLinkAsync(string fileName, string folder)
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public void PushNotificationToStudent(Message message)
    {
        try
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("private_key.json"),
                });
            }

            FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public string PushNotificationToTopic(string topic)
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("private_key.json"),
            });
        }

        // This registration token comes from the client FCM SDKs.
        // See documentation on defining a message payload.
        var message = new Message()
        {
            Data = new Dictionary<string, string>()
            {
                { "myData", "2024" },
            },
            //Token = registrationToken,
            Topic = topic,
            Notification = new Notification()
            {
                Title = "Test from code 1",
                Body = "Here is your test!",
                ImageUrl = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/accounts%2FUB.png?alt=media&token=15664825-9e64-490c-9c50-211a6b022775"
            }
        };

        // Send a message to the device corresponding to the provided
        // registration token.
        return FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
    }

    public async Task<bool> RemoveFileAsync(string fileName, string folder)
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<FireBaseFile> UploadFileAsync(IFormFile fileUpload, string folder)
    {
        try
        {
            FileInfo fileInfo = new(fileUpload.FileName);
            string fileName = Ulid.NewUlid().ToString();
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
                    ).Child(folder).Child(fileName)
                    .PutAsync(fs, CancellationToken.None);
                try
                {
                    var result = await cancellation;

                    return new FireBaseFile
                    {
                        FileName = fileName,
                        URL = result,
                        Extension = fileInfo.Extension
                    };
                }
                catch (Exception ex)
                {
                    throw new InvalidParameterException(ex.Message);
                }
            }
            else throw new InvalidParameterException("Tập tin không tồn tại");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
