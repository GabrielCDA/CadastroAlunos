CREATE PROC EE_SP_IncNotasAluno
    @IDMATERIA     INT,
    @IDALUNO       INT,
    @NOTA          DECIMAL (18,2)
AS
BEGIN
	INSERT INTO MATERIA_ALUNO VALUES (@IDMATERIA, @IDALUNO, @NOTA)

END
