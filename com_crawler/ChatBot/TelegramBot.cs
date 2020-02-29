// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Setting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace com_crawler.ChatBot
{
    public class TelegramBot : BotModel
    {
        TelegramBotClient bot;

        public TelegramBot()
        {
            bot = new TelegramBotClient(Settings.Instance.Model.BotSettings.TelegramBotAccessToken);

            bot.OnMessage += Bot_OnMessage;
        }

        public override Task SendMessage(BotUserIdentifier user, string message)
        {
            return bot.SendTextMessageAsync((user as TelegramBotIdentifier).user, message);
        }

        public override void Start()
        {
            bot.StartReceiving();
        }

        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message == null || e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text) return;
            await BotAPI.ProcessMessage(this, new TelegramBotIdentifier(e.Message.Chat.Id), e.Message.Text);
        }
    }

    public class TelegramBotIdentifier : BotUserIdentifier
    {
        public long user { get; private set; }

        public TelegramBotIdentifier(long user)
        {
            this.user = user;
        }

        public override bool Equals(BotUserIdentifier other)
        {
            if (!(other is TelegramBotIdentifier))
                return false;
            return user == (other as TelegramBotIdentifier).user;
        }
    }

}
