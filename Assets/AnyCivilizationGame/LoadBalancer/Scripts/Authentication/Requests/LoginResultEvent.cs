using ACGAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AnyCivilizationGame.LoadBalancer.Scripts.Authentication.Requests
{
    public class LoginResultEvent : IEvent
    {
        public bool IsSuccess { get; set; }
        public LoginResultEvent(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }

}
