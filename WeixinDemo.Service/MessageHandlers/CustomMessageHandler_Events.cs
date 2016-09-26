using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.Entities;

namespace WeixinDemo.Service.MessageHandlers
{
    public partial class CustomMessageHandler
    {
        private string GetWelcomInfo()
        {
            return string.Format(@"
欢迎关注【微信平台】，当前时间:{0}
官方地址:http://doku.ngrok.cc/
", DateTime.Now);
        }

        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = GetWelcomInfo();
            if (!string.IsNullOrEmpty(requestMessage.EventKey))
            {
                responseMessage.Content += "\r\n==========\r\n场景值:" + requestMessage.EventKey;
            }

            return responseMessage;
        }
    }

    
}
