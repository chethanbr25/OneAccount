using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace One_Account
{
    class FieldSP:DBConnection
    {
        #region Functions
        /// <summary>
        /// Function for add the fields
        /// </summary>
        /// <param name="infoField"></param>
        public void FieldsAdd(FieldInfo infoField)
        {
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("FieldsAdd", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@formId", SqlDbType.VarChar).Value = infoField.FormId;
                sqlcmd.Parameters.Add("@fieldName", SqlDbType.VarChar).Value = infoField.FieldName;
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Purchers add", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
          
        }
        /// <summary>
        /// Function for edit the fields
        /// </summary>
        /// <param name="infoField"></param>
        public void FieldsEdit(FieldInfo infoField)
        {
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("FieldsEdit", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@fieldId", SqlDbType.VarChar).Value = infoField.FieldId;
                sqlcmd.Parameters.Add("@formId", SqlDbType.VarChar).Value = infoField.FormId;
                sqlcmd.Parameters.Add("@fieldName", SqlDbType.VarChar).Value = infoField.FieldName;
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Purchers add", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        /// <summary>
        /// Function for view the fields
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public FieldInfo FieldsView(int fieldId)
        {
            FieldInfo infoField = new FieldInfo();
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("FieldsView", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@fieldId", SqlDbType.Int).Value = fieldId;
                SqlDataReader sqldr = sqlcmd.ExecuteReader();
                while (sqldr.Read())
                {
                    infoField.FieldId = int.Parse(sqldr["FieldId"].ToString());
                    infoField.FormId =  int.Parse(sqldr["FormId"].ToString());
                    infoField.FieldName = sqldr["FieldName"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FieldsView", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
            return infoField;
        }
        /// <summary>
        /// Function for view all the fields
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        public DataTable FieldsViewAll(int FormId)
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
                sqldaPurchers.SelectCommand.Parameters.Add("@formId", SqlDbType.Int).Value = FormId;
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
        /// <summary>
        /// Function for delete the fields
        /// </summary>
        /// <param name="formId"></param>
        public void FieldsDelete(int formId)
        {
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sccmd = new SqlCommand("FieldsDelete", sqlcon);
                sccmd.CommandType = CommandType.StoredProcedure;
                SqlParameter sprmparam = new SqlParameter();
                sprmparam = sccmd.Parameters.Add("@formId", SqlDbType.Int);
                sprmparam.Value = formId;
                sccmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                sqlcon.Close();
            }
        }
        #endregion
    }
}
