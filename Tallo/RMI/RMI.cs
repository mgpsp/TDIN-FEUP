using System;
using System.Collections;

public interface ISingleServer
{
    // Register client address
    void ClientAddress(Guid guid, string address);

    // Get reference to remote object
    void GetReference(Guid guid);
}

public interface IClientRem
{
    // Send message to client
    void SendMessage(string message);

    // Send active users
    void UpdateActiveUsersList(Hashtable users);
}
