// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Setting;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace com_crawler.DataBase
{
    public abstract class SQLWrapper
    {
        public abstract bool CreateTable<T>(string name);
    }

    public class MySQLWrapper : SQLWrapper
    {
        public static MySqlConnection CreateConnection()
            => new MySqlConnection($"Server={Settings.Instance.Model.DataBaseSettings.EndPoint};Database={Settings.Instance.Model.DataBaseSettings.DataBaseName};Uid={Settings.Instance.Model.DataBaseSettings.UserName};Pwd={Settings.Instance.Model.DataBaseSettings.Password}");

        private bool execute_nq(string query)
        {
            var conn = CreateConnection();
            conn.Open();
            var nq = new MySqlCommand(query, conn);
            var r = nq.ExecuteNonQuery();
            conn.Close();
            return r == 1;
        }

        public override bool CreateTable<T>(string name)
        {
            return execute_nq($@"
                CREATE TABLE `{name}` (
                id INT NOT NULL, test VARCHAR(15) NOT NULL,
                PRIMARY KEY (id)) COLLATE='utf8_general_ci';");
        }

        public void Insert<T>(string table, T data)
        {
            var conn = CreateConnection();
            conn.Query<T>($"SELECT * FROM {table};");
        }
    }
}
