using Interfaces;
using Model;
using Moq;
using Services;
using System;
using Xunit;

namespace XUnitTestProject
{
    public class StudentServiceTest
    {
        private Mock<IStudentRepository> repoMock;

        public StudentServiceTest()
        {
            repoMock = new Mock<IStudentRepository>();
        }

        [Fact]
        public void CreateStudentService()
        {
            // arrange
            IStudentRepository repo = repoMock.Object;

            // act
            StudentService service = new StudentService(repo);

            // assert
            Assert.NotNull(service);
        }

        [Fact]
        public void AddValidStudent()
        {
            // arrange
            IStudentRepository repo = repoMock.Object;
            StudentService service = new StudentService(repo);

            Student s = new Student()
            {
                Id = 1
            };

            // act
            service.AddStudent(s);

            // assert
            repoMock.Verify(repo => repo.Add(It.Is<Student>((st => st == s))), Times.Once);
        }
    }
}
