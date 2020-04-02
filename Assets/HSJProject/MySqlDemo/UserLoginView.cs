#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             UserLogin
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserLoginView : MonoBehaviour
{
    public InputField userNameInput;
    public InputField passwordInput;
    
    public Text loginMessage;//提示用户登录信息
    public Button loginBtn;//登录
    public Button resigerBtn;//注册
    public Button logoutBtn;//注销
    public Button changepasswordBtn;//注销
    public string host; //IP地址
    public string port;//端口号
    public string userName; //用户名
    public string password; //密码
    private string dataSchemasName;//数据库名称
    private HsjMySQLInstance mysql;//封装好的数据库类
    private string userInfoID;
    private void Start()
    {
        dataSchemasName = "mydata";
        mysql = new HsjMySQLInstance(host, port, userName, password, dataSchemasName);
        loginBtn.onClick.AddListener(Login);
        resigerBtn.onClick.AddListener(Resigion);
        logoutBtn.onClick.AddListener(Logout);
        changepasswordBtn.onClick.AddListener(ChangePassword);
    }
    /// <summary>
    /// 按下登录按钮
    /// </summary>
    private void Login()
    {
        mysql.OpenSql();
        DataSet ds = mysql.Select( "userinfo" , new string[] { "`" + "username_m" + "`", "`" + "password_m" + "`" }, new string[] { "=", "=" }, new string[] { userNameInput.text, passwordInput.text });
        if (ds != null)
        {
            DataTable table = ds.Tables[0];   
            if (table.Rows.Count > 0)
            {
                loginMessage.text = "登陆成功！";
                loginMessage.color = Color.green;
                Debug.Log("用户id：" + table.Rows[0][0]);
                userInfoID = table.Rows[0][0].ToString();
            }
            else
            {
                loginMessage.text = "用户名或密码错误！";
                loginMessage.color = Color.red;
            }
        }
        mysql.CloseSql();
    }
    private void Resigion()
    {
        mysql.OpenSql();
        DataSet ds = mysql.Add("userinfo", userNameInput.text, passwordInput.text );
        if (ds != null)
        {
            loginMessage.text = "注册成功！";
            loginMessage.color = Color.green;
        }
        else
        {
            loginMessage.text = "注册成功！";
            loginMessage.color = Color.red;
        }
        mysql.CloseSql();
    }
    private void Logout()
    {
        if (userInfoID == "" || userInfoID == null)
        {
            loginMessage.text = "请用户先登录后才能注销！";
            Debug.Log("请用户先登录后才能注销");
            return;
        }
        mysql.OpenSql();
        DataSet ds = mysql.Delete("userinfo", userInfoID);
        try
        {
            if (ds != null && ds.HasErrors == false)
            {
                loginMessage.text = "注销成功！";
                loginMessage.color = Color.green;
            }
            else
            {

                loginMessage.text = "注销失败！";
                loginMessage.color = Color.red;
            }
        }
        catch (System.Exception e)
        {

            throw new System.Exception(e.Message);
        }
       
        mysql.CloseSql();
    }
    private void ChangePassword()
    {
        if (userInfoID == "" || userInfoID == null)
        {
            loginMessage.text = "请用户先登录后才能改密！";
            Debug.Log("请用户先登录后才能改密");
            return;
        }
        mysql.OpenSql();
        DataSet ds = mysql.Update("userinfo", userNameInput.text, passwordInput.text,userInfoID);
        try
        {
            if (ds != null && ds.HasErrors == false)
            {
                loginMessage.text = "注销成功！";
                loginMessage.color = Color.green;
            }
            else
            {

                loginMessage.text = "注销失败！";
                loginMessage.color = Color.red;
            }
        }
        catch (System.Exception e)
        {

            throw new System.Exception(e.Message);
        }

        mysql.CloseSql();
    }
    private void OnDestroy()
    {
        mysql.CloseSql();
    }
}
