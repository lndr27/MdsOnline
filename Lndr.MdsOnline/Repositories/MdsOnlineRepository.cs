using Lndr.MdsOnline.Helpers.Extensions;
using Lndr.MdsOnline.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Lndr.MdsOnline.Repositories
{
    public class MdsOnlineRepository : BaseRepository, IMdsOnlineRepository
    {
        public IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> ObterRTU(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
SELECT 
     SolicitacaoID
    ,Sequencia
    ,Condicao
    ,DadosEntrada
    ,ResultadoEsperado
    ,Verificacao
    ,ComoTestar
    ,Observacoes
FROM dbo.SolicitacaoRoteiroTesteUnitario
WHERE SolicitacaoID = @SolicitacaoID";
            #endregion

            return base.Repository.FindAll<SolicitacaoRoteiroTesteUnitarioDomain>(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
            });
        }

        #region RTU +
        public void SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioDomain> rtu)
        {
            if (rtu.IsNullOrEmpty()) return;

            this.ApagarTestesNaoEncontrados(rtu.First().SolicitacaoID, rtu.Select(r => r.SolicitacaoRoteiroTesteUnitarioID));

            foreach (var linha in rtu)
            {                
                this.InserirAtualizarRTU(linha);
            }
        }

        private void InserirAtualizarRTU(SolicitacaoRoteiroTesteUnitarioDomain rtu)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO dbo.SolicitacaoRoteiroTesteUnitario (SolicitacaoID, Sequencia, Condicao, DadosEntrada, ResultadoEsperado, Verificacao, ComoTestar, Observacoes)
VALUES (@SolicitacaoID, @Sequencia, @Condicao, @DadosEntrada, @ResultadoEsperado, @Verificacao, @ComoTestar, @Observacoes)";

            const string sqlAtualizar = @"
UPDATE dbo.SolicitacaoRoteiroTesteUnitario SET
     Sequencia           = @Sequencia
    ,Condicao            = @Condicao 
    ,DadosEntrada        = @DadosEntrada 
    ,ResultadoEsperado   = @ResultadoEsperado
    ,Verificacao         = @Verificacao
    ,ComoTestar          = @ComoTestar 
    ,Observacoes         = @Observacoes
WHERE SolicitacaoRoteiroTesteUnitarioID = @SolicitacaoRoteiroTesteUnitarioID";

            var sql = rtu.SolicitacaoRoteiroTesteUnitarioID > 0 ? sqlAtualizar : sqlInserir;
            #endregion

            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@SolicitacaoRoteiroTesteUnitarioID", rtu.SolicitacaoRoteiroTesteUnitarioID);
                p.AddWithValue("@SolicitacaoID", rtu.SolicitacaoID);
                p.AddWithValue("@Sequencia", rtu.Sequencia);
                p.AddWithValue("@Condicao", rtu.Condicao);
                p.AddWithValue("@DadosEntrada", rtu.DadosEntrada);
                p.AddWithValue("@ResultadoEsperado", rtu.ResultadoEsperado);
                p.AddWithValue("@Verificacao", rtu.Verificacao);
                p.AddWithValue("@ComoTestar", rtu.ComoTestar);
                p.AddWithValue("@Observacoes", rtu.Observacoes);
            });
        }

        private void ApagarTestesNaoEncontrados(int solicitacaoID, IEnumerable<int> testes)
        {
            const string sql = @"
DELETE SRTU 
FROM dbo.SolicitacaoRoteiroTesteUnitario SRTU
WHERE SRTU.SolicitacaoRoteiroTesteUnitarioID NOT IN (@testes)
AND SRTU.SolicitacaoID = @SolicitacaoID";

            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
                p.AddWithValue("@Testes", string.Join(",", testes));
            });
        }
        #endregion
    }
}