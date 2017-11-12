//This is a source code or part of OneAccount project
//Copyright (C) 2013 OneAccount
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//GNU General Public License for more details.
//You should have received a copy of the GNU General Public License
//along with this program. If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace AccountI.DAL
{
    public class CompanyNameSP : DBConnection
    {
        public String GetCompanyName()
        {
            if (sqlCon.State == System.Data.ConnectionState.Closed)
            {
                sqlCon.Open();
            }
            SqlCommand sqlCmd = new SqlCommand("AccountI_SelectCompanyName", sqlCon);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            return Convert.ToString((sqlCmd.ExecuteScalar() != null) ? sqlCmd.ExecuteScalar() : string.Empty);
        }
    }
}
