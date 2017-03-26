using System;
using System.Collections;

public enum Operation { Add, Remove };
public delegate void AlterDelegate(Operation op, String username);

[Serializable]
public class Message
{
    public String sender;
    public String text;

    public Message(String sender, String text)
    {
        this.sender = sender;
        this.text = text;
    }
}

public interface ISingleServer
{
    event AlterDelegate alterEvent;

    // Register client address
    void RegisterAddress(String username, string address);
    // Get reference to remote object
    void GetReference(String username);
    Boolean LoginUser(string username, string password);
    Hashtable getUsers();
    void Logout(String username);
}

public interface IClientRem
{
    void SendReference(String address, String username);
    void SendMessage(Message msg);
}

public class AlterEventRepeater : MarshalByRefObject
{
    public event AlterDelegate alterEvent;

    public override object InitializeLifetimeService()
    {
        return null;
    }

    public void Repeater(Operation op, String username)
    {
        if (alterEvent != null)
            alterEvent(op, username);
    }
}