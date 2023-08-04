namespace API.Enums
{
    public static class EnumsList
    {
        public enum StatusCode
        {
            Success = 200,
            NotFound = 300,
            UnAuthorize = 403,
            BadRequest = 500,
        }

        public enum Permission
        {
            View,
            Create,
            Update,
            Delete
        }

        public enum Role
        {
            Admin = 1,
            User = 2
        }
    }
}
