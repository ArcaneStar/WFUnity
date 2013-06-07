using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WarframeUnity.UI
{
    /// <summary>
    /// Interaction logic for MainNotificationWindow.xaml
    /// </summary>
    public partial class MainNotificationWindow : Window
    {
        #region Fields
        DispatcherTimer dispatcherTimer;
        #endregion

        #region Properties
        public Alerts alerts { get; set; }
        #endregion

        public MainNotificationWindow()
        {
            InitializeComponent();
            alerts = new Alerts(taskbarIcon);
            this.alertList.objectList.ItemsSource = this.alerts.List;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            if (WarframeUnity.Properties.Settings.Default.ShowAll)
            {
                ProgramOptions.ShowAll = true;
            }
            else
            {
                ProgramOptions.ShowAll = false;
            }
            ProgramOptions.MinCredits = Int32.Parse(WarframeUnity.Properties.Settings.Default.MinCredits);
            ProgramOptions.Sound = GetSound(WarframeUnity.Properties.Settings.Default.Sound);
            ProgramOptions.PlaySound = WarframeUnity.Properties.Settings.Default.PlaySound;

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < alerts.List.Count; i++)
            {
                Alert alert = alerts.List[i];
                try
                {
                    if (!ProgramOptions.ShowAll && ProgramOptions.MinCredits > Int32.Parse(alert.Credits) && alert.Reward != null)
                    {
                        alert.Show = false;
                    }
                    else
                    {
                        alert.Show = true;
                    }
                    DateTime Expires = alert.Expires;
                    DateTime Now = DateTime.Now;
                    TimeSpan Remaining = Expires - Now;
                    alert.Remaining = Remaining;
                    if (Remaining.Minutes < 1)
                    {
                        alert.HasExpired = true;
                        alerts.List.Remove(alert);
                    }
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.StackTrace);
                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            new Options().ShowDialog();
        }

        private Stream GetSound(int index)
        {
            if (index == 0)
            {
                return Properties.Resources.Ringtone_Alarm;
            }
            else if (index == 1)
            {
                return Properties.Resources.Ringtone_CipherFail;
            }
            else if (index == 2)
            {
                return Properties.Resources.TextMessage_EnergyPickup;
            }
            else if (index == 3)
            {
                return Properties.Resources.TextMessage_ShipPagerDing;
            }
            else if (index == 4)
            {
                return Properties.Resources.TextMessage_SingleDrumHit;
            }
            return null;
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            new Credits().ShowDialog();
        }
    }
}
