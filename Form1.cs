using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TugceErciyesProject.Models;

namespace TugceErciyesProject
{
    public partial class Form1 : Form
    {
        private readonly Context _context;
        private bool isLoaded = false;
        public Form1()
        {
            _context = Program.ServiceProvider.GetRequiredService<Context>();
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
        }

        #region Buttons

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void insertButton_Click(object sender, EventArgs e)
        {
            bool canConvertECTSCredits = int.TryParse(ectsCreditsTextBox.Text, out int ectsCredits);
            if (termComboBox.SelectedItem != null && !string.IsNullOrWhiteSpace(courseCodeTextBox.Text) && !string.IsNullOrWhiteSpace(courseNameTextBox.Text) && !string.IsNullOrWhiteSpace(letterGradeTextBox.Text) && canConvertECTSCredits)
            {
                string selectedTermComboBox = termComboBox.SelectedItem.ToString(); // Fall ya da Spring
                var newCourse = new CourseModel
                {
                    Term = "Fall" == selectedTermComboBox ? TermEnum.Fall : TermEnum.Spring,
                    CourseCode = courseCodeTextBox.Text,
                    CourseName = courseNameTextBox.Text,
                    ECTSCredits = ectsCredits,
                    LetterGrade = letterGradeTextBox.Text,
                };
                await _context.Courses.AddAsync(newCourse);
                await _context.SaveChangesAsync();
                if (isLoaded)
                {
                    dataGridView1.Rows.Add(newCourse.Id, newCourse.Term, newCourse.CourseCode, newCourse.CourseName, newCourse.ECTSCredits, newCourse.LetterGrade);
                }
                _context.ChangeTracker.Clear();
                Reset();

            }
        }

        private async void loadButton_Click(object sender, EventArgs e)
        {
            if(!isLoaded)
            {
                var courses = await _context.Courses.AsNoTracking().ToListAsync();
                for (int i = 0; i < courses.Count; i++)
                {
                    dataGridView1.Rows.Add(courses[i].Id, courses[i].Term, courses[i].CourseCode, courses[i].CourseName, courses[i].ECTSCredits, courses[i].LetterGrade);
                }
                isLoaded = true;
            }
        }

        private async void searchButton_Click(object sender, EventArgs e)
        {
            if (isLoaded)
            {
                var query = _context.Courses.AsNoTracking();
                if (termComboBox.SelectedItem != null)
                {
                    string selectedTermComboBox = termComboBox.SelectedItem.ToString();
                    query = _context.Courses.Where(c => c.Term == ("Fall" == selectedTermComboBox ? TermEnum.Fall : TermEnum.Spring));
                }
                if (!string.IsNullOrWhiteSpace(courseCodeTextBox.Text))
                {
                    query = _context.Courses.Where(c => c.CourseCode == courseCodeTextBox.Text);
                }
                if (!string.IsNullOrWhiteSpace(courseNameTextBox.Text))
                {
                    query = _context.Courses.Where(c => c.CourseName == courseNameTextBox.Text);
                }
                if(!string.IsNullOrWhiteSpace(ectsCreditsTextBox.Text) && int.TryParse(ectsCreditsTextBox.Text, out int ectsCredits))
                {
                    query = _context.Courses.Where(c => c.ECTSCredits == ectsCredits);
                }
                if (!string.IsNullOrWhiteSpace(letterGradeTextBox.Text))
                {
                    query = _context.Courses.Where(c => c.LetterGrade == letterGradeTextBox.Text);
                }

                var courses = await query.ToListAsync();
                dataGridView1.Rows.Clear();
                for (int i = 0; i < courses.Count; i++)
                {
                    dataGridView1.Rows.Add(courses[i].Id, courses[i].Term, courses[i].CourseCode, courses[i].CourseName, courses[i].ECTSCredits, courses[i].LetterGrade);
                }
            }
        }

        private void clearAllButton_Click(object sender, EventArgs e)
        {
            Reset();
            dataGridView1.Rows.Clear();
            isLoaded = false;
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var currentRowIndex = dataGridView1.CurrentRow.Index;
                var currentRowId = dataGridView1.Rows[currentRowIndex].Cells[0];

                _context.Courses.Remove(new CourseModel { Id = Convert.ToInt32(currentRowId.Value) });
                bool isDeleted = await _context.SaveChangesAsync() > 0;
                if (isDeleted)
                {
                    dataGridView1.Rows.RemoveAt(currentRowIndex);
                }
                _context.ChangeTracker.Clear();
            }
        }

        #endregion

        private void Reset()
        {
            courseCodeTextBox.Clear();
            courseNameTextBox.Clear();
            ectsCreditsTextBox.Clear();
            letterGradeTextBox.Clear();
            termComboBox.SelectedIndex = -1;
            termComboBox.Text = "Select";
        }

        private void SetupDataGridView()
        {
            this.Controls.Add(dataGridView1);

            dataGridView1.ColumnCount = 6;

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font =
                new Font(dataGridView1.Font, FontStyle.Bold);

            //dataGridView1.Name = "dataGridView1";
            //dataGridView1.Location = new Point(8, 8);
            //dataGridView1.Size = new Size(500, 250);
            //dataGridView1.AutoSizeRowsMode =
            //    DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            //dataGridView1.ColumnHeadersBorderStyle =
            //    DataGridViewHeaderBorderStyle.Single;
            //dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            //dataGridView1.GridColor = Color.Black;
            //dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns[0].Name = "#";
            dataGridView1.Columns[1].Name = "Term";
            dataGridView1.Columns[2].Name = "Course Code";
            dataGridView1.Columns[3].Name = "Course Name";
            dataGridView1.Columns[4].Name = "ECTS Credits";
            dataGridView1.Columns[5].Name = "Letter Grade";
            dataGridView1.Columns[5].DefaultCellStyle.Font =
                new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Italic);

            //dataGridView1.SelectionMode =
            //    DataGridViewSelectionMode.FullRowSelect;
            //dataGridView1.MultiSelect = false;
            //dataGridView1.Dock = DockStyle.Fill;

            //dataGridView1.CellFormatting += new
            //    DataGridViewCellFormattingEventHandler(
            //    dataGridView1_CellFormatting);
        }

      
    }
}
