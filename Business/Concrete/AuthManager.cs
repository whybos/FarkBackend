using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            var rawEmail = !string.IsNullOrWhiteSpace(userForRegisterDto?.email)
                ? userForRegisterDto.email
                : userForRegisterDto?.username;

            if (userForRegisterDto == null || string.IsNullOrWhiteSpace(rawEmail) || string.IsNullOrWhiteSpace(password))
            {
                return new ErrorDataResult<User>("E-posta ve şifre alanları zorunludur.");
            }

            var normalizedEmail = Truncate(rawEmail.Trim(), 50);
            var defaultName = normalizedEmail.Contains("@") ? normalizedEmail.Split('@')[0] : normalizedEmail;

            var rawFirstName = !string.IsNullOrWhiteSpace(userForRegisterDto.firstName)
                ? userForRegisterDto.firstName
                : (!string.IsNullOrWhiteSpace(userForRegisterDto.name) ? userForRegisterDto.name : defaultName);

            var rawLastName = !string.IsNullOrWhiteSpace(userForRegisterDto.lastName)
                ? userForRegisterDto.lastName
                : "Admin";

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var hasAnySuperAdmin = _userService.AnySuperAdminExists();

            var user = new User
            {
                Email = normalizedEmail,
                FirstName = Truncate(rawFirstName.Trim(), 50),
                LastName = Truncate(rawLastName.Trim(), 50),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };

            _userService.Add(user);

            // EF Core Id ataması yapılmadıysa e-posta üzerinden kaydedilen kullanıcıyı çek
            if (user.Id <= 0)
            {
                var createdUser = _userService.GetByMail(normalizedEmail)?.Data;
                if (createdUser != null)
                {
                    user = createdUser;
                }
            }

            // Eğer sistemde hiç superAdmin yoksa ilk eklenen kullanıcıyı superAdmin yap, varsa admin rolü ata
            var roleToAssign = !hasAnySuperAdmin ? "superAdmin" : "admin";
            if (user.Id > 0)
            {
                try
                {
                    _userService.AddRoleToUser(user.Id, roleToAssign);
                }
                catch
                {
                    // Rol tablosu kaynaklı bir hata kullanıcı kaydını engellemesin
                }
            }

            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        private string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        [SecuredOperation("superAdmin,admin")]
        public IResult Update(User userForRegisterDto, string password)
        {
            var getResult = _userService.GetById(userForRegisterDto.Id);

            if (getResult == null || getResult.Data == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            getResult.Data.Email = userForRegisterDto.Email != null ? userForRegisterDto.Email : getResult.Data.Email;
            getResult.Data.FirstName = userForRegisterDto.FirstName != null ? userForRegisterDto.FirstName : getResult.Data.FirstName;
            getResult.Data.LastName = userForRegisterDto.LastName != null ? userForRegisterDto.LastName : getResult.Data.LastName;

            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
                getResult.Data.PasswordHash = passwordHash;
                getResult.Data.PasswordSalt = passwordSalt;
            }

            _userService.Update(getResult.Data);

            return new SuccessResult(Messages.Updated);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            if (userForLoginDto == null || string.IsNullOrWhiteSpace(userForLoginDto.Email) || string.IsNullOrWhiteSpace(userForLoginDto.Password))
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            var userToCheck = _userService.GetByMail(userForLoginDto.Email.Trim()).Data;
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);
        }

        public IResult UserExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new ErrorResult("Geçersiz e-posta adresi");
            }

            if (_userService.GetByMail(email.Trim()).Data != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claimsResult = _userService.GetClaims(user);
            var claimsList = claimsResult?.Data ?? new List<OperationClaim>();

            // Eğer giriş yapan veya yeni eklenen kullanıcının henüz rolü yoksa otomatik rol ata
            if (!claimsList.Any() && user != null && user.Id > 0)
            {
                try
                {
                    var roleToAssign = !_userService.AnySuperAdminExists() ? "superAdmin" : "admin";
                    _userService.AddRoleToUser(user.Id, roleToAssign);
                    claimsList = _userService.GetClaims(user)?.Data ?? new List<OperationClaim>();
                }
                catch
                {
                }
            }

            var accessToken = _tokenHelper.CreateToken(user, claimsList);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        [SecuredOperation("superAdmin,admin")]
        public IResult Validate()
        {
            return new SuccessResult();
        }
    }
}
