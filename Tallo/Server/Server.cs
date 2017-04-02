using System;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Runtime.Remoting;
using System.Threading;
using System.Collections.Generic;

class Server
{
    static void Main(string[] args)
    {
        RemotingConfiguration.Configure("Server.exe.config", false);
        Console.WriteLine("[Server]: Press Return to terminate.");
        Console.ReadLine();
    }
}

public class GroupChat
{
    public String name;
    public List<string> users;

    public GroupChat(String name)
    {
        this.name = name;
        users = new List<string>();
    }

    public void AddUser(String username)
    {
        users.Add(username);
    }
}

public class SingleServer : MarshalByRefObject, ISingleServer
{
    Hashtable activeUsers = new Hashtable();
    Hashtable groupChats = new Hashtable();

    public event AlterDelegate alterEvent;
    MySql.Data.MySqlClient.MySqlConnection conn;
    string myConnectionString = "server=localhost;uid=root;" +
                                "pwd=root;database=tdin;";

    public Boolean RegisterAddress(String username, string address)
    {
        if (activeUsers.Contains(username))
            return false;
        else
        {
            IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), address); // Obtain a reference to the client remote object
            Console.WriteLine("[SingleServer]: Sending active clients list");
            activeUsers.Add(username, address);
            Console.WriteLine("[SingleServer]: Registered " + address);
            NotifyClients(Operation.Add, username);
            return true;
        }
    }

    public Boolean LoginUser(string username, string password)
    {
        try
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = myConnectionString;
            string sql = " SELECT * FROM user WHERE username = ?username;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("username", username);
            conn.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                if (reader.GetString("password") == password)
                    return true;
                else
                    return false;
            }
            else
                return false;

        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            conn.Close();
        }
        return false;
    }

    public Boolean RegisterUser(string username, string password, string firstName, string lastName)
    {
        try
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = myConnectionString;
            string sql = " SELECT * FROM user WHERE username = ?username;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("username", username);
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
                return false;
            else
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                sql = "INSERT INTO user(username,password, firstName, lastName) VALUES(?username, ?password, ?firstName, ?lastName)";
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.Parameters.AddWithValue("firstName", firstName);
                cmd.Parameters.AddWithValue("lastName", lastName);
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        finally
        {
            conn.Close();
        }
    }
    
    public List<string> getUsers()
    {
        List<string> usernames = new List<string>();
        foreach (DictionaryEntry elm in activeUsers)
        {
            usernames.Add(elm.Key.ToString());
        }
        return usernames;
    }

    public List<string> getGroupChats()
    {
        List<string> names = new List<string>();
        foreach (DictionaryEntry elm in groupChats)
        {
            names.Add(elm.Key.ToString());
        }
        return names;
    }

    public void Logout(String username)
    {
        activeUsers.Remove(username);
        foreach (GroupChat gc in groupChats.Values)
            if (gc.users.Contains(username))
            {
                gc.users.Remove(username);
                foreach (string user in gc.users)
                {
                    IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[user]);
                    rem.RemoveUserFromGroupChat(username, gc.name);
                }
            }
        NotifyClients(Operation.Remove, username);
    }

    public void RequestConversation(String sender, String receiver)
    {
        IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[receiver]);
        rem.ReceiveRequest(sender);
    }

    public void RequestAccepted(String sender, String receiver)
    {
        IClientRem remSender = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[sender]);
        IClientRem remReceiver = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[receiver]);
        remSender.RequestAccepted(receiver, (string)activeUsers[receiver]);
        remReceiver.ReceiveAddress(sender, (string)activeUsers[sender]);
    }

    public void RequestRefused(String sender, String receiver)
    {
        IClientRem remSender = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[sender]);
        remSender.RequestRefused(receiver);
    }

    void NotifyClients(Operation op, String username)
    {
        if (alterEvent != null)
        {
            Delegate[] invkList = alterEvent.GetInvocationList();

            foreach (AlterDelegate handler in invkList)
            {
                new Thread(() => {
                    try
                    {
                        handler(op, username);
                        Console.WriteLine("Invoking event handler");
                    }
                    catch (Exception)
                    {
                        alterEvent -= handler;
                        Console.WriteLine("Exception: Removed an event handler");
                    }
                }).Start();
            }
        }
    }

    public Boolean CreateGroupChat(String name)
    {
        if (groupChats.Contains(name))
            return false;
        else
        {
            groupChats.Add(name, new GroupChat(name));
            NotifyClients(Operation.GroupChat, name);
            return true;
        }
    }

    public void AddUserToGroupChat(String groupChatName, String username)
    {
        GroupChat gc = (GroupChat)groupChats[groupChatName];
        foreach (string user in gc.users)
        {
            IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[user]);
            rem.AddUserToGroupChat(username, groupChatName);
        }
        gc.users.Add(username);
    }

    public void SendGroupChatMessage(String name, Message msg)
    {
        GroupChat gc = (GroupChat)groupChats[name];
        foreach (string username in gc.users)
        {
            if (username != msg.sender)
            {
                IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[username]);
                rem.ReceiveMessage(msg);
            }
        }
    }

    public List<String> GetGroupChatUsers(String name)
    {
        GroupChat gc = (GroupChat)groupChats[name];
        return gc.users;
    }

    public void InviteUserToGroup(String username, String groupChatName)
    {
        IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[username]);
        rem.InvitedToGroupChat(groupChatName);
    }

    public Boolean IsUserInGroup(String username, String groupChatName)
    {
        GroupChat gc = (GroupChat)groupChats[groupChatName];
        if (gc.users.Contains(username))
            return true;
        else
            return false;
    }
}
