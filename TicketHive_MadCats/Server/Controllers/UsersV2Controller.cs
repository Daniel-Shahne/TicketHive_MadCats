﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.WebSockets;
using TicketHive_MadCats.Server.Models;
using TicketHive_MadCats.Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketHive_MadCats.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersV2Controller : ControllerBase
    {
        private readonly UserManager<CustomUser> userManager;

        public UsersV2Controller(UserManager<CustomUser> userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// Gets the country of an user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<string>> GetCountry(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                return Ok(user.Country);
            }
            else return NotFound($"Could not find any user with name {username}");
        }

        // PUT api/<UsersV2Controller>/5
        /// <summary>
        /// Updates an users password and/or country
        /// </summary>
        /// <param name="updateUserModel">An UpdateUserModel serialized using Newtonsoft</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Put([FromBody]string updateUserModel)
        {
            // Attempts deserializing, returns bad request if couldnt
            UpdateUserModel? deserializedBody = JsonConvert.DeserializeObject<UpdateUserModel>(updateUserModel);
            if (deserializedBody == null)
            {
                return BadRequest("No valid UpdateUserModel could be found");
            }

            // Attempts finding user in database, returns not found if couldnt
            CustomUser? user = await userManager.FindByNameAsync(deserializedBody.Username);
            if (user == null)
            {
                return BadRequest($"Could not find user {deserializedBody.Username}");
            }

            // Changes the passwords if both are not null
            if(deserializedBody.Password != null && deserializedBody.CurrentPassword != null)
            {
                // Compares the sent current password to the users actual current password
                // Returns Unauthorized if password is wrong
                bool correctPassword = await userManager.CheckPasswordAsync(user, deserializedBody.CurrentPassword);
                if (!correctPassword)
                {
                    return Unauthorized("Current password is wrong");
                }

                // Changes users password, returns Conflict if failed
                var changePassResult = await userManager.ChangePasswordAsync(user, deserializedBody.CurrentPassword, deserializedBody.Password);
                if (!changePassResult.Succeeded)
                {
                    return Conflict("Could not change password");
                }
            }

            // Changes country if it isnt null
            if(deserializedBody.Country != null)
            {
                // Changes users country
                user.Country = deserializedBody.Country;
            }

            // Saves all the changes, returns Conflict if failed
            var saveResult = await userManager.UpdateAsync(user);
            if(!saveResult.Succeeded)
            {
                return Conflict("Could not save changes to database");
            }

            // Else if everything went ok, return Ok
            return Ok();
        }
    }
}
