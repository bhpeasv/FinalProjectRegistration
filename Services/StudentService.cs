

using Castle.Core.Internal;
using Interfaces;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;

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
            if (repo.GetById(s.Id) != null)
            {
                throw new InvalidOperationException("Student already exist");
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
                && (s.Email == null || s.Email != ""));
        }

        public void UpdateStudent(Student s)
        {
            if (s == null)
            {
                throw new ArgumentException("Student is missing");
            }
            if (! IsValidStudent(s))
            {
                throw new ArgumentException("Invalid student property");
            }
            if (repo.GetById(s.Id) == null)
            {
                throw new InvalidOperationException("Student does not exist");
            }
            repo.Update(s);
        }

        public void RemoveStudent(Student s)
        {
            if (repo.GetById(s.Id) == null)
            {
                throw new InvalidOperationException("Cannot remove not-existing student");
            }
            repo.Remove(s);
        }

        public Student GetStudentById(int id)
        {
            return repo.GetById(id);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return repo.GetAll();
        }
    }
}
