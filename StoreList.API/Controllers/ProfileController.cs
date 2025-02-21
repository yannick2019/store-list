﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreList.Application.Interfaces;
using StoreList.Domain.Entities;

namespace StoreList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserContextService _userContextService;
        private readonly UserManager<AppUser> _userManager;

        public ProfileController(IUserContextService userContextService, UserManager<AppUser> userManager)
        {
            _userContextService = userContextService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = _userContextService.GetCurrentUserId();
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var userInfo = new
                {
                    user.UserName,
                    user.Email,
                    user.FirstName,
                    user.LastName
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
