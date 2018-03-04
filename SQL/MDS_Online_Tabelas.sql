IF NOT EXISTS (SELECT TOP 1 1 FROM sys.databases WHERE [name] = 'BDMdsOnline')
BEGIN
	CREATE DATABASE BDMdsOnline
END
GO
USE BDMdsOnline
GO

IF OBJECT_ID('dbo.RtuTesteHistorico') IS NOT NULL DROP TABLE dbo.RtuTesteHistorico
GO
IF OBJECT_ID('dbo.RtuTeste') IS NOT NULL DROP TABLE dbo.RtuTeste
GO
IF OBJECT_ID('dbo.RtuHistorico') IS NOT NULL DROP TABLE dbo.RtuHistorico
GO
IF OBJECT_ID('dbo.rtu') IS NOT NULL DROP TABLE dbo.rtu
GO
IF OBJECT_ID('dbo.RtfTesteEvidenciaHistorico') IS NOT NULL DROP TABLE dbo.RtfTesteEvidenciaHistorico
GO
IF OBJECT_ID('dbo.RtfTesteEvidencia')  IS NOT NULL DROP TABLE dbo.RtfTesteEvidencia
GO
IF OBJECT_ID('dbo.RtfTesteHistorico') IS NOT NULL DROP TABLE dbo.RtfTesteHistorico
GO
IF OBJECT_ID('dbo.RtfTeste') IS NOT NULL DROP TABLE dbo.RtfTeste
GO
IF OBJECT_ID('dbo.RtfHistorico') IS NOT NULL DROP TABLE dbo.RtfHistorico
GO
IF OBJECT_ID('dbo.RTF') IS NOT NULL DROP TABLE dbo.RTF
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

IF OBJECT_ID('dbo.RTU') IS NULL
CREATE TABLE dbo.RTU (
	 RtuID					INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID			INT NOT NULL
	,DataCriacao			DATETIME NOT NULL
	,DataAtualizacao		DATETIME NOT NULL
	,UsuarioID				INT NOT NULL
	,UsuarioVerificacaoID	INT NOT NULL
	,CONSTRAINT PK_RTU PRIMARY KEY (RtuID)
	,CONSTRAINT FK_RTU_UsuarioID
		FOREIGN KEY (UsuarioID)
		REFERENCES dbo.Usuario (UsuarioID)
	,CONSTRAINT FK_RTU_UsuarioVerificacaoID
		FOREIGN KEY (UsuarioVerificacaoID)
		REFERENCES dbo.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_RTU_SolicitacaoID ON dbo.RTU (SolicitacaoID)
CREATE INDEX IDX_RTU_UsuarioID ON dbo.RTU (UsuarioID)
CREATE INDEX IDX_RTU_UsuarioVerificacaoID ON dbo.RTU (UsuarioVerificacaoID)
GO

IF OBJECT_ID('dbo.RtuHistorico') IS NULL
CREATE TABLE dbo.RtuHistorico (
	 RtuHistoricoID			INT NOT NULL IDENTITY(1, 1)
	,RtuID					INT NOT NULL
	,SolicitacaoID			INT NOT NULL
	,DataCriacao			DATETIME NOT NULL
	,DataAtualizacao		DATETIME NOT NULL
	,UsuarioID				INT NOT NULL
	,UsuarioVerificacaoID	INT NOT NULL
	,CONSTRAINT PK_RtuHistorico PRIMARY KEY (RtuHistoricoID)
	,CONSTRAINT FK_RtuHistorico_RtuID
		FOREIGN KEY (RtuID)
		REFERENCES dbo.RTU (RtuID)
)
GO
CREATE INDEX IDX_RtuHistorico_RtuID ON dbo.RTU (RtuID)
CREATE INDEX IDX_RtuHistorico_UsuarioID ON dbo.RTU (UsuarioID)
CREATE INDEX IDX_RtuHistorico_UsuarioVerificacaoID ON dbo.RTU (UsuarioVerificacaoID)
GO

IF OBJECT_ID('dbo.RtuTeste') IS NULL
CREATE TABLE dbo.RtuTeste (
	 RtuTesteID							INT NOT NULL IDENTITY(1, 1)
	,RtuID								INT NOT NULL
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
	,CONSTRAINT PK_RtuTeste PRIMARY KEY (RtuTesteID)
	,CONSTRAINT FK_RtuTeste_RTU
		FOREIGN KEY (RtuID)
		REFERENCES dbo.RTU (RtuID)
	,CONSTRAINT FK_RtuTeste_StatusVerificacaoTesteUnitario
		FOREIGN KEY (StatusVerificacaoTesteUnitarioID)
		REFERENCES dbo.StatusVerificacaoTesteUnitario (StatusVerificacaoTesteUnitarioID)
	,CONSTRAINT FK_RtuTeste_Usuario 
		FOREIGN KEY (UsuarioID)
		REFERENCES dbo.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_RtuTeste_RtuID ON dbo.RtuTeste (RtuID)
CREATE INDEX IDX_RtuTeste_StatusVerificacaoTesteUnitarioID ON dbo.RtuTeste (StatusVerificacaoTesteUnitarioID)
CREATE INDEX IDX_RtuTeste_UsuarioID ON dbo.RtuTeste (UsuarioID)
GO

IF OBJECT_ID('dbo.RtuTesteHistorico') IS NULL
CREATE TABLE dbo.RtuTesteHistorico (
	 RtuTesteHistoricoID				INT NOT NULL IDENTITY(1, 1)
	,RtuTesteID							INT NOT NULL
	,RtuID								INT NOT NULL
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
	,CONSTRAINT PK_RtuTesteHistorico PRIMARY KEY (RtuTesteHistoricoID)
	,CONSTRAINT FK_RtuTesteHistorico_RtuTeste
		FOREIGN KEY (RtuTesteID)
		REFERENCES dbo.RtuTeste (RtuTesteID)
)
GO
CREATE INDEX IDX_SolicitacaoRTUHistorico_RtuTesteID ON dbo.RtuTesteHistorico (RtuTesteID)
CREATE INDEX IDX_SolicitacaoRTUHistorico_UsuarioID ON dbo.RtuTesteHistorico (UsuarioID)
CREATE INDEX IDX_SolicitacaoRTUHistorico_RtuID ON dbo.RtuTesteHistorico (RtuID)
CREATE INDEX IDX_SolicitacaoRTUHistorico_StatusVerificacaoTesteUnitarioID ON dbo.RtuTesteHistorico (StatusVerificacaoTesteUnitarioID)
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


IF OBJECT_ID('dbo.RTF') IS NULL
CREATE TABLE dbo.RTF (
	 RtfID					INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID			INT NOT NULL
	,DataCriacao			DATETIME NOT NULL
	,DataAtualizacao		DATETIME NOT NULL
	,UsuarioID				INT NOT NULL
	,UsuarioVerificacaoID	INT NOT NULL
	,CONSTRAINT PK_RTF PRIMARY KEY (RtfID)
	,CONSTRAINT FK_RTF_UsuarioID
		FOREIGN KEY (UsuarioID)
		REFERENCES dbo.Usuario (UsuarioID)
	,CONSTRAINT FK_RTF_UsuarioVerificacaoID
		FOREIGN KEY (UsuarioVerificacaoID)
		REFERENCES dbo.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_RTF_SolicitacaoID ON dbo.RTF (SolicitacaoID)
CREATE INDEX IDX_RTF_UsuarioID ON dbo.RTF (UsuarioID)
CREATE INDEX IDX_RTF_UsuarioVerificacaoID ON dbo.RTF (UsuarioVerificacaoID)
GO

IF OBJECT_ID('dbo.RtfHistorico') IS NULL
CREATE TABLE dbo.RtfHistorico (
	 RtfHistoricoID			INT NOT NULL IDENTITY(1, 1)
	,RtfID					INT NOT NULL
	,SolicitacaoID			INT NOT NULL
	,DataCriacao			DATETIME NOT NULL
	,DataAtualizacao		DATETIME NOT NULL
	,UsuarioID				INT NOT NULL
	,UsuarioVerificacaoID	INT NOT NULL
	,CONSTRAINT PK_RtfHistorico PRIMARY KEY (RtfHistoricoID)
	,CONSTRAINT FK_RtfHistorico_RtfID
		FOREIGN KEY (RtfID)
		REFERENCES dbo.RTF (RtfID)
)
GO
CREATE INDEX IDX_RtfHistorico_RtfID ON dbo.RTF (RtfID)
CREATE INDEX IDX_RtfHistorico_UsuarioID ON dbo.RTF (UsuarioID)
CREATE INDEX IDX_RtfHistorico_UsuarioVerificacaoID ON dbo.RTF (UsuarioVerificacaoID)
GO

IF OBJECT_ID('dbo.RtfTeste') IS NULL
CREATE TABLE dbo.RtfTeste (
	 RtfTesteID					INT NOT NULL IDENTITY(1, 1)
	,RtfID							INT NOT NULL
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
	,CONSTRAINT PK_RtfTeste PRIMARY KEY (RtfTesteID)
	,CONSTRAINT FK_RtfTeste_Rtf
		FOREIGN KEY (RtfID)
		REFERENCES dbo.RTF (RtfID)
	,CONSTRAINT FK_RtfTeste_StatusExecucaoHomologacao 
		FOREIGN KEY (StatusExecucaoHomologacaoID)
		REFERENCES dbo.StatusExecucaoHomologacao (StatusExecucaoHomologacaoID)
	,CONSTRAINT FK_RtfTeste_Usuario
		FOREIGN KEY (UsuarioID)
		REFERENCES dbo.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_RtfTeste_RtfID ON dbo.RTF (RtfID)
CREATE INDEX IDX_RtfTeste_StatusExecucaoHomologacaoID ON dbo.RtfTeste (StatusExecucaoHomologacaoID)
CREATE INDEX IDX_RtfTeste_UsuarioID ON dbo.RtfTeste (UsuarioID)
GO

IF OBJECT_ID('dbo.RtfTesteHistorico') IS NULL
CREATE TABLE dbo.RtfTesteHistorico (
	 RtfTesteHistoricoID			INT NOT NULL IDENTITY(1, 1)
	,RtfTesteID						INT NOT NULL
	,RtfID							INT NOT NULL
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
	,CONSTRAINT PK_RtfTesteHistorico PRIMARY KEY (RtfTesteHistoricoID)
	,CONSTRAINT FK_RtfTesteHistorico_RtfTesteID
		FOREIGN KEY (RtfTesteID)
		REFERENCES dbo.RtfTeste (RtfTesteID)

)
GO
CREATE INDEX IDX_RtfTesteHistorico_RtfID ON dbo.RtfTesteHistorico (RtfID)
CREATE INDEX IDX_RtfTesteHistorico_RtfTesteID ON dbo.RtfTesteHistorico (RtfTesteID)
CREATE INDEX IDX_RtfTesteHistorico_StatusExecucaoHomologacaoID ON dbo.RtfTesteHistorico (StatusExecucaoHomologacaoID)
CREATE INDEX IDX_RtfTesteHistorico_UsuarioID ON dbo.RtfTesteHistorico (UsuarioID)
GO

IF OBJECT_ID('dbo.RtfTesteEvidencia') IS NULL
CREATE TABLE dbo.RtfTesteEvidencia (
	 RtfTesteEvidenciaID		INT NOT NULL IDENTITY(1, 1)
	,RtfTesteID					INT NOT NULL
	,TipoEvidenciaID			INT NOT NULL
	,ArquivoID					INT NOT NULL
	,Descricao					VARCHAR(MAX)
	,Ordem						INT NOT NULL
	,DataAtualizacao			DATETIME NOT NULL
	,UsuarioID					INT NOT NULL
	,CONSTRAINT PK_RtfTesteEvidencia PRIMARY KEY (RtfTesteEvidenciaID)
	,CONSTRAINT FK_RtfTesteEvidencia_RtfTeste
		FOREIGN KEY (RtfTesteID)
		REFERENCES dbo.RtfTeste (RtfTesteID)
	,CONSTRAINT FK_RtfTesteEvidencia_Arquivo
		FOREIGN KEY (ArquivoID)
		REFERENCES dbo.Arquivo (ArquivoID)
	,CONSTRAINT FK_RtfTesteEvidencia_TipoEvidencia
		FOREIGN KEY (TipoEvidenciaID)
		REFERENCES dbo.TipoEvidencia (TipoEvidenciaID)
	,CONSTRAINT FK_RtfTesteEvidencia_Usuario
		FOREIGN KEY (UsuarioID)
		REFERENCES dbo.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_SolicitacaoRTFEvidencia_RtfTesteID			ON dbo.RtfTesteEvidencia (RtfTesteID)
CREATE INDEX IDX_SolicitacaoRTFEvidencia_ArquivoID			ON dbo.RtfTesteEvidencia (ArquivoID)
CREATE INDEX IDX_SolicitacaoRTFEvidencia_TipoEvidenciaID	ON dbo.RtfTesteEvidencia (TipoEvidenciaID)
CREATE INDEX IDX_SolicitacaoRTFEvidencia_UsuarioID			ON dbo.RtfTesteEvidencia (UsuarioID)
GO

IF OBJECT_ID('dbo.RtfTesteEvidenciaHistorico') IS NULL
CREATE TABLE dbo.RtfTesteEvidenciaHistorico (
	 RtfTesteEvidenciaHistoricoID		INT NOT NULL IDENTITY(1, 1)
	,RtfTesteEvidenciaID				INT NOT NULL
	,RtfTesteID							INT NOT NULL
	,TipoEvidenciaID					INT NOT NULL
	,ArquivoID							INT NOT NULL
	,Descricao							VARCHAR(MAX)
	,Ordem								INT NOT NULL
	,DataAtualizacao					DATETIME NOT NULL
	,UsuarioID							INT NOT NULL
	,CONSTRAINT PK_RtfTesteEvidenciaHistorico PRIMARY KEY (RtfTesteEvidenciaHistoricoID)
	,CONSTRAINT FK_RtfTesteEvidenciaHistorico_RtfTesteEvidencia
		FOREIGN KEY (RtfTesteEvidenciaID)
		REFERENCES dbo.RtfTesteEvidencia (RtfTesteEvidenciaID)
)
GO
CREATE INDEX IDX_RtfTesteEvidenciaHistorico_RtfTesteEvidenciaID ON dbo.RtfTesteEvidenciaHistorico (RtfTesteEvidenciaID)
CREATE INDEX IDX_RtfTesteEvidenciaHistorico_RtfTesteID			ON dbo.RtfTesteEvidenciaHistorico (RtfTesteID)
CREATE INDEX IDX_RtfTesteEvidenciaHistorico_ArquivoID			ON dbo.RtfTesteEvidenciaHistorico (ArquivoID)
CREATE INDEX IDX_RtfTesteEvidenciaHistorico_TipoEvidenciaID		ON dbo.RtfTesteEvidenciaHistorico (TipoEvidenciaID)
CREATE INDEX IDX_RtfTesteEvidenciaHistorico_UsuarioID			ON dbo.RtfTesteEvidenciaHistorico (UsuarioID)
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