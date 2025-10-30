using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SchoolV01.Application.Requests.Identity;

public class RegisterRequest
{
    public string Id { get; set; }
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
    public string FullName => FirstName + " " + LastName;
    [EmailAddress]
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }

    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }
    [DataType(DataType.PhoneNumber)]
    public string HomePhoneNumber { get; set; }
    public string Address { get; set; }
    public string ClientType { get; set; }
    public bool IsActive { get; set; } = true;
    public bool AutoConfirmEmail { get; set; } = true;
    public string PictureUrl { get; set; } = "../media/default-person.png";
    public UploadRequest UploadRequest { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is not RegisterRequest otherInput)
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