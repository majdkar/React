using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolV01.Core.Entities
{
    public class Event
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }

        [Column("Name"), StringLength(1000)]
        public string Name { set; get; }

        [Column("Description"), StringLength(Int32.MaxValue)]
        public string Description { set; get; }

        [Column("EnglishName"), StringLength(1000)]
        public string EnglishName { set; get; }

        [Column("EnglishDescription"), StringLength(Int32.MaxValue)]
        public string EnglishDescription { set; get; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}"),]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }

        public int RecordOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsVisible { get; set; }


        [ForeignKey("EventCategory")]
        public int CategoryId { get; set; }
        public EventCategory EventCategory { get; set; }

        public string Image { get; set; }
        public string Video { get; set; }

        public string File { get; set; }

        public string Url { get; set; }
        public List<EventPhoto> EventPhotos { get; set; }
        public List<EventAttachement> EventAttachements { get; set; }
    }
}
