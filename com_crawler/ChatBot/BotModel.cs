// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace com_crawler.ChatBot
{
    public abstract class BotModel
    {
        public abstract void Start();
        public abstract Task SendMessage(BotUserIdentifier user, string message);
    }

    public abstract class BotUserIdentifier : IEquatable<BotUserIdentifier>
    {
        public abstract bool Equals(BotUserIdentifier other);

        public static bool operator ==(BotUserIdentifier uid1, BotUserIdentifier uid2)
        {
            return uid1.Equals(uid2);
        }

        public static bool operator !=(BotUserIdentifier uid1, BotUserIdentifier uid2)
        {
            return !uid1.Equals(uid2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            BotUserIdentifier objAsBotUserIdentifier = obj as BotUserIdentifier;
            if (objAsBotUserIdentifier == null) return false;
            else return Equals(objAsBotUserIdentifier);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
    }
}
