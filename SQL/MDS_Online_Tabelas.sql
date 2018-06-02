IF NOT EXISTS (SELECT TOP 1 1 FROM sys.databases WHERE [name] = 'BDMdsOnline')
BEGIN
	CREATE DATABASE BDMdsOnline
END
GO
USE BDMdsOnline
GO
IF NOT EXISTS (SELECT TOP 1 1 FROM sys.schemas WHERE [name] = 'MDS')
BEGIN
	DECLARE @CreateSchema NVARCHAR(MAX) = N'CREATE SCHEMA MDS';
	EXEC SP_EXECUTESQL @CreateSchema
END
GO

IF OBJECT_ID('MDS.RtuTesteHistorico') IS NOT NULL DROP TABLE MDS.RtuTesteHistorico
GO
IF OBJECT_ID('MDS.RtuTeste') IS NOT NULL DROP TABLE MDS.RtuTeste
GO
IF OBJECT_ID('MDS.RtuHistorico') IS NOT NULL DROP TABLE MDS.RtuHistorico
GO
IF OBJECT_ID('MDS.rtu') IS NOT NULL DROP TABLE MDS.rtu
GO
IF OBJECT_ID('MDS.RtfTesteEvidenciaHistorico') IS NOT NULL DROP TABLE MDS.RtfTesteEvidenciaHistorico
GO
IF OBJECT_ID('MDS.RtfTesteEvidencia')  IS NOT NULL DROP TABLE MDS.RtfTesteEvidencia
GO
IF OBJECT_ID('MDS.RtfTesteHistorico') IS NOT NULL DROP TABLE MDS.RtfTesteHistorico
GO
IF OBJECT_ID('MDS.RtfTeste') IS NOT NULL DROP TABLE MDS.RtfTeste
GO
IF OBJECT_ID('MDS.RtfHistorico') IS NOT NULL DROP TABLE MDS.RtfHistorico
GO
IF OBJECT_ID('MDS.Rtf') IS NOT NULL DROP TABLE MDS.Rtf
GO
IF OBJECT_ID('MDS.StatusExecucaoHomologacao') IS NOT NULL DROP TABLE MDS.StatusExecucaoHomologacao
GO
IF OBJECT_ID('MDS.StatusVerificacaoTesteUnitario') IS NOT NULL DROP TABLE MDS.StatusVerificacaoTesteUnitario
GO
IF OBJECT_ID('MDS.CheckListHistorico') IS NOT NULL DROP TABLE MDS.CheckListHistorico
GO
IF OBJECT_ID('MDS.CheckListGrupoItemHistorico') IS NOT NULL DROP TABLE MDS.CheckListGrupoItemHistorico
GO
IF OBJECT_ID('MDS.CheckListItemHistorico') IS NOT NULL DROP TABLE MDS.CheckListItemHistorico
GO
IF OBJECT_ID('MDS.CheckListItemRespostaHistorico') IS NOT NULL DROP TABLE MDS.CheckListItemRespostaHistorico
GO
IF OBJECT_ID('MDS.CheckListItemResposta') IS NOT NULL DROP TABLE MDS.CheckListItemResposta
GO
IF OBJECT_ID('MDS.CheckListItem') IS NOT NULL DROP TABLE MDS.CheckListItem
GO
IF OBJECT_ID('MDS.CheckListGrupoItem') IS NOT NULL DROP TABLE MDS.CheckListGrupoItem
GO
IF OBJECT_ID('MDS.CheckList') IS NOT NULL DROP TABLE MDS.CheckList
GO
IF OBJECT_ID('MDS.CheckListSolicitacaoHistorico') IS NOT NULL DROP TABLE MDS.CheckListSolicitacaoHistorico
GO
IF OBJECT_ID('MDS.CheckListSolicitacao') IS NOT NULL DROP TABLE MDS.CheckListSolicitacao
GO
IF OBJECT_ID('MDS.Arquivo') IS NOT NULL DROP TABLE MDS.Arquivo
GO
IF OBJECT_ID('MDS.TipoEvidencia') IS NOT NULL DROP TABLE MDS.TipoEvidencia
GO
IF OBJECT_ID('MDS.Usuario') IS NOT NULL DROP TABLE MDS.Usuario
GO



-- GENERICAS --
IF OBJECT_ID('MDS.Usuario') IS NULL
CREATE TABLE MDS.Usuario (
	 UsuarioID			INT NOT NULL IDENTITY(1, 1)
	,Nome				VARCHAR(200) NOT NULL
	,Codigo				VARCHAR(100) NOT NULL
	,Email				VARCHAR(200) NOT NULL
	,DataCriacao		DATETIME NOT NULL
	,DataAtualizacao	DATETIME NOT NULL
	,CONSTRAINT PK_Usuario PRIMARY KEY (UsuarioID)
)
GO

IF OBJECT_ID('MDS.Arquivo') IS NULL
CREATE TABLE MDS.Arquivo (
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
	,CONSTRAINT FK_Arquivo_Usuario FOREIGN KEY (UsuarioID) REFERENCES MDS.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_Arquivo_Guid ON MDS.Arquivo ([Guid])
CREATE INDEX IDX_Arquivo_Usuario ON MDS.Usuario (UsuarioID)
GO

--=== Rtu ============================================================

IF OBJECT_ID('MDS.StatusVerificacaoTesteUnitario') IS NULL
CREATE TABLE MDS.StatusVerificacaoTesteUnitario (
	 StatusVerificacaoTesteUnitarioID INT NOT NULL IDENTITY(1, 1)
	,Nome VARCHAR(100) NOT NULL
	,CONSTRAINT PK_StatusVerificacaoTesteUnitario PRIMARY KEY (StatusVerificacaoTesteUnitarioID)
)
GO
SET IDENTITY_INSERT MDS.StatusVerificacaoTesteUnitario ON
	INSERT INTO MDS.StatusVerificacaoTesteUnitario (StatusVerificacaoTesteUnitarioID, Nome)
	VALUES (1, 'Não Testado'), (2, 'Sim, OK'), (3, 'Sim, NOK')
SET IDENTITY_INSERT MDS.StatusVerificacaoTesteUnitario OFF
GO

IF OBJECT_ID('MDS.Rtu') IS NULL
CREATE TABLE MDS.Rtu (
	 RtuID					INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID			INT NOT NULL
	,DataCriacao			DATETIME NOT NULL
	,DataAtualizacao		DATETIME NOT NULL
	,UsuarioID				INT NOT NULL
	,UsuarioVerificacaoID	INT
	,UsuarioAtualizacaoID	INT NOT NULL
	,CONSTRAINT PK_RTU PRIMARY KEY (RtuID)
	,CONSTRAINT FK_RTU_UsuarioID
		FOREIGN KEY (UsuarioID)
		REFERENCES MDS.Usuario (UsuarioID)
	,CONSTRAINT FK_RTU_UsuarioVerificacaoID
		FOREIGN KEY (UsuarioVerificacaoID)
		REFERENCES MDS.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_RTU_SolicitacaoID ON MDS.Rtu (SolicitacaoID)
CREATE INDEX IDX_RTU_UsuarioID ON MDS.Rtu (UsuarioID)
CREATE INDEX IDX_RTU_UsuarioVerificacaoID ON MDS.Rtu (UsuarioVerificacaoID)
GO

IF OBJECT_ID('MDS.RtuHistorico') IS NULL
CREATE TABLE MDS.RtuHistorico (
	 RtuHistoricoID			INT NOT NULL IDENTITY(1, 1)
	,RtuID					INT NOT NULL
	,SolicitacaoID			INT
	,DataCriacao			DATETIME
	,DataAtualizacao		DATETIME
	,UsuarioID				INT
	,UsuarioVerificacaoID	INT
	,UsuarioAtualizacaoID	INT NOT NULL
	,CONSTRAINT PK_RtuHistorico PRIMARY KEY (RtuHistoricoID)
)
GO
CREATE INDEX IDX_RtuHistorico_RtuID ON MDS.Rtu (RtuID)
CREATE INDEX IDX_RtuHistorico_UsuarioID ON MDS.Rtu (UsuarioID)
CREATE INDEX IDX_RtuHistorico_UsuarioVerificacaoID ON MDS.Rtu (UsuarioVerificacaoID)
GO

IF OBJECT_ID('MDS.RtuTeste') IS NULL
CREATE TABLE MDS.RtuTeste (
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
		REFERENCES MDS.Rtu (RtuID)
	,CONSTRAINT FK_RtuTeste_StatusVerificacaoTesteUnitario
		FOREIGN KEY (StatusVerificacaoTesteUnitarioID)
		REFERENCES MDS.StatusVerificacaoTesteUnitario (StatusVerificacaoTesteUnitarioID)
	,CONSTRAINT FK_RtuTeste_Usuario 
		FOREIGN KEY (UsuarioID)
		REFERENCES MDS.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_RtuTeste_RtuID ON MDS.RtuTeste (RtuID)
CREATE INDEX IDX_RtuTeste_StatusVerificacaoTesteUnitarioID ON MDS.RtuTeste (StatusVerificacaoTesteUnitarioID)
CREATE INDEX IDX_RtuTeste_UsuarioID ON MDS.RtuTeste (UsuarioID)
GO

IF OBJECT_ID('MDS.RtuTesteHistorico') IS NULL
CREATE TABLE MDS.RtuTesteHistorico (
	 RtuTesteHistoricoID				INT NOT NULL IDENTITY(1, 1)
	,RtuTesteID							INT NOT NULL
	,RtuID								INT
	,Sequencia							VARCHAR(MAX)
	,Condicao							VARCHAR(MAX)
	,DadosEntrada						VARCHAR(MAX)
	,ResultadoEsperado					VARCHAR(MAX)
	,StatusVerificacaoTesteUnitarioID	INT
	,ComoTestar							VARCHAR(MAX)
	,Observacoes						VARCHAR(MAX)
	,Ordem								INT
	,DataAtualizacao					DATETIME
	,UsuarioID							INT
	,CONSTRAINT PK_RtuTesteHistorico PRIMARY KEY (RtuTesteHistoricoID)
)
GO
CREATE INDEX IDX_SolicitacaoRTUHistorico_RtuTesteID ON MDS.RtuTesteHistorico (RtuTesteID)
CREATE INDEX IDX_SolicitacaoRTUHistorico_UsuarioID ON MDS.RtuTesteHistorico (UsuarioID)
CREATE INDEX IDX_SolicitacaoRTUHistorico_RtuID ON MDS.RtuTesteHistorico (RtuID)
CREATE INDEX IDX_SolicitacaoRTUHistorico_StatusVerificacaoTesteUnitarioID ON MDS.RtuTesteHistorico (StatusVerificacaoTesteUnitarioID)
GO


--=== Rtf ============================================================

IF OBJECT_ID('MDS.TipoEvidencia') IS NULL
CREATE TABLE MDS.TipoEvidencia (
	 TipoEvidenciaID INT NOT NULL IDENTITY(1, 1)
	,Nome VARCHAR(100) NOT NULL
	,CONSTRAINT PK_TipoEvidencia PRIMARY KEY (TipoEvidenciaID)
)
GO
SET IDENTITY_INSERT MDS.TipoEvidencia ON
	INSERT INTO MDS.TipoEvidencia (TipoEvidenciaID, Nome)
	VALUES (1, 'Sucesso'), (2, 'Erro')
SET IDENTITY_INSERT MDS.TipoEvidencia OFF
GO

IF OBJECT_ID('MDS.StatusExecucaoHomologacao') IS NULL
CREATE TABLE MDS.StatusExecucaoHomologacao (
	 StatusExecucaoHomologacaoID INT NOT NULL IDENTITY(1, 1)
	,Nome VARCHAR(100) NOT NULL
	,CONSTRAINT PK_StatusExecucaoHomologacao PRIMARY KEY (StatusExecucaoHomologacaoID)
)
GO
SET IDENTITY_INSERT MDS.StatusExecucaoHomologacao ON
	INSERT INTO MDS.StatusExecucaoHomologacao (StatusExecucaoHomologacaoID, Nome)
	VALUES (1, 'Não Testado'), (2, 'Sim, OK'), (3, 'Sim, NOK')
SET IDENTITY_INSERT MDS.StatusExecucaoHomologacao OFF
GO


IF OBJECT_ID('MDS.Rtf') IS NULL
CREATE TABLE MDS.Rtf (
	 RtfID					INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID			INT NOT NULL
	,DataCriacao			DATETIME NOT NULL
	,DataAtualizacao		DATETIME NOT NULL
	,UsuarioID				INT NOT NULL
	,UsuarioVerificacaoID	INT
	,UsuarioAtualizacaoID	INT NOT NULL
	,CONSTRAINT PK_RTF PRIMARY KEY (RtfID)
	,CONSTRAINT FK_RTF_UsuarioID
		FOREIGN KEY (UsuarioID)
		REFERENCES MDS.Usuario (UsuarioID)
	,CONSTRAINT FK_RTF_UsuarioVerificacaoID
		FOREIGN KEY (UsuarioVerificacaoID)
		REFERENCES MDS.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_RTF_SolicitacaoID ON MDS.Rtf (SolicitacaoID)
CREATE INDEX IDX_RTF_UsuarioID ON MDS.Rtf (UsuarioID)
CREATE INDEX IDX_RTF_UsuarioVerificacaoID ON MDS.Rtf (UsuarioVerificacaoID)
GO

IF OBJECT_ID('MDS.RtfHistorico') IS NULL
CREATE TABLE MDS.RtfHistorico (
	 RtfHistoricoID			INT NOT NULL IDENTITY(1, 1)
	,RtfID					INT NOT NULL
	,SolicitacaoID			INT
	,DataCriacao			DATETIME 
	,DataAtualizacao		DATETIME 
	,UsuarioID				INT
	,UsuarioVerificacaoID	INT
	,UsuarioAtualizacaoID	INT NOT NULL
	,CONSTRAINT PK_RtfHistorico PRIMARY KEY (RtfHistoricoID)
)
GO
CREATE INDEX IDX_RtfHistorico_RtfID ON MDS.Rtf (RtfID)
CREATE INDEX IDX_RtfHistorico_UsuarioID ON MDS.Rtf (UsuarioID)
CREATE INDEX IDX_RtfHistorico_UsuarioVerificacaoID ON MDS.Rtf (UsuarioVerificacaoID)
GO

IF OBJECT_ID('MDS.RtfTeste') IS NULL
CREATE TABLE MDS.RtfTeste (
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
		REFERENCES MDS.Rtf (RtfID)
	,CONSTRAINT FK_RtfTeste_StatusExecucaoHomologacao 
		FOREIGN KEY (StatusExecucaoHomologacaoID)
		REFERENCES MDS.StatusExecucaoHomologacao (StatusExecucaoHomologacaoID)
	,CONSTRAINT FK_RtfTeste_Usuario
		FOREIGN KEY (UsuarioID)
		REFERENCES MDS.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_RtfTeste_RtfID ON MDS.Rtf (RtfID)
CREATE INDEX IDX_RtfTeste_StatusExecucaoHomologacaoID ON MDS.RtfTeste (StatusExecucaoHomologacaoID)
CREATE INDEX IDX_RtfTeste_UsuarioID ON MDS.RtfTeste (UsuarioID)
GO

IF OBJECT_ID('MDS.RtfTesteHistorico') IS NULL
CREATE TABLE MDS.RtfTesteHistorico (
	 RtfTesteHistoricoID			INT NOT NULL IDENTITY(1, 1)
	,RtfTesteID						INT NOT NULL
	,RtfID							INT
	,Sequencia						VARCHAR(MAX)
	,Funcionalidade					VARCHAR(MAX)
	,CondicaoCenario				VARCHAR(MAX)
	,PreCondicao					VARCHAR(MAX)
	,DadosEntrada					VARCHAR(MAX)
	,ResultadoEsperado				VARCHAR(MAX)
	,Observacoes					VARCHAR(MAX)
	,StatusExecucaoHomologacaoID	INT
	,Ordem							INT
	,DataAtualizacao				DATETIME
	,UsuarioID						INT
	,CONSTRAINT PK_RtfTesteHistorico PRIMARY KEY (RtfTesteHistoricoID)
)
GO
CREATE INDEX IDX_RtfTesteHistorico_RtfID ON MDS.RtfTesteHistorico (RtfID)
CREATE INDEX IDX_RtfTesteHistorico_RtfTesteID ON MDS.RtfTesteHistorico (RtfTesteID)
CREATE INDEX IDX_RtfTesteHistorico_StatusExecucaoHomologacaoID ON MDS.RtfTesteHistorico (StatusExecucaoHomologacaoID)
CREATE INDEX IDX_RtfTesteHistorico_UsuarioID ON MDS.RtfTesteHistorico (UsuarioID)
GO

IF OBJECT_ID('MDS.RtfTesteEvidencia') IS NULL
CREATE TABLE MDS.RtfTesteEvidencia (
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
		REFERENCES MDS.RtfTeste (RtfTesteID)
		ON DELETE CASCADE
	,CONSTRAINT FK_RtfTesteEvidencia_Arquivo
		FOREIGN KEY (ArquivoID)
		REFERENCES MDS.Arquivo (ArquivoID)
	,CONSTRAINT FK_RtfTesteEvidencia_TipoEvidencia
		FOREIGN KEY (TipoEvidenciaID)
		REFERENCES MDS.TipoEvidencia (TipoEvidenciaID)
	,CONSTRAINT FK_RtfTesteEvidencia_Usuario
		FOREIGN KEY (UsuarioID)
		REFERENCES MDS.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_SolicitacaoRTFEvidencia_RtfTesteID			ON MDS.RtfTesteEvidencia (RtfTesteID)
CREATE INDEX IDX_SolicitacaoRTFEvidencia_ArquivoID			ON MDS.RtfTesteEvidencia (ArquivoID)
CREATE INDEX IDX_SolicitacaoRTFEvidencia_TipoEvidenciaID	ON MDS.RtfTesteEvidencia (TipoEvidenciaID)
CREATE INDEX IDX_SolicitacaoRTFEvidencia_UsuarioID			ON MDS.RtfTesteEvidencia (UsuarioID)
GO

IF OBJECT_ID('MDS.RtfTesteEvidenciaHistorico') IS NULL
CREATE TABLE MDS.RtfTesteEvidenciaHistorico (
	 RtfTesteEvidenciaHistoricoID		INT NOT NULL IDENTITY(1, 1)
	,RtfTesteEvidenciaID				INT NOT NULL
	,RtfTesteID							INT 
	,TipoEvidenciaID					INT 
	,ArquivoID							INT 
	,Descricao							VARCHAR(MAX)
	,Ordem								INT
	,DataAtualizacao					DATETIME
	,UsuarioID							INT
	,CONSTRAINT PK_RtfTesteEvidenciaHistorico PRIMARY KEY (RtfTesteEvidenciaHistoricoID)
	,CONSTRAINT FK_RtfTesteEvidenciaHistorico_Arquivo
		FOREIGN KEY (ArquivoID)
		REFERENCES MDS.Arquivo (ArquivoID)
		ON DELETE CASCADE
)
GO
CREATE INDEX IDX_RtfTesteEvidenciaHistorico_RtfTesteEvidenciaID ON MDS.RtfTesteEvidenciaHistorico (RtfTesteEvidenciaID)
CREATE INDEX IDX_RtfTesteEvidenciaHistorico_RtfTesteID			ON MDS.RtfTesteEvidenciaHistorico (RtfTesteID)
CREATE INDEX IDX_RtfTesteEvidenciaHistorico_ArquivoID			ON MDS.RtfTesteEvidenciaHistorico (ArquivoID)
CREATE INDEX IDX_RtfTesteEvidenciaHistorico_TipoEvidenciaID		ON MDS.RtfTesteEvidenciaHistorico (TipoEvidenciaID)
CREATE INDEX IDX_RtfTesteEvidenciaHistorico_UsuarioID			ON MDS.RtfTesteEvidenciaHistorico (UsuarioID)
GO

--============================================================================================================================

-- CHECKLIST ========================================

IF OBJECT_ID('MDS.CheckList') IS NULL
CREATE TABLE MDS.CheckList (
	 CheckListID			INT NOT NULL IDENTITY(1, 1)
	,Nome					VARCHAR(100) NOT NULL
	,Descricao				VARCHAR(MAX) NULL
	,DataCriacao			DATETIME NOT NULL
	,UsuarioCriacaoID		INT NOT NULL
	,DataAtualizacao		DATETIME NOT NULL
	,UsuarioAtualizacaoID	INT NOT NULL
	,CONSTRAINT PK_CheckList PRIMARY KEY (CheckListID)
	,CONSTRAINT FK_CheckList_UsuarioCriacaoID
		FOREIGN KEY (UsuarioCriacaoID)
		REFERENCES MDS.Usuario (UsuarioID)
	,CONSTRAINT FK_CheckList_UsuarioAtualizacaoID
		FOREIGN KEY (UsuarioAtualizacaoID)
		REFERENCES MDS.Usuario (UsuarioID)
)
GO
CREATE INDEX IDX_CheckList_UsuarioCriacaoID ON MDS.CheckList (UsuarioCriacaoID)
GO
CREATE INDEX IDX_CheckList_UsuarioAtualizacaoID ON MDS.CheckList (UsuarioAtualizacaoID)
GO

IF OBJECT_ID('MDS.CheckListHistorico') IS NULL
CREATE TABLE MDS.CheckListHistorico (
	 CheckListHistoricoID	INT NOT NULL IDENTITY(1, 1)
	,CheckListID			INT NOT NULL
	,Nome					VARCHAR(100) NOT NULL
	,Descricao				VARCHAR(MAX) NULL
	,DataCriacao			DATETIME NOT NULL
	,UsuarioCriacaoID		INT NOT NULL
	,DataAtualizacao		DATETIME NOT NULL
	,UsuarioAtualizacaoID	INT NOT NULL
	,CONSTRAINT PK_CheckListHistorico PRIMARY KEY (CheckListHistoricoID)
)
GO
CREATE INDEX IDX_CheckListHistorico_UsuarioCriacaoID ON MDS.CheckListHistorico (UsuarioCriacaoID)
CREATE INDEX IDX_CheckListHistorico_UsuarioAtualizacaoID ON MDS.CheckListHistorico (UsuarioAtualizacaoID)
GO

IF OBJECT_ID('MDS.CheckListGrupoItem') IS NULL
CREATE TABLE MDS.CheckListGrupoItem (
	 CheckListGrupoItemID	INT NOT NULL IDENTITY(1, 1)
	,CheckListID			INT NOT NULL
	,Nome					VARCHAR(1000) NOT NULL
	,Descricao				VARCHAR(MAX) NULL
	,CONSTRAINT PK_CheckListGrupoItem PRIMARY KEY (CheckListGrupoItemID)
	,CONSTRAINT FK_CheckListGrupoItem_CheckList 
		FOREIGN KEY (CheckListID) 
		REFERENCES MDS.CheckList (CheckListID)
)
GO
CREATE INDEX IDX_CheckListGrupoItem_CheckList ON MDS.CheckListGrupoItem (CheckListID)
GO

IF OBJECT_ID('MDS.CheckListGrupoItemHistorico') IS NULL
CREATE TABLE MDS.CheckListGrupoItemHistorico (
	 CheckListGrupoItemHistoricoID	INT NOT NULL IDENTITY(1, 1)
	,CheckListGrupoItemID			INT NOT NULL
	,CheckListID					INT NOT NULL
	,Nome							VARCHAR(1000) NOT NULL
	,Descricao						VARCHAR(MAX) NULL
	,CONSTRAINT PK_CheckListGrupoItemHistorico PRIMARY KEY (CheckListGrupoItemHistoricoID)
)
GO
CREATE INDEX IDX_CheckListGrupoItemHistorico_CheckListGrupoItem ON MDS.CheckListGrupoItemHistorico (CheckListGrupoItemID)
CREATE INDEX IDX_CheckListGrupoItemHistorico_CheckList ON MDS.CheckListGrupoItemHistorico (CheckListID)
GO

IF OBJECT_ID('MDS.CheckListItem') IS NULL
CREATE TABLE MDS.CheckListItem (
	 CheckListItemID		INT NOT NULL IDENTITY(1, 1)
	,CheckListGrupoItemID	INT NOT NULL
	,Nome					VARCHAR(1000)	NOT NULL
	,Descricao				VARCHAR(MAX)	NULL
	,CONSTRAINT PK_CheckListItem PRIMARY KEY (CheckListItemID)
	,CONSTRAINT FK_CheckListItem_CheckListGrupoItem
		FOREIGN KEY (CheckListGrupoItemID)
		REFERENCES MDS.CheckListGrupoItem (CheckListGrupoItemID)
)
GO
CREATE INDEX IDX_CheckListItem_CheckListGrupoItem ON MDS.CheckListItem (CheckListGrupoItemID)
GO

IF OBJECT_ID('MDS.CheckListItemHistorico') IS NULL
CREATE TABLE MDS.CheckListItemHistorico (
	 CheckListItemHistoricoID	INT NOT NULL IDENTITY(1, 1)
	,CheckListItemID			INT NOT NULL
	,CheckListGrupoItemID		INT NOT NULL
	,Nome						VARCHAR(1000)  NULL
	,Descricao					VARCHAR(MAX) NULL
	,CONSTRAINT PK_CheckListItemHistorico PRIMARY KEY (CheckListItemHistoricoID)
)
GO
CREATE INDEX IDX_CheckListItemHistorico_CheckListGrupoItem ON MDS.CheckListItemHistorico (CheckListGrupoItemID)
CREATE INDEX IDX_CheckListItemHistorico_CheckListItem ON MDS.CheckListItemHistorico (CheckListItemID)
GO

IF OBJECT_ID('MDS.CheckListItemResposta') IS NULL
CREATE TABLE MDS.CheckListItemResposta (
	 CheckListItemRespostaID	INT NOT NULL IDENTITY(1, 1)
	,CheckListItemID			INT NOT NULL
	,Sim						BIT NOT NULL CONSTRAINT DF_CheckListItemResposta_Sim			DEFAULT(0)
	,Nao						BIT NOT NULL CONSTRAINT DF_CheckListItemResposta_Nao			DEFAULT(0)
	,NaoAplicavel				BIT NOT NULL CONSTRAINT DF_CheckListItemResposta_NaoAplicavel	DEFAULT(0)
	,Observacao					VARCHAR(MAX) NULL
	,CONSTRAINT PK_ChecklistItemResposta PRIMARY KEY (CheckListItemRespostaID)
	,CONSTRAINT FK_CheckListItemResposta_CheckListItem 
		FOREIGN KEY (CheckListItemID)
		REFERENCES MDS.CheckListItem (CheckListItemID)
)
GO
CREATE INDEX IDX_CheckListItemResposta_CheckListItem ON MDS.CheckListItemResposta (CheckListItemID)
GO

IF OBJECT_ID('MDS.CheckListItemRespostaHistorico') IS NULL
CREATE TABLE MDS.CheckListItemRespostaHistorico (
	 CheckListItemRespostaHistoricoID	INT NOT NULL IDENTITY(1, 1)
	,CheckListItemRespostaID			INT NOT NULL 
	,CheckListItemID					INT NOT NULL
	,Sim								BIT NULL
	,Nao								BIT NULL
	,NaoAplicavel						BIT NULL
	,Observacao							VARCHAR(MAX) NULL
	,CONSTRAINT PK_ChecklistItemRespostaHistorico PRIMARY KEY (CheckListItemRespostaHistoricoID)
)
GO
CREATE INDEX IDX_CheckListItemRespostaHistorico_CheckListItem ON MDS.CheckListItemRespostaHistorico (CheckListItemID)
GO

IF OBJECT_ID('MDS.CheckListSolicitacao') IS NULL
CREATE TABLE MDS.CheckListSolicitacao (
	 CheckListSolicitacaoID	INT NOT NULL IDENTITY(1, 1)
	,CheckListID			INT NOT NULL
	,SolicitacaoID			INT NOT NULL
	,UsuarioID				INT NOT NULL
	,UsuarioVerificacaoID	INT NOT NULL
	,DataCriacao			DATETIME NOT NULL
	,DataAtualizacao		DATETIME NOT NULL
	,UsuarioAtualizacaoID	INT NOT NULL
	,CONSTRAINT PK_CheckListSolicitacao PRIMARY KEY (CheckListSolicitacaoID)
	,CONSTRAINT FK_CheckListSolicitacao_CheckList
		FOREIGN KEY (CheckListID)
		REFERENCES MDS.CheckList (CheckListID)
	,CONSTRAINT FK_CheckListSolicitacao_Usuario
		FOREIGN KEY (UsuarioID)
		REFERENCES MDS.Usuario (UsuarioID)
	,CONSTRAINT FK_CheckListSolicitacao_UsuarioVerificacao
		FOREIGN KEY (UsuarioVerificacaoID)
		REFERENCES MDS.Usuario (UsuarioID)
	,CONSTRAINT FK_CheckListSolicitacao_UsuarioAtualizacao
		FOREIGN KEY (UsuarioAtualizacaoID)
		REFERENCES MDS.Usuario (UsuarioID)
);
GO
CREATE INDEX IDX_CheckListSolicitacao_CheckList ON MDS.CheckListSolicitacao (CheckListID)
CREATE INDEX IDX_CheckListSolicitacao_Usuario ON MDS.CheckListSolicitacao (UsuarioID)
CREATE INDEX IDX_CheckListSolicitacao_UsuarioVerificacao ON MDS.CheckListSolicitacao (UsuarioVerificacaoID)
CREATE INDEX IDX_CheckListSolicitacao_UsuarioAtualizacao ON MDS.CheckListSolicitacao (UsuarioAtualizacaoID)
GO

IF OBJECT_ID('MDS.CheckListSolicitacaoHistorico') IS NULL
CREATE TABLE MDS.CheckListSolicitacaoHistorico (
	 CheckListSolicitacaoHistoricoID	INT NOT NULL IDENTITY(1, 1)
	,CheckListSolicitacaoID				INT NOT NULL
	,CheckListID						INT NOT NULL
	,SolicitacaoID						INT NOT NULL
	,UsuarioID							INT NOT NULL
	,UsuarioVerificacaoID				INT NOT NULL
	,DataCriacao						DATETIME NOT NULL
	,DataAtualizacao					DATETIME NOT NULL
	,UsuarioAtualizacaoID				INT NOT NULL
	,CONSTRAINT PK_CheckListSolicitacaoHistorico PRIMARY KEY (CheckListSolicitacaoHistoricoID)
);
GO

--============================================================================================================================
print 'DONE 1'
RETURN







































CREATE TABLE MDS.TipoDocumento (
	 TipoDocumentoID	INT NOT NULL IDENTITY(1, 1)
	,Nome				VARCHAR(100)
)

CREATE TABLE MDS.DocumentoTemplate (
	 DocumentoTemplateID	INT NOT NULL IDENTITY(1, 1)
	,TipoDocumentoID		INT NOT NULL
	,Nome					VARCHAR(200) NOT NULL
	,Descricao				VARCHAR(1000) NULL
	,Template				VARCHAR(MAX) NOT NULL
)

IF OBJECT_ID('MDS.SolicitacaoPropostaComercial') IS NULL
CREATE TABLE MDS.SolicitacaoPropostaComercial (
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

IF OBJECT_ID('MDS.SolicitacaoHistoricoPropostaComercial') IS NULL
CREATE TABLE MDS.SolicitacaoHistoricoPropostaComercial (
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

IF OBJECT_ID('MDS.SolictiacaoEstimativa') IS NULL
CREATE TABLE MDS.SolictiacaoEstimativa (
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
);