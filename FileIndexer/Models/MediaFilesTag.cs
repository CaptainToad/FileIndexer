using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIndexer.Models
{
    public record MediaFileTag : EntityBase
    {
        public long MediaFileId { get; set; }
        public long TagId { get; set; }

        public MediaFileTag(long mediaFileId, long tagId)
        {
            MediaFileId = mediaFileId;
            TagId = tagId;
        }
    }
}
