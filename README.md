# Projet : Le Pouvoir des Fleurs
## Modélisation de données et gestion de boutiques florales

### Introduction
Depuis plus de vingt ans, Michel Bellefleur possède plusieurs boutiques d’arrangements floraux. Pour mieux servir ses clients, il souhaite moderniser son système d’information. Dans ce projet, nous avons :
- Conçu un schéma Entité/Association pour représenter les boutiques de Michel Bellefleur.
- Créé une base de données MySQL basée sur cette modélisation.
- Développé une application de gestion des boutiques en C# avec Visual Studio.

### Description du Système

#### Schéma E/A
Le schéma Entité/Association est structuré de la manière suivante :
- **Client** : relié à un niveau de **fidélité** et à une **commande** via un bon de commande.
- **Commande** : associée à un **magasin**, un **accessoire**, et un **bouquet**.
- **Bouquet** : composé de plusieurs **fleurs**, et chaque fleur peut apparaître dans plusieurs bouquets.
 
#### Modèle de Données (SQL)
Le modèle de données comporte 9 tables : `Client`, `Commande`, `BonDeCommande`, `Fidélité`, `Magasin`, `Produit`, `Fleur`, `Bouquet`, et `Accessoire`. Voici un aperçu de chaque table :
1. **Fidélité** : Contient les informations de fidélité des clients et leurs réductions.
2. **Client** : Stocke les informations client et est relié à la table **Fidélité**.
3. **BonDeCommande** : Détient les détails des commandes, en lien avec la table **Client**.
4. **Commande** : Contient les informations générales sur les commandes, reliée à **BonDeCommande**.
5. **Magasin** : Représente chaque boutique avec des informations telles que l’ID et le chiffre d’affaires.
6. **Produit** : Stocke les informations sur les produits en commande.
7. **Accessoire** : Enregistre les accessoires disponibles en magasin, en lien avec **BonDeCommande**.
8. **Bouquet** : Permet de stocker les informations des bouquets, associée à **Produit**.
9. **Fleur** : Liste les types de fleurs disponibles en stock, en lien avec **Magasin**.

Les tables sont liées par des clés étrangères, et nous avons utilisé des `JOIN` pour faciliter les requêtes entre les tables.

### Application Visual Studio

L'interface utilisateur a été conçue avec Visual Studio en C#. Elle comporte deux sections principales :
1. **Passer une Commande** : Permet de gérer les commandes, soit en créant un nouveau client, soit en identifiant un client existant.
2. **Consulter le Flux** : Permet de vérifier les statistiques de vente, les stocks en magasin, les informations des clients, et l'état des commandes.

Exemple de fonction pour afficher le magasin ayant réalisé le meilleur chiffre d'affaires :
```csharp
public void AfficherMeilleurCA()
{
    // Connexion à la base de données
    using (var connection = new MySqlConnection(connectionString))
    {
        connection.Open();
        MySqlCommand cmd = new MySqlCommand("SELECT ID, CA FROM Magasin ORDER BY CA DESC LIMIT 1", connection);
        MySqlDataReader reader = cmd.ExecuteReader();
        
        while (reader.Read())
        {
            Console.WriteLine("Magasin : {0}, Chiffre d'affaires : {1}", reader.GetString(0), reader.GetString(1));
        }
    }
}
