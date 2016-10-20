IF OBJECT_ID('Ciselnik.p_GetSchemaColBySchemaKod') IS NOT NULL 
BEGIN
	DROP PROCEDURE Ciselnik.p_GetSchemaColBySchemaKod
END
GO
----------------------------------------------------------------------------------
-- Modified:	-
-- Changeset:	-
-- Description:	-
----------------------------------------------------------------------------------
CREATE PROCEDURE Ciselnik.p_GetSchemaColBySchemaKod
    @Key NVARCHAR(250)
AS
BEGIN

	SET NOCOUNT ON
	
	SELECT tsc.ColKod - 1 AS [Index], CASE tsc.ColType WHEN 'In' THEN 'Input' WHEN 'Out' THEN 'Output' END AS [InputOutput]
	FROM Ciselnik.TranslatorSchema AS ts
	JOIN Ciselnik.TranslatorSchemaCol AS tsc ON tsc.IdTranslatorSchema = ts.IdTranslatorSchema
	WHERE Kod = @Key
	ORDER BY PoradiParamMetod

END

GO