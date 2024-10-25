#SELECT COUNT ETC

# Magasin au meilleur CA
SELECT Id_magasin, CA
FROM magasin
ORDER BY CA DESC
Limit 1;

# Bouquet le plus cher
SELECT
	distinct produit.Id_bon_commande, 
    produit.Prix_produit
FROM produit
WHERE  produit.Id_bon_commande NOT LIKE "A_%"
ORDER BY Prix_produit DESC
LIMIT 1;

# MOYENNE PRIX BOUQUET
SELECT AVG(Prix_produit) AS prix_moyen 
FROM produit
WHERE  produit.Id_bon_commande NOT LIKE "A_%";

# CLIENT QUI A FAIT LE PLUS DE COMMANDES
SELECT nomClient, COUNT(*) AS total_commande
FROM bon_commande
GROUP BY nomClient
ORDER BY total_commande DESC
LIMIT 1;

#MEILLEUR COMMANDE 
SELECT 
	Id_client as client, 
    commande.Id_bon_commande, 
    prix_final
FROM commande
ORDER BY prix_final DESC
LIMIT 1;



# TOTAL COMMANDE PASSES D'UN CLIENT
SELECT 
    distinct Id_bon_commande AS commande,
    dateCommande
FROM commande, client
WHERE Id_client = 'sevane@gmail.com';

#REQUETES DE JOINTURES
# PRIX D'UNE COMMANDE (DOUBLE JOIN)
SELECT 
	distinct client.mail,
	(commande.Id_bon_commande),
    produit.prix_produit
FROM commande
JOIN client ON commande.Id_client = client.mail
JOIN produit ON commande.Id_bon_commande = produit.Id_bon_commande;

# PRIX AVEC REDUCTION DE TOUTES LES COMMANDES (TRIPLE JOIN)
SELECT 
	 distinct mail,
     reduction,
     bon_commande.Id_BonCommande,
     produit.Prix_produit,
     produit.Prix_produit*(1- reduction/100) AS prix_final
FROM client
JOIN fidélité ON fidélité.grade = client.f_grade 
JOIN bon_commande ON bon_commande.nomClient = client.mail
JOIN produit ON  produit.Id_bon_commande = bon_commande.Id_BonCommande;

#II) RESSORTIR LE MEILLEUR CLIENT SUR L'ANNEE 2022
SELECT 
	commande.Id_client AS BEST_CUSTOMER,
    SUM(prix_final) AS Total_achat
FROM commande
WHERE YEAR(commande.dateCommande) = 2022
GROUP BY BEST_CUSTOMER
ORDER BY Total_achat DESC
LIMIT 1;


#4 magasin et stock
SELECT 
	distinct Id_magasin, 
    Id_fleur as produit, 
    Quantite_en_stock
FROM magasin
LEFT JOIN fleur 
ON magasin.Id_magasin = fleur.identifiant_m
where Quantite_en_stock < 5
UNION ALL
SELECT 
	distinct Id_magasin, 
	Id_Access as produit, 
    Quantite_stock
FROM magasin
LEFT JOIN accessoire 
ON magasin.Id_magasin = accessoire.IdMag
WHERE Quantite_stock < 5
ORDER BY Quantite_en_stock;

#6 Valeur en stock par magasin
SELECT distinct Id_Access	, Quantite_stock
FROM accessoire 
where IdMag ='La Defense';

#caract d'un client
SELECT 	
	distinct nom, 
	prenom, 
    f_grade as fidelite,
    commande.Id_bon_commande,
    commande.dateCommande
FROM client
JOIN commande 
ON client.mail = commande.Id_client
where mail = 'sevane@gmail.com';

#commande a une certaine date
SELECT etat, commande.Id_bon_commande FROM commande where dateCommande ='2022-10-12';


SELECT 
	commande.Id_client AS BEST_CUSTOMER,
    SUM(prix_final) AS Total_achat
FROM commande
WHERE MONTH(commande.dateCommande) = 8
GROUP BY BEST_CUSTOMER
ORDER BY Total_achat DESC
LIMIT 1;


