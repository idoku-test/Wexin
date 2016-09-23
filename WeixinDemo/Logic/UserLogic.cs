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
        private readonly string usersData = "/App_Data/users.json";
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

        public string AddUser(UserDto user)
        {
            var userList = GetAllUser();
            if (userList.All(u => u.UserId != user.UserId))
            {
                userList.Add(user);
                var filePath = HttpContext.Current.Server.MapPath(usersData);
                if (File.Exists(filePath))
                {
                    var jsonText = JsonTools.ObjectToJson2(userList);
                    File.WriteAllText(filePath, jsonText);

                    return "添加成功";
                }
            }
            return "已存在";
        }

        public string UpdateUser(UserDto user)
        {
            var userList = GetAllUser();           
            var filePath = HttpContext.Current.Server.MapPath(usersData);
            if (File.Exists(filePath))
            {
                foreach (var u in userList)
                {
                    if (u.UserId == user.UserId)
                        u.OpenId = user.OpenId;
                }
                var jsonText = JsonTools.ObjectToJson2(userList);
                File.WriteAllText(filePath, jsonText);

                return "添加成功";
            }
            return "添加失败";
        }
    }
}