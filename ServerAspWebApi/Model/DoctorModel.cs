namespace ServerAspWebApi.Model
{
    public class DoctorModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Cabinet { get; set; }
        public string Specialization { get; set; }
        public int Region { get; set; }
    }
}
