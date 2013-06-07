using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using WarframeUnity.Feed;
using WarframeUnity.UI;

namespace WarframeUnity
{
    public class Alerts
    {
        #region Fields
        private Twitter twitter;
        private ObservableCollection<Alert> viewList;
        private DateTime lastCheck;
        private int refreshInterval;
        private TaskbarIcon taskbarIcon;
        private SoundPlayer sp;
        #endregion

        #region Properties
        public delegate void HandleAlert();
        public event HandleAlert OnAlert;
        public List<Alert> list;
        public ObservableCollection<Alert> List 
        { 
            get { return viewList; } 
            set { viewList = value; } 
        }
        public int RefreshInterval
        {
            get { return refreshInterval; }
            set { refreshInterval = value; }
        }
        #endregion

        #region Constructors
        public Alerts(TaskbarIcon icon)
        {
            twitter = new Twitter();
            list = new List<Alert>();
            //sp = new SoundPlayer(Properties.Resources.AlertSound);
            viewList = new ObservableCollection<Alert>();
            lastCheck = DateTime.MinValue;
            refreshInterval = 10;
            taskbarIcon = icon;
        }
        #endregion

        #region Methods
        public void Update()
        {
            if ((DateTime.Now - lastCheck).TotalSeconds > refreshInterval)
            {
                lastCheck = DateTime.Now;

                List<Tweet> fresh = twitter.FetchTweets();
                if (fresh != null && fresh.Count > 0)
                {
                    foreach (Tweet t in fresh)
                    {
                        Alert newAlert = null;

                        string tweetText = t.Text;
                        Console.WriteLine(t.Text);
                        Match match = Regex.Match(tweetText, @"([A-Za-z\s\-)]+) (\([A-Za-z\s]+\)): ([A-Za-z\t\s\']+) - ([0-9]+)m - ([0-9]+)cr[\t\s]?-?[\s]?([A-Za-z\t\s\(\)]+)?", RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            string location = match.Groups[1].Value + " " + match.Groups[2].Value;
                            string title = match.Groups[3].Value;
                            string duration = match.Groups[4].Value;
                            string credits = match.Groups[5].Value;
                            string reward = match.Groups[6].Value;
                            Console.WriteLine(reward);
                            newAlert = new Alert(location, title, duration, credits, reward, t.Created, GetEndTime(t.Created, duration));
                        }

                        if (newAlert != null)
                        {
                            DateTime Expires = newAlert.Expires;
                            DateTime Now = DateTime.Now;
                            TimeSpan Remaining = Expires - Now;
                            newAlert.Remaining = Remaining;
                            if (!ProgramOptions.ShowAll && ProgramOptions.MinCredits > Int32.Parse(newAlert.Credits) && newAlert.Reward != null)
                            {
                                newAlert.Show = false;
                            }
                            if (Remaining.Minutes > 1 && newAlert.Show)
                            {
                                list.Add(newAlert);
                                App.Current.Dispatcher.BeginInvoke((Action)delegate()
                                {
                                    Balloon balloon = new Balloon();
                                    balloon.DataContext = newAlert;
                                    taskbarIcon.ShowCustomBalloon(balloon, PopupAnimation.Slide, 99999999);
                                    viewList.Add(newAlert);
                                    if (ProgramOptions.PlaySound)
                                    {
                                        sp = new SoundPlayer(ProgramOptions.Sound);
                                        sp.Stream.Position = 0;
                                        sp.Play();
                                    }
                                });
                            }
                        }
                    }

                    if (OnAlert != null)
                        OnAlert.Invoke();
                }
            }
        }

        private DateTime GetEndTime(DateTime Started, string duration)
        {
            int minutes = Int32.Parse(duration);
            return Started.Add(new TimeSpan(0, minutes, 0));
        }
        #endregion
    }
}
