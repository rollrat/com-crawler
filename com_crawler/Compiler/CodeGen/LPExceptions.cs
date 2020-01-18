// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler.Compiler.CodeGen
{
    public class LPExceptions : Exception
    {
    }

    public class LPOperatorNotFoundException : LPExceptions
    {
        public LPOperator Operator { get; private set; }
        public LPOperatorNotFoundException(LPOperator op)
        {
            Operator = op;
        }
    }
}
