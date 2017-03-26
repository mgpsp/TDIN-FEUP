using System;
using System.Collections;

public enum Operation { New, Change };
public delegate void AlterDelegate(Operation op, String username);

public interface ISingleServer
{
    event AlterDelegate alterEvent;

    // Register client address
    void RegisterAddress(String username, string address);

    // Get reference to remote object
    void GetReference(Guid guid);

    // Login
    Boolean LoginUser(string username, string password);

    Hashtable getUsers();
}

public interface IClientRem
{
    // Send message to client
    void SendMessage(string message);
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