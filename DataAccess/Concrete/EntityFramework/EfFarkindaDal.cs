using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfFarkindaDal : EfEntityRepositoryBase<Farkinda, MyDatabaseContext>, IFarkindaDal
    {
        private static bool _tableInitialized = false;

        public EfFarkindaDal(DbContextOptions<MyDatabaseContext> dbContextOptions) : base(dbContextOptions)
        {
            if (!_tableInitialized)
            {
                try
                {
                    using (var context = new MyDatabaseContext(dbContextOptions))
                    {
                        context.Database.ExecuteSqlRaw(@"
                            CREATE TABLE IF NOT EXISTS ""Farkinda"" (
                                ""Id"" SERIAL PRIMARY KEY,
                                ""Title"" TEXT NULL,
                                ""Date"" TEXT NULL,
                                ""Link"" TEXT NULL,
                                ""Photo"" TEXT NULL
                            );
                            ALTER TABLE ""Farkinda"" ADD COLUMN IF NOT EXISTS ""Photo"" TEXT NULL;
                        ");
                    }
                    _tableInitialized = true;
                }
                catch
                {
                    // Bağlantı henüz hazır değilse sonraki istekte tekrar dener
                }
            }
        }
    }
}
