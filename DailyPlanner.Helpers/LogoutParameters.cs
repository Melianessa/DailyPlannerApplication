using System;
using System.Collections.Generic;
using System.Text;

namespace DailyPlanner.Helpers
{
    public class LogoutParameters
    {
        public string Id_token_hint { get; set; }
        public string Post_logout_redirect_uri { get; set; }
        public string State { get; set; }
    }
}
