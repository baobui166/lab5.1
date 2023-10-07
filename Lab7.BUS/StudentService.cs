using Lab7.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7.BUS
{
    public class StudentService
    {
        public List<Student> getAll()
        {
            StudentModel context = new StudentModel();
            return context.Students.ToList();
        }

        public List<Student> getAllHasNoMajor()
        {
            StudentModel context = new StudentModel();
            return context.Students.Where(p=> p.MajorID == null).ToList();
        }

        public List<Student> getAllHasNoMajor(int facultyID)
        {
            StudentModel context = new StudentModel();
            return context.Students.Where(p => p.MajorID == null && p.FacultyID == facultyID ).ToList();
        }

        public Student findByID(string id) 
        {
            StudentModel context = new StudentModel();
            return context.Students.FirstOrDefault(p => p.StudentID == id);
        }

        public void InsertUpdate(Student s)
        {
            StudentModel context = new StudentModel();
            context.Students.AddOrUpdate(s);
            context.SaveChanges();
        }
    }
}
