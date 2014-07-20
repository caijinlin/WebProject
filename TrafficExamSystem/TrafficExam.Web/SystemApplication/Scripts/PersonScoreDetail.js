﻿var loginName = getQueryStringByName("LoginName");
var examId = getQueryStringByName("ExamId");

$(document).ready(function ()
{
    LoadScoreDetail();
})

function LoadScoreDetail()
{
    //加载页面数据
    $.ajax({ global: true, url: "CountPaperRequest.aspx", data: { ActionName: "GetPersonPersonDetail", ExamId: examId, LoginName: loginName },
        beforeSend: function ()
        {
            $.blockUI({
                css: { border: '1px solid #000', width: '10%', left: '45%' },
                overlayCSS: { backgroundColor: '#fff', opacity: 0 },
                message: '<img src="/Images/wait.gif" /><br />数据加载中，请稍候...'
            });
        },
        success: function (data)
        {
            $.unblockUI(); //IE7已上解决锁定方法
            if (data == null)
            {
                alert('没有查询数据!');
            }
            else
            {
                //将数据呈现到页面
                DataToTable(data);
            }
        },
        dataType: "json",
        type: "POST",
        async: true
    });
}

var prekeys = "";
function DataToTable(oLists)
{
    var cols = "";
    var col = "";
    var colName = "";
    var personDetail = "";
    var str = "<tr><td colspan='2' class='Adminrighttitle' style='text-align: center'>人员及考点信息</td><td class='Adminrighttitle'>对</td><td class='Adminrighttitle'>错</td><td class='Adminrighttitle'>题目数</td><td class='Adminrighttitle'>正确率(%)</td></tr>";
    var score = "";
    var v = 0;
    for (var i = 0; i < oLists.PersonDetail.length; i++)
    {
        personDetail = oLists.PersonDetail[i];
// <<<<<<< .mine
        v = parseFloat(personDetail.PaperTrue / (personDetail.PaperTrue + personDetail.PaperFalse)) * 100;
        score = Round(v,2) + "%";
// =======
        v = parseFloat(personDetail.PaperTrue / (personDetail.PaperTrue + personDetail.PaperFalse)) * 100;
        score = Round(v, 2) + "%";
 // >>>>>>> .r108
        if (i == 0)
        {
            str += "<tr>";
            str += "<td class='Adminrighttitle' rowspan='" + oLists.PersonDetail.length + "' style='text-align: center'>" + personDetail.RealName + "</td>";
            str += "<td class='Adminrighttitle'>" + personDetail.SubjectType + "</td>";
            str += "<td class='Adminrighttitle'>" + personDetail.PaperTrue + "</td>";
            str += "<td class='Adminrighttitle'>" + personDetail.PaperFalse + "</td>";
            str += "<td class='Adminrighttitle'>" + parseInt(personDetail.PaperTrue + personDetail.PaperFalse) + "</td>";
            str += "<td class='Adminrighttitle'>" + score + "</td>";
            str += "</tr>"
        }
        else
        {

            str += "<tr><td class='Adminrighttitle'>" + personDetail.SubjectType + "</td><td class='Adminrighttitle'>" + personDetail.PaperTrue + "</td><td class='Adminrighttitle'>" + personDetail.PaperFalse + "</td><td class='Adminrighttitle'>" + parseInt(personDetail.PaperTrue + personDetail.PaperFalse) + "</td><td class='Adminrighttitle'>" + score + "</td></tr>";
        }
    }
    $("#showScoreDetail").append(str);

}
//根据QueryString参数名称获取值 
function getQueryStringByName(name)
{
    var result = location.search.match(new RegExp("[\?\&]" + name + "=([^\&]+)", "i"));
    if (result == null || result.length < 1)
    {
        return "";
    }
    return result[1];
}

/*写法巨强JS四舍五入方法*/
function Round(v, e)
{
    //v表示要转换的值
    //e表示要保留的位数
    var t = 1;
    for (; e > 0; t *= 10, e--);
    for (; e < 0; t /= 10, e++);
    return Math.round(v * t) / t;
}