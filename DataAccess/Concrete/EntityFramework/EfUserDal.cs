using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, MyDatabaseContext>, IUserDal
    {
        private readonly DbContextOptions<MyDatabaseContext> _dbContextOptions;
        public EfUserDal(DbContextOptions<MyDatabaseContext> dbContextOptions) : base(dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return result.ToList();

            }
        }

        public List<UserDetailDto> GetUserDetails()
        {
            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                var result = from u in context.Users
                             select new UserDetailDto
                             {
                                 Id = u.Id,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName,
                                 Email = u.Email,
                                 Status = u.Status
                             };
                return result.ToList();
            }
        }
        public void AddRoleToUser(int userId, string roleName)
        {
            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                // Rolü bul, yoksa ekle
                var claim = context.OperationClaims.SingleOrDefault(c => c.Name == roleName);
                if (claim == null)
                {
                    claim = new OperationClaim { Name = roleName };
                    context.OperationClaims.Add(claim);
                    context.SaveChanges();
                }

                // Kullanıcıya rol ekle, tekrar eklenmesin
                var exists = context.UserOperationClaims
                    .Any(uoc => uoc.UserId == userId && uoc.OperationClaimId == claim.Id);

                if (!exists)
                {
                    context.UserOperationClaims.Add(new UserOperationClaim
                    {
                        UserId = userId,
                        OperationClaimId = claim.Id
                    });
                    context.SaveChanges();
                }
            }
        }


    }
}
