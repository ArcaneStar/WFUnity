using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarframeUnity 
{
    public class Alert : INotifyPropertyChanged
    {
        #region Fields
        private TimeSpan _remaining;
        private bool _hasExpired;
        private bool _show;
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public string Location { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string Credits { get; set; }
        public string Reward {get; set;}
        public string CreditDisplay {get; set;}
        public DateTime Started { get; set; }
        public DateTime Expires { get; set; }
        public bool Show
        {
            get { return _show; }
            set { _show = value; onPropertyChanged(this, "Show"); }
        }
        public TimeSpan Remaining
        {
            get { return _remaining; }
            set { _remaining = value; onPropertyChanged(this, "Remaining"); }
        }
        public bool HasExpired
        {
            get { return _hasExpired; }
            set { _hasExpired = value; onPropertyChanged(this, "HasExpired"); }
        }
        #endregion

        #region Constructors
        public Alert(string location, string title, string duration, string credits, string reward, DateTime started, DateTime expires)
        {
            this.Location = location;
            this.Title = title;
            this.Duration = duration;
            this.Credits = credits;
            this.Reward = Reward;
            this.CreditDisplay = credits + " " + reward;
            this.Started = started;
            this.Expires = expires;
            this.HasExpired = false;
            this.Remaining = new TimeSpan();
            this.Show = true;
        }
        #endregion

        private void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
