using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreList.Application.DTOs;
using StoreList.Application.Interfaces;

namespace StoreList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly IUserContextService _userContextService;

        public ShoppingListController(IShoppingListService shoppingListService, IUserContextService userContextService)
        {
            _shoppingListService = shoppingListService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = _userContextService.GetCurrentUserId();
            var shoppingLists = await _shoppingListService.GetAllAsync(userId);
            return Ok(shoppingLists);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = _userContextService.GetCurrentUserId();
            var shoppingList = await _shoppingListService.GetByIdAsync(id, userId);
            if (shoppingList == null)
            {
                return NotFound($"Shopping list with ID {id} not found.");
            }
            return Ok(shoppingList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShoppingListDto shoppingListDto)
        {
            if (shoppingListDto == null || string.IsNullOrWhiteSpace(shoppingListDto.Name))
            {
                return BadRequest("Invalid shopping list data");
            }

            var userId = _userContextService.GetCurrentUserId();
            await _shoppingListService.AddAsync(shoppingListDto, userId);
            return CreatedAtAction(nameof(GetById), new { id = shoppingListDto.Id }, shoppingListDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ShoppingListDto shoppingListDto)
        {
            if (id != shoppingListDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var userId = _userContextService.GetCurrentUserId();
            var existingList = await _shoppingListService.GetByIdAsync(id, userId);
            if (existingList == null)
            {
                return NotFound($"Shopping list with ID {id} not found.");
            }

            await _shoppingListService.UpdateAsync(shoppingListDto, userId);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = _userContextService.GetCurrentUserId();
            var existingList = await _shoppingListService.GetByIdAsync(id, userId);
            if (existingList == null)
            {
                return NotFound($"Shopping list with ID {id} not found.");
            }

            await _shoppingListService.DeleteAsync(id, userId);
            return NoContent();
        }

        /// <summary>
        /// Updates the checked state of a shopping list item.
        /// </summary>
        [HttpPatch("items/{itemId:guid}/check")]
        public async Task<IActionResult> UpdateItemCheckState(Guid itemId, [FromBody] bool isChecked)
        {
            var userId = _userContextService.GetCurrentUserId();
            var result = await _shoppingListService.UpdateItemCheckStateAsync(itemId, isChecked, userId);

            if (!result)
            {
                return NotFound($"Item with ID {itemId} not found.");
            }

            return NoContent();
        }
    }
}
