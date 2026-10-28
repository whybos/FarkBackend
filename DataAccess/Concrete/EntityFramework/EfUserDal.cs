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
                var normalizedRole = (roleName ?? "admin").Trim();
                var claim = context.OperationClaims
                    .AsEnumerable()
                    .FirstOrDefault(c => string.Equals(c.Name?.Trim(), normalizedRole, StringComparison.OrdinalIgnoreCase));

                if (claim == null)
                {
                    claim = new OperationClaim { Name = normalizedRole };
                    context.OperationClaims.Add(claim);
                    context.SaveChanges();
                }

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

        public bool IsSuperAdmin(int userId)
        {
            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                var claims = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == userId
                             select operationClaim.Name;

                return claims
                    .AsEnumerable()
                    .Any(name => string.Equals(name?.Trim(), "superAdmin", StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool AnySuperAdminExists()
        {
            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                var superAdminClaims = context.OperationClaims
                    .AsEnumerable()
                    .Where(c => string.Equals(c.Name?.Trim(), "superAdmin", StringComparison.OrdinalIgnoreCase))
                    .Select(c => c.Id)
                    .ToList();

                if (!superAdminClaims.Any())
                {
                    return false;
                }

                return context.UserOperationClaims.Any(uoc =>
                    superAdminClaims.Contains(uoc.OperationClaimId) &&
                    context.Users.Any(u => u.Id == uoc.UserId));
            }
        }

        public void DeleteUserClaims(int userId)
        {
            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                var claims = context.UserOperationClaims.Where(uoc => uoc.UserId == userId).ToList();
                if (claims.Any())
                {
                    context.UserOperationClaims.RemoveRange(claims);
                    context.SaveChanges();
                }
            }
        }

        public int ResetUsersExceptSuperAdmin()
        {
            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                var superAdminUserIds = GetProtectedSuperAdminUserIds(context);

                var nonSuperAdminClaims = context.UserOperationClaims
                    .Where(uoc => !superAdminUserIds.Contains(uoc.UserId))
                    .ToList();
                if (nonSuperAdminClaims.Any())
                {
                    context.UserOperationClaims.RemoveRange(nonSuperAdminClaims);
                }

                var usersToDelete = context.Users
                    .Where(u => !superAdminUserIds.Contains(u.Id))
                    .ToList();
                if (usersToDelete.Any())
                {
                    context.Users.RemoveRange(usersToDelete);
                }

                context.SaveChanges();
                return usersToDelete.Count;
            }
        }

        public int ResetDatabaseExceptSuperAdmin()
        {
            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                var deletedUsersCount = ResetUsersExceptSuperAdmin();

                context.NewsDetail.RemoveRange(context.NewsDetail.ToList());
                context.News.RemoveRange(context.News.ToList());
                context.Blog.RemoveRange(context.Blog.ToList());
                context.Slider.RemoveRange(context.Slider.ToList());
                context.FormsActive.RemoveRange(context.FormsActive.ToList());
                context.AboutUs.RemoveRange(context.AboutUs.ToList());
                context.Footer.RemoveRange(context.Footer.ToList());
                context.Navbars.RemoveRange(context.Navbars.ToList());

                try
                {
                    context.Farkinda.RemoveRange(context.Farkinda.ToList());
                }
                catch
                {
                    // Farkinda tablosu henüz veritabanında oluşturulmadıysa hata fırlatmasını engelle
                }

                context.SaveChanges();
                return deletedUsersCount;
            }
        }

        private List<int> GetProtectedSuperAdminUserIds(MyDatabaseContext context)
        {
            var superAdminClaimIds = context.OperationClaims
                .AsEnumerable()
                .Where(c => string.Equals(c.Name?.Trim(), "superAdmin", StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Id)
                .ToList();

            var superAdminUserIds = context.UserOperationClaims
                .Where(uoc => superAdminClaimIds.Contains(uoc.OperationClaimId))
                .Select(uoc => uoc.UserId)
                .Distinct()
                .ToList();

            // Güvenlik önlemi: Eğer tabloda superAdmin rolü atanmış aktif bir kullanıcı bulunamazsa,
            // ilk kullanıcıyı (en küçük Id'li) superAdmin olarak koru ki sistem tamamen kullanıcısız kalmasın.
            if (!superAdminUserIds.Any() || !context.Users.Any(u => superAdminUserIds.Contains(u.Id)))
            {
                var firstUser = context.Users.OrderBy(u => u.Id).FirstOrDefault();
                if (firstUser != null)
                {
                    superAdminUserIds.Add(firstUser.Id);
                }
            }

            return superAdminUserIds;
        }
    }
}
