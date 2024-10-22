CREATE DATABASE RuleEvaluatorTest ON (
 NAME='RuleEvaluatorTest', 
 FILENAME='c:\Users\pbalas\Source\Repos\RuleEvaluator\RuleEvaluator.Repository.Database.Test\RuleEvaluatorTest.mdf')
 COLLATE Czech_CI_AS
GO


USE RuleEvaluatorTest
GO
/****** Object:  Schema [Ciselnik]    Script Date: 26.1.2017 19:17:26 ******/
CREATE SCHEMA [Ciselnik]
GO
/****** Object:  StoredProcedure [Ciselnik].[p_GetSchemaColBySchemaKod]    Script Date: 26.1.2017 19:17:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
----------------------------------------------------------------------------------
-- Modified:	2016-12-05 - D3GROUP\mbenek (2016-12-05 - D3GROUP\mbenek)
-- Changeset:	#294135 (#294124)
-- Description:	-
----------------------------------------------------------------------------------
CREATE PROCEDURE [Ciselnik].[p_GetSchemaColBySchemaKod]
    @Key NVARCHAR(250)
AS
BEGIN

	SET NOCOUNT ON
	
 	SELECT 0 [Index], 'PrimaryKey' [InputOutput], -1 [Poradi]
 	FROM [Ciselnik].[TranslatorSchema] AS ts 
 	JOIN [Ciselnik].[TranslatorSchemaCol] AS tsc ON tsc.IdTranslatorSchema = ts.IdTranslatorSchema 
 	WHERE ts.Kod = @Key 	

	UNION

	SELECT tsc.ColKod AS [Index], CASE tsc.ColType WHEN 'In' THEN 'Input' WHEN 'Out' THEN 'Output' END AS [InputOutput], tsc.Poradi
	FROM Ciselnik.TranslatorSchema AS ts
	JOIN Ciselnik.TranslatorSchemaCol AS tsc ON tsc.IdTranslatorSchema = ts.IdTranslatorSchema
	WHERE Kod = @Key
	ORDER BY Poradi

END

--GRANT ALL ON [Ciselnik].[p_GetSchemaColBySchemaKod] TO PUBLIC
GO
/****** Object:  StoredProcedure [Ciselnik].[p_GetTranslatorDataBySchemaKod]    Script Date: 26.1.2017 19:17:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------------------
-- Modified:	2016-12-05 - D3GROUP\mbenek (2016-12-05 - D3GROUP\mbenek)
-- Changeset:	#294135 (#294124)
-- Description:	-
----------------------------------------------------------------------------------
CREATE PROCEDURE [Ciselnik].[p_GetTranslatorDataBySchemaKod]
    @Key NVARCHAR(250)
AS
BEGIN

	SET NOCOUNT ON
	
	SELECT td.IdTranslatorData, td.Col01, td.Col02, td.Col03, td.Col04, td.Col05, td.Col06, td.Col07, td.Col08, td.Col09, td.Col10, td.Col11, td.Col12, td.Col13, td.Col14, td.Col15, td.Col16, td.Col17, td.Col18, td.Col19, td.Col20
	FROM Ciselnik.TranslatorData AS td
	JOIN Ciselnik.Translator AS t ON t.IdTranslator = td.IdTranslator
	JOIN Ciselnik.TranslatorSchema AS ts ON ts.IdTranslator = t.IdTranslator
	WHERE ts.Kod = @Key
	ORDER BY Poradi

END

--GRANT ALL ON [Ciselnik].[p_GetTranslatorDataBySchemaKod] TO PUBLIC
GO
/****** Object:  Table [Ciselnik].[Translator]    Script Date: 26.1.2017 19:17:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ciselnik].[Translator](
	[IdTranslator] [int] IDENTITY(1,1) NOT NULL,
	[Kod] [varchar](50) NOT NULL,
	[Nazev] [varchar](250) NOT NULL,
 CONSTRAINT [PK_Translator] PRIMARY KEY CLUSTERED 
(
	[IdTranslator] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ciselnik].[TranslatorData]    Script Date: 26.1.2017 19:17:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ciselnik].[TranslatorData](
	[IdTranslatorData] [int] IDENTITY(1,1) NOT NULL,
	[IdTranslator] [int] NOT NULL,
	[Col01] [varchar](max) NULL,
	[Col02] [varchar](max) NULL,
	[Col03] [varchar](max) NULL,
	[Col04] [varchar](max) NULL,
	[Col05] [varchar](max) NULL,
	[Col06] [varchar](max) NULL,
	[Col07] [varchar](max) NULL,
	[Col08] [varchar](max) NULL,
	[Col09] [varchar](max) NULL,
	[Col10] [varchar](max) NULL,
	[Col11] [varchar](255) NULL,
	[Col12] [varchar](255) NULL,
	[Col13] [varchar](255) NULL,
	[Col14] [varchar](255) NULL,
	[Col15] [varchar](255) NULL,
	[Col16] [varchar](255) NULL,
	[Col17] [varchar](255) NULL,
	[Col18] [varchar](255) NULL,
	[Col19] [varchar](255) NULL,
	[Col20] [varchar](255) NULL,
	[Poradi] [int] NULL,
 CONSTRAINT [PK_TranslatorData] PRIMARY KEY CLUSTERED 
(
	[IdTranslatorData] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ciselnik].[TranslatorSchema]    Script Date: 26.1.2017 19:17:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ciselnik].[TranslatorSchema](
	[IdTranslatorSchema] [int] IDENTITY(1,1) NOT NULL,
	[IdTranslatorTyp] [int] NOT NULL,
	[IdTranslator] [int] NOT NULL,
	[Kod] [varchar](50) NOT NULL,
	[Nazev] [varchar](250) NOT NULL,
	[Popis] [varchar](250) NULL,
	[NazevRole] [varchar](250) NULL,
	[Comment] [varchar](250) NULL,
 CONSTRAINT [PK_TranslatorSchema] PRIMARY KEY CLUSTERED 
(
	[IdTranslatorSchema] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ciselnik].[TranslatorSchemaCol]    Script Date: 26.1.2017 19:17:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ciselnik].[TranslatorSchemaCol](
	[IdTranslatorSchemaCol] [int] IDENTITY(1,1) NOT NULL,
	[IdTranslatorSchema] [int] NOT NULL,
	[ColKod] [int] NOT NULL,
	[ColNazev] [varchar](250) NOT NULL,
	[ColJednotka] [varchar](250) NULL,
	[ColType] [varchar](50) NULL,
	[ColAttribute] [varchar](250) NULL,
	[Poradi] [int] NULL,
 CONSTRAINT [PK_TranslatorSchemaCol] PRIMARY KEY CLUSTERED 
(
	[IdTranslatorSchemaCol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [Ciselnik].[TranslatorTyp]    Script Date: 26.1.2017 19:17:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Ciselnik].[TranslatorTyp](
	[IdTranslatorTyp] [int] IDENTITY(1,1) NOT NULL,
	[Kod] [varchar](50) NOT NULL,
	[Nazev] [varchar](250) NOT NULL,
 CONSTRAINT [PK_TranslatorTyp] PRIMARY KEY CLUSTERED 
(
	[IdTranslatorTyp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [Ciselnik].[Translator] ON 

INSERT [Ciselnik].[Translator] ([IdTranslator], [Kod], [Nazev]) VALUES (1, N'OdhadBodu', N'Předpis pro odhad bodů')
INSERT [Ciselnik].[Translator] ([IdTranslator], [Kod], [Nazev]) VALUES (2, N'OdhadBoduViditelnostPolozek', N'Viditelnost datových položek pro odhad bodů')
SET IDENTITY_INSERT [Ciselnik].[Translator] OFF
SET IDENTITY_INSERT [Ciselnik].[TranslatorData] ON 

INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (88, 1, N'.*', N'.*', N'.*', N'7BN Perspektiva Důchod', NULL, NULL, NULL, NULL, NULL, NULL, N'1[0-4]', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'C2/400*0.7', 10000100)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (89, 1, N'.*', N'.*', N'.*', N'7BN Perspektiva Důchod', NULL, NULL, NULL, NULL, NULL, NULL, N'1[5-9]|2[0-4]', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'C2/240*0.7', 10000200)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (90, 1, N'.*', N'.*', N'.*', N'7BN Perspektiva Důchod', NULL, NULL, NULL, NULL, NULL, NULL, N'2[5-9]|[3-9][0-9]', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'C2/180*0.7', 10000300)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (91, 1, N'.*', N'.*', N'.*', N'(ZFP Život [+] Unisex.*|ZFP Život[+])', NULL, NULL, NULL, NULL, NULL, NULL, N'1[0-4]', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'C2/470*0.7', 10000400)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (92, 1, N'.*', N'.*', N'.*', N'(ZFP Život [+] Unisex.*|ZFP Život[+])', NULL, NULL, NULL, NULL, NULL, NULL, N'1[5-9]|2[0-4]', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'C2/280*0.7', 10000500)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (93, 1, N'.*', N'.*', N'.*', N'(ZFP Život [+] Unisex.*|ZFP Život[+])', NULL, NULL, NULL, NULL, NULL, NULL, N'2[5-9]|[3-9][0-9]', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'C2/200*0.7', 10000600)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (94, 1, N'.*', N'.*', N'.*', N'ZFP Život[+] UNISEX', NULL, NULL, NULL, NULL, NULL, NULL, N'1[0-4]', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'C1/500*0.7', 10000700)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (95, 1, N'.*', N'.*', N'.*', N'ZFP Život[+] UNISEX', NULL, NULL, NULL, NULL, NULL, NULL, N'1[5-9]|2[0-4]', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'C1/280*0.7', 10000800)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (96, 1, N'.*', N'.*', N'.*', N'ZFP Život[+] UNISEX', NULL, NULL, NULL, NULL, NULL, NULL, N'2[5-9]|[3-9][0-9]', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'C1/180*0.7', 10000900)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (97, 1, N'.*', N'.*', N'.*', N'ZFP realitní fond', NULL, NULL, NULL, NULL, NULL, NULL, N'.*', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'C1*0.029126/324', 10001000)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (98, 1, N'.*', N'.*', N'.*', N'ZFP Invest II', NULL, NULL, NULL, NULL, NULL, NULL, N'.*', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'C4/330', 10001100)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (99, 1, N'.*', N'.*', N'.*', N'.*', NULL, NULL, NULL, NULL, NULL, NULL, N'.*', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'0', 10009900)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (100, 2, N'.*', N'.*', N'.*', N'(ZFP Život [+] Unisex.*|ZFP Život[+]|7BN Perspektiva Důchod)', N'', N'Roční pojistné', N'', N'', N'Pojistná doba', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 10000100)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (101, 2, N'.*', N'.*', N'.*', N'ZFP Život[+] UNISEX', N'Pojistná částka', N'', N'', N'', N'Pojistná doba', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 10000100)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (102, 2, N'.*', N'.*', N'.*', N'ZFP realitní fond', N'Vklad', N'', N'', N'', N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 10000100)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (103, 2, N'.*', N'.*', N'.*', N'ZFP Invest II', N'', N'', N'', N'Vstupní poplatek', N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 10000100)
INSERT [Ciselnik].[TranslatorData] ([IdTranslatorData], [IdTranslator], [Col01], [Col02], [Col03], [Col04], [Col05], [Col06], [Col07], [Col08], [Col09], [Col10], [Col11], [Col12], [Col13], [Col14], [Col15], [Col16], [Col17], [Col18], [Col19], [Col20], [Poradi]) VALUES (104, 2, N'.*', N'.*', N'.*', N'.*', N'', N'', N'', N'', N'', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 10009900)
SET IDENTITY_INSERT [Ciselnik].[TranslatorData] OFF
SET IDENTITY_INSERT [Ciselnik].[TranslatorSchema] ON 

INSERT [Ciselnik].[TranslatorSchema] ([IdTranslatorSchema], [IdTranslatorTyp], [IdTranslator], [Kod], [Nazev], [Popis], [NazevRole], [Comment]) VALUES (1, 1, 1, N'OdhadBodu', N'Předpis pro odhad bodů', N'Získání předpisu pro odhad bodů', N'', N'')
INSERT [Ciselnik].[TranslatorSchema] ([IdTranslatorSchema], [IdTranslatorTyp], [IdTranslator], [Kod], [Nazev], [Popis], [NazevRole], [Comment]) VALUES (2, 1, 2, N'OdhadBoduViditelnostPolozek', N'Viditelnost datových položek pro odhad bodů', N'Získání kolekce položek pro evidenci podkladových dat pro odhad bodů', N'', N'')
SET IDENTITY_INSERT [Ciselnik].[TranslatorSchema] OFF
SET IDENTITY_INSERT [Ciselnik].[TranslatorSchemaCol] ON 

INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (16, 1, 1, N'Mateřská firma', N'', N'In', N'', 1)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (17, 1, 2, N'Partner', N'', N'In', N'', 2)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (18, 1, 3, N'Kategorie', N'', N'In', N'', 3)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (19, 1, 4, N'Produkt', N'', N'In', N'', 4)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (20, 1, 11, N'Pojistná doba', N'', N'In', N'', 5)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (21, 1, 20, N'Předpis', N'', N'Out', N'', 6)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (22, 2, 1, N'Mateřská firma', N'', N'In', N'', 1)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (23, 2, 2, N'Partner', N'', N'In', N'', 2)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (24, 2, 3, N'Kategorie', N'', N'In', N'', 3)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (25, 2, 4, N'Produkt', N'', N'In', N'', 4)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (26, 2, 5, N'C1', N'', N'Out', N'', 5)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (27, 2, 6, N'C2', N'', N'Out', N'', 6)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (28, 2, 7, N'C3', N'', N'Out', N'', 7)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (29, 2, 8, N'C4', N'', N'Out', N'', 8)
INSERT [Ciselnik].[TranslatorSchemaCol] ([IdTranslatorSchemaCol], [IdTranslatorSchema], [ColKod], [ColNazev], [ColJednotka], [ColType], [ColAttribute], [Poradi]) VALUES (30, 2, 9, N'C5', N'', N'Out', N'', 9)
SET IDENTITY_INSERT [Ciselnik].[TranslatorSchemaCol] OFF
SET IDENTITY_INSERT [Ciselnik].[TranslatorTyp] ON 

INSERT [Ciselnik].[TranslatorTyp] ([IdTranslatorTyp], [Kod], [Nazev]) VALUES (1, N'Zakladni', N'Základní')
INSERT [Ciselnik].[TranslatorTyp] ([IdTranslatorTyp], [Kod], [Nazev]) VALUES (2, N'Pasmovy', N'Pásmový')
SET IDENTITY_INSERT [Ciselnik].[TranslatorTyp] OFF
ALTER TABLE [Ciselnik].[TranslatorData]  WITH CHECK ADD  CONSTRAINT [fk_TranslatorData_Translator] FOREIGN KEY([IdTranslator])
REFERENCES [Ciselnik].[Translator] ([IdTranslator])
GO
ALTER TABLE [Ciselnik].[TranslatorData] CHECK CONSTRAINT [fk_TranslatorData_Translator]
GO
ALTER TABLE [Ciselnik].[TranslatorSchema]  WITH CHECK ADD  CONSTRAINT [fk_TranslatorSchema_Translator] FOREIGN KEY([IdTranslator])
REFERENCES [Ciselnik].[Translator] ([IdTranslator])
GO
ALTER TABLE [Ciselnik].[TranslatorSchema] CHECK CONSTRAINT [fk_TranslatorSchema_Translator]
GO
ALTER TABLE [Ciselnik].[TranslatorSchema]  WITH CHECK ADD  CONSTRAINT [fk_TranslatorSchema_TranslatorTyp] FOREIGN KEY([IdTranslatorTyp])
REFERENCES [Ciselnik].[TranslatorTyp] ([IdTranslatorTyp])
GO
ALTER TABLE [Ciselnik].[TranslatorSchema] CHECK CONSTRAINT [fk_TranslatorSchema_TranslatorTyp]
GO
ALTER TABLE [Ciselnik].[TranslatorSchemaCol]  WITH CHECK ADD  CONSTRAINT [fk_TranslatorSchemaCol_TranslatorSchema] FOREIGN KEY([IdTranslatorSchema])
REFERENCES [Ciselnik].[TranslatorSchema] ([IdTranslatorSchema])
GO
ALTER TABLE [Ciselnik].[TranslatorSchemaCol] CHECK CONSTRAINT [fk_TranslatorSchemaCol_TranslatorSchema]
GO
