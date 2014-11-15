using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobFinder.Web.Models
{
    public class MessageViewModel
    {
        public MessageType Type { get; set; }

        public string Text { get; set; }
    }

    public enum MessageType
    {
        Success,
        Error,
        Warning
    }
}