using Microsoft.AspNetCore.Mvc;
using StoryList.Application.DTOs;
using StoryList.Application.Interfaces;

namespace StoryList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shoppingLists = await _shoppingListService.GetAllAsync();
            return Ok(shoppingLists);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var shoppingList = await _shoppingListService.GetByIdAsync(id);
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

            await _shoppingListService.AddAsync(shoppingListDto);
            return CreatedAtAction(nameof(GetById), new { id = shoppingListDto.Id }, shoppingListDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ShoppingListDto shoppingListDto)
        {
            if (id != shoppingListDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var existingList = await _shoppingListService.GetByIdAsync(id);
            if (existingList == null)
            {
                return NotFound($"Shopping list with ID {id} not found.");
            }

            await _shoppingListService.UpdateAsync(shoppingListDto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingList = await _shoppingListService.GetByIdAsync(id);
            if (existingList == null)
            {
                return NotFound($"Shopping list with ID {id} not found.");
            }

            await _shoppingListService.DeleteAsync(id);
            return NoContent();
        }
    }
}
