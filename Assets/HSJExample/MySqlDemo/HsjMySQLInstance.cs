#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             MySqlAccess
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class HsjMySQLInstance
{
    private static MySqlConnection mySqlConnection;
    private static string host;
    private static string port;
    private static string userName;
    private static string password;
    private static string databaseName;
    //grant all on *.* to hsj@'%' identified by 'romantic123456' with grant option; 
    //grant all privileges on *.* to 'hsj'@'%' identified by 'romantic123456' with grant option;
    //GRANT ALL PRIVILEGES ON *.* TO 'hsj'@'%' IDENTIFIED BY 'romantic123456';
    //grant all privileges on *.* to 'root'@'%' identified by 'romantic123456' with grant option;  
    //flush privileges;
    //1.set global validate_password_policy=0;
//2.grant all privileges on *.* to 'root'@'%'identified by 'Aa123456' with grant option;
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="_host">ip地址</param>
    /// <param name="_userName">用户名</param>
    /// <param name="_password">密码</param>
    /// <param name="_databaseName">数据库名称</param>
    public HsjMySQLInstance(string _host, string _port, string _userName, string _password, string _databaseName)
    {
        host = _host;
        port = _port;
        userName = _userName;
        password = _password;
        databaseName = _databaseName;
    }

    /// 打开数据库
    public void OpenSql()
    {
        try
        {
            string mySqlString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};port={4}" , databaseName, host, userName, password, port);
            mySqlConnection = new MySqlConnection(mySqlString);
            mySqlConnection.Open();
            Debug.Log("打开数据库：" + host + "/" + port + "/" + databaseName);
        }
        catch (Exception e)
        {
            throw new Exception("连接失败，请检查MySql服务是否打开：" + e.Message.ToString());
        }
    }

    /// 关闭数据库
    public void CloseSql()
    {
        if (mySqlConnection != null)
        {
            mySqlConnection.Close();
            mySqlConnection.Dispose();
            mySqlConnection = null;
            Debug.Log("关闭数据库连接");
        }
    }

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="items">要查询的列</param>
    /// <param name="whereColumnName">查询的条件列</param>
    /// <param name="operation">条件操作符</param>
    /// <param name="value">条件的值</param>
    /// <returns></returns>
    public DataSet Select(string tableName, string[] whereColumnName, string[] operation, string[] value)
    {
        if (whereColumnName.Length != operation.Length || operation.Length != value.Length)
        {
            throw new Exception("输入不正确：" + "要查询的条件、条件操作符、条件值 的数量不一致！");
        }
        string querySQL = "Select " + "*";
        querySQL += " FROM " + tableName + " WHERE " + whereColumnName[0] + " " + operation[0] + " '" + value[0] + "'";
        for (int i = 1; i < whereColumnName.Length; i++)
        {
            querySQL += " and " + whereColumnName[i] + " " + operation[i] + " '" + value[i] + "'";
        }
        Debug.Log(querySQL);
        return QuerySet(querySQL);
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="userName">用户名</param>
    /// <param name="password">密码</param>
    /// INSERT INTO `mydata`.`userinfo` (`id`, `username_m`, `password_m`) VALUES ('3', 'd', 'dd');
    /// <returns></returns>
    public DataSet Add(string tableName, string userName,string password)
    {
        string addSQL = "INSERT INTO `mydata`.`userinfo` (`username_m`, `password_m`) VALUES " + "('" + userName + "','" + password + "')";
        Debug.Log(addSQL);
        return QuerySet(addSQL);
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="id">唯一主键</param>
    ///DELETE FROM `mydata`.`userinfo` WHERE (`id` = '4');
    /// <returns></returns>
    public DataSet Delete(string tableName, string id)
    {
        string deleteSQL = "DELETE FROM `mydata`.`"+tableName+"` WHERE (`id` = '"+ id +"');";
        Debug.Log(deleteSQL);
        return QuerySet(deleteSQL);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="id"></param>
    ///UPDATE `mydata`.`userinfo` SET `username_m` = 'g', `password_m` = 'gg' WHERE (`id` = '10');
    /// <returns></returns>
    public DataSet Update(string tableName, string userName, string password,string id)
    {
        string updateSQL = "UPDATE `"+ tableName + "` SET `username_m` = '"+ userName + "', `password_m` = '"+password+"' WHERE (`id` = '"+id+"')";
        Debug.Log(updateSQL);
      
        return QuerySet(updateSQL);
    }


    /// <summary>
    /// 执行SQL语句
    /// </summary>
    /// <param name="sqlString">sql语句</param>
    /// <returns></returns>
    private DataSet QuerySet(string sqlString)
    {
        if (mySqlConnection.State == ConnectionState.Open)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlDataAdapter mySqlAdapter = new MySqlDataAdapter(sqlString, mySqlConnection);
                mySqlAdapter.Fill(ds);
            }
            catch (Exception e)
            {
                throw new Exception("SQL:" + sqlString + "/r/n" + e.Message.ToString());
            }
            finally
            {
            }
            return ds;
        }
        return null;
    }
}
