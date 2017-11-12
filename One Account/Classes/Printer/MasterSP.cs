using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace Open_Miracle
{
    class MasterSPrint : DBConnection
    {
        public int MasterAdd(MasterInfo infoMaster)
        {
            int retunvalue = 0;
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("MasterAdd", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@formName", SqlDbType.Int).Value = infoMaster.FormName;
                sqlcmd.Parameters.Add("@isTwoLineForHedder", SqlDbType.Bit).Value = infoMaster.IsTwoLineForHedder;
                sqlcmd.Parameters.Add("@isTwoLineForDetails", SqlDbType.Bit).Value = infoMaster.IsTwoLineForDetails;
                sqlcmd.Parameters.Add("@pageSize1", SqlDbType.Int).Value = infoMaster.PageSize1;
                sqlcmd.Parameters.Add("@pageSizeOther", SqlDbType.Int).Value = infoMaster.PageSizeOther;
                sqlcmd.Parameters.Add("@blankLneForFooter", SqlDbType.Int).Value = infoMaster.BlankLneForFooter;
                sqlcmd.Parameters.Add("@footerLocation", SqlDbType.VarChar).Value = infoMaster.FooterLocation;
                sqlcmd.Parameters.Add("@lineCountBetweenTwo", SqlDbType.Int).Value = infoMaster.LineCountBetweenTwo;
                sqlcmd.Parameters.Add("@pitch", SqlDbType.VarChar).Value = infoMaster.Pitch;
                sqlcmd.Parameters.Add("@condensed", SqlDbType.VarChar).Value = infoMaster.Condensed;
                sqlcmd.Parameters.Add("@lineCountAfterPrint", SqlDbType.VarChar).Value = infoMaster.LineCountAfterPrint;
                retunvalue = int.Parse(sqlcmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MasterAdd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
            return retunvalue;
        }
        public void MasterCopyAdd(MasterInfo infoMaster)
        {
            //int retunvalue = 0;
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("MasterCopyAdd", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@masterId", SqlDbType.Int).Value = infoMaster.MasterId;
                sqlcmd.Parameters.Add("@formName", SqlDbType.Int).Value = infoMaster.FormName;
                sqlcmd.Parameters.Add("@isTwoLineForHedder", SqlDbType.Bit).Value = infoMaster.IsTwoLineForHedder;
                sqlcmd.Parameters.Add("@isTwoLineForDetails", SqlDbType.Bit).Value = infoMaster.IsTwoLineForDetails;
                sqlcmd.Parameters.Add("@pageSize1", SqlDbType.Int).Value = infoMaster.PageSize1;
                sqlcmd.Parameters.Add("@pageSizeOther", SqlDbType.Int).Value = infoMaster.PageSizeOther;
                sqlcmd.Parameters.Add("@blankLneForFooter", SqlDbType.Int).Value = infoMaster.BlankLneForFooter;
                sqlcmd.Parameters.Add("@footerLocation", SqlDbType.VarChar).Value = infoMaster.FooterLocation;
                sqlcmd.Parameters.Add("@lineCountBetweenTwo", SqlDbType.Int).Value = infoMaster.LineCountBetweenTwo;
                sqlcmd.Parameters.Add("@pitch", SqlDbType.VarChar).Value = infoMaster.Pitch;
                sqlcmd.Parameters.Add("@condensed", SqlDbType.VarChar).Value = infoMaster.Condensed;
                sqlcmd.Parameters.Add("@lineCountAfterPrint", SqlDbType.VarChar).Value = infoMaster.LineCountAfterPrint;
                sqlcmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MasterCopyAdd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
            //return retunvalue;
        }
        public void MasterEdit(MasterInfo infoMaster)
        {
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("MasterEdit", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@masterId", SqlDbType.Int).Value = infoMaster.MasterId;
                sqlcmd.Parameters.Add("@formName", SqlDbType.Int).Value = infoMaster.FormName;
                sqlcmd.Parameters.Add("@isTwoLineForHedder", SqlDbType.Bit).Value = infoMaster.IsTwoLineForHedder;
                sqlcmd.Parameters.Add("@isTwoLineForDetails", SqlDbType.Bit).Value = infoMaster.IsTwoLineForDetails;
                sqlcmd.Parameters.Add("@pageSize1", SqlDbType.Int).Value = infoMaster.PageSize1;
                sqlcmd.Parameters.Add("@pageSizeOther", SqlDbType.Int).Value = infoMaster.PageSizeOther;
                sqlcmd.Parameters.Add("@blankLneForFooter", SqlDbType.Int).Value = infoMaster.BlankLneForFooter;
                sqlcmd.Parameters.Add("@footerLocation", SqlDbType.VarChar).Value = infoMaster.FooterLocation;
                sqlcmd.Parameters.Add("@lineCountBetweenTwo", SqlDbType.Int).Value = infoMaster.LineCountBetweenTwo;
                sqlcmd.Parameters.Add("@pitch", SqlDbType.VarChar).Value = infoMaster.Pitch;
                sqlcmd.Parameters.Add("@condensed", SqlDbType.VarChar).Value = infoMaster.Condensed;
                sqlcmd.Parameters.Add("@lineCountAfterPrint", SqlDbType.VarChar).Value = infoMaster.LineCountAfterPrint;
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MasterEdit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        public void MasterCopyEdit(MasterInfo infoMaster)
        {
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("MasterCopyEdit", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@masterId", SqlDbType.Int).Value = infoMaster.MasterId;
                sqlcmd.Parameters.Add("@formName", SqlDbType.Int).Value = infoMaster.FormName;
                sqlcmd.Parameters.Add("@isTwoLineForHedder", SqlDbType.Bit).Value = infoMaster.IsTwoLineForHedder;
                sqlcmd.Parameters.Add("@isTwoLineForDetails", SqlDbType.Bit).Value = infoMaster.IsTwoLineForDetails;
                sqlcmd.Parameters.Add("@pageSize1", SqlDbType.Int).Value = infoMaster.PageSize1;
                sqlcmd.Parameters.Add("@pageSizeOther", SqlDbType.Int).Value = infoMaster.PageSizeOther;
                sqlcmd.Parameters.Add("@blankLneForFooter", SqlDbType.Int).Value = infoMaster.BlankLneForFooter;
                sqlcmd.Parameters.Add("@footerLocation", SqlDbType.VarChar).Value = infoMaster.FooterLocation;
                sqlcmd.Parameters.Add("@lineCountBetweenTwo", SqlDbType.Int).Value = infoMaster.LineCountBetweenTwo;
                sqlcmd.Parameters.Add("@pitch", SqlDbType.VarChar).Value = infoMaster.Pitch;
                sqlcmd.Parameters.Add("@condensed", SqlDbType.VarChar).Value = infoMaster.Condensed;
                sqlcmd.Parameters.Add("@lineCountAfterPrint", SqlDbType.VarChar).Value = infoMaster.LineCountAfterPrint;
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MasterCopyEdit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        public void MasterDelate(decimal MasterId)
        {
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                    sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("MasterDelete", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@masterId", SqlDbType.Int).Value = MasterId;
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MasterDelate");
            }
            finally
            {
                sqlcon.Close();
            }
        }
        public MasterInfo MasterViewByFormName(int formName)
        {
            MasterInfo infoMaster = new MasterInfo();
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("MasterViewByFormName", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@formName", SqlDbType.Int).Value = formName;
                SqlDataReader sqldr = sqlcmd.ExecuteReader();
                while (sqldr.Read())
                {
                    infoMaster.MasterId = int.Parse(sqldr["masterId"].ToString());
                    infoMaster.FormName = int.Parse(sqldr["formName"].ToString());
                    infoMaster.IsTwoLineForHedder =bool.Parse(sqldr["isTwoLineForHedder"].ToString());
                    infoMaster.IsTwoLineForDetails = bool.Parse(sqldr["isTwoLineForDetails"].ToString());
                    infoMaster.PageSize1 =  int.Parse(sqldr["pageSize1"].ToString());
                    infoMaster.PageSizeOther = int.Parse(sqldr["pageSizeOther"].ToString());
                    infoMaster.BlankLneForFooter = int.Parse(sqldr["blankLneForFooter"].ToString());
                    infoMaster.FooterLocation = sqldr["footerLocation"].ToString();
                    infoMaster.LineCountBetweenTwo = int.Parse(sqldr["lineCountBetweenTwo"].ToString());
                    infoMaster.Pitch = sqldr["pitch"].ToString();
                    infoMaster.Condensed = sqldr["condensed"].ToString();
                    infoMaster.LineCountAfterPrint = int.Parse(sqldr["lineCountAfterPrint"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MasterViewByFormName", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
            return infoMaster;
        }
        public MasterInfo MasterCopyViewByFormName(int formName)
        {
            MasterInfo infoMaster = new MasterInfo();
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("MasterCopyViewByFormName", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@formName", SqlDbType.Int).Value = formName;
                SqlDataReader sqldr = sqlcmd.ExecuteReader();
                while (sqldr.Read())
                {
                    infoMaster.MasterId = int.Parse(sqldr["masterId"].ToString());
                    infoMaster.FormName = int.Parse(sqldr["formName"].ToString());
                    infoMaster.IsTwoLineForHedder = bool.Parse(sqldr["isTwoLineForHedder"].ToString());
                    infoMaster.IsTwoLineForDetails = bool.Parse(sqldr["isTwoLineForDetails"].ToString());
                    infoMaster.PageSize1 = int.Parse(sqldr["pageSize1"].ToString());
                    infoMaster.PageSizeOther = int.Parse(sqldr["pageSizeOther"].ToString());
                    infoMaster.BlankLneForFooter = int.Parse(sqldr["blankLneForFooter"].ToString());
                    infoMaster.FooterLocation = sqldr["footerLocation"].ToString();
                    infoMaster.LineCountBetweenTwo = int.Parse(sqldr["lineCountBetweenTwo"].ToString());
                    infoMaster.Pitch = sqldr["pitch"].ToString();
                    infoMaster.Condensed = sqldr["condensed"].ToString();
                    infoMaster.LineCountAfterPrint = int.Parse(sqldr["lineCountAfterPrint"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MasterViewByFormName", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
            return infoMaster;
        }
        public DataTable FormViewAll()
        {
            DataTable dtblPurchers = new DataTable();
            try
            {

                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sqldaPurchers = new SqlDataAdapter("FormViewAll", sqlcon);
                sqldaPurchers.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldaPurchers.Fill(dtblPurchers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FormViewAll", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
            return dtblPurchers;
        }
        public DataTable FieldsViewAll(int fieldId)
        {
            DataTable dtblPurchers = new DataTable();
            try
            {

                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlDataAdapter sqldaPurchers = new SqlDataAdapter("FieldsViewAll", sqlcon);
                sqldaPurchers.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqldaPurchers.SelectCommand.Parameters.Add("@formId", SqlDbType.Int).Value = fieldId;
                sqldaPurchers.Fill(dtblPurchers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FieldsViewAll", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
            return dtblPurchers;
        }
        public bool MasterCopyExistCheck(int masterId)
        {
            bool isOk = false;
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("MasterCopyExistCheck", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@masterId", SqlDbType.Int).Value = masterId;
                object obj = sqlcmd.ExecuteScalar();
                if (obj != null)
                {
                    isOk = bool.Parse(obj.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MasterCopyExistCheck", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
            return isOk;
        }

    }
}
