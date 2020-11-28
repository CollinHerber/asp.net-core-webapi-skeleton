using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreExample.Server.Models.Abstracts
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
