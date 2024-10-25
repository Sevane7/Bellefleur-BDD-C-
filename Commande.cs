using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu_BellesFleurs_SQL_C_
{
    internal class Commande
    {
        private Client client;
        private string Id_Commande;

        public Commande(Client client, string id_Commande)
        {
            this.client = client;
            this.Id_Commande = id_Commande;
        }
    }
}
