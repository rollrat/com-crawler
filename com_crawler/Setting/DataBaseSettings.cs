// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler.Setting
{
    public enum DataBaseServer
    {
        MySQL,
        OracleDataBase,
        MSSQLServer,
        MongoDB,
        SQLite
    }

    public class DataBaseSettings
    {
        public DataBaseServer DataBaseServer;

        public string ConnectionString;
    }
}
