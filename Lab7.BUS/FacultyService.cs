using Lab7.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Lab7.BUS
{
    public class FacultyService
    {
        public List<Faculty> getAll()
        {
            StudentModel context = new StudentModel();
            return context.Faculties.ToList();
        }
    }
}
