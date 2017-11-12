using System;
using System.Collections.Generic;
using System.Text;

namespace Open_Miracle
{
    class FormInfo
    {
        private int _formId;
        private string _formName;
        public int FormId
        {
            set
            {
                _formId = value;
            }
            get
            {
                return _formId;
            }
        }
        public string FormName
        {
            set
            {
                _formName = value;
            }
            get
            {
                return _formName;
            }
        }
    }
}
