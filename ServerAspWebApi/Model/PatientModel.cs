using System;

namespace ServerAspWebApi.Model
{
    public class PatientModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Address { get; set; }
        public DateTime DateBirthDay { get; set; }
        public string Sex { get; set; }
        public int Region { get; set; }
    }
}
