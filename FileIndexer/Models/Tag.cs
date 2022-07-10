using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIndexer.Models
{
    public record Tag : EntityBase
    {
        public string Text { get; set; }

        public Tag(string text)
        {
            Text = text;
        }
    }
}
