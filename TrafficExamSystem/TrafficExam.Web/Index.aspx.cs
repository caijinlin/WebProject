﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TrafficExam.Bll;
using TrafficExam.Command;
using TrafficExam.Model;

namespace TrafficExam.Web
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Login_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
               
                if (!string.IsNullOrEmpty(txtUserName.Value.Trim()) && !string.IsNullOrEmpty(txtPassword.Value.Trim()))
                {
                    BllUser bllUser = new BllUser();
                    string loginName = MethodResources.FilterSQL(txtUserName.Value.Trim());
                    //D41D8CD98F00B204E9800998ECF8427E
                    //string loginPassWord = FormsAuthentication.HashPasswordForStoringInConfigFile(MethodResources.FilterSQL(txtPassword.Value.Trim()), "MD5");
                    string loginPassWord = txtPassword.Value.Trim();
                    IList<SystemUser> listUser = bllUser.GetUserInfoList(loginName, loginPassWord);
                    /*验证用户名与密码*/
                    if (listUser.Count > 0)
                    {
                        BllExamTime bllExamTime = new BllExamTime();
                        IList<ExamTime> examTimeList = bllExamTime.GetExamTimeList();
                        /*判断时间*/

                        //根据用户名与密码返回回来只有一条记录信息,所以这里直接 listUser[0]
                        Session["LoginName"] = listUser[0].LoginName.ToString();
                        Session["GroupId"] = listUser[0].GroupId.ToString();
                       
                        switch (listUser[0].GroupId)
                        {
                            case 1:
                                if (examTimeList[0].CurrentTime >= examTimeList[0].StartTime && examTimeList[0].CurrentTime <= examTimeList[0].EndTime)
                                {
                                    /*验证用户是否处于打开状态*/
                                    bool result = bllUser.IsOpen(loginName);
                                    if (result)
                                    {
                                        string checkIp = bllUser.CheckIpAddress(loginName);
                                        string ip = HttpContext.Current.Request.UserHostAddress;
                                        string type1 = "Exam";
                                        /*验证Ip地址是否为空*/
                                        if (!string.IsNullOrEmpty(checkIp))
                                        {
                                            if (checkIp != ip)
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('请勿代考!');</script>");
                                            }
                                            else
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>window.location.href='./Instruction.aspx?type="+type1+"'</script>");
                                            }
                                        }
                                        else
                                        {
                                            bool updateUserState = bllUser.UpdateUserIp(ip, loginName);
                                            if (updateUserState)
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>window.location.href='./Instruction.aspx?type="+type1+"'</script>");
                                            }
                                            else
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('写入Ip地址失败!');</script>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string type2 = "Pratice";

                                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('该人员已经完成考试!');</script>");
                                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>window.location.href='./Instruction.aspx?type="+type2+"'</script>");
                                    }
                                }
                                else
                                {
                                    string type2 = "Pratice";

                                    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('现在不是考试时间!');</script>");
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>window.location.href='./Instruction.aspx?type="+type2+"'</script>");
                                }
                                break;
                            case 2:
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>window.location.href='Default.aspx'</script>");
                                break;
                            case 3:
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>window.location.href='Default.aspx'</script>");
                                break;
                            default:
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>window.location.href='Index.aspx'</script>");
                                break;
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('用户名或密码错误!');</script>");
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script>alert('请输入用户名与密码!');</script>");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogInfo log = ExceptionLogInfo.LogInstance();
                log.WriterLog(ex.ToString());
            }
        }
    }
}