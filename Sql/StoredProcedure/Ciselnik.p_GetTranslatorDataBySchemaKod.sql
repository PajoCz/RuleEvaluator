IF OBJECT_ID('Ciselnik.p_GetTranslatorDataBySchemaKod') IS NOT NULL 
BEGIN
	DROP PROCEDURE Ciselnik.p_GetTranslatorDataBySchemaKod
END
GO
----------------------------------------------------------------------------------
-- Modified:	-
-- Changeset:	-
-- Description:	-
----------------------------------------------------------------------------------
CREATE PROCEDURE Ciselnik.p_GetTranslatorDataBySchemaKod
    @Key NVARCHAR(250)
AS
BEGIN

	SET NOCOUNT ON
	
	SELECT td.Col01, td.Col02, td.Col03, td.Col04, td.Col05, td.Col06, td.Col07, td.Col08, td.Col09, td.Col10, td.Col11, td.Col12, td.Col13, td.Col14, td.Col15, td.Col16, td.Col17, td.Col18, td.Col19, td.Col20
	FROM Ciselnik.TranslatorData AS td
	JOIN Ciselnik.Translator AS t ON t.IdTranslator = td.IdTranslator
	JOIN Ciselnik.TranslatorSchema AS ts ON ts.IdTranslator = t.IdTranslator
	WHERE ts.Kod = @Key
	ORDER BY Poradi

END

GO