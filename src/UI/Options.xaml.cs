using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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

namespace WarframeUnity.UI
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {
            InitializeComponent();
            Uri iconUri = new Uri("pack://application:,,,/UI/Images/icon1.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);

            if (WarframeUnity.Properties.Settings.Default.ShowAll)
            {
                ShowAll.IsChecked = true;
                ShowOnly.IsChecked = false;
            }
            else
            {
                ShowAll.IsChecked = false;
                ShowOnly.IsChecked = true;
            }
            Credits.Text = WarframeUnity.Properties.Settings.Default.MinCredits;
            PlaySound.IsChecked = WarframeUnity.Properties.Settings.Default.PlaySound;
            SoundSelect.SelectedIndex = WarframeUnity.Properties.Settings.Default.Sound;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ProgramOptions.Sound = GetSound(SoundSelect.SelectedIndex);
            if (ShowAll.IsChecked == true)
            {
                ProgramOptions.ShowAll = true;
            }
            else
            {
                try
                {
                    ProgramOptions.ShowAll = false;
                    int credits = Int32.Parse(Credits.Text);
                    ProgramOptions.MinCredits = credits;
                }
                catch (Exception e2)
                {
                    MessageBox.Show("Credit amount must be a whole number");
                    return;
                }
            }

            if (ShowAll.IsChecked == true)
            {
                WarframeUnity.Properties.Settings.Default.ShowAll = true;
            }
            else
            {
                WarframeUnity.Properties.Settings.Default.ShowAll = false;
            }
            WarframeUnity.Properties.Settings.Default.MinCredits = Credits.Text;
            WarframeUnity.Properties.Settings.Default.PlaySound = PlaySound.IsChecked == true;
            WarframeUnity.Properties.Settings.Default.Sound = SoundSelect.SelectedIndex;
            WarframeUnity.Properties.Settings.Default.Save();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SoundPlayer sp = new SoundPlayer(GetSound(SoundSelect.SelectedIndex));
            sp.Stream.Position = 0; 
            sp.Play();
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

        private void ShowAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Credits.IsEnabled = false;
            }
            catch (Exception e3)
            {
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
