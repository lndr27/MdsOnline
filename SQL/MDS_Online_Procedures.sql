USE BDMdsOnline
GO


--========================================================================================================
IF OBJECT_ID('dbo.GravarHistoricoAlteracaoDocumento') IS NOT NULL			DROP PROC dbo.GravarHistoricoAlteracaoDocumento
GO
IF OBJECT_ID('dbo.GravarHistoricoAlteracaoDocumentacaoSolicitacao') IS NOT NULL DROP PROC dbo.GravarHistoricoAlteracaoDocumentacaoSolicitacao
GO
--========================================================================================================



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC dbo.GravarHistoricoAlteracaoDocumento (@DocumentoID INT)
AS
BEGIN
	
	-- dbo.DocumentoHistorico
	--========================================================================================================
	INSERT INTO dbo.DocumentoHistorico (
			DocumentoID, Nome, Descricao, TipoDocumentoID, DataCriacao, DataAtualizacao, UsuarioAtualizacaoID)
	SELECT	DocumentoID, Nome, Descricao, TipoDocumentoID, DataCriacao, DataAtualizacao, UsuarioAtualizacaoID
	FROM dbo.Documento
	WHERE DocumentoID = @DocumentoID

	-- dbo.DocumentoGrupoItemCheckListHistorico
	--========================================================================================================
	INSERT INTO dbo.DocumentoGrupoItemCheckListHistorico (
			DocumentoGrupoItemCheckListID, DocumentoID, Nome)
	SELECT	DocumentoGrupoItemCheckListID, DocumentoID, Nome
	FROM dbo.DocumentoGrupoItemCheckList
	WHERE DocumentoID = @DocumentoID

	-- dbo.DocumentoItemCheckListHistorico
	--========================================================================================================
	INSERT INTO dbo.DocumentoItemCheckListHistorico (
			DocumentoItemCheckListID, DocumentoGrupoItemCheckListID, Texto)
	SELECT	DICL.DocumentoItemCheckListID, DICL.DocumentoGrupoItemCheckListID, DICL.Texto
	FROM dbo.DocumentoItemCheckList DICL
	JOIN dbo.DocumentoGrupoItemCheckList DGICL
		ON DGICL.DocumentoGrupoItemCheckListID = DICL.DocumentoGrupoItemCheckListID
	WHERE DGICL.DocumentoID = @DocumentoID

	-- dbo.DocumentoTopicoHistorico
	--========================================================================================================
	INSERT INTO dbo.DocumentoTopicoHistorico (
			DocumentoTopicoID, DocumentoID, Nome)
	SELECT	DocumentoTopicoID, DocumentoID, Nome
	FROM dbo.DocumentoTopico
	WHERE DocumentoID = @DocumentoID

	--========================================================================================================
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC dbo.GravarHistoricoAlteracaoDocumentacaoSolicitacao (@DocumentoID INT, @SolicitacaoID INT)
AS
BEGIN

	-- dbo.DocumentoSolicitacaoTopicoConteudoHistorico
	--========================================================================================================
	INSERT INTO dbo.DocumentoSolicitacaoTopicoConteudoHistorico (
			DocumentoSolicitacaoTopicoConteudoID, SolicitacaoID,    DocumentoTopicoID, Conteudo, DataAtualizacao, UsuarioAtualizacaoID)
	SELECT	DocumentoSolicitacaoTopicoConteudoID, SolicitacaoID, DT.DocumentoTopicoID, Conteudo, DataAtualizacao, UsuarioAtualizacaoID
	FROM dbo.DocumentoSolicitacaoTopicoConteudo DSTC
	JOIN dbo.DocumentoTopico DT ON DT.DocumentoTopicoID = DSTC.DocumentoTopicoID
	WHERE	SolicitacaoID = @SolicitacaoID
		AND DocumentoID	  = @DocumentoID

	-- dbo.DocumentoSolicitacaoItemCheckListRespostaHistorico
	--========================================================================================================
	INSERT INTO dbo.DocumentoSolicitacaoItemCheckListRespostaHistorico (
			DocumentoSolicitacaoItemCheckListRespostaID, SolicitacaoID,		DocumentoItemCheckListID, Sim, Nao, NaoAplicavel, DataAtualizacao, UsuarioAtualizacaoID)
	SELECT	DocumentoSolicitacaoItemCheckListRespostaID, SolicitacaoID, DIC.DocumentoItemCheckListID, Sim, Nao, NaoAplicavel, DataAtualizacao, UsuarioAtualizacaoID
	FROM dbo.DocumentoSolicitacaoItemCheckListResposta DSICR
	JOIN dbo.DocumentoItemCheckList DIC ON DIC.DocumentoItemCheckListID = DSICR.DocumentoItemCheckListID
	JOIN dbo.DocumentoGrupoItemCheckList DGIC ON DGIC.DocumentoGrupoItemCheckListID = DIC.DocumentoGrupoItemCheckListID
	WHERE	SolicitacaoID = @SolicitacaoID
		AND DocumentoID	  = @DocumentoID
	
END
GO