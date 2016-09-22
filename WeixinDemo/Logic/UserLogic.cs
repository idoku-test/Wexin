using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WeixinDemo.Models;

using System.Web.Script.Serialization;

namespace WeixinDemo.Logic
{
    public class UserLogic
    {
        private readonly string usersData = "App_Data/users.json";
        public IList<UserDto> GetAllUser()
        {
            var userList = new List<UserDto>();
            var filePath = HttpContext.Current.Server.MapPath(usersData);
            if (File.Exists(filePath))
            {
                var jsonText = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(jsonText))
                {
                    userList = JsonTools.JsonToObject2<List<UserDto>>(jsonText);
                }
            }
            return userList;
        }
    }
}