namespace WebAPI.PostModels
{
    public class UserBody
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string UserPassword { get; set; }
        public string? UserEmail { get; set; }

    }
}
