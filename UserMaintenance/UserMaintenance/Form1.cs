using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserMaintenance.Entities;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lblLastName.Text = Resource1.FirstName;
            lblFirstName.Text = Resource1.LastName;
            btnAdd.Text = Resource1.Add;

            listUsers.DataSource = users;
            listUsers.ValueMember = "ID";
            listUsers.DisplayMember = "FullName";
        }
        BindingList<User> users = new BindingList<User>();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            User u = new User();
            u.LastName = tbLastName.Text;
            u.FirstName = tbFirstName.Text;
            users.Add(u);
        }
    }
}
