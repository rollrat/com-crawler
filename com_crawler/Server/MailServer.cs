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
                //Console.WriteLine(message.ToString());

                return Task.FromResult(SmtpResponse.Ok);
            }
        }

        public class MF : IMailboxFilter, IMailboxFilterFactory
        {
            public Task<MailboxFilterResult> CanAcceptFromAsync(ISessionContext context, IMailbox @from, int size = 0, CancellationToken token)
            {
                if (String.Equals(@from.Host, "test.com"))
                {
                    return Task.FromResult(MailboxFilterResult.Yes);
                }

                return Task.FromResult(MailboxFilterResult.NoPermanently);
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
                //Console.WriteLine("User={0} Password={1}", user, password);

                return Task.FromResult(user.Length > 4);
            }

            public IUserAuthenticator CreateInstance(ISessionContext context)
            {
                return new UA();
            }
        }
    }
}
