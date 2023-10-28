using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.Data;
using RealEstate.Api.Models;

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        ApiDbContext _dbContext = new ApiDbContext();

        [HttpGet("PropertyList")]
        [Authorize]
        public IActionResult GetProperties(int categoryId)
        {
            var result = _dbContext.Properties.Where(x => x.CategoryId == categoryId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("PropertyDetail")]
        [Authorize]
        public IActionResult GetPropertyDetail(int id)
        {
            var result = _dbContext.Properties.FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("TrendingProperties")]
        [Authorize]
        public IActionResult GetTrendingProperties()
        {
            var result = _dbContext.Properties.Where(x => x.IsTrending == true);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("SearchProperties")]
        [Authorize]
        public IActionResult SearchProperties( string address)
        {
            var result = _dbContext.Properties.Where(x => x.Address.Contains(address));

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Property property)
        {
            if (property == null)
            {
                return NoContent();
            }
            else
            {
                // ? which mean => if its not null
                var userEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                var user = _dbContext.Users.FirstOrDefault(x => x.Email == userEmail);

                if (user == null)
                {
                    return NotFound();
                }

                property.IsTrending = false;

                property.UserId = user.Id;

                _dbContext.Properties.Add(property);

                _dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status201Created);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateProperty(int id, [FromBody] Property property)
        {
            var propertyResult = _dbContext.Properties.FirstOrDefault(x => x.Id == id);

            if (propertyResult == null)
            {
                return NotFound();
            }
            else
            {
                // ? which mean => if its not null
                var userEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                var user = _dbContext.Users.FirstOrDefault(x => x.Email == userEmail);

                if (user == null)
                {
                    return NotFound();
                }

                // edit code goes here
                if (propertyResult.UserId == user.Id)
                {
                    propertyResult.Name = property.Name;
                    propertyResult.Detail = property.Detail;
                    propertyResult.Price = property.Price;
                    propertyResult.Address = property.Address;
                    propertyResult.ImageUrl = property.ImageUrl;

                    property.IsTrending = false;
                    property.UserId = user.Id;

                    _dbContext.SaveChanges();

                    return Ok("update successfully");
                }

                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var propertyResult = _dbContext.Properties.FirstOrDefault(x => x.Id == id);

            if (propertyResult == null)
            {
                return NotFound();
            }
            else
            {
                // ? which mean => if its not null
                var userEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                var user = _dbContext.Users.FirstOrDefault(x => x.Email == userEmail);

                if (user == null)
                {
                    return NotFound();
                }

                if (propertyResult.UserId == user.Id)
                {
                    _dbContext.Properties.Remove(propertyResult);

                    _dbContext.SaveChanges();

                    return Ok("Deleted Successfully");
                }
            }

            return BadRequest();

        }
    }
}