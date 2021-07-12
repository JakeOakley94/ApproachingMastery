using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.Models
{
    public partial class UserLogin
    {
        internal static class Constants
        {
            internal const string EMAIL_PARAM = "@strEmailAddress",
                                  PASSWORD_PARAM = "@strPassword",
                                  IP_ADDRESS_PARAM = "@strIPAddress",
                                  EMAIL_CONFIRMATION_PARAM = "@guidEmailValidationID",
                                  LOGIN_ID_PARAM = "@guidLoginID",
                                  FIRST_NAME_PARAM = "@strFirstName",
                                  MIDDLE_NAME_PARAM = "@strMiddleName",
                                  LAST_NAME_PARAM = "@strLastName",
                                  PHONE_NUMBER_PARAM = "@strPhoneNumber";

            internal const int EMAIL_FIELD_SIZE = 255,
                               PASSWORD_FIELD_SIZE = 50,
                               IP_ADDRESS_FIELD_SIZE = 50;

        }
    }
}
