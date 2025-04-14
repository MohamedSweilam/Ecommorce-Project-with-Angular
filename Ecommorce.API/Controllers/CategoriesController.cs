using AutoMapper;
using Ecommorce.API.Helper;
using Ecommorce.Core.DTO;
using Ecommorce.Core.Entities.Product;
using Ecommorce.Core.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ecommorce.API.Controllers
{

    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitofwork unitwork, IMapper mapper) : base(unitwork, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> Getall()
        {
            try
            {
                var category = await _unitwork.CategoryRepository.GetAllAsync();
                if (category is null)
                    return BadRequest(new ApiResponse(400));
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("get-by-id/{id})")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await _unitwork.CategoryRepository.GetByIdAsync(id);
                if (category is null) return BadRequest(new ApiResponse(400,"item is not found"));
                return Ok(category);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("add-category")]

        public async Task<IActionResult> Add(CategoryDTO categoryDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDTO); 
                await _unitwork.CategoryRepository.AddAsync(category);
                return Ok(new ApiResponse(200, "Item Has been Added"));

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-category")]
        public async Task<IActionResult>Update(UpdateDTO updateDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(updateDTO);
                await _unitwork.CategoryRepository.UpdateAsync(category);
                return Ok(new ApiResponse(200,"Item Has Been Updated"));

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _unitwork.CategoryRepository.DeleteAsync(id);
                return Ok(new {message="item has been deleted"});
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse(400));
            }

        }

    }
}
