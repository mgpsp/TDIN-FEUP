using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public enum Operation { Add, Remove, GroupChat };
public delegate void AlterDelegate(Operation op, String username);

[Serializable]
public class Message
{
    public String sender;
    public String text;
    public String tabName;

    public Message(String sender, String text, String tabName)
    {
        this.sender = sender;
        this.text = text;
        this.tabName = tabName;
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
        textBox.AppendText("Conversation request sent." + Environment.NewLine);
    }

    public void RequestAccepted(String username)
    {
        textBox.Invoke((MethodInvoker)delegate () {
            textBox.SelectionAlignment = HorizontalAlignment.Center;
            textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
            textBox.AppendText(username + " accepted your request." + Environment.NewLine + Environment.NewLine);
        });
    }

    public void RequestRefused(String username)
    {
        textBox.Invoke((MethodInvoker)delegate () {
            textBox.SelectionAlignment = HorizontalAlignment.Center;
            textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
            textBox.AppendText(username + " refused your request." + Environment.NewLine);
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

    public void AddGroupChatUser(String username)
    {
        textBox.SelectionAlignment = HorizontalAlignment.Center;
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
        textBox.AppendText(username + " has joined the conversation." + Environment.NewLine);
    }

    public void RemoveGroupChatUser(String username)
    {
        textBox.SelectionAlignment = HorizontalAlignment.Center;
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
        textBox.AppendText(username + " has left the conversation." + Environment.NewLine);
    }

    public void JoinGroupChat(List<String> users)
    {
        textBox.SelectionAlignment = HorizontalAlignment.Center;
        textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
        if (users.Count > 0)
        {
            textBox.AppendText("You joined \"" + title + "\" along with ");
            for (int i = 0; i < users.Count - 2; i++)
            {
                textBox.AppendText(users[i] + ", ");
            }
            if (users.Count > 1)
                textBox.AppendText(users[users.Count - 2] + " and " + users[users.Count - 1] + "." + Environment.NewLine);
            else
                textBox.AppendText(users[0] + "." + Environment.NewLine);
        }
        else
            textBox.AppendText("You joined \"" + title + "\". This conversation has no active users." + Environment.NewLine);

    }
}

public interface ISingleServer
{
    event AlterDelegate alterEvent;

    Boolean RegisterAddress(String username, string address);
    Boolean LoginUser(string username, string password);
    Boolean RegisterUser(string username, string password, string firstName, string lastName);
    List<string> getUsers();
    List<string> getGroupChats();
    void Logout(String username);
    void RequestConversation(String sender, String receiver);
    void RequestAccepted(String sender, String receiver);
    Boolean CreateGroupChat(String name);
    void AddUserToGroupChat(String groupChatName, String username);
    void SendGroupChatMessage(String name, Message msg);
    List<String> GetGroupChatUsers(String name);
    void RequestRefused(String sender, String receiver);
    void InviteUserToGroup(String username, String groupChatName);
    Boolean IsUserInGroup(String username, String groupChatName);
}

public interface IClientRem
{
    void ReceiveRequest(String username);
    void ReceiveMessage(Message msg);
    void RequestAccepted(String username, String address);
    void RequestRefused(String username);
    void ReceiveAddress(String username, String address);
    void AddUserToGroupChat(String username, String chatName);
    void RemoveUserFromGroupChat(String username, String chatName);
    void InvitedToGroupChat(String chatName);
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