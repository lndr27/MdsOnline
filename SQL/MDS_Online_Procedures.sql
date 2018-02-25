USE BDMdsOnline
GO

IF OBJECT_ID('usp_GravarHistoricoSolicitacaoRoteiroTesteUnitario') IS NOT NULL DROP PROC usp_GravarHistoricoSolicitacaoRoteiroTesteUnitario
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC dbo.usp_GravarHistoricoSolicitacaoRoteiroTesteUnitario (@SolicitacaoRoteiroTesteUnitarioID INT)
AS
BEGIN
	
	INSERT INTO dbo.SolicitacaoRoteiroTesteUnitarioHistorico (
		 SolicitacaoRoteiroTesteUnitarioID
		,SolicitacaoID
		,Sequencia
		,Condicao
		,DadosEntrada
		,ResultadoEsperado
		,StatusVerificacaoTesteUnitarioID
		,ComoTestar
		,Observacoes
		,Ordem
	)
	SELECT
		 SolicitacaoRoteiroTesteUnitarioID
		,SolicitacaoID
		,Sequencia
		,Condicao
		,DadosEntrada
		,ResultadoEsperado
		,StatusVerificacaoTesteUnitarioID
		,ComoTestar
		,Observacoes
		,Ordem
	FROM dbo.SolicitacaoRoteiroTesteUnitario
	WHERE SolicitacaoRoteiroTesteUnitarioID = @SolicitacaoRoteiroTesteUnitarioID

END
GO

--==============================================================================================

USE BDMdsOnline
GO

IF OBJECT_ID('usp_GravarHistoricoSolicitacaoRoteiroTesteFuncional') IS NOT NULL DROP PROC usp_GravarHistoricoSolicitacaoRoteiroTesteFuncional
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC dbo.usp_GravarHistoricoSolicitacaoRoteiroTesteFuncional (@SolicitacaoRoteiroTesteFuncionalID INT)
AS
BEGIN
	
	INSERT INTO dbo.SolicitacaoRoteiroTesteFuncionalHistorico (
		 SolicitacaoRoteiroTesteFuncionalID 
		,SolicitacaoID						
		,Sequencia							
		,Funcionalidade						
		,CondicaoCenario					
		,PreCondicao						
		,DadosEntrada						
		,ResultadoEsperado					
		,Observacoes						
		,StatusExecucaoHomologacaoID		
		,Ordem								
	)
	SELECT
		 SolicitacaoRoteiroTesteFuncionalID 
		,SolicitacaoID						
		,Sequencia							
		,Funcionalidade						
		,CondicaoCenario					
		,PreCondicao						
		,DadosEntrada						
		,ResultadoEsperado					
		,Observacoes						
		,StatusExecucaoHomologacaoID		
		,Ordem
	FROM dbo.SolicitacaoRoteiroTesteFuncional
	WHERE SolicitacaoRoteiroTesteFuncionalID = @SolicitacaoRoteiroTesteFuncionalID

END
GO