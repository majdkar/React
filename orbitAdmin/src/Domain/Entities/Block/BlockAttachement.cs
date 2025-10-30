using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolV01.Core.Entities
{

    public class BlockAttachement
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }
        public string File { get; set; }
        public string Name { get; set; }


        [ForeignKey("Block")]
        public int BlockId { get; set; }
        public Block Block { get; set; }
    }
}
