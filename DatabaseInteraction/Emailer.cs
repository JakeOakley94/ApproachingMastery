using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction
{
    
    public static class Emailer
    {
        public enum Senders
        {
            DoNotReply,
            ContactUs
        }

        public static bool SendEmail(Senders sender, string recipientAddress,string subject, string messageText, bool isHtml)
        {
            try
            {
                string senderAddress = null;
                string senderPassword = null;
                switch(sender)
                {
                    case Senders.DoNotReply:
                        senderAddress = Properties.DBSettings.Default.DoNotReplyAddress;
                        senderPassword = Properties.DBSettings.Default.DoNotReplyPassword;
                        break;
                    default:
                        return false;
                }


                MailMessage mail = new MailMessage(new MailAddress(senderAddress), new MailAddress(recipientAddress));
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(senderAddress, senderPassword);
                client.Host = Properties.DBSettings.Default.EmailHost;
                mail.Subject = subject;
                mail.IsBodyHtml = isHtml;
                mail.Body = messageText;
                client.Send(mail);

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

    }
}
