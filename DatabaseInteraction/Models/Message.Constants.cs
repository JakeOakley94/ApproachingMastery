using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.Models
{
    public partial class Message
    {
        public static class Constants
        {
            public const string MESSAGE_ID_COLUMN = "guidMessageID",
                                SENDER_COLUMN = "guidSenderID",
                                RECIPIENT_ID_COLUMN = "guidRecipientID",
                                PARRENT_MESSAGE_COLUMN = "guidParentMessageID",
                                MESSAGE_COLUMN = "strMessage",
                                MESSAGE_DATE_TIME_COLUMN = "dtmMessageDateTime";

            public static readonly string MESSAGE_ID_PARAM = $"@{MESSAGE_ID_COLUMN}",
                                          SENDER_PARAM = $"@{SENDER_COLUMN}",
                                          RECIPIENT_PARAM = $"@{SENDER_COLUMN}",
                                          PARENT_PARAM = $"@{PARRENT_MESSAGE_COLUMN}",
                                          MESSAGE_PARAM = $"@{MESSAGE_COLUMN}";
        }
    }
}
