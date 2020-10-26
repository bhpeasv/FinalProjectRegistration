using Interfaces;
using Model;
using Moq;
using Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTestProject
{
    public class TeamServiceTest
    {
        private Mock<ITeamRepository> repoMock = new Mock<ITeamRepository>();

        [Fact]
        public void CreateTeamService_ValidRepository()
        {
            // arrange
            ITeamRepository repo = repoMock.Object;

            // act
            TeamService service = new TeamService(repo);

            // assert
            Assert.NotNull(service);
        }

        [Fact]
        public void CreateTeamService_RepositoryIsNull_ExpectArgumentException()
        {
            // act + assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                TeamService service = new TeamService(null);
            });

            Assert.Equal("Team repository is missing", ex.Message);
        }

        [Fact]
        public void CreateTeam_ValidNonExisting()
        {
            // arrange
            int id = 1;

            // Make sure, the team does not exist in the repository
            repoMock.Setup(repo => repo.GetById(It.Is<int>((x => x == id)))).Returns(() => null);

            TeamService service = new TeamService(repoMock.Object);

            // act
            service.CreateTeam(id);

            // assert
            repoMock.Verify(repo => repo.Add(It.Is<Team>(team => team.Id == id)), Times.Once);
        }

        [Fact]
        public void CreateTeam_TeamExists_ExpectInvalidOperationException()
        {
            // arrange
            Team t = new Team()
            {
                Id = 1
            };

            // Make sure, the team exists in the repository
            repoMock.Setup(repo => repo.GetById(It.Is<int>((x => x == t.Id)))).Returns(() => t);

            TeamService service = new TeamService(repoMock.Object);

            // act
            var ex = Assert.Throws<InvalidOperationException>(() => service.CreateTeam(t.Id));

            // assert
            Assert.Equal("Team already exists", ex.Message);
            repoMock.Verify(repo => repo.Add(It.Is<Team>(team => team.Id == t.Id)), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateTeam_InvalidTeamId_ExpectArgumentException(int teamId)
        {
            // arrange
            TeamService service = new TeamService(repoMock.Object);

            // act + assert
            var ex = Assert.Throws<ArgumentException>(() =>service.CreateTeam(teamId));
            
            Assert.Equal("Invalid Team-Id", ex.Message);
            repoMock.Verify(repo => repo.Add(It.Is<Team>( t => t.Id == teamId)), Times.Never);
        }

        [Fact]
        public void RemoveTeam_ValidExisting()
        {
            // arrange
            Team t = new Team()
            { 
                Id = 1
            };

            // Make sure the team exists in the Repository
            repoMock.Setup(x => x.GetById(It.Is<int>(id => id == t.Id))).Returns(() => t);

            TeamService service = new TeamService(repoMock.Object);

            // act
            service.RemoveTeam(t);

            // assert
            repoMock.Verify(repo => repo.Remove(It.Is<Team>((team) => team.Id == t.Id)), Times.Once);
        }

        [Fact]
        public void RemoveTeam_TeamDoesNotExist_ExpectInvalidOperationException()
        {
            // arrange
            Team t = new Team()
            { 
                Id = 1
            };

            // Make sure the team does not exist in the Repository
            repoMock.Setup(x => x.GetById(It.Is<int>(id => id == t.Id))).Returns(() => null);

            TeamService service = new TeamService(repoMock.Object);

            // act
            var ex = Assert.Throws<InvalidOperationException>(() => service.RemoveTeam(t));

            // assert
            Assert.Equal("Team to remove does not exist", ex.Message);
            repoMock.Verify(repo => repo.Remove(It.Is<Team>((team) => team.Id == t.Id)), Times.Never);
        }
    }
}
