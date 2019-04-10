namespace P01_StudentSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class StudentCourse
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        [ForeignKey(nameof(StudentId))] public Student Student { get; set; }

        [ForeignKey(nameof(CourseId))] public Course Course { get; set; }
    }
}