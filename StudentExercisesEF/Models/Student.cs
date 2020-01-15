using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesEF.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SlackHandle{ get; set; }

        public int CohortId { get; set; }

        public Cohort Cohort { get; set; }

        // Add user Id to students
        // Identity stores user's PKs as strings

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public List<StudentExercise> StudentExercises { get; set; } = new List<StudentExercise>();

    
    }
}
