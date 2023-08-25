using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NoteBusiness : INoteBusiness
    {
        private readonly INoteRepo NoteRepo;
        public NoteBusiness(INoteRepo NoteRepo)
        {
            this.NoteRepo = NoteRepo;
        }
        public NoteEntity CreateNote(NoteModel noteModel, long userID)
        {
            try
            {
                return NoteRepo.CreateNote(noteModel, userID);
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
                return NoteRepo.GetAll().ToList();
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
                return NoteRepo.GetNoteById(UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string UpdateNoteById(long Notesid, string Takenote)
        {
            try
            {
                return NoteRepo.UpdateNoteById(Notesid, Takenote);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public long DeleteNote(long Noteid)
        {
            try
            {
                return NoteRepo.DeleteNote(Noteid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ArchiveByNoteId(long Noteid, long Userid)
        {
            try
            {
                return NoteRepo.ArchiveByNoteId(Noteid, Userid);
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
                return NoteRepo.PinByNoteId(Noteid, Userid);
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
                return NoteRepo.TrashByNoteId(Noteid, Userid);
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
                return NoteRepo.UpdateColour(Noteid,colour, Userid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<NoteEntity> GetParticularUser(long UserId)
        {
            try
            {
                return NoteRepo.GetParticularUser(UserId);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<NoteEntity> GetNoteByString(string stringData, long userId)
        {
            try
            {
                return NoteRepo.GetNoteByString(stringData, userId);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<int, string>> UploadImageById(long Noteid, long userId,IFormFile imageFile)
        {
            try
            {
                return await NoteRepo.UploadImageById(Noteid,userId, imageFile);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity CopyNoteById(long Noteid, long userId)
        {
            try
            {
                return NoteRepo.CopyNoteById(Noteid,userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
