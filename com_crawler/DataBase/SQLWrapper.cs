// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Setting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace com_crawler.DataBase
{
    public abstract class SQLWrapper
    {
        public abstract void CreateDatabase(string name);
        public abstract void CreateTable<T>(string name);
    }

    public class MySQLWrapper : SQLWrapper
    {
        public override void CreateDatabase(string dbname)
        {
            //using (var conn = new MySqlConnection(Settings.Instance.Model.DataBaseSettings.ConnectionString))
            //using (var cmd = conn.CreateCommand())
            //{
            //    conn.Open();
            //    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{dbname}`;";
            //    cmd.ExecuteNonQuery();
            //}
        }

        public override void CreateTable<T>(string dbname)
        {
            //using (var conn = new MySqlConnection(Settings.Instance.Model.DataBaseSettings.ConnectionString))
            //using (var cmd = conn.CreateCommand())
            //{
            //    conn.Open();
            //}
        }
    }
}
