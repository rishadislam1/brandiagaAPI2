using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingMethodController : ControllerBase
    {
        private readonly IShippingMethodService _shippingMethodService;

        public ShippingMethodController(IShippingMethodService shippingMethodService)
        {
            _shippingMethodService = shippingMethodService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateShippingMethod([FromBody] ShippingMethodCreateDto shippingMethodCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var shippingMethod = await _shippingMethodService.CreateShippingMethodAsync(shippingMethodCreateDto);
                return Ok(ResponseDTO<ShippingMethodResponseDto>.Success(shippingMethod, "Shipping method created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("{shippingMethodId:guid}")]
        public async Task<IActionResult> GetShippingMethodById(Guid shippingMethodId)
        {
            try
            {
                var shippingMethod = await _shippingMethodService.GetShippingMethodByIdAsync(shippingMethodId);
                return Ok(ResponseDTO<ShippingMethodResponseDto>.Success(shippingMethod, "Shipping method retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllShippingMethods()
        {
            try
            {
                var shippingMethods = await _shippingMethodService.GetAllShippingMethodsAsync();
                return Ok(ResponseDTO<IEnumerable<ShippingMethodResponseDto>>.Success(shippingMethods, "Shipping methods retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{shippingMethodId:guid}")]
        public async Task<IActionResult> UpdateShippingMethod(Guid shippingMethodId, [FromBody] ShippingMethodUpdateDto shippingMethodUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var shippingMethod = await _shippingMethodService.UpdateShippingMethodAsync(shippingMethodId, shippingMethodUpdateDto);
                return Ok(ResponseDTO<ShippingMethodResponseDto>.Success(shippingMethod, "Shipping method updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{shippingMethodId:guid}")]
        public async Task<IActionResult> DeleteShippingMethod(Guid shippingMethodId)
        {
            try
            {
                await _shippingMethodService.DeleteShippingMethodAsync(shippingMethodId);
                return Ok(ResponseDTO<object>.Success(null, "Shipping method deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}
