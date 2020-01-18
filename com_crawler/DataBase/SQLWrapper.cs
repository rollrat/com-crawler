// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler.DataBase
{
    public abstract class SQLWrapper
    {
        public static SQLWrapper Instance;

        protected SQLWrapper(string connection_string) { }
        public abstract int EvalCount(string table, string attr);
        public abstract void EvalNqSql(string sql);
        public abstract void EvalReadSql<T>(string sql, Action<DbDataReader> data_reader);
    }

    public abstract class IDataBase<T>
    {
        public abstract void Insert(T cxt);
        public abstract void Update(T cxt);
        public abstract void Remove(T cxt);
        public abstract List<T> Query(string where = "");
    }
}
