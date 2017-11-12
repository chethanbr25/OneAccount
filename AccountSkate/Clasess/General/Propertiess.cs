using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AccountSkate
{
    public class Propertiess : INotifyPropertyChanged
    {

        private bool LogIn = false;

        public bool login
        {
            get { return this.LogIn; }
            set
            {
                if (value == true)
                {
                    this.LogIn = value;
                    NotifyPropertyChanged("logIn_True");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
