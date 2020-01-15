// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler.Postprocessor
{
    /// <summary>
    /// Postprocessor interface
    /// </summary>
    public abstract class IPostprocessor
    {
        public abstract void Run(NetTask task);
    }
}
