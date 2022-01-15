using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Agenda.Domain;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace Agenda.DAL.Test
{
    [TestFixture]
    public class BaseTest
    {
        private string _script;
        private string _con;
        private string _catalogTest;
        private IConfiguration Configuration;
        public BaseTest()
        {
            _script = @"Project_Create.sql";
            _con = @"Data Source =.\sqlexpress; Initial Catalog = Agenda; Integrated Security = True;";
            _catalogTest = "AgendaTest";
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CreateDBTest();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            DeleteDbTest();
        }

        private void CreateDBTest()
        {
            using (var con = new SqlConnection(_con))
            {
                con.Open();
                var scriptSql = File
                    .ReadAllText($@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}\{_script}")
                    .Replace("$(DefaultDataPath)", $@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}")
                    .Replace("$(DefaultLogPath)", $@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}")
                    .Replace("$(DefaultFilePrefix)", _catalogTest)
                    .Replace("$(DatabaseName)", _catalogTest)
                    .Replace("WITH (DATA_COMPRESSION - PAGE)", String.Empty)
                    .Replace("SET NOEXEC ON", String.Empty)
                    .Replace("GO\r\n", "|");

                ExecuteScriptSql(con, scriptSql);

            }
        }

        private void ExecuteScriptSql(SqlConnection con, string scriptSql)
        {
            using (var cmd = con.CreateCommand())
            {
                foreach (var sql in scriptSql.Split('|'))
                {
                    cmd.CommandText = sql;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(sql);
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        private void DeleteDbTest()
        {
            using (var con = new SqlConnection(_con))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = $@"   USE [master];
                                            DECLARE @kill varchar(8000) = '';
                                            SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'
                                            FROM sys.dm_exec_sessions
                                            WHERE database_id = db_id('{_catalogTest}')
                                            EXEC(@kill);";

                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"DROP DATABASE {_catalogTest}";
                    cmd.ExecuteNonQuery();
                }

            }
        }
    }
}
