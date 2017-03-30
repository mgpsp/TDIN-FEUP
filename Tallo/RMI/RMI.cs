using System;
using System.Collections;
using System.Collections.Generic;
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
        this.offline = true;
    }

    public void AddReceiverText(String msg, String username)
    {
        FormatMessage(msg, username, HorizontalAlignment.Left);
    }

    public void AddSenderText(String msg, String username)
    {
        FormatMessage(msg, username, HorizontalAlignment.Right);
    }

    public void SendConversationRequest()
    {
        textBox.SelectionAlignment = HorizontalAlignment.Center;
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
        textBox.AppendText(Environment.NewLine + "Conversation request sent." + Environment.NewLine);
    }

    public void RequestAccepted(String username)
    {
        textBox.Invoke((MethodInvoker)delegate () {
            textBox.SelectionAlignment = HorizontalAlignment.Center;
            textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
            textBox.AppendText(username + " accepted your request." + Environment.NewLine + Environment.NewLine);
        });
    }

    public void SetOfflineMsg(String username)
    {
        this.offline = true;
        textBox.SelectionAlignment = HorizontalAlignment.Center;
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
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

    private void FormatMessage(String msg, String username, HorizontalAlignment alignment)
    {
        textBox.SelectionAlignment = alignment;
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Bold);
        textBox.AppendText("[" + username + "]: ");
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Regular);
        textBox.AppendText(msg + Environment.NewLine);
    }
}

public interface ISingleServer
{
    event AlterDelegate alterEvent;

    void RegisterAddress(String username, string address);
    Boolean LoginUser(string username, string password);
    List<string> getUsers();
    void Logout(String username, String address);
    void RequestConversation(String sender, String receiver);
    void RequestAccepted(String sender, String receiver);
}

public interface IClientRem
{
    void ReceiveRequest(String username);
    void ReceiveMessage(Message msg);
    void RequestAccepted(String username, String address);
    void ReceiveAddress(String username, String address);
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