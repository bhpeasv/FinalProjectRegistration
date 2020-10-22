

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
            repo.Add(s);
        }
    }
}
