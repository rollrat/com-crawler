// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace com_crawler.Cache
{
    public class ProgramLock
    {
        public const string Name = "com-crawler.lock";
        public static FileStream LockStream;
        public static bool ProgramCrashed = false;

        /// <summary>
        /// Create Lock File
        /// </summary>
        /// <returns></returns>
        public static bool Lock()
        {
            try
            {
                var full_path = Path.Combine(AppProvider.ApplicationPath, Name);
                if (!File.Exists(full_path))
                    using (File.Create(full_path)) { }
                else
                    ProgramCrashed = true;
                LockStream = new FileStream(full_path, FileMode.Open, FileAccess.Read, FileShare.None);
                return true;
            }
            catch (FileNotFoundException e)
            {
                // Write Permission Error
                Logs.Instance.PushError("[Program Lock] Locking pp-error - " + e.Message + "\r\n" + e.StackTrace);
                return false;
            }
            catch (IOException e)
            {
                Logs.Instance.PushError("[Program Lock] Locking io-error - " + e.Message + "\r\n" + e.StackTrace);
                return false;
            }
            catch (Exception e)
            {
                Logs.Instance.PushError("[Program Lock] Locking unhandled-error - " + e.Message + "\r\n" + e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Delete Lock File
        /// </summary>
        public static void UnLock()
        {
            LockStream.Close();
            var full_path = Path.Combine(Directory.GetCurrentDirectory(), Name);
            File.Delete(full_path);
        }
    }
}
