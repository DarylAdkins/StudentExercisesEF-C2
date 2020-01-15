using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentExercisesEF.Models;

namespace StudentExercisesEF.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Cohort> Cohort { get; set; }
        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<StudentExercise> StudentExercise { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
