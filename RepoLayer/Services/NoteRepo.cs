using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RepoLayer.Services
{
    public class NoteRepo : INoteRepo
    {
        private readonly FundooContext _fundooContext;
        private readonly IConfiguration configuration;
        private readonly Cloudinary _cloudinary;
        private readonly FileService _fileService;
        public NoteRepo(FundooContext fundooContext, IConfiguration configuration, Cloudinary cloudinary, FileService fileService)
        {
            this._fundooContext = fundooContext;
            this.configuration = configuration;
            this._cloudinary = cloudinary;
            this._fileService = fileService;
        }
        public NoteEntity CreateNote(NoteModel noteModel,long userID)
        {
          try
            {
                NoteEntity noteEntity = new NoteEntity();
                noteEntity.Title = noteModel.Title; 
                noteEntity.TakeNote = noteModel.TakeNote;
                noteEntity.UserID = userID;
                _fundooContext.Note.Add(noteEntity);
                _fundooContext.SaveChanges();
                if (noteModel != null)
                {
                    return noteEntity;
                }
                else 
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public List<NoteEntity> GetAll()
        {
            try
            {
                List<NoteEntity> list = new List<NoteEntity>();
                list = _fundooContext.Note.ToList();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<NoteEntity> GetNoteById(long UserId)
        {
            try
            {
                var noteEntity = _fundooContext.Note.FirstOrDefault(x => x.UserID == UserId);
                if(UserId == noteEntity.UserID)
                {
                    List<NoteEntity> noteData = new List<NoteEntity>();
                    noteData.Add(noteEntity);
                    return noteData;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }
        public string UpdateNoteById(long Notesid, string Takenote)
        {
            try
            {
                var noteData = _fundooContext.Note.FirstOrDefault(x => x.NoteID == Notesid);
                if(noteData != null)
                {
                    noteData.TakeNote = noteData.TakeNote + Takenote;
                    _fundooContext.Note.Update(noteData);
                    _fundooContext.SaveChanges();
                    return noteData.TakeNote;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public long DeleteNote(long Noteid)
        {
            try
            {
                var result = _fundooContext.Note.FirstOrDefault(x => x.NoteID == Noteid);
                if(result != null)
                {
                    _fundooContext.Note.Remove(result);
                    _fundooContext.SaveChanges();
                    return result.NoteID;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool ArchiveByNoteId(long Noteid, long Userid)
        {
            try
            {
                var result = _fundooContext.Note.FirstOrDefault(x => x.NoteID == Noteid && x.UserID == Userid);

                if(result != null)
                {
                    result.IsArchive = true;
                    _fundooContext.Note.Update(result);
                    _fundooContext.SaveChanges();
                    return true;
                }
                else
                { 
                    return false; 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool PinByNoteId(long Noteid, long Userid)
        {
            try
            {
                var result = _fundooContext.Note.FirstOrDefault(x => x.NoteID == Noteid && x.UserID == Userid);

                if (result != null)
                {
                    result.IsPin = true;
                    _fundooContext.Note.Update(result);
                    _fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool TrashByNoteId(long Noteid, long Userid)
        {
            try
            {
                var existingNote = _fundooContext.Note.FirstOrDefault(x => x.NoteID == Noteid && x.UserID == Userid);
                if (existingNote != null)
                {
                    existingNote.IsTrash = true;
                    _fundooContext.Note.Update(existingNote);
                    _fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string UpdateColour(long Noteid,string colour, long Userid)
        {
            try
            {
                var existingNote = _fundooContext.Note.FirstOrDefault(x => x.NoteID == Noteid && x.UserID == Userid);
                if (existingNote != null)
                {
                    existingNote.Colour = colour;
                    _fundooContext.Note.Update(existingNote);
                    _fundooContext.SaveChanges();
                    return existingNote.Colour;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<NoteEntity> GetParticularUser(long UserId)
        {
            return _fundooContext.Note.Where(user => user.UserID == UserId).ToList();
        }
        public List<NoteEntity> GetNoteByString(string stringData, long userId)
        {
            try
            {
                var userEntity = _fundooContext.Note.FirstOrDefault(user => user.UserID == userId);
                var noteExist = _fundooContext.Note.Where(x => x.UserID == userId && (x.Title.Contains(stringData)||x.TakeNote.Contains(stringData))).ToList();
                if(userEntity != null && noteExist != null)
                {
                    return noteExist;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Tuple<int, string>> UploadImageById(long Noteid, long userId,IFormFile imageFile)
        {
            try
            {
                var note = _fundooContext.Note.FirstOrDefault(data => data.NoteID == Noteid && data.UserID == userId);
                if(note != null)
                {
                    var fileServiceResult = await _fileService.SaveImage(imageFile);
                    if(fileServiceResult.Item1 == 0)
                    {
                        return new Tuple<int, string>(0, fileServiceResult.Item2);
                    }
                    //Upload image to Cloudinary
                    var uploading = new ImageUploadParams
                    {
                        File = new CloudinaryDotNet.FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                    };
                    ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploading);

                    //Update product entity with image URL from Cloudinary
                    string ImageUrl = uploadResult.SecureUrl.AbsoluteUri;
                    //Add the product entity to the DbContext
                    note.Image = ImageUrl;
                    _fundooContext.Note.Update(note);
                    _fundooContext.SaveChanges();
                    return new Tuple<int, string>(1, "Product added with Image Successfully");
                }
                return null;
            }
            catch(Exception ex)
            {
                return new Tuple<int, string>(0, "An error occured: " + ex.Message);
            }
        }

        public NoteEntity CopyNoteById(long Noteid, long userId)
        {
            try
            {
                var checkNote = _fundooContext.Note.FirstOrDefault(x => x.NoteID == Noteid && x.UserID  == userId);
                if(checkNote != null)
                {
                    NoteEntity noteEntity = new NoteEntity();
                    noteEntity.Title = checkNote.Title;
                    noteEntity.TakeNote = checkNote.TakeNote;
                    noteEntity.UserID = checkNote.UserID;
                    _fundooContext.Note.Add(noteEntity);
                    _fundooContext.SaveChanges();
                    return noteEntity;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
