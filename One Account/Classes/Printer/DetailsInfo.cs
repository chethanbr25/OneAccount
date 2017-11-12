using System;
using System.Collections.Generic;
using System.Text;

namespace Open_Miracle
{
    class DetailsInfo
    {
        private int _detailsId, _masterId, _row, _columns, _width, _wrapLineCount;
        private string _name, _text, _dbf, _DorH, _repeat, _align, _repeatAll, _FooterRepeatAll, _textWrap, _fieldsForExtra, _extraFieldName;
        public string FieldsForExtra
        {
            set
            {
                _fieldsForExtra = value;
            }
            get
            {
                return _fieldsForExtra;
            }
        }
        public string ExtraFieldName
        {
            set
            {
                _extraFieldName = value;
            }
            get
            {
                return _extraFieldName;
            }
        }
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
        public int DetailsId
        {
            set
            {
                _detailsId = value;
            }
            get
            {
                return _detailsId;
            }
        }
        public int Row
        {
            set
            {
                _row = value;
            }
            get
            {
                return _row;
            }
        }
        public int Columns
        {
            set
            {
                _columns = value;
            }
            get
            {
                return _columns;
            }
        }
        public int Width
        {
            set
            {
                _width = value;
            }
            get
            {
                return _width;
            }
        }
        public int WrapLineCount
        {
            set
            {
                _wrapLineCount = value;
            }
            get
            {
                return _wrapLineCount;
            }
        }
        public string Name
        {
            set
            {
                _name = value;
            }
            get
            {
                return _name;
            }
        }
        public string TextWrap
        {
            set
            {
                _textWrap = value;
            }
            get
            {
                return _textWrap;
            }
        }
        public string Text
        {
            set
            {
                _text = value;
            }
            get
            {
                return _text;
            }
        }
        public string DBF
        {
            set
            {
                _dbf = value;
            }
            get
            {
                return _dbf;
            }
        }
        public string DorH
        {
            set
            {
                _DorH = value;
            }
            get
            {
                return _DorH;
            }
        }
        public string Repeat
        {
            set
            {
                _repeat = value;
            }
            get
            {
                return _repeat;
            }
        }
        public string Align
        {
            set
            {
                _align = value;
            }
            get
            {
                return _align;
            }
        }
        public string RepeatAll
        {
            set
            {
                _repeatAll = value;
            }
            get
            {
                return _repeatAll;
            }
        }
        public string FooterRepeatAll
        {
            set
            {
                _FooterRepeatAll = value;
            }
            get
            {
                return _FooterRepeatAll;
            }
        }
    }
}
