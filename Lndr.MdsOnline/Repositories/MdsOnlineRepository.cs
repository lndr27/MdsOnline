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
INSERT INTO dbo.Arquivo ([Guid], Nome, Extensao, ContentType, TamanhoKb, Arquivo, IsRascunho, DataUpload)
VALUES (@Guid, @Nome, @Extensao, @ContentType, @TamanhoKb, @Arquivo, @IsRascunho, @DataUpload)";
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
SELECT TOP 1 [Guid], Nome, Extensao, ContentType, TamanhoKb, Arquivo, IsRascunho, DataUpload
FROM dbo.Arquivo
WHERE [Guid] = @Guid";
            return base.Repository.FindOne<ArquivoDTO>(sql, p => p.AddWithValue("@Guid", guid));
        }
        #endregion

        #region RTU +
        public IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> ObterRTU(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
SELECT
     SolicitacaoRoteiroTesteUnitarioID
    ,SolicitacaoID
    ,Sequencia
    ,Condicao
    ,DadosEntrada
    ,ResultadoEsperado
    ,Verificacao
    ,ComoTestar
    ,Observacoes
    ,DataAtualizacao
    ,ISNULL(NULLIF(Ordem, 0), ROW_NUMBER() OVER (ORDER BY SolicitacaoRoteiroTesteUnitarioID)) Ordem
FROM dbo.SolicitacaoRoteiroTesteUnitario
WHERE SolicitacaoID = @SolicitacaoID
ORDER BY Ordem";
            #endregion

            return base.Repository.FindAll<SolicitacaoRoteiroTesteUnitarioDomain>(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
            });
        }

        public void SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> rtu, int solicitacaoID)
        {
            if (rtu.IsNullOrEmpty()) return;

            this.ApagarTestesUnitariosNaoEncontrados(solicitacaoID, rtu.Where(r => r.SolicitacaoRoteiroTesteUnitarioID > 0).Select(r => r.SolicitacaoRoteiroTesteUnitarioID));

            var dataAtualizacao = DateTime.Now;

            foreach (var linha in rtu)
            {
                linha.DataAtualizacao = dataAtualizacao;
                this.InserirAtualizarRTU(linha, solicitacaoID);
            }
        }

        private void InserirAtualizarRTU(SolicitacaoRoteiroTesteUnitarioDomain rtu, int solicitacaoID)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO dbo.SolicitacaoRoteiroTesteUnitario 
        (SolicitacaoID,  Sequencia,  Condicao,  DadosEntrada,  ResultadoEsperado,  Verificacao,  ComoTestar,  Observacoes,  Ordem,   DataAtualizacao)
VALUES  (@SolicitacaoID, @Sequencia, @Condicao, @DadosEntrada, @ResultadoEsperado, @Verificacao, @ComoTestar, @Observacoes, @Ordem, @DataAtualizacao)

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC dbo.usp_GravarHistoricoSolicitacaoRoteiroTesteUnitario @ID";

            const string sqlAtualizar = @"
UPDATE dbo.SolicitacaoRoteiroTesteUnitario SET
     Sequencia           = @Sequencia
    ,Condicao            = @Condicao 
    ,DadosEntrada        = @DadosEntrada 
    ,ResultadoEsperado   = @ResultadoEsperado
    ,Verificacao         = @Verificacao
    ,ComoTestar          = @ComoTestar 
    ,Observacoes         = @Observacoes
    ,Ordem               = @Ordem
    ,DataAtualizacao     = @DataAtualizacao
WHERE SolicitacaoRoteiroTesteUnitarioID = @SolicitacaoRoteiroTesteUnitarioID

EXEC dbo.usp_GravarHistoricoSolicitacaoRoteiroTesteUnitario @SolicitacaoRoteiroTesteUnitarioID";

            var sql = rtu.SolicitacaoRoteiroTesteUnitarioID > 0 ? sqlAtualizar : sqlInserir;
            #endregion
            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@SolicitacaoRoteiroTesteUnitarioID", rtu.SolicitacaoRoteiroTesteUnitarioID);
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
            });
        }

        private void ApagarTestesUnitariosNaoEncontrados(int solicitacaoID, IEnumerable<int> testes)
        {
            if (testes.IsNullOrEmpty()) return;

            const string sql = @"
DELETE SRTU 
FROM dbo.SolicitacaoRoteiroTesteUnitario SRTU
WHERE SRTU.SolicitacaoRoteiroTesteUnitarioID NOT IN (@Testes)
AND SRTU.SolicitacaoID = @SolicitacaoID";

            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
                p.AddWithValues("@Testes", testes);
            });
        }
        #endregion

        #region RTF +
        public IEnumerable<SolicitacaoRoteiroTesteFuncionalDomain> ObterTestesRTF(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
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
    ,DataAtualizacao
    ,ISNULL(NULLIF(Ordem, 0), ROW_NUMBER() OVER (ORDER BY SolicitacaoRoteiroTesteFuncionalID)) Ordem
FROM dbo.SolicitacaoRoteiroTesteFuncional
WHERE SolicitacaoID = @SolicitacaoID
ORDER BY Ordem";
            #endregion
            return base.Repository.FindAll<SolicitacaoRoteiroTesteFuncionalDomain>(sql, p => p.AddWithValue("@SolicitacaoID", solicitacaoID));
        }

        public IEnumerable<SolicitacaoRoteiroTesteFuncionalEvidenciaDTO> ObterEvidenciasRTF(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
SELECT 
     SRTEE.SolicitacaoRoteiroTesteFuncionalEvidenciaID
    ,SRTEE.SolicitacaoRoteiroTesteFuncionalID
    ,SRTEE.TipoEvidenciaID
    ,CAST(A.[Guid] AS CHAR(36)) AS GuidImagem
    ,SRTEE.Descricao
    ,SRTEE.Ordem
FROM dbo.SolicitacaoRoteiroTesteFuncionalEvidencia SRTEE
JOIN dbo.SolicitacaoRoteiroTesteFuncional SRTE 
    ON SRTE.SolicitacaoRoteiroTesteFuncionalID = SRTEE.SolicitacaoRoteiroTesteFuncionalID
JOIN dbo.Arquivo A 
    ON A.ArquivoID = SRTEE.ArquivoID
WHERE SRTE.SolicitacaoID = @SolicitacaoID
ORDER BY SRTEE.Ordem, SRTEE.SolicitacaoRoteiroTesteFuncionalEvidenciaID";
            #endregion
            return base.Repository.FindAll<SolicitacaoRoteiroTesteFuncionalEvidenciaDTO>(sql, p => p.AddWithValue("@SolicitacaoID", solicitacaoID));
        }

        public void SalvarRTF(IEnumerable<SolicitacaoRoteiroTesteFuncionalDTO> rtf, int solicitacaoID)
        {
            if (rtf.IsNullOrEmpty()) return;

            var evidencias = new List<SolicitacaoRoteiroTesteFuncionalEvidenciaDTO>();
            var dataAtualizacao = DateTime.Now;

            using (var tran = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                this.ApagarTestesUnitariosNaoEncontrados(solicitacaoID, rtf.Where(r => r.SolicitacaoRoteiroTesteFuncionalID > 0).Select(r => r.SolicitacaoRoteiroTesteFuncionalID));

                foreach (var linha in rtf)
                {
                    linha.DataAtualizacao = dataAtualizacao;
                    linha.SolicitacaoRoteiroTesteFuncionalID = this.InserirAtualizarRTF(linha, solicitacaoID);

                    if (!linha.Evidencias.IsNullOrEmpty())
                    {
                        linha.Evidencias.ToList().ForEach(e => 
                        {
                            e.SolicitacaoRoteiroTesteFuncionalID = linha.SolicitacaoRoteiroTesteFuncionalID;
                            e.DataAtualizacao = dataAtualizacao;
                        });
                        evidencias.AddRange(linha.Evidencias);
                    }
                    if (!linha.Erros.IsNullOrEmpty())
                    {
                        linha.Erros.ToList().ForEach(e => e.SolicitacaoRoteiroTesteFuncionalID = linha.SolicitacaoRoteiroTesteFuncionalID);
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
FROM dbo.SolicitacaoRoteiroTesteFuncional SRTF
WHERE SRTF.SolicitacaoRoteiroTesteFuncionalID NOT IN (@Testes)
AND SRTF.SolicitacaoID = @SolicitacaoID";

            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
                p.AddWithValues("@Testes", testes);
            });
        }

        private int InserirAtualizarRTF(SolicitacaoRoteiroTesteFuncionalDTO rtf, int solicitacaoID)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO dbo.SolicitacaoRoteiroTesteFuncional 
        (SolicitacaoID,   Sequencia,  Funcionalidade,  CondicaoCenario,  PreCondicao,  DadosEntrada,  ResultadoEsperado,  Observacoes,  StatusExecucaoHomologacaoID,  Ordem,  DataAtualizacao)
VALUES  (@SolicitacaoID, @Sequencia, @Funcionalidade, @CondicaoCenario, @PreCondicao, @DadosEntrada, @ResultadoEsperado, @Observacoes, @StatusExecucaoHomologacaoID, @Ordem, @DataAtualizacao)

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC dbo.usp_GravarHistoricoSolicitacaoRoteiroTesteFuncional @ID

SELECT @ID";

            const string sqlAtualizar = @"
UPDATE dbo.SolicitacaoRoteiroTesteFuncional SET
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
WHERE   SolicitacaoRoteiroTesteFuncionalID = @SolicitacaoRoteiroTesteFuncionalID

EXEC dbo.usp_GravarHistoricoSolicitacaoRoteiroTesteFuncional @SolicitacaoRoteiroTesteFuncionalID

SELECT @SolicitacaoRoteiroTesteFuncionalID";

            var sql = rtf.SolicitacaoRoteiroTesteFuncionalID > 0 ? sqlAtualizar : sqlInserir;
            #endregion
            return base.Repository.ExecuteScalar<int>(sql, p =>
            {
                p.AddWithValue("@SolicitacaoRoteiroTesteFuncionalID", rtf.SolicitacaoRoteiroTesteFuncionalID);
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
            });
        }
        
        private void SalvarEvidencias(int solicitacaoID, IEnumerable<SolicitacaoRoteiroTesteFuncionalEvidenciaDTO> evidencias)
        {
            if (evidencias.IsNullOrEmpty()) return;

            this.ApagarEvidenciasNaoEncontradas(solicitacaoID, evidencias.Where(e => e.SolicitacaoRoteiroTesteFuncionalEvidenciaID > 0).Select(e => e.SolicitacaoRoteiroTesteFuncionalEvidenciaID));

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
DECLARE @TEMP TABLE (ArquivoID INT, SolicitacaoRoteiroTesteFuncionalEvidenciaID INT)

INSERT INTO @TEMP (ArquivoID, SolicitacaoRoteiroTesteFuncionalEvidenciaID)
SELECT SRTFE.ArquivoID, SRTFE.SolicitacaoRoteiroTesteFuncionalEvidenciaID 
FROM dbo.SolicitacaoRoteiroTesteFuncionalEvidencia SRTFE
JOIN dbo.SolicitacaoRoteiroTesteFuncional SRTF
    ON SRTF.SolicitacaoRoteiroTesteFuncionalID = SRTFE.SolicitacaoRoteiroTesteFuncionalID
WHERE   SRTF.SolicitacaoID = @SolicitacaoID
    AND SRTFE.SolicitacaoRoteiroTesteFuncionalEvidenciaID NOT IN (@Evidencias)

DELETE SRTFE
FROM dbo.SolicitacaoRoteiroTesteFuncionalEvidencia SRTFE
JOIN @TEMP T
    ON T.SolicitacaoRoteiroTesteFuncionalEvidenciaID = SRTFE.SolicitacaoRoteiroTesteFuncionalEvidenciaID

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

        private void InserirAtualizarEvidencia(SolicitacaoRoteiroTesteFuncionalEvidenciaDTO evidencia)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO dbo.SolicitacaoRoteiroTesteFuncionalEvidencia 
        (SolicitacaoRoteiroTesteFuncionalID, TipoEvidenciaID, ArquivoID, Descricao, Ordem, DataAtualizacao)
SELECT   @SolicitacaoRoteiroTesteFuncionalID, @TipoEvidenciaID, A.ArquivoID, @Descricao, @Ordem, @DataAtualizacao
FROM dbo.Arquivo A
WHERE [Guid] = @GuidImagem

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC dbo.usp_GravarHistoricoSolicitacaoRoteiroTesteFuncionalEvidencia @ID

UPDATE dbo.Arquivo SET IsRascunho = 0 WHERE [Guid] = @GuidImagem";

            const string sqlUpdate = @"
UPDATE SRTFE SET
     ArquivoID  = (SELECT TOP 1 ArquivoID FROM dbo.Arquivo WHERE [Guid] = @GuidImagem)
    ,Descricao  = @Descricao
    ,Ordem      = @Ordem
    ,DataAtualizacao = @DataAtualizacao
FROM dbo.SolicitacaoRoteiroTesteFuncionalEvidencia SRTFE
WHERE SolicitacaoRoteiroTesteFuncionalEvidenciaID = @SolicitacaoRoteiroTesteFuncionalEvidenciaID

EXEC dbo.usp_GravarHistoricoSolicitacaoRoteiroTesteFuncionalEvidencia @SolicitacaoRoteiroTesteFuncionalEvidenciaID

UPDATE dbo.Arquivo SET IsRascunho = 0 WHERE [Guid] = @GuidImagem";

            var sql = evidencia.SolicitacaoRoteiroTesteFuncionalEvidenciaID > 0 ? sqlUpdate : sqlInserir;
            #endregion
            base.Repository.ExecuteNonQuery(sql, p => 
            {
                p.AddWithValue("@SolicitacaoRoteiroTesteFuncionalEvidenciaID ", evidencia.SolicitacaoRoteiroTesteFuncionalEvidenciaID);
                p.AddWithValue("@SolicitacaoRoteiroTesteFuncionalID", evidencia.SolicitacaoRoteiroTesteFuncionalID);
                p.AddWithValue("@TipoEvidenciaID", evidencia.TipoEvidenciaID);
                p.AddWithValue("@GuidImagem", evidencia.GuidImagem);
                p.AddWithValue("@Descricao", evidencia.Descricao);
                p.AddWithValue("@Ordem", evidencia.Ordem);
                p.AddWithValue("@DataAtualizacao", evidencia.DataAtualizacao);
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

