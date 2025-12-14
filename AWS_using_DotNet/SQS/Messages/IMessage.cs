using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
    public interface IMessage
    {
        public string MessageTypeName { get; }
    }
}
