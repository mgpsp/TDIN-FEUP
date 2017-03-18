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

    static Boolean LoginUser(string username, string password)
    {

        return false;
    }

    static void RegisterUser()
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString;

        myConnectionString = "server=localhost;uid=root;" +
            "pwd=tdin2017;database=tdin;";

        try
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = myConnectionString;
            string sql = " SELECT * FROM user  ";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetString("username"));
                Console.WriteLine(reader.GetString("password"));
            }
            Console.Read();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

public class SingleServer : MarshalByRefObject, ISingleServer
{
    Hashtable table = new Hashtable();

    public void ClientAddress(Guid guid, string address)
    {
        IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), address); // Obtain a reference to the client remote object
        Console.WriteLine("[SingleServer]: Sending active clients list");
        rem.UpdateActiveUsersList(table);
        table.Add(guid, address);
        Console.WriteLine("[SingleServer]: Registered " + address);
    }

    public void GetReference(Guid guid)
    {
        if (table.ContainsKey(guid))
        {
            IClientRem rem = (IClientRem)RemotingServices.Connect(typeof(IClientRem), (string)table[guid]); // Obtain a reference to the client remote object
            Console.WriteLine("[SingleServer]: Obtained the client remote object");
            rem.SendMessage("Server calling Client"  );
        }
    }
}
