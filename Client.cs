using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu_BellesFleurs_SQL_C_
{
    internal class Client
    {
        private string nom;
        private string prenom;
        private int tel;
        private string mail;
        private string mdp;
        private string adresse_facturation;
        private int cb;
        private string grade;
        private int totalcommande;
        public Client()
        {
            Creation_Client();
        }

        public Client(string mail)
        {
            string connectionString = "SERVER = localhost; PORT=3306; DATABASE=fleurs; UID=root; PASSWORD=SevSurSQL2023!";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM client where mail = '{mail}';";
            MySqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
                this.nom = reader.GetString(0);
                this.prenom = reader.GetString(1);
                this.tel = Convert.ToInt32(reader.GetString(2));
                this.mdp = reader.GetString(4);
                this.adresse_facturation = reader.GetString(5);
                this.cb = Convert.ToInt32(reader.GetString(6));
                this.grade = reader.GetString(7);
            }

            connection.Close();
            this.mail = mail;


            this.totalcommande++;

        }

        public void Creation_Client()
        {
            Console.WriteLine("Créons un client : ");

            Console.WriteLine("Nom : ");
            this.nom = Console.ReadLine();

            Console.WriteLine("Prénom : ");
            this.prenom = Console.ReadLine();

            Console.WriteLine("Téléphone : ");
            this.tel = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Mail : ");
            this.mail = Console.ReadLine();

            Console.WriteLine("Mot de passe : ");
            this.mdp = Console.ReadLine();

            Console.WriteLine("Adresse de facturation :");
            this.adresse_facturation = Console.ReadLine();

            Console.WriteLine("carte bleue : ");
               this.cb = Convert.ToInt32(Console.ReadLine());

            this.grade = "Bois";
            this.totalcommande = 1;
        }

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public string Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }

        public int Tel
        {
            get { return tel; }
            set { tel = value; }
        }

        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        public string Mdp
        {
            get { return mdp; }
            set { mdp = value; }
        }

        public string AdresseFacturation
        {
            get { return adresse_facturation; }
            set { adresse_facturation = value; }
        }

        public int CB
        {
            get { return cb; }
            set { cb = value; }
        }
        public string Grade
        {
            get { return grade; }
            set { grade = value; }
        }
        public int TotalCommande
        {
            get { return this.totalcommande; }
            set { this.totalcommande = value; }
        }

    }
}
