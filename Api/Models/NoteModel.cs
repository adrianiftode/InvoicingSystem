using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace Api.Models
{
    public class NoteModel
    {
        public int NoteId { get; set; }
        public string Text { get; set; }
    }

    public static class NoteMapper
    {
        public static NoteModel Map(this Note note)
            => note != null ? new NoteModel
            {
                NoteId = note.NoteId,
                Text = note.Text,
            } : null;

        public static IEnumerable<NoteModel> Map(this IEnumerable<Note> notes)
            => notes.Select(Map);
    }
}
