using Microsimulation.Entities;
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

namespace Microsimulation
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        public Form1()
        {
            InitializeComponent();
            Population = GetPopulation(@"C:\Temp\nép.csv");
            BirthProbabilities = GetBirthProbability(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbability(@"C:\Temp\halál.csv");
            dataGridView1.DataSource = DeathProbabilities;
        }
        private List<Person> GetPopulation(string path)
        {
            using(StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(';');
                    int birthYear = int.Parse(line[0]);
                    int nbrOfChildren = int.Parse(line[2]);
                    Person p = new Person()
                    {
                        BirthYear = birthYear,
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = nbrOfChildren
                    };
                    Population.Add(p);
                }
            }
            return Population;
        }
        private List<BirthProbability> GetBirthProbability(string path)
        {
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(';');
                    int age = int.Parse(line[0]);
                    int nbrOfChildren = int.Parse(line[1]);
                    double birthProbability = double.Parse(line[2]);
                    BirthProbability bp = new BirthProbability()
                    {
                        Age = age,
                        NbrOfChildren = nbrOfChildren,
                        BirthP = birthProbability
                    };
                    BirthProbabilities.Add(bp);
                }
            }
            return BirthProbabilities;
        }
        private List<DeathProbability> GetDeathProbability(string path)
        {
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(';');
                    int age = int.Parse(line[1]);
                    double deathProbability = double.Parse(line[2]);
                    DeathProbability dp = new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = age,
                        DeathP = deathProbability
                    };
                    DeathProbabilities.Add(dp);
                }
            }
            return DeathProbabilities;
        }
    }

}
