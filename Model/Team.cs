using System.Collections.Generic;

namespace Model
{
    public class Team
    {
        public int Id { get;  set; }
        public List<Student> Students { get;  private set; }

        public Team()
        {
            Students = new List<Student>();
        }
    }
}
