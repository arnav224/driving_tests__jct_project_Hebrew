using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

//namespace Tools
//{
//    static public class SendEmail   //@ public accesability is temporary!
//    {
//        static public bool SendingEmail(/*MailAddress recipients*/string recipients, string subject, string body)
//        {
//            try
//            {
//                var client = new SmtpClient("smtp.gmail.com", 587)
//                {
//                    Credentials = new NetworkCredential(Configuration.SenderEmailAddress.Address, Configuration.EmailServerPasword),
//                    EnableSsl = true
//                };
//                MailMessage mailMessage = new MailMessage(recipients, recipients) { Body = body };
//                client.Send(mailMessage);
//                //client.Send(Configuration.SenderEmailAddress, recipients, subject, body);
//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        //static public bool TestReminderToTrainee(BE.Test test, BE.Trainee trainee)    //
//        //{
//        //    try
//        //    {
//        //        SendingEmail(trainee.MailAddress, "מועד הטסט שלך מתקרב", "asdfghjkl");
//        //        return true;
//        //    }
//        //    catch (Exception)
//        //    {
//        //        return false;
//        //    }
//        //}


//    }
//}
