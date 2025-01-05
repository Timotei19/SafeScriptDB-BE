namespace Models.AppConstants
{
    public class Enums
    {
        public enum Status
        {
            NotStarted = 0,
            InProgress = 1,
            Finished = 2,
            Failed = 3
        }

        public enum Result
        {
            Failed = 0,
            Success = 1,
        }

        public enum Role
        {
            Admin = 1,
            User = 2,
        }
    }
}
