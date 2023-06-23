using ACGAuthentication;

public class DateMessageRequestEvent : IEvent
{

    public string date;
    public string accessToken;


    public DateMessageRequestEvent(string date, string accessToken)
    {


        this.date = date;
        this.accessToken = accessToken;

    }

}