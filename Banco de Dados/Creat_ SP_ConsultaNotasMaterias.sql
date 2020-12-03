CREATE PROC EE_SP_ConsNotasMaterias
	@ALUNOID INT
AS
BEGIN
	IF(ISNULL(@ALUNOID,0) = 0)
		SELECT Materia_Aluno.Nota, Aluno.*, Materia.*
		FROM Materia_Aluno 
		JOIN Aluno ON Materia_Aluno.AlunoID = Aluno.AlunoID
		JOIN Materia ON Materia_Aluno.MateriaID = Materia.MateriaID
		ORDER BY Aluno.AlunoID , Materia.MateriaID 

	ELSE
		SELECT Materia_Aluno.Nota, Aluno.*, Materia.*
		FROM Materia_Aluno 
		JOIN Aluno ON Materia_Aluno.AlunoID = Aluno.AlunoID
		JOIN Materia ON Materia_Aluno.MateriaID = Materia.MateriaID
		WHERE Aluno.AlunoID = @ALUNOID
		ORDER BY Aluno.AlunoID , Materia.MateriaID 

END