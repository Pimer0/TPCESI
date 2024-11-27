-- Table: Recette
CREATE TABLE Recette (
                         Id SERIAL PRIMARY KEY,
                         Nom VARCHAR(50) NOT NULL,
                         TempsPrep FLOAT NOT NULL,
                         TempsCuisson FLOAT NOT NULL,
                         Difficulte INT NOT NULL
);

-- Table: Ingredient
CREATE TABLE Ingredient (
                            Id SERIAL PRIMARY KEY,
                            Nom VARCHAR(500) NOT NULL
);

-- Table: Commentaire
CREATE TABLE Commentaire (
                             Id SERIAL PRIMARY KEY,
                             Description TEXT NOT NULL
);

-- Table: Etapes
CREATE TABLE Etapes (
                        Id SERIAL PRIMARY KEY,
                        Nom VARCHAR(500) NOT NULL
);

-- Table: Categorie
CREATE TABLE Categorie (
                           Id SERIAL PRIMARY KEY,
                           Nom VARCHAR(500) NOT NULL
);

-- Table: Avoir
CREATE TABLE Avoir (
                       Id INT NOT NULL,
                       Id_Recette INT NOT NULL,
                       PRIMARY KEY (Id, Id_Recette),
                       FOREIGN KEY (Id) REFERENCES Ingredient(Id),
                       FOREIGN KEY (Id_Recette) REFERENCES Recette(Id)
);

-- Table: Posseder
CREATE TABLE Posseder (
                          Id INT NOT NULL,
                          Id_Recette INT NOT NULL,
                          PRIMARY KEY (Id, Id_Recette),
                          FOREIGN KEY (Id) REFERENCES Commentaire(Id),
                          FOREIGN KEY (Id_Recette) REFERENCES Recette(Id)
);

-- Table: Composer
CREATE TABLE Composer (
                          Id INT NOT NULL,
                          Id_Recette INT NOT NULL,
                          PRIMARY KEY (Id, Id_Recette),
                          FOREIGN KEY (Id) REFERENCES Etapes(Id),
                          FOREIGN KEY (Id_Recette) REFERENCES Recette(Id)
);

-- Table: Appartenir
CREATE TABLE Appartenir (
                            Id INT NOT NULL,
                            Id_Categorie INT NOT NULL,
                            PRIMARY KEY (Id, Id_Categorie),
                            FOREIGN KEY (Id) REFERENCES Recette(Id),
                            FOREIGN KEY (Id_Categorie) REFERENCES Categorie(Id)
);