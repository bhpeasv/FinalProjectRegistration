

using Castle.Core.Internal;
using Interfaces;
using Model;
using System;

namespace Services
{
    public class StudentService
    {
        private IStudentRepository repo;

        public StudentService(IStudentRepository repo)
        {
            this.repo = repo;
        }

        public void AddStudent(Student s)
        {
            if (s == null)
            {
                throw new ArgumentException("Student is missing");
            }
            if (! IsValidStudent(s))
            {
                throw new ArgumentException("Invalid student property");
            }
            repo.Add(s);
        }

        private bool IsValidStudent(Student s)
        {
            return (s.Id > 0
                && ! s.Name.IsNullOrEmpty()
                && ! s.Address.IsNullOrEmpty()
                && s.ZipCode > 0
                && ! s.PostalDistrict.IsNullOrEmpty()
                && s.Email.Length > 0);
        }
    }
}
