--CREATE DATABASE BDMdsOnline
--GO
USE BDMdsOnline
GO

/*
IF OBJECT_ID('dbo.DocumentoHistorico') IS NOT NULL									DROP TABLE dbo.DocumentoHistorico
IF OBJECT_ID('dbo.DocumentoGrupoItemCheckListHistorico') IS NOT NULL				DROP TABLE dbo.DocumentoGrupoItemCheckListHistorico
IF OBJECT_ID('dbo.DocumentoItemCheckListHistorico') IS NOT NULL						DROP TABLE dbo.DocumentoItemCheckListHistorico
IF OBJECT_ID('dbo.DocumentoSolicitacaoTopicoConteudoHistorico') IS NOT NULL			DROP TABLE dbo.DocumentoSolicitacaoTopicoConteudoHistorico
IF OBJECT_ID('dbo.DocumentoSolicitacaoItemCheckListRespostaHistorico') IS NOT NULL	DROP TABLE dbo.DocumentoSolicitacaoItemCheckListRespostaHistorico
IF OBJECT_ID('dbo.DocumentoTopicoHistorico') IS NOT NULL							DROP TABLE dbo.DocumentoTopicoHistorico
IF OBJECT_ID('dbo.DocumentoSolicitacaoTopicoConteudo') IS NOT NULL					DROP TABLE dbo.DocumentoSolicitacaoTopicoConteudo
IF OBJECT_ID('dbo.DocumentoTopico') IS NOT NULL										DROP TABLE dbo.DocumentoTopico
IF OBJECT_ID('dbo.DocumentoSolicitacaoImagem') IS NOT NULL							DROP TABLE dbo.DocumentoSolicitacaoImagem
IF OBJECT_ID('dbo.DocumentoSolicitacaoItemCheckListResposta') IS NOT NULL			DROP TABLE dbo.DocumentoSolicitacaoItemCheckListResposta
IF OBJECT_ID('dbo.DocumentoItemCheckList') IS NOT NULL								DROP TABLE dbo.DocumentoItemCheckList
IF OBJECT_ID('dbo.DocumentoGrupoItemCheckList') IS NOT NULL							DROP TABLE dbo.DocumentoGrupoItemCheckList
IF OBJECT_ID('dbo.DocumentoSolicitacao') IS NOT NULL								DROP TABLE dbo.DocumentoSolicitacao
IF OBJECT_ID('dbo.Documento') IS NOT NULL											DROP TABLE dbo.Documento
IF OBJECT_ID('dbo.TipoDocumento') IS NOT NULL										DROP TABLE dbo.TipoDocumento
*/



--===============================================================================================
-- TipoDocumento
--===============================================================================================
CREATE TABLE dbo.TipoDocumento (
	 TipoDocumentoID	INT NOT NULL IDENTITY(1, 1)
	,Nome				VARCHAR(200) NOT NULL
	,CONSTRAINT PK_TipoDocumento PRIMARY KEY (TipoDocumentoID)
)
GO
INSERT INTO dbo.TipoDocumento
VALUES ('Texto'), ('Checklist')
GO




--===============================================================================================
-- DOCUMENTO / DOCUMENTO HISTORICO
--===============================================================================================
CREATE TABLE dbo.Documento (
	 DocumentoID			INT NOT NULL IDENTITY(1, 1)
	,Nome					VARCHAR(200) NOT NULL
	,Descricao				VARCHAR(8000) NOT NULL
	,TipoDocumentoID		INT NOT NULL
	,DataCriacao			DATETIME NOT NULL
	,DataAtualizacao		DATETIME NOT NULL
	,UsuarioAtualizacaoID	INT NOT NULL
	,CONSTRAINT PK_Documento PRIMARY KEY (DocumentoID)
	,CONSTRAINT FK_Documento_TipoDocumento FOREIGN KEY (TipoDocumentoID) REFERENCES dbo.TipoDocumento (TipoDocumentoID)
)
GO
CREATE INDEX IDX_Documento_TipoDocumento ON dbo.Documento (TipoDocumentoID)
GO
--===============================================================================================
CREATE TABLE dbo.DocumentoHistorico (
	 DocumentoHistoricoID	INT NOT NULL IDENTITY(1, 1)
	,DocumentoID			INT NOT NULL
	,Nome					VARCHAR(200) NOT NULL
	,Descricao				VARCHAR(8000) NOT NULL
	,TipoDocumentoID		INT NOT NULL
	,DataCriacao			DATETIME NOT NULL
	,DataAtualizacao		DATETIME NOT NULL
	,UsuarioAtualizacaoID	INT NOT NULL
	,CONSTRAINT PK_DocumentoHistorico PRIMARY KEY (DocumentoHistoricoID)
	,CONSTRAINT FK_DocumentoHistorico_Documento FOREIGN KEY (DocumentoID) REFERENCES dbo.Documento (DocumentoID)
)
GO
CREATE INDEX IDX_DocumentoHistorico_Documento ON dbo.DocumentoHistorico (DocumentoID)
GO




--===============================================================================================
-- Documento Itens
--===============================================================================================
CREATE TABLE dbo.DocumentoTopico (
  	 DocumentoTopicoID	INT NOT NULL IDENTITY(1, 1)
	,DocumentoID		INT NOT NULL
	,Nome				VARCHAR(8000) NOT NULL
	,CONSTRAINT PK_DocumentoTopico PRIMARY KEY (DocumentoTopicoID)
	,CONSTRAINT FK_DocumentoTopico_Documento FOREIGN KEY (DocumentoID) REFERENCES dbo.Documento (DocumentoID)
)
GO
CREATE INDEX IDX_DocumentoTopico_Documento on dbo.DocumentoTopico (DocumentoID)
GO
--===============================================================================================
CREATE TABLE dbo.DocumentoTopicoHistorico (
	 DocumentoTopicoHistoricoID INT NOT NULL IDENTITY(1, 1)
  	,DocumentoTopicoID	INT NOT NULL
	,DocumentoID		INT NOT NULL
	,Nome				VARCHAR(8000) NOT NULL
	,CONSTRAINT PK_DocumentoTopicoHistorico PRIMARY KEY (DocumentoTopicoHistoricoID)
	,CONSTRAINT FK_DocumentoTopicoHistorico_DocumentoTopico FOREIGN KEY (DocumentoTopicoID) REFERENCES dbo.DocumentoTopico (DocumentoTopicoID)
)
GO
CREATE INDEX IDX_DocumentoTopicoHistorico_DocumentoTopico on dbo.DocumentoTopicoHistorico (DocumentoTopicoID)
GO
--===============================================================================================
CREATE TABLE dbo.DocumentoSolicitacaoTopicoConteudo (
	 DocumentoSolicitacaoTopicoConteudoID	INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID							INT NOT NULL
	,DocumentoTopicoID						INT NOT NULL
	,Conteudo								VARCHAR(MAX) NOT NULL
	,DataAtualizacao						DATETIME NOT NULL
	,UsuarioAtualizacaoID					INT NOT NULL
	,CONSTRAINT PK_DocumentoSolicitacaoTopicoConteudo PRIMARY KEY (DocumentoSolicitacaoTopicoConteudoID)
	,CONSTRAINT FK_DocumentoSolicitacaoTopicoConteudo_DocumentoTopico FOREIGN KEY (DocumentoTopicoID) REFERENCES dbo.DocumentoTopico (DocumentoTopicoID)
)
GO
CREATE INDEX IDX_DocumentoSolicitacaoTopicoConteudo_DocumentoTopico ON dbo.DocumentoSolicitacaoTopicoConteudo (DocumentoTopicoID)
GO
--===============================================================================================
CREATE TABLE dbo.DocumentoSolicitacaoTopicoConteudoHistorico (
	 DocumentoSolicitacaoTopicoConteudoHistoricoID	INT NOT NULL IDENTITY(1, 1)
	,DocumentoSolicitacaoTopicoConteudoID			INT NOT NULL
	,SolicitacaoID									INT NOT NULL
	,DocumentoTopicoID								INT NOT NULL
	,Conteudo										VARCHAR(MAX) NOT NULL
	,DataAtualizacao								DATETIME NOT NULL
	,UsuarioAtualizacaoID							INT NOT NULL
	,CONSTRAINT PK_DocumentoSolicitacaoTopicoConteudoHistorico PRIMARY KEY (DocumentoSolicitacaoTopicoConteudoHistoricoID)
	,CONSTRAINT FK_DocumentoSolicitacaoTopicoConteudoHistorico_DocumentoSolicitacaoTopicoConteudo FOREIGN KEY (DocumentoSolicitacaoTopicoConteudoID) REFERENCES dbo.DocumentoSolicitacaoTopicoConteudo (DocumentoSolicitacaoTopicoConteudoID)
)
GO
CREATE INDEX IDX_DocumentoSolicitacaoTopicoConteudoHistorico_DocumentoSolicitacaoTopicoConteudo ON dbo.DocumentoSolicitacaoTopicoConteudoHistorico (DocumentoSolicitacaoTopicoConteudoID)
GO




--===============================================================================================
-- DocumentoSolicitacaoImagem
--===============================================================================================
CREATE TABLE dbo.DocumentoSolicitacaoImagem (
	 DocumentoSolicitacaoImagemID	INT NOT NULL IDENTITY(1, 1)
	,DocumentoID					INT NOT NULL
	,SolicitacaoID					INT NOT NULL	
	,Nome							VARCHAR(500) NOT NULL
	,Extensao						VARCHAR(20) NOT NULL
	,TamanhoKB						FLOAT NOT NULL
	,Imagem							VARBINARY(MAX) NOT NULL
	,CONSTRAINT PK_DocumentoSolicitacaoImagem PRIMARY KEY (DocumentoSolicitacaoImagemID)
	,CONSTRAINT FK_DocumentoSolicitacaoImagem_Documento FOREIGN KEY (DocumentoID) REFERENCES dbo.Documento (DocumentoID)
)
GO
CREATE INDEX IDX_DocumentSolicitacaoImagem_Documento ON dbo.DocumentoSolicitacaoImagem (DocumentoID)
GO



--===============================================================================================
-- DocumentoGrupoItemCheckList / DocumentoGrupoItemCheckListHistorico
--===============================================================================================
CREATE TABLE dbo.DocumentoGrupoItemCheckList (
	 DocumentoGrupoItemCheckListID	INT NOT NULL IDENTITY(1, 1)
	,DocumentoID					INT NOT NULL
	,Nome							VARCHAR(8000) NOT NULL
	,CONSTRAINT PK_DocumentoGrupoItemCheckList PRIMARY KEY (DocumentoGrupoItemCheckListID)
	,CONSTRAINT FK_DocumentoGrupoItemCheckList_Documento FOREIGN KEY (DocumentoID) REFERENCES dbo.Documento (DocumentoID)
)
GO
CREATE INDEX IDX_DocumentoGrupoItemCheckList_Documento ON dbo.DocumentoGrupoItemCheckList (DocumentoID)
GO
--===============================================================================================
CREATE TABLE dbo.DocumentoGrupoItemCheckListHistorico (
	 DocumentoGrupoItemCheckListHistoricoID INT NOT NULL IDENTITY(1, 1)
	,DocumentoGrupoItemCheckListID			INT NOT NULL
	,DocumentoID							INT NOT NULL
	,Nome									VARCHAR(8000) NOT NULL
	,CONSTRAINT PK_DocumentoGrupoItemCheckListHistorico PRIMARY KEY (DocumentoGrupoItemCheckListHistoricoID)
	,CONSTRAINT FK_DocumentoGrupoItemCheckListHistorico_DocumentoGrupoItemCheckList FOREIGN KEY (DocumentoGrupoItemCheckListID) REFERENCES dbo.DocumentoGrupoItemCheckList (DocumentoGrupoItemCheckListID)
)
GO
CREATE INDEX IDX_DocumentoGrupoItemCheckListHistorico_DocumentoGrupoItemCheckList ON dbo.DocumentoGrupoItemCheckListHistorico (DocumentoGrupoItemCheckListID)
GO




--===============================================================================================
-- DocumentoItemCheckList / DocumentoItemCheckListHistorico
--===============================================================================================
CREATE TABLE dbo.DocumentoItemCheckList (
	 DocumentoItemCheckListID		INT NOT NULL IDENTITY(1, 1)
	,DocumentoGrupoItemCheckListID	INT NOT NULL
	,Texto							VARCHAR(MAX) NOT NULL
	,CONSTRAINT PK_DocumentoItemCheckList PRIMARY KEY (DocumentoItemCheckListID)
	,CONSTRAINT FK_DocumentoItemCheckList_DocumentoGrupoItemCheckList FOREIGN KEY (DocumentoGrupoItemCheckListID) REFERENCES dbo.DocumentoGrupoItemCheckList (DocumentoGrupoItemCheckListID)
)
GO
CREATE INDEX IDX_DocumentoItemCheckList_DocumentoGrupoItemCheckList ON dbo.DocumentoItemCheckList (DocumentoGrupoItemCheckListID)
GO
--===============================================================================================
CREATE TABLE dbo.DocumentoItemCheckListHistorico (
	 DocumentoItemCheckListHistoricoID	INT NOT NULL IDENTITY(1, 1)
	,DocumentoItemCheckListID			INT NOT NULL
	,DocumentoGrupoItemCheckListID		INT NOT NULL
	,Texto								VARCHAR(MAX)	
	,CONSTRAINT PK_DocumentoItemCheckListHistorico PRIMARY KEY (DocumentoItemCheckListHistoricoID)
	,CONSTRAINT FK_DocumentoItemCheckListHistorico_DocumentoItemCheckList FOREIGN KEY (DocumentoItemCheckListID) REFERENCES dbo.DocumentoItemCheckList (DocumentoItemCheckListID)
)
GO
CREATE INDEX IDX_DocumentoItemCheckListHistorico_DocumentoItemCheckList ON dbo.DocumentoItemCheckListHistorico (DocumentoItemCheckListID)
GO
--===============================================================================================
CREATE TABLE dbo.DocumentoSolicitacaoItemCheckListResposta (
	 DocumentoSolicitacaoItemCheckListRespostaID	INT NOT NULL IDENTITY(1, 1)
	,SolicitacaoID									INT NOT NULL
	,DocumentoItemCheckListID						INT NOT NULL
	,Sim											BIT NOT NULL CONSTRAINT DF_DocumentoSolicitacaoItemCheckListReposta_Sim DEFAULT(0)
	,Nao											BIT NOT NULL CONSTRAINT DF_DocumentoSolicitacaoItemCheckListReposta_Nao DEFAULT(0)
	,NaoAplicavel									BIT NOT NULL CONSTRAINT DF_DocumentoSolicitacaoItemCheckListReposta_NaoAplicavel DEFAULT(0)
	,DataAtualizacao								DATETIME NOT NULL
	,UsuarioAtualizacaoID							INT NOT NULL
	,CONSTRAINT PK_DocumentoSolicitacaoItemCheckListResposta PRIMARY KEY (DocumentoSolicitacaoItemCheckListRespostaID)
	,CONSTRAINT FK_DocumentoSolicitacaoItemCheckListResposta_DocumentoItemCheckList FOREIGN KEY (DocumentoItemCheckListID) REFERENCES dbo.DocumentoItemCheckList (DocumentoItemCheckListID)
)
GO
CREATE INDEX IDX_DocumentoSolicitacaoItemCheckListResposta_DocumentoItemCheckList ON dbo.DocumentoSolicitacaoItemCheckListResposta (DocumentoItemCheckListID)
GO
--===============================================================================================
CREATE TABLE dbo.DocumentoSolicitacaoItemCheckListRespostaHistorico (
	 DocumentoSolicitacaoItemCheckListRespostaHistoricoID	INT NOT NULL IDENTITY(1, 1)
	,DocumentoSolicitacaoItemCheckListRespostaID			INT NOT NULL
	,SolicitacaoID											INT NOT NULL
	,DocumentoItemCheckListID								INT NOT NULL
	,Sim													BIT NOT NULL
	,Nao													BIT NOT NULL
	,NaoAplicavel											BIT NOT NULL
	,DataAtualizacao										DATETIME NOT NULL
	,UsuarioAtualizacaoID									INT NOT NULL
	,CONSTRAINT PK_DocumentoSolicitacaoItemCheckListRespostaHistorico PRIMARY KEY (DocumentoSolicitacaoItemCheckListRespostaHistoricoID)
	,CONSTRAINT FK_DocumentoSolicitacaoItemCheckListRespostaHistorico_DocumentoSolicitacaoItemCheckListResposta FOREIGN KEY (DocumentoSolicitacaoItemCheckListRespostaID) REFERENCES dbo.DocumentoSolicitacaoItemCheckListResposta (DocumentoSolicitacaoItemCheckListRespostaID)
)
GO
CREATE INDEX IDX_DocumentoSolicitacaoItemCheckListRespostaHistorico_DocumentoSolicitacaoItemCheckListResposta ON dbo.DocumentoSolicitacaoItemCheckListRespostaHistorico (DocumentoSolicitacaoItemCheckListRespostaID)
GO






--===============================================================================================
-- DocumentoSolicitacao
--===============================================================================================
CREATE TABLE dbo.DocumentoSolicitacao (
	 DocumentoID	INT NOT NULL
	,SolicitacaoID	INT NOT NULL
	,CONSTRAINT PK_DocumentoSolicitacao PRIMARY KEY (DocumentoID, SolicitacaoID)
	,CONSTRAINT FK_DocumentoSolicitacao_Documento FOREIGN KEY (DocumentoID) REFERENCES dbo.Documento (DocumentoID)
)