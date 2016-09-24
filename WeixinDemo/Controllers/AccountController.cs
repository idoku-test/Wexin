using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WeixinDemo.Logic;
using WeixinDemo.Models;

namespace WeixinDemo.Controllers
{
    public class AccountController : Controller
    {
        private string appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private string secret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        private UserLogic userLogic = new UserLogic();
        // GET: Account
        public ActionResult Index()
        {
            var user = Session["User"] as UserDto;           
            return View(user);
        }

        public ActionResult List()
        {
            var list = userLogic.GetAllUser();
            return View(list);
        }

        public ActionResult InitData()
        {
            var userList = new List<UserDto>();
            userList.Add(new UserDto() { OpenId = "oDsxRwIWa-h9n467cw6fFs3qLyGw", UserId = "冷神" });
            userList.Add(new UserDto() { OpenId = "oDsxRwKyjgOpprvKTIrszpnObRgw", UserId = "www.邓昊.org" });
            userList.Add(new UserDto() { OpenId = "oDsxRwGPioUgweqLNUertJ1Mkzao", UserId = "唐福涛" });
            userList.Add(new UserDto() { OpenId = "oDsxRwAA0CWpgn_VsJ0XEhsEEXrE", UserId = "刘小磊" });
            userList.Add(new UserDto() { OpenId = "oDsxRwJBL5BvoIQ9UP7sBHl2Is8M", UserId = "刘兴华" });
            userList.Add(new UserDto() { OpenId = "oDsxRwMW-HOjk8TkHJXe690KxaFw", UserId = "酷龙" });
            foreach (var user in userList)
            {
                userLogic.AddUser(user);
            }
            return RedirectToAction("Register");
        }

        public ActionResult Register()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserDto user)
        {        
            var tokenResult = Session["OAuthAccessToken"] as OAuthAccessTokenResult;
            if (tokenResult != null)
            {
                user.OpenId = tokenResult.openid;
            }
            userLogic.AddUser(user);
            Session["User"] = user;
            
            return RedirectToAction("Index");
        }

        public ActionResult Bind(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            if (state != "STATE")
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                return Content("验证失败！请从正规途径进入！");
            }

            OAuthAccessTokenResult result = null;

            //通过，用code换取access_token
            try
            {
                result = OAuthApi.GetAccessToken(appId, secret, code);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }
            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            Session["OAuthAccessTokenStartTime"] = DateTime.Now;
            Session["OAuthAccessToken"] = result;

            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                var userList = userLogic.GetAllUser();
                if (userList.Any(u=>u.OpenId==result.openid))
                {
                    Session["User"] = userList.FirstOrDefault(u => u.OpenId == result.openid);
                    //已经绑定,直接登录
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bind(UserDto user)
        {
            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            { 
                var userList = userLogic.GetAllUser();
                if (userList.Any(u => u.UserId == user.UserId))
                {
                    var curUser = userList.FirstOrDefault(u => u.UserId == user.UserId);

                    if (curUser != null)
                    {
                        var userInfo = Session["OAuthAccessToken"] as OAuthAccessTokenResult;
                        if (userInfo != null)
                        {
                            curUser.OpenId = userInfo.openid;
                            userLogic.UpdateUser(curUser);
                            Session["User"] = curUser;
                            return RedirectToAction("Index");
                        }
                    }
                    return View(curUser);
                }
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}