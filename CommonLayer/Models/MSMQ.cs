﻿using System;
using System.Net.Mail;
using System.Net;
using Experimental.System.Messaging;

namespace CommonLayer.Models
{
    public class MSMQ
    {
        MessageQueue messageQ = new MessageQueue();

        public void sendData2Queue(string token)
        {
            messageQ.Path = @".\private$\Bills";
            if (!MessageQueue.Exists(messageQ.Path))
            {
                MessageQueue.Create(messageQ.Path);//Exists

            }
            messageQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messageQ.ReceiveCompleted += MessageQ_ReceiveCompleted;
            messageQ.Send(token);
            messageQ.BeginReceive();
            messageQ.Close();
        }

        private void MessageQ_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = messageQ.EndReceive(e.AsyncResult);
                string body = msg.Body.ToString();
                string subject = "FundooNote Reset Link";
                var SMTP = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("sainikhil392@gmail.com", "njbmrjdmoxasqrpc"),
                    EnableSsl = true
                };
                SMTP.Send("sainikhil392@gmail.com", "sainikhil392@gmail.com", subject, body);
                // Process the logic be sending the message
                //Restart the asynchronous receive operation.
                messageQ.BeginReceive();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

