CREATE PROC EE_SP_IncAluno
    @NOME          VARCHAR (255)
AS

BEGIN
	INSERT INTO ALUNO(NOMEALUNO) 
	VALUES (@NOME)
	SELECT SCOPE_IDENTITY()
END
