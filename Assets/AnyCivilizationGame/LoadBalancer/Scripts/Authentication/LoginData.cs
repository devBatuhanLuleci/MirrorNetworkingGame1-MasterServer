﻿

using ACGAuthentication;

public class LoginData : ILoginData
{
    public int Id { get; set; }
    public string WalletId { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string AccessToken { get; set; }
}



