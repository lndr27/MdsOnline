using Lndr.MdsOnline.Helpers.Extensions;
using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Lndr.MdsOnline.Repositories
{
    public class MdsOnlineRepository : BaseRepository, IMdsOnlineRepository
    {
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

            foreach (var linha in rtu)
            {
                this.InserirAtualizarRTU(linha, solicitacaoID);
            }
        }

        private void InserirAtualizarRTU(SolicitacaoRoteiroTesteUnitarioDomain rtu, int solicitacaoID)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO dbo.SolicitacaoRoteiroTesteUnitario 
        (SolicitacaoID,  Sequencia,  Condicao,  DadosEntrada,  ResultadoEsperado,  Verificacao,  ComoTestar,  Observacoes,  Ordem)
VALUES  (@SolicitacaoID, @Sequencia, @Condicao, @DadosEntrada, @ResultadoEsperado, @Verificacao, @ComoTestar, @Observacoes, @Ordem)

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
     SRTEE.SolicitacaoRoteiroTesteFuncionalID
    ,SRTEE.TipoEvidenciaID
    ,I.Guid AS GuidImagem
    ,SRTEE.Descricao
FROM dbo.SolicitacaoRoteiroTesteFuncionalEvidencia SRTEE
JOIN dbo.SolicitacaoRoteiroTesteFuncional SRTE 
    ON SRTE.SolicitacaoRoteiroTesteFuncionalID = SRTEE.SolicitacaoRoteiroTesteFuncionalID
JOIN dbo.Imagem I 
    ON I.ImagemID = SRTEE.ImagemID
WHERE SRTE.SolicitacaoID = @SolicitacaoID";
            #endregion
            return base.Repository.FindAll<SolicitacaoRoteiroTesteFuncionalEvidenciaDTO>(sql, p => p.AddWithValue("@SolicitacaoID", solicitacaoID));
        }

        public void SalvarRTF(IEnumerable<SolicitacaoRoteiroTesteFuncionalDomain> rtf, int solicitacaoID)
        {
            if (rtf.IsNullOrEmpty()) return;

            this.ApagarTestesUnitariosNaoEncontrados(solicitacaoID, rtf.Where(r => r.SolicitacaoRoteiroTesteFuncionalID > 0).Select(r => r.SolicitacaoRoteiroTesteFuncionalID));

            foreach (var linha in rtf)
            {
                this.InserirAtualizarRTU(linha, solicitacaoID);
            }
        }

        private void InserirAtualizarRTU(SolicitacaoRoteiroTesteFuncionalDomain rtf, int solicitacaoID)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO dbo.SolicitacaoRoteiroTesteFuncional 
        (SolicitacaoID,   Sequencia,  Funcionalidade,  CondicaoCenario,  PreCondicao,  DadosEntrada,  ResultadoEsperado,  Observacoes,  StatusExecucaoHomologacaoID,  Ordem)
VALUES  (@SolicitacaoID, @Sequencia, @Funcionalidade, @CondicaoCenario, @PreCondicao, @DadosEntrada, @ResultadoEsperado, @Observacoes, @StatusExecucaoHomologacaoID, @Ordem)

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC dbo.usp_GravarHistoricoSolicitacaoRoteiroTesteFuncional @ID";

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
WHERE   SolicitacaoRoteiroTesteFuncionalID = @SolicitacaoRoteiroTesteFuncionalID

EXEC dbo.usp_GravarHistoricoSolicitacaoRoteiroTesteFuncional @SolicitacaoRoteiroTesteFuncionalID";

            var sql = rtf.SolicitacaoRoteiroTesteFuncionalID > 0 ? sqlAtualizar : sqlInserir;
            #endregion
            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@SolicitacaoRoteiroTesteFuncionalID", rtf.SolicitacaoRoteiroTesteFuncionalID);
                p.AddWithValue("@SolicitacaoID", rtf.SolicitacaoID);
                p.AddWithValue("@Sequencia", rtf.Sequencia);
                p.AddWithValue("@Funcionalidade", rtf.Funcionalidade);
                p.AddWithValue("@CondicaoCenario", rtf.CondicaoCenario);
                p.AddWithValue("@PreCondicao", rtf.PreCondicao);
                p.AddWithValue("@DadosEntrada", rtf.DadosEntrada);
                p.AddWithValue("@ResultadoEsperado", rtf.ResultadoEsperado);
                p.AddWithValue("@Observacoes", rtf.Observacoes);
                p.AddWithValue("@StatusExecucaoHomologacaoID", rtf.StatusExecucaoHomologacaoID);
                p.AddWithValue("@Ordem", rtf.Ordem);
            });
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

