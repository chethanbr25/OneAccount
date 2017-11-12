using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace One_Account
{
   public class ErrorMessageInfo : INotifyPropertyChanged
    {
        public string ErrorData = "Error Message !";
        public string ErrorString
        {
            get { return this.ErrorData; }
            set
            {
                if (this.ErrorData != value)
                {
                    this.ErrorData = value;
                    NotifyPropertyChanged(ErrorData);
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
