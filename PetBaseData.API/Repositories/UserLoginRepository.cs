using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PetBaseData.API.Data;
using PetBaseData.API.Entities;
using PetBaseData.API.Models;
using PetBaseData.API.Helpers;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetBaseData.API.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace PetBaseData.API.Repositories
{
    public class UserLoginRepository : IUserLoginRepository
    {
        private readonly MailSettings _mailSettings;
        private readonly IUserDataContext _context;
        private readonly IConfiguration _configuration;

        private const long otpTimeOut = (long)600000000 * 5;

        public UserLoginRepository(IUserDataContext context, IConfiguration configuration, IOptions<MailSettings> mailSettings)
        {
            _context = context;
            _configuration = configuration;
            _mailSettings = mailSettings.Value;
        }

        public async Task<LoginResponse> LoginUser(LoginRequest loginRequest)
        {
            var hashPass = $"{loginRequest.Password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == loginRequest.Username);
            UserData userData = userAsyncCursor.FirstOrDefault();

            if(userData == null)
            {
                return new LoginResponse()
                {
                    ErrorMessage = ErrorCases.UserDoesNotExist
                };
            }

            if(userData.Password != hashPass)
            {
                return new LoginResponse()
                {
                    ErrorMessage = ErrorCases.IncorrectPassword
                };
            }

            if (!userData.IsVerified)
            {
                return new LoginResponse()
                {
                    ErrorMessage = ErrorCases.AccountNotVerified
                };
            }

            IAsyncCursor<SavedData> savedDataCursor = await _context.SavedDataCollection.FindAsync(g => g.Id == userData.SavedDataId);

            LoginResponse returnValue = new()
            {
                PlayerData = savedDataCursor.SingleOrDefault(),
                PlayerId = userData.Id,
                ErrorMessage = ErrorCases.AllGood
            };

            return returnValue;
        }

        public async Task<LoginResponse> RegisterUser(LoginRequest loginRequest)
        {

            if (loginRequest.EmailAddress.IsValidEmail())
            {
                return new LoginResponse()
                {
                    ErrorMessage = ErrorCases.IncorrectEmail
                };
            }

            var hashPass = $"{loginRequest.Password}{_configuration.GetValue<string>("Salt")}".Sha256();
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == loginRequest.Username);

            UserData userData = userAsyncCursor.FirstOrDefault();

            if(userData != null)
            {
                return new LoginResponse()
                {
                    ErrorMessage = ErrorCases.UserNameInUse
                };
            }

            SavedData newSavedData = UserDataSeed.GetDefaultSavedData();
            newSavedData.ListOfPets = _context.PetObjectCollection
                                                                .Find(p => true)
                                                                .ToEnumerable();


            Random generator = new Random();
            string OtpCode = generator.Next(0, 1000000).ToString("D6");

            await _context.SavedDataCollection.InsertOneAsync(newSavedData);
            UserData newUser = new()
            {
                SavedDataId = newSavedData.Id,
                Username = loginRequest.Username,
                Password = hashPass,
                EmailAddress = loginRequest.EmailAddress,
                Otp = OtpCode,
                CodeGenerateTime = DateTime.Now.Ticks.ToString(),
                IsVerified = false
            };

            await _context.UserDataCollection.InsertOneAsync(newUser);

            SendOtpEmail(OtpCode, userData.EmailAddress);

            LoginResponse returnValue = new()
            {
                PlayerData = newSavedData,
                PlayerId = newUser.Id,
                ErrorMessage = ErrorCases.AllGood
            };
            return returnValue;
        }

        public async Task<LoginResponse> ValidateUser(LoginRequest loginRequest)
        {
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == loginRequest.Username);

            UserData userData = userAsyncCursor.FirstOrDefault();

            if(userData == null)
            {
                return new LoginResponse
                {
                    ErrorMessage = ErrorCases.UserMismatch
                };
            }

            if(userData.Otp != loginRequest.OtpCode)
            {
                Random generator = new Random();
                string OtpCode = generator.Next(0, 1000000).ToString("D6");
                SendOtpEmail(OtpCode, userData.EmailAddress);

                userData.Otp = OtpCode;
                userData.CodeGenerateTime = DateTime.Now.Ticks.ToString();

                var replaceResultExpire = await _context.UserDataCollection.ReplaceOneAsync(p => p.Id == userData.Id, userData);

                if (replaceResultExpire.IsAcknowledged && replaceResultExpire.ModifiedCount > 0)
                {
                    return new LoginResponse
                    {
                        ErrorMessage = ErrorCases.OtpIncorrect
                    };
                }

                return new LoginResponse
                {
                    ErrorMessage = ErrorCases.UnknownError
                };
            }

            long timeDifference = DateTime.Now.Ticks - long.Parse(userData.CodeGenerateTime);

            if(timeDifference > otpTimeOut)
            {
                Random generator = new Random();
                string OtpCode = generator.Next(0, 1000000).ToString("D6");
                SendOtpEmail(OtpCode, userData.EmailAddress);

                userData.Otp = OtpCode;
                userData.CodeGenerateTime = DateTime.Now.Ticks.ToString();

                var replaceResultExpire = await _context.UserDataCollection.ReplaceOneAsync(p => p.Id == userData.Id, userData);

                if (replaceResultExpire.IsAcknowledged && replaceResultExpire.ModifiedCount > 0)
                {
                    return new LoginResponse
                    {
                        ErrorMessage = ErrorCases.OtpExpired
                    };
                }

                return new LoginResponse
                {
                    ErrorMessage = ErrorCases.UnknownError
                };
            }

            userData.IsVerified = true;

            var replaceResult = await _context.UserDataCollection.ReplaceOneAsync(p => p.Id == userData.Id, userData);

            if(replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0)
            {
                return new LoginResponse
                {
                    ErrorMessage = ErrorCases.AllGood
                };
            }

            return new LoginResponse
            {
                ErrorMessage = ErrorCases.UnknownError
            };
        }

        public async Task<LoginResponse> ResendOtp(LoginRequest loginRequest)
        {
            IAsyncCursor<UserData> userAsyncCursor = await _context.UserDataCollection.FindAsync(p => p.Username == loginRequest.Username);

            UserData userData = userAsyncCursor.FirstOrDefault();

            if (userData == null)
            {
                return new LoginResponse
                {
                    ErrorMessage = ErrorCases.UserMismatch
                };
            }

            Random generator = new Random();
            string OtpCode = generator.Next(0, 1000000).ToString("D6");

            SendOtpEmail(OtpCode, userData.EmailAddress);

            userData.Otp = OtpCode;
            userData.CodeGenerateTime = DateTime.Now.Ticks.ToString();

            var replaceResult = await _context.UserDataCollection.ReplaceOneAsync(p => p.Id == userData.Id, userData);

            if(replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0)
            {
                return new LoginResponse
                {
                    ErrorMessage = ErrorCases.AllGood
                };
            }

            return new LoginResponse
            {
                ErrorMessage = ErrorCases.UnknownError
            };
        }

        private async void SendOtpEmail(string OtpCode, string emailAddress)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(emailAddress));
            email.Subject = $"Pet Cavern Verification Code: {OtpCode}";
            var builder = new BodyBuilder();

            builder.HtmlBody = "Use this code to verify your account";
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
        }
    }
}
