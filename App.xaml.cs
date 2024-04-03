using System.Configuration;
using System.Data;
using System.Drawing;
using System.Media;
using System.Windows;
using Windows.Foundation.Collections;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;


namespace knoknok;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private Mutex instanceMutex;
    protected override void OnStartup(StartupEventArgs e)
    {
        bool createdNew = false;

        instanceMutex = new Mutex(true, "knoknok app instance", out createdNew);

        if (!createdNew)
        {
            instanceMutex = null;
            MessageBox.Show("Instance of knoknok already active");
            Environment.Exit(0);
        }
        else
        {
            Info.Start();
        }
    }
    public App()
    {
        // Listen to notification activation
        ToastNotificationManagerCompat.OnActivated += toastArgs =>
        {
            // Obtain the arguments from the notification
            ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

            // Obtain any user input (text boxes, menu selections) from the notification
            ValueSet userInput = toastArgs.UserInput;

            // Need to dispatch to UI thread if performing UI operations
            Application.Current.Dispatcher.Invoke(delegate
            {
                //Listen for the reply button
                if (args["action"] == "reply")
                {
                    SoundPlayer player = new("Audio/confirm.wav");
                    player.Play();

                    var client = new UdpClient();
                    IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 11000); // endpoint where server is listening
                    client.Connect(ep);

                    Debug.WriteLine("Sending acknowledgment");
                    // send data
                    client.Send(new byte[] { 1, Info.ID }, 2);
                }
            });
        };
    }


    protected override void OnExit(ExitEventArgs e)
    {
        if (instanceMutex != null)
            instanceMutex.ReleaseMutex();

        base.OnExit(e);
    }

    void AppExit(object sender, ExitEventArgs e)
    {
        ToastNotificationManagerCompat.History.Clear();
        ToastNotificationManagerCompat.Uninstall();
    }
}