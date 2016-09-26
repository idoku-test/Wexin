using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.AppStore;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;

namespace WeixinDemo.Service.MessageHandlers
{
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {

        private string appId = WebConfigurationManager.AppSettings["WeixinAppId"];
        private string appSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];

        public CustomMessageHandler(Stream inputStream, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null)
            : base(inputStream, postModel, maxRecordCount, developerInfo)
        {
            base.WeixinContext.ExpireMinutes = 3;
            if (!string.IsNullOrEmpty(postModel.AppId))
                appId = postModel.AppId;
            base.OmitRepeatedMessageFunc = requestMessage =>
            {
                var textRequestMessage = requestMessage as RequestMessageText;
                if (textRequestMessage != null && textRequestMessage.Content == "容错")
                {
                    return false;
                }
                return true;
            };
        }

        public override void OnExecuting()
        {
            if (CurrentMessageContext.StorageData == null)
                CurrentMessageContext.StorageData = 0;
            base.OnExecuting();
        }

        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int) CurrentMessageContext.StorageData) + 1;
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            if (requestMessage.Content == null)
            {

            }
            else if (requestMessage.Content == "测试")
            {
                responseMessage.Content = @"您正在进行微信内置浏览器的判断测试.您可以:
                   <a href=""http://doku.ngrok.cc/FilterTest"">点击这里</a>进行客户端测试.或:
                    <a href=""http://doku.ngrok.cc/FilterTest\Redirect"">点击这里</a>进行客户端测试";
            }
            else
            {
                var result=new StringBuilder();
                result.AppendFormat("您刚才发送了文字消息:{0}\r\n\r\n", requestMessage.Content);
                if (CurrentMessageContext.RequestMessages.Count > 1)
                {
                    result.AppendFormat("您刚才还发送了如下消息({0}/{1}):\r\n", CurrentMessageContext.RequestMessages.Count,
                         CurrentMessageContext.StorageData);
                    for (int i = CurrentMessageContext.RequestMessages.Count-2; i >= 0; i--)
                    {
                        var historyMessage = CurrentMessageContext.RequestMessages[i];
                        result.AppendFormat("{0}【{1}】{2}\r\n",
                            historyMessage.CreateTime.ToShortTimeString(),
                            historyMessage.MsgType.ToString(),
                            (historyMessage is RequestMessageText)
                                ? (historyMessage as RequestMessageText).Content
                                : "非文字消息");
                    } 
                    result.AppendLine("\r\n");
                }
                result.AppendFormat("如果您在{0}分钟内连续发送消息,记录将被自动保留(当前设置:最多记录{1}条)",
                    WeixinContext.ExpireMinutes, WeixinContext.MaxRecordCount);
                result.AppendLine("\r\n");
                responseMessage.Content = result.ToString();
            }

            return responseMessage;
        }

        public override Senparc.Weixin.MP.Entities.IResponseMessageBase DefaultResponseMessage(
            Senparc.Weixin.MP.Entities.IRequestMessageBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这条消息来自默认返回.";
            return responseMessage;
        }
      
       

       
    }

}
