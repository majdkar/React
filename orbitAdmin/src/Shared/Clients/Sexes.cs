using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Shared.Constants.Clients
{
    public static class Sexes
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Male", "Male" },
            {"Female", "Female" },
        };
    }

    public enum SexEnum
    {
        Male,
        Female,
    }
}
