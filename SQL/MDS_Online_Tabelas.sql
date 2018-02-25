IF NOT EXISTS (SELECT TOP 1 1 FROM sys.databases WHERE [name] = 'BDMdsOnline')
BEGIN
	CREATE DATABASE BDMdsOnline
END
GO
USE BDMdsOnline
GO

DROP TABLE dbo.SolicitacaoRoteiroTesteUnitarioHistorico
GO
DROP TABLE dbo.SolicitacaoRoteiroTesteUnitario
GO
DROP TABLE dbo.SolicitacaoRoteiroTesteFuncionalHistorico
GO
DROP TABLE dbo.SolicitacaoRoteiroTesteFuncional
GO
DROP TABLE dbo.StatusExecucaoHomologacao
GO
DROP TABLE dbo.StatusVerificacaoTesteUnitario
GO
DROP TABLE dbo.Imagem
GO
DROP TABLE dbo.TipoEvidencia
GO


CREATE TABLE dbo.TipoDocumento (
	 TipoDocumentoID	INT NOT NULL IDENTITY(1, 1)
	,Nome				VARCHAR(100)
)

CREATE TABLE dbo.DocumentoTemplate (
	 DocumentoTemplateID	INT NOT NULL IDENTITY(1, 1)
	,TipoDocumentoID		INT NOT NULL
	,Nome					VARCHAR(200) NOT NULL
	,Descricao				VARCHAR(1000) NULL
	,Template				VARCHAR(MAX) NOT NULL
)

IF OBJECT_ID('dbo.SolicitacaoPropostaComercial') IS NULL
CREATE TABLE dbo.SolicitacaoPropostaComercial (
	 SolicitacaoPropostaComercialID INT NOT NULL IDENTITY(1, 1)
	,DataAlteracaoStatus DATETIME
	,OGMin FLOAT
	,OGMax FLOAT 
	,DataEnvioOG DATETIME
	,Observacao VARCHAR(8000)
	,DataFimHomologacao DATETIME
	,DataGoLive DATETIME
	,Responsavel INT
	,MotivoImpedimento VARCHAR(8000)
	,CONSTRAINT PK_SolicitacaoPropostaComercial PRIMARY KEY (SolicitacaoPropostaComercialID)
)

IF OBJECT_ID('dbo.SolicitacaoHistoricoPropostaComercial') IS NULL
CREATE TABLE dbo.SolicitacaoHistoricoPropostaComercial (
	 SolicitacaoHistoricoPropostaComercialID INT NOT NULL IDENTITY(1, 1)
	,DataAlteracaoStatus DATETIME
	,OGMin FLOAT
	,OGMax FLOAT 
	,DataEnvioOG DATETIME
	,Observacao VARCHAR(8000)
	,DataFimHomologacao DATETIME
	,DataGoLive DATETIME
	,Responsavel INT
	,MotivoImpedimento VARCHAR(8000)
	,CONSTRAINT PK_SolicitacaoHistoricoPropostaComercial PRIMARY KEY (SolicitacaoHistoricoPropostaComercialID)
)

IF OBJECT_ID('dbo.SolictiacaoEstimativa') IS NULL
CREATE TABLE dbo.SolictiacaoEstimativa (
	 SolictiacaoEstimativaID INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID INT
	,TagEstimativa INT
	,RN VARCHAR(100)
	,Atividade VARCHAR(MAX)
	,Analise FLOAT
	,Processo FLOAT
	,Testes FLOAT
	,Homologacao FLOAT
	,Gestao FLOAT
	,Total FLOAT
	,TotalDesenvolvimento FLOAT
	,CONSTRAINT PK_SolictiacaoEstimativa PRIMARY KEY (SolictiacaoEstimativaID)
)


IF OBJECT_ID('dbo.Imagem') IS NULL
CREATE TABLE dbo.Imagem (
	 ImagemID		INT NOT NULL IDENTITY(1, 1)
	,[Guid]			UNIQUEIDENTIFIER NOT NULL	
	,Nome			VARCHAR(1000)
	,Extensao		VARCHAR(100)
	,ContentType	VARCHAR(100)
	,TamanhoKB		FLOAT
	,Arquivo		VARBINARY(MAX)
	,CONSTRAINT PK_Imagem PRIMARY KEY (ImagemID)
)
GO
CREATE INDEX IDX_Imagem_Guid ON dbo.Imagem ([Guid])
GO

--=== RTU ============================================================

IF OBJECT_ID('dbo.StatusVerificacaoTesteUnitario') IS NULL
CREATE TABLE dbo.StatusVerificacaoTesteUnitario (
	 StatusVerificacaoTesteUnitarioID INT NOT NULL IDENTITY(1, 1)
	,Nome VARCHAR(100) NOT NULL
	,CONSTRAINT PK_StatusVerificacaoTesteUnitario PRIMARY KEY (StatusVerificacaoTesteUnitarioID)
)
GO
SET IDENTITY_INSERT dbo.StatusVerificacaoTesteUnitario ON
	INSERT INTO dbo.StatusVerificacaoTesteUnitario (StatusVerificacaoTesteUnitarioID, Nome)
	VALUES (1, 'Não Testado'), (2, 'Sim, OK'), (3, 'Sim, NOK')
SET IDENTITY_INSERT dbo.StatusVerificacaoTesteUnitario OFF
GO

IF OBJECT_ID('dbo.SolicitacaoRoteiroTesteUnitario') IS NULL
CREATE TABLE dbo.SolicitacaoRoteiroTesteUnitario (
	 SolicitacaoRoteiroTesteUnitarioID	INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID						INT NOT NULL
	,Sequencia							VARCHAR(MAX) NULL
	,Condicao							VARCHAR(MAX) NULL
	,DadosEntrada						VARCHAR(MAX) NULL
	,ResultadoEsperado					VARCHAR(MAX) NULL
	,StatusVerificacaoTesteUnitarioID 	INT	NOT NULL
	,ComoTestar							VARCHAR(MAX) NULL
	,Observacoes						VARCHAR(MAX) NULL
	,Ordem								INT NOT NULL
	,CONSTRAINT PK_SolicitacaoRoteiroTesteUnitario PRIMARY KEY (SolicitacaoRoteiroTesteUnitarioID)
	,CONSTRAINT FK_SolicitacaoRoteiroTesteUnitario_StatusVerificacaoTesteUnitario
		FOREIGN KEY (StatusVerificacaoTesteUnitarioID)
		REFERENCES dbo.StatusVerificacaoTesteUnitario (StatusVerificacaoTesteUnitarioID)
)
GO
CREATE INDEX IDX_SolicitacaoRoteiroTesteUnitario_StatusVerificacaoTesteUnitario ON dbo.SolicitacaoRoteiroTesteUnitario (StatusVerificacaoTesteUnitarioID)
GO

IF OBJECT_ID('dbo.SolicitacaoRoteiroTesteUnitarioHistorico') IS NULL
CREATE TABLE dbo.SolicitacaoRoteiroTesteUnitarioHistorico (
	 SolicitacaoRoteiroTesteUnitarioHistoricoID	INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoRoteiroTesteUnitarioID	INT NOT NULL
	,SolicitacaoID						INT NOT NULL
	,Sequencia							VARCHAR(MAX) NULL
	,Condicao							VARCHAR(MAX) NULL
	,DadosEntrada						VARCHAR(MAX) NULL
	,ResultadoEsperado					VARCHAR(MAX) NULL
	,StatusVerificacaoTesteUnitarioID	INT	NOT NULL
	,ComoTestar							VARCHAR(MAX) NULL
	,Observacoes						VARCHAR(MAX) NULL
	,Ordem								INT NOT NULL
	,CONSTRAINT PK_SolicitacaoRoteiroTesteUnitarioHistorico PRIMARY KEY (SolicitacaoRoteiroTesteUnitarioHistoricoID)
	,CONSTRAINT FK_SolicitacaoRoteiroTesteUnitarioHistorico_SolicitacaoRoteiroTesteUnitario 
		FOREIGN KEY (SolicitacaoRoteiroTesteUnitarioID) 
		REFERENCES dbo.SolicitacaoRoteiroTesteUnitario (SolicitacaoRoteiroTesteUnitarioID)
)
GO
CREATE INDEX IDX_SolicitacaoRoteiroTesteUnitarioHistorico_SolicitacaoRoteiroTesteUnitario ON dbo.SolicitacaoRoteiroTesteUnitarioHistorico (SolicitacaoRoteiroTesteUnitarioID)
GO


--=== RTF ============================================================

IF OBJECT_ID('dbo.TipoEvidencia') IS NULL
CREATE TABLE dbo.TipoEvidencia (
	 TipoEvidenciaID INT NOT NULL IDENTITY(1, 1)
	,Nome VARCHAR(100) NOT NULL
	,CONSTRAINT PK_TipoEvidencia PRIMARY KEY (TipoEvidenciaID)
)
GO
SET IDENTITY_INSERT dbo.TipoEvidencia ON
	INSERT INTO dbo.TipoEvidencia (TipoEvidenciaID, Nome)
	VALUES (1, 'Sucesso'), (2, 'Erro')
SET IDENTITY_INSERT dbo.TipoEvidencia OFF
GO

IF OBJECT_ID('dbo.StatusExecucaoHomologacao') IS NULL
CREATE TABLE dbo.StatusExecucaoHomologacao (
	 StatusExecucaoHomologacaoID INT NOT NULL IDENTITY(1, 1)
	,Nome VARCHAR(100) NOT NULL
	,CONSTRAINT PK_StatusExecucaoHomologacao PRIMARY KEY (StatusExecucaoHomologacaoID)
)
GO
SET IDENTITY_INSERT dbo.StatusExecucaoHomologacao ON
	INSERT INTO dbo.StatusExecucaoHomologacao (StatusExecucaoHomologacaoID, Nome)
	VALUES (1, 'Não Testado'), (2, 'Sim, OK'), (3, 'Sim, NOK')
SET IDENTITY_INSERT dbo.StatusExecucaoHomologacao OFF
GO

IF OBJECT_ID('dbo.SolicitacaoRoteiroTesteFuncional') IS NULL
CREATE TABLE dbo.SolicitacaoRoteiroTesteFuncional (
	 SolicitacaoRoteiroTesteFuncionalID INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID						INT NOT NULL
	,Sequencia							VARCHAR(MAX) NULL
	,Funcionalidade						VARCHAR(MAX) NULL
	,CondicaoCenario					VARCHAR(MAX) NULL
	,PreCondicao						VARCHAR(MAX) NULL
	,DadosEntrada						VARCHAR(MAX) NULL
	,ResultadoEsperado					VARCHAR(MAX) NULL
	,Observacoes						VARCHAR(MAX) NULL
	,StatusExecucaoHomologacaoID		INT	NOT NULL
	,Ordem								INT NOT NULL
	,CONSTRAINT PK_SolicitacaoRoteiroTesteFuncional PRIMARY KEY (SolicitacaoRoteiroTesteFuncionalID)
	,CONSTRAINT FK_SolicitacaoRoteiroTesteFuncional_StatusExecucaoHomologacao 
		FOREIGN KEY (StatusExecucaoHomologacaoID)
		REFERENCES dbo.StatusExecucaoHomologacao (StatusExecucaoHomologacaoID)
)
GO
CREATE INDEX IDX_SolicitacaoRoteiroTesteFuncional_StatusExecucaoHomologacao ON dbo.SolicitacaoRoteiroTesteFuncional (StatusExecucaoHomologacaoID)
GO

IF OBJECT_ID('dbo.SolicitacaoRoteiroTesteFuncionalHistorico') IS NULL
CREATE TABLE dbo.SolicitacaoRoteiroTesteFuncionalHistorico (
	 SolicitacaoRoteiroTesteFuncionalHistoricoID INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoRoteiroTesteFuncionalID INT NOT NULL
	,SolicitacaoID						INT NOT NULL
	,Sequencia							VARCHAR(MAX) NULL
	,Funcionalidade						VARCHAR(MAX) NULL
	,CondicaoCenario					VARCHAR(MAX) NULL
	,PreCondicao						VARCHAR(MAX) NULL
	,DadosEntrada						VARCHAR(MAX) NULL
	,ResultadoEsperado					VARCHAR(MAX) NULL
	,Observacoes						VARCHAR(MAX) NULL
	,StatusExecucaoHomologacaoID		INT	NOT NULL
	,Ordem								INT NOT NULL
	,CONSTRAINT PK_SolicitacaoRoteiroTesteFuncionalHistorico PRIMARY KEY (SolicitacaoRoteiroTesteFuncionalHistoricoID)
	,CONSTRAINT FK_SolicitacaoRoteiroTesteFuncionalHistorico_SolicitacaoRoteiroTesteFuncional 
		FOREIGN KEY (SolicitacaoRoteiroTesteFuncionalHistoricoID)
		REFERENCES dbo.SolicitacaoRoteiroTesteFuncional (SolicitacaoRoteiroTesteFuncionalID)
)
GO
CREATE INDEX IDX_SolicitacaoRoteiroTesteFuncionalHistorico_SolicitacaoRoteiroTesteFuncional ON dbo.SolicitacaoRoteiroTesteFuncionalHistorico (SolicitacaoRoteiroTesteFuncionalID)
GO

CREATE TABLE dbo.SolicitacaoRoteiroTesteFuncionalEvidencia (
	 SolicitacaoRoteiroTesteFuncionalEvidenciaID INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoRoteiroTesteFuncionalID INT NOT NULL
	,TipoEvidenciaID INT NOT NULL
	,ImagemID		 INT NOT NULL
	,Descricao		 VARCHAR(MAX)
	,CONSTRAINT PK_SolicitacaoRoteiroTesteFuncionalEvidencia PRIMARY KEY (SolicitacaoRoteiroTesteFuncionalEvidenciaID)
	,CONSTRAINT FK_SolicitacaoRoteiroTesteFuncionalEvidencia_SolicitacaoRoteiroTesteFuncional
		FOREIGN KEY (SolicitacaoRoteiroTesteFuncionalID)
		REFERENCES dbo.SolicitacaoRoteiroTesteFuncional (SolicitacaoRoteiroTesteFuncionalID)
	,CONSTRAINT FK_SolicitacaoRoteiroTesteFuncionalEvidencia_Imagem
		FOREIGN KEY (ImagemID)
		REFERENCES dbo.Imagem (ImagemID)
	,CONSTRAINT FK_SolicitacaoRoteiroTesteFuncionalEvidencia_TipoEvidencia
		FOREIGN KEY (TipoEvidenciaID)
		REFERENCES dbo.TipoEvidencia (TipoEvidenciaID)
)
GO
CREATE INDEX IDX_SolicitacaoRoteiroTesteFuncionalEvidencia_SolicitacaoRoteiroTesteFuncional ON dbo.SolicitacaoRoteiroTesteFuncionalEvidencia (SolicitacaoRoteiroTesteFuncionalID)
CREATE INDEX IDX_SolicitacaoRoteiroTesteFuncionalEvidencia_Imagem ON dbo.SolicitacaoRoteiroTesteFuncionalEvidencia (ImagemID)
CREATE INDEX IDX_SolicitacaoRoteiroTesteFuncionalEvidencia_TipoEvidencia ON dbo.SolicitacaoRoteiroTesteFuncionalEvidencia (TipoEvidenciaID)
GO
