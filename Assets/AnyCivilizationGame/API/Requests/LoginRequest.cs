using Assets.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LoginRequest : IGetRequest
{
    public string Url => $"Auth/Master/GetUser/{accessToken}";

    public string accessToken;
    public LoginRequest(string walletID)
    {
        accessToken = walletID;
    }


}

