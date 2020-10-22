using Interfaces;
using Model;
using Moq;
using Services;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

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
            StudentService service = new StudentService(repoMock.Object);

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

        [Fact]
        public void UpdateValidStudent()
        {
            // arrange
            Student s = new Student()
            {
                Id = 1,
                Name = "name",
                Address = "address",
                ZipCode = 1111,
                PostalDistrict = "district",
                Email = "e@mail.dk"
            };

            // make sure student exist in repository for test
            repoMock.Setup(repo => repo.GetById(It.Is<int>(x => x == s.Id))).Returns(() => s);

            StudentService service = new StudentService(repoMock.Object);


            // act
            service.UpdateStudent(s);

            // assert
            repoMock.Verify(repo => repo.Update(It.Is<Student>((st => st == s))), Times.Once);
        }

        [Fact]
        public void UpdateStudentIsNullExpectArgumentException()
        {
            // arrange
            StudentService service = new StudentService(repoMock.Object);

            // act + assert
            var ex = Assert.Throws<ArgumentException>(() => service.UpdateStudent(null));

            Assert.Equal("Student is missing", ex.Message);
            repoMock.Verify(repo => repo.Update(It.Is<Student>(s => s == null)), Times.Never);
        }

        [Fact]
        public void UpdateStudentNotExistingExpectInvalidOperationException()
        {
            // arrange
            Student s = new Student()
            {
                Id = 1,
                Name = "name",
                Address = "address",
                ZipCode = 1111,
                PostalDistrict = "district",
                Email = "e@mail.dk"
            };
            // make sure student does not exist in repository before test.
            repoMock.Setup(repo => repo.GetById(It.Is<int>(x => x == s.Id))).Returns(() => null);

            StudentService service = new StudentService(repoMock.Object);

            // act + assert
            var ex = Assert.Throws<InvalidOperationException>(() => service.UpdateStudent(s));

            Assert.Equal("Student does not exist", ex.Message);
            repoMock.Verify(repo => repo.Update(It.Is<Student>(s => s == null)), Times.Never);
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
        public void UpdateStudentInvalidPropertyExpectArgumentException(int id, string name, string address, int zipcode, string district, string email)
        {
            // arrange
            StudentService service = new StudentService(repoMock.Object);

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
            var ex = Assert.Throws<ArgumentException>(() => service.UpdateStudent(s));

            Assert.Equal("Invalid student property", ex.Message);
            repoMock.Verify(repo => repo.Update(It.Is<Student>((st => st == s))), Times.Never);
        }

        [Fact]
        public void removeExistingStudent()
        {
            // arrange
            Student s = new Student()
            {
                Id = 1,
                Name = "name",
                Address = "address",
                ZipCode = 1111,
                PostalDistrict = "district",
                Email = "e@mail.dk"
            };

            // make sure student exists in repository before test.
            repoMock.Setup(repo => repo.GetById(It.Is<int>(x => x == s.Id))).Returns(() => s);

            StudentService service = new StudentService(repoMock.Object);

            // act
            service.RemoveStudent(s);

            // assert
            repoMock.Verify(repo => repo.Remove(It.Is<Student>(st => st == s)), Times.Once);
        }

        [Fact]
        public void RemoveStudentNotExistExpectInvalidOperationException()
        {
            // arrange
            Student s = new Student()
            {
                Id = 1,
                Name = "name",
                Address = "address",
                ZipCode = 1111,
                PostalDistrict = "district",
                Email = "e@mail.dk"
            };

            // make sure student does not exist in repository before test.
            repoMock.Setup(repo => repo.GetById(It.Is<int>(x => x == s.Id))).Returns(() => null);

            StudentService service = new StudentService(repoMock.Object);

            // act + assert
            var ex = Assert.Throws<InvalidOperationException>(() => service.RemoveStudent(s));

            Assert.Equal("Cannot remove not-existing student", ex.Message);
            repoMock.Verify(repo => repo.Remove(It.Is<Student>(st => st == s)), Times.Never);
        }

        [Fact]
        public void GetStudentByIdExistingStudent()
        {
            // arrange
            Student s = new Student()
            {
                Id = 1,
                Name = "name",
                Address = "address",
                ZipCode = 1111,
                PostalDistrict = "district",
                Email = "e@mail.dk"
            };

            // make sure student exists in repository before test.
            repoMock.Setup(repo => repo.GetById(It.Is<int>(x => x == s.Id))).Returns(() => s);

            StudentService service = new StudentService(repoMock.Object);

            // act
            var result = service.GetStudentById(s.Id);

            // assert
            Assert.Equal(result, s);
            repoMock.Verify(repo => repo.GetById(It.Is<int>(x => x == s.Id)), Times.Once);
        }

        [Fact]
        public void GetStudentByIdNonExistingStudentReturnsNull()
        {
            // arrange
            int id = 1;

            // make sure student does not exist in repository before test.
            repoMock.Setup(repo => repo.GetById(It.Is<int>(x => x == id))).Returns(() => null);

            StudentService service = new StudentService(repoMock.Object);

            // act
            var result = service.GetStudentById(id);

            // assert
            Assert.Null(result);
            repoMock.Verify(repo => repo.GetById(It.Is<int>(x => x == id)), Times.Once);
        }

        [Theory]        // empty repository, 1 in repository, n in repository
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAllStudents(int studentCount)
        {
            // arrange
            List<Student> data = new List<Student>()
            {
                new Student(){ Id = 1},
                new Student() { Id = 2}
            };

            repoMock.Setup(repo => repo.GetAll()).Returns(() => data.GetRange(0, studentCount));

            StudentService service = new StudentService(repoMock.Object);

            // act

            var result = service.GetAllStudents();

            // assert
            Assert.Equal(result.Count(), studentCount);
            Assert.Equal(result.ToList(), data.GetRange(0, studentCount));
            repoMock.Verify(repo => repo.GetAll(), Times.Once);
        }
    }
}
