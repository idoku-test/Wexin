using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;

namespace WeixinTest
{
    /// <summary>
    /// TemplateTest 的摘要说明
    /// </summary>
    [TestClass]
    public class TemplateTest:CommonApiTest
    {
        [TestMethod]
        public void SendTemplateMessageTest()
        {
            var openId = "oDsxRwKyjgOpprvKTIrszpnObRgw";//换成已经关注用户的openId
            var templateId = "oBXrjpEgiS6EhJY0Lzveg79onspmgw1uWrX27row5QM";//换成已经在微信后台添加的模板Id
            var accessToken = AccessTokenContainer.GetAccessToken(_appId);
            var testData = new //TestTemplateData()
            {
                first = new TemplateDataItem("【测试】您好，审核通过。"),
                keyword1 = new TemplateDataItem(openId),
                keyword2 = new TemplateDataItem("单元测试"),
                keyword3 = new TemplateDataItem(DateTime.Now.ToString()),
                remark = new TemplateDataItem("更详细信息，请到网站（http://doku.ngrok.cc/）查看！")
            };
            var result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage(accessToken, openId, templateId, "http://doku.ngrok.cc/", testData);

            Assert.AreEqual(ReturnCode.请求成功, result.errcode);
        }
    }
}
