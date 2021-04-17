using API_Training3.App.Controllers;
using API_Training3.Modules.Accounts.Entities;
using API_Training3.Modules.Accounts.Requests;
using API_Training3.Modules.Accounts.Response;
using API_Training3.Modules.Accounts.Services;
using AutoMapper;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API_Training3.Modules.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {

        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody]  LoginRequest loginRequest)
        {
            (Object account, string message) = _accountService.Login(loginRequest);
            if (account != null)
            {
                return ResponseOk(account, message);
            }
            else
            {
                return ResponseBadRequest(message);
            }
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] SignUpRequest signUpRequest)
        {
            (Object data, string message) = _accountService.SignUp(signUpRequest);
            if (data != null)
            {
                return ResponseCreated(data, message);
            }
            else
            {
                return ResponseBadRequest(message);
            }
        }

        [HttpPost("SocialLogin")]
        public IActionResult SocialLogin([FromBody] LoginSocialRequest loginSocialRequest)
        {
            (Object data, string message) = _accountService.SocialLogin(loginSocialRequest).Result;
            if (data != null)
            {
                return ResponseOk(data, message);
            }
            else
            {
                return ResponseBadRequest(message);
            }
        }
        [HttpPost("FirebaseToken")]
        public IActionResult FirebaseSocialLogin([FromBody] LoginSocialRequest token)
        {
            string customToken = _accountService.TestSocialLogin(token).Result;
            return ResponseOk(customToken);
        }

        [HttpPost("UploadFiles")]
        public async Task<List<String>> Post(string path, string name)
        {
            List<string> urls = await _accountService.UploadFileURL(path, name);
            return urls;
        }

        [HttpPost("UploadFilesToServer")]

        public async Task<List<string>> OnPostUploadAsync(List<IFormFile> files)
        {
            List<string> result = await _accountService.UploadFile(files);
            return result;
        }

        [RequestSizeLimit(10L * 1024L * 1024L * 1024L)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10L * 1024L * 1024L * 1024L)]
        [HttpPost("UploadFilesToFirebase")]

        public async Task<List<string>> OnPostUploadAsync2(List<IFormFile> files)
        {
            List<string> result = await _accountService.UploadFileFirebase(files);
            return result;
        }

        /*[RequestSizeLimit(10L * 1024L * 1024L * 1024L)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10L * 1024L * 1024L * 1024L)]*/
        /* [HttpPost("ConvertToPng")]

         public async Task<List<string>> ConvertToPng([FromForm] List<IFormFile> files)
         {
             List<string> result = await _accountService.ConvertDICOMtoPng(files);
             return result;
         }

         [HttpPost("ConvertToPng2")]

         public async Task<List<string>> ConvertToPng2([FromForm] List<IFormFile> files)
         {
             List<string> result = await _accountService.ConvertDICOMtoPng2(files);
             return result;
         }

         [HttpPost("ConvertToPng3")]

         public async Task<List<string>> ConvertToPng3([FromForm] List<IFormFile> files)
         {
             List<string> result = await _accountService.ConvertDICOMtoPng3(files);
             return result;
         }

         [DisableRequestSizeLimit]
         [HttpPost("ConvertToPng4")]

         public async Task<List<string>> ConvertToPng4([FromForm] List<IFormFile> files)
         {
             List<string> result = await _accountService.ConvertDICOMtoPng4(files);
             return result;
         }*/

        [HttpPost("ConvertToPng")]

        public async Task<List<string>> ConvertToPng2([FromForm] List<IFormFile> files)
        {
            List<string> result = await _accountService.ConvertDICOMtoPng5(files);
            return result;
        }

        [HttpPost("Detect")]

        public async Task<IActionResult> Detect([FromForm] IFormFile file)
        {
            (Object response, string message) = await _accountService.DetectPng(file);
            return ResponseOk(response, message);
        }
    }
}
