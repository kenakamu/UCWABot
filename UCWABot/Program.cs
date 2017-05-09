using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Skype.UCWA;
using Microsoft.Skype.UCWA.Enums;
using Microsoft.Skype.UCWA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;

namespace UCWABot
{
    public class Program
    {
        private string tenant = ConfigurationManager.AppSettings["Tenant"].ToString();
        private string directLineSecret = ConfigurationManager.AppSettings["DirectLineSecret"].ToString();
        private UCWAClient client;
        private DirectLineClient dClient;

        List<UserQueue> queues = new List<UserQueue>();
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Signin();
            Console.WriteLine("Hit Enter to finish.");
            Console.Read();
        }

        public Program()
        {
            dClient = new DirectLineClient(directLineSecret);
            client = new UCWAClient();
            client.SendingRequest += (client, resource) => { client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenService.AquireAADToken(resource)); };

            client.MessageReceived += Client_MessageReceived;
            client.MessagingInvitationReceived += Client_MessagingInvitationReceived;
            client.OnlineMeetingInvitationReceived += Client_OnlineMeetingInvitationReceived;
        }

        private void Signin()
        {
            client.Initialize(tenant).Wait();
            client.SignIn(availability: Availability.Online, supportMessage: true, supportAudio: false, supportPlainText: true, supportHtmlFormat: false, phoneNumber: "", keepAlive: true).Wait();

            #region Initial message

            Console.WriteLine("**************************************************");
            Console.WriteLine("****  UCWA C# SDK Sample Bot application. ****");
            Console.WriteLine("**************************************************");
            Console.WriteLine("");

            #endregion
        }

        private void Client_MessageReceived(Message message)
        {
            var sip = message.GetParticipant().Result.Uri;
            // Check conversationId from queue
            var queue = queues.Where(x => x.Sip == sip).FirstOrDefault();
            if(queue == null)
            {
                var conversation = dClient.Conversations.StartConversation();
                queue = new UserQueue() { Sip = sip, ConversationId = conversation.ConversationId, WaterMark = null };
                queues.Add(queue);
            }

            dClient.Conversations.PostActivity(
                queue.ConversationId, 
                new Activity(
                    type: ActivityTypes.Message, 
                    text: message.Text, 
                    fromProperty: new ChannelAccount(sip, sip)));

            var dResult = dClient.Conversations.GetActivities(queue.ConversationId, queue.WaterMark);
            queue.WaterMark = dResult.Watermark;
            foreach (var activity in dResult.Activities.Where(x =>x.From.Id != sip))
            {
                client.ReplyMessage(activity.Text, message).Wait();
            }
        }

        private void Client_MessagingInvitationReceived(MessagingInvitation messagingInvitation)
        {
            messagingInvitation.Accept().Wait();
        }

        private void Client_OnlineMeetingInvitationReceived(OnlineMeetingInvitation onlineMeetingInvitation)
        {
            onlineMeetingInvitation.Accept().Wait();
        }
        
    }
}
