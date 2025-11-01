using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Shared.ViewModels.Blocks
{
    public class BlockVideoUpdateModel
    {
        public int Id { set; get; }

        public string Url { get; set; }

        public int BlockId { get; set; }
    }

    public class BlockVideoUpdateValidator : AbstractValidator<BlockVideoUpdateModel>
    {
        public BlockVideoUpdateValidator()
        {


        }
    }
}
