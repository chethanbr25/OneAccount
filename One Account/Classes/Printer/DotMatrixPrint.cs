using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Collections;
namespace Open_Miracle
{
    public class DotMatrixPrint
    {
        //public static 
        public static DataTable dtbl2 = new DataTable();
        #region PRINTER SETTINGS
        public static void ResetPrinter(StreamWriter sw)
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)64);
            sequence.ToString();
            sw.Write(sequence.ToString());
        }
        public static void Pitch10(StreamWriter sw)
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)80);
            sequence.ToString();
            sw.Write(sequence.ToString());
        }
        public static void Pitch12(StreamWriter sw)
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)77);
            sequence.ToString();
            sw.Write(sequence.ToString());
        }
        public static void CondensedOn(StreamWriter sw)
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)15);
            sequence.Append((char)15);
            sequence.ToString();
            sw.Write(sequence.ToString());
        }
        public static void CondensedOff(StreamWriter sw)
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)18);
            sequence.ToString();
            sw.Write(sequence.ToString());
        }

        public void TearOn(StreamWriter sw)
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)25);
            sequence.Append((char)52);
            sequence.ToString();
            sw.Write(sequence.ToString());
        }
        public void TearOff(StreamWriter sw)
        {
            StringBuilder sequence = new StringBuilder();
            //sequence.Append((char)76);
            //sequence.Append((char)70);

            //sequence.Append((char)27);
            //sequence.Append((char)106);
            //sequence.Append((char)216);
            //sequence.Append((char)25);
            //sequence.Append((char)48);
            sequence.ToString();
            sw.Write(sequence.ToString());
        }
        #endregion
        public static void PrintDesign(int inFormId, DataTable dtblHedder, DataTable dtblDetails, DataTable dtblFooter)
        {
            DataTable dtbl = new DataTable();
            MasterSPrint spMaster = new MasterSPrint();
            MasterInfo infoMaster = new MasterInfo();
            DetailsSP spDetails = new DetailsSP();
            infoMaster = spMaster.MasterViewByFormName(inFormId);
            if (infoMaster.MasterId != 0)
                dtbl = spDetails.DetailsViewAll(infoMaster.MasterId);
            frmPrintDesigner.isDoubleLineRepeat = infoMaster.IsTwoLineForDetails;
            frmPrintDesigner.isDoubleLineNonRepeat = infoMaster.IsTwoLineForHedder;
            frmPrintDesigner.inLineCountBetweenTwoPages = infoMaster.LineCountBetweenTwo;
            frmPrintDesigner.inPageSizeInFirst = infoMaster.PageSize1;
            frmPrintDesigner.inPageSizeInOther = infoMaster.PageSizeOther;
            frmPrintDesigner.strPitch = infoMaster.Pitch == "10" ? "10" : "12";
            frmPrintDesigner.strCondensed = infoMaster.Condensed == "On" ? "On" : "Off";
            frmPrintDesigner.inBlankLineForFooter = infoMaster.BlankLneForFooter;
            frmPrintDesigner.strFooterLocation = infoMaster.FooterLocation;
            frmPrintDesigner.inLineCountAfterPrint = infoMaster.LineCountAfterPrint;
            bool isFirstFooter = true;
            bool isFisrtPFooter=true;
            DataTable dtblHeddingCheck = dtbl.Copy();
            dtblHeddingCheck.Rows.Clear();
            DataTable dtblDetailsCheck = dtblHeddingCheck.Copy();
            DataTable dtblFooterCheck = dtblHeddingCheck.Copy();
            DataRow[] drS = dtbl.Select("repeat='false'");
            foreach (DataRow dr in drS)
            {
                dtblHeddingCheck.Rows.Add();
                int inI = dtblHeddingCheck.Rows.Count - 1;
                for (int i = 0; i < dtblHeddingCheck.Columns.Count; i++)
                {
                    dtblHeddingCheck.Rows[inI][i] = dr[i];
                }
            }
            drS = dtbl.Select("repeat='true'");
            foreach (DataRow dr in drS)
            {
                dtblDetailsCheck.Rows.Add();
                int inI = dtblDetailsCheck.Rows.Count - 1;
                for (int i = 0; i < dtblDetailsCheck.Columns.Count; i++)
                {
                    dtblDetailsCheck.Rows[inI][i] = dr[i];
                }
            }
            drS = dtbl.Select("repeat='footer'");
            foreach (DataRow dr in drS)
            {
                dtblFooterCheck.Rows.Add();
                int inI = dtblFooterCheck.Rows.Count - 1;
                for (int i = 0; i < dtblFooterCheck.Columns.Count; i++)
                {
                    dtblFooterCheck.Rows[inI][i] = dr[i];
                }
            }
            int inCheckCountHedder = dtblHeddingCheck.Rows.Count;
            int inCheckCountDetails = dtblDetailsCheck.Rows.Count;
            int inCheckCountFooter = dtblFooterCheck.Rows.Count;
            DataTable dtblHExtra = new DataTable();
            DataTable dtblDExtra = new DataTable();
            DataTable dtblFExtra = new DataTable();
            dtblHExtra.Columns.Add("id");
            dtblHExtra.Columns.Add("extraFieldName");
            dtblHExtra.Columns.Add("fieldsForExtra");
            dtblHExtra.Columns.Add("width");

            dtblDExtra.Columns.Add("id");
            dtblDExtra.Columns.Add("extraFieldName");
            dtblDExtra.Columns.Add("fieldsForExtra");
            dtblDExtra.Columns.Add("width");

            dtblFExtra.Columns.Add("id");
            dtblFExtra.Columns.Add("extraFieldName");
            dtblFExtra.Columns.Add("fieldsForExtra");
            dtblFExtra.Columns.Add("width");

            for (int lH = 0; lH < inCheckCountHedder; lH++)
            {
                if (dtblHeddingCheck.Rows[lH]["extraFieldName"].ToString() != "")
                {
                    dtblHExtra.Rows.Add();
                    int inExtraCount = dtblHExtra.Rows.Count-1;
                    dtblHExtra.Rows[inExtraCount]["id"] = dtblHeddingCheck.Rows[lH]["name"].ToString();
                    dtblHExtra.Rows[inExtraCount]["extraFieldName"] = dtblHeddingCheck.Rows[lH]["extraFieldName"].ToString();
                    dtblHExtra.Rows[inExtraCount]["fieldsForExtra"] = dtblHeddingCheck.Rows[lH]["fieldsForExtra"].ToString();
                    dtblHExtra.Rows[inExtraCount]["width"] = dtblHeddingCheck.Rows[lH]["width"].ToString();
                }
            }
            for (int lH = 0; lH < inCheckCountDetails; lH++)
            {
                if (dtblDetailsCheck.Rows[lH]["extraFieldName"].ToString() != "")
                {
                    dtblDExtra.Rows.Add();
                    int inExtraCount = dtblDExtra.Rows.Count - 1;
                    dtblDExtra.Rows[inExtraCount]["id"] = dtblDetailsCheck.Rows[lH]["name"].ToString();
                    dtblDExtra.Rows[inExtraCount]["extraFieldName"] = dtblDetailsCheck.Rows[lH]["extraFieldName"].ToString();
                    dtblDExtra.Rows[inExtraCount]["fieldsForExtra"] = dtblDetailsCheck.Rows[lH]["fieldsForExtra"].ToString();
                    dtblDExtra.Rows[inExtraCount]["width"] = dtblDetailsCheck.Rows[lH]["width"].ToString();
                }
            }
            for (int lH = 0; lH < inCheckCountFooter; lH++)
            {
                if (dtblFooterCheck.Rows[lH]["extraFieldName"].ToString() != "")
                {
                    dtblFExtra.Rows.Add();
                    int inExtraCount = dtblFExtra.Rows.Count - 1;
                    dtblFExtra.Rows[inExtraCount]["id"] = dtblFooterCheck.Rows[lH]["name"].ToString();
                    dtblFExtra.Rows[inExtraCount]["extraFieldName"] = dtblFooterCheck.Rows[lH]["extraFieldName"].ToString();
                    dtblFExtra.Rows[inExtraCount]["fieldsForExtra"] = dtblFooterCheck.Rows[lH]["fieldsForExtra"].ToString();
                    dtblFExtra.Rows[inExtraCount]["width"] = dtblFooterCheck.Rows[lH]["width"].ToString();
                }
            }
            int inExtraAddCount=dtblHExtra.Rows.Count;
            for (int lH = 0; lH < inExtraAddCount; lH++)
            {
                int inExtraWidth = int.Parse(dtblHExtra.Rows[lH]["width"].ToString());
                string strField = dtblHExtra.Rows[lH]["fieldsForExtra"].ToString();
                string strSubField = dtblHExtra.Rows[lH]["extraFieldName"].ToString();
                string strText;
                if (dtblHedder.Rows[0][strField].ToString().Length >= (inExtraWidth * (lH + 1) + inExtraWidth))
                {
                    strText = dtblHedder.Rows[0][strField].ToString().Substring((inExtraWidth * (lH + 1)), inExtraWidth);
                }
                else
                {
                    int inSubPoint = (dtblHedder.Rows[0][strField].ToString().Length - (inExtraWidth * (lH + 1))) < 0 ? 0 : (dtblHedder.Rows[0][strField].ToString().Length - (inExtraWidth * (lH + 1)));
                    int inLeftPoint = (inExtraWidth * (lH + 1)) <= dtblHedder.Rows[0][strField].ToString().Length ? (inExtraWidth * (lH + 1)) : 0;
                    strText = dtblHedder.Rows[0][strField].ToString().Substring(inLeftPoint, inSubPoint);
                }
                 
                dtblHedder.Columns.Add(strSubField);
                dtblHedder.Rows[0][strSubField] = strText;
            }
            inExtraAddCount = dtblDExtra.Rows.Count;
            for (int lH = 0; lH < inExtraAddCount; lH++)
            {
                int inExtraWidth = int.Parse(dtblDExtra.Rows[lH]["width"].ToString());
                string strField = dtblDExtra.Rows[lH]["fieldsForExtra"].ToString();
                string strSubField = dtblDExtra.Rows[lH]["extraFieldName"].ToString();
                string strText;
                if (dtblDetails.Rows[0][strField].ToString().Length >= (inExtraWidth * (lH + 1) + inExtraWidth))
                {
                    strText = dtblDetails.Rows[0][strField].ToString().Substring((inExtraWidth * (lH + 1)), inExtraWidth);
                }
                else
                {
                    int inSubPoint = (dtblDetails.Rows[0][strField].ToString().Length - (inExtraWidth * (lH + 1))) < 0 ? 0 : (dtblDetails.Rows[0][strField].ToString().Length - (inExtraWidth * (lH + 1)));
                    int inLeftPoint = (inExtraWidth * (lH + 1)) <= dtblDetails.Rows[0][strField].ToString().Length ? (inExtraWidth * (lH + 1)) : 0;
                    strText = dtblDetails.Rows[0][strField].ToString().Substring(inLeftPoint, inSubPoint);
                }

                dtblDetails.Columns.Add(strSubField);
                int inExtraDC=dtblDetails.Rows.Count;
                for (int lfd = 0; lfd < inExtraDC; lfd++)
                {
                    dtblDetails.Rows[lfd][strSubField] = strText;
                }
            }

            inExtraAddCount = dtblFExtra.Rows.Count;
            for (int lH = 0; lH < inExtraAddCount; lH++)
            {
                int inExtraWidth = int.Parse(dtblFExtra.Rows[lH]["width"].ToString());
                string strField = dtblFExtra.Rows[lH]["fieldsForExtra"].ToString();
                string strSubField = dtblFExtra.Rows[lH]["extraFieldName"].ToString();
                string strText;
                if (dtblFooter.Rows[0][strField].ToString().Length >= (inExtraWidth * (lH + 1) + inExtraWidth))
                {
                    strText = dtblFooter.Rows[0][strField].ToString().Substring((inExtraWidth * (lH + 1)), inExtraWidth);
                }
                else
                {
                    int inSubPoint = (dtblFooter.Rows[0][strField].ToString().Length - (inExtraWidth * (lH + 1))) < 0 ? 0 : (dtblFooter.Rows[0][strField].ToString().Length - (inExtraWidth * (lH + 1)));
                    int inLeftPoint = (inExtraWidth * (lH + 1)) <= dtblFooter.Rows[0][strField].ToString().Length ? (inExtraWidth * (lH + 1)) : 0;
                    strText = dtblFooter.Rows[0][strField].ToString().Substring(inLeftPoint, inSubPoint);
                }

                dtblFooter.Columns.Add(strSubField);
                dtblFooter.Rows[0][strSubField] = strText;
            }
            PrintDialog pd = new PrintDialog();
            StreamWriter sw = null;
            int length = 0;
            int inHeight = 0;
            int inMasterHeight = 0;
            int inD = 0;
            int inCountD = dtblDetails.Rows.Count;
            bool isNextPage = false;
            string strRepeat = "false";
            try
            {
                string strExtraLine = "", strExtraLine2 = "";
                DataRow[] dr = new DataRow[10];
                string filePath = Application.StartupPath + "\\print.txt";
                sw = new StreamWriter(filePath, false);
                //ResetPrinter(sw);
                if (frmPrintDesigner.strPitch == "10")
                {
                    Pitch10(sw);
                }
                else
                {
                    Pitch12(sw);
                }
                if (frmPrintDesigner.strCondensed == "On")
                {
                    CondensedOn(sw);
                }
                else
                {
                    CondensedOff(sw);
                }
                FileInfo finfo = new FileInfo(filePath);
                length = 135;
                int inMaxLenght = length;
                int inMaxRow = int.Parse(dtbl.Compute("MAX(row)", "").ToString());
                int inMinRow = int.Parse(dtbl.Compute("MIN(row)", "").ToString());
                string strMaxFooter = dtbl.Compute("MAX(row)", "Repeat='Footer'").ToString();
                string strMinFooter = dtbl.Compute("MIN(row)", "Repeat='Footer'").ToString();
                int inMaxFooter = int.Parse(strMaxFooter == "" ? "-2" : strMaxFooter);
                int inMinFooter = int.Parse(strMinFooter == "" ? "-1" : strMinFooter);
                for (int inloop = 0; inloop <= inMaxRow; inloop++)
                {
                    string strLine = "";
                    int inCount = int.Parse(dtbl.Compute("Count(row)", "row='" + inloop.ToString() + "'").ToString());
                    DataRow[] drCurrent = dtbl.Select("row='" + inloop.ToString() + "'", "columns");
                    for (int iniR = 0; iniR < drCurrent.Length; iniR++)
                        if (drCurrent[iniR]["Repeat"].ToString() == "true")
                        {
                            strRepeat = "true";
                            break;
                        }
                        else if (drCurrent[iniR]["Repeat"].ToString() == "false")
                            strRepeat = "false";
                        else
                            strRepeat = "Footer";
                    if (strRepeat == "false")
                    {
                        strExtraLine = strExtraLine2 = "";
                        for (int innerLoop = 0; innerLoop < inCount; innerLoop++)
                        {
                            DataRow currentRow = drCurrent[innerLoop];
                            int inRowPos = int.Parse(currentRow["row"].ToString());
                            int inColPos = int.Parse(currentRow["columns"].ToString());
                            int inTextWidh = int.Parse(currentRow["width"].ToString());
                            string strCotant;
                            if (currentRow["DBF"].ToString() != "")
                                strCotant = dtblHedder.Rows[0][currentRow["DBF"].ToString()].ToString();
                            else if (currentRow["extraFieldName"].ToString() != "")
                                strCotant = dtblHedder.Rows[0][currentRow["extraFieldName"].ToString()].ToString();
                            else
                                strCotant = currentRow["text"].ToString();
                            string strBefore = "", strAfter = "";
                            for (int c = 0; c < inColPos - strLine.Length; c++)
                                strBefore = strBefore + " ";
                            if (inTextWidh >= strCotant.Length)
                            {
                                string strContantSpace = "";
                                for (int a = 0; a < inTextWidh - strCotant.Length; a++)
                                    strAfter = strAfter + " ";
                                for (int b = 0; b < strCotant.Length; b++)
                                    strContantSpace = strContantSpace + " ";
                                strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                            }
                            else
                            {
                                string strSubAfter = "", strSubAfter2 = "", strNextLineContant = "", strNextLineContant2 = "";
                                if (currentRow["textWrap"].ToString().ToLower() == "true")
                                {
                                    string strNextLineFull = strCotant.Substring(inTextWidh);
                                    strNextLineContant = strCotant.Substring(inTextWidh, inTextWidh);
                                    for (int a = 0; a < inTextWidh - strNextLineContant.Length; a++)
                                        strSubAfter = strSubAfter + " ";

                                    if (inTextWidh < strNextLineFull.Length && int.Parse(currentRow["wrapLineCount"].ToString()) > 1)
                                    {
                                        strNextLineContant2 = strNextLineFull.Substring(inTextWidh, strNextLineFull.Length - inTextWidh);
                                        for (int a = 0; a < inTextWidh - strNextLineContant2.Length; a++)
                                            strSubAfter2 = strSubAfter2 + " ";
                                    }

                                    //-------- Font Size-----------
                                    #region Give Font Size
                                    if (currentRow["dOrH"].ToString() == "Hedding")
                                    {
                                        strNextLineContant = RawPrinterHelper.Headder(strNextLineContant);
                                        strNextLineContant2 = RawPrinterHelper.Headder(strNextLineContant2);
                                    }
                                    else if (currentRow["dOrH"].ToString() == "Bold")
                                    {
                                        strNextLineContant = RawPrinterHelper.Bold(strNextLineContant);
                                        strNextLineContant2 = RawPrinterHelper.Bold(strNextLineContant2);
                                    }
                                    else if (currentRow["dOrH"].ToString() == "Italic")
                                    {
                                        strNextLineContant = RawPrinterHelper.Italic(strNextLineContant);
                                        strNextLineContant2 = RawPrinterHelper.Italic(strNextLineContant2);
                                    }
                                    #endregion Give Font Size
                                    //-------- Font Size-----------

                                    //-------- Font Alignment-----------
                                    if (currentRow["Align"].ToString() == "Left")
                                    {
                                        strExtraLine = strExtraLine + strBefore + strNextLineContant + strSubAfter;
                                        strExtraLine2 = strExtraLine2 + strBefore + strNextLineContant2 + strSubAfter2;
                                    }
                                    else if (currentRow["Align"].ToString() == "Right")
                                    {
                                        strExtraLine = strExtraLine + strBefore + strSubAfter + strNextLineContant;
                                        strExtraLine2 = strExtraLine2 + strBefore + strSubAfter2 + strNextLineContant2;
                                    }
                                    else
                                    {
                                        string strBeforeText, strBeforeText2, strAfterText, strAfterText2;
                                        strBeforeText = strSubAfter.Substring(0, strSubAfter.Length / 2);
                                        strBeforeText2 = strSubAfter2.Substring(0, strSubAfter2.Length / 2);
                                        if (strSubAfter.Length % 2 == 0)
                                            strAfterText = strBeforeText;
                                        else
                                            strAfterText = strBeforeText.Insert(0, " ");
                                        if (strSubAfter2.Length % 2 == 0)
                                            strAfterText2 = strBeforeText2;
                                        else
                                            strAfterText2 = strBeforeText2.Insert(0, " ");
                                        strExtraLine = strExtraLine + strBefore + strBeforeText + strNextLineContant + strAfterText;
                                        strExtraLine2 = strExtraLine2 + strBefore + strBeforeText2 + strNextLineContant2 + strAfterText2;
                                    }
                                    //-----------------
                                }
                                else
                                {
                                    string strContantSpace = ""; ;
                                    string strCurrentContant = strCotant.Substring(0, inTextWidh);
                                    for (int b = 0; b < strCurrentContant.Length; b++)
                                        strContantSpace = strContantSpace + " ";
                                    strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                    strExtraLine2 = strExtraLine2 + strBefore + strContantSpace + strAfter;
                                    //strExtraLine = "";
                                }
                                strCotant = strCotant.Substring(0, inTextWidh);
                            }
                            //-------- Font Size-----------
                            #region Give Font Size
                            if (currentRow["dOrH"].ToString() == "Hedding")
                                strCotant = RawPrinterHelper.Headder(strCotant);
                            else if (currentRow["dOrH"].ToString() == "Bold")
                                strCotant = RawPrinterHelper.Bold(strCotant);
                            else if (currentRow["dOrH"].ToString() == "Italic")
                                strCotant = RawPrinterHelper.Italic(strCotant);
                            #endregion Give Font Size
                            //-------- Font Size-----------

                            //-------- Font Alignment-----------
                            if (currentRow["Align"].ToString() == "Left")
                                strLine = strLine + strBefore + strCotant + strAfter;
                            else if (currentRow["Align"].ToString() == "Right")
                                strLine = strLine + strBefore + strAfter + strCotant;
                            else
                            {
                                string strBeforeText, strAfterText;
                                strBeforeText = strAfter.Substring(0, strAfter.Length / 2);
                                if (strAfter.Length % 2 == 0)
                                    strAfterText = strBeforeText;
                                else
                                    strAfterText = strBeforeText.Insert(0, " ");
                                strLine = strLine + strBefore + strBeforeText + strCotant + strAfterText;
                            }
                            //-------- Font Alignment-----------
                        }
                        sw.Write(strLine);
                        sw.WriteLine();
                        inHeight++;
                        inMasterHeight++;
                        isNextPage = false;
                        if (strExtraLine.Trim() != "")
                        {
                            sw.Write(strExtraLine);
                            sw.WriteLine();
                            inHeight++;
                            inMasterHeight++;
                            strExtraLine = "";
                            isNextPage = false;
                        }
                        if (strExtraLine2.Trim() != "")
                        {
                            sw.Write(strExtraLine2);
                            sw.WriteLine();
                            inHeight++;
                            inMasterHeight++;
                            strExtraLine2 = "";
                            isNextPage = false;
                        }
                    }
                    else if (strRepeat == "true")
                    {
                        strExtraLine = "";
                        strExtraLine2 = "";
                        for (; inD < inCountD; inD++)
                        {
                            strLine = "";

                            for (int innerLoop = 0; innerLoop < inCount; innerLoop++)
                            {
                                DataRow currentRow = drCurrent[innerLoop];
                                int inRowPos = int.Parse(currentRow["row"].ToString());
                                int inColPos = int.Parse(currentRow["columns"].ToString());
                                int inTextWidh = int.Parse(currentRow["width"].ToString());
                                string strCotant;
                                if (currentRow["DBF"].ToString() != "")
                                    strCotant = dtblDetails.Rows[inD][currentRow["DBF"].ToString()].ToString();
                                else if (currentRow["extraFieldName"].ToString() != "")
                                    strCotant = dtblDetails.Rows[inD][currentRow["extraFieldName"].ToString()].ToString();
                                else
                                    strCotant = currentRow["text"].ToString();
                                string strBefore = "", strAfter = "";
                                for (int c = 0; c < inColPos - strLine.Length; c++)
                                    strBefore = strBefore + " ";
                                //-- Check Length grater than width---------
                                if (inTextWidh >= strCotant.Length)
                                {
                                    string strContantSpace = "";
                                    for (int a = 0; a < inTextWidh - strCotant.Length; a++)
                                        strAfter = strAfter + " ";

                                    for (int b = 0; b < strCotant.Length; b++)
                                        strContantSpace = strContantSpace + " ";
                                    strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                }
                                else
                                {
                                    string strSubAfter = "", strSubAfter2 = "", strNextLineContant = "", strNextLineContant2 = "";

                                    if (currentRow["textWrap"].ToString().ToLower() == "true")
                                    {
                                        string strNextLineFull = strCotant.Substring(inTextWidh);
                                        strNextLineContant = strCotant.Substring(inTextWidh, inTextWidh);
                                        for (int a = 0; a < inTextWidh - strNextLineContant.Length; a++)
                                            strSubAfter = strSubAfter + " ";
                                        if (inTextWidh < strNextLineFull.Length && int.Parse(currentRow["wrapLineCount"].ToString()) > 1)
                                        {
                                            strNextLineContant2 = strNextLineFull.Substring(inTextWidh, strNextLineFull.Length - inTextWidh);
                                            for (int a = 0; a < inTextWidh - strNextLineContant2.Length; a++)
                                                strSubAfter2 = strSubAfter2 + " ";
                                        }
                                        //-------- Font Size-----------
                                        #region Give Font Size
                                        if (currentRow["dOrH"].ToString() == "Hedding")
                                        {
                                            strNextLineContant = RawPrinterHelper.Headder(strNextLineContant);
                                            strNextLineContant2 = RawPrinterHelper.Headder(strNextLineContant2);
                                        }
                                        else if (currentRow["dOrH"].ToString() == "Bold")
                                        {
                                            strNextLineContant = RawPrinterHelper.Bold(strNextLineContant);
                                            strNextLineContant2 = RawPrinterHelper.Bold(strNextLineContant2);
                                        }
                                        else if (currentRow["dOrH"].ToString() == "Italic")
                                        {
                                            strNextLineContant = RawPrinterHelper.Italic(strNextLineContant);
                                            strNextLineContant2 = RawPrinterHelper.Italic(strNextLineContant2);
                                        }
                                        #endregion Give Font Size
                                        //-------- Font Size-----------

                                        //-------- Font Alignment-----------
                                        if (currentRow["Align"].ToString() == "Left")
                                        {
                                            strExtraLine = strExtraLine + strBefore + strNextLineContant + strSubAfter;
                                            strExtraLine2 = strExtraLine2 + strBefore + strNextLineContant2 + strSubAfter2;
                                        }
                                        else if (currentRow["Align"].ToString() == "Right")
                                        {
                                            strExtraLine = strExtraLine + strBefore + strSubAfter + strNextLineContant;
                                            strExtraLine2 = strExtraLine2 + strBefore + strSubAfter2 + strNextLineContant2;
                                        }
                                        else
                                        {
                                            string strBeforeText, strBeforeText2, strAfterText, strAfterText2;
                                            strBeforeText = strSubAfter.Substring(0, strSubAfter.Length / 2);
                                            strBeforeText2 = strSubAfter2.Substring(0, strSubAfter2.Length / 2);
                                            if (strSubAfter.Length % 2 == 0)
                                                strAfterText = strBeforeText;
                                            else
                                                strAfterText = strBeforeText.Insert(0, " ");

                                            if (strSubAfter2.Length % 2 == 0)
                                                strAfterText2 = strBeforeText2;
                                            else
                                                strAfterText2 = strBeforeText2.Insert(0, " ");
                                            strExtraLine = strExtraLine + strBefore + strBeforeText + strNextLineContant + strAfterText;
                                            strExtraLine2 = strExtraLine2 + strBefore + strBeforeText2 + strNextLineContant2 + strAfterText2;
                                        }
                                        //-----------------
                                    }
                                    else
                                    {
                                        string strContantSpace = ""; ;
                                        string strCurrentContant = strCotant.Substring(0, inTextWidh);
                                        for (int b = 0; b < strCurrentContant.Length; b++)
                                            strContantSpace = strContantSpace + " ";
                                        strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                        strExtraLine2 = strExtraLine2 + strBefore + strContantSpace + strAfter;
                                        //strExtraLine = "";
                                    }
                                    strCotant = strCotant.Substring(0, inTextWidh);
                                }
                                //-------- Font Size-----------
                                #region Give Font Size
                                if (currentRow["dOrH"].ToString() == "Hedding")
                                    strCotant = RawPrinterHelper.Headder(strCotant);
                                else if (currentRow["dOrH"].ToString() == "Bold")
                                    strCotant = RawPrinterHelper.Bold(strCotant);
                                else if (currentRow["dOrH"].ToString() == "Italic")
                                    strCotant = RawPrinterHelper.Italic(strCotant);
                                #endregion Give Font Size
                                //-------- Font Size-----------

                                //-------- Font Alignment-----------
                                if (currentRow["Align"].ToString() == "Left")
                                    strLine = strLine + strBefore + strCotant + strAfter;
                                else if (currentRow["Align"].ToString() == "Right")
                                    strLine = strLine + strBefore + strAfter + strCotant;
                                else
                                {
                                    string strBeforeText, strAfterText;
                                    strBeforeText = strAfter.Substring(0, strAfter.Length / 2);
                                    if (strAfter.Length % 2 == 0)
                                        strAfterText = strBeforeText;
                                    else
                                        strAfterText = strBeforeText.Insert(0, " ");
                                    strLine = strLine + strBefore + strBeforeText + strCotant + strAfterText;
                                }
                                //-------- Font Alignment-----------
                            }
                            sw.Write(strLine);
                            sw.WriteLine();
                            inHeight++;
                            isNextPage = false;
                            if (inHeight - inMasterHeight >= frmPrintDesigner.inPageSizeInFirst)
                            {
                                isNextPage = true;
                                break;
                            }
                            if (strExtraLine.Trim() != "")
                            {
                                sw.Write(strExtraLine);
                                sw.WriteLine();
                                inHeight++;
                                strExtraLine = "";
                                isNextPage = false;
                                if (inHeight - inMasterHeight >= frmPrintDesigner.inPageSizeInFirst)
                                {
                                    isNextPage = true;
                                    break;
                                }
                            }
                            if (strExtraLine2.Trim() != "")
                            {
                                sw.Write(strExtraLine2);
                                sw.WriteLine();
                                inHeight++;
                                strExtraLine2 = "";
                                isNextPage = false;
                                if (inHeight - inMasterHeight >= frmPrintDesigner.inPageSizeInFirst)
                                {
                                    isNextPage = true;
                                    break;
                                }
                            }
                        }

                        for (int inF = inMinFooter; inF <= inMaxFooter; inF++)
                        {
                            strLine = "";
                            int inFCount = int.Parse(dtbl.Compute("Count(row)", "row='" + inF.ToString() + "'").ToString());
                            DataRow[] drFCurrent = dtbl.Select("row='" + inF.ToString() + "'", "columns");
                            strExtraLine = "";
                            strExtraLine2 = "";
                            bool isHere = false;
                            for (int inForCheckingInner = 0; inForCheckingInner < inFCount; inForCheckingInner++)
                            {
                                DataRow currentCheckingFRow = drFCurrent[inForCheckingInner];
                                isHere = bool.Parse(currentCheckingFRow["FooterRepeatAll"].ToString());
                                if (isHere)
                                    break;
                               // else
                                    //if (isNextPage)
                                       // sw.WriteLine();// For printig null footer for all other pages 
                            }
                            if (isHere)
                            {
                                if (frmPrintDesigner.strFooterLocation == "PageEnd")
                                {
                                    if (isFisrtPFooter)
                                    {
                                        //if (inHeight - inMasterHeight < frmPrintDesigner.inPageSizeInFirst)
                                        //{
                                            int w = inHeight - inMasterHeight;
                                            while (w < frmPrintDesigner.inPageSizeInFirst)
                                            {
                                                sw.WriteLine();
                                                w++;
                                            }
                                        //}
                                        isFisrtPFooter = false;
                                    }
                                }
                                else
                                {
                                    if (isFirstFooter)
                                    {
                                        int w = 0;
                                        while (w < frmPrintDesigner.inBlankLineForFooter)
                                        {
                                            sw.WriteLine();
                                            w++;
                                        }
                                        isFirstFooter = false;
                                    }
                                }
                            }
                            for (int inFInner = 0; inFInner < inFCount; inFInner++)
                            {
                                DataRow currentFRow = drFCurrent[inFInner];
                                if (bool.Parse(currentFRow["FooterRepeatAll"].ToString()))
                                {
                                    int inRowPos = int.Parse(currentFRow["row"].ToString());
                                    int inColPos = int.Parse(currentFRow["columns"].ToString());
                                    int inTextWidh = int.Parse(currentFRow["width"].ToString());
                                    string strCotant;
                                    if (currentFRow["DBF"].ToString() != "")
                                        strCotant = dtblFooter.Rows[0][currentFRow["DBF"].ToString()].ToString();
                                    else if (currentFRow["extraFieldName"].ToString() != "")
                                        strCotant = dtblFooter.Rows[0][currentFRow["extraFieldName"].ToString()].ToString();
                                    else
                                        strCotant = currentFRow["text"].ToString();
                                  
                                    string strBefore = "", strAfter = "";
                                    for (int c = 0; c < inColPos - strLine.Length; c++)
                                        strBefore = strBefore + " ";

                                    if (inTextWidh >= strCotant.Length)
                                    {
                                        string strContantSpace = "";
                                        for (int a = 0; a < inTextWidh - strCotant.Length; a++)
                                            strAfter = strAfter + " ";
                                        for (int b = 0; b < strCotant.Length; b++)
                                            strContantSpace = strContantSpace + " ";
                                        strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                    }
                                    else
                                    {
                                        string strSubAfter = "", strSubAfter2 = "", strNextLineContant = "", strNextLineContant2 = "";

                                        if (currentFRow["textWrap"].ToString().ToLower() == "true")
                                        {
                                            string strNextLineFull = strCotant.Substring(inTextWidh);
                                            strNextLineContant = strCotant.Substring(inTextWidh, inTextWidh);
                                            for (int a = 0; a < inTextWidh - strNextLineContant.Length; a++)
                                                strSubAfter = strSubAfter + " ";
                                            if (inTextWidh < strNextLineFull.Length && int.Parse(currentFRow["wrapLineCount"].ToString()) > 1)
                                            {
                                                strNextLineContant2 = strNextLineFull.Substring(inTextWidh, strNextLineFull.Length - inTextWidh);
                                                for (int a = 0; a < inTextWidh - strNextLineContant2.Length; a++)
                                                    strSubAfter2 = strSubAfter2 + " ";
                                            }

                                            //-------- Font Size-----------
                                            #region Give Font Size
                                            if (currentFRow["dOrH"].ToString() == "Hedding")
                                            {
                                                strNextLineContant = RawPrinterHelper.Headder(strNextLineContant);
                                                strNextLineContant2 = RawPrinterHelper.Headder(strNextLineContant2);
                                            }
                                            else if (currentFRow["dOrH"].ToString() == "Bold")
                                            {
                                                strNextLineContant = RawPrinterHelper.Bold(strNextLineContant);
                                                strNextLineContant2 = RawPrinterHelper.Bold(strNextLineContant2);
                                            }
                                            else if (currentFRow["dOrH"].ToString() == "Italic")
                                            {
                                                strNextLineContant = RawPrinterHelper.Italic(strNextLineContant);
                                                strNextLineContant2 = RawPrinterHelper.Italic(strNextLineContant2);
                                            }
                                            #endregion Give Font Size
                                            //-------- Font Size-----------

                                            //-------- Font Alignment-----------
                                            if (currentFRow["Align"].ToString() == "Left")
                                            {
                                                strExtraLine = strExtraLine + strBefore + strNextLineContant + strSubAfter;
                                                strExtraLine2 = strExtraLine2 + strBefore + strNextLineContant2 + strSubAfter2;
                                            }
                                            else if (currentFRow["Align"].ToString() == "Right")
                                            {
                                                strExtraLine = strExtraLine + strBefore + strSubAfter + strNextLineContant;
                                                strExtraLine2 = strExtraLine2 + strBefore + strSubAfter2 + strNextLineContant2;
                                            }
                                            else
                                            {
                                                string strBeforeText, strBeforeText2, strAfterText, strAfterText2;
                                                strBeforeText = strSubAfter.Substring(0, strSubAfter.Length / 2);
                                                strBeforeText2 = strSubAfter2.Substring(0, strSubAfter2.Length / 2);
                                                if (strSubAfter.Length % 2 == 0)
                                                    strAfterText = strBeforeText;
                                                else
                                                    strAfterText = strBeforeText.Insert(0, " ");
                                                if (strSubAfter2.Length % 2 == 0)
                                                    strAfterText2 = strBeforeText2;
                                                else
                                                    strAfterText2 = strBeforeText2.Insert(0, " ");
                                                strExtraLine = strExtraLine + strBefore + strBeforeText + strNextLineContant + strAfterText;
                                                strExtraLine2 = strExtraLine2 + strBefore + strBeforeText2 + strNextLineContant2 + strAfterText2;
                                            }
                                            //-----------------
                                        }
                                        else
                                        {
                                            string strContantSpace = ""; ;
                                            string strCurrentContant = strCotant.Substring(0, inTextWidh);
                                            for (int b = 0; b < strCurrentContant.Length; b++)
                                                strContantSpace = strContantSpace + " ";
                                            strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                            strExtraLine2 = strExtraLine2 + strBefore + strContantSpace + strAfter;
                                            // strExtraLine = "";
                                        }
                                        strCotant = strCotant.Substring(0, inTextWidh);
                                    }
                                    //-------- Font Size-----------
                                    #region Give Font Size
                                    if (currentFRow["dOrH"].ToString() == "Hedding")
                                        strCotant = RawPrinterHelper.Headder(strCotant);
                                    else if (currentFRow["dOrH"].ToString() == "Bold")
                                        strCotant = RawPrinterHelper.Bold(strCotant);
                                    else if (currentFRow["dOrH"].ToString() == "Italic")
                                        strCotant = RawPrinterHelper.Italic(strCotant);
                                    #endregion Give Font Size
                                    //-------- Font Size-----------

                                    //-------- Font Alignment-----------
                                    if (currentFRow["Align"].ToString() == "Left")
                                        strLine = strLine + strBefore + strCotant + strAfter;
                                    else if (currentFRow["Align"].ToString() == "Right")
                                        strLine = strLine + strBefore + strAfter + strCotant;
                                    else
                                        strLine = strLine + strBefore + strAfter + strCotant;
                                    //-------- Font Alignment-----------
                                }
                            }
                            if (strLine != "")
                            {
                                sw.Write(strLine);
                                sw.WriteLine();
                                inHeight++;
                                inMasterHeight++;
                            }
                            if (strExtraLine.Trim() != "")
                            {
                                sw.Write(strExtraLine);
                                sw.WriteLine();
                                inHeight++;
                                inMasterHeight++;
                                strExtraLine = "";
                            }
                            if (strExtraLine2.Trim() != "")
                            {
                                sw.Write(strExtraLine2);
                                sw.WriteLine();
                                inHeight++;
                                inMasterHeight++;
                                strExtraLine2 = "";
                            }
                        }
                    }
                    isNextPage = false;
                    if (inHeight - inMasterHeight >= frmPrintDesigner.inPageSizeInFirst && inD < inCountD - 1)
                    {
                        isNextPage = true;
                        int w = 0;
                        while (frmPrintDesigner.inLineCountBetweenTwoPages > w)
                        {
                            sw.WriteLine();
                            w++;
                        }
                        inD++;
                        break;
                    }
                }
                if (isNextPage)
                {
                    isFisrtPFooter = true;
                    isFirstFooter = true;
                    inHeight = 0;
                    inMasterHeight = 0;
                    int inloop = 0;
                    for (; inloop <= inMaxRow; inloop++)
                    {
                        strRepeat = "";
                        if (inloop == 0)
                        {
                            isFisrtPFooter = true;
                            isFirstFooter = true;
                            inHeight = 0;
                            inMasterHeight = 0;
                        }
                        if (strExtraLine.Trim() != "")
                        {
                            sw.Write(strExtraLine);
                            sw.WriteLine();
                            inHeight++;
                            strExtraLine = "";
                            strExtraLine2 = "";
                            isNextPage = false;
                            if (inHeight >= frmPrintDesigner.inPageSizeInOther)
                            {
                                isNextPage = true;
                                int w = 0;
                                while (frmPrintDesigner.inLineCountBetweenTwoPages > w)
                                {
                                    sw.WriteLine();
                                    w++;
                                }
                                inHeight = 0;
                                inloop = -1;
                                inD++;
                            }
                        }
                        else
                            strExtraLine = "";
                        if (strExtraLine2.Trim() != "")
                        {
                            sw.Write(strExtraLine2);
                            sw.WriteLine();
                            inHeight++;
                            strExtraLine2 = "";
                            isNextPage = false;
                            if (inHeight >= frmPrintDesigner.inPageSizeInOther)
                            {
                                isNextPage = true;
                                int w = 0;
                                while (frmPrintDesigner.inLineCountBetweenTwoPages > w)
                                {
                                    sw.WriteLine();
                                    w++;
                                }
                                inHeight = 0;
                                inloop = -1;
                                inD++;
                            }
                        }
                        else
                            strExtraLine2 = "";
                        string strLine = "";
                        int inCount = int.Parse(dtbl.Compute("Count(row)", "row='" + inloop.ToString() + "'").ToString());
                        DataRow[] drCurrent = dtbl.Select("row='" + inloop.ToString() + "'", "columns");
                        for (int iniR = 0; iniR < drCurrent.Length; iniR++)
                        {
                            if (drCurrent[iniR]["Repeat"].ToString() == "true")
                            {
                                strRepeat = "true";
                                break;
                            }
                            else if (drCurrent[iniR]["Repeat"].ToString() == "false")
                            {
                                strRepeat = "false";
                            }
                            else
                            {
                                strRepeat = "Footer";
                            }
                        }
                        if (strRepeat == "false") ///////Header
                        {
                            strExtraLine = "";
                            strExtraLine2 = "";
                            for (int innerLoop = 0; innerLoop < inCount; innerLoop++)
                            {
                                DataRow currentRow = drCurrent[innerLoop];
                                bool isHere = bool.Parse(currentRow["RepeatAllPage"].ToString());
                                if (isHere)
                                {
                                    int inRowPos = int.Parse(currentRow["row"].ToString());
                                    int inColPos = int.Parse(currentRow["columns"].ToString());
                                    int inTextWidh = int.Parse(currentRow["width"].ToString());
                                    string strCotant;
                                    if (currentRow["DBF"].ToString() != "")
                                        strCotant = dtblHedder.Rows[0][currentRow["DBF"].ToString()].ToString();
                                    else if (currentRow["extraFieldName"].ToString() != "")
                                        strCotant = dtblHedder.Rows[0][currentRow["extraFieldName"].ToString()].ToString();
                                    else
                                        strCotant = currentRow["text"].ToString();
                                    string strBefore = "", strAfter = "";
                                    for (int c = 0; c < inColPos - strLine.Length; c++)
                                        strBefore = strBefore + " ";

                                    if (inTextWidh >= strCotant.Length)
                                    {
                                        string strContantSpace = "";
                                        for (int a = 0; a < inTextWidh - strCotant.Length; a++)
                                            strAfter = strAfter + " ";
                                        for (int b = 0; b < strCotant.Length; b++)
                                            strContantSpace = strContantSpace + " ";
                                        strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                    }
                                    else
                                    {
                                        string strSubAfter = "", strSubAfter2 = "", strNextLineContant = "", strNextLineContant2 = "";
                                        if (currentRow["textWrap"].ToString().ToLower() == "true")
                                        {
                                            string strNextLineFull = strCotant.Substring(inTextWidh);
                                            strNextLineContant = strCotant.Substring(inTextWidh, inTextWidh);
                                            for (int a = 0; a < inTextWidh - strCotant.Substring(inTextWidh).Length; a++)
                                                strSubAfter = strSubAfter + " ";

                                            if (inTextWidh < strNextLineFull.Length && int.Parse(currentRow["wrapLineCount"].ToString()) > 1)
                                            {
                                                strNextLineContant2 = strNextLineFull.Substring(inTextWidh, strNextLineFull.Length - inTextWidh);
                                                for (int a = 0; a < inTextWidh - strNextLineContant2.Length; a++)
                                                    strSubAfter2 = strSubAfter2 + " ";
                                            }

                                            //-------- Font Size-----------
                                            #region Give Font Size
                                            if (currentRow["dOrH"].ToString() == "Hedding")
                                            {
                                                strNextLineContant = RawPrinterHelper.Headder(strNextLineContant);
                                                strNextLineContant2 = RawPrinterHelper.Headder(strNextLineContant2);
                                            }
                                            else if (currentRow["dOrH"].ToString() == "Bold")
                                            {
                                                strNextLineContant = RawPrinterHelper.Bold(strNextLineContant);
                                                strNextLineContant2 = RawPrinterHelper.Bold(strNextLineContant2);
                                            }
                                            else if (currentRow["dOrH"].ToString() == "Italic")
                                            {
                                                strNextLineContant = RawPrinterHelper.Italic(strNextLineContant);
                                                strNextLineContant2 = RawPrinterHelper.Italic(strNextLineContant2);
                                            }
                                            #endregion Give Font Size
                                            //-------- Font Size-----------

                                            //-------- Font Alignment-----------
                                            if (currentRow["Align"].ToString() == "Left")
                                            {
                                                strExtraLine = strExtraLine + strBefore + strNextLineContant + strSubAfter;
                                                strExtraLine2 = strExtraLine2 + strBefore + strNextLineContant2 + strSubAfter2;
                                            }
                                            else if (currentRow["Align"].ToString() == "Right")
                                            {
                                                strExtraLine = strExtraLine + strBefore + strSubAfter + strNextLineContant;
                                                strExtraLine2 = strExtraLine2 + strBefore + strSubAfter2 + strNextLineContant2;
                                            }
                                            else
                                            {
                                                string strBeforeText, strBeforeText2, strAfterText, strAfterText2;
                                                strBeforeText = strSubAfter.Substring(0, strSubAfter.Length / 2);
                                                strBeforeText2 = strSubAfter2.Substring(0, strSubAfter2.Length / 2);
                                                if (strSubAfter.Length % 2 == 0)
                                                    strAfterText = strBeforeText;
                                                else
                                                    strAfterText = strBeforeText.Insert(0, " ");
                                                if (strSubAfter2.Length % 2 == 0)
                                                    strAfterText2 = strBeforeText2;
                                                else
                                                    strAfterText2 = strBeforeText2.Insert(0, " ");
                                                strExtraLine = strExtraLine + strBefore + strBeforeText + strNextLineContant + strAfterText;
                                                strExtraLine2 = strExtraLine2 + strBefore + strBeforeText2 + strNextLineContant2 + strAfterText2;
                                            }
                                            //-----------------
                                        }
                                        else
                                        {
                                            string strContantSpace = ""; ;
                                            string strCurrentContant = strCotant.Substring(0, inTextWidh);
                                            for (int b = 0; b < strCurrentContant.Length; b++)
                                                strContantSpace = strContantSpace + " ";
                                            strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                            strExtraLine2 = strExtraLine2 + strBefore + strContantSpace + strAfter;
                                        }
                                        strCotant = strCotant.Substring(0, inTextWidh);
                                    }
                                    //-------- Font Size-----------
                                    #region Give Font Size
                                    if (currentRow["dOrH"].ToString() == "Hedding")
                                        strCotant = RawPrinterHelper.Headder(strCotant);
                                    else if (currentRow["dOrH"].ToString() == "Bold")
                                        strCotant = RawPrinterHelper.Bold(strCotant);
                                    else if (currentRow["dOrH"].ToString() == "Italic")
                                        strCotant = RawPrinterHelper.Italic(strCotant);
                                    #endregion Give Font Size
                                    //-------- Font Size-----------

                                    //-------- Font Alignment-----------
                                    if (currentRow["Align"].ToString() == "Left")
                                        strLine = strLine + strBefore + strCotant + strAfter;
                                    else if (currentRow["Align"].ToString() == "Right")
                                        strLine = strLine + strBefore + strAfter + strCotant;
                                    else
                                    {
                                        string strBeforeText, strAfterText;
                                        strBeforeText = strAfter.Substring(0, strAfter.Length / 2);
                                        if (strAfter.Length % 2 == 0)
                                            strAfterText = strBeforeText;
                                        else
                                            strAfterText = strBeforeText.Insert(0, " ");
                                        strLine = strLine + strBefore + strAfterText + strCotant + strAfterText;
                                    }
                                    //-------- Font Alignment-----------
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (strLine != "")
                            {
                                sw.Write(strLine);
                                sw.WriteLine();
                                inHeight++;
                                inMasterHeight++;
                                isNextPage = false;
                                strLine = "";
                            }
                            if (strExtraLine.Trim() != "")
                            {
                                sw.Write(strExtraLine);
                                sw.WriteLine();
                                inHeight++;
                                inMasterHeight++;
                                strExtraLine = "";
                            }
                            if (strExtraLine2.Trim() != "")
                            {
                                sw.Write(strExtraLine2);
                                sw.WriteLine();
                                inHeight++;
                                inMasterHeight++;
                                strExtraLine2 = "";
                            }
                        }
                        else if (strRepeat == "true")
                        {
                            strExtraLine = "";
                            strExtraLine2 = "";
                            for (; inD < inCountD; inD++)
                            {
                                strLine = "";
                                for (int innerLoop = 0; innerLoop < inCount; innerLoop++)
                                {
                                    DataRow currentRow = drCurrent[innerLoop];
                                    int inRowPos = int.Parse(currentRow["row"].ToString());
                                    int inColPos = int.Parse(currentRow["columns"].ToString());
                                    int inTextWidh = int.Parse(currentRow["width"].ToString());
                                    string strCotant;
                                    if (currentRow["DBF"].ToString() != "")
                                        strCotant = dtblDetails.Rows[inD][currentRow["DBF"].ToString()].ToString();
                                    else if (currentRow["extraFieldName"].ToString() != "")
                                        strCotant = dtblDetails.Rows[inD][currentRow["extraFieldName"].ToString()].ToString();
                                    else
                                        strCotant = currentRow["text"].ToString();
                               
                                    string strBefore = "", strAfter = "";
                                    for (int c = 0; c < inColPos - strLine.Length; c++)
                                        strBefore = strBefore + " ";
                                    //-- Check Length grater than width---------
                                    if (inTextWidh >= strCotant.Length)
                                    {
                                        string strContantSpace = "";
                                        for (int a = 0; a < inTextWidh - strCotant.Length; a++)
                                            strAfter = strAfter + " ";

                                        for (int b = 0; b < strCotant.Length; b++)
                                            strContantSpace = strContantSpace + " ";
                                        strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                    }
                                    else
                                    {
                                        string strSubAfter = "", strSubAfter2 = "", strNextLineContant = "", strNextLineContant2 = "";

                                        if (currentRow["textWrap"].ToString().ToLower() == "true")
                                        {
                                            string strNextLineFull = strCotant.Substring(inTextWidh);
                                            strNextLineContant = strCotant.Substring(inTextWidh, inTextWidh);
                                            for (int a = 0; a < inTextWidh - strNextLineContant.Length; a++)
                                                strSubAfter = strSubAfter + " ";
                                            if (inTextWidh < strNextLineFull.Length && int.Parse(currentRow["wrapLineCount"].ToString()) > 1)
                                            {
                                                strNextLineContant2 = strNextLineFull.Substring(inTextWidh, strNextLineFull.Length - inTextWidh);
                                                for (int a = 0; a < inTextWidh - strNextLineContant2.Length; a++)
                                                    strSubAfter2 = strSubAfter2 + " ";
                                            }
                                            //-------- Font Size-----------
                                            #region Give Font Size
                                            if (currentRow["dOrH"].ToString() == "Hedding")
                                            {
                                                strNextLineContant = RawPrinterHelper.Headder(strNextLineContant);
                                                strNextLineContant2 = RawPrinterHelper.Headder(strNextLineContant2);
                                            }
                                            else if (currentRow["dOrH"].ToString() == "Bold")
                                            {
                                                strNextLineContant = RawPrinterHelper.Bold(strNextLineContant);
                                                strNextLineContant2 = RawPrinterHelper.Bold(strNextLineContant2);
                                            }
                                            else if (currentRow["dOrH"].ToString() == "Italic")
                                            {
                                                strNextLineContant = RawPrinterHelper.Italic(strNextLineContant);
                                                strNextLineContant2 = RawPrinterHelper.Italic(strNextLineContant2);
                                            }
                                            #endregion Give Font Size
                                            //-------- Font Size-----------

                                            //-------- Font Alignment-----------
                                            if (currentRow["Align"].ToString() == "Left")
                                            {
                                                strExtraLine = strExtraLine + strBefore + strNextLineContant + strSubAfter;
                                                strExtraLine2 = strExtraLine2 + strBefore + strNextLineContant2 + strSubAfter2;
                                            }
                                            else if (currentRow["Align"].ToString() == "Right")
                                            {
                                                strExtraLine = strExtraLine + strBefore + strSubAfter + strNextLineContant;
                                                strExtraLine2 = strExtraLine2 + strBefore + strSubAfter2 + strNextLineContant2;
                                            }
                                            else
                                            {
                                                string strBeforeText, strBeforeText2, strAfterText, strAfterText2;
                                                strBeforeText = strSubAfter.Substring(0, strSubAfter.Length / 2);
                                                strBeforeText2 = strSubAfter2.Substring(0, strSubAfter2.Length / 2);
                                                if (strSubAfter.Length % 2 == 0)
                                                    strAfterText = strBeforeText;
                                                else
                                                    strAfterText = strBeforeText.Insert(0, " ");

                                                if (strSubAfter2.Length % 2 == 0)
                                                    strAfterText2 = strBeforeText2;
                                                else
                                                    strAfterText2 = strBeforeText2.Insert(0, " ");
                                                strExtraLine = strExtraLine + strBefore + strBeforeText + strNextLineContant + strAfterText;
                                                strExtraLine2 = strExtraLine2 + strBefore + strBeforeText2 + strNextLineContant2 + strAfterText2;
                                            }
                                            //-----------------
                                        }
                                        else
                                        {
                                            string strContantSpace = ""; ;
                                            string strCurrentContant = strCotant.Substring(0, inTextWidh);
                                            for (int b = 0; b < strCurrentContant.Length; b++)
                                                strContantSpace = strContantSpace + " ";
                                            strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                            strExtraLine2 = strExtraLine2 + strBefore + strContantSpace + strAfter;
                                            //strExtraLine = "";
                                        }
                                        strCotant = strCotant.Substring(0, inTextWidh);
                                    }
                                    //-------- Font Size-----------
                                    #region Give Font Size
                                    if (currentRow["dOrH"].ToString() == "Hedding")
                                        strCotant = RawPrinterHelper.Headder(strCotant);
                                    else if (currentRow["dOrH"].ToString() == "Bold")
                                        strCotant = RawPrinterHelper.Bold(strCotant);
                                    else if (currentRow["dOrH"].ToString() == "Italic")
                                        strCotant = RawPrinterHelper.Italic(strCotant);
                                    #endregion Give Font Size
                                    //-------- Font Size-----------

                                    //-------- Font Alignment-----------
                                    if (currentRow["Align"].ToString() == "Left")
                                        strLine = strLine + strBefore + strCotant + strAfter;
                                    else if (currentRow["Align"].ToString() == "Right")
                                        strLine = strLine + strBefore + strAfter + strCotant;
                                    else
                                    {
                                        string strBeforeText, strAfterText;
                                        strBeforeText = strAfter.Substring(0, strAfter.Length / 2);
                                        if (strAfter.Length % 2 == 0)
                                            strAfterText = strBeforeText;
                                        else
                                            strAfterText = strBeforeText.Insert(0, " ");
                                        strLine = strLine + strBefore + strBeforeText + strCotant + strAfterText;
                                    }
                                    //-------- Font Alignment-----------
                                }
                                sw.Write(strLine);
                                sw.WriteLine();
                                inHeight++;
                                isNextPage = false;
                                if (inHeight - inMasterHeight >= frmPrintDesigner.inPageSizeInOther)
                                {
                                    isNextPage = true;
                                    break;
                                }
                                if (strExtraLine.Trim() != "")
                                {
                                    sw.Write(strExtraLine);
                                    sw.WriteLine();
                                    inHeight++;
                                    strExtraLine = "";
                                    isNextPage = false;
                                    if (inHeight - inMasterHeight >= frmPrintDesigner.inPageSizeInOther)
                                    {
                                        isNextPage = true;
                                        break;
                                    }
                                }
                                if (strExtraLine2.Trim() != "")
                                {
                                    sw.Write(strExtraLine2);
                                    sw.WriteLine();
                                    inHeight++;
                                    strExtraLine2 = "";
                                    isNextPage = false;
                                    if (inHeight - inMasterHeight >= frmPrintDesigner.inPageSizeInOther)
                                    {
                                        isNextPage = true;
                                        break;
                                    }
                                }
                            }
                            for (int inF = inMinFooter; inF <= inMaxFooter; inF++)
                            {
                                strLine = "";
                                int inFCount = int.Parse(dtbl.Compute("Count(row)", "row='" + inF.ToString() + "'").ToString());
                                DataRow[] drFCurrent = dtbl.Select("row='" + inF.ToString() + "'", "columns");
                                strExtraLine = "";
                                strExtraLine2 = "";
                                bool isHere = false;
                                for (int inForCheckingInner = 0; inForCheckingInner < inFCount; inForCheckingInner++)
                                {
                                    DataRow currentCheckingFRow = drFCurrent[inForCheckingInner];
                                    isHere = bool.Parse(currentCheckingFRow["FooterRepeatAll"].ToString());
                                    if (isHere)
                                        break;
                                    //else
                                      //  if (inD + 1 < inCountD)
                                        //    sw.WriteLine();   // null footer for other pages
                                }
                                if (isHere)
                                {
                                    if (frmPrintDesigner.strFooterLocation == "PageEnd")
                                    {
                                        
                                            if (isFisrtPFooter)
                                            {
                                                int w = inHeight - inMasterHeight;
                                                while (w < frmPrintDesigner.inPageSizeInFirst)
                                                {
                                                    sw.WriteLine();
                                                    w++;
                                                }
                                                isFisrtPFooter = false;
                                            }
                                        
                                    }
                                    else
                                    {
                                        if (isFirstFooter)
                                        {
                                            int w = 0;
                                            while (w < frmPrintDesigner.inBlankLineForFooter)
                                            {
                                                sw.WriteLine();
                                                w++;
                                            }
                                            isFirstFooter = false;
                                        }
                                    }
                                }
                                for (int inFInner = 0; inFInner < inFCount; inFInner++)
                                {
                                    DataRow currentFRow = drFCurrent[inFInner];
                                    if (bool.Parse(currentFRow["FooterRepeatAll"].ToString()))
                                    {
                                        int inRowPos = int.Parse(currentFRow["row"].ToString());
                                        int inColPos = int.Parse(currentFRow["columns"].ToString());
                                        int inTextWidh = int.Parse(currentFRow["width"].ToString());
                                        string strCotant;
                                        if (currentFRow["DBF"].ToString() != "")
                                            strCotant = dtblFooter.Rows[0][currentFRow["DBF"].ToString()].ToString();
                                        else if (currentFRow["extraFieldName"].ToString() != "")
                                            strCotant = dtblFooter.Rows[0][currentFRow["extraFieldName"].ToString()].ToString();
                                        else
                                            strCotant = currentFRow["text"].ToString();
                                        string strBefore = "", strAfter = "";
                                        for (int c = 0; c < inColPos - strLine.Length; c++)
                                            strBefore = strBefore + " ";

                                        if (inTextWidh >= strCotant.Length)
                                        {
                                            string strContantSpace = "";
                                            for (int a = 0; a < inTextWidh - strCotant.Length; a++)
                                                strAfter = strAfter + " ";
                                            for (int b = 0; b < strCotant.Length; b++)
                                                strContantSpace = strContantSpace + " ";
                                            strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                        }
                                        else
                                        {
                                            string strSubAfter = "", strSubAfter2 = "", strNextLineContant = "", strNextLineContant2 = "";

                                            if (currentFRow["textWrap"].ToString().ToLower() == "true")
                                            {
                                                string strNextLineFull = strCotant.Substring(inTextWidh, inTextWidh);
                                                strNextLineContant = strCotant.Substring(inTextWidh, inTextWidh);
                                                for (int a = 0; a < inTextWidh - strNextLineContant.Length; a++)
                                                    strSubAfter = strSubAfter + " ";
                                                if (inTextWidh < strNextLineFull.Length && int.Parse(currentFRow["wrapLineCount"].ToString()) > 1)
                                                {
                                                    strNextLineContant2 = strNextLineFull.Substring(inTextWidh, strNextLineFull.Length - inTextWidh);
                                                    for (int a = 0; a < inTextWidh - strNextLineContant2.Length; a++)
                                                        strSubAfter2 = strSubAfter2 + " ";
                                                }

                                                //-------- Font Size-----------
                                                #region Give Font Size
                                                if (currentFRow["dOrH"].ToString() == "Hedding")
                                                {
                                                    strNextLineContant = RawPrinterHelper.Headder(strNextLineContant);
                                                    strNextLineContant2 = RawPrinterHelper.Headder(strNextLineContant2);
                                                }
                                                else if (currentFRow["dOrH"].ToString() == "Bold")
                                                {
                                                    strNextLineContant = RawPrinterHelper.Bold(strNextLineContant);
                                                    strNextLineContant2 = RawPrinterHelper.Bold(strNextLineContant2);
                                                }
                                                else if (currentFRow["dOrH"].ToString() == "Italic")
                                                {
                                                    strNextLineContant = RawPrinterHelper.Italic(strNextLineContant);
                                                    strNextLineContant2 = RawPrinterHelper.Italic(strNextLineContant2);
                                                }
                                                #endregion Give Font Size
                                                //-------- Font Size-----------

                                                //-------- Font Alignment-----------
                                                if (currentFRow["Align"].ToString() == "Left")
                                                {
                                                    strExtraLine = strExtraLine + strBefore + strNextLineContant + strSubAfter;
                                                    strExtraLine2 = strExtraLine2 + strBefore + strNextLineContant2 + strSubAfter2;
                                                }
                                                else if (currentFRow["Align"].ToString() == "Right")
                                                {
                                                    strExtraLine = strExtraLine + strBefore + strSubAfter + strNextLineContant;
                                                    strExtraLine2 = strExtraLine2 + strBefore + strSubAfter2 + strNextLineContant2;
                                                }
                                                else
                                                {
                                                    string strBeforeText, strBeforeText2, strAfterText, strAfterText2;
                                                    strBeforeText = strSubAfter.Substring(0, strSubAfter.Length / 2);
                                                    strBeforeText2 = strSubAfter2.Substring(0, strSubAfter2.Length / 2);
                                                    if (strSubAfter.Length % 2 == 0)
                                                        strAfterText = strBeforeText;
                                                    else
                                                        strAfterText = strBeforeText.Insert(0, " ");
                                                    if (strSubAfter2.Length % 2 == 0)
                                                        strAfterText2 = strBeforeText2;
                                                    else
                                                        strAfterText2 = strBeforeText2.Insert(0, " ");
                                                    strExtraLine = strExtraLine + strBefore + strBeforeText + strNextLineContant + strAfterText;
                                                    strExtraLine2 = strExtraLine2 + strBefore + strBeforeText2 + strNextLineContant2 + strAfterText2;
                                                }
                                                //-----------------
                                            }
                                            else
                                            {
                                                string strContantSpace = ""; ;
                                                string strCurrentContant = strCotant.Substring(0, inTextWidh);
                                                for (int b = 0; b < strCurrentContant.Length; b++)
                                                    strContantSpace = strContantSpace + " ";
                                                strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                                strExtraLine2 = strExtraLine2 + strBefore + strContantSpace + strAfter;
                                                // strExtraLine = "";
                                            }
                                            strCotant = strCotant.Substring(0, inTextWidh);
                                        }
                                        //-------- Font Size-----------
                                        #region Give Font Size
                                        if (currentFRow["dOrH"].ToString() == "Hedding")
                                            strCotant = RawPrinterHelper.Headder(strCotant);
                                        else if (currentFRow["dOrH"].ToString() == "Bold")
                                            strCotant = RawPrinterHelper.Bold(strCotant);
                                        else if (currentFRow["dOrH"].ToString() == "Italic")
                                            strCotant = RawPrinterHelper.Italic(strCotant);
                                        #endregion Give Font Size
                                        //-------- Font Size-----------

                                        //-------- Font Alignment-----------
                                        if (currentFRow["Align"].ToString() == "Left")
                                            strLine = strLine + strBefore + strCotant + strAfter;
                                        else if (currentFRow["Align"].ToString() == "Right")
                                            strLine = strLine + strBefore + strAfter + strCotant;
                                        else
                                            strLine = strLine + strBefore + strAfter + strCotant;
                                        //-------- Font Alignment-----------
                                    }
                                }
                                if (strLine != "")
                                {
                                    sw.Write(strLine);
                                    sw.WriteLine();
                                    inHeight++;
                                    inMasterHeight++;
                                }
                                if (strExtraLine.Trim() != "")
                                {
                                    sw.Write(strExtraLine);
                                    sw.WriteLine();
                                    inHeight++;
                                    inMasterHeight++;
                                    strExtraLine = "";
                                }
                                if (strExtraLine2.Trim() != "")
                                {
                                    sw.Write(strExtraLine2);
                                    sw.WriteLine();
                                    inHeight++;
                                    inMasterHeight++;
                                    strExtraLine2 = "";
                                }
                            }
                        }
                        isNextPage = false;
                        if (inHeight - inMasterHeight >= frmPrintDesigner.inPageSizeInOther && inD<inCountD-1)
                        {
                            isNextPage = true;
                            int w = 0;
                            //while (frmPrintDesigner.inLineCountBetweenTwoPages > w)
                            //{
                            //    sw.WriteLine();
                            //    w++;
                            //}
                            if (inD + 1 < inCountD)
                            {
                                inloop = -1;
                                while (frmPrintDesigner.inLineCountBetweenTwoPages > w)
                                {
                                    sw.WriteLine();
                                    w++;
                                }
                                inHeight = 0;
                                inMasterHeight = 0;
                            }
                          
                            inD++;
                        }
                       
                    }

                }
                for (int inF = inMinFooter; inF <= inMaxFooter; inF++)
                {
                    string strLine = "";
                    int inFCount = int.Parse(dtbl.Compute("Count(row)", "row='" + inF.ToString() + "'").ToString());
                    DataRow[] drFCurrent = dtbl.Select("row='" + inF.ToString() + "'", "columns");
                    strExtraLine = "";
                    strExtraLine2 = "";
                    bool isHere = false;
                    for (int inForCheckingInner = 0; inForCheckingInner < inFCount; inForCheckingInner++)
                    {
                        DataRow currentCheckingFRow = drFCurrent[inForCheckingInner];
                        isHere = !bool.Parse(currentCheckingFRow["FooterRepeatAll"].ToString());
                        if (isHere)
                            break;
                    }
                    if (isHere)
                    {
                        if (frmPrintDesigner.strFooterLocation == "PageEnd")
                        {
                            if (inHeight - inMasterHeight < frmPrintDesigner.inPageSizeInOther)
                            {

                                if (isFisrtPFooter)
                                {
                                    int w = inHeight - inMasterHeight;
                                    while (w < frmPrintDesigner.inPageSizeInOther)
                                    {
                                        sw.WriteLine();
                                        w++;
                                    }
                                    isFisrtPFooter = false;
                                }
                            }
                        }
                        else
                        {
                            if (isFirstFooter)
                            {
                                int w = 0;
                                while (w < frmPrintDesigner.inBlankLineForFooter)
                                {
                                    sw.WriteLine();
                                    w++;
                                }
                                isFirstFooter = false;
                            }
                        }
                    }
                    for (int inFInner = 0; inFInner < inFCount; inFInner++)
                    {
                        DataRow currentFRow = drFCurrent[inFInner];
                        if (!bool.Parse(currentFRow["FooterRepeatAll"].ToString()))
                        {
                            int inRowPos = int.Parse(currentFRow["row"].ToString());
                            int inColPos = int.Parse(currentFRow["columns"].ToString());
                            int inTextWidh = int.Parse(currentFRow["width"].ToString());
                            string strCotant;
                            if (currentFRow["DBF"].ToString() != "")
                                strCotant = dtblFooter.Rows[0][currentFRow["DBF"].ToString()].ToString();
                            else if (currentFRow["extraFieldName"].ToString() != "")
                                strCotant = dtblFooter.Rows[0][currentFRow["extraFieldName"].ToString()].ToString();
                            else
                                strCotant = currentFRow["text"].ToString();
                            string strBefore = "", strAfter = "";
                            for (int c = 0; c < inColPos - strLine.Length; c++)
                                strBefore = strBefore + " ";

                            if (inTextWidh >= strCotant.Length)
                            {
                                string strContantSpace = "";
                                for (int a = 0; a < inTextWidh - strCotant.Length; a++)
                                    strAfter = strAfter + " ";
                                for (int b = 0; b < strCotant.Length; b++)
                                    strContantSpace = strContantSpace + " ";
                                strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                            }
                            else
                            {
                                string strSubAfter = "", strSubAfter2 = "", strNextLineContant = "", strNextLineContant2 = "";

                                if (currentFRow["textWrap"].ToString().ToLower() == "true")
                                {
                                    string strNextLineFull = strCotant.Substring(inTextWidh);
                                    strNextLineContant = strCotant.Substring(inTextWidh, inTextWidh);
                                    for (int a = 0; a < inTextWidh - strNextLineContant.Length; a++)
                                        strSubAfter = strSubAfter + " ";
                                    if (inTextWidh < strNextLineFull.Length && int.Parse(currentFRow["wrapLineCount"].ToString()) > 1)
                                    {
                                        strNextLineContant2 = strNextLineFull.Substring(inTextWidh, strNextLineFull.Length - inTextWidh);
                                        for (int a = 0; a < inTextWidh - strNextLineContant2.Length; a++)
                                            strSubAfter2 = strSubAfter2 + " ";
                                    }

                                    //-------- Font Size-----------
                                    #region Give Font Size
                                    if (currentFRow["dOrH"].ToString() == "Hedding")
                                    {
                                        strNextLineContant = RawPrinterHelper.Headder(strNextLineContant);
                                        strNextLineContant2 = RawPrinterHelper.Headder(strNextLineContant2);
                                    }
                                    else if (currentFRow["dOrH"].ToString() == "Bold")
                                    {
                                        strNextLineContant = RawPrinterHelper.Bold(strNextLineContant);
                                        strNextLineContant2 = RawPrinterHelper.Bold(strNextLineContant2);
                                    }
                                    else if (currentFRow["dOrH"].ToString() == "Italic")
                                    {
                                        strNextLineContant = RawPrinterHelper.Italic(strNextLineContant);
                                        strNextLineContant2 = RawPrinterHelper.Italic(strNextLineContant2);
                                    }
                                    #endregion Give Font Size
                                    //-------- Font Size-----------

                                    //-------- Font Alignment-----------
                                    if (currentFRow["Align"].ToString() == "Left")
                                    {
                                        strExtraLine = strExtraLine + strBefore + strNextLineContant + strSubAfter;
                                        strExtraLine2 = strExtraLine2 + strBefore + strNextLineContant2 + strSubAfter2;
                                    }
                                    else if (currentFRow["Align"].ToString() == "Right")
                                    {
                                        strExtraLine = strExtraLine + strBefore + strSubAfter + strNextLineContant;
                                        strExtraLine2 = strExtraLine2 + strBefore + strSubAfter2 + strNextLineContant2;
                                    }
                                    else
                                    {
                                        string strBeforeText, strBeforeText2, strAfterText, strAfterText2;
                                        strBeforeText = strSubAfter.Substring(0, strSubAfter.Length / 2);
                                        strBeforeText2 = strSubAfter2.Substring(0, strSubAfter2.Length / 2);
                                        if (strSubAfter.Length % 2 == 0)
                                            strAfterText = strBeforeText;
                                        else
                                            strAfterText = strBeforeText.Insert(0, " ");
                                        if (strSubAfter2.Length % 2 == 0)
                                            strAfterText2 = strBeforeText2;
                                        else
                                            strAfterText2 = strBeforeText2.Insert(0, " ");
                                        strExtraLine = strExtraLine + strBefore + strBeforeText + strNextLineContant + strAfterText;
                                        strExtraLine2 = strExtraLine2 + strBefore + strBeforeText2 + strNextLineContant2 + strAfterText2;
                                    }
                                    //-----------------
                                }
                                else
                                {
                                    string strContantSpace = ""; ;
                                    string strCurrentContant = strCotant.Substring(0, inTextWidh);
                                    for (int b = 0; b < strCurrentContant.Length; b++)
                                        strContantSpace = strContantSpace + " ";
                                    strExtraLine = strExtraLine + strBefore + strContantSpace + strAfter;
                                    strExtraLine2 = strExtraLine2 + strBefore + strContantSpace + strAfter;
                                    // strExtraLine = "";
                                }
                                strCotant = strCotant.Substring(0, inTextWidh);
                            }
                            //-------- Font Size-----------
                            #region Give Font Size
                            if (currentFRow["dOrH"].ToString() == "Hedding")
                                strCotant = RawPrinterHelper.Headder(strCotant);
                            else if (currentFRow["dOrH"].ToString() == "Bold")
                                strCotant = RawPrinterHelper.Bold(strCotant);
                            else if (currentFRow["dOrH"].ToString() == "Italic")
                                strCotant = RawPrinterHelper.Italic(strCotant);
                            #endregion Give Font Size
                            //-------- Font Size-----------

                            //-------- Font Alignment-----------
                            if (currentFRow["Align"].ToString() == "Left")
                                strLine = strLine + strBefore + strCotant + strAfter;
                            else if (currentFRow["Align"].ToString() == "Right")
                                strLine = strLine + strBefore + strAfter + strCotant;
                            else
                                strLine = strLine + strBefore + strAfter + strCotant;
                            //-------- Font Alignment-----------
                        }
                    }
                    if (strLine != "")
                    {
                        sw.Write(strLine);
                        sw.WriteLine();
                        inHeight++;
                        inMasterHeight++;
                    }
                    if (strExtraLine.Trim() != "")
                    {
                        sw.Write(strExtraLine);
                        sw.WriteLine();
                        inHeight++;
                        inMasterHeight++;
                        strExtraLine = "";
                    }
                    if (strExtraLine2.Trim() != "")
                    {
                        sw.Write(strExtraLine2);
                        sw.WriteLine();
                        inHeight++;
                        inMasterHeight++;
                        strExtraLine2 = "";
                    }
                }
                int x=0;
                while (x < frmPrintDesigner.inLineCountAfterPrint)
                {
                    sw.WriteLine();
                    x++;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                sw.Close();
            }
            RawPrinterHelper.SendFileToPrinter(pd.PrinterSettings.PrinterName, Application.StartupPath + "\\Print.txt");
        }
    }
}