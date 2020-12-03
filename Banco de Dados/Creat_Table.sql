CREATE TABLE Materia (
    MateriaID int NOT NULL IDENTITY(1,1),
    NomeMateria varchar(255) NOT NULL,   
    PRIMARY KEY (MateriaID)
);


CREATE TABLE Aluno (
    AlunoID int NOT NULL IDENTITY(1,1),
    NomeAluno varchar(255) NOT NULL,
    PRIMARY KEY (AlunoID),
);

CREATE TABLE Materia_Aluno (
    MateriaID int NOT NULL,
	AlunoID int NOT NULL,
	Nota decimal NOT NULL,
	FOREIGN KEY (MateriaID) REFERENCES Materia(MateriaID),
    FOREIGN KEY (AlunoID) REFERENCES Aluno(AlunoID)
);

CREATE TABLE Autenticacao (
    Usuario varchar(MAX),
    Senha varchar(MAX) 
);

