DROP DATABASE IF EXISTS fleurs;
CREATE DATABASE IF NOT EXISTS fleurs;


DROP TABLE IF EXISTS Fidélité;
CREATE TABLE IF NOT EXISTS Fidélité
(
	grade VARCHAR(40),
	reduction INTEGER,
    PRIMARY KEY (grade, reduction)
    );

DROP TABLE IF EXISTS Client;
CREATE TABLE IF NOT EXISTS Client
(
	nom VARCHAR(40),
    prenom VARCHAR(40),
    tel INTEGER,
    mail VARCHAR(40),
    mdp VARCHAR(40),
    adresse_fac VARCHAR(40),
    cb INTEGER,
    f_grade VARCHAR(40),
	PRIMARY KEY(mail),
    FOREIGN KEY (f_grade) REFERENCES fidélité(grade)
    );

DROP TABLE IF EXISTS bon_commande;
CREATE TABLE IF NOT EXISTS Bon_Commande
(
	Id_BonCommande VARCHAR(40),
    nomClient VARCHAR(40),
	adresse_liv VARCHAR(40),
	dateDesiree date,
	message VARCHAR (100),
    PRIMARY KEY (Id_BonCommande, nomClient, dateDesiree),
    FOREIGN KEY (nomClient) REFERENCES Client(mail)
	);

DROP TABLE IF EXISTS Commande;
CREATE TABLE IF NOT EXISTS Commande
(
    Id_bon_commande VARCHAR(40),
	dateCommande date,
    etat VARCHAR(40),
	Id_client VARCHAR(40),
    PRIMARY KEY(Id_bon_commande, dateCommande, Id_client),
    FOREIGN KEY (Id_client) REFERENCES client(mail),
    FOREIGN KEY (Id_bon_commande) REFERENCES Bon_Commande(Id_BonCommande)
	);

DROP TABLE IF EXISTS Magasin;
CREATE TABLE IF NOT EXISTS Magasin
(
	Id_magasin VARCHAR(40) PRIMARY KEY,
    CA INTEGER
);

DROP TABLE IF EXISTS Produit;
CREATE TABLE IF NOT EXISTS Produit
(
	Id_bon_commande VARCHAR(40),
	Prix_produit DECIMAl(8,4),
    PRIMARY KEY (Id_bon_commande, Prix_produit),
    FOREIGN KEY (Id_bon_commande) REFERENCES bon_commande(Id_BonCommande)
	);

DROP TABLE IF EXISTS Accessoire;
CREATE TABLE IF NOT EXISTS Accessoire
(
	Id_Access VARCHAR(40),
    prix_access DECIMAL(8,4),
    Quantite_stock INTEGER,
    IdMag VARCHAR(40),
    PRIMARY KEY (Id_Access, IdMag),
    FOREIGN KEY (Id_Access) REFERENCES produit(Id_bon_Commande),
    FOREIGN KEY (IdMag) REFERENCES Magasin(Id_magasin)
);

DROP TABLE IF EXISTS Bouquet;
CREATE TABLE IF NOT EXISTS Bouquet
(
	Id_bouquet VARCHAR(40) PRIMARY KEY,
    prix_bouquet DECIMAL(8,4),
    FOREIGN KEY (Id_bouquet) REFERENCES Produit(Id_bon_commande)
    );

DROP TABLE IF EXISTS Fleur;
CREATE TABLE IF NOT EXISTS Fleur
(
	Id_fleur VARCHAR(40),
    prixF DECIMAL(8,4),
    Quantite_en_stock INTEGER,
    occasion VARCHAR(100),
    _type VARCHAR(40),
    Qt_vendue INTEGER,
    identifiant_m VARCHAR(40),
    PRIMARY KEY(Id_fleur, identifiant_m),
    FOREIGN KEY (identifiant_m) REFERENCES magasin(Id_magasin)
	);

