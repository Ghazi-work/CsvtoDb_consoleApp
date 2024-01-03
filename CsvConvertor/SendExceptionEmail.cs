using System.Net.Mail;
using System.Net;
using System;

namespace CsvConvertor
{
    internal class SendExceptionEmail
    {
        public void SendEmail(string subject,string body)
        {

            var fromAddress = new MailAddress("ghazi.awesometech@gmail.com", "Ghazi Ali");

            var toAddress = new MailAddress("gzaidi69@gmail.com", "Ali");

            const string fromPassword = "jraexrqznrtkmmkz";
            
            

            var smtp = new SmtpClient

            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)

            {
                Subject = subject,

                Body = body
            })

            {
                smtp.Send(message);
                Console.WriteLine("\nEmail sent successfully.");
            }
        }



    }
}
