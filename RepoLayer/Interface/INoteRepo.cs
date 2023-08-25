using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Interface
{
    public interface INoteRepo
    {
        public NoteEntity CreateNote(NoteModel noteModel,long userID);
        public List<NoteEntity> GetAll();
        public List<NoteEntity> GetNoteById(long UserId);
        public string UpdateNoteById(long Notesid, string Takenote);
        public long DeleteNote(long Noteid);
        public bool ArchiveByNoteId(long Noteid, long Userid);
        public bool PinByNoteId(long Noteid, long Userid);
        public bool TrashByNoteId(long Noteid, long Userid);
        public string UpdateColour(long Noteid,string colour, long Userid);
        public List<NoteEntity> GetParticularUser(long UserId);
        public List<NoteEntity> GetNoteByString(string stringData, long userId);
        public  Task<Tuple<int, string>> UploadImageById(long Noteid, long userId, IFormFile imageFile);
        public NoteEntity CopyNoteById(long Noteid, long userId);
    }
}
