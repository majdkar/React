using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Shared.ViewModels.Blocks
{
    public class BlockVideoInsertModel
    {

        public int BlockId { get; set; }
        public string Url { get; set; }
    }

    public class BlockVideoInsertValidator : AbstractValidator<BlockVideoInsertModel>
    {
        public BlockVideoInsertValidator()
        {

        }
    }
}
