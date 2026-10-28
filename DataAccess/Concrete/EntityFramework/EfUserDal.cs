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
        public User AddUser(User user)
        {
            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                SyncUserTableSequences(context);

                try
                {
                    user.Id = 0;
                    context.Users.Add(user);
                    context.SaveChanges();
                    return user;
                }
                catch
                {
                    // Eğer PostgreSQL'de Users.Id kolonu SERIAL/IDENTITY değilse veya sequence kaymışsa manuel Id ata
                }
            }

            using (var fallbackContext = new MyDatabaseContext(_dbContextOptions))
            {
                var nextId = (fallbackContext.Users.AsNoTracking().Select(u => (int?)u.Id).Max() ?? 0) + 1;
                user.Id = nextId;
                fallbackContext.Users.Add(user);
                fallbackContext.SaveChanges();
                return user;
            }
        }

        private void SyncUserTableSequences(MyDatabaseContext context)
        {
            try
            {
                context.Database.ExecuteSqlRaw(@"
                    SELECT setval(pg_get_serial_sequence('""Users""', 'Id'), COALESCE((SELECT MAX(""Id"") FROM ""Users""), 0) + 1, false);
                ");
            }
            catch { }

            try
            {
                context.Database.ExecuteSqlRaw(@"
                    SELECT setval(pg_get_serial_sequence('""OperationClaims""', 'Id'), COALESCE((SELECT MAX(""Id"") FROM ""OperationClaims""), 0) + 1, false);
                ");
            }
            catch { }

            try
            {
                context.Database.ExecuteSqlRaw(@"
                    SELECT setval(pg_get_serial_sequence('""UserOperationClaims""', 'Id'), COALESCE((SELECT MAX(""Id"") FROM ""UserOperationClaims""), 0) + 1, false);
                ");
            }
            catch { }
        }

        public void AddRoleToUser(int userId, string roleName)
        {
            var normalizedRole = (roleName ?? "admin").Trim();
            OperationClaim claim = null;

            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                SyncUserTableSequences(context);

                var allClaims = context.OperationClaims.AsNoTracking().ToList();
                claim = allClaims.FirstOrDefault(c => string.Equals(c.Name?.Trim(), normalizedRole, StringComparison.OrdinalIgnoreCase));

                if (claim == null)
                {
                    try
                    {
                        var newClaim = new OperationClaim { Name = normalizedRole };
                        context.OperationClaims.Add(newClaim);
                        context.SaveChanges();
                        claim = newClaim;
                    }
                    catch
                    {
                        using (var fallbackContext = new MyDatabaseContext(_dbContextOptions))
                        {
                            var nextClaimId = (fallbackContext.OperationClaims.AsNoTracking().Select(c => (int?)c.Id).Max() ?? 0) + 1;
                            var newClaim = new OperationClaim { Id = nextClaimId, Name = normalizedRole };
                            fallbackContext.OperationClaims.Add(newClaim);
                            fallbackContext.SaveChanges();
                            claim = newClaim;
                        }
                    }
                }
            }

            if (claim == null || userId <= 0)
            {
                return;
            }

            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                var exists = context.UserOperationClaims
                    .AsNoTracking()
                    .Any(uoc => uoc.UserId == userId && uoc.OperationClaimId == claim.Id);

                if (!exists)
                {
                    try
                    {
                        var userClaim = new UserOperationClaim
                        {
                            UserId = userId,
                            OperationClaimId = claim.Id
                        };
                        context.UserOperationClaims.Add(userClaim);
                        context.SaveChanges();
                    }
                    catch
                    {
                        using (var fallbackContext = new MyDatabaseContext(_dbContextOptions))
                        {
                            var nextUocId = (fallbackContext.UserOperationClaims.AsNoTracking().Select(uoc => (int?)uoc.Id).Max() ?? 0) + 1;
                            var userClaim = new UserOperationClaim
                            {
                                Id = nextUocId,
                                UserId = userId,
                                OperationClaimId = claim.Id
                            };
                            fallbackContext.UserOperationClaims.Add(userClaim);
                            fallbackContext.SaveChanges();
                        }
                    }
                }
            }
        }

        public bool IsSuperAdmin(int userId)
        {
            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                var claims = (from operationClaim in context.OperationClaims
                              join userOperationClaim in context.UserOperationClaims
                                  on operationClaim.Id equals userOperationClaim.OperationClaimId
                              where userOperationClaim.UserId == userId
                              select operationClaim.Name).ToList();

                return claims.Any(name => string.Equals(name?.Trim(), "superAdmin", StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool AnySuperAdminExists()
        {
            using (var context = new MyDatabaseContext(_dbContextOptions))
            {
                var superAdminClaims = context.OperationClaims
                    .AsNoTracking()
                    .ToList()
                    .Where(c => string.Equals(c.Name?.Trim(), "superAdmin", StringComparison.OrdinalIgnoreCase))
                    .Select(c => c.Id)
                    .ToList();

                if (!superAdminClaims.Any())
                {
                    return false;
                }

                var allUserIds = context.Users.AsNoTracking().Select(u => u.Id).ToList();
                var allUserClaims = context.UserOperationClaims.AsNoTracking().ToList();

                return allUserClaims.Any(uoc =>
                    superAdminClaims.Contains(uoc.OperationClaimId) &&
                    allUserIds.Contains(uoc.UserId));
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
