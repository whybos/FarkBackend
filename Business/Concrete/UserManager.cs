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
            return new SuccessDataResult<User>(_userDal.Get(u => u.Email == email));
        }

        public IDataResult<List<UserDetailDto>> GetUserDetails()
        {
            var result = _userDal.GetUserDetails();
            return new SuccessDataResult<List<UserDetailDto>>(result);
        }

        public IResult Update(User user)
        {
            var userToUpdate = _userDal.Get(u => u.Id == user.Id);
            userToUpdate.Status = user.Status;
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

            _userDal.AddRoleToUser(userId, roleName); // UserRoles tablosuna ekleme yap

            return new SuccessResult("Rol başarıyla eklendi");
        }
        [SecuredOperation("superAdmin")]
        public IResult Delete(int userId)
        {
            var userToDelete = _userDal.Get(u => u.Id == userId);
            if (userToDelete == null)
                return new ErrorResult("Kullanıcı bulunamadı");

            _userDal.Delete(userToDelete);
            return new SuccessResult("Kullanıcı silindi");
        }

    }
}
