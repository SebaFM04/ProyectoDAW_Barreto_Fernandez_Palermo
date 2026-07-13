using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Acceso
    {
        string conn;

        public Acceso()
        {
            conn = "Data Source=.;Initial Catalog=dawRefugio;Integrated Security=True";
            
        }

        public List<T> RetornarLista<T>(string query, Func<SqlDataReader, T> mapFunc, Dictionary<string, object> parametros = null)
        {
            List<T> lista = new List<T>();
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                AsignarParametros(cmd, parametros);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(mapFunc(reader));
                    }
                }
            }
            return lista;
        }

        public void Query(string query, Dictionary<string, object> parametros = null)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                AsignarParametros(cmd, parametros);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public object EjecutarEscalar(string query, Dictionary<string, object> parametros = null)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                AsignarParametros(cmd, parametros);
                connection.Open();
                return cmd.ExecuteScalar();
            }
        }

        private void AsignarParametros(SqlCommand cmd, Dictionary<string, object> parametros)
        {
            cmd.Parameters.Clear();
            if (parametros != null)
            {
                foreach (var parametro in parametros)
                {
                    cmd.Parameters.AddWithValue(parametro.Key, parametro.Value ?? DBNull.Value);
                }
            }
        }

        public string ObtenerCarpetaBackupSQL()
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(
                    @"DECLARE @ruta NVARCHAR(512);
              EXEC master.dbo.xp_instance_regread 
                  N'HKEY_LOCAL_MACHINE',
                  N'Software\Microsoft\MSSQLServer\MSSQLServer',
                  N'BackupDirectory',
                  @ruta OUTPUT;
              SELECT @ruta;", connection))
                {
                    object resultado = cmd.ExecuteScalar();
                    return resultado?.ToString();
                }
            }
        }

        //public void RestaurarBaseDatos(string rutaBackup)
        //{
        //    const string miBase = "dawRefugio";

        //    var builder = new SqlConnectionStringBuilder(conn)
        //    {
        //        InitialCatalog = "master"
        //    };
        //    string connectionStringMaster_941lp = builder.ConnectionString;

        //    using (SqlConnection connection_941lp = new SqlConnection(connectionStringMaster_941lp))
        //    {
        //        connection_941lp.Open();

        //        // 1. Verificar metadata del backup
        //        using (SqlCommand checkCmd = new SqlCommand("RESTORE HEADERONLY FROM DISK = @ruta", connection_941lp))
        //        {
        //            checkCmd.Parameters.AddWithValue("@ruta", rutaBackup);

        //            using (SqlDataReader reader = checkCmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    string dbName = reader["DatabaseName"].ToString();

        //                    if (!string.Equals(dbName, miBase, StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        throw new InvalidOperationException(
        //                            $"El backup corresponde a la base '{dbName}', pero solo se permite restaurar '{miBase}'.");
        //                    }
        //                }
        //                else
        //                {
        //                    throw new InvalidOperationException("No se pudo leer la cabecera del backup.");
        //                }
        //            }
        //        }

        //        using (SqlCommand cmd_941lp = connection_941lp.CreateCommand())
        //        {
        //            // 2. Matar conexiones abiertas
        //            cmd_941lp.CommandText = $@"
        //            DECLARE @kill varchar(8000) = '';
        //            SELECT @kill = @kill + 'KILL ' + CONVERT(varchar(5), session_id) + ';'
        //            FROM sys.dm_exec_sessions
        //            WHERE database_id = DB_ID('{miBase}') AND session_id <> @@SPID;
        //            EXEC(@kill);";
        //            cmd_941lp.ExecuteNonQuery();

        //            // 3. Single user
        //            cmd_941lp.CommandText = $@"ALTER DATABASE [{miBase}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
        //            cmd_941lp.ExecuteNonQuery();

        //            // 4. Restaurar
        //            cmd_941lp.CommandText = $@"RESTORE DATABASE [{miBase}] 
        //                               FROM DISK = @ruta 
        //                               WITH REPLACE;";
        //            cmd_941lp.Parameters.AddWithValue("@ruta", rutaBackup);
        //            cmd_941lp.ExecuteNonQuery();

        //            // 5. Multi user
        //            cmd_941lp.Parameters.Clear();
        //            cmd_941lp.CommandText = $@"ALTER DATABASE [{miBase}] SET MULTI_USER;";
        //            cmd_941lp.ExecuteNonQuery();
        //        }
        //    }
        //}

        public void RestaurarBaseDatos(string rutaBackup)
        {
            const string miBase = "dawRefugio";

            var builder = new SqlConnectionStringBuilder(conn)
            {
                InitialCatalog = "master"
            };
            string connectionStringMaster_941lp = builder.ConnectionString;

            using (SqlConnection connection_941lp = new SqlConnection(connectionStringMaster_941lp))
            {
                connection_941lp.Open();

                // 1. Verificar metadata del backup
                using (SqlCommand checkCmd = new SqlCommand("RESTORE HEADERONLY FROM DISK = @ruta", connection_941lp))
                {
                    checkCmd.Parameters.AddWithValue("@ruta", rutaBackup);

                    using (SqlDataReader reader = checkCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string dbName = reader["DatabaseName"].ToString();

                            if (!string.Equals(dbName, miBase, StringComparison.OrdinalIgnoreCase))
                            {
                                throw new InvalidOperationException(
                                    $"El backup corresponde a la base '{dbName}', pero solo se permite restaurar '{miBase}'.");
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("No se pudo leer la cabecera del backup.");
                        }
                    }
                }

                using (SqlCommand cmd_941lp = connection_941lp.CreateCommand())
                {
                    cmd_941lp.CommandTimeout = 300; // 5 minutos, evita timeout en restores grandes

                    // 2. Matar conexiones abiertas
                    cmd_941lp.CommandText = $@"
                        DECLARE @kill varchar(8000) = '';
                        SELECT @kill = @kill + 'KILL ' + CONVERT(varchar(5), session_id) + ';'
                        FROM sys.dm_exec_sessions
                        WHERE database_id = DB_ID('{miBase}') AND session_id <> @@SPID;
                        EXEC(@kill);";
                    cmd_941lp.ExecuteNonQuery();

                    // 3. Single user
                    cmd_941lp.CommandText = $@"ALTER DATABASE [{miBase}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                    cmd_941lp.ExecuteNonQuery();

                    try
                    {
                        // 4. Restaurar
                        cmd_941lp.CommandText = $@"RESTORE DATABASE [{miBase}] 
                                   FROM DISK = @ruta 
                                   WITH REPLACE;";
                        cmd_941lp.Parameters.AddWithValue("@ruta", rutaBackup);
                        cmd_941lp.ExecuteNonQuery();
                    }
                    finally
                    {
                        // 5. Multi user -- se ejecuta SIEMPRE, incluso si el restore falló
                        cmd_941lp.Parameters.Clear();
                        cmd_941lp.CommandText = $@"ALTER DATABASE [{miBase}] SET MULTI_USER;";
                        cmd_941lp.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
