using System;
using System.Collections.Generic;
using System.Data;

namespace DatabaseInteraction.Models
{
    public partial class Message
    {
        #region Public Properties

        public Guid? MessageID { get; set; } = null;
        public Guid SenderID { get; set; }
        public Guid? RecipientID { get; set; } = null;
        public string MessageText { get; set; }
        public DateTime MessageDateTime { get; set; }
        public List<Message> Replies { get; set; }

        #endregion

        #region Constructors

        public Message(Guid messageID, Guid senderID, Guid recipientID, string message, DateTime messageDateTime)
        {
            MessageID = messageID;
            SenderID = SenderID;
            RecipientID = recipientID;
            MessageText = message;
            MessageDateTime = messageDateTime;
        }

        public Message(DataRow dr)
        {

            MessageID = (Guid)dr[Constants.MESSAGE_ID_COLUMN];
            SenderID = (Guid)dr[Constants.SENDER_COLUMN];
            RecipientID = dr[Constants.RECIPIENT_ID_COLUMN]==DBNull.Value?null:(Guid?)dr[Constants.RECIPIENT_ID_COLUMN];
            string message = dr[Constants.MESSAGE_COLUMN].ToString();
            MessageDateTime = (DateTime)dr[Constants.MESSAGE_DATE_TIME_COLUMN];
        }

        public Message(string message, Guid senderID)
        {
            MessageText = message;
            SenderID = senderID;
        }

        #endregion

        public bool GetReplies()
        {
            try
            {
                List<Message> replies = new List<Message>();
                if (!Database.GetMessageReplies(MessageID, out replies))
                {
                    return false;
                }
                Replies = replies;
                foreach (Message reply in Replies)
                {
                    reply.GetReplies();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Replies = null;
                return false;
            }
        }


    }
}