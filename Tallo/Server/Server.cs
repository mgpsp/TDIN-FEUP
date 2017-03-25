using System;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Runtime.Remoting;

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
    MySql.Data.MySqlClient.MySqlConnection conn;
    string myConnectionString = "server=localhost;uid=root;" +
                                "pwd=tdin2017;database=tdin;";

    public void RegisterAddress(Guid guid, string address)
    {
        IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), address); // Obtain a reference to the client remote object
        Console.WriteLine("[SingleServer]: Sending active clients list");
        rem.UpdateActiveUsersList(activeUsers);
        activeUsers.Add(guid, address);
        Console.WriteLine("[SingleServer]: Registered " + address);
    }

    public void GetReference(Guid guid)
    {
        if (activeUsers.ContainsKey(guid))
        {
            IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)activeUsers[guid]); // Obtain a reference to the client remote object
            Console.WriteLine("[SingleServer]: Obtained the client remote object");
            rem.SendMessage("Server calling Client"  );
        }
    }

    public Boolean LoginUser(string username, string password)
    {
        Console.WriteLine(username);
        Console.WriteLine(password);
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

    public Hashtable GetActiveUsers()
    {
        return activeUsers;
    }
    
}
