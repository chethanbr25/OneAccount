using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Data;


namespace Open_Miracle
{
    public class RawPrinterHelper
    {
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);
        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.
            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";
            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            // Open the file.
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            // Create a BinaryReader on the file.
            BinaryReader br = new BinaryReader(fs);
            // Dim an array of bytes big enough to hold the file's contents.
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            // Read the contents of the file into the array.
            bytes = br.ReadBytes(nLength);
            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // Send the unmanaged bytes to the printer.
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }
        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
        public static string LeftAlign(int length, string field)
        {
            if (field.Trim().Length > length)
            {
                field = field.Trim().Remove(length);
            }
            string retField = "";
            for (int i = 0; i <= length - field.Trim().Length; i++)
            {
                retField = retField + " ";
            }
            retField = field.Trim() + retField;
            return retField;
        }
        public static string RightAlign(int length, string field)
        {
            if (field.Trim().Length > length)
            {
                field = field.Remove(length);
            }
            string retField = "";
            int x = field.Trim().Length;
            for (int i = 0; i <= length - field.Trim().Length; i++)
            {
                retField = retField + " ";
            }
            retField = retField + field;
            return retField;
        }
      
        public static string CentreAlign(int length, string field)
        {
            if (field.Trim().Length > length)
            {
                field = field.Remove(length);
            }
            string retField = "";
            for (int i = 0; i <= length - field.Trim().Length; i++)
            {
                retField = retField + " ";
            }
            retField = retField.Remove(retField.Length / 2);
            retField = retField + field + retField;
            return retField;
        }
        private static String ItalicsOn()
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)52);
            return sequence.ToString();
        }
        private static String ItalicsOff()
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)53);
            return sequence.ToString();
        }
        public static String BoldOn()
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)69);
            return sequence.ToString();
        }
        public static String BoldOff()
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)70);
            return sequence.ToString();
        }
        private static String EnlargeOn()
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)87);
            sequence.Append((char)49);
            return sequence.ToString();
        }
        private static String EnlargeOff()
        {
            StringBuilder sequence = new StringBuilder();
            sequence.Append((char)27);
            sequence.Append((char)87);
            sequence.Append((char)48);
            return sequence.ToString();
        }
        public static void CentreText(StreamWriter sw, string Headder, int Length)
        {
            sw.Write(CentreAlign(Length, Headder));
        }
        public static void LeftText(StreamWriter sw, string Headder, int Length)
        {
            sw.Write(LeftAlign(Length, Headder));
        }
        public static void RightText(StreamWriter sw, string Headder, int Length)
        {
            sw.Write(RightAlign(Length, Headder));
        }
       
        public static string Headder( string Headder)
        {
           return  EnlargeOn() + BoldOn() + Headder + BoldOff() + EnlargeOff();
        }
        public static string Bold(string Headder)
        {
            return  BoldOn() + Headder + BoldOff() ;
        }
        public static string Italic(string Headder)
        {
            return ItalicsOn() + Headder + ItalicsOff();
        }
       
        public static void Headder1(StreamWriter sw, string Headder, int Length)
        {
            sw.Write(CentreAlign(Length, BoldOn() + Headder + BoldOff()));
        }
        public static void Italics(StreamWriter sw, string Headder, int Length)
        {
            sw.Write(CentreAlign(Length, ItalicsOn() + Headder + ItalicsOff()));
        }
        
        public static void WriteLine(StreamWriter sw, string Char, int Length)
        {
            for (int i = 0; i <= Length; i++)
            {
                sw.Write(Char);
            }
        }
       
        public static int GetLength(DataTable dt)
        {
            int length = 0;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                length = length + dt.Columns[i].ColumnName.Length;
            }

            for (int i = 0; i < dt.Columns.Count - 1; i++)
            {
                length = length + 3;
            }
            return length;
        }       
    
        #region DOT MATRIX
        public static void WriteColumnHedrDotMatrix(StreamWriter sw, DataTable dt)
        {
            int i = 0;
            for (i = 0; i < dt.Columns.Count; i++)
            {
                sw.Write(dt.Columns[i].ColumnName + "   ");
            }
            //sw.Write(dt.Columns[i].ColumnName);
        }
        public static void WriteTableDataDotMatrix(StreamWriter sw, DataTable dt, string fortrue, string forfalse)
        {
            string leng = null;
            int i = 0;
            string strItemName = "";
            int inItemLength = 0;
            foreach (DataRow row in dt.Rows)
            {
                object[] array = row.ItemArray;
                int inLen = 0;
                int index = dt.Rows.IndexOf(row);
                if (dt.Rows.IndexOf(row) == dt.Rows.Count - 1)//&& dt.Columns.Contains(" QTY ") && dt.Rows[0][" QTY "].ToString().Trim() != "")
                {
                    if (dt.Rows.Count < 3)
                    {
                        sw.WriteLine();
                        sw.WriteLine();
                    }
                    int length = RawPrinterHelper.GetLength(dt);
                    WriteLine(sw, "-", length);
                    sw.WriteLine();
                }
                for (i = 0; i < array.Length - 1; i++)
                {
                    leng = null;

                    if (dt.Columns[i].DataType.ToString() == "System.String"
                     || dt.Columns[i].DataType.ToString() == "System.Decimal"
                     || dt.Columns[i].DataType.ToString() == "System.Int32")
                    {
                        for (int j = 0; j < dt.Columns[i].ColumnName.Length - array[i].ToString().Length; j++)
                        {
                            leng = leng + " ";
                        }
                    }
                    if (dt.Columns[i].DataType.ToString() == "System.String" || dt.Columns[i].DataType.ToString() == "System.Int32")
                    {
                        string str = array[i].ToString();
                       
                        inLen = 0;
                        if (i <= 1)
                        {
                            inLen = dt.Columns[i].ColumnName.Length;

                        }
                        else
                        {
                            inLen = dt.Columns[i].ColumnName.Length + 2;

                        }
                        if (dt.Columns[i].ColumnName.Trim() == "COMMODITY / ITEM" || dt.Columns[i].ColumnName.Trim() == "COMMODITY/ITEM" || dt.Columns[i].ColumnName.Trim() == "Account Ledger" || dt.Columns[i].ColumnName.Trim() == "ITEM" || dt.Columns[i].ColumnName == "Commodity Item")
                            sw.Write("  " + LeftAlign(inLen, str) + " ");

                        else
                            sw.Write(RightAlign(inLen, str).Substring(1) + " ");//+ leng + "   "
                    }
                    else if (dt.Columns[i].DataType.ToString() == "System.Decimal")
                    {
                        string str = array[i].ToString();
                        if (array[i].ToString().Length > dt.Columns[i].ColumnName.Length)
                        {
                            str = array[i].ToString().Substring(0, dt.Columns[i].ColumnName.Length);
                        }
                        sw.Write(leng + str + "   ");
                    }
                    else if (dt.Columns[i].DataType.ToString() == "System.Boolean")
                    {

                        if (bool.Parse(array[i].ToString()) == true)
                        {
                            string str = array[i].ToString();
                            if (array[i].ToString().Length > fortrue.Length)
                            {
                                str = array[i].ToString().Substring(0, fortrue.Length);
                            }
                            sw.Write(fortrue + leng + "   ");
                        }
                        else
                        {
                            string str = array[i].ToString();
                            if (array[i].ToString().Length > forfalse.Length)
                            {
                                str = array[i].ToString().Substring(0, forfalse.Length);
                            }
                            sw.Write(forfalse + leng + "   ");
                        }
                    }
                }
                if (dt.Columns.Count > 0)
                {
                    inLen = dt.Columns[i].ColumnName.Length;
                        sw.Write(RightAlign(inLen, array[i].ToString()).Substring(1));
                }
                if (strItemName != "")
                {
                    sw.WriteLine();
                    if (dt.Columns.Contains("SL NO"))
                    {
                        sw.Write(RightAlign(6, "") + " ");//+ leng + "   "

                    }
                    if (""=="")
                    {
                        sw.Write(RightAlign(4, "") + " ");//+ leng + "   "

                    }
                    sw.Write(LeftAlign(strItemName.Length, strItemName) + " ");
                    strItemName = "";
                }
                //sw.Write(array[i].ToString());
                sw.WriteLine();
            }
        }
              #endregion
        public static void printWithLocation(StreamWriter sw, string text, int column, int width)
        {
            try
            {
                string strBefore = "", strAfter="";
                for (int c = 0; c < column; c++)
                    strBefore = strBefore + " ";
                for (int a = 0; a < width-text.Length; a++)
                     strAfter= strAfter + " ";
                 sw.Write(strBefore + text + strAfter);
            }
            catch(Exception ex)
            {
              
            }
        }
    }
}
