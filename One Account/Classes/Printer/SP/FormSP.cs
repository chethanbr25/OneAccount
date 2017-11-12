using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace One_Account
{
    class FormSP:DBConnection
    {
        #region Functions
        /// <summary>
        /// Function for add the form
        /// </summary>
        /// <param name="infoForm"></param>
        /// <returns></returns>
        public int FormAdd(FormInfo infoForm)
        {
            int retunvalue = 0;
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("FormAdd", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@formName", SqlDbType.VarChar).Value = infoForm.FormName;
                retunvalue = int.Parse(sqlcmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FormAdd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
            return retunvalue;
        }
        /// <summary>
        /// Function for edit the form
        /// </summary>
        /// <param name="infoForm"></param>
        /// <returns></returns>
        public bool FormEdit(FormInfo infoForm)
        {
            bool isOk = false;
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("FormEdit", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@formId", SqlDbType.VarChar).Value = infoForm.FormId;
                sqlcmd.Parameters.Add("@formName", SqlDbType.VarChar).Value = infoForm.FormName;
                isOk=bool.Parse(sqlcmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FormEdit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
            return isOk;
        }
        /// <summary>
        /// Function for edit all
        /// </summary>
        /// <param name="infoForm"></param>
        public void FormEditFull(FormInfo infoForm)
        {
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("FormEditFull", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@formId", SqlDbType.VarChar).Value = infoForm.FormId;
                sqlcmd.Parameters.Add("@formName", SqlDbType.VarChar).Value = infoForm.FormName;
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FormEditFull", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
        }
        /// <summary>
        /// Function for view the form
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        public FormInfo FormView(int formId)
        {
            FormInfo infoForm = new FormInfo();
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sqlcmd = new SqlCommand("FormView", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.Add("@formId", SqlDbType.Int).Value = formId;
                SqlDataReader sqldr = sqlcmd.ExecuteReader();
                while (sqldr.Read())
                {
                    infoForm.FormId = int.Parse(sqldr["formId"].ToString());
                    infoForm.FormName = sqldr["formName"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "FormView", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlcon.Close();
            }
            return infoForm;
        }
        /// <summary>
        /// Function for view all the form
        /// </summary>
        /// <returns></returns>
        public DataTable FormViewAll()
        {
            DataTable dtblPurchers = new DataTable();
            try
            {
                dtblPurchers.Columns.Add("slNo", typeof(int));
                dtblPurchers.Columns["slNo"].AutoIncrement = true;
                dtblPurchers.Columns["slNo"].AutoIncrementSeed = 1;
                dtblPurchers.Columns["slNo"].AutoIncrementStep = 1;
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
        /// <summary>
        /// Function for delete the form
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        public bool FormDelete(int formId)
        {
            bool isOk = false;
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                SqlCommand sccmd = new SqlCommand("FormDelete", sqlcon);
                sccmd.CommandType = CommandType.StoredProcedure;
                SqlParameter sprmparam = new SqlParameter();
                sprmparam = sccmd.Parameters.Add("@formId", SqlDbType.Int);
                sprmparam.Value = formId;
               isOk= bool.Parse(sccmd.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"FormDelete");
            }

            finally
            {
                sqlcon.Close();
            }
            return isOk;
        }
        #endregion
    }
}
