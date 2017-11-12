//This is a source code or part of OpenAccount project
//Copyright (C) 2013 OpenAccount
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//GNU General Public License for more details.
//You should have received a copy of the GNU General Public License
//along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Entity
{
    public class CategoryInfo : INotifyPropertyChanged
    {

        private string MainModule = "Overall Statistics";
        private DateTime FromDate = DateTime.Today;
        private DateTime ToDate = DateTime.Today;
        private string Type = "Day";
        private string SortBy = "Amount";
        private string Catagory = "Overall Statistics";
        private string SubCatagory = "Overall Statistics";
        private string CheckedValue = string.Empty;


        public string[] SplitValue { get; set; }

        public string ModuleName
        {
            get { return this.MainModule; }
            set
            {
                if (this.MainModule != value)
                {
                    this.MainModule = value;
                    NotifyPropertyChanged("ModuleName");
                }

            }
        }
        public DateTime DateFrom
        {
            get { return this.FromDate; }
            set
            {
                if (this.FromDate != value)
                {
                    this.FromDate = value;
                    NotifyPropertyChanged("Dates");
                }
            }
        }
        public DateTime DateTo
        {
            get { return this.ToDate; }
            set
            {
                if (this.ToDate != value)
                {
                    this.ToDate = value;
                    NotifyPropertyChanged("Dates");
                }
            }
        }
        public string TypeName
        {
            get { return this.Type; }
            set
            {
               // if (this.Type != value)
                {
                    this.Type = value;
                    NotifyPropertyChanged("TypeName");
                }

            }
        }
        public string SortByName
        {
            get { return this.SortBy; }
            set
            {
                if (this.SortBy != value)
                {
                    this.SortBy = value;
                    NotifyPropertyChanged("SortByName");
                }
            }
        }
        public string CatagoryName
        {
            get { return this.Catagory; }
            set
            {
                if (this.Catagory != value)
                {
                    this.Catagory = value;
                    NotifyPropertyChanged("CatagoryName");
                }
            }
        }
        public string SubCatagoryName
        {
            get { return this.SubCatagory; }
            set
            {
                if (this.SubCatagory != value)
                {
                    this.SubCatagory = value;
                    NotifyPropertyChanged("SubCatagoryName");
                }
            }
        }
        public string CheckedValueName
        {
            get { return this.CheckedValue; }
            set
            {
                this.CheckedValue = value;
                NotifyPropertyChanged("CheckedValueName");
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
