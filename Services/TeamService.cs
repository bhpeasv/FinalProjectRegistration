using Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class TeamService
    {
        private ITeamRepository repo;

        public TeamService(ITeamRepository repo)
        {
            if (repo == null)
            {
                throw new ArgumentException("Team repository is missing");
            }
            this.repo = repo;
        }

        public void CreateTeam(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid Team-Id");
            }

            if (repo.GetById(id) != null)
            {
                throw new InvalidOperationException("Team already exists");
            }

            Team t = new Team() { Id = id};
            repo.Add(t);
        }
    }
}
