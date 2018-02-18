using Lndr.MdsOnline.Helpers.Extensions;
using Lndr.MdsOnline.Models.Domain;
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
    ,Ordem
FROM dbo.SolicitacaoRoteiroTesteUnitario
WHERE SolicitacaoID = @SolicitacaoID";
            #endregion

            return base.Repository.FindAll<SolicitacaoRoteiroTesteUnitarioDomain>(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
            });
        }

        public void SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> rtu, int solicitacaoID)
        {
            if (rtu.IsNullOrEmpty()) return;

            this.ApagarTestesNaoEncontrados(solicitacaoID, rtu.Where(r => r.SolicitacaoRoteiroTesteUnitarioID > 0).Select(r => r.SolicitacaoRoteiroTesteUnitarioID));

            foreach (var linha in rtu)
            {                
                this.InserirAtualizarRTU(linha, solicitacaoID);
            }
        }

        private void InserirAtualizarRTU(SolicitacaoRoteiroTesteUnitarioDomain rtu, int solicitacaoID)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO dbo.SolicitacaoRoteiroTesteUnitario (SolicitacaoID, Sequencia, Condicao, DadosEntrada, ResultadoEsperado, Verificacao, ComoTestar, Observacoes, Ordem)
VALUES (@SolicitacaoID, @Sequencia, @Condicao, @DadosEntrada, @ResultadoEsperado, @Verificacao, @ComoTestar, @Observacoes, @Ordem)

DECLARE @ID INT = SCOPE_IDENTITY()ç

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
                p.AddWithValue("@Verificacao", rtu.Verificacao);
                p.AddWithValue("@ComoTestar", rtu.ComoTestar);
                p.AddWithValue("@Observacoes", rtu.Observacoes);
                p.AddWithValue("@Ordem", rtu.Ordem);
            });
        }

        private void ApagarTestesNaoEncontrados(int solicitacaoID, IEnumerable<int> testes)
        {
            if (testes.IsNullOrEmpty()) return;

            const string sql = @"
DELETE SRTU 
FROM dbo.SolicitacaoRoteiroTesteUnitario SRTU
WHERE SRTU.SolicitacaoRoteiroTesteUnitarioID NOT IN (@testes)
AND SRTU.SolicitacaoID = @SolicitacaoID";

            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
                p.AddWithValues("@Testes", testes);
            });
        }
        #endregion
    }
}