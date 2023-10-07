using Lab7.BUS;
using Lab7.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7.GUI
{
    public partial class Form1 : Form
    {

         private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvStudent);
                var listFacultys = facultyService.getAll();
                var listStudents = studentService.getAll();
                FillFaculty(listFacultys);
                BindGrid(listStudents);
            }
            catch( Exception ex ) 
            {
                MessageBox.Show(ex.Message);
            }

            txtDiem.Text = "";
            txtHoTen.Text = "";
            txtMSSV.Text = "";
        }

        private void FillFaculty(List<Faculty> listFaculty)
        {
            listFaculty.Insert(0, new Faculty());
            this.cmbKhoa.DataSource = listFaculty;
            this.cmbKhoa.DisplayMember = "FacultyName";
            this.cmbKhoa.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                if(item.Faculty != null)
                {
                    dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName ;
                }

                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore + "";

                if(item.MajorID != null)
                {
                    dgvStudent.Rows[index].Cells[4].Value = item.Major.Name +  " ";
                }
                showAvatar(item.Avatar);
            }
        }

        private void showAvatar(string imgName)
        {
            if(string.IsNullOrEmpty(imgName))
            {
                pictureBox1.Image = null;
            }
            else
            {
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imgPath = Path.Combine(parentDirectory,"Images", imgName);
                pictureBox1.Image = Image.FromFile(imgPath);
                pictureBox1.Refresh();
            }
        }

        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle  = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionForeColor = Color.DarkGoldenrod;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            StudentModel context = new StudentModel();
            Student dbDelete = context.Students.FirstOrDefault(p=>p.StudentID == txtMSSV.Text);

            if(dbDelete != null)
            {
                context.Students.Remove(dbDelete);
                context.SaveChanges();
            }

            Form1_Load(sender, e);
        }

        private void btnAddAndEDIT_Click(object sender, EventArgs e)
        {
            StudentModel context = new StudentModel();
            Student db = context.Students.FirstOrDefault(p => p.StudentID == txtMSSV.Text);
            Student s = new Student()
            {
                StudentID = txtMSSV.Text,
                FullName = txtHoTen.Text,
                FacultyID = int.Parse(cmbKhoa.SelectedValue.ToString()),
                AverageScore = float.Parse(txtDiem.Text)
            };  

            context.Students.Add(s);
            context.SaveChanges();

            Form1_Load(sender, e);
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            DataGridViewRow row = dgvStudent.Rows[e.RowIndex];
            txtMSSV.Text = row.Cells[0].Value.ToString();
            txtHoTen.Text = row.Cells[1].Value.ToString();
            txtDiem.Text = row.Cells[3].Value.ToString();
            cmbKhoa.Text = row.Cells[2].Value.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = new List<Student>();
            if (this.checkBox1.Checked)
            {
                listStudents = studentService.getAllHasNoMajor();
            }
            else
            {
                listStudents = studentService.getAll();
            }

            BindGrid(listStudents);

        }

        private void eXITToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            StudentModel context = new StudentModel();
            Student dbUpdate = context.Students.FirstOrDefault(p => p.StudentID == txtMSSV.Text);

            if (dbUpdate != null)
            {
                dbUpdate.FullName = txtHoTen.Text;
                dbUpdate.StudentID = txtMSSV.Text;
                dbUpdate.AverageScore = float.Parse(txtDiem.Text);
                dbUpdate.FacultyID = int.Parse(cmbKhoa.SelectedValue.ToString());
                context.SaveChanges();
            }

            Form1_Load(sender, e);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imgPath = openFileDialog.FileName;
                Image image = Image.FromFile(imgPath);
                // Hiển thị hình ảnh đã chọn trong PictureBox
                pictureBox1.Image = image;
            }
        }
    }
}
