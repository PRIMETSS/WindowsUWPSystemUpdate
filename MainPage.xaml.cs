using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WindowsUWPSystemUpdate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private string state = null;
        public string State { get { return this.state; } set { this.state = value; NotifyPropertyChanged("State"); } }

        public event PropertyChangedEventHandler PropertyChanged;


        public MainPage()
        {
            this.DataContext = this;

            this.InitializeComponent();

            this.State = Windows.System.Update.SystemUpdateManager.State.ToString();
            Windows.System.Update.SystemUpdateManager.StateChanged += SystemUpdateManager_StateChangedAsync;
        }

        private async void SystemUpdateManager_StateChangedAsync(object sender, object e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.State = $"{Windows.System.Update.SystemUpdateManager.State.ToString()} Download: {Windows.System.Update.SystemUpdateManager.DownloadProgress * 100} Install: {Windows.System.Update.SystemUpdateManager.InstallProgress * 100}";
            });

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var items = Windows.System.Update.SystemUpdateManager.GetUpdateItems();
                this.State = $"Items Pending = {items.Count}";
            }
            catch (Exception ex)
            {
                this.State = Windows.System.Update.SystemUpdateManager.ExtendedError.Message;
            }
        }

        private void RebootButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Windows.System.Update.SystemUpdateManager.State == Windows.System.Update.SystemUpdateManagerState.RebootRequired)
                    Windows.System.Update.SystemUpdateManager.RebootToCompleteInstall();
                else
                    Windows.System.ShutdownManager.BeginShutdown(Windows.System.ShutdownKind.Restart, new TimeSpan(0));
            }
            catch
            {
                this.State = Windows.System.Update.SystemUpdateManager.ExtendedError.Message;
            }
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Windows.System.Update.SystemUpdateManager.StartInstall(Windows.System.Update.SystemUpdateStartInstallAction.AllowReboot);
                this.State = Windows.System.Update.SystemUpdateManager.State.ToString();
            }
            catch (Exception ex)
            {
                this.State = Windows.System.Update.SystemUpdateManager.ExtendedError.Message;
            }
        }

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
