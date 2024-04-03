using System.Windows;
using Microsoft.Toolkit.Uwp.Notifications;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace knoknok;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private byte id;
    private int port = 11000;
    
    public MainWindow()
    {
        InitializeComponent();

        //get id
        Random rnd = new Random();
        id = (byte)rnd.Next(1,255);
        
        //server thread
        Thread serverThread = new Thread(() => Server(port, id));
        serverThread.IsBackground = true;
        serverThread.Start();

    }

    private void Server(int port, byte id)
    {
        UdpClient udpServer = new UdpClient(port);

        Debug.WriteLine("Starting server");
        while (true)
        {
            var remoteEP = new IPEndPoint(IPAddress.Any, 11000); 
            byte[] data = udpServer.Receive(ref remoteEP); // listen on port 11000
            Debug.WriteLine("receive data from " + remoteEP.ToString());

            if (data.Length != 2)
                return;

            if (data[0] == (byte)255 && data[1] != id)
                ReceiveKnock();
        }
    }

    public void KnockButtonClick(object sender, EventArgs e)
    {
        var client = new UdpClient();
        IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 11000); // endpoint where server is listening
        client.Connect(ep);

        // send data
        client.Send(new byte[] { 255, id }, 2);
        Debug.WriteLine("Sending knock");
        SendKnock();
    }
    
    //Close to tray instead
    public void HandleClosing(object sender, CancelEventArgs  e)
    {
        this.Visibility = Visibility.Hidden;
        e.Cancel = true;
    }
    
    private static void ReceiveKnock()
    {
        // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
        new ToastContentBuilder()
            .AddText("Knock Knock!")
            .AddText("I'm trying to get your attention")
            .SetToastScenario(ToastScenario.Alarm)
            .AddAudio(new Uri("ms-winsoundevent:Notification.Looping.Alarm2"), true)
            .AddInlineImage(new Uri(Path.GetFullPath("images/kirby.gif")))
            // Buttons
            .AddButton(new ToastButton()
                .SetContent("Understood!")
                .AddArgument("action", "reply")
                .SetBackgroundActivation())
            .Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater
    }
    
    private static void SendKnock()
    {
        // Requires Microsoft.Toolkit.Uwp.Notifications NuGet package version 7.0 or greater
        new ToastContentBuilder()
            .AddText("Knock Knock!")
            .AddText("You sent a knock")
            .AddAudio(new Uri("ms-winsoundevent:Notification.Reminder"))
            .Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater
    }

    private void CloseApplication(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}