CREATE DATABASE dokotera;

\c dokotera

CREATE TABLE IF NOT EXISTS Patient(
    id SERIAL PRIMARY KEY,
    nom VARCHAR (30),
    age INT
);

CREATE TABLE IF NOT EXISTS Parametre(
    id SERIAL PRIMARY KEY,
    nom VARCHAR (50)
);

CREATE TABLE IF NOT EXISTS Maladie(
    id SERIAL PRIMARY KEY,
    nom VARCHAR (50)
);

CREATE TABLE IF NOT EXISTS TrancheAge(
    id SERIAL PRIMARY KEY,
    description VARCHAR (50),
    min INT,
    max INT
);

CREATE TABLE IF NOT EXISTS Medicament(
    id SERIAL PRIMARY KEY,
    nom VARCHAR (50),
    ageMin INT,
    ageMax INT,
    prix DOUBLE PRECISION
);

CREATE TABLE IF NOT EXISTS MedicamentParametre(
    id SERIAL PRIMARY KEY,
    idMedicament INT,
    idParametre INT,
    apport DOUBLE PRECISION,
    FOREIGN KEY (idMedicament) REFERENCES Medicament(id),
    FOREIGN KEY (idParametre) REFERENCES Parametre(id)
);

CREATE TABLE IF NOT EXISTS ContreIndication(
    id SERIAL PRIMARY KEY,
    idMedicament INT,
    idParametre INT,
    apportNegatif DOUBLE PRECISION,
    FOREIGN KEY (idMedicament) REFERENCES Medicament(id),
    FOREIGN KEY (idParametre) REFERENCES Parametre(id)
);

CREATE TABLE IF NOT EXISTS MaladieParametre(
    id SERIAL PRIMARY KEY,
    idMaladie INT,
    idParametre INT,
    niveauMin DOUBLE PRECISION,
    niveauMax DOUBLE PRECISION,
    idTrancheAge INT,
    FOREIGN KEY (idMaladie) REFERENCES Maladie(id),
    FOREIGN KEY (idParametre) REFERENCES Parametre(id),
    FOREIGN KEY (idTrancheAge) REFERENCES TrancheAge(id)
);



CREATE TABLE IF NOT EXISTS PatientParametre (
    id SERIAL PRIMARY KEY,
    idPatient INT,
    idParametre INT,
    niveau DOUBLE PRECISION,
    FOREIGN KEY (idPatient) REFERENCES Patient(id),
    FOREIGN KEY (idParametre) REFERENCES Parametre(id)
);


CREATE OR REPLACE VIEW view_MaladieParametreWithNom AS
SELECT 
    MaladieParametre.id AS id,
    MaladieParametre.idMaladie AS idMaladie,
    MaladieParametre.idParametre AS idParametre,
    MaladieParametre.niveauMin AS niveauMin,
    MaladieParametre.niveauMax AS niveauMax,
    MaladieParametre.idTrancheAge AS idTrancheAge,
    Maladie.nom AS nomMaladie
From MaladieParametre 
JOIN Maladie on MaladieParametre.idMaladie = Maladie.id;


CREATE OR REPLACE VIEW view_MedicamentParametreWithTrancheAge AS
SELECT 
    MedicamentParametre.id AS id,
    MedicamentParametre.idMedicament AS idMedicament,
    MedicamentParametre.idParametre AS idParametre,
    MedicamentParametre.apport AS apport,
    Parametre.nom AS nomParametre ,
    Medicament.ageMin AS ageMin,
    Medicament.ageMax AS ageMax
From  MedicamentParametre
JOIN Medicament on MedicamentParametre.idMedicament = Medicament.id
JOIN Parametre on MedicamentParametre.idParametre = Parametre.id;




-------------------DONNE AKO ------------------
INSERT INTO Parametre VALUES (default , 'loha');
INSERT INTO Parametre VALUES (default , 'kibo');
INSERT INTO Parametre VALUES (default , 'lelo');
INSERT INTO Parametre VALUES (default , 'caca');



INSERT INTO Maladie VALUES (default , 'vavony');
INSERT INTO Maladie VALUES (default , 'grippe');



INSERT INTO TrancheAge VALUES (default , 'bebe' , 1 , 5);
INSERT INTO TrancheAge VALUES (default , 'enfant' , 6 , 12);
INSERT INTO TrancheAge VALUES (default , 'jeune' , 13 , 20);
INSERT INTO TrancheAge VALUES (default , 'adult' , 21 , 40);
INSERT INTO TrancheAge VALUES (default , 'vieux' , 41 , 200);



INSERT INTO Medicament VALUES (default , 'Fervex' , 18  ,200, 1000);
INSERT INTO Medicament VALUES (default , 'Dolliprane' , 1  ,200, 2500);
INSERT INTO Medicament VALUES (default , 'paracetamol' , 1  ,200, 1100);
INSERT INTO Medicament VALUES (default , 'vitamine C' , 1  ,200, 2100);
INSERT INTO Medicament VALUES (default , 'med6' , 1  ,200, 2000);



INSERT INTO MedicamentParametre VALUES (default , 1 , 1 , 3);

INSERT INTO MedicamentParametre VALUES (default , 2 , 1 , 6);
INSERT INTO MedicamentParametre VALUES (default , 2 , 4 , 2);

INSERT INTO MedicamentParametre VALUES (default , 3 , 4 , 3);

INSERT INTO MedicamentParametre VALUES (default , 4 , 3 , 5);

INSERT INTO MedicamentParametre VALUES (default , 5 , 2 , 5);
INSERT INTO MedicamentParametre VALUES (default , 5 , 1, 5);





INSERT INTO ContreIndication VALUES (default , 1 , 4 , 2);
INSERT INTO ContreIndication VALUES (default , 2 , 2 , 3);



INSERT INTO MaladieParametre VALUES (default , 1 , 2 , 3 , 8 , 2);
INSERT INTO MaladieParametre VALUES (default , 1 , 4 , -9 , -2 , 2);
INSERT INTO MaladieParametre VALUES (default , 2 , 1 , 3 , 9 , 2);
INSERT INTO MaladieParametre VALUES (default , 2 , 3 , 2 , 10 , 2);

INSERT INTO MaladieParametre VALUES (default , 2 , 2 , 2 , 10 , 3);
