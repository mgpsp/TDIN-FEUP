using System;
using System.Collections;

public interface ISingleServer
{
    // Register client address
    void RegisterAddress(Guid guid, string address);

    // Get reference to remote object
    void GetReference(Guid guid);

    // Login
    Boolean LoginUser(string username, string password);
}

public interface IClientRem
{
    // Send message to client
    void SendMessage(string message);

    // Send active users
    void UpdateActiveUsersList(Hashtable users);
}
