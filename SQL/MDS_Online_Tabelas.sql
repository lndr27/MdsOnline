IF NOT EXISTS (SELECT TOP 1 1 FROM sys.databases WHERE [name] = 'BDMdsOnline')
BEGIN
	CREATE DATABASE BDMdsOnline
END
GO
USE BDMdsOnline
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

IF OBJECT_ID('dbo.SolicitacaoRoteiroTesteUnitario') IS NULL
CREATE TABLE dbo.SolicitacaoRoteiroTesteUnitario (
	 SolicitacaoRoteiroTesteUnitarioID	INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID						INT NOT NULL
	,Sequencia							VARCHAR(MAX) NULL
	,Condicao							VARCHAR(MAX) NULL
	,DadosEntrada						VARCHAR(MAX) NULL
	,ResultadoEsperado					VARCHAR(MAX) NULL
	,Verificacao						INT	NOT NULL
	,ComoTestar							VARCHAR(MAX) NULL
	,Observacoes						VARCHAR(MAX) NULL
	,CONSTRAINT PK_SolicitacaoRoteiroTesteUnitario PRIMARY KEY (SolicitacaoRoteiroTesteUnitarioID)
)

IF OBJECT_ID('dbo.SolicitacaoRoteiroTesteUnitarioHistorico') IS NULL
CREATE TABLE dbo.SolicitacaoRoteiroTesteUnitarioHistorico (
	 SolicitacaoRoteiroTesteUnitarioHistoricoID	INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoRoteiroTesteUnitarioID	INT NOT NULL
	,SolicitacaoID						INT NOT NULL
	,Sequencia							VARCHAR(MAX) NULL
	,Condicao							VARCHAR(MAX) NULL
	,DadosEntrada						VARCHAR(MAX) NULL
	,ResultadoEsperado					VARCHAR(MAX) NULL
	,Verificacao						INT	NOT NULL
	,ComoTestar							VARCHAR(MAX) NULL
	,Observacoes						VARCHAR(MAX) NULL
	,CONSTRAINT PK_SolicitacaoRoteiroTesteUnitarioHistorico PRIMARY KEY (SolicitacaoRoteiroTesteUnitarioHistoricoID)
	,CONSTRAINT FK_SolicitacaoRoteiroTesteUnitarioHistorico_SolicitacaoRoteiroTesteUnitario 
		FOREIGN KEY (SolicitacaoRoteiroTesteUnitarioID) 
		REFERENCES dbo.SolicitacaoRoteiroTesteUnitario (SolicitacaoRoteiroTesteUnitarioID)
)