using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesEF.Models
{
    public class Cohort
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
    }
}
