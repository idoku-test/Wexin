using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;

namespace WeixinDemo.Controllers
{
    public class CommonController : ApiController
    {
        private string _appId = ConfigurationManager.AppSettings["WeixinAppId"];        
        private string templateId = "oBXrjpEgiS6EhJY0Lzveg79onspmgw1uWrX27row5QM";//换成已经在微信后台添加的模板Id

        [HttpGet]
        public string Send(string openId)
        {

            var accessToken = AccessTokenContainer.GetAccessToken(_appId);
            var testData = new //TestTemplateData()
            {
                first = new TemplateDataItem("【测试】您好，审核通过。"),
                keyword1 = new TemplateDataItem(openId),
                keyword2 = new TemplateDataItem("单元测试"),
                keyword3 = new TemplateDataItem(DateTime.Now.ToString()),
                remark = new TemplateDataItem("更详细信息，请到网站（http://doku.ngrok.cc/）查看！")
            };
            var result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, openId, templateId,
                "http://doku.ngrok.cc/", testData);
            return result.errcode.ToString();
        }
    }
}
