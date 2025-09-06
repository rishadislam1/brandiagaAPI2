using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateInventory([FromBody] InventoryCreateDto inventoryCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var inventory = await _inventoryService.CreateInventoryAsync(inventoryCreateDto);
                return Ok(ResponseDTO<InventoryResponseDto>.Success(inventory, "Inventory created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpGet("{inventoryId:guid}")]
        public async Task<IActionResult> GetInventoryById(Guid inventoryId)
        {
            try
            {
                var inventory = await _inventoryService.GetInventoryByIdAsync(inventoryId);
                return Ok(ResponseDTO<InventoryResponseDto>.Success(inventory, "Inventory retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpGet("product/{productId:guid}")]
        public async Task<IActionResult> GetInventoryByProductId(Guid productId)
        {
            try
            {
                var inventory = await _inventoryService.GetInventoryByProductIdAsync(productId);
                return Ok(ResponseDTO<InventoryResponseDto>.Success(inventory, "Inventory retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllInventories()
        {
            try
            {
                var inventories = await _inventoryService.GetAllInventoriesAsync();
                return Ok(ResponseDTO<IEnumerable<InventoryResponseDto>>.Success(inventories, "Inventories retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{inventoryId:guid}")]
        public async Task<IActionResult> UpdateInventory(Guid inventoryId, [FromBody] InventoryUpdateDto inventoryUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var inventory = await _inventoryService.UpdateInventoryAsync(inventoryId, inventoryUpdateDto);
                return Ok(ResponseDTO<InventoryResponseDto>.Success(inventory, "Inventory updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{inventoryId:guid}")]
        public async Task<IActionResult> DeleteInventory(Guid inventoryId)
        {
            try
            {
                await _inventoryService.DeleteInventoryAsync(inventoryId);
                return Ok(ResponseDTO<object>.Success(null, "Inventory deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}
