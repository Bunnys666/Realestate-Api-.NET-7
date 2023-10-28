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
    public class BookmarksController : ControllerBase
    {
        ApiDbContext _dbContext = new ApiDbContext();

        // GET: api/Bookmarks
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var userEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound();
            }

            var bookmarks = from b in _dbContext.Bookmarks.Where(b => b.User_Id == user.Id)
                            join p in _dbContext.Properties on b.PropertyId equals p.Id
                            where b.Status == true
                            select new
                            {
                                b.Id,
                                p.Name,
                                p.Price,
                                p.Address,
                                p.ImageUrl,
                                b.Status,
                                b.User_Id,
                                b.PropertyId
                            };

            return Ok(bookmarks);
        }

        // POST: api/Bookmarks
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] Bookmark bookmarkItem)
        {
            bookmarkItem.Status = true;

            if (bookmarkItem.Status == false)
            {
                return BadRequest();
            }

            _dbContext.Bookmarks.Add(bookmarkItem);
            _dbContext.SaveChanges();

            return Ok("Bookmark added");
        }

        // DELETE: api/Bookmarks/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var bookmarkResult = _dbContext.Bookmarks.FirstOrDefault(x => x.Id == id);

            if (bookmarkResult == null)
            {
                return NotFound();
            }
            else
            {
                var userEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                var user = _dbContext.Users.FirstOrDefault(u => u.Email == userEmail);

                if (user == null)
                {
                    return NotFound();
                }

                if (bookmarkResult.User_Id == user.Id)
                {
                    _dbContext.Bookmarks.Remove(bookmarkResult);

                    _dbContext.SaveChanges();

                    return Ok("Bookmark Deleted Successfully");
                }

                return BadRequest();
            }
        }
    }
}
