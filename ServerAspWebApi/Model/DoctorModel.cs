namespace ServerAspWebApi.Model
{
    public class DoctorModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string FullName => $"{FirstName} {LastName} {Patronymic}";
    }
}
