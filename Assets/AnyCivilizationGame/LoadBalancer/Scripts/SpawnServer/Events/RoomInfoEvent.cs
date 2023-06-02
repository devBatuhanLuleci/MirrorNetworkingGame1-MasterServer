using ACGAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class RoomInfoEvent : IEvent
{
    public string[] teamA { get; set; } 
    public string[] teamB { get; set; }
}

