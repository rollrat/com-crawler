// This source code is a part of Community Crawler Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using com_crawler.Utils;
using SmtpServer;
using SmtpServer.Authentication;
using SmtpServer.Mail;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace com_crawler.Server
{
    public class MailServer : ILazy<MailServer>
    {
        public async void StartServer()
        {
            var options = new SmtpServerOptionsBuilder()
                .ServerName("localhost")
                .Port(25, 587)
                .MessageStore(new MS())
                .MailboxFilter(new MF())
                .UserAuthenticator(new UA())
                .Build();

            var smtpServer = new SmtpServer.SmtpServer(options);
            await smtpServer.StartAsync(CancellationToken.None);
        }

        public class MS : MessageStore
        {
            public override Task<SmtpResponse> SaveAsync(ISessionContext context, IMessageTransaction transaction, CancellationToken cancellationToken)
            {
                var textMessage = (ITextMessage)transaction.Message;

                var message = MimeKit.MimeMessage.Load(textMessage.Content);

                var save_path = Path.Combine(AppProvider.ApplicationPath, "mailbox", $"{DateTime.Now}-{message.Subject.ToBase64()}.mime");
                File.WriteAllText(save_path, message.ToString());

                Log.Logs.Instance.Push($"[Mail Server] Mail received. from='{message.From}', to='{message.To}', title='{message.Subject}'");

                return Task.FromResult(SmtpResponse.Ok);
            }
        }

        public class MF : IMailboxFilter, IMailboxFilterFactory
        {
            public Task<MailboxFilterResult> CanAcceptFromAsync(ISessionContext context, IMailbox @from, int size, CancellationToken token)
            {
                return Task.FromResult(MailboxFilterResult.Yes);
                
                //if (String.Equals(@from.Host, "test.com"))
                //{
                //    return Task.FromResult(MailboxFilterResult.Yes);
                //}
                //
                //return Task.FromResult(MailboxFilterResult.NoPermanently);
            }

            public Task<MailboxFilterResult> CanDeliverToAsync(ISessionContext context, IMailbox to, IMailbox @from, CancellationToken token)
            {
                return Task.FromResult(MailboxFilterResult.Yes);
            }

            public IMailboxFilter CreateInstance(ISessionContext context)
            {
                return new MF();
            }
        }

        public class UA : IUserAuthenticator, IUserAuthenticatorFactory
        {
            public Task<bool> AuthenticateAsync(ISessionContext context, string user, string password, CancellationToken token)
            {
                Log.Logs.Instance.Push($"[Mail Server] New authentication request received user='{user}', pwd='{password}'");

                if (Setting.Settings.Instance.Model.ServerSettings.AuthMailUser == user &&
                    Setting.Settings.Instance.Model.ServerSettings.AuthMailPassword == password)
                    return Task.FromResult(true);

                return Task.FromResult(false);
            }

            public IUserAuthenticator CreateInstance(ISessionContext context)
            {
                return new UA();
            }
        }
    }
}
