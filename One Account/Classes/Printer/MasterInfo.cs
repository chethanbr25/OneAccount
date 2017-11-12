using System;
using System.Collections.Generic;
using System.Text;

namespace Open_Miracle
{
    class MasterInfo
    {
        private int _masterId, _pageSize1, _pageSizeOther, _blankLneForFooter, _lineCountBetweenTwo, _formName, _lineCountAfterPrint;
        private string _footerLocation,_condensed, _pitch;
        private bool _isTwoLineForHedder, _isTwoLineForDetails;

        public int MasterId
        {
            set
            {
                _masterId = value;
            }
            get
            {
                return _masterId;
            }
        }
        public int LineCountAfterPrint
        {
            set
            {
                _lineCountAfterPrint = value;
            }
            get
            {
                return _lineCountAfterPrint;
            }
        }
        public int PageSize1
        {
            set
            {
                _pageSize1 = value;
            }
            get
            {
                return _pageSize1;
            }
        }
        public int PageSizeOther
        {
            set
            {
                _pageSizeOther = value;
            }
            get
            {
                return _pageSizeOther;
            }
        }
        public int BlankLneForFooter
        {
            set
            {
                _blankLneForFooter = value;
            }
            get
            {
                return _blankLneForFooter;
            }
        }
        public int LineCountBetweenTwo
        {
            set
            {
                _lineCountBetweenTwo = value;
            }
            get
            {
                return _lineCountBetweenTwo;
            }
        }
        public string FooterLocation
        {
            set
            {
                _footerLocation = value;
            }
            get
            {
                return _footerLocation;
            }
        }
        public string Condensed
        {
            set
            {
                _condensed = value;
            }
            get
            {
                return _condensed;
            }
        }
        public string Pitch
        {
            set
            {
                _pitch = value;
            }
            get
            {
                return _pitch;
            }
        }
        public int FormName
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
        public bool IsTwoLineForHedder
        {
            set
            {
                _isTwoLineForHedder = value;
            }
            get
            {
                return _isTwoLineForHedder;
            }
        }
        public bool IsTwoLineForDetails
        {
            set
            {
                _isTwoLineForDetails = value;
            }
            get
            {
                return _isTwoLineForDetails;
            }
        }
    }
}
