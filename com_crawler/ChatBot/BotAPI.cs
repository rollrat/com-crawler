// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Setting;
using com_crawler.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com_crawler.ChatBot
{
    public class BotAPI
    {
        static readonly List<BotUserIdentifier> hh = new List<BotUserIdentifier>();

        public static Task ProcessMessage(BotModel bot, BotUserIdentifier user, string msg)
        {
            return Task.Run(async () =>
            {
                try
                {
                    var command = msg.Split(' ')[0];

                    Log.Logs.Instance.Push("[Bot] Received Message - " + msg + "\r\n" + Log.Logs.SerializeObject(user));

                    switch (command.ToLower())
                    {
                        case "/rap": // request access permission

                            if (hh.Contains(user))
                            {
                                await bot.SendMessage(user, "Already verified.");
                                break;
                            }

                            var rp = msg.Split(' ')[1];
                            if (rp == Settings.Instance.Model.BotSettings.AccessIdentifierMessage)
                            {
                                hh.Add(user);
                                await bot.SendMessage(user, "The new identity has been added to the list.");
                                break;
                            }

                            await bot.SendMessage(user, "It does not match the identification message.\r\nPlease try again.");
                            break;

                        case "/time": // request server time

                            await bot.SendMessage(user, DateTime.Now.ToString());
                            break;

                        case "/help":

                            var builder = new StringBuilder();
                            builder.Append("API for Custom Crawler Internal Server\r\n");
                            builder.Append("\r\n");
                            builder.Append("/rap <msg> => Request access permission\r\n");
                            builder.Append("/time => Request server time.\r\n");
                            await bot.SendMessage(user, builder.ToString());
                            break;

                        default:
                            await bot.SendMessage(user, $"'{command}' is not proper command!\r\nEnter '/help' to get more informations.");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log.Logs.Instance.PushError("[Bot] " + e.Message + "\r\n" + e.StackTrace);
                    await bot.SendMessage(user, "Internal server error!\r\nIf the error persists, please report to rollrat.cse@gmail.com.");
                }

            });
        }
    }
}
