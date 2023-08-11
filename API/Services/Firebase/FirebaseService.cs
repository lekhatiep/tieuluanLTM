using API.Dto.Firebase;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Services.Firebase
{
    public class FirebaseService : IFirebaseService
    {

        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;

        public FirebaseService(IHostingEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _config = configuration;
        }
        public async Task<string> UploadFile(FileUploadDto model)
        {
            string ApiKey = _config.GetValue<string>("Firebase:apiKey");
            string Bucket = _config.GetValue<string>("Firebase:bucket");//"ltmtieuluan.appspot.com";
            string AuthEmail = _config.GetValue<string>("Firebase:authEmail");// "tieplk@gmail.com";
            string AuthPassword = _config.GetValue<string>("Firebase:authPassword"); //"Tieplk@123";
            var file = model.File;

            Stream ms = null;
            if (file.Length > 0)
            {

                string path = Path.Combine(_env.WebRootPath, $"images/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //1--Save file and read
                //using (fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                //{
                //    await file.CopyToAsync(fs);
                //}

                //fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Open);
                //2--Dont save file

                ms = file.OpenReadStream();

                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                    })
                    .Child("images")
                    .Child(file.FileName)
                    .PutAsync(ms, cancellation.Token);

                try
                {
                    task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");
                    var link = await task;
                    return link;
                }
                catch (Exception ex)
                {

                    throw;
                }
               
            }

            return "";
        }
    }
}
