// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Setting;
using com_crawler.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace com_crawler.ChatBot
{
    public class BotManager : ILazy<BotManager>
    {
        List<BotModel> bots = new List<BotModel>();

        public void StartBots()
        {
            if (Settings.Instance.Model.BotSettings.EnableTelegramBot)
                bots.Add(new TelegramBot());

            bots.ForEach(x => x.Start());
        }
    }
}
