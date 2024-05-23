using System;
using System.Data;
using System.Data.SqlClient;

namespace Northwind.DataAccess.SqlServer;

internal static class SqlCommandExtensions
{
    public static void SetParameter<T>(this SqlCommand command, string name, T value, SqlDbType type, int? size = null, bool isNullable = true)
    {
        if (size.HasValue)
        {
            command.Parameters.Add(name, type, size.Value);
        }
        else
        {
            command.Parameters.Add(name, type);
        }

        command.Parameters[name].IsNullable = isNullable;
        command.Parameters[name].Value = value ?? Convert.DBNull;
    }
}
