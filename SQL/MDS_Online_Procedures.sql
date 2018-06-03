USE BDMdsOnline
GO

--=======================================================================================
-- RTU
--=======================================================================================

IF OBJECT_ID('MDS.usp_GravarHistoricoRtu') IS NOT NULL DROP PROC MDS.usp_GravarHistoricoRtu
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC MDS.usp_GravarHistoricoRtu (@RtuID INT)
AS
BEGIN
	
	INSERT INTO MDS.RtuHistorico (
		 RtuID
		,SolicitacaoID
		,DataCriacao
		,DataAtualizacao
		,UsuarioID
		,UsuarioVerificacaoID
		,UsuarioAtualizacaoID
	)
	SELECT
		 RtuID
		,SolicitacaoID
		,DataCriacao
		,DataAtualizacao
		,UsuarioID
		,UsuarioVerificacaoID
		,UsuarioAtualizacaoID
	FROM MDS.Rtu
	WHERE RtuID = @RtuID

END
GO

--==============================================================================================


IF OBJECT_ID('MDS.usp_GravarHistoricoRtuTeste') IS NOT NULL DROP PROC MDS.usp_GravarHistoricoRtuTeste
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================

CREATE PROC MDS.usp_GravarHistoricoRtuTeste (@RtuTesteID INT)
AS
BEGIN
	INSERT INTO MDS.RtuTesteHistorico (
		 RtuTesteID
		,RtuID
		,Sequencia
		,Condicao
		,DadosEntrada
		,ResultadoEsperado
		,StatusVerificacaoTesteUnitarioID
		,ComoTestar
		,Observacoes
		,Ordem
		,DataAtualizacao
		,UsuarioID
	)
	SELECT 
		 RtuTesteID
		,RtuID
		,Sequencia
		,Condicao
		,DadosEntrada
		,ResultadoEsperado
		,StatusVerificacaoTesteUnitarioID
		,ComoTestar
		,Observacoes
		,Ordem
		,DataAtualizacao
		,UsuarioID
	FROM MDS.RtuTeste
	WHERE RtuTesteID = @RtuTesteID
END
GO



--==============================================================================================

USE BDMdsOnline
GO

--=======================================================================================
-- RTF
--=======================================================================================

IF OBJECT_ID('MDS.usp_GravarHistoricoRtf') IS NOT NULL DROP PROC MDS.usp_GravarHistoricoRtf
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC MDS.usp_GravarHistoricoRtf (@RtfID INT)
AS
BEGIN
	
	INSERT INTO MDS.RtfHistorico (
		 RtfID 
		,SolicitacaoID						
		,DataCriacao
		,DataAtualizacao
		,UsuarioID
		,UsuarioVerificacaoID
		,UsuarioAtualizacaoID
	)
	SELECT
		 RtfID 
		,SolicitacaoID						
		,DataCriacao
		,DataAtualizacao
		,UsuarioID
		,UsuarioVerificacaoID
		,UsuarioAtualizacaoID
	FROM MDS.Rtf
	WHERE @RtfID = @RtfID

END
GO

--==============================================================================================

USE BDMdsOnline
GO

IF OBJECT_ID('MDS.usp_GravarHistoricoRtfTeste') IS NOT NULL DROP PROC MDS.usp_GravarHistoricoRtfTeste
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC MDS.usp_GravarHistoricoRtfTeste (@RtfTesteID INT)
AS
BEGIN
	
	INSERT INTO MDS.RtfTesteHistorico (
		  RtfTesteID
		 ,RtfID
		 ,Sequencia
		 ,Funcionalidade
		 ,CondicaoCenario
		 ,PreCondicao
		 ,DadosEntrada
		 ,ResultadoEsperado
		 ,Observacoes
		 ,StatusExecucaoHomologacaoID
		 ,Ordem
		 ,DataAtualizacao
		 ,UsuarioID
	)
	SELECT
		  RtfTesteID
		 ,RtfID
		 ,Sequencia
		 ,Funcionalidade
		 ,CondicaoCenario
		 ,PreCondicao
		 ,DadosEntrada
		 ,ResultadoEsperado
		 ,Observacoes
		 ,StatusExecucaoHomologacaoID
		 ,Ordem
		 ,DataAtualizacao
		 ,UsuarioID
	FROM MDS.RtfTeste
	WHERE RtfTesteID = @RtfTesteID

END
GO

--==============================================================================================

USE BDMdsOnline
GO

IF OBJECT_ID('MDS.usp_GravarHistoricoTesteEvidencia') IS NOT NULL DROP PROC MDS.usp_GravarHistoricoTesteEvidencia
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC MDS.usp_GravarHistoricoTesteEvidencia (@RtfTesteEvidenciaID INT)
AS
BEGIN
	
	INSERT INTO MDS.RtfTesteEvidenciaHistorico (
		 RtfTesteEvidenciaID
		,RtfTesteID
		,TipoEvidenciaID
		,ArquivoID
		,Descricao
		,Ordem
		,DataAtualizacao
		,UsuarioID
	)
	SELECT
		  RtfTesteEvidenciaID
		,RtfTesteID
		,TipoEvidenciaID
		,ArquivoID
		,Descricao
		,Ordem
		,DataAtualizacao
		,UsuarioID
	FROM MDS.RtfTesteEvidencia
	WHERE RtfTesteEvidenciaID = @RtfTesteEvidenciaID

END
GO



--=======================================================================================
-- CHECKLIST
--=======================================================================================


IF OBJECT_ID('MDS.usp_GravarCheckListHistorico') IS NOT NULL DROP PROC MDS.usp_GravarCheckListHistorico
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC MDS.usp_GravarCheckListHistorico (@CheckListID INT)
AS
BEGIN
	INSERT INTO MDS.CheckListHistorico (CheckListID, Nome, Descricao, DataCriacao, UsuarioCriacaoID, DataAtualizacao, UsuarioAtualizacaoID)
	SELECT CheckListID, Nome, Descricao, DataCriacao, UsuarioCriacaoID, DataAtualizacao, UsuarioAtualizacaoID
	FROM MDS.CheckList
	WHERE CheckListID = @CheckListID
END
GO

--==============================================================================================


IF OBJECT_ID('MDS.usp_GravarCheckListGrupoItem') IS NOT NULL DROP PROC MDS.usp_GravarCheckListGrupoItem
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC MDS.usp_GravarCheckListGrupoItem (@CheckListGrupoItemID INT)
AS
BEGIN
	INSERT INTO MDS.CheckListGrupoItemHistorico (CheckListGrupoItemID, CheckListID, Nome, Descricao)
	SELECT CheckListGrupoItemID, CheckListID, Nome, Descricao
	FROM MDS.CheckListGrupoItem
	WHERE CheckListGrupoItemID = @CheckListGrupoItemID
END
GO

--==============================================================================================

IF OBJECT_ID('MDS.usp_GravarCheckListItemHistorico') IS NOT NULL DROP PROC MDS.usp_GravarCheckListItemHistorico
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC MDS.usp_GravarCheckListItemHistorico (@CheckListItemID INT)
AS
BEGIN
	INSERT INTO MDS.CheckListItemHistorico (CheckListItemID, CheckListGrupoItemID, Descricao, Nome)
	SELECT CheckListItemID, CheckListGrupoItemID, Descricao, Nome
	FROM MDS.CheckListItem
	WHERE CheckListItemID = @CheckListItemID
END
GO