using API_Training3.App.Databases.MongoDB;
using API_Training3.Modules.Accounts.Entities;
using API_Training3.Modules.Accounts.Requests;
using API_Training3.Modules.Accounts.Response;
using Aspose.Imaging.ImageOptions;
using AutoMapper;
using Firebase.Auth;
using Firebase.Storage;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RasterEdge.Imaging.DICOM;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dicom;
using Dicom.Imaging;
using Dicom.Imaging.Codec;

namespace API_Training3.Modules.Accounts.Services
{
    public interface IAccountService
    {
        (object data, string message) Login(LoginRequest request);
        (object data, string message) SignUp(SignUpRequest signUpRequest);
        Task<(object data, string message)> SocialLogin(LoginSocialRequest request);
        Task<List<string>> UploadFileURL(string path, string name);
        Task<string> TestSocialLogin(LoginSocialRequest token);
        Task<List<string>> UploadFile(List<IFormFile> files);
        Task<List<string>> UploadFileFirebase(List<IFormFile> files);

        Task<List<string>> ConvertDICOMtoPng(List<IFormFile> files);

        Task<List<string>> ConvertDICOMtoPng2(List<IFormFile> files);
        Task<List<string>> ConvertDICOMtoPng3(List<IFormFile> files);
        Task<List<string>> ConvertDICOMtoPng4(List<IFormFile> files);


    }

    public class AccountService : IAccountService
    {
        private readonly string savePathAspose = @"C:\Users\Tan\Desktop\Dicom Data\ConvertToPNG\";
        private readonly string savePathRasterEdge = @"C:\Users\Tan\Desktop\Dicom Data\ConvertToPNG\RasterEdge\";

        private readonly IMongoDBWrapper _mongoDb;
        private readonly IMapper Mapper;
        private readonly IConfiguration _configuration;
        private readonly FirebaseAuthProvider firebaseAuthProvider;
        public AccountService(IConfiguration configuration, IMapper mapper, IMongoDBWrapper mongoDb)
        {
            _configuration = configuration;
            Mapper = mapper;
            _mongoDb = mongoDb;
            firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(_configuration["Firebase:APIKey"]));

        }

        public async Task<List<string>> UploadFile(List<IFormFile> files)
        {

            // full path to file in temp location
            /*var filePath = Path.GetTempFileName();*/
            var cancellation = new CancellationTokenSource();
            List<string> urls = new List<string>();
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        stream.Position = 0;
                        var data = new MemoryStream();
                        data.Seek(0, SeekOrigin.Begin);
                        await formFile.CopyToAsync(data);
                        stream.Close();
                        var task = new FirebaseStorage(_configuration["Firebase:Bucket"])
                            .Child("images")
                            .Child(formFile.FileName)
                            .PutAsync(data, cancellation.Token);
                        // Track progress of the upload
                        task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                        // await the task to wait until upload completes and get the download url
                        urls.Add(task.TargetUrl);
                    }
                }
                // process uploaded files
                // Don't rely on or trust the FileName property without validation.
            }
            return urls;
        }

        public async Task<List<string>> UploadFileFirebase(List<IFormFile> files)
        {

            // full path to file in temp location
            /*var filePath = Path.GetTempFileName();*/
            var cancellation = new CancellationTokenSource();
            List<string> urls = new List<string>();
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        stream.Close();
                        var task = new FirebaseStorage(_configuration["Firebase:Bucket"])
                            .Child("images")
                            .Child(formFile.FileName)
                            .PutAsync(File.OpenRead(filePath), cancellation.Token);
                        // Track progress of the upload
                        task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

                        // await the task to wait until upload completes and get the download url
                        urls.Add(task.TargetUrl);
                    }
                }
                // process uploaded files
                // Don't rely on or trust the FileName property without validation.
            }
            return urls;
        }

        public async Task<List<string>> UploadFileURL(string path, string name)
        {

            // full path to file in temp location
            var cancellation = new CancellationTokenSource();
            List<string> urls = new List<string>();
            /* long size = files.Sum(f => f.Length);*/

            /*    foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {*/
            /*     var filePath = Path.GetTempFileName();*/

            /* using (var stream = new FileStream(filePath,
             FileMode.Create))
                     {*/
            var task = new FirebaseStorage(_configuration["Firebase:Bucket"])
                .Child("images")
                .Child(name)
                .PutAsync(File.Open(path, FileMode.Open));

            // Track progress of the upload
            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // await the task to wait until upload completes and get the download url
            urls.Add(task.TargetUrl);
            /*    }
            }*/
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            /*   }*/
            return urls;

        }

        public (object data, string message) Login(LoginRequest request)
        {
            Account acc = _mongoDb.Accounts.FindByCondition(a => a.Email.Equals(request.Email) && a.Password.Equals(request.Password)).FirstOrDefault();
            if (acc != null)
            {
                LoginResponse loginResponse = new LoginResponse()
                {
                    DisplayName = acc.DisplayName,
                    Token = GenerateToken(acc)
                };
                return (loginResponse, "Success");
            }
            return (null, "Email or password is not correct");
        }


        public string GenerateToken(Account user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("AccountId", user.Id),
                    new Claim("DisplayName",user.DisplayName),
                    new Claim("Email", user.Email),
                    //new Claim("DisplayName", user.DisplayName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public (object data, string message) SignUp(SignUpRequest signUpRequest)
        {
            Account account = Mapper.Map<Account>(signUpRequest);
            _mongoDb.Accounts.Add(account);
            return (account, "Success");
        }

        public async Task<string> TestSocialLogin(LoginSocialRequest token)
        {
            //, Application Default Credentials (ADC) is able to implicitly determine your credentials, 
            //allowing you to use service account credentials when testing or running in non-Google environments.
            /*   FirebaseApp.Create(new AppOptions()
               {
                   Credential = GoogleCredential.GetApplicationDefault(),
               });*/
            var decoded = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token.Token);
            UserRecord userRecord = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.GetUserAsync(decoded.Uid);
            /*FirebaseAuthLink firebaseResult = null;*/
            Object data = await firebaseAuthProvider.GetUserAsync(token.Token);
            string customToken = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(decoded.Uid);

            return customToken;
        }
        public async Task<(object data, string message)> SocialLogin(LoginSocialRequest request)
        {

            FirebaseAuthLink firebaseResult = null;
            try
            {
                firebaseResult = await firebaseAuthProvider.SignInWithOAuthAsync(FirebaseAuthType.Google, request.Token);
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                return (null, JObject.Parse(ex.ResponseData)["error"]["message"].ToString());
            }
            if (firebaseResult is null)
            {
                return (null, "Error when login");
            }
            if (_mongoDb.Accounts.Any(a => a.Email.Equals(firebaseResult.User.Email) || a.Id.Equals(firebaseResult.User.LocalId)))
            {
                LoginResponse loginResponse = new LoginResponse()
                {
                    Id = firebaseResult.User.LocalId,
                    DisplayName = firebaseResult.User.DisplayName,
                    Email = firebaseResult.User.Email,
                    Phone = firebaseResult.User.PhoneNumber,
                    PhotoUrl = firebaseResult.User.PhotoUrl,
                    Token = GenerateToken(
                        new Account()
                        {
                            Id = firebaseResult.User.LocalId,
                            DisplayName = firebaseResult.User.DisplayName,
                            Email = firebaseResult.User.Email,
                            PhotoUrl = firebaseResult.User.PhotoUrl,
                        }
                     )
                };
                return (loginResponse, "Login Success");
            }
            else
            {
                return (null, "This account doesn't have permission");
            }

        }

        public async Task<List<string>> ConvertDICOMtoPng(List<IFormFile> files)
        {
            List<string> filePaths = new List<string>();
              string fileName;
             foreach (var file in files)
             {
                 if (file.Length > 0)
                 {
                     using (var ms = new MemoryStream())
                     {
                         await file.CopyToAsync(ms);


                         using (Aspose.Imaging.FileFormats.Dicom.DicomImage image = new Aspose.Imaging.FileFormats.Dicom.DicomImage(ms))
                         {
                            // Set the active page to be converted to JPEG
                            ms.Position = 0;
                             int activePage = image.ActivePageIndex;
                             image.ActivePage = (Aspose.Imaging.FileFormats.Dicom.DicomPage)image.Pages[0];
                             // Save as PNG
                             fileName = Path.ChangeExtension(file.FileName, ".png");
                             image.Save(savePathAspose + fileName, new PngOptions());
                             filePaths.Add(savePathAspose + fileName);
                         }
                     }
                 }
             }
             return filePaths;
        }

        public async Task<List<string>> ConvertDICOMtoPng2(List<IFormFile> files)
        {
            List<string> filePaths = new List<string>();
            string fileName;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);

                        DCMDocument doc = new DCMDocument(ms);
                        
                        fileName = file.FileName.Replace(".dicom", "");
                        doc.ConvertToImages(RasterEdge.Imaging.Basic.ImageType.PNG, savePathRasterEdge, fileName);
                             filePaths.Add(savePathRasterEdge + fileName);

                    }
                }
            }
            return filePaths;
        }

        public async Task<List<string>> ConvertDICOMtoPng3(List<IFormFile> files)
        {
            List<string> filePaths = new List<string>();
            var cancellation = new CancellationTokenSource();
            long size = files.Sum(f => f.Length);
            string fileName;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();

                    fileName = Path.ChangeExtension(formFile.FileName, ".png");

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        using (Aspose.Imaging.FileFormats.Dicom.DicomImage image = new Aspose.Imaging.FileFormats.Dicom.DicomImage(stream))
                        {
                            await formFile.CopyToAsync(stream);

                            // Set the active page to be converted to JPEG
                            bool info = image.FileInfo.IsLittleEndian;
                            if (!info)
                            {
                                image.ActivePage = (Aspose.Imaging.FileFormats.Dicom.DicomPage)image.Pages[0];
                                // Save as PNG
                                image.Save(savePathAspose + fileName, new PngOptions());
                            }
                            else
                            {
                                await formFile.CopyToAsync(stream);
                                stream.Close();
                                DicomFile file = DicomFile.Open(filePath);
                                var transcoder = new DicomTranscoder(file.Dataset.InternalTransferSyntax, DicomTransferSyntax.ImplicitVRLittleEndian);
                                var newFile = transcoder.Transcode(file);
                                newFile.Save(savePathAspose + fileName);
                            }
                            
                            filePaths.Add(savePathAspose + fileName);
                        }
                        // await the task to wait until upload completes and get the download url
                    }
                }
                // process uploaded files
                // Don't rely on or trust the FileName property without validation.
            }
            return filePaths;
        }

        public async Task<List<string>> ConvertDICOMtoPng4(List<IFormFile> files)
        {
            List<string> filePaths = new List<string>();
            string fileName;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                        fileName = Path.ChangeExtension(formFile.FileName, ".png");
                    /* using (MemoryStream ms = new MemoryStream())
                     {
                         await formFile.CopyToAsync(ms);
                         DicomFile dcmf = DicomFile.Open(ms, FileReadOption.ReadAll);
                     }

                     using (FileStream stream = new FileStream(filePath, FileMode.Create))
                     {
                         await formFile.CopyToAsync(stream);
                         stream.Close();
                     }*/
                    Stream dcmStream = formFile.OpenReadStream();

                    DicomFile file = DicomFile.Open(dcmStream);
                        if (file.Dataset.InternalTransferSyntax.Equals(DicomTransferSyntax.ImplicitVRLittleEndian))
                        {
                            DCMDocument doc = new DCMDocument(formFile.OpenReadStream());
                            doc.ConvertToImages(RasterEdge.Imaging.Basic.ImageType.PNG, savePathRasterEdge, fileName);
                            filePaths.Add(savePathRasterEdge + formFile.FileName);
                        }
                        else
                        {
                            var transcoder = new DicomTranscoder(file.Dataset.InternalTransferSyntax, DicomTransferSyntax.ImplicitVRLittleEndian);
                            var newFile = transcoder.Transcode(file);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            newFile.Save(ms);
                            DCMDocument doc = new DCMDocument(ms);
                            doc.ConvertToImages(RasterEdge.Imaging.Basic.ImageType.PNG, savePathRasterEdge, fileName);
                            filePaths.Add(savePathRasterEdge + formFile.FileName);
                        }
                            
                        }
                    }
            }
            return filePaths;
        }
    }
}



