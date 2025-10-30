using SchoolV01.Application.Enums;
using SchoolV01.Application.Requests.Identity;
using System.Reflection;

namespace SchoolV01.Application.Requests
{
    public class UploadRequest
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public UploadType UploadType { get; set; }
        public byte[] Data { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is not UploadRequest otherInput)
                return false;
            PropertyInfo[] properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (!Equals(property.GetValue(this, null), property.GetValue(otherInput, null)))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                PropertyInfo[] properties = this.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    hash = hash * 23 + property.GetHashCode();
                }
                return hash;
            }
        }
    }
}