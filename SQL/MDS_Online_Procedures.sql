USE BDMdsOnline
GO

IF OBJECT_ID('usp_GravarHistoricoSolicitacaoRTU') IS NOT NULL DROP PROC usp_GravarHistoricoSolicitacaoRTU
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC dbo.usp_GravarHistoricoSolicitacaoRTU (@SolicitacaoRTUID INT)
AS
BEGIN
	
	INSERT INTO dbo.SolicitacaoRTUHistorico (
		 SolicitacaoRTUID
		,SolicitacaoID
		,Sequencia
		,Condicao
		,DadosEntrada
		,ResultadoEsperado
		,StatusVerificacaoTesteUnitarioID
		,ComoTestar
		,Observacoes
		,Ordem
		,DataAtualizacao
	)
	SELECT
		 SolicitacaoRTUID
		,SolicitacaoID
		,Sequencia
		,Condicao
		,DadosEntrada
		,ResultadoEsperado
		,StatusVerificacaoTesteUnitarioID
		,ComoTestar
		,Observacoes
		,Ordem
		,DataAtualizacao
	FROM dbo.SolicitacaoRTU
	WHERE SolicitacaoRTUID = @SolicitacaoRTUID

END
GO

--==============================================================================================

USE BDMdsOnline
GO

IF OBJECT_ID('usp_GravarHistoricoSolicitacaoRTF') IS NOT NULL DROP PROC usp_GravarHistoricoSolicitacaoRTF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC dbo.usp_GravarHistoricoSolicitacaoRTF (@SolicitacaoRTFID INT)
AS
BEGIN
	
	INSERT INTO dbo.SolicitacaoRTFHistorico (
		 SolicitacaoRTFID 
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
		,DataAtualizacao				
	)
	SELECT
		 SolicitacaoRTFID 
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
		,DataAtualizacao
	FROM dbo.SolicitacaoRTF
	WHERE SolicitacaoRTFID = @SolicitacaoRTFID

END
GO

--==============================================================================================

USE BDMdsOnline
GO

IF OBJECT_ID('usp_GravarHistoricoSolicitacaoRTFEvidencia') IS NOT NULL DROP PROC usp_GravarHistoricoSolicitacaoRTFEvidencia
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--========================================================
-- Author: LNDR
-- Date	 : 28/01/2018
--========================================================
CREATE PROC dbo.usp_GravarHistoricoSolicitacaoRTFEvidencia (@SolicitacaoRTFEvidenciaID INT)
AS
BEGIN
	
	INSERT INTO dbo.SolicitacaoRTFEvidenciaHistorico (
		  SolicitacaoRTFEvidenciaID
		 ,SolicitacaoRTFID			
		 ,TipoEvidenciaID							
		 ,ArquivoID									
		 ,Descricao									
		 ,Ordem										
		 ,DataAtualizacao
	)
	SELECT
		  SolicitacaoRTFEvidenciaID
		 ,SolicitacaoRTFID			
		 ,TipoEvidenciaID							
		 ,ArquivoID									
		 ,Descricao									
		 ,Ordem										
		 ,DataAtualizacao
	FROM dbo.SolicitacaoRTFEvidencia
	WHERE SolicitacaoRTFEvidenciaID = @SolicitacaoRTFEvidenciaID

END
GO