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

        [Theory]
        [InlineData(1, "Name", "Address", 1111, "District", "e@mail.dk")]
        [InlineData(1, "Name", "Address", 1111, "District", null)]
        public void AddValidStudent(int id, string name, string address, int zipcode, string district, string email)
        {
            // arrange
            IStudentRepository repo = repoMock.Object;
            StudentService service = new StudentService(repo);

            Student s = new Student()
            {
                Id = id,
                Name = name,
                Address = address,
                ZipCode = zipcode,
                PostalDistrict = district,
                Email = email
            };

            // act
            service.AddStudent(s);

            // assert
            repoMock.Verify(repo => repo.Add(It.Is<Student>((st => st == s))), Times.Once);
        }

        [Fact]
        public void AddStudentIsNullExpectArgumentException()
        {
            // arrange
            StudentService service = new StudentService(repoMock.Object);

            // act + assert
            var ex = Assert.Throws<ArgumentException>(() => service.AddStudent(null));

            Assert.Equal("Student is missing", ex.Message);
            repoMock.Verify(repo => repo.Add(It.Is<Student>(s => s == null)), Times.Never);
        }

        [Theory]
        [InlineData(0, "Name", "Address", 1111, "District", "e@mail.dk")]       // invalid Id
        [InlineData(-1, "Name", "Address", 1111, "District", "e@mail.dk")]      // invalid Id
        [InlineData(1, null, "Address", 1111, "District", "e@mail.dk")]         // name is missing
        [InlineData(1, "", "Address", 1111, "District", "e@mail.dk")]           // name is empty
        [InlineData(1, "Name", null, 1111, "District", "e@mail.dk")]            // address is missing
        [InlineData(1, "Name", "", 1111, "District", "e@mail.dk")]              // address is empty
        [InlineData(1, "Name", "Address", 0, "District", "e@mail.dk")]          // invalid zipcode
        [InlineData(1, "Name", "Address", -1, "District", "e@mail.dk")]         // invalid zipcode
        [InlineData(1, "Name", "Address", 1111, null, "e@mail.dk")]             // postaldistrict is missing
        [InlineData(1, "Name", "Address", 1111, "", "e@mail.dk")]               // postaldistrict is empty
        [InlineData(1, "Name", "Address", 1111, "District", "")]                // email is empty
        public void AddInvalidStudentExpectArgumentException(int id, string name, string address, int zipcode, string district, string email)
        {
            // arrange
            IStudentRepository repo = repoMock.Object;
            StudentService service = new StudentService(repo);

            Student s = new Student()
            {
                Id = id,
                Name = name,
                Address = address,
                ZipCode = zipcode,
                PostalDistrict = district,
                Email = email
            };

            // act + assert
            var ex = Assert.Throws<ArgumentException>(() => service.AddStudent(s));
            
            Assert.Equal("Invalid student property", ex.Message);
            repoMock.Verify(repo => repo.Add(It.Is<Student>((st => st == s))), Times.Never);
        }


    }
}
