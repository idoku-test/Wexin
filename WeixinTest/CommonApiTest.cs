using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.Threads;

namespace WeixinTest
{
    [TestClass]
    public class CommonApiTest
    {

        private dynamic _appConfig;
        protected dynamic AppConfig
        {
            get
            {
                if (_appConfig == null)
                {
                    if (File.Exists("../../test.config"))
                    {
                        var doc = XDocument.Load("../../test.config");
                        _appConfig = new
                        {
                            AppId = doc.Root.Element("AppId").Value,
                            Secret = doc.Root.Element("Secret").Value,
                           
                        };
                    }
                    else
                    {
                        _appConfig = new
                        {
                            AppId = "YourAppId", //换成你的信息
                            Secret = "YourSecret",//换成你的信息                          
                        };
                    }
                }
                return _appConfig;
            }
        }


        protected string _appId
        {
            get { return AppConfig.AppId; }
        }

        protected string _appSecret
        {
            get { return AppConfig.Secret; }
        }

        /* 由于获取accessToken有次数限制，为了节约请求，
     * 可以到 http://sdk.weixin.senparc.com/Menu 获取Token之后填入下方，
     * 使用当前可用Token直接进行测试。
     */
        private string _access_token = null;

        protected string _testOpenId = "oDsxRwMW-HOjk8TkHJXe690KxaFw";//换成实际关注者的OpenId


        public CommonApiTest()
        {
            //全局只需注册一次
            AccessTokenContainer.Register(_appId, _appSecret);

            ThreadUtility.Register();
        }

        [TestMethod]
        public void GetUserInfoTest()
        {
            try
            {
                var accessToken = AccessTokenContainer.GetAccessToken(_appId);
                var result = CommonApi.GetUserInfo(accessToken, _testOpenId);
                Assert.IsNotNull(result);
            }
            catch (Exception ex)
            {
                //如果不参加内测，只是“服务号”，这类接口仍然不能使用，会抛出异常：错误代码：45009：api freq out of limit
            }
        }
    }
}
