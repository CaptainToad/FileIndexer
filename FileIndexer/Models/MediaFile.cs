using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIndexer.Models
{
    public record MediaFile : EntityBase
    {
        public string Filename { get;}
        public string Path { get; }
        public string Extension { get; }
        public long Size { get; }

        public MediaFile(FileInfo fileInfo)
        {
            Filename = fileInfo.Name;
            Path = fileInfo.Directory.FullName;
            Extension = fileInfo.Extension;
            Size = fileInfo.Length;
        }
    }
}
