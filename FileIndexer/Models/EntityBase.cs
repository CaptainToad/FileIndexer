using System.ComponentModel.DataAnnotations;

namespace FileIndexer.Models
{
    public abstract record EntityBase
    {
        [Key]
        public long Id { get; set; }
    }
}
