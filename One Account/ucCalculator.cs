using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace One_Account
{
    public partial class ucCalculator : UserControl
    {
        bool blResult = false;
        char chPreviousOperator = '\0';
        double dblResult = 0;
        int[] inEquals = { 13, 61 };
        int[] inOperators = { 42, 43, 45, 47 };
        public ucCalculator()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text != string.Empty && !inOperators.Contains((int)txtDisplay.Text[txtDisplay.TextLength - 1]))
            {
                if (!(txtDisplay.Lines[txtDisplay.Lines.Length - 1] == string.Empty && blResult == false))
                {
                    if (blResult)
                    {
                        txtDisplay.SelectionStart -= 2;
                        txtDisplay.SelectionLength = 2;
                        txtDisplay.SelectedText = "";
                    }
                    txtDisplay.SelectionStart -= txtDisplay.Lines[txtDisplay.Lines.Length - 1].Length;
                    if (txtDisplay.Lines[txtDisplay.Lines.Length - 1].Contains('-'))
                    {
                        txtDisplay.SelectionLength = 1;
                        txtDisplay.SelectedText = "";
                    }
                    else
                        txtDisplay.SelectedText = "-";
                    txtDisplay.SelectionStart = txtDisplay.TextLength;
                }
            }
            this.ActiveControl = txtDisplay;
        }

        private void ClearAll(object sender, EventArgs e)
        {
            blResult = false;
            chPreviousOperator = '\0';
            dblResult = 0;
            txtDisplay.Text = "";
            this.ActiveControl = txtDisplay;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text != "" && txtDisplay.Lines[txtDisplay.Lines.Length - 1] != "")
            {
                if (inOperators.Contains((int)txtDisplay.Text[txtDisplay.TextLength - 1]) && txtDisplay.Lines[txtDisplay.Lines.Length - 1] != "-") { }
                else
                    txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.TextLength - txtDisplay.Lines[txtDisplay.Lines.Length - 1].Length);
                txtDisplay.SelectionStart = txtDisplay.TextLength;
            }
            this.ActiveControl = txtDisplay;
        }

        private void txtDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue > 36 && e.KeyValue < 41)
                e.Handled = true;
        }

        private void txtDisplay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !inEquals.Contains((int)e.KeyChar) && !inOperators.Contains(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != 8)
                e.Handled = true;
            else if (txtDisplay.SelectionLength != 0) { }
            else
            {
                if (inOperators.Contains(e.KeyChar))
                {
                    if (txtDisplay.Lines[txtDisplay.Lines.Length - 1] == "-")
                        txtDisplay.SelectedText = "0.0";
                    if (blResult)
                    {
                        txtDisplay.SelectionStart -= 2;
                        txtDisplay.SelectionLength = 2;
                        txtDisplay.SelectedText = "";
                        txtDisplay.SelectedText = e.KeyChar.ToString();
                    }
                    blResult = false;
                    if (txtDisplay.Text != "" && txtDisplay.Lines[txtDisplay.Lines.Length - 1] == ".")
                    {
                        txtDisplay.SelectionStart -= 1;
                        txtDisplay.SelectionLength = 1;
                        txtDisplay.SelectedText = "0.0";
                    }
                    else if (txtDisplay.Text != "" && txtDisplay.Text[txtDisplay.Text.Length - 1] == '.')
                        txtDisplay.SelectedText = "0";
                    if (txtDisplay.Text == "" || txtDisplay.Lines[txtDisplay.Lines.Length - 1] == string.Empty) e.Handled = true;
                    else if (txtDisplay.Text != "" && inOperators.Contains(txtDisplay.Text[txtDisplay.TextLength - 1]))
                    {
                        txtDisplay.SelectionStart -= 1;
                        txtDisplay.SelectionLength = 1;
                        chPreviousOperator = e.KeyChar;
                    }
                    else if (chPreviousOperator == '\0')
                    {
                        dblResult = double.Parse(txtDisplay.Lines[txtDisplay.Lines.Length - 1]);
                        chPreviousOperator = e.KeyChar;
                    }
                    else
                    {
                        DoOperation();
                        chPreviousOperator = e.KeyChar;
                    }

                }
                else if (char.IsDigit(e.KeyChar) || e.KeyChar == '.')
                {
                    if (txtDisplay.Text != "" && txtDisplay.Lines[txtDisplay.Lines.Length - 1] == "-") { }
                    else if (txtDisplay.Text != "" && (inOperators.Contains((int)txtDisplay.Text[txtDisplay.TextLength - 1])) || blResult)
                        txtDisplay.SelectedText = "\r\n";
                    if (txtDisplay.Text != "" && e.KeyChar == '.' && txtDisplay.Lines[txtDisplay.Lines.Length - 1].Contains('.'))
                        e.Handled = true;
                    blResult = false;

                }
                else if (inEquals.Contains(e.KeyChar))
                {
                    if (txtDisplay.Text == "") e.Handled = true;
                    else
                    {
                        if (chPreviousOperator != '\0' && txtDisplay.Lines[txtDisplay.Lines.Length - 1] == "-")
                            txtDisplay.SelectedText = "0.0";
                        else if (chPreviousOperator != '\0' && txtDisplay.Lines[txtDisplay.Lines.Length - 1] == ".")
                        {
                            txtDisplay.SelectionStart -= 1;
                            txtDisplay.SelectionLength = 1;
                            txtDisplay.SelectedText = "0.0";
                        }
                        else if (txtDisplay.Text != "" && txtDisplay.Lines[txtDisplay.Lines.Length - 1] == ".")
                        {
                            txtDisplay.SelectionStart -= 1;
                            txtDisplay.SelectionLength = 1;
                            txtDisplay.SelectedText = "0.";
                        }
                        else if (txtDisplay.Text != "" && chPreviousOperator != '\0' && txtDisplay.Text[txtDisplay.Text.Length - 1] == '.')
                            txtDisplay.SelectedText = "0";
                        if (chPreviousOperator == '\0') e.Handled = true;
                        else if (!inOperators.Contains(txtDisplay.Text[txtDisplay.TextLength - 1]))
                        {
                            DoOperation();
                            txtDisplay.SelectedText = " =\r\n_______________\r\n" + dblResult;
                            blResult = true;
                            chPreviousOperator = '\0';
                        }
                        else
                        {
                            txtDisplay.SelectionStart -= 1;
                            txtDisplay.SelectionLength = 1;
                            txtDisplay.SelectedText = "";
                            txtDisplay.SelectedText = " =\r\n_______________\r\n" + dblResult;
                            blResult = true;
                            chPreviousOperator = '\0';
                        }
                    }
                }
                else if (e.KeyChar == 8)
                {
                    if (txtDisplay.SelectionLength == txtDisplay.Text.Length)
                        button3.PerformClick();
                    if (txtDisplay.Text != "" && (txtDisplay.Lines[txtDisplay.Lines.Length - 1] == "" || (inOperators.Contains((int)txtDisplay.Text[txtDisplay.TextLength - 1]) && txtDisplay.Lines[txtDisplay.Lines.Length - 1] != "-")))
                        e.Handled = true;
                }
            }
        }
        void DoOperation()
        {
            switch (chPreviousOperator)
            {
                case '+':
                    dblResult = dblResult + double.Parse(txtDisplay.Lines[txtDisplay.Lines.Length - 1]);
                    break;
                case '-':
                    dblResult = dblResult - double.Parse(txtDisplay.Lines[txtDisplay.Lines.Length - 1]);
                    break;
                case '*':
                    dblResult = dblResult * double.Parse(txtDisplay.Lines[txtDisplay.Lines.Length - 1]);
                    break;
                case '/':
                    dblResult = dblResult / double.Parse(txtDisplay.Lines[txtDisplay.Lines.Length - 1]);
                    break;

                default:
                    break;
            }
        }

        private void txtDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            txtDisplay.Select(txtDisplay.Text.Length, +1);
            txtDisplay.SelectionLength = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}

