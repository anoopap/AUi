using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Imap4;
using static System.Net.Imap4.Imap4Client;
using System.IO;
using System.Net.Mail;

namespace AUi
{
    public static class Email
    {
        public static List<Imap4Message> GetAllMessages(string userName, string password, int mailCount = 10, bool unreadOnly = false, bool markasUnread = false, string mailFolder = "Inbox", string hostname = "imap.gmail.com",int portnumber = 993)
        {
            List<Imap4Message> mailList = new List<Imap4Message>();           
            var client = new Imap4Client();
            client.Connect(hostname, portnumber, true);
            client.SendAuthUserPass(userName, password, AuthTypes.Plain);
            client.SelectFolder(mailFolder);
            var emails = client.UnreadEmails;
            int count = Convert.ToInt32(client.GetEmailCount());
            int initial = count - mailCount - 1;
            if (unreadOnly)
            {
                for (uint i = (uint)count; i >= 0; i--)
                {
                    mailList.Add(client.GetEmail(i));
                    if (markasUnread)
                    {
                        client.MarkAsRead(i);
                    }
                    if (mailList.Count() == mailCount)
                    {
                        break;
                    }
                }
            }
            else
            {
                for (uint i = (uint)count; i >= initial; i--)
                {
                    mailList.Add(client.GetEmail(i));
                    if (markasUnread)
                    {
                        client.MarkAsRead(i);
                    }
                }
            }
            
            client.Disconnect();
            return mailList;
        }

        public static List<string> SaveAttachements(Imap4Message message, string filePath)
        {
            List<string> listofAttachements = new List<string>();
            foreach (var attachment in message.Attachments)
            {
                var fileName = attachment.Name;
                if (string.IsNullOrEmpty(fileName) & (attachment.Type == "application/vnd.ms-excel"))
                {
                    fileName = RandomName() + ".xlsx";
                }
                File.WriteAllBytes(filePath + "\\" + fileName, attachment.Data);
                listofAttachements.Add(filePath + "\\" + fileName);
            }
            return listofAttachements;
        }

        public static bool SendEmail(string userName, string password, string from, string to, string subject = "", string body = "", bool isBodyHTML = false, string cc = null, string[] attachments = null, string server = "smtp.gmail.com", int port = 25)
        {
            bool status = false;
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(server);
            mail.From = new MailAddress(from);
            if (to.Contains(","))
            {
                foreach (var sto in to.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.To.Add(sto);
                }
            }
            else
            {
                mail.To.Add(to);
            }
            if (!String.IsNullOrEmpty(cc))
            {
                if (cc.Contains(";"))
                {
                    foreach (var sto in cc.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mail.To.Add(sto);
                    }
                }
                else
                {
                    mail.CC.Add(cc);
                }
            }

            mail.Subject = subject;
            if (isBodyHTML)
            {
                mail.IsBodyHtml = true;
            }
            mail.Body = body;
            if (attachments != null)
            {
                foreach (var att in attachments)
                {
                    Attachment attachment = new Attachment(att);
                    mail.Attachments.Add(attachment);
                }
            }
            SmtpServer.Port = port;
            SmtpServer.Credentials = new System.Net.NetworkCredential(userName, password);
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
            return status;
        }

        private static string RandomName()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
    }
}
