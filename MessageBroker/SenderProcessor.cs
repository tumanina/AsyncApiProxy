﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MessageBroker
{
    public class SenderProcessor: ISenderProcessor
    {
        private readonly IEnumerable<ISender> _senders;

        public SenderProcessor(IEnumerable<ISender> senders)
        {
            _senders = senders;
        }
        
        public void SendMessage(string type, string message)
        {
            var sender = _senders.FirstOrDefault(t => t.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase));

            if (sender == null)
            {
                throw new Exception($"Sender for type '{type}' not found.");
            }

            sender.SendMessage(message);
        }
    }
}
