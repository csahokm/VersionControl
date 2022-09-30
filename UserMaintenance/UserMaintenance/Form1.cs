using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            lblFullName.Text = Resource1.FullName;
            btnAdd.Text = Resource1.Add;
            btnWrite.Text = Resource1.Write;
            btnDelete.Text = Resource1.Delete;

            listUsers.DataSource = users;
            listUsers.ValueMember = "ID";
            listUsers.DisplayMember = "FullName";
        }
        BindingList<User> users = new BindingList<User>();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            User u = new User();
            u.FullName = tbFullName.Text;
            users.Add(u);
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.InitialDirectory = Application.StartupPath;
            sfd.Filter = "Comma Seperated Values (*.csv)|*.csv";
            sfd.DefaultExt = "csv";
            sfd.AddExtension = true;

            if (sfd.ShowDialog() != DialogResult.OK) return;

            using (StreamWriter sr = new StreamWriter(sfd.FileName,false,Encoding.Default))
            {
                foreach(User u in users)
                {
                    sr.Write(u.ID.ToString());
                    sr.Write(";");
                    sr.Write(u.FullName);
                    sr.Write("\n");
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(users.Count > 0)
            {
                Guid current_userID = (Guid)listUsers.SelectedValue;
                foreach (var u in users)
                {
                    if (u.ID == current_userID)
                    {
                        users.Remove(u);
                        return;
                    }
                }
            }



        }
    }
}
