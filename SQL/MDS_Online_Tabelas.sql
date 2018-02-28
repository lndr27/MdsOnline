IF NOT EXISTS (SELECT TOP 1 1 FROM sys.databases WHERE [name] = 'BDMdsOnline')
BEGIN
	CREATE DATABASE BDMdsOnline
END
GO
USE BDMdsOnline
GO

IF OBJECT_ID('dbo.SolicitacaoRTUHistorico') IS NOT NULL DROP TABLE dbo.SolicitacaoRTUHistorico
GO
IF OBJECT_ID('dbo.SolicitacaoRTU') IS NOT NULL DROP TABLE dbo.SolicitacaoRTU
GO
IF OBJECT_ID('dbo.SolicitacaoRTFEvidencia') IS NOT NULL DROP TABLE dbo.SolicitacaoRTFEvidencia
GO
IF OBJECT_ID('dbo.SolicitacaoRTFHistorico') IS NOT NULL DROP TABLE dbo.SolicitacaoRTFHistorico
GO
IF OBJECT_ID('dbo.SolicitacaoRTF') IS NOT NULL DROP TABLE dbo.SolicitacaoRTF
GO
IF OBJECT_ID('dbo.StatusExecucaoHomologacao') IS NOT NULL DROP TABLE dbo.StatusExecucaoHomologacao
GO
IF OBJECT_ID('dbo.StatusVerificacaoTesteUnitario') IS NOT NULL DROP TABLE dbo.StatusVerificacaoTesteUnitario
GO
IF OBJECT_ID('dbo.Arquivo') IS NOT NULL DROP TABLE dbo.Arquivo
GO
IF OBJECT_ID('dbo.TipoEvidencia') IS NOT NULL DROP TABLE dbo.TipoEvidencia
GO
IF OBJECT_ID('dbo.Usuario') IS NOT NULL DROP TABLE dbo.Usuario
GO


IF OBJECT_ID('dbo.Usuario') IS NULL
CREATE TABLE dbo.Usuario (
	 UsuarioID			INT NOT NULL IDENTITY(1, 1)
	,Nome				VARCHAR(200) NOT NULL
	,Codigo				VARCHAR(100) NOT NULL
	,Email				VARCHAR(200) NOT NULL
	,DataCriacao		DATETIME NOT NULL
	,DataAtualizacao	DATETIME NOT NULL
	,CONSTRAINT PK_Usuario PRIMARY KEY (UsuarioID)
)
GO


IF OBJECT_ID('dbo.Arquivo') IS NULL
CREATE TABLE dbo.Arquivo (
	 ArquivoID		INT NOT NULL IDENTITY(1, 1)
	,[Guid]			UNIQUEIDENTIFIER NOT NULL	
	,Nome			VARCHAR(1000)
	,Extensao		VARCHAR(100)
	,ContentType	VARCHAR(100)
	,TamanhoKB		FLOAT
	,Arquivo		VARBINARY(MAX)
	,IsRascunho		BIT NOT NULL CONSTRAINT DF_Arquivo_IsRascunho DEFAULT(0)
	,DataUpload		DATETIME NOT NULL
	,UsuarioID		INT NOT NULL
	,CONSTRAINT PK_Arquivo PRIMARY KEY (ArquivoID)
	,CONSTRAINT FK_Arquivo_Usuario FOREIGN KEY (UsuarioID) REFERENCES dbo.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_Arquivo_Guid ON dbo.Arquivo ([Guid])
CREATE INDEX IDX_Arquivo_Usuario ON dbo.Usuario (UsuarioID)
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

IF OBJECT_ID('dbo.SolicitacaoRTU') IS NULL
CREATE TABLE dbo.SolicitacaoRTU (
	 SolicitacaoRTUID					INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID						INT NOT NULL
	,Sequencia							VARCHAR(MAX) NULL
	,Condicao							VARCHAR(MAX) NULL
	,DadosEntrada						VARCHAR(MAX) NULL
	,ResultadoEsperado					VARCHAR(MAX) NULL
	,StatusVerificacaoTesteUnitarioID 	INT	NOT NULL
	,ComoTestar							VARCHAR(MAX) NULL
	,Observacoes						VARCHAR(MAX) NULL
	,Ordem								INT NOT NULL
	,DataAtualizacao					DATETIME NOT NULL
	,UsuarioID							INT NOT NULL
	,CONSTRAINT PK_SolicitacaoRTU PRIMARY KEY (SolicitacaoRTUID)
	,CONSTRAINT FK_SolicitacaoRTU_StatusVerificacaoTesteUnitario
		FOREIGN KEY (StatusVerificacaoTesteUnitarioID)
		REFERENCES dbo.StatusVerificacaoTesteUnitario (StatusVerificacaoTesteUnitarioID)
	,CONSTRAINT FK_SolicitacaoRTU_Usuario 
		FOREIGN KEY (UsuarioID)
		REFERENCES dbo.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_SolicitacaoRTU_StatusVerificacaoTesteUnitario ON dbo.SolicitacaoRTU (StatusVerificacaoTesteUnitarioID)
CREATE INDEX IDX_SolicitacaoRTU_Usuario ON dbo.SolicitacaoRTU (UsuarioID)
GO

IF OBJECT_ID('dbo.SolicitacaoRTUHistorico') IS NULL
CREATE TABLE dbo.SolicitacaoRTUHistorico (
	 SolicitacaoRTUHistoricoID			INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoRTUID					INT NOT NULL
	,SolicitacaoID						INT NOT NULL
	,Sequencia							VARCHAR(MAX) NULL
	,Condicao							VARCHAR(MAX) NULL
	,DadosEntrada						VARCHAR(MAX) NULL
	,ResultadoEsperado					VARCHAR(MAX) NULL
	,StatusVerificacaoTesteUnitarioID	INT	NOT NULL
	,ComoTestar							VARCHAR(MAX) NULL
	,Observacoes						VARCHAR(MAX) NULL
	,Ordem								INT NOT NULL
	,DataAtualizacao					DATETIME NOT NULL
	,UsuarioID							INT NOT NULL
	,CONSTRAINT PK_SolicitacaoRTUHistorico PRIMARY KEY (SolicitacaoRTUHistoricoID)
)
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

IF OBJECT_ID('dbo.SolicitacaoRTF') IS NULL
CREATE TABLE dbo.SolicitacaoRTF (
	 SolicitacaoRTFID				INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID					INT NOT NULL
	,Sequencia						VARCHAR(MAX) NULL
	,Funcionalidade					VARCHAR(MAX) NULL
	,CondicaoCenario				VARCHAR(MAX) NULL
	,PreCondicao					VARCHAR(MAX) NULL
	,DadosEntrada					VARCHAR(MAX) NULL
	,ResultadoEsperado				VARCHAR(MAX) NULL
	,Observacoes					VARCHAR(MAX) NULL
	,StatusExecucaoHomologacaoID	INT	NOT NULL
	,Ordem							INT NOT NULL
	,DataAtualizacao				DATETIME NOT NULL
	,UsuarioID						INT NOT NULL
	,CONSTRAINT PK_SolicitacaoRTF PRIMARY KEY (SolicitacaoRTFID)
	,CONSTRAINT FK_SolicitacaoRTF_StatusExecucaoHomologacao 
		FOREIGN KEY (StatusExecucaoHomologacaoID)
		REFERENCES dbo.StatusExecucaoHomologacao (StatusExecucaoHomologacaoID)
	,CONSTRAINT FK_SolicitacaoRTF_Usuario
		FOREIGN KEY (UsuarioID)
		REFERENCES dbo.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_SolicitacaoRTF_StatusExecucaoHomologacao ON dbo.SolicitacaoRTF (StatusExecucaoHomologacaoID)
CREATE INDEX IDX_SolicitacaoRTF_Usuario ON dbo.SolicitacaoRTF (UsuarioID)
GO

IF OBJECT_ID('dbo.SolicitacaoRTFHistorico') IS NULL
CREATE TABLE dbo.SolicitacaoRTFHistorico (
	 SolicitacaoRTFHistoricoID		INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoRTFID				INT NOT NULL
	,SolicitacaoID					INT NOT NULL
	,Sequencia						VARCHAR(MAX) NULL
	,Funcionalidade					VARCHAR(MAX) NULL
	,CondicaoCenario				VARCHAR(MAX) NULL
	,PreCondicao					VARCHAR(MAX) NULL
	,DadosEntrada					VARCHAR(MAX) NULL
	,ResultadoEsperado				VARCHAR(MAX) NULL
	,Observacoes					VARCHAR(MAX) NULL
	,StatusExecucaoHomologacaoID	INT	NOT NULL
	,Ordem							INT NOT NULL
	,DataAtualizacao				DATETIME NOT NULL
	,UsuarioID						INT NOT NULL
	,CONSTRAINT PK_SolicitacaoRTFHistorico PRIMARY KEY (SolicitacaoRTFHistoricoID)
)
GO

CREATE TABLE dbo.SolicitacaoRTFEvidencia (
	 SolicitacaoRTFEvidenciaID	INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoRTFID			INT NOT NULL
	,TipoEvidenciaID			INT NOT NULL
	,ArquivoID					INT NOT NULL
	,Descricao					VARCHAR(MAX)
	,Ordem						INT NOT NULL
	,DataAtualizacao			DATETIME NOT NULL
	,UsuarioID					INT NOT NULL
	,CONSTRAINT PK_SolicitacaoRTFEvidencia PRIMARY KEY (SolicitacaoRTFEvidenciaID)
	,CONSTRAINT FK_SolicitacaoRTFEvidencia_SolicitacaoRTF
		FOREIGN KEY (SolicitacaoRTFID)
		REFERENCES dbo.SolicitacaoRTF (SolicitacaoRTFID)
	,CONSTRAINT FK_SolicitacaoRTFEvidencia_Arquivo
		FOREIGN KEY (ArquivoID)
		REFERENCES dbo.Arquivo (ArquivoID)
	,CONSTRAINT FK_SolicitacaoRTFEvidencia_TipoEvidencia
		FOREIGN KEY (TipoEvidenciaID)
		REFERENCES dbo.TipoEvidencia (TipoEvidenciaID)
	,CONSTRAINT FK_SolicitacaoRTFEvidencia_Usuario
		FOREIGN KEY (UsuarioID)
		REFERENCES dbo.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_SolicitacaoRTFEvidencia_SolicitacaoRTF ON dbo.SolicitacaoRTFEvidencia (SolicitacaoRTFID)
CREATE INDEX IDX_SolicitacaoRTFEvidencia_Arquivo ON dbo.SolicitacaoRTFEvidencia (ArquivoID)
CREATE INDEX IDX_SolicitacaoRTFEvidencia_TipoEvidencia ON dbo.SolicitacaoRTFEvidencia (TipoEvidenciaID)
CREATE INDEX IDX_SolicitacaoRTFEvidencia_Usuario ON dbo.SolicitacaoRTFEvidencia (UsuarioID)
GO

CREATE TABLE dbo.SolicitacaoRTFEvidenciaHistorico (
	 SolicitacaoRTFEvidenciaHistoricoID INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoRTFEvidenciaID			INT NOT NULL
	,SolicitacaoRTFID					INT NOT NULL
	,TipoEvidenciaID					INT NOT NULL
	,ArquivoID							INT NOT NULL
	,Descricao							VARCHAR(MAX)
	,Ordem								INT NOT NULL
	,DataAtualizacao					DATETIME NOT NULL
	,CONSTRAINT PK_SolicitacaoRTFEvidenciaHistorico PRIMARY KEY (SolicitacaoRTFEvidenciaHistoricoID)
)
GO






--============================================================================================================================
RETURN
print 'DONE'










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