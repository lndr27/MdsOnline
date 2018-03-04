using Lndr.MdsOnline.Helpers.Extensions;
using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Lndr.MdsOnline.Repositories
{
    public class MdsOnlineRepository : BaseRepository, IMdsOnlineRepository
    {
        #region Upload Arquivo +
        public void UploadArquivo(ArquivoDTO arquivo)
        {
            #region SQL +
            const string sql = @"
INSERT INTO dbo.Arquivo ([Guid], Nome, Extensao, ContentType, TamanhoKb, Arquivo, IsRascunho, DataUpload, UsuarioID)
VALUES (@Guid, @Nome, @Extensao, @ContentType, @TamanhoKb, @Arquivo, @IsRascunho, @DataUpload, @UsuarioID)";
            #endregion
            base.Repository.ExecuteScalar<string>(sql, p => {
                p.AddWithValue("@Guid", arquivo.Guid);
                p.AddWithValue("@Nome", arquivo.Nome);
                p.AddWithValue("@Extensao", arquivo.Extensao);
                p.AddWithValue("@ContentType", arquivo.ContentType);
                p.AddWithValue("@TamanhoKb", arquivo.TamanhoKb);
                p.AddWithValue("@Arquivo", arquivo.Arquivo);
                p.AddWithValue("@IsRascunho", arquivo.IsRascunho);
                p.AddWithValue("@DataUpload", arquivo.DataUpload);
                p.AddWithValue("@UsuarioID", arquivo.UsuarioID);
            });
        }

        public void RemoverArquivo(string guid)
        {
            const string sql = @"DELETE FROM dbo.Arquivo WHERE [Guid] = @Guid";
            base.Repository.ExecuteNonQuery(sql, p => p.AddWithValue("@Guid", guid));
        }

        public ArquivoDTO ObterArquivo(string guid)
        {
            const string sql = @"
SELECT TOP 1 [Guid], Nome, Extensao, ContentType, TamanhoKb, Arquivo, IsRascunho, DataUpload, UsuarioID
FROM dbo.Arquivo
WHERE [Guid] = @Guid";
            return base.Repository.FindOne<ArquivoDTO>(sql, p => p.AddWithValue("@Guid", guid));
        }
        #endregion

        #region RTU +
        public IEnumerable<SolicitacaoRTUDomain> ObterRTU(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
SELECT
     SolicitacaoRTUID
    ,SolicitacaoID
    ,Sequencia
    ,Condicao
    ,DadosEntrada
    ,ResultadoEsperado
    ,Verificacao
    ,ComoTestar
    ,Observacoes
    ,DataAtualizacao
    ,ISNULL(NULLIF(Ordem, 0), ROW_NUMBER() OVER (ORDER BY SolicitacaoRTUID)) Ordem
    ,UsuarioID
FROM dbo.SolicitacaoRTU
WHERE SolicitacaoID = @SolicitacaoID
ORDER BY Ordem";
            #endregion

            return base.Repository.FindAll<SolicitacaoRTUDomain>(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
            });
        }

        public void SalvarRTU(IEnumerable<SolicitacaoRTUDomain> rtu, int solicitacaoID)
        {
            if (rtu.IsNullOrEmpty()) return;

            this.ApagarTestesUnitariosNaoEncontrados(solicitacaoID, rtu.Where(r => r.SolicitacaoRTUID > 0).Select(r => r.SolicitacaoRTUID));

            var dataAtualizacao = DateTime.Now;

            foreach (var linha in rtu)
            {
                linha.DataAtualizacao = dataAtualizacao;
                this.InserirAtualizarRTU(linha, solicitacaoID);
            }
        }

        private void InserirAtualizarRTU(SolicitacaoRTUDomain rtu, int solicitacaoID)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO dbo.SolicitacaoRTU 
        (SolicitacaoID,  Sequencia,  Condicao,  DadosEntrada,  ResultadoEsperado,  Verificacao,  ComoTestar,  Observacoes,  Ordem,   DataAtualizacao,  UsuarioID)
VALUES  (@SolicitacaoID, @Sequencia, @Condicao, @DadosEntrada, @ResultadoEsperado, @Verificacao, @ComoTestar, @Observacoes, @Ordem, @DataAtualizacao, @UsuarioID)

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC dbo.usp_GravarHistoricoSolicitacaoRTU @ID";

            const string sqlAtualizar = @"
UPDATE dbo.SolicitacaoRTU SET
     Sequencia           = @Sequencia
    ,Condicao            = @Condicao 
    ,DadosEntrada        = @DadosEntrada 
    ,ResultadoEsperado   = @ResultadoEsperado
    ,Verificacao         = @Verificacao
    ,ComoTestar          = @ComoTestar 
    ,Observacoes         = @Observacoes
    ,Ordem               = @Ordem
    ,DataAtualizacao     = @DataAtualizacao
    ,UsuarioID           = @UsuarioID
WHERE SolicitacaoRTUID = @SolicitacaoRTUID

EXEC dbo.usp_GravarHistoricoSolicitacaoRTU @SolicitacaoRTUID";

            var sql = rtu.SolicitacaoRTUID > 0 ? sqlAtualizar : sqlInserir;
            #endregion
            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@SolicitacaoRTUID", rtu.SolicitacaoRTUID);
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
                p.AddWithValue("@Sequencia", rtu.Sequencia);
                p.AddWithValue("@Condicao", rtu.Condicao);
                p.AddWithValue("@DadosEntrada", rtu.DadosEntrada);
                p.AddWithValue("@ResultadoEsperado", rtu.ResultadoEsperado);
                p.AddWithValue("@Verificacao", rtu.StatusVerificacaoTesteUnitarioID);
                p.AddWithValue("@ComoTestar", rtu.ComoTestar);
                p.AddWithValue("@Observacoes", rtu.Observacoes);
                p.AddWithValue("@Ordem", rtu.Ordem);
                p.AddWithValue("@DataAtualizacao", rtu.DataAtualizacao);
                p.AddWithValue("@UsuarioID", rtu.UsuarioID);
            });
        }

        private void ApagarTestesUnitariosNaoEncontrados(int solicitacaoID, IEnumerable<int> testes)
        {
            if (testes.IsNullOrEmpty()) return;

            const string sql = @"
DELETE SRTU 
FROM dbo.SolicitacaoRTU SRTU
WHERE SRTU.SolicitacaoRTUID NOT IN (@Testes)
AND SRTU.SolicitacaoID = @SolicitacaoID";

            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
                p.AddWithValues("@Testes", testes);
            });
        }
        #endregion

        #region RTF +
        public IEnumerable<SolicitacaoRTFDomain> ObterTestesRTF(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
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
    ,DataAtualizacao
    ,ISNULL(NULLIF(Ordem, 0), ROW_NUMBER() OVER (ORDER BY SolicitacaoRTFID)) Ordem
    ,UsuarioID
FROM dbo.SolicitacaoRTF
WHERE SolicitacaoID = @SolicitacaoID
ORDER BY Ordem";
            #endregion
            return base.Repository.FindAll<SolicitacaoRTFDomain>(sql, p => p.AddWithValue("@SolicitacaoID", solicitacaoID));
        }

        public IEnumerable<SolicitacaoRTFEvidenciaDTO> ObterEvidenciasRTF(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
SELECT 
     SRTEE.SolicitacaoRTFEvidenciaID
    ,SRTEE.SolicitacaoRTFID
    ,SRTEE.TipoEvidenciaID
    ,CAST(A.[Guid] AS CHAR(36)) AS GuidImagem
    ,SRTEE.Descricao
    ,SRTEE.Ordem
    ,SRTEE.UsuarioID
FROM dbo.SolicitacaoRTFEvidencia SRTEE
JOIN dbo.SolicitacaoRTF SRTE 
    ON SRTE.SolicitacaoRTFID = SRTEE.SolicitacaoRTFID
JOIN dbo.Arquivo A 
    ON A.ArquivoID = SRTEE.ArquivoID
WHERE SRTE.SolicitacaoID = @SolicitacaoID
ORDER BY SRTEE.Ordem, SRTEE.SolicitacaoRTFEvidenciaID";
            #endregion
            return base.Repository.FindAll<SolicitacaoRTFEvidenciaDTO>(sql, p => p.AddWithValue("@SolicitacaoID", solicitacaoID));
        }

        public void SalvarRTF(IEnumerable<SolicitacaoRTFDTO> rtf, int solicitacaoID)
        {
            if (rtf.IsNullOrEmpty()) return;

            var evidencias = new List<SolicitacaoRTFEvidenciaDTO>();
            var dataAtualizacao = DateTime.Now;

            using (var tran = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                this.ApagarTestesUnitariosNaoEncontrados(solicitacaoID, rtf.Where(r => r.SolicitacaoRTFID > 0).Select(r => r.SolicitacaoRTFID));

                foreach (var linha in rtf)
                {
                    linha.DataAtualizacao = dataAtualizacao;
                    linha.SolicitacaoRTFID = this.InserirAtualizarRTF(linha, solicitacaoID);

                    if (!linha.Evidencias.IsNullOrEmpty())
                    {
                        linha.Evidencias.ToList().ForEach(e => 
                        {
                            e.SolicitacaoRTFID = linha.SolicitacaoRTFID;
                            e.DataAtualizacao = dataAtualizacao;
                        });
                        evidencias.AddRange(linha.Evidencias);
                    }
                    if (!linha.Erros.IsNullOrEmpty())
                    {
                        linha.Erros.ToList().ForEach(e => e.SolicitacaoRTFID = linha.SolicitacaoRTFID);
                        evidencias.AddRange(linha.Erros);
                    }
                }
                this.SalvarEvidencias(solicitacaoID, evidencias);

                tran.Complete();
            }
        }

        private void ApagarTestesFuncionaisNaoEncontrados(int solicitacaoID, IEnumerable<int> testes)
        {
            if (testes.IsNullOrEmpty()) return;

            const string sql = @"
DELETE SRTU 
FROM dbo.SolicitacaoRTF SRTF
WHERE SRTF.SolicitacaoRTFID NOT IN (@Testes)
AND SRTF.SolicitacaoID = @SolicitacaoID";

            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
                p.AddWithValues("@Testes", testes);
            });
        }

        private int InserirAtualizarRTF(SolicitacaoRTFDTO rtf, int solicitacaoID)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO dbo.SolicitacaoRTF 
        (SolicitacaoID,   Sequencia,  Funcionalidade,  CondicaoCenario,  PreCondicao,  DadosEntrada,  ResultadoEsperado,  Observacoes,  StatusExecucaoHomologacaoID,  Ordem,  DataAtualizacao,  UsuarioID)
VALUES  (@SolicitacaoID, @Sequencia, @Funcionalidade, @CondicaoCenario, @PreCondicao, @DadosEntrada, @ResultadoEsperado, @Observacoes, @StatusExecucaoHomologacaoID, @Ordem, @DataAtualizacao, @UsuarioID)

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC dbo.usp_GravarHistoricoSolicitacaoRTF @ID

SELECT @ID";

            const string sqlAtualizar = @"
UPDATE dbo.SolicitacaoRTF SET
         Sequencia					 = @Sequencia							
		,Funcionalidade				 = @Funcionalidade						
		,CondicaoCenario			 = @CondicaoCenario					
		,PreCondicao				 = @PreCondicao						
		,DadosEntrada				 = @DadosEntrada						
		,ResultadoEsperado			 = @ResultadoEsperado					
		,Observacoes				 = @Observacoes						
		,StatusExecucaoHomologacaoID = @StatusExecucaoHomologacaoID		
		,Ordem                       = @Ordem
        ,DataAtualizacao             = @DataAtualizacao
        ,UsuarioID                   = @UsuarioID
WHERE   SolicitacaoRTFID = @SolicitacaoRTFID

EXEC dbo.usp_GravarHistoricoSolicitacaoRTF @SolicitacaoRTFID

SELECT @SolicitacaoRTFID";

            var sql = rtf.SolicitacaoRTFID > 0 ? sqlAtualizar : sqlInserir;
            #endregion
            return base.Repository.ExecuteScalar<int>(sql, p =>
            {
                p.AddWithValue("@SolicitacaoRTFID", rtf.SolicitacaoRTFID);
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
                p.AddWithValue("@Sequencia", rtf.Sequencia);
                p.AddWithValue("@Funcionalidade", rtf.Funcionalidade);
                p.AddWithValue("@CondicaoCenario", rtf.CondicaoCenario);
                p.AddWithValue("@PreCondicao", rtf.PreCondicao);
                p.AddWithValue("@DadosEntrada", rtf.DadosEntrada);
                p.AddWithValue("@ResultadoEsperado", rtf.ResultadoEsperado);
                p.AddWithValue("@Observacoes", rtf.Observacoes);
                p.AddWithValue("@DataAtualizacao", rtf.DataAtualizacao);
                p.AddWithValue("@StatusExecucaoHomologacaoID", rtf.StatusExecucaoHomologacaoID);
                p.AddWithValue("@Ordem", rtf.Ordem);
                p.AddWithValue("@UsuarioID", rtf.UsuarioID);
            });
        }
        
        private void SalvarEvidencias(int solicitacaoID, IEnumerable<SolicitacaoRTFEvidenciaDTO> evidencias)
        {
            if (evidencias.IsNullOrEmpty()) return;

            this.ApagarEvidenciasNaoEncontradas(solicitacaoID, evidencias.Where(e => e.SolicitacaoRTFEvidenciaID > 0).Select(e => e.SolicitacaoRTFEvidenciaID));

            var index = 1;
            foreach(var evidencia in evidencias)
            {
                evidencia.Ordem = index++;
                this.InserirAtualizarEvidencia(evidencia);
            }
        }

        private void ApagarEvidenciasNaoEncontradas(int solicitacaoID, IEnumerable<int> evidencias)
        {
            if (evidencias.IsNullOrEmpty()) return;

            #region SQL +
            const string sql = @"
DECLARE @TEMP TABLE (ArquivoID INT, SolicitacaoRTFEvidenciaID INT)

INSERT INTO @TEMP (ArquivoID, SolicitacaoRTFEvidenciaID)
SELECT SRTFE.ArquivoID, SRTFE.SolicitacaoRTFEvidenciaID 
FROM dbo.SolicitacaoRTFEvidencia SRTFE
JOIN dbo.SolicitacaoRTF SRTF
    ON SRTF.SolicitacaoRTFID = SRTFE.SolicitacaoRTFID
WHERE   SRTF.SolicitacaoID = @SolicitacaoID
    AND SRTFE.SolicitacaoRTFEvidenciaID NOT IN (@Evidencias)

DELETE SRTFE
FROM dbo.SolicitacaoRTFEvidencia SRTFE
JOIN @TEMP T
    ON T.SolicitacaoRTFEvidenciaID = SRTFE.SolicitacaoRTFEvidenciaID

DELETE A
FROM dbo.Arquivo A
JOIN @TEMP T
    ON T.ArquivoID = A.ArquivoID";
            #endregion
            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
                p.AddWithValues("@Evidencias", evidencias);
            });
        }

        private void InserirAtualizarEvidencia(SolicitacaoRTFEvidenciaDTO evidencia)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO dbo.SolicitacaoRTFEvidencia 
        (SolicitacaoRTFID, TipoEvidenciaID, ArquivoID, Descricao, Ordem, DataAtualizacao, UsuarioID)
SELECT   @SolicitacaoRTFID, @TipoEvidenciaID, A.ArquivoID, @Descricao, @Ordem, @DataAtualizacao, @UsuarioID
FROM dbo.Arquivo A
WHERE [Guid] = @GuidImagem

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC dbo.usp_GravarHistoricoSolicitacaoRTFEvidencia @ID

UPDATE dbo.Arquivo SET IsRascunho = 0 WHERE [Guid] = @GuidImagem";

            const string sqlUpdate = @"
UPDATE SRTFE SET
     ArquivoID  = (SELECT TOP 1 ArquivoID FROM dbo.Arquivo WHERE [Guid] = @GuidImagem)
    ,Descricao  = @Descricao
    ,Ordem      = @Ordem
    ,DataAtualizacao = @DataAtualizacao
    ,UsuarioID  = @UsuarioID
FROM dbo.SolicitacaoRTFEvidencia SRTFE
WHERE SolicitacaoRTFEvidenciaID = @SolicitacaoRTFEvidenciaID

EXEC dbo.usp_GravarHistoricoSolicitacaoRTFEvidencia @SolicitacaoRTFEvidenciaID

UPDATE dbo.Arquivo SET IsRascunho = 0 WHERE [Guid] = @GuidImagem";

            var sql = evidencia.SolicitacaoRTFEvidenciaID > 0 ? sqlUpdate : sqlInserir;
            #endregion
            base.Repository.ExecuteNonQuery(sql, p => 
            {
                p.AddWithValue("@SolicitacaoRTFEvidenciaID ", evidencia.SolicitacaoRTFEvidenciaID);
                p.AddWithValue("@SolicitacaoRTFID", evidencia.SolicitacaoRTFID);
                p.AddWithValue("@TipoEvidenciaID", evidencia.TipoEvidenciaID);
                p.AddWithValue("@GuidImagem", evidencia.GuidImagem);
                p.AddWithValue("@Descricao", evidencia.Descricao);
                p.AddWithValue("@Ordem", evidencia.Ordem);
                p.AddWithValue("@DataAtualizacao", evidencia.DataAtualizacao);
                p.AddWithValue("@UsuarioID", evidencia.UsuarioID);
            });
        }        
        #endregion

        #region EDES +
        //TODO
        #endregion

        #region OG +
        //TODO
        #endregion

        #region DED +
        //TODO
        #endregion

        #region DRS +
        //TODO
        #endregion

        #region CheckLists +
        //TODO
        #endregion
    }
}

