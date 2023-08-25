using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration.UserSecrets;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System.Threading.Tasks;
using RepoLayer.Context;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using RepoLayer.Entity;
using Microsoft.EntityFrameworkCore;
//using MassTransit.Internals;

namespace FundooNoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBusiness _noteBusiness;
        private readonly Cloudinary _cloudinary;
        private readonly FundooContext _fundooContext;
        private readonly IDistributedCache _distributedCache;


        public NoteController(INoteBusiness noteBusiness, Cloudinary cloudinary, FundooContext fundooContext,IDistributedCache distributedCache)
        {
            _noteBusiness = noteBusiness;
            _cloudinary = cloudinary;
            _fundooContext= fundooContext;
            _distributedCache = distributedCache;
        }

        
        [Authorize]
        [HttpPost]
        [Route("Notemaking")]
        public IActionResult NoteRegistration(NoteModel noteModel)
        {
            long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = _noteBusiness.CreateNote(noteModel, userId);
            if (result != null)
            {
                return this.Ok(new { success = true, message = "Note Created Successfully", data = result });

            }
            else
            {
                return this.BadRequest(new { success = false, message = "Note Creation UnSuccessful", data = result });
            }
        }
        [HttpGet]
        [Route("AllNotes")]
        public IActionResult GetAllNotesData()
        {
            var result = _noteBusiness.GetAll();
            if (result != null)
            {
                return this.Ok(new { success = true, message = "Notes Data Retrieved Successfully", data = result });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Notes Data Retrieval Unsuccessful", data = result });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("ParticularUserId")]
        public IActionResult GetParticularUserNotes()
        {
            var userClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId").Value;
            int userId = int.Parse(userClaim);
            var result = _noteBusiness.GetParticularUser(userId);
            if (result != null)
            {
                return this.Ok(new { success = true, message = "User data retrieved", data = result });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "User not found", data = result });
            }
        }

        [Authorize]
        [HttpGet("GetAllNoteByRedis")]
        public async Task<IActionResult> GetAllNotesByIdRedis()
        {
            var userIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value;
            int userId = int.Parse(userIdClaim);
            if (userIdClaim != null)
            {
                // Check if data is cached
                var cachedData = await _distributedCache.GetStringAsync($"Notes{userId}");
                if (!string.IsNullOrEmpty(cachedData))
                {
                    var notes = JsonConvert.DeserializeObject<List<NoteEntity>>(cachedData);
                     return Ok(new { success = true, message = "Get All Notes Successfully by Redis from cache", data = notes });
                }
                else
                {
                    var notesFromDb = _fundooContext.Note.Where(note => note.UserID == userId).ToList();

                    // Cache the data for 1 hour
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                    };
                    await _distributedCache.SetStringAsync($"Notes{userId}", JsonConvert.SerializeObject(notesFromDb), cacheOptions);

                    //return Ok(notesFromDb);
                    if (notesFromDb != null)
                    {
                        return Ok(new { success = true, message = "Get All Notes Successfully by Redis from database", data = notesFromDb });
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "Notes Not Found", data = notesFromDb });
                    }
                }
            }
            else
            {
                return Ok(null);
            }


        }


        /*[Authorize]
        [HttpGet]
        [Route("AllNotesByRedis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {

            var cacheKey = "NotesList";
            string serializedNoteList;
            List<NoteEntity> notesList;

            var redisNoteList = await _distributedCache.GetStringAsync(cacheKey);
            if (redisNoteList != null)
            {
                //serializedCustomerList = Encoding.UTF8.GetString(redisCustomerList);
                notesList = JsonConvert.DeserializeObject<List<NoteEntity>>(redisNoteList);
            }
            else
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                notesList = _noteBusiness.GetParticularUser(userId);
                //NotesList = await _fundooContext.Note.ToListAsync();
                serializedNoteList = JsonConvert.SerializeObject(notesList);

                await _distributedCache.SetStringAsync(cacheKey, serializedNoteList, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                });
            }

            if (notesList != null)
            {
                return Ok(new { success = true, message = "Get All Notes Successfully by Redis", data = notesList });
            }
            else
            {
                return BadRequest(new { success = false, message = "Notes Not Found", data = notesList });
            }
        }*/

        [Authorize]
        [HttpGet]
        [Route("NoteByID")]
        public IActionResult GetNoteByIdData(long UserId)
        {
            var result = _noteBusiness.GetNoteById(UserId);
            if (result != null)
            {
                return this.Ok(new { success = true, message = "Note Data Retrieved Successfully", data = result });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Note Data Retrieval Unsuccessful", data = result });
            }
        }
        
        [HttpPatch]
        [Route("ByID")]
        public IActionResult UpdateNoteByIdData(long id, string Takenote)
        {
            var result = _noteBusiness.UpdateNoteById(id, Takenote);
            if (result != null)
            {
                return this.Ok(new { success = true, message = "Note Data Updated Successfully", data = result });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Note Data Update Unsuccessful", data = result });
            }
        }
        [HttpDelete]
        [Route("ByID")]
        public IActionResult DeleteNoteByIdData(long id)
        {
            var result = _noteBusiness.DeleteNote(id);
            if (result != 0)
            {
                return this.Ok(new { success = true, message = "Data Deleted Successfully", data = result });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Data Deletion Unsuccessful", data = result });
            }

        }
        [Authorize]
        [HttpPut]
        [Route("Archive")]
        public IActionResult IsArchiveData(long Noteid)
        {
            var userIdClaim = User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value;
            int userId = int.Parse(userIdClaim);
            var result = _noteBusiness.ArchiveByNoteId(Noteid, userId);
            if (result == true)
            {
                return this.Ok(new { success = true, message = "Note Archived Successfully", data = result });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Note Archive Unsuccessful", data = result });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("Pin")]
        public IActionResult IsPinData(long Noteid)
        {
            var userClaim = User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value;
            int userId = int.Parse(userClaim);
            var result = _noteBusiness.PinByNoteId(Noteid, userId);
            if (result == true)
            {
                return this.Ok(new { success = true, message = "Pin Updated Successfully", data = result });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Pin Update Unsuccessful", data = result });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Trash")]
        public IActionResult IsTrashData(long Noteid)
        {
            var userClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId").Value;
            int userId = int.Parse(userClaim);
            var result = _noteBusiness.TrashByNoteId(Noteid, userId);
            if (result == true)
            {
                return this.Ok(new { success = true, message = "Trash Updated Successfully", data = result });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Trash Update Unsuccessful", data = result });
            }
        }
        [Authorize]
        [HttpPatch]
        [Route("Colour")]
        public IActionResult UpdateColour(long Noteid, string colour)
        {
            var userClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId").Value;
            int userId = int.Parse(userClaim);
            var result = _noteBusiness.UpdateColour(Noteid, colour, userId);
            if (result != null)
            {
                return this.Ok(new { success = true, message = "Colour Updated Successfully with " + result + " colour" });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Colour Update Failed" });
            }
        }
        
        [Authorize]
        [HttpGet]
        [Route("ByString")]
        public IActionResult GetNotesByStringData(string stringData)
        {
            var userClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId").Value;
            int userId = int.Parse(userClaim);
            var result = _noteBusiness.GetNoteByString(stringData, userId);
            if(result != null ) 
            {
                return this.Ok(new { success = true, message = "Data Retrieved Successfully", data = result });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Data Retrieval Unsuccessful", data = result });
            }
        }

        [Authorize]
        [HttpPatch]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage(long Noteid, IFormFile image)
        {
            var userClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId").Value;
            int userId = int.Parse(userClaim);
            var result =await _noteBusiness.UploadImageById(Noteid, userId, image);
            if(result.Item1 == 1)
            {
                return this.Ok(new { success = true, message = "Image uploaded Successfully", data = result });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Image upload Unsuccessful", data = result });
            }

        }

        [Authorize]
        [HttpPost]
        [Route("CopyNoteById")]
        public IActionResult CopyNote(long Noteid)
        {
            var userClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId").Value;
            int userId = int.Parse(userClaim);
            var result = _noteBusiness.CopyNoteById(Noteid, userId);
            if(result != null)
            {
                return this.Ok(new {success = true, message = "Note Copied Successfully", data = result});
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Note Copy Unsuccessful" });
            }
        }


    }
}
