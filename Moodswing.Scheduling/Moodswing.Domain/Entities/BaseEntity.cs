using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moodswing.Domain.Entities
{
    public class BaseEntity
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("create_at")]
        public DateTime CreateAt { get; set; }

        [Column("update_at")]
        public DateTime UpdateAt { get; set; }
    }
}
