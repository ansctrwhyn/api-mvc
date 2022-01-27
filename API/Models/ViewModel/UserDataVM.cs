using System;

namespace API.Models.ViewModel
{
    public class UserDataVM
    {

        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public string Degree { get; set; }
        public string GPA { get; set; }
        public string UniversityName { get; set; }
        public Gender Gender { get; set; }
    }
}
