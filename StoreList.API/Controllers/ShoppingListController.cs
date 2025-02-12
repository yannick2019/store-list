using Microsoft.AspNetCore.Mvc;
using StoreList.Application.DTOs;
using StoreList.Application.Interfaces;

namespace StoreList.API.Controllers
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

        /// <summary>
        /// Gets all shopping lists.
        /// </summary>
        /// <returns>A list of shopping lists</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shoppingLists = await _shoppingListService.GetAllAsync();
            return Ok(shoppingLists);
        }

        /// <summary>
        /// Gets a shopping list by ID.
        /// </summary>
        /// <param name="id">The ID of the shopping list.</param>
        /// <returns>The shopping list with the specified ID.</returns>
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

        /// <summary>
        /// Creates a new shopping list.
        /// </summary>
        /// <param name="shoppingListDto">The shopping list data transfer object.</param>
        /// <returns>The created shopping list.</returns>
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

        /// <summary>
        /// Update an existing shopping list.
        /// </summary>
        /// <param name="id">The ID of the shopping list to update.</param>
        /// <param name="shoppingListDto">The updated shopping list data transfer object.</param>
        /// <returns>No content.</returns>
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

        /// <summary>
        /// Deletes a shopping list by ID.
        /// </summary>
        /// <param name="id">The ID of the shopping list to delete.</param>
        /// <returns>No content.</returns>
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
