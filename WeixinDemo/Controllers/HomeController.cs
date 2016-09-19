using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;

namespace WeixinDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly string Token = "weixin";

        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel,string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content(echostr);
            }
            else
            {
                return Content("failed:" + postModel.Signature + ","
                    + CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }
        
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post()
        {
            return View();
        }
    }
}
