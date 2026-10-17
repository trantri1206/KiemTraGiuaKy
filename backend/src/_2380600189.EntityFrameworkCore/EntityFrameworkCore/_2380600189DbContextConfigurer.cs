using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace _2380600189.EntityFrameworkCore;

public static class _2380600189DbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<_2380600189DbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<_2380600189DbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}
