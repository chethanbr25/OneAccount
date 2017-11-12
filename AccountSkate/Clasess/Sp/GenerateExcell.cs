using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AccountSkate
{
    class GenerateExcell
    {
        Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
        Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;
        Microsoft.Office.Interop.Excel.Range formatRange;

        public void GenerateExcellWithData(System.Windows.Controls.DataGrid dgExport, string path, Microsoft.Office.Interop.Excel.Range fontRange)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
            int ind = 1;
            dgExport.Dispatcher.Invoke((Action)(() =>
    {
        foreach (object ob in dgExport.Columns.Select(cs => cs.Header).ToList())
        {
            xlWorkSheet.Cells[1, ind] = ob.ToString();
            ind++;
        }
    }));

            ind = 2;
            foreach (System.Data.DataRowView item in dgExport.Items)
            {
                for (int i = 0; i < dgExport.Columns.Count; i++)
                {
                    xlWorkSheet.Cells[ind, i + 1] = item[i].ToString();
                }
                ind++;
            }
            xlWorkSheet.Columns.AutoFit();


            if (!string.IsNullOrEmpty(path))
            {
                int countColumns = xlWorkSheet.UsedRange.Columns.Count;
                fontRange = xlWorkSheet.Range[xlWorkSheet.Cells[1, 1], xlWorkSheet.Cells[1, countColumns]];

                fontRange.Font.Bold = true;
                fontRange.EntireColumn.ColumnWidth = 30;
                fontRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                xlWorkBook.SaveAs(path, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Saved = true;
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Export to excel is Sucessfully done").ShowDialog()));
            }
        }

        private bool Opendialog()
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel is not installed").ShowDialog()));
                return false;
            }
            else
            {
                FolderBrowserDialog folderbrowser = new FolderBrowserDialog();
                folderbrowser.ShowNewFolderButton = true;
                if (folderbrowser.ShowDialog() == DialogResult.OK)
                {
                    //   path = folderbrowser.SelectedPath;
                    return true;
                }
            }
            return false;
        }

        private void SetColour(string a, bool Colour)
        {
            formatRange = xlWorkSheet.get_Range(a);
            formatRange.Font.Bold = true;
            formatRange.EntireColumn.ColumnWidth = 30;
            formatRange.Font.Color = (Colour == true) ? System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red) : System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
            formatRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        }

        public void Customer(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Opening balance";
                SetColour("a1", false);
                xlWorkSheet.Cells[1, 2] = "Bill by Bill";
                SetColour("b1", false);
                xlWorkSheet.Cells[1, 3] = "Credit Limit";
                SetColour("c1", false);
                xlWorkSheet.Cells[1, 4] = "Credit Period";
                SetColour("d1", false);
                xlWorkSheet.Cells[1, 5] = "Customer Name";
                SetColour("e1", true);
                xlWorkSheet.Cells[1, 6] = "Branch Name";
                SetColour("f1", false);
                xlWorkSheet.Cells[1, 7] = "Mobile";
                SetColour("g1", false);
                xlWorkSheet.Cells[1, 8] = "Address";
                SetColour("h1", false);
                xlWorkSheet.Cells[1, 9] = "CST";
                SetColour("i1", false);
                xlWorkSheet.Cells[1, 10] = "Area";
                SetColour("j1", false);
                xlWorkSheet.Cells[1, 11] = "Route";
                SetColour("k1", false);
                xlWorkSheet.Cells[1, 12] = "Mailing Name";
                SetColour("l1", false);
                xlWorkSheet.Cells[1, 13] = "Account Number";
                SetColour("m1", false);
                xlWorkSheet.Cells[1, 14] = "Branch code";
                SetColour("n1", false);
                xlWorkSheet.Cells[1, 15] = "Phone";
                SetColour("o1", false);
                xlWorkSheet.Cells[1, 16] = "E-mail";
                SetColour("p1", false);
                xlWorkSheet.Cells[1, 17] = "Pricing level";
                SetColour("q1", false);
                xlWorkSheet.Cells[1, 18] = "TIN";
                SetColour("r1", false);
                xlWorkSheet.Cells[1, 19] = "PAN";
                SetColour("s1", false);
                xlWorkSheet.Cells[1, 20] = "Narration";
                SetColour("t1", false);
                xlWorkSheet.Cells[1, 21] = "CrorDr";
                SetColour("u1", false);
                xlWorkBook.SaveAs(path + @"\Customer.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file path" + path + "->Customer.xls").ShowDialog()));
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Exception Occured while releasing object " + ex.ToString()).ShowDialog()));
            }
            finally
            {
                GC.Collect();
            }
        }

        public void Supplier(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Opening balance";
                SetColour("a1", false);
                xlWorkSheet.Cells[1, 2] = "Bill by Bill";
                SetColour("b1", false);
                xlWorkSheet.Cells[1, 3] = "Credit Limit";
                SetColour("c1", false);
                xlWorkSheet.Cells[1, 4] = "Credit Period";
                SetColour("d1", false);
                xlWorkSheet.Cells[1, 5] = "Supplier Name";
                SetColour("e1", true);
                xlWorkSheet.Cells[1, 6] = "Branch Name";
                SetColour("f1", false);
                xlWorkSheet.Cells[1, 7] = "Mobile";
                SetColour("g1", false);
                xlWorkSheet.Cells[1, 8] = "Address";
                SetColour("h1", false);
                xlWorkSheet.Cells[1, 9] = "CST";
                SetColour("i1", false);
                xlWorkSheet.Cells[1, 10] = "Area";
                SetColour("j1", false);
                xlWorkSheet.Cells[1, 11] = "Route";
                SetColour("k1", false);
                xlWorkSheet.Cells[1, 12] = "Mailing Name";
                SetColour("l1", false);
                xlWorkSheet.Cells[1, 13] = "Account Number";
                SetColour("m1", false);
                xlWorkSheet.Cells[1, 14] = "Branch code";
                SetColour("n1", false);
                xlWorkSheet.Cells[1, 15] = "Phone";
                SetColour("o1", false);
                xlWorkSheet.Cells[1, 16] = "E-mail";
                SetColour("p1", false);
                xlWorkSheet.Cells[1, 17] = "Pricing level";
                SetColour("q1", false);
                xlWorkSheet.Cells[1, 18] = "TIN";
                SetColour("r1", false);
                xlWorkSheet.Cells[1, 19] = "PAN";
                SetColour("s1", false);
                xlWorkSheet.Cells[1, 20] = "Narration";
                SetColour("t1", false);
                xlWorkSheet.Cells[1, 21] = "CrorDr";
                SetColour("u1", false);
                xlWorkBook.SaveAs(path + @"\Supplier-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file " + path + "Supplier-Excel.xls").ShowDialog()));
            }
        }

        public void AccountGroup(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Account Group Name";
                SetColour("a1", true);
                xlWorkSheet.Cells[1, 2] = "Under";
                SetColour("b1", true);
                xlWorkSheet.Cells[1, 3] = "Narration";
                SetColour("c1", false);
                xlWorkSheet.Cells[1, 4] = "Affect Gross Profit";
                SetColour("d1", false);
                xlWorkBook.SaveAs(path + @"\AccountGroup-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file " + path + "->AccountGroup-Excel.xls").ShowDialog()));
            }
        }

        public void PricingLevel(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                    xlWorkBook = xlApp.Workbooks.Add(misValue);
                    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                    xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                    xlWorkSheet.Cells[1, 1] = "Pricing Level Name";
                    SetColour("a1", true);
                    xlWorkSheet.Cells[1, 2] = "Narration";
                    SetColour("b1", false);
                    xlWorkBook.SaveAs(path + @"\PricingLevel-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();
                    releaseObject(xlWorkSheet);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created. \n You can find the file " + path + "PricingLevel-Excel.xls").ShowDialog()));
                }
                catch (Exception)
                { }

            }
        }

        public void ProductGroup(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Group Name";
                SetColour("a1", true);
                xlWorkSheet.Cells[1, 2] = "Group Under";
                SetColour("b1", false);
                xlWorkSheet.Cells[1, 3] = "Narration";
                SetColour("c1", false);
                xlWorkBook.SaveAs(path + @"\ProductGroup-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file " + path + "->ProductGroup-Excel.xls").ShowDialog()));
            }
        }

        public void AccountLeger(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Account Ledger Name";
                SetColour("a1", true);
                xlWorkSheet.Cells[1, 2] = "Account Group";
                SetColour("b1", true);
                xlWorkSheet.Cells[1, 3] = "Opening Balance";
                SetColour("c1", false);
                xlWorkSheet.Cells[1, 4] = "Narration";
                SetColour("d1", false);
                xlWorkSheet.Cells[1, 5] = "CrorDr";
                SetColour("e1", false);
                xlWorkBook.SaveAs(path + @"\AccountLeger-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file " + path + "->AccountLeger-Excel.xls").ShowDialog()));
            }
        }

        public void Unit(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Unit Name";
                SetColour("a1", true);
                xlWorkSheet.Cells[1, 2] = "Formal Name";
                SetColour("b1", true);
                xlWorkSheet.Cells[1, 3] = "No Of Decimal Places";
                SetColour("c1", true);
                xlWorkSheet.Cells[1, 4] = "Narration";
                SetColour("d1", false);
                xlWorkBook.SaveAs(path + @"\unit-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file" + path + "unit-Excel.xls").ShowDialog()));
            }
        }

        public void Godown(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Goddown Name";
                SetColour("a1", true);
                xlWorkSheet.Cells[1, 2] = "Narration";
                SetColour("b1", false);
                xlWorkBook.SaveAs(path + @"\Godown-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file" + path + "->Godown-Excel.xls").ShowDialog()));
            }
        }

        public void Product(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Product Code";
                SetColour("a1", true);
                xlWorkSheet.Cells[1, 2] = "Product Name";
                SetColour("b1", true);
                xlWorkSheet.Cells[1, 3] = "Group";
                SetColour("c1", true);
                xlWorkSheet.Cells[1, 4] = "Tax";
                SetColour("d1", false);
                xlWorkSheet.Cells[1, 5] = "Unit";
                SetColour("e1", true);
                xlWorkSheet.Cells[1, 6] = "Tax Applicable On";
                SetColour("f1", false);
                xlWorkSheet.Cells[1, 7] = "Default Godown";
                SetColour("g1", false);
                xlWorkSheet.Cells[1, 8] = "Rack";
                SetColour("h1", false);
                xlWorkSheet.Cells[1, 9] = "Allow Batch";
                SetColour("i1", false);
                xlWorkSheet.Cells[1, 10] = "Minimum Stock";
                SetColour("j1", false);
                xlWorkSheet.Cells[1, 11] = "Conversion Rate";
                SetColour("k1", false);
                xlWorkSheet.Cells[1, 12] = "Reorder Level";
                SetColour("l1", false);
                xlWorkSheet.Cells[1, 13] = "Multiple Unit";
                SetColour("m1", false);
                xlWorkSheet.Cells[1, 14] = "Opening Stock";
                SetColour("n1", false);
                xlWorkSheet.Cells[1, 15] = "Narration";
                SetColour("o1", false);
                xlWorkSheet.Cells[1, 16] = "Purchase Rate";
                SetColour("p1", false);
                xlWorkSheet.Cells[1, 17] = "Maximum Stock";
                SetColour("q1", false);
                xlWorkSheet.Cells[1, 18] = "MRP";
                SetColour("r1", false);
                xlWorkSheet.Cells[1, 19] = "Sales Rate";
                SetColour("s1", false);
                xlWorkBook.SaveAs(path + @"\Product-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file" + path + "->Product-Excel.xls")));
            }
        }

        public void Batch(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Product Name";
                SetColour("a1", true);
                xlWorkSheet.Cells[1, 2] = "MFG Date";
                SetColour("b1", false);
                Microsoft.Office.Interop.Excel.Range rg = formatRange.Cells[1, 1];
                rg.EntireColumn.NumberFormat = "dd mmm yyyy";
                xlWorkSheet.Cells[1, 3] = "Batch Name";
                SetColour("c1", true);
                Microsoft.Office.Interop.Excel.Range rg1 = formatRange.Cells[1, 2];
                rg1.EntireColumn.NumberFormat = "dd mmm yyyy";
                xlWorkSheet.Cells[1, 4] = "Expiry Date";
                SetColour("d1", false);
                xlWorkSheet.Cells[1, 5] = "Narration";
                SetColour("e1", false);
                xlWorkBook.SaveAs(path + @"\Batch-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file" + path + "->Batch-Excel.xls")));
            }
        }

        public void Currency(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Currency Name";
                SetColour("a1", true);
                xlWorkSheet.Cells[1, 2] = "Currency Symbol";
                SetColour("b1", true);
                xlWorkSheet.Cells[1, 3] = "No of Decimal Place";
                SetColour("c1", true);
                xlWorkSheet.Cells[1, 4] = "Narration";
                SetColour("d1", false);
                xlWorkSheet.Cells[1, 5] = "Sub Unit Name";
                SetColour("e1", false);
                xlWorkBook.SaveAs(path + @"\Currency-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file" + path + "->Currency-Excel.xls")));
            }

        }

        public void Vochertype(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Name";
                SetColour("a1", true);
                xlWorkSheet.Cells[1, 2] = "Type of Voucher";
                SetColour("b1", true);
                xlWorkSheet.Cells[1, 3] = "Method Of Voucher Numbering";
                SetColour("c1", true);
                xlWorkSheet.Cells[1, 4] = "Narration";
                SetColour("d1", false);
                xlWorkSheet.Cells[1, 5] = "Declaration";
                SetColour("e1", false);
                xlWorkSheet.Cells[1, 6] = "Active";
                SetColour("f1", false);
                xlWorkSheet.Cells[1, 7] = "Dot Matrix Print Format";
                SetColour("g1", true);
                xlWorkBook.SaveAs(path + @"\VoucherType-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file" + path + "->VoucherType-Excel.xls")));
            }
        }

        public void SalesMan(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Salesman code";
                SetColour("a1", true);
                xlWorkSheet.Cells[1, 2] = "Name";
                SetColour("b1", true);
                xlWorkSheet.Cells[1, 3] = "E-mail";
                SetColour("c1", false);
                xlWorkSheet.Cells[1, 4] = "Phone";
                SetColour("d1", false);
                xlWorkSheet.Cells[1, 5] = "Mobile";
                SetColour("e1", false);
                xlWorkSheet.Cells[1, 6] = "Address";
                SetColour("f1", false);
                xlWorkSheet.Cells[1, 7] = "Narration";
                SetColour("g1", false);
                xlWorkSheet.Cells[1, 8] = "Active";
                SetColour("h1", false);

                xlWorkBook.SaveAs(path + @"\Salesman-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file " + path + "->Salesman-Excel.xls")));
            }
        }

        public void Stock(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application(); //Intialize the excel object
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells.EntireColumn.NumberFormat = "@";
                xlWorkSheet.Cells[1, 1] = "Name";
                SetColour("a1", true);
                xlWorkSheet.Cells[1, 2] = "Closing Values";
                SetColour("b1", false);
                xlWorkSheet.Cells[1, 3] = "Opening Stock Number";
                SetColour("c1", false);
                xlWorkSheet.Cells[1, 4] = "Base Units";
                SetColour("d1", false);
                xlWorkBook.SaveAs(path + @"\Stock-Excel.xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => new wdoMessageBox("Excel file created.  \nYou can find the file" + path + "->Stock-Excel.xls")));
            }
        }
    }
}
