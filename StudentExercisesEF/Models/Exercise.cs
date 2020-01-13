using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesEF.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public List<StudentExercise> StudentExercises { get; set; } = new List<StudentExercise>();
    }
}
