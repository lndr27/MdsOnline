USE BDMdsOnline
GO

IF OBJECT_ID('usp_GravarHistoricoRtu') IS NOT NULL DROP PROC usp_GravarHistoricoRtu
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC dbo.usp_GravarHistoricoRtu (@RtuID INT)
AS
BEGIN
	
	INSERT INTO dbo.RtuHistorico (
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
	FROM dbo.Rtu
	WHERE RtuID = @RtuID

END
GO

--==============================================================================================


IF OBJECT_ID('usp_GravarHistoricoRtuTeste') IS NOT NULL DROP PROC usp_GravarHistoricoRtuTeste
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================

CREATE PROC dbo.usp_GravarHistoricoRtuTeste (@RtuTesteID INT)
AS
BEGIN
	INSERT INTO dbo.RtuTesteHistorico (
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
	FROM dbo.RtuTeste
	WHERE RtuTesteID = @RtuTesteID
END
GO



--==============================================================================================

USE BDMdsOnline
GO

IF OBJECT_ID('usp_GravarHistoricoRtf') IS NOT NULL DROP PROC usp_GravarHistoricoRtf
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC dbo.usp_GravarHistoricoRtf (@RtfID INT)
AS
BEGIN
	
	INSERT INTO dbo.RtfHistorico (
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
	FROM dbo.Rtf
	WHERE @RtfID = @RtfID

END
GO

--==============================================================================================

USE BDMdsOnline
GO

IF OBJECT_ID('usp_GravarHistoricoRtfTeste') IS NOT NULL DROP PROC usp_GravarHistoricoRtfTeste
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC dbo.usp_GravarHistoricoRtfTeste (@RtfTesteID INT)
AS
BEGIN
	
	INSERT INTO dbo.RtfTesteHistorico (
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
	FROM dbo.RtfTeste
	WHERE RtfTesteID = @RtfTesteID

END
GO

--==============================================================================================

USE BDMdsOnline
GO

IF OBJECT_ID('usp_GravarHistoricoTesteEvidencia') IS NOT NULL DROP PROC usp_GravarHistoricoTesteEvidencia
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC dbo.usp_GravarHistoricoTesteEvidencia (@RtfTesteEvidenciaID INT)
AS
BEGIN
	
	INSERT INTO dbo.RtfTesteEvidenciaHistorico (
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
	FROM dbo.RtfTesteEvidencia
	WHERE RtfTesteEvidenciaID = @RtfTesteEvidenciaID

END
GO