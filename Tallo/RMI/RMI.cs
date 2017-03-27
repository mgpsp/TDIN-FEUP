using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

public enum Operation { Add, Remove };
public delegate void AlterDelegate(Operation op, String username, String address);

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

public class ChatTab
{
    public TabPage tabPage;
    public RichTextBox textBox;
    public Boolean offline;
    String title;

    public ChatTab(String title)
    {
        this.tabPage = new TabPage(title);
        this.title = title;
        this.textBox = new RichTextBox();
        this.textBox.Multiline = true;
        this.textBox.ReadOnly = true;
        this.tabPage.Controls.Add(textBox);
        this.textBox.Dock = DockStyle.Fill;
        this.offline = false;
    }

    public void AddReceiverText(String msg, String username)
    {
        textBox.SelectionAlignment = HorizontalAlignment.Left;
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Bold);
        textBox.AppendText("[" + username + "]: ");
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Regular);
        textBox.AppendText(msg + Environment.NewLine);
    }

    public void AddSenderText(String msg, String username)
    {
        textBox.SelectionAlignment = HorizontalAlignment.Right;
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Bold);
        textBox.AppendText("[" + username + "]: ");
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Regular);
        textBox.AppendText(msg + Environment.NewLine);
    }

    public void SetOfflineMsg(String username)
    {
        this.offline = true;
        textBox.SelectionAlignment = HorizontalAlignment.Center;
        textBox.AppendText(username + " has disconnected.");
    }

    public void NewMessages()
    {
        tabPage.Text = "**" + title + "**";
    }

    public void MessagesRead()
    {
        tabPage.Text = title;
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
    void Logout(String username, String address);
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

    public void Repeater(Operation op, String username, String address)
    {
        if (alterEvent != null)
            alterEvent(op, username, address);
    }
}