using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(user));
        }

        public IResult Add(User user)
        {
            _userDal.Add(user);
            return new SuccessResult(Messages.Added);
        }
        [SecuredOperation("superAdmin")]
        public IDataResult<List<User>> GetAll()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetAll());
        }

        public IDataResult<User> GetByMail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new ErrorDataResult<User>();
            }
            var normalizedEmail = email.Trim();
            return new SuccessDataResult<User>(_userDal.Get(u => u.Email == normalizedEmail));
        }

        public IDataResult<List<UserDetailDto>> GetUserDetails()
        {
            var result = _userDal.GetUserDetails();
            return new SuccessDataResult<List<UserDetailDto>>(result);
        }

        public IResult Update(User user)
        {
            var userToUpdate = _userDal.Get(u => u.Id == user.Id);
            if (userToUpdate == null)
            {
                return new ErrorResult("Kullanıcı bulunamadı");
            }

            userToUpdate.FirstName = user.FirstName ?? userToUpdate.FirstName;
            userToUpdate.LastName = user.LastName ?? userToUpdate.LastName;
            userToUpdate.Email = user.Email ?? userToUpdate.Email;
            userToUpdate.Status = user.Status;
            if (user.PasswordHash != null && user.PasswordHash.Length > 0)
            {
                userToUpdate.PasswordHash = user.PasswordHash;
            }
            if (user.PasswordSalt != null && user.PasswordSalt.Length > 0)
            {
                userToUpdate.PasswordSalt = user.PasswordSalt;
            }

            _userDal.Update(userToUpdate);
            return new SuccessResult(Messages.Updated);
        }

        public IDataResult<User> GetById(int id)
        {
            var user = _userDal.Get(u => u.Id == id);

            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            return new ErrorDataResult<User>();
        }

        public IResult AddRoleToUser(int userId, string roleName)
        {
            var user = _userDal.Get(u => u.Id == userId);
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı");

            _userDal.AddRoleToUser(userId, roleName);

            return new SuccessResult("Rol başarıyla eklendi");
        }

        public bool IsSuperAdmin(int userId)
        {
            return _userDal.IsSuperAdmin(userId);
        }

        public bool AnySuperAdminExists()
        {
            return _userDal.AnySuperAdminExists();
        }

        [SecuredOperation("superAdmin")]
        public IResult Delete(int userId)
        {
            var userToDelete = _userDal.Get(u => u.Id == userId);
            if (userToDelete == null)
                return new ErrorResult("Kullanıcı bulunamadı");

            if (_userDal.IsSuperAdmin(userId))
                return new ErrorResult("SuperAdmin kullanıcısı silinemez!");

            _userDal.DeleteUserClaims(userId);
            _userDal.Delete(userToDelete);
            return new SuccessResult("Kullanıcı silindi");
        }

        [SecuredOperation("superAdmin")]
        public IResult ResetUsers()
        {
            var deletedCount = _userDal.ResetUsersExceptSuperAdmin();
            return new SuccessResult($"SuperAdmin dışındaki tüm kullanıcılar ({deletedCount} kullanıcı) başarıyla silindi.");
        }

        [SecuredOperation("superAdmin")]
        public IResult ResetDatabase()
        {
            var deletedCount = _userDal.ResetDatabaseExceptSuperAdmin();
            return new SuccessResult($"Veritabanı sıfırlandı. SuperAdmin korundu, {deletedCount} kullanıcı ve tüm içerik verileri temizlendi.");
        }

    }
}
