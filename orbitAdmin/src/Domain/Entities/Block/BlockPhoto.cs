using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolV01.Core.Entities
{

    public class BlockPhoto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }
        public string Image { get; set; }


        [ForeignKey("Block")]
        public int BlockId { get; set; }
        public Block Block { get; set; }
    }
}
