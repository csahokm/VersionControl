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
        Random rng = new Random(1234);
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        public Form1()
        {
            InitializeComponent();
        }
        private List<Person> GetPopulation(string path)
        {
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
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
        private void SimStep(int year, Person person)
        {
            //Ha halott akkor kihagyjuk, ugrunk a ciklus következő lépésére
            if (!person.IsAlive) return;

            // Letároljuk az életkort, hogy ne kelljen mindenhol újraszámolni
            byte age = (byte)(year - person.BirthYear);

            // Halál kezelése
            // Halálozási valószínűség kikeresése
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.DeathP).FirstOrDefault();
            // Meghal a személy?
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

            //Születés kezelése - csak az élő nők szülnek
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                //Szülési valószínűség kikeresése
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.BirthP).FirstOrDefault();
                //Születik gyermek?
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }
        List<int> maleNumber = new List<int>();
        List<int> femaleNumber = new List<int>();
        void Simulation()
        {
            richTextBox1.Clear();
            maleNumber.Clear();
            femaleNumber.Clear();

            Population = GetPopulation(textBox1.Text);
            BirthProbabilities = GetBirthProbability(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbability(@"C:\Temp\halál.csv");
            for (int year = 2005; year <= numericUpDown1.Value; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                maleNumber.Add(nbrOfMales);
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                femaleNumber.Add(nbrOfFemales);
                Console.WriteLine(
                    string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
            }
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            Simulation();
            DisplayResults();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }
        private void DisplayResults()
        {
            int i = 0;
            for (int year = 2005; year <= numericUpDown1.Value; year++)
            {
                richTextBox1.Text += $"Szimulációs év: {year}\n\tFiúk: {maleNumber[i]}\n\tLányok: {femaleNumber[i]}\n\n";
                i = i + 1;
            }
        }
    }

}
