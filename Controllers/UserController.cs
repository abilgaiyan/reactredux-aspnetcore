using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using B2BReports.Model;
using B2BReports.Repository;
using B2BReports.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace B2BReports.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IOptions<MySettingsModel> appSettings;

        public UserController(IOptions<MySettingsModel> app)
        {
            appSettings = app;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var data = DbClientFactory<UserDbClient>.Instance.GetAllUsers(appSettings.Value.connectionString);
            return Ok(data);
        }

        [HttpPost]
        [Route("SaveUser")]
        public IActionResult SaveUser([FromBody]UsersModel model)
        {
            var msg = new Message<UsersModel>();
            var data = DbClientFactory<UserDbClient>.Instance.SaveUser(model, appSettings.Value.connectionString);
            if (data == "C200")
            {
                msg.IsSuccess = true;
                if (model.Id == 0)
                    msg.ReturnMessage = "User saved successfully";
                else
                    msg.ReturnMessage = "User updated successfully";
            }
            else if (data == "C201")
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "Email Id already exists";
            }
            else if (data == "C202")
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "Mobile Number already exists";
            }
            return Ok(msg);
        }

        [HttpPost]
        [Route("DeleteUser")]
        public IActionResult DeleteUser([FromBody]UsersModel model)
        {
            var msg = new Message<UsersModel>();
            var data = DbClientFactory<UserDbClient>.Instance.DeleteUser(model.Id, appSettings.Value.connectionString);
            if (data == "C200")
            {
                msg.IsSuccess = true;
                msg.ReturnMessage = "User Deleted";
            }
            else if (data == "C203")
            {
                msg.IsSuccess = false;
                msg.ReturnMessage = "Invalid record";
            }
            return Ok(msg);
        }
    }

}


