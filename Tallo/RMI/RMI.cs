using System;
using System.Collections;
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

    public ChatTab(String title)
    {
        this.tabPage = new TabPage(title);
        this.textBox = new RichTextBox();
        this.textBox.Multiline = true;
        this.textBox.ReadOnly = true;
        this.tabPage.Controls.Add(textBox);
        this.textBox.Dock = DockStyle.Fill;
    }

    public void AddReceiverText(String msg)
    {
        textBox.SelectionAlignment = HorizontalAlignment.Left;
        textBox.AppendText(msg + Environment.NewLine);
    }

    public void AddSenderText(String msg)
    {
        textBox.SelectionAlignment = HorizontalAlignment.Right;
        textBox.AppendText(msg + Environment.NewLine);
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