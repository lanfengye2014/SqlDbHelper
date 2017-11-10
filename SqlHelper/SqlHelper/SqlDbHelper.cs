using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 针对sql server 数据库操作的通用类
/// 作者：蓝枫叶
/// 日期：2017-11-10
/// version：1.0
/// </summary>

namespace SqlHelper
{
    public class SqlDbHelper
    {
        private string connectionString;

        /// <summary>
        /// 设置数据连接字符串
        /// </summary>
        public string ConnectionString
        {
            set
            {
                connectionString = value;
            }
        }
        /// <summary>
        /// SqlDbHelper构造函数
        /// </summary>
        public SqlDbHelper()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        public SqlDbHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// 执行一个查询，返回一个结果集
        /// </summary>
        /// <param name="sql">要执行的查询SQL文本命令</param>
        /// <returns>返回查询结果集</returns>
        public DataTable ExecuteDataTable(string sql)
        {

            return ExecuteDataTable(sql, CommandType.Text, null);
        }
        /// <summary>
        /// 执行一个查询，返回一个结果集
        /// </summary>
        /// <param name="sql">要执行的查询SQL文本命令</param>
        /// <param name="type">查询语句类型，如存储过程或者sql文本命令</param>
        /// <param name="parameter">参数数组</param>
        /// <returns>返回查询结果集</returns>

        public DataTable ExecuteDataTable(string sql, CommandType type, SqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    //设置查询语句类型
                    command.CommandType = type;
                    //如果传入了参数，则设置参数
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }

                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    adapter.Fill(dataTable);

                }
            }
            return dataTable;
        }
        /// <summary>
        /// 执行一个查询，返回一个SqlDataReader对象实例
        /// </summary>
        /// <param name="sql">要执行的查询SQL文本命令</param>
        /// <returns>返回一个SqlDataReader对象实例</returns>
        public SqlDataReader ExecuteDataReader(string sql)
        {

            return ExecuteDataReader(sql, CommandType.Text, null);
        }

        /// <summary>
        /// 执行一个查询，返回一个SqlDataReader对象实例
        /// </summary>
        /// <param name="sql">要执行的查询SQL文本命令</param>
        /// <param name="commandType">查询语句类型，如存储过程或者sql文本命令</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>返回一个SqlDataReader对象实例</returns>
        public SqlDataReader ExecuteDataReader(string sql, CommandType commandType, SqlParameter[] parameters)
        {
 
            SqlConnection connection = new SqlConnection(this.connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandType = commandType;

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);

                }

            }
            connection.Open();
            //CommandBehavior.CloseConnection 参数指示关闭reader时并关闭connection连接
            return  command.ExecuteReader(CommandBehavior.CloseConnection);

        }
        /// <summary>
        /// 执行一个查询，返回查询结果集的第一行第一列
        /// </summary>
        /// <param name="sql">要执行的查询SQL文本命令</param>
        /// <returns>返回查询结果集的第一行第一列</returns>
        public Object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, CommandType.Text, null);
        }
        /// <summary>
        /// 执行一个查询，返回查询结果集的第一行第一列
        /// </summary>
        /// <param name="sql">要执行的查询SQL文本命令</param>
        /// <param name="commandType">查询语句类型，如存储过程或者sql文本命令</param>
        /// <returns>返回查询结果集的第一行第一列</returns>
        public Object ExecuteScalar(string sql, CommandType commandType)
        {
            return ExecuteScalar(sql, commandType, null);
        }
        /// <summary>
        /// 执行一个查询，返回查询结果集的第一行第一列
        /// </summary>
        /// <param name="sql">要执行的查询SQL文本命令</param>
        /// <param name="commandType">查询语句类型，如存储过程或者sql文本命令</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>返回查询结果集的第一行第一列</returns>
        public Object ExecuteScalar(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            Object result = null;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql,connection))
                {
                    command.CommandType = commandType;
                    if (parameters!=null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    connection.Open();

                    result = command.ExecuteScalar();
                    
                }
            }
            return result;
        }
        /// <summary>
        /// 对数据库进行增删改操作
        /// </summary>
        /// <param name="sql">要执行的查询SQL文本命令</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(string sql) 
        {
            return ExecuteNonQuery(sql, CommandType.Text, null);
        }
        /// <summary>
        /// 对数据库进行增删改操作
        /// </summary>
        /// <param name="sql">要执行的查询SQL文本命令</param>
        /// <param name="commandType">查询语句类型，如存储过程或者sql文本命令</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(string sql, CommandType commandType, SqlParameter[] parameters)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql,connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        foreach (SqlParameter parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }
                    }
                    connection.Open();
                    count = command.ExecuteNonQuery();
                }
                
            }
            return count;
        }
    }
}
