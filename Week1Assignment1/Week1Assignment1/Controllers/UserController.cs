﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Week1Assignment1.Data;
using Week1Assignment1.DTO.User;
using Week1Assignment1.Helper;
using Week1Assignment1.Models;
using Week1Assignment1.Services.AuthServices;

namespace Week1Assignment1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IAuthService _authUser;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(IAuthService authUser, UserManager<IdentityUser> userManager)
        {
            _authUser = authUser;
            _userManager = userManager;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(UserRegDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authUser.RegisterUser(request);

                if (result.Succeeded == false)
                    return BadRequest(result.Errors, MsgKeys.RegisterFailed);

                return Ok(MsgKeys.RegisterUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex, ex.Message);
            }
        }

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto request)
        {
            try
            {
                var token = await _authUser.Login(request);

                if (string.IsNullOrEmpty(token))
                    return BadRequest(MsgKeys.UsernameOrPasswordIncorrect);

                return Ok( new { token } , MsgKeys.LoginUserSuccess);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Reset password by email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost("setPassword")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);

        //        var user = await _userManager.FindByEmailAsync(model.Email);

        //        if (user == null)
        //            return BadRequest(MsgKeys.UserNotFound);

        //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

        //        if (!result.Succeeded)
        //            return BadRequest(result.Errors);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
