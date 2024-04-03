using System.Configuration;
using System.Data;
using System.Drawing;
using System.Media;
using System.Windows;
using Windows.Foundation.Collections;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Toolkit.Uwp.Notifications;


namespace knoknok;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private Mutex instanceMutex;
    protected override void OnStartup(StartupEventArgs e)
    {
        //bool createdNew = false;

        //instanceMutex = new Mutex(true, "knoknok app instance", out createdNew);

        //if (!createdNew)
        //{
        //    instanceMutex = null;
        //    MessageBox.Show("Instance of knoknok already active");
        //    Application.Current.Shutdown();
        //    return;
        //}
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