using Microsoft.Data.SqlClient;
using System;

namespace Consulta_medica.Repository
{
    public class SqlGenericParameters
    {
        public SqlParameter pFilterOne { get; set; }
        public SqlParameter pFilterTwo { get; set; }
        public SqlParameter pFilterThree { get; set; }

        public SqlGenericParameters() 
        {
            pFilterOne = new SqlParameter("@sFilterOne", DBNull.Value);
            pFilterTwo = new SqlParameter("@sFilterTwo", DBNull.Value);
            pFilterThree = new SqlParameter("@sFilterThree", DBNull.Value);
        }
    }
}
