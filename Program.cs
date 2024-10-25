using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Windows.Markup;

namespace Menu_BellesFleurs_SQL_C_
{
    internal class Program
    {
        static void Menu()
        {
            //Connection au serveur
            string connectionString = "SERVER = localhost; PORT=3306;DATABASE=fleurs;UID=root;PASSWORD=SevSurSQL2023!";
            MySqlConnection connection = new MySqlConnection(connectionString);

            Console.WriteLine("Bonjour, bienvenue sur la base de données de BelleFleurs");
            Console.WriteLine("Dans quel magasin vous trouvez-vous?");
            string magasin = Console.ReadLine();
            Console.WriteLine("Que souhaitez-vous faire ? " +
                "\n 1- Passer une commande?" +
                "\n 2- Consulter le flux");

            Console.WriteLine("Généralement il est conseillé de verifier les stocks de fleurs pour pouvoir envoyer toutes les commandes sous 3 jours");

            int reponse = 0;
            while (reponse != 1 && reponse != 2) reponse = Convert.ToInt32(Console.ReadLine());

            switch (reponse)
            {
                //Créer une commande
                case 1:
                    {
                        //INTRO
                        Console.WriteLine("Etes vous déjà client ? (oui ou non)\nAttention, il faut être sûr de votre (non)existence, sinon la commmande sera annulée.");
                        string ansewer = Console.ReadLine();
                        
                        while (ansewer != "oui" && ansewer != "non") ansewer = Console.ReadLine();

                        Client current_client;

                        //CREATION D'UN CLIENT                       
                        if (ansewer == "non")
                        {
                            current_client = new Client();
                            connection.Open();
                            MySqlCommand command1 = connection.CreateCommand();
                            command1.CommandText = $"INSERT INTO Client values ('{current_client.Nom}','{current_client.Prenom}',{current_client.Tel},'{current_client.Mail}','{current_client.Mdp}','{current_client.AdresseFacturation}',{current_client.CB},'{current_client.Grade}');";
                            command1.ExecuteNonQuery();
                            connection.Close();
                        }

                        //RECUPERATION DES INFOS DU CLIENTS
                        else
                        {
                            Console.WriteLine("Quelle est votre adresse mail?");
                            string mail = Console.ReadLine();
                            current_client = new Client(mail);
                        }

                        //CREATION DE LA COMMANDE
                        Console.WriteLine(" Créons une commande : ");
                        Console.WriteLine(" Que voulez vous commander, \n1- un accessoire\n2- un bouquet standard\n3- un bouquet personnalisé\n4- vous êtes indécis?");
                        int choix = Convert.ToInt32(Console.ReadLine());
                        string id_bon_commande = "";
                        double prix_sans_red = 0;

                        while (choix !=1 && choix != 2 && choix != 3 && choix != 4) choix = Convert.ToInt32(Console.ReadLine());
                        switch (choix)
                        {
                            case (1):
                                {
                                    connection.Open();
                                    MySqlCommand commande = connection.CreateCommand();
                                    commande.CommandText = $"SELECT \r\n\t" +
                                        $"Id_Access as accessoire,\r\n" +
                                        $"Quantite_stock as stock\r\n" +
                                        $"FROM accessoire\r\n" +
                                        $"Where Id_Access LIKE 'A_%' AND IdMag = '{magasin}';";
                                    MySqlDataReader reader;
                                    reader = commande.ExecuteReader();

                                    int Quantite_stock = 0;
                                    string Id_access = "";

                                    while (reader.Read())
                                    {
                                        Id_access = reader.GetString(0);
                                        Quantite_stock = Convert.ToInt32(reader.GetString(1));
                                        Console.WriteLine($"Il y a {Quantite_stock} {Id_access.Substring(2)}");
                                    }

                                    connection.Close();

                                    Console.WriteLine("Vous pouvez choisir entre : \n1- Vase : 20 euros\n2- Ruban : 2 euros\n3- Boite : 10 euros");
                                    int choix2 = Convert.ToInt32(Console.ReadLine());
                                    while (choix2 != 1 && choix2 != 2 && choix2 != 3) choix2 = Convert.ToInt32(Console.ReadLine());

                                    switch (choix2)
                                    {
                                        case (1):
                                            id_bon_commande = "A_vase";
                                            prix_sans_red = 20;
                                            break;
                                        case (2):
                                            id_bon_commande = "A_ruban";
                                            prix_sans_red = 2;
                                            break;
                                        case(3):
                                            id_bon_commande = "A_boites";
                                            prix_sans_red = 10;
                                            break;
                                    }
                                    connection.Open();
                                    MySqlCommand c = connection.CreateCommand();
                                    c.CommandText = $"UPDATE accessoire SET Quantite_stock = Quantite_stock - 1 WHERE Id_Access = '{id_bon_commande}' AND IdMag = '{magasin}';";
                                    c.ExecuteNonQuery();
                                    connection.Close();
                                    break;
                                }
                            case (2):
                                {
                                    connection.Open();
                                    Console.WriteLine($"Voici les stocks de bouquet standard dans notre magasin {magasin} : ");
                                    MySqlCommand command2 = connection.CreateCommand();
                                    command2.CommandText = $"SELECT \r\n\tId_Access AS Bouquet_Standard, \r\n\tQuantite_stock FROM \r\n    accessoire \r\nWHERE Id_Access LIKE 'S_%'  AND IdMag = '{magasin}' \r\nORDER BY Bouquet_Standard ASC;";
                                    MySqlDataReader reader;
                                    reader = command2.ExecuteReader();

                                    int Quantite_stock;
                                    string Id_Access;
                                    int counter = 1;

                                    while (reader.Read())
                                    {
                                        Id_Access = reader.GetString(0);
                                        Quantite_stock = Convert.ToInt32(reader.GetString(1));
                                        Console.WriteLine($"{counter}- Il y a {Quantite_stock} de {Id_Access}");
                                        counter++;
                                    }
                                    
                                    connection.Close();

                                    Console.WriteLine("Sélectionner un numéro de bouquet.");
                                    int choix3 = Convert.ToInt32(Console.ReadLine());

                                    while (choix3 > 5 && choix3 < 0) choix3 = Convert.ToInt32(Console.ReadLine());

                                    if (choix3 == 1)
                                    {
                                        id_bon_commande = "S_Ger&RosB&Lys&Al";
                                        prix_sans_red = 80;
                                    }
                                    if (choix3 == 2)
                                    {
                                        id_bon_commande = "S_Gin&Gin&Ois&Ros&Gen";
                                        prix_sans_red = 40;
                                    }
                                    if (choix3 == 3)
                                    {
                                        id_bon_commande = "S_Lys&Orch";
                                        prix_sans_red = 120;
                                    }
                                    if (choix3 == 4)
                                    {
                                        id_bon_commande = "S_Mar&Ver";
                                        prix_sans_red = 45;
                                    }
                                    if (choix3 == 5)
                                    {
                                        id_bon_commande = "S_RosB&RosR";
                                        prix_sans_red = 65;
                                    }
                                    connection.Open();
                                    MySqlCommand c1 = connection.CreateCommand();
                                    c1.CommandText = $"UPDATE accessoire SET Quantite_stock = Quantite_stock - 1 WHERE Id_Access = '{id_bon_commande}' AND IdMag = '{magasin}';";
                                    c1.ExecuteNonQuery();
                                    connection.Close();
                                    break;
                                }
                            case (3):
                                {
                                    Console.WriteLine("Voici les differentes fleurs disponibles : \n 1- Gerbera : 5,00 euros \n 2- Ginger : 4,00 euros \n 3- Glaïeul : 1,00 euros \n 4- Marguerite : 2,25 euros \n 5- Rose Rouge : 2,50 euros \n 6- STOP\n Laquelle voulez-vous mettre dans votre bouquet?");
                                    int choix_fleur = Convert.ToInt32(Console.ReadLine());
                                    while(choix_fleur != 1 && choix_fleur != 2 && choix_fleur != 3 && choix_fleur != 4 && choix_fleur != 5 && choix_fleur != 6) choix_fleur = Convert.ToInt32(Console.ReadLine());
                                    int qtGerb = 0;
                                    int qtGing = 0;
                                    int qtGla = 0;
                                    int qtMarg= 0;
                                    int qtRosR = 0;

                                    string bonGerb = "";
                                    string bonGing = "";
                                    string bonGla = "";
                                    string bonMarg = "";
                                    string bonRose = "";

                                    while (choix_fleur != 6)
                                    {
                                        switch (choix_fleur)
                                        {
                                            //Gerbera
                                            case 1:
                                                {
                                                    connection.Open();
                                                    MySqlCommand command3 = connection.CreateCommand();
                                                    command3.CommandText = $"Select Quantite_en_stock from fleur WHERE id_fleur = 'Gerbera' AND identifiant_m =  '{magasin}';";
                                                    MySqlDataReader reader;
                                                    reader = command3.ExecuteReader();
                                                    int qtGerbstock;
                                                    while (reader.Read())// parcourt ligne par ligne
                                                    {
                                                        qtGerbstock = Convert.ToInt32(reader.GetString(0));
                                                        Console.WriteLine("Nous avons " + qtGerbstock + " Gerbera en stock, combien en voulez vous ?");
                                                    }
                                                    connection.Close();
                                                    qtGerb = Convert.ToInt32(Console.ReadLine());
                                                    bonGerb = $"{qtGerb}Gerb";

                                                    //modification des stocks et qt vendue
                                                    connection.Open();
                                                    MySqlCommand c3 = connection.CreateCommand();
                                                    c3.CommandText = $"UPDATE fleur \r\nSET \r\nQuantite_en_stock = Quantite_en_stock - {qtGerb},\r\nQt_vendue = Qt_vendue + {qtGerb}\r\nWHERE Id_fleur = 'Gerbera' AND identifiant_m = '{{magasin}}';";
                                                    c3.ExecuteNonQuery();
                                                    connection.Close();

                                                    Console.WriteLine("Choisissez une autre fleur (6 pour quitter)");
                                                    choix_fleur = Convert.ToInt32(Console.ReadLine());
                                                    while (choix_fleur != 1 && choix_fleur != 2 && choix_fleur != 3 && choix_fleur != 4 && choix_fleur != 5 && choix_fleur != 6) choix_fleur = Convert.ToInt32(Console.ReadLine());
                                                    break;
                                                }
                                            //Ginger
                                            case 2:
                                                {
                                                    connection.Open();
                                                    MySqlCommand command4 = connection.CreateCommand();
                                                    command4.CommandText = $"Select Quantite_en_stock from fleur WHERE id_fleur = 'Ginger' AND identifiant_m =  '{magasin}';";
                                                    MySqlDataReader reader = command4.ExecuteReader();
                                                    int qtGinstock;
                                                    while (reader.Read())// parcourt ligne par ligne
                                                    {
                                                        qtGinstock = Convert.ToInt32(reader.GetString(0));
                                                        Console.WriteLine("Nous avons " + qtGinstock + " Ginger en stock, combien en voulez vous ?");
                                                    }
                                                    qtGing = Convert.ToInt32(Console.ReadLine());
                                                    bonGing = $"{qtGing}Ging";
                                                    connection.Close();

                                                    //modification des stocks et qt vendue
                                                    connection.Open();
                                                    MySqlCommand c4 = connection.CreateCommand();
                                                    c4.CommandText = $"UPDATE fleur \r\nSET \r\nQuantite_en_stock = Quantite_en_stock - {qtGing},\r\nQt_vendue = Qt_vendue + {qtGing}\r\nWHERE Id_fleur = 'Ginger' AND identifiant_m = '{magasin}';";
                                                    c4.ExecuteNonQuery();
                                                    connection.Close();

                                                    Console.WriteLine("Choisissez une autre fleur (6 pour quitter)");
                                                    choix_fleur = Convert.ToInt32(Console.ReadLine());
                                                    while (choix_fleur != 1 && choix_fleur != 2 && choix_fleur != 3 && choix_fleur != 4 && choix_fleur != 5 && choix_fleur != 6) choix_fleur = Convert.ToInt32(Console.ReadLine());
                                                    break;
                                                }
                                            //Glaieul
                                            case 3:
                                                {
                                                    connection.Open();
                                                    MySqlCommand command5 = connection.CreateCommand();
                                                    command5.CommandText = $"Select Quantite_en_stock from fleur WHERE id_fleur = 'Glaieul' AND identifiant_m =  '{magasin}';";
                                                    MySqlDataReader reader = command5.ExecuteReader();
                                                    int qtGlaieulstock;
                                                    while (reader.Read())// parcourt ligne par ligne
                                                    {
                                                        qtGlaieulstock = Convert.ToInt32(reader.GetString(0));
                                                        Console.WriteLine("Nous avons " + qtGlaieulstock + " Glaieul en stock, combien en voulez vous ?");
                                                    }
                                                    qtGla = Convert.ToInt32(Console.ReadLine());
                                                    bonGla = $"{qtGla}Gla";
                                                    connection.Close();

                                                    //modification des stocks et qt vendue
                                                    connection.Open();
                                                    MySqlCommand c5 = connection.CreateCommand();
                                                    c5.CommandText = $"UPDATE fleur \r\nSET \r\nQuantite_en_stock = Quantite_en_stock - {qtGla},\r\nQt_vendue = Qt_vendue + {qtGla}\r\nWHERE Id_fleur = 'Gerbera' AND identifiant_m = '{magasin}';";
                                                    c5.ExecuteNonQuery();
                                                    connection.Close();

                                                    Console.WriteLine("Choisissez une autre fleur (6 pour quitter)");
                                                    choix_fleur = Convert.ToInt32(Console.ReadLine());
                                                    while (choix_fleur != 1 && choix_fleur != 2 && choix_fleur != 3 && choix_fleur != 4 && choix_fleur != 5 && choix_fleur != 6) choix_fleur = Convert.ToInt32(Console.ReadLine());
                                                    break;
                                                }
                                            //Marguerite 
                                            case 4:
                                                {
                                                    connection.Open();
                                                    MySqlCommand command6 = connection.CreateCommand();
                                                    command6.CommandText = $"Select Quantite_en_stock from fleur WHERE id_fleur = 'Marguerite' AND identifiant_m =  '{magasin}';";
                                                    MySqlDataReader reader = command6.ExecuteReader();
                                                    int qtMargueStock;
                                                    while (reader.Read())// parcourt ligne par ligne
                                                    {
                                                        qtMargueStock = Convert.ToInt32(reader.GetString(0));
                                                        Console.WriteLine("Nous avons " + qtMargueStock + " Marguerite en stock, combien en voulez vous ?");
                                                    }
                                                    connection.Close();
                                                    qtMarg = Convert.ToInt32(Console.ReadLine());
                                                    bonMarg = $"{qtMarg}Marg";

                                                    //modification des stocks et qt vendue
                                                    connection.Open();
                                                    MySqlCommand c6 = connection.CreateCommand();
                                                    c6.CommandText = $"UPDATE fleur \r\nSET \r\nQuantite_en_stock = Quantite_en_stock - {qtMarg},\r\nQt_vendue = Qt_vendue + {qtMarg}\r\nWHERE Id_fleur = 'Gerbera' AND identifiant_m = '{magasin}';";
                                                    c6.ExecuteNonQuery();
                                                    connection.Close();

                                                    Console.WriteLine("Choisissez une autre fleur (6 pour quitter)");
                                                    choix_fleur = Convert.ToInt32(Console.ReadLine());
                                                    while (choix_fleur != 1 && choix_fleur != 2 && choix_fleur != 3 && choix_fleur != 4 && choix_fleur != 5 && choix_fleur != 6) choix_fleur = Convert.ToInt32(Console.ReadLine());
                                                    break;
                                                }
                                            //Rose Rouge
                                            case 5:
                                                {
                                                    connection.Open();
                                                    MySqlCommand command7 = connection.CreateCommand();
                                                    command7.CommandText = $"Select Quantite_en_stock from fleur WHERE id_fleur = 'Rose Rouge' AND identifiant_m =  '{magasin}';";
                                                    MySqlDataReader reader = command7.ExecuteReader();
                                                    int qtRoseRStock;
                                                    while (reader.Read())// parcourt ligne par ligne
                                                    {
                                                        qtRoseRStock = Convert.ToInt32(reader.GetString(0));
                                                        Console.WriteLine("Nous avons " + qtRoseRStock + " Rose rouge en stock, combien en voulez vous ?");
                                                    }
                                                    connection.Close();
                                                    qtRosR = Convert.ToInt32(Console.ReadLine());
                                                    bonRose = $"{qtRosR}RosR";

                                                    //modification des stocks et qt vendue
                                                    connection.Open();
                                                    MySqlCommand c7 = connection.CreateCommand();
                                                    c7.CommandText = $"UPDATE fleur \r\nSET \r\nQuantite_en_stock = Quantite_en_stock - {qtRosR},\r\nQt_vendue = Qt_vendue + {qtRosR}\r\nWHERE Id_fleur = 'Gerbera' AND identifiant_m = '{magasin}';";
                                                    c7.ExecuteNonQuery();
                                                    connection.Close();

                                                    Console.WriteLine("Choisissez une autre fleur (6 pour quitter)");
                                                    choix_fleur = Convert.ToInt32(Console.ReadLine());
                                                    while (choix_fleur != 1 && choix_fleur != 2 && choix_fleur != 3 && choix_fleur != 4 && choix_fleur != 5 && choix_fleur != 6) choix_fleur = Convert.ToInt32(Console.ReadLine());
                                                    break;
                                                }
                                        }
                                    }
                                    id_bon_commande = "P_";

                                    int[] tab = new int[] { qtGerb, qtGing, qtGla, qtMarg, qtRosR };
                                    double[] prixfleur = new double[] { 5, 4, 1, 2.25, 2.50 };
                                    string[] tab2 = new string[] { bonGerb, bonGing, bonGla, bonMarg, bonRose };

                                    for (int i = 0; i < tab.Length; i++)
                                    {
                                        if (tab[i] == 0) continue;
                                        prix_sans_red += tab[i] * prixfleur[i];
                                        id_bon_commande += tab2[i] + "&";
                                    }
                                    id_bon_commande = id_bon_commande.TrimEnd('&');
                                    break;
                                }
                            case (4):
                                {
                                    int qtGerb = 0;
                                    int qtGing = 0;
                                    int qtGla = 0;
                                    int qtMarg = 0;
                                    int qtRosR = 0;

                                    string bonGerb = "Gerb";
                                    string bonGing = "Ging";
                                    string bonGla = "Gla";
                                    string bonMarg = "Marg";
                                    string bonRose = "RosR";

                                    id_bon_commande = "P_";

                                    Console.WriteLine("Quel est votre budget ?");
                                    double budget = Convert.ToDouble(Console.ReadLine());
                                    prix_sans_red = 0;
                                    double reste = budget;
                                    while (reste > 1.0 && budget >= prix_sans_red)
                                    {
                                        Console.WriteLine("Quelle fleur voulez-vous ajouter au bouquet ?");
                                        Console.WriteLine("1 - Gerbera (5,00€)");
                                        Console.WriteLine("2 - Ginger (4,00€)");
                                        Console.WriteLine("3 - Glaïeul (1,00€)");
                                        Console.WriteLine("4 - Marguerite (2,25€)");
                                        Console.WriteLine("5 - Rose Rouge (2,50€)");
                                        int choice = Convert.ToInt32(Console.ReadLine());

                                        switch (choice)
                                        {
                                            case 1:
                                                if (prix_sans_red + 5.00 <= budget)
                                                {
                                                    qtGerb++;
                                                    prix_sans_red += 5.00;
                                                    reste -= 5.00;
                                                    Console.WriteLine($"Gerbera ajouté au bouquet, il vous reste {reste} euros.");
                                                    //modification des stocks et qt vendue
                                                    connection.Open();
                                                    MySqlCommand c8 = connection.CreateCommand();
                                                    c8.CommandText = $"UPDATE fleur \r\nSET \r\nQuantite_en_stock = Quantite_en_stock - {qtGerb},\r\nQt_vendue = Qt_vendue + {qtGerb}\r\nWHERE Id_fleur = 'Gerbera' AND identifiant_m = '{magasin}';";
                                                    c8.ExecuteNonQuery();
                                                    connection.Close();
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"Budget insuffisant pour ajouter un Gerbera, il vous reste {reste} euros.");
                                                }
                                                break;
                                            case 2:
                                                if (prix_sans_red + 4.00 <= budget)
                                                {
                                                    qtGing++;
                                                    prix_sans_red += 4.00;
                                                    reste -= 4.00;
                                                    Console.WriteLine($"Ginger ajouté au bouquet, il vous reste {reste} euros.");
                                                    //modification des stocks et qt vendue
                                                    connection.Open();
                                                    MySqlCommand c9 = connection.CreateCommand();
                                                    c9.CommandText = $"UPDATE fleur \r\nSET \r\nQuantite_en_stock = Quantite_en_stock - {qtGing},\r\nQt_vendue = Qt_vendue + {qtGing}\r\nWHERE Id_fleur = 'Gerbera' AND identifiant_m = '{magasin}';";
                                                    c9.ExecuteNonQuery();
                                                    connection.Close();
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"Budget insuffisant pour ajouter un Ginger, il vous reste {reste} euros.");
                                                }
                                                break;
                                            case 3:
                                                if (prix_sans_red + 1.00 <= budget)
                                                {
                                                    qtGla++;
                                                    prix_sans_red += 1.00;
                                                    reste -= 1.00;
                                                    Console.WriteLine($"Glaïeul ajouté au bouquet, il vous reste {reste} euros.");
                                                    //modification des stocks et qt vendue
                                                    connection.Open();
                                                    MySqlCommand c10 = connection.CreateCommand();
                                                    c10.CommandText = $"UPDATE fleur \r\nSET \r\nQuantite_en_stock = Quantite_en_stock - {qtGla},\r\nQt_vendue = Qt_vendue + {qtGla}\r\nWHERE Id_fleur = 'Gerbera' AND identifiant_m = '{magasin}';";
                                                    c10.ExecuteNonQuery();
                                                    connection.Close();
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"Budget insuffisant pour ajouter un Glaïeul, il vous reste {reste} euros.");
                                                }
                                                break;
                                            case 4:
                                                if (prix_sans_red + 2.25 <= budget)
                                                {
                                                    qtMarg++;
                                                    prix_sans_red += 2.25;
                                                    reste -= 2.25;
                                                    Console.WriteLine($"Marguerite ajoutée au bouquet, il vous reste {reste} euros.");
                                                    //modification des stocks et qt vendue
                                                    connection.Open();
                                                    MySqlCommand c11 = connection.CreateCommand();
                                                    c11.CommandText = $"UPDATE fleur \r\nSET \r\nQuantite_en_stock = Quantite_en_stock - {qtMarg},\r\nQt_vendue = Qt_vendue + {qtMarg}\r\nWHERE Id_fleur = 'Gerbera' AND identifiant_m = '{magasin}';";
                                                    c11.ExecuteNonQuery();
                                                    connection.Close();
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"Budget insuffisant pour ajouter une Marguerite, il vous reste {reste} euros.");
                                                }
                                                break;
                                            case 5:
                                                if (prix_sans_red + 2.50 <= budget)
                                                {
                                                    qtRosR++;
                                                    prix_sans_red += 2.50;
                                                    reste -= 2.50;
                                                    Console.WriteLine($"Rose Rouge ajoutée au bouquet, il vous reste {reste} euros.");
                                                    //modification des stocks et qt vendue
                                                    connection.Open();
                                                    MySqlCommand c12 = connection.CreateCommand();
                                                    c12.CommandText = $"UPDATE fleur \r\nSET \r\nQuantite_en_stock = Quantite_en_stock - {qtRosR},\r\nQt_vendue = Qt_vendue + {qtRosR}\r\nWHERE Id_fleur = 'Gerbera' AND identifiant_m = '{magasin}';";
                                                    c12.ExecuteNonQuery();
                                                    connection.Close();
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"Budget insuffisant pour ajouter une Rose Rouge, il vous reste {reste} euros.");
                                                }
                                                break;
                                            default:
                                                Console.WriteLine("Choix invalide");
                                                break;
                                        }
                                    }

                                    int[] tab = new int[] { qtGerb, qtGing, qtGla, qtMarg, qtRosR };
                                    double[] prixfleur = new double[] { 5, 4, 1, 2.25, 2.50 };
                                    string[] tab2 = new string[] { bonGerb, bonGing, bonGla, bonMarg, bonRose };

                                    for (int i = 0; i < tab.Length; i++)
                                    {
                                        if (tab[i] == 0) continue;
                                        prix_sans_red += tab[i] * prixfleur[i];
                                        id_bon_commande += tab[i].ToString() + tab2[i] + "&";
                                    }
                                    id_bon_commande = id_bon_commande.TrimEnd('&'); //supprime le dernier &  du bon de commande
                                    break;
                                }
                        }

                        Console.WriteLine("Quelle adresse de livraison ? ");
                        string adresse_liv = Console.ReadLine();

                        Console.WriteLine("Quelle date de livraison souhaitez-vous ? \n format : AAAA/MM/JJ");
                        DateTime dateliv = Convert.ToDateTime(Console.ReadLine());
                        //string date_str_1 = $"'{dateliv.Year}-{dateliv.Month}-{dateliv.Day}'";

                        Console.WriteLine("Tapez votre message ? ");
                        string message = Console.ReadLine();

                        DateTime date_comm = DateTime.Now;
                        //string date_str_2 = $"'{date_comm.Year}-{date_comm.Month}-{date_comm.Day}'";

                        string etat = "CC";
                        
                        int reduc = 0;
                        if (current_client.Grade == "Or") reduc = 15;
                        if (current_client.Grade == "Bronze") reduc = 5;
                        if (current_client.Grade == "Bois") reduc = 0;
                        double reduc_double = Convert.ToDouble(reduc);
                        double prix_final = prix_sans_red * (1.00 - reduc_double / 100);

                        connection.Open();

                        string sqlCommand9 = "INSERT INTO bon_Commande (id_BonCommande, nomClient, adresse_liv, dateDesiree, message) VALUES (@id_bon_commande, @nomClient, @adresse_liv, @dateDesiree, @message);";

                        MySqlCommand command9 = new MySqlCommand(sqlCommand9, connection);
                        command9.Parameters.AddWithValue("@id_bon_commande", id_bon_commande);
                        command9.Parameters.AddWithValue("@nomClient", current_client.Mail);
                        command9.Parameters.AddWithValue("@adresse_liv", adresse_liv);
                        command9.Parameters.AddWithValue("@dateDesiree", dateliv);
                        command9.Parameters.AddWithValue("@message", message);

                        command9.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();

                        string sqlCommand10 = "INSERT INTO Commande (Id_bon_commande, dateCommande, etat, Id_client, reduction, prix_final) VALUES (@Id_bon_commande, @dateCommande, @etat, @Id_client, @reduction, @prix_final);";

                        MySqlCommand command10 = new MySqlCommand(sqlCommand10, connection);
                        command10.Parameters.AddWithValue("@Id_bon_commande", id_bon_commande);
                        command10.Parameters.AddWithValue("@dateCommande", date_comm);
                        command10.Parameters.AddWithValue("@etat", etat);
                        command10.Parameters.AddWithValue("@Id_client", current_client.Mail);
                        command10.Parameters.AddWithValue("@reduction", reduc);
                        command10.Parameters.AddWithValue("@prix_final", prix_final);

                        command10.ExecuteNonQuery();
                        connection.Close();
                        
                        connection.Open();
                        
                        string sqlCommand11 = "INSERT IGNORE INTO produit (Id_bon_commande, Prix_produit) VALUES (@Id_bon_commande, @Prix_produit);";
                        MySqlCommand command11 = new MySqlCommand(sqlCommand11, connection);
                        command11.Parameters.AddWithValue("@Id_bon_commande", id_bon_commande);
                        command11.Parameters.AddWithValue("@Prix_produit", prix_sans_red);

                        command11.ExecuteNonQuery();
                        connection.Close();

                        connection.Open();

                        string sqlCommand12 = "INSERT IGNORE INTO bouquet (Id_bouquet, prix_bouquet) VALUES (@Id_bouquet, @prix_bouquet);";
                        MySqlCommand command12 = new MySqlCommand(sqlCommand12, connection);
                        command12.Parameters.AddWithValue("@Id_bouquet", id_bon_commande);
                        command12.Parameters.AddWithValue("@prix_bouquet", prix_sans_red);
                        command12.ExecuteNonQuery();
                        connection.Close();

                        Console.Write($"Le prix finale à payer est donc de {prix_final} Euros");                       
                        break;
                    }
                //Gérer le flux
                case (2):
                    {
                        //EXPLICATION DU FLUX : 
                        //Il y a un switch à 4 cases (chaque case représente une méthode ci dessous.
                        //Dans statistique il y a un switch  à 8 case (et chaque case représente une méthode, propore à la statistique recherchée)
                        Console.WriteLine("Bienvenue dans le menu flux, que souhaitez vous faire ?\n" +
                            "1- Vérifier les stocks\n" +
                            "2- Voir les statistiques\n" +
                            "3- Voir les clients\n" +
                            "4- Vérifier l'état des commandes\n" +
                            "5- Quitter le flux.\n");

                        //Faire en sorte que le choix soit compris entre 1 et 5
                        int choice = Convert.ToInt32(Console.ReadLine());
                        while (choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 5) Convert.ToInt32(Console.ReadLine());

                        static void VérifierLesStocks(MySqlConnection connection)
                        {
                            connection.Open();
                            MySqlCommand com = connection.CreateCommand();
                            Console.WriteLine("Voici les produit ou la quantité en stock est inférieur à 5 : \n");
                            com.CommandText = "SELECT \r\n\tdistinct Id_magasin, \r\n    Id_fleur as produit, \r\n    Quantite_en_stock\r\nFROM magasin\r\nLEFT JOIN fleur \r\nON magasin.Id_magasin = fleur.identifiant_m\r\nwhere Quantite_en_stock < 5\r\nUNION ALL\r\nSELECT \r\n\tdistinct Id_magasin, \r\n\tId_Access as produit, \r\n    Quantite_stock\r\nFROM magasin\r\nLEFT JOIN accessoire \r\nON magasin.Id_magasin = accessoire.IdMag\r\nWHERE Quantite_stock < 5\r\nORDER BY Quantite_en_stock;";
                            MySqlDataReader reader2;
                            reader2 = com.ExecuteReader();
                            while (reader2.Read()) Console.WriteLine($"\nIl y a {reader2.GetString(2)} {reader2.GetString(1)} dans le magasin {reader2.GetString(0)};\n" +
                                $"des {reader2.GetString(1)} ont été commandées au magasin {reader2.GetString(0)}");
                            connection.Close();


                            connection.Open();
                            MySqlCommand com2 = connection.CreateCommand();
                            com2.CommandText = "UPDATE fleur\r\nSET Quantite_en_stock = Quantite_en_stock + 10\r\nWHERE Quantite_en_stock < 5;";
                            com2.ExecuteNonQuery();
                            connection.Close();

                            connection.Open();
                            MySqlCommand com3 = connection.CreateCommand();
                            com3.CommandText = "UPDATE accessoire\r\nSET Quantite_stock = Quantite_stock + 10\r\nWHERE Quantite_stock < 5;\r\n";
                            com3.ExecuteNonQuery();
                            connection.Close();
                        }
                        static void Cara_Client(MySqlConnection connection)
                        {
                            connection.Open();
                            Console.WriteLine("De quel client souhaitez vous voir les informations ? " +
                                "Selectionnez son mail ");
                            string mail = Console.ReadLine();
                            MySqlCommand command = connection.CreateCommand();
                            command.CommandText = $"SELECT \t\r\n\tnom, \r\n\tprenom, \r\n\t f_grade as fidelite,\r\n\t commande.Id_bon_commande,\r\n\t commande.dateCommande\r\nFROM client\r\nJOIN commande \r\nON client.mail = commande.Id_client\r\nwhere mail = '{mail}';";

                            MySqlDataReader reader;
                            reader = command.ExecuteReader();

                            while (reader.Read())// parcourt ligne par ligne
                            {
                                Console.WriteLine($"{reader.GetString(1)} {reader.GetString(0)} a une fidélité de {reader.GetString(2)} et a commandé {reader.GetString(3)} le {reader.GetString(4)}");
                            }
                            connection.Close();
                        }
                        static void Etat_Commande(MySqlConnection connection)
                        {
                            Console.WriteLine("Arbitrairement, vous ne pouvez que vérifier l'état des commandes à venir.\n" +
                                "Selectionnez le numéro du mois de l'année 2023 dont vous voulez avoir les informations.");
                            int month = Convert.ToInt32(Console.ReadLine());

                            connection.Open();
                            MySqlCommand comand = connection.CreateCommand();
                            comand.CommandText = $"SELECT \r\n\tcommande.Id_bon_commande,\r\n    Id_client,\r\n    etat,\r\n    dateDesiree\r\nFROM commande\r\nINNER JOIN bon_commande ON commande.Id_bon_commande = bon_commande.Id_BonCommande\r\nWHERE YEAR(dateDesiree) = 2023 AND MONTH(dateDesiree) = {month};";
                            MySqlDataReader reader = comand.ExecuteReader();
                            if (reader.FieldCount == 0) Console.WriteLine("\nIl n'y a pas eu de commande à cette date");
                            while (reader.Read())// parcourt ligne par ligne
                            {
                                Console.WriteLine($"Il y un un {reader.GetString(0)} à livrer à {reader.GetString(1)}. L'état de la commande est {reader.GetString(2)}.\n" +
                                    $"Il faut livrer avent le {reader.GetString(3)}");
                            }
                            connection.Close();
                        }
                        static void Statistiques(MySqlConnection connection)
                        {
                            Console.WriteLine("Quelle stat voulez vous voir ?\n" +
                                "1- Le magasin avec le plus de CA\n" +
                                "2- Le magasin avec le moins de CA\n" +
                                "3- Le prix moyen d'un bouquet\n" +
                                "4- La fleur exotique la moins vendue\n" +
                                "5- Le meuilleur client de l'année\n" +
                                "6- Le meilleur client du mois\n" +
                                "7- Le client qui a fait le plus de commandes\n" +
                                "8- Quitter Statistiques.");
                            static void Meilleur_CA(MySqlConnection connection)
                            {
                                connection.Open();
                                MySqlCommand command = connection.CreateCommand();
                                command.CommandText =
                                 " SELECT Id_magasin, CA FROM magasin ORDER BY CA DESC LIMIT 1;";

                                MySqlDataReader reader;
                                reader = command.ExecuteReader();

                                string id_magasin;
                                string CA;
                                while (reader.Read())// parcourt ligne par ligne
                                {

                                    id_magasin = reader.GetString(0);  // récupération de la 1ère colonne 
                                    CA = reader.GetString(1); // récupération de la 2ème
                                    Console.WriteLine("l'ID du magasin avec le meuilleur CA est " + id_magasin + " et son CA est de " + CA + " euros");
                                }
                                connection.Close();
                            }
                            static void Pire_CA(MySqlConnection connection)
                            {
                                connection.Open();
                                MySqlCommand command = connection.CreateCommand();
                                command.CommandText =
                                 " SELECT Id_magasin, CA FROM magasin ORDER BY CA ASC LIMIT 1;";

                                MySqlDataReader reader;
                                reader = command.ExecuteReader();

                                string id_magasin;
                                string CA;
                                while (reader.Read())// parcourt ligne par ligne
                                {
                                    id_magasin = reader.GetString(0);  // récupération de la 1ère colonne 
                                    CA = reader.GetString(1);
                                    Console.WriteLine("l'ID du magasin avec le pire CA est " + id_magasin + " et son CA est de " + CA + " euros");
                                }
                                connection.Close();
                            }
                            static void Moyenne_bouqet(MySqlConnection connection)
                            {
                                connection.Open();
                                MySqlCommand command = connection.CreateCommand();
                                command.CommandText =
                                 "SELECT AVG(Prix_produit) AS prix_moyen \r\nFROM produit\r\nWHERE  produit.Id_bon_commande NOT LIKE 'A_%'; ";

                                MySqlDataReader reader;
                                reader = command.ExecuteReader();
                                while(reader.Read()) Console.WriteLine($"Le prix moyen d'un bouquet est de {reader.GetString(0)} euros.");
                                connection.Close();
                            }
                            static void Exotique(MySqlConnection connection)
                            {
                                connection.Open();
                                MySqlCommand command = connection.CreateCommand();
                                command.CommandText =
                                 "Select Id_fleur, identifiant_m, Qt_vendue from fleur order by Qt_vendue ASC limit 1; ";

                                MySqlDataReader reader;
                                reader = command.ExecuteReader();
                                while (reader.Read())// parcourt ligne par ligne
                                {
                                    Console.WriteLine($"La fleur exotique la moins vendue se trouve dans le magasin {reader.GetString(1)}, c'est la {reader.GetString(0)} : {reader.GetString(2)} ont été vendues!");
                                }
                                connection.Close();
                            }
                            static void YEAR_CUSTOMER(MySqlConnection connection)
                            {
                                connection.Open();
                                MySqlCommand cm8 = connection.CreateCommand();
                                cm8.CommandText = "SELECT \r\ncommande.Id_client AS BEST_CUSTOMER,\r\nSUM(prix_final) AS Total_achat \r\nFROM commande \r\nWHERE YEAR(commande.dateCommande) = 2022\r\nGROUP BY BEST_CUSTOMER\r\nORDER BY Total_achat DESC \r\nLIMIT 1;";
                                MySqlDataReader reader;
                                reader = cm8.ExecuteReader();
                                while(reader.Read()) Console.WriteLine($"Le meilleur client de l'année est {reader.GetString(0)} avec une commande à {reader.GetString(1)}");
                                connection.Close();
                            }
                            static void MONTH_CUSTOMER(MySqlConnection connection)
                            {
                                connection.Open();
                                MySqlCommand com5 = connection.CreateCommand();
                                com5.CommandText = "SELECT \r\ncommande.Id_client AS BEST_CUSTOMER,\r\nSUM(prix_final) AS Total_achat \r\nFROM commande \r\nWHERE MONTH(commande.dateCommande) = 2\r\nGROUP BY BEST_CUSTOMER\r\nORDER BY Total_achat DESC \r\nLIMIT 1;";
                                MySqlDataReader reader;
                                reader = com5.ExecuteReader();
                                while(reader.Read()) Console.WriteLine($"Le meilleur client du mois d'aout est {reader.GetString(0)} avec une commande à {reader.GetString(1)}");
                                connection.Close();
                            }
                            static void MaxCommande(MySqlConnection connection)
                            {
                                connection.Open();
                                MySqlCommand command = connection.CreateCommand();
                                command.CommandText = "SELECT nomClient, COUNT(*) AS total_commande\r\nFROM bon_commande\r\nGROUP BY nomClient\r\nORDER BY total_commande DESC\r\nLIMIT 1;";
                                MySqlDataReader reader;
                                reader = command.ExecuteReader();
                                while(reader.Read()) Console.WriteLine($"Le client qui a fait le plus de commandes est {reader.GetString(0)} avec un total de {reader.GetString(1)} commandes.");
                                connection.Close();
                            }
                            int choixStr = Convert.ToInt32(Console.ReadLine());
                            while (choixStr != 1 && choixStr != 2 && choixStr != 3 && choixStr != 4 && choixStr != 5 && choixStr != 6 && choixStr != 7 && choixStr != 8) Convert.ToInt32(Console.ReadLine());
                            while(choixStr != 8)
                            {
                                switch (choixStr)
                                {
                                    case 1:
                                        {
                                            Meilleur_CA(connection);
                                            Console.WriteLine("Quelle stat voulez vous voir ?\n" +
                                                            "1- Le magasin avec le plus de CA\n" +
                                                            "2- Le magasin avec le moins de CA\n" +
                                                            "3- Le prix moyen d'un bouquet\n" +
                                                            "4- La fleur exotique la moins vendue\n" +
                                                            "5- Le meuilleur client de l'année\n" +
                                                            "6- Le meilleur client du mois\n" +
                                                            "7- Le client qui a fait le plus de commandes\n" +
                                                            "8- Quitter Statistiques.");
                                            choixStr = Convert.ToInt32(Console.ReadLine());
                                            while (choixStr != 1 && choixStr != 2 && choixStr != 3 && choixStr != 4 && choixStr != 5 && choixStr != 6 && choixStr != 7 && choixStr != 8) Convert.ToInt32(Console.ReadLine());
                                            break;
                                        }
                                    case 2:
                                        {
                                            Pire_CA(connection);
                                            Console.WriteLine("Quelle stat voulez vous voir ?\n" +
                                                            "1- Le magasin avec le plus de CA\n" +
                                                            "2- Le magasin avec le moins de CA\n" +
                                                            "3- Le prix moyen d'un bouquet\n" +
                                                            "4- La fleur exotique la moins vendue\n" +
                                                            "5- Le meuilleur client de l'année\n" +
                                                            "6- Le meilleur client du mois\n" +
                                                            "7- Le client qui a fait le plus de commandes\n" +
                                                            "8- Quitter Statistiques.");
                                            choixStr = Convert.ToInt32(Console.ReadLine());
                                            while (choixStr != 1 && choixStr != 2 && choixStr != 3 && choixStr != 4 && choixStr != 5 && choixStr != 6 && choixStr != 7 && choixStr != 8) Convert.ToInt32(Console.ReadLine());
                                            break;
                                        }
                                    case 3:
                                        {
                                            Moyenne_bouqet(connection);
                                            Console.WriteLine("Quelle stat voulez vous voir ?\n" +
                                                            "1- Le magasin avec le plus de CA\n" +
                                                            "2- Le magasin avec le moins de CA\n" +
                                                            "3- Le prix moyen d'un bouquet\n" +
                                                            "4- La fleur exotique la moins vendue\n" +
                                                            "5- Le meuilleur client de l'année\n" +
                                                            "6- Le meilleur client du mois\n" +
                                                            "7- Le client qui a fait le plus de commandes\n" +
                                                            "8- Quitter Statistiques.");
                                            choixStr = Convert.ToInt32(Console.ReadLine());
                                            while (choixStr != 1 && choixStr != 2 && choixStr != 3 && choixStr != 4 && choixStr != 5 && choixStr != 6 && choixStr != 7 && choixStr != 8) Convert.ToInt32(Console.ReadLine());
                                            break;
                                        }
                                    case 4:
                                        {
                                            Exotique(connection);
                                            Console.WriteLine("Quelle stat voulez vous voir ?\n" +
                                                            "1- Le magasin avec le plus de CA\n" +
                                                            "2- Le magasin avec le moins de CA\n" +
                                                            "3- Le prix moyen d'un bouquet\n" +
                                                            "4- La fleur exotique la moins vendue\n" +
                                                            "5- Le meuilleur client de l'année\n" +
                                                            "6- Le meilleur client du mois\n" +
                                                            "7- Le client qui a fait le plus de commandes\n" +
                                                            "8- Quitter Statistiques.");
                                            choixStr = Convert.ToInt32(Console.ReadLine());
                                            while (choixStr != 1 && choixStr != 2 && choixStr != 3 && choixStr != 4 && choixStr != 5 && choixStr != 6 && choixStr != 7 && choixStr != 8) Convert.ToInt32(Console.ReadLine());
                                            break;
                                        }
                                    case 5:
                                        {
                                            YEAR_CUSTOMER(connection);
                                            Console.WriteLine("Quelle stat voulez vous voir ?\n" +
                                                            "1- Le magasin avec le plus de CA\n" +
                                                            "2- Le magasin avec le moins de CA\n" +
                                                            "3- Le prix moyen d'un bouquet\n" +
                                                            "4- La fleur exotique la moins vendue\n" +
                                                            "5- Le meuilleur client de l'année\n" +
                                                            "6- Le meilleur client du mois\n" +
                                                            "7- Le client qui a fait le plus de commandes\n" +
                                                            "8- Quitter Statistiques.");
                                            choixStr = Convert.ToInt32(Console.ReadLine());
                                            while (choixStr != 1 && choixStr != 2 && choixStr != 3 && choixStr != 4 && choixStr != 5 && choixStr != 6 && choixStr != 7 && choixStr != 8) Convert.ToInt32(Console.ReadLine());
                                            break;
                                        }
                                    case 6:
                                        {
                                            MONTH_CUSTOMER(connection);
                                            Console.WriteLine("Quelle stat voulez vous voir ?\n" +
                                                            "1- Le magasin avec le plus de CA\n" +
                                                            "2- Le magasin avec le moins de CA\n" +
                                                            "3- Le prix moyen d'un bouquet\n" +
                                                            "4- La fleur exotique la moins vendue\n" +
                                                            "5- Le meuilleur client de l'année\n" +
                                                            "6- Le meilleur client du mois\n" +
                                                            "7- Le client qui a fait le plus de commandes\n" +
                                                            "8- Quitter Statistiques.");
                                            choixStr = Convert.ToInt32(Console.ReadLine());
                                            while (choixStr != 1 && choixStr != 2 && choixStr != 3 && choixStr != 4 && choixStr != 5 && choixStr != 6 && choixStr != 7 && choixStr != 8) Convert.ToInt32(Console.ReadLine());
                                            break;
                                        }
                                    case 7:
                                        {
                                            MaxCommande(connection);
                                            Console.WriteLine("Quelle stat voulez vous voir ?\n" +
                                                            "1- Le magasin avec le plus de CA\n" +
                                                            "2- Le magasin avec le moins de CA\n" +
                                                            "3- Le prix moyen d'un bouquet\n" +
                                                            "4- La fleur exotique la moins vendue\n" +
                                                            "5- Le meuilleur client de l'année\n" +
                                                            "6- Le meilleur client du mois\n" +
                                                            "7- Le client qui a fait le plus de commandes\n" +
                                                            "8- Quitter Statistiques.");
                                            choixStr = Convert.ToInt32(Console.ReadLine());
                                            while (choixStr != 1 && choixStr != 2 && choixStr != 3 && choixStr != 4 && choixStr != 5 && choixStr != 6 && choixStr != 7 && choixStr != 8) Convert.ToInt32(Console.ReadLine());
                                            break;
                                        }
                                }
                            }
                        }

                        //Tant que "Quitter le flux n'est pas selectionner", continuer.
                        while (choice != 5)
                        {
                            switch (choice)
                            {
                                case 1:
                                    {
                                        VérifierLesStocks(connection);
                                        Console.WriteLine("1- Vérifier les stocks\n" +
                                                            "2- Voir les statistiques\n" +
                                                            "3- Voir les clients\n" +
                                                            "4- Vérifier l'état des commandes\n" +
                                                            "5- Quitter le flux.");
                                        choice = Convert.ToInt32(Console.ReadLine());
                                        while (choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 5) Convert.ToInt32(Console.ReadLine());
                                        break;
                                    }
                                case 2:
                                    {
                                        Statistiques(connection);
                                        Console.WriteLine("1- Vérifier les stocks\n" +
                                                        "2- Voir les statistiques\n" +
                                                        "3- Voir les clients\n" +
                                                        "4- Vérifier l'état des commandes\n" +
                                                        "5- Quitter le flux.");
                                        choice = Convert.ToInt32(Console.ReadLine());
                                        while (choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 5) Convert.ToInt32(Console.ReadLine());
                                        break;
                                    }
                                case 3:
                                    {
                                        Cara_Client(connection);
                                        Console.WriteLine("1- Vérifier les stocks\n" +
                                                        "2- Voir les statistiques\n" +
                                                        "3- Voir les clients\n" +
                                                        "4- Vérifier l'état des commandes\n" +
                                                        "5- Quitter le flux.");
                                        choice = Convert.ToInt32(Console.ReadLine());
                                        while (choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 5) Convert.ToInt32(Console.ReadLine());
                                        break;
                                    }
                                case 4:
                                    {
                                        Etat_Commande(connection);
                                        Console.WriteLine("1- Vérifier les stocks\n" +
                                                        "2- Voir les statistiques\n" +
                                                        "3- Voir les clients\n" +
                                                        "4- Vérifier l'état des commandes\n" +
                                                        "5- Quitter le flux.");
                                        choice = Convert.ToInt32(Console.ReadLine());
                                        while (choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 5) Convert.ToInt32(Console.ReadLine());
                                        break;
                                    }
                            }
                        }
                        break;
                    }
            }
        }
        static void Main(string[] args)
        {
            Menu();
            Console.ReadLine();
        }
    }
}