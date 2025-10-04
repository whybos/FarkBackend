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
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user = new User
            {
                Email = userForRegisterDto.email,
                FirstName = userForRegisterDto.firstName,
                LastName = userForRegisterDto.lastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true // kullanıcı aktif olsun
            };

            // Kullanıcıyı ekle
            _userService.Add(user);

            // Super admin eklediği için admin rolünü ata
            _userService.AddRoleToUser(user.Id, "admin");

            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        [SecuredOperation("superAdmin")]
        public IResult Update(User userForRegisterDto, string password)
        {
            var getResult = _userService.GetById(userForRegisterDto.Id);

            if (getResult == null)
            {
                return new ErrorResult("Data Bulunamadı");
            }

            getResult.Data.Email = userForRegisterDto.Email != null ? userForRegisterDto.Email : getResult.Data.Email;

            _userService.Update(getResult.Data);

            return new SuccessResult();
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email).Data;
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
            if (_userService.GetByMail(email).Data != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        [SecuredOperation("superAdmin,admin")]
        public IResult Validate()
        {
            return new SuccessResult();
        }
    }
}
