namespace SchoolV01.Client.Infrastructure.Routes
{
    public static class AccountEndpoints
    {
        public static string Register = "api/identity/account/register";
        public static string ChangePassword = "api/identity/account/changepassword";
        public static string UpdateProfile = "api/identity/account/updateprofile";

        public static string ResetPasswordAndEmail = "api/identity/account/ResetPasswordAndEmail";

        public static string ChangePasswordByAdmin = "api/identity/account/changepasswordByAdmin";
        public static string UpdateProfileByAdmin = "api/identity/account/updateprofileByAdmin";

        public static string GetProfilePicture(string userId)
        {
            return $"api/identity/account/profile-picture/{userId}";
        }

        public static string UpdateProfilePicture(string userId)
        {
            return $"api/identity/account/profile-picture/{userId}";
        }
    }
}