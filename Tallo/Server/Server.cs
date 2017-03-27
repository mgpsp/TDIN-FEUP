using System;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Runtime.Remoting;
using System.Threading;

class Server
{
    static void Main(string[] args)
    {
        RemotingConfiguration.Configure("Server.exe.config", false);
        Console.WriteLine("[Server]: Press Return to terminate.");
        Console.ReadLine();
    }
}

public class SingleServer : MarshalByRefObject, ISingleServer
{
    Hashtable activeUsers = new Hashtable();
    public event AlterDelegate alterEvent;
    MySql.Data.MySqlClient.MySqlConnection conn;
    string myConnectionString = "server=localhost;uid=root;" +
                                "pwd=tdin2017;database=tdin;";

    public void RegisterAddress(String username, string address)
    {
        IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), address); // Obtain a reference to the client remote object
        Console.WriteLine("[SingleServer]: Sending active clients list");
        activeUsers.Add(username, address);
        Console.WriteLine("[SingleServer]: Registered " + address);
        NotifyClients(Operation.Add, username, address);
    }

    public void GetReference(String username)
    {
        if (activeUsers.ContainsKey(username))
        {
            IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[username]); // Obtain a reference to the client remote object
            Console.WriteLine("[SingleServer]: Obtained the client remote object");
            rem.SendReference((string)activeUsers[username], username);
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
            {
                // new user
                RegisterUser(username, password);
                return true;
            }

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

    public void RegisterUser(string username, string password)
    {
        try
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = myConnectionString;
            string sql = "INSERT INTO user(username,password) VALUES(?username, ?password)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }
    
    public Hashtable getUsers()
    {
        return activeUsers;
    }

    public void Logout(String username, String address)
    {
        activeUsers.Remove(username);
        NotifyClients(Operation.Remove, username, address);
    }

    void NotifyClients(Operation op, String username, String address)
    {
        if (alterEvent != null)
        {
            Delegate[] invkList = alterEvent.GetInvocationList();

            foreach (AlterDelegate handler in invkList)
            {
                new Thread(() => {
                    try
                    {
                        handler(op, username, address);
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
}
