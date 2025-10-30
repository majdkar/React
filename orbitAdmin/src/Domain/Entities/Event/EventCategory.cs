using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolV01.Core.Entities
{

    public class EventCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }
 
        [Column("Name"), StringLength(200)]
        public string Name { set; get; }
 
        [Column("Description"), StringLength(Int32.MaxValue)]
        public string Description { set; get; }
        [Column("EnglishName"), StringLength(1000)]
        public string EnglishName { set; get; }

        [Column("EnglishDescription"), StringLength(Int32.MaxValue)]
        public string EnglishDescription { set; get; }

        public string Image { get; set; }

        public bool IsActive { get; set; } = true;

        public int RecordOrder { get; set; }

        public List<Event> Events { get; set; }

    }
  }
