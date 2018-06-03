using Lndr.MdsOnline.Services;
using Lndr.MdsOnline.Web.Helpers.Extensions;
using Lndr.MdsOnline.Web.Models.Domain;
using Lndr.MdsOnline.Web.Models.Domain.Rtf;
using Lndr.MdsOnline.Web.Models.Domain.Rtu;
using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Web.Models.DTO.CheckList;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using Lndr.MdsOnline.Web.Models.DTO.Rtu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Lndr.MdsOnline.Web.Repositories
{
    public class MdsOnlineRepository : BaseRepository, IMdsOnlineRepository
    {
        private IServiceContext _context;

        public MdsOnlineRepository(IServiceContext context)
        {
            this._context = context;
        }

        #region Upload Arquivo +
        public void UploadArquivo(ArquivoDTO arquivo)
        {
            #region SQL +
            const string sql = @"
INSERT INTO MDS.Arquivo ([Guid], Nome, Extensao, ContentType, TamanhoKb, Arquivo, IsRascunho, DataUpload, UsuarioID)
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
            const string sql = @"DELETE FROM MDS.Arquivo WHERE [Guid] = @Guid";
            base.Repository.ExecuteNonQuery(sql, p => p.AddWithValue("@Guid", guid));
        }

        public ArquivoDTO ObterArquivo(string guid)
        {
            const string sql = @"
SELECT TOP 1 [Guid], Nome, Extensao, ContentType, TamanhoKb, Arquivo, IsRascunho, DataUpload, UsuarioID
FROM MDS.Arquivo
WHERE [Guid] = @Guid";
            return base.Repository.FindOne<ArquivoDTO>(sql, p => p.AddWithValue("@Guid", guid));
        }
        #endregion

        #region RTU +
        public RtuDomain ObterRtu(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
SELECT 
     RtuID
    ,SolicitacaoID
    ,DataCriacao
    ,DataAtualizacao
    ,UsuarioID
    ,UsuarioAtualizacaoID
FROM Rtu
WHERE SolicitacaoID = @SolicitacaoID";
            #endregion
            return base.Repository.FindOne<RtuDomain>(sql, p => p.AddWithValue("@SolicitacaoID", solicitacaoID));
        }

        public IEnumerable<RtuTesteDomain> ObterTestesRTU(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
SELECT
     RT.RtuTesteID
    ,RT.RtuID
    ,RT.Sequencia
    ,RT.Condicao
    ,RT.DadosEntrada
    ,RT.ResultadoEsperado
    ,RT.StatusVerificacaoTesteUnitarioID
    ,RT.ComoTestar
    ,RT.Observacoes
    ,RT.DataAtualizacao
    ,ISNULL(NULLIF(RT.Ordem, 0), ROW_NUMBER() OVER (ORDER BY RT.RtuTesteID)) Ordem
    ,RT.UsuarioID
FROM MDS.RtuTeste RT
JOIN MDS.Rtu RTU ON RTU.RtuID = RT.RtuID
WHERE RTU.SolicitacaoID = @SolicitacaoID
ORDER BY Ordem";
            #endregion

            return base.Repository.FindAll<RtuTesteDomain>(sql, p => p.AddWithValue("@SolicitacaoID", solicitacaoID));
        }

        public void SalvarRtu(RtuDTO rtu)
        {
            using (var tran = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                var dataAtualizacao = DateTime.Now;
                var rtuID = this.InserirAtualizarRtu(rtu, dataAtualizacao);
                this.SalvarTestesRtu(rtuID, rtu.Testes, dataAtualizacao);
                tran.Complete();
            }
        }

        private int InserirAtualizarRtu(RtuDTO rtu, DateTime dataAtualizacao)
        {
            #region SQL +
            const string sqlInsert = @"
INSERT INTO MDS.Rtu (SolicitacaoID, DataCriacao, DataAtualizacao, UsuarioID, UsuarioVerificacaoID, UsuarioAtualizacaoID)
VALUES (@SolicitacaoID, @DataAtualizacao, @DataAtualizacao, @UsuarioID, @UsuarioVerificacaoID, @UsuarioAtualizacaoID)

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC MDS.usp_GravarHistoricoRtu @ID

SELECT @ID";

            const string sqlUpdate = @"
UPDATE MDS.Rtu SET
     DataAtualizacao      = @DataAtualizacao
    ,UsuarioID            = @UsuarioID
    ,UsuarioVerificacaoID = @UsuarioVerificacaoID
    ,UsuarioAtualizacaoID = @UsuarioAtualizacaoID
WHERE RtuID = @RtuID

EXEC MDS.usp_GravarHistoricoRtu @RtuID

SELECT @RtuID";

            var sql = rtu.RtuID > 0 ? sqlUpdate : sqlInsert;
            #endregion
            return base.Repository.ExecuteScalar<int>(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", rtu.SolicitacaoID);
                p.AddWithValue("@DataAtualizacao", dataAtualizacao);
                p.AddWithValue("@UsuarioID", rtu.UsuarioID);
                p.AddWithValue("@UsuarioVerificacaoID", rtu.UsuarioVerificacaoID);
                p.AddWithValue("@UsuarioAtualizacaoID", this._context.UsuarioID);
            });
        }

        private void SalvarTestesRtu(int rtuID, IEnumerable<RtuTesteDTO> testes, DateTime dataAtualizacao)
        {
            if (testes.IsNullOrEmpty()) return;

            this.ApagarTestesUnitariosNaoEncontrados(rtuID, testes.Where(r => r.RtuTesteID > 0).Select(r => r.RtuTesteID));

            foreach (var teste in testes)
            {
                teste.DataAtualizacao = dataAtualizacao;
                teste.RtuID = rtuID;
                this.InserirAtualizarRtuTeste(teste);
            }
        }

        private void InserirAtualizarRtuTeste(RtuTesteDTO teste)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO MDS.RtuTeste 
        ( RtuID,  Sequencia,  Condicao,  DadosEntrada,  ResultadoEsperado,  StatusVerificacaoTesteUnitarioID,  ComoTestar,  Observacoes,  Ordem,   DataAtualizacao,  UsuarioID)
VALUES  (@RtuID, @Sequencia, @Condicao, @DadosEntrada, @ResultadoEsperado, @StatusVerificacaoTesteUnitarioID, @ComoTestar, @Observacoes, @Ordem, @DataAtualizacao, @UsuarioID)

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC MDS.usp_GravarHistoricoRtuTeste @ID";

            const string sqlAtualizar = @"
UPDATE MDS.RtuTeste SET
     Sequencia           = @Sequencia
    ,Condicao            = @Condicao 
    ,DadosEntrada        = @DadosEntrada 
    ,ResultadoEsperado   = @ResultadoEsperado
    ,StatusVerificacaoTesteUnitarioID         = @StatusVerificacaoTesteUnitarioID
    ,ComoTestar          = @ComoTestar 
    ,Observacoes         = @Observacoes
    ,Ordem               = @Ordem
    ,DataAtualizacao     = @DataAtualizacao
    ,UsuarioID           = @UsuarioID
WHERE RtuTesteID = @RtuTesteID

EXEC MDS.usp_GravarHistoricoRtuTeste @RtuTesteID";

            var sql = teste.RtuTesteID > 0 ? sqlAtualizar : sqlInserir;
            #endregion
            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@RtuTesteID", teste.RtuTesteID);
                p.AddWithValue("@RtuID", teste.RtuID);
                p.AddWithValue("@Sequencia", teste.Sequencia);
                p.AddWithValue("@Condicao", teste.Condicao);
                p.AddWithValue("@DadosEntrada", teste.DadosEntrada);
                p.AddWithValue("@ResultadoEsperado", teste.ResultadoEsperado);
                p.AddWithValue("@StatusVerificacaoTesteUnitarioID", teste.StatusVerificacaoTesteUnitarioID);
                p.AddWithValue("@ComoTestar", teste.ComoTestar);
                p.AddWithValue("@Observacoes", teste.Observacoes);
                p.AddWithValue("@Ordem", teste.Ordem);
                p.AddWithValue("@DataAtualizacao", teste.DataAtualizacao);
                p.AddWithValue("@UsuarioID", this._context.UsuarioID);
            });
        }

        private void ApagarTestesUnitariosNaoEncontrados(int rtuID, IEnumerable<int> testes)
        {
            if (testes.IsNullOrEmpty()) return;

            const string sql = @"
DELETE RT 
FROM MDS.RtuTeste RT
WHERE RT.RtuTesteID NOT IN (@RtuTesteID)
AND RT.RtuID = @RtuID";

            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@RtuID", rtuID);
                p.AddWithValues("@RtuTesteID", testes);
            });
        }
        #endregion

        #region RTF +
        public RtfDomain ObterRtf(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
SELECT 
     RtfID
    ,SolicitacaoID
    ,DataCriacao
    ,DataAtualizacao
    ,UsuarioID
    ,UsuarioVerificacaoID
    ,UsuarioAtualizacaoID
FROM MDS.Rtf
WHERE SolicitacaoID = @SolicitacaoID";
            #endregion
            return base.Repository.FindOne<RtfDomain>(sql, p => p.AddWithValue("@SolicitacaoID", solicitacaoID));
        }

        public IEnumerable<RtfTesteDomain> ObterRtfTestes(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
SELECT
     RT.RtfTesteID
    ,RT.RtfID
    ,RT.Sequencia
    ,RT.Funcionalidade
    ,RT.CondicaoCenario
    ,RT.PreCondicao
    ,RT.DadosEntrada
    ,RT.ResultadoEsperado
    ,RT.Observacoes
    ,RT.StatusExecucaoHomologacaoID
    ,ISNULL(NULLIF(RT.Ordem, 0), ROW_NUMBER() OVER (ORDER BY RT.RtfTesteID)) Ordem
    ,RT.DataAtualizacao
    ,RT.UsuarioID
FROM MDS.RtfTeste RT
JOIN MDS.Rtf R ON R.RtfID = RT.RtfID
WHERE R.SolicitacaoID = @SolicitacaoID
ORDER BY Ordem";
            #endregion
            return base.Repository.FindAll<RtfTesteDomain>(sql, p => p.AddWithValue("@SolicitacaoID", solicitacaoID));
        }

        public IEnumerable<RtfTesteEvidenciaDTO> ObterRtfTesteEvidencias(int solicitacaoID)
        {
            #region SQL +
            const string sql = @"
SELECT 
     RTE.RtfTesteEvidenciaID
    ,RTE.RtfTesteID
    ,RTE.TipoEvidenciaID
    ,CONVERT(NVARCHAR(36), A.Guid) GuidImagem
    ,RTE.Descricao
    ,RTE.Ordem
FROM MDS.RtfTesteEvidencia RTE
JOIN MDS.RtfTeste RT ON RT.RtfTesteID = RTE.RtfTesteID
JOIN MDS.Rtf R ON R.RtfID = RT.RtfID
JOIN MDS.Arquivo A ON A.ArquivoID = RTE.ArquivoID
WHERE R.SolicitacaoID = @SolicitacaoID";
            #endregion
            return base.Repository.FindAll<RtfTesteEvidenciaDTO>(sql, p => p.AddWithValue("@SolicitacaoID", solicitacaoID));
        }

        public void SalvarRTF(RtfDTO rtf)
        {
            using (var tran = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                var evidencias = new List<RtfTesteEvidenciaDTO>();
                var dataAtualizacao = DateTime.Now;

                var rtfId = this.SalvarInserirRtf(rtf, dataAtualizacao);

                this.ApagarTestesFuncionaisNaoEncontrados(rtfId, rtf.Testes.Where(r => r.RtfTesteID > 0).Select(r => r.RtfTesteID));

                foreach (var teste in rtf.Testes)
                {
                    teste.RtfTesteID = this.InserirAtualizarRtfTeste(teste, rtfId, dataAtualizacao);

                    // Preenche ids dos testes criados
                    if (!teste.Evidencias.IsNullOrEmpty())
                    {
                        teste.Evidencias.ToList().ForEach(e => e.RtfTesteID = teste.RtfTesteID);
                        evidencias.AddRange(teste.Evidencias);
                    }                    
                    if (!teste.Erros.IsNullOrEmpty())
                    {
                        teste.Erros.ToList().ForEach(e => e.RtfTesteID = teste.RtfTesteID);
                        evidencias.AddRange(teste.Erros);
                    }
                }
                this.SalvarEvidencias(rtfId, evidencias, dataAtualizacao);
                tran.Complete();
            }
        }

        private int SalvarInserirRtf(RtfDTO rtf, DateTime dataAtualizacao)
        {
            #region SQL +
            const string sqlInsert = @"
INSERT INTO MDS.Rtf ( 
        SolicitacaoID,   DataCriacao,  DataAtualizacao,  UsuarioID,  UsuarioVerificacaoID,  UsuarioAtualizacaoID)
VALUES (@SolicitacaoID, @DataCriacao, @DataAtualizacao, @UsuarioID, @UsuarioVerificacaoID, @UsuarioAtualizacaoID)

DECLARE @ID INT = SCOPE_IDENTITY()

EXEC MDS.usp_GravarHistoricoRtf @ID

SELECT @ID";

            const string sqlUpdate = @"
UPDATE MDS.Rtf SET 
     DataAtualizacao = @DataAtualizacao
    ,UsuarioID = @UsuarioID
    ,UsuarioVerificacaoID = @UsuarioVerificacaoID
    ,UsuarioAtualizacaoID = @UsuarioAtualizacaoID
WHERE RtfID = @RtfID

EXEC MDS.usp_GravarHistoricoRtf @RtfID

SELECT @RtfID";

            var sql = rtf.RtfID > 0 ? sqlUpdate : sqlInsert;
            #endregion
            return base.Repository.ExecuteScalar<int>(sql, p => {
                p.AddWithValue("@RtfID", rtf.RtfID);
                p.AddWithValue("@SolicitacaoID", rtf.SolicitacaoID);
                p.AddWithValue("@DataCriacao", dataAtualizacao);
                p.AddWithValue("@DataAtualizacao", dataAtualizacao);
                p.AddWithValue("@UsuarioID", rtf.UsuarioID);
                p.AddWithValue("@UsuarioVerificacaoID", rtf.UsuarioVerificacaoID);
                p.AddWithValue("@UsuarioAtualizacaoID", this._context.UsuarioID);
            });
        }

        private void ApagarTestesFuncionaisNaoEncontrados(int rtfId, IEnumerable<int> testes)
        {
            if (testes.IsNullOrEmpty()) return;

            const string sql = @"
DELETE RT 
FROM MDS.RtfTeste RT
JOIN MDS.Rtf R ON R.RtfID = RT.RtfID
WHERE RT.RtfTesteID NOT IN (@Testes)
AND R.RtfID = @RtfID";

            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@RtfID", rtfId);
                p.AddWithValues("@Testes", testes);
            });
        }

        private int InserirAtualizarRtfTeste(RtfTesteDTO teste, int rtfId, DateTime dataAtualizacao)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO MDS.RtfTeste 
        ( RtfID,  Sequencia,  Funcionalidade,  CondicaoCenario,  PreCondicao,  DadosEntrada,  ResultadoEsperado,  Observacoes,  StatusExecucaoHomologacaoID,  Ordem,  DataAtualizacao,  UsuarioID)
VALUES  (@RtfID, @Sequencia, @Funcionalidade, @CondicaoCenario, @PreCondicao, @DadosEntrada, @ResultadoEsperado, @Observacoes, @StatusExecucaoHomologacaoID, @Ordem, @DataAtualizacao, @UsuarioID)

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC MDS.usp_GravarHistoricoRtfTeste @ID

SELECT @ID";

            const string sqlAtualizar = @"
UPDATE MDS.RtfTeste SET
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
WHERE   RtfTesteID = @RtfTesteID

EXEC MDS.usp_GravarHistoricoRtfTeste @RtfTesteID

SELECT @RtfTesteID";

            var sql = teste.RtfTesteID > 0 ? sqlAtualizar : sqlInserir;
            #endregion
            return base.Repository.ExecuteScalar<int>(sql, p =>
            {
                p.AddWithValue("@RtfTesteID", teste.RtfTesteID);
                p.AddWithValue("@RtfID", rtfId);
                p.AddWithValue("@Sequencia", teste.Sequencia);
                p.AddWithValue("@Funcionalidade", teste.Funcionalidade);
                p.AddWithValue("@CondicaoCenario", teste.CondicaoCenario);
                p.AddWithValue("@PreCondicao", teste.PreCondicao);
                p.AddWithValue("@DadosEntrada", teste.DadosEntrada);
                p.AddWithValue("@ResultadoEsperado", teste.ResultadoEsperado);
                p.AddWithValue("@Observacoes", teste.Observacoes);
                p.AddWithValue("@DataAtualizacao", dataAtualizacao);
                p.AddWithValue("@StatusExecucaoHomologacaoID", teste.StatusExecucaoHomologacaoID);
                p.AddWithValue("@Ordem", teste.Ordem);
                p.AddWithValue("@UsuarioID", this._context.UsuarioID);
            });
        }
        
        private void SalvarEvidencias(int rtfId, IEnumerable<RtfTesteEvidenciaDTO> evidencias, DateTime dataAtualizacao)
        {
            if (evidencias.IsNullOrEmpty()) return;

            this.ApagarEvidenciasNaoEncontradas(rtfId, evidencias.Where(e => e.RtfTesteEvidenciaID > 0).Select(e => e.RtfTesteEvidenciaID));

            var index = 1;
            foreach(var evidencia in evidencias)
            {
                evidencia.Ordem = index++;
                this.InserirAtualizarEvidencia(evidencia, dataAtualizacao);
            }
        }

        private void ApagarEvidenciasNaoEncontradas(int rtfId, IEnumerable<int> evidencias)
        {
            if (evidencias.IsNullOrEmpty()) return;

            #region SQL +
            const string sql = @"
DECLARE @TEMP TABLE (ArquivoID INT, RtfTesteEvidenciaID INT)

INSERT INTO @TEMP (ArquivoID, RtfTesteEvidenciaID)
SELECT RTE.ArquivoID, RTE.RtfTesteEvidenciaID 
FROM MDS.RtfTesteEvidencia RTE
JOIN MDS.RtfTeste RT ON RT.RtfTesteID = RTE.RtfTesteID
JOIN MDS.Rtf R ON R.RtfID = RT.RtfID
WHERE   R.RtfID = @RtfID
    AND RTE.RtfTesteEvidenciaID NOT IN (@Evidencias)

DELETE RTE
FROM MDS.RtfTesteEvidencia RTE
JOIN @TEMP T
    ON T.RtfTesteEvidenciaID = RTE.RtfTesteEvidenciaID

DELETE A
FROM MDS.Arquivo A
JOIN @TEMP T
    ON T.ArquivoID = A.ArquivoID";
            #endregion
            base.Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@RtfID", rtfId);
                p.AddWithValues("@Evidencias", evidencias);
            });
        }

        private void InserirAtualizarEvidencia(RtfTesteEvidenciaDTO evidencia, DateTime dataAtualizacao)
        {
            #region SQL +
            const string sqlInserir = @"
INSERT INTO MDS.RtfTesteEvidencia 
        ( RtfTesteID,  TipoEvidenciaID,   ArquivoID,  Descricao,  Ordem,  DataAtualizacao,  UsuarioID)
SELECT   @RtfTesteID, @TipoEvidenciaID, A.ArquivoID, @Descricao, @Ordem, @DataAtualizacao, @UsuarioID
FROM MDS.Arquivo A
WHERE [Guid] = @GuidImagem

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC MDS.usp_GravarHistoricoTesteEvidencia @ID

UPDATE MDS.Arquivo SET IsRascunho = 0 WHERE [Guid] = @GuidImagem";

            const string sqlUpdate = @"
UPDATE RTE SET
     ArquivoID  = (SELECT TOP 1 ArquivoID FROM MDS.Arquivo WHERE [Guid] = @GuidImagem)
    ,Descricao  = @Descricao
    ,Ordem      = @Ordem
    ,DataAtualizacao = @DataAtualizacao
    ,UsuarioID  = @UsuarioID
FROM MDS.RtfTesteEvidencia RTE
WHERE RtfTesteEvidenciaID = @RtfTesteEvidenciaID

EXEC MDS.usp_GravarHistoricoTesteEvidencia @RtfTesteEvidenciaID

UPDATE MDS.Arquivo SET IsRascunho = 0 WHERE [Guid] = @GuidImagem";

            var sql = evidencia.RtfTesteEvidenciaID > 0 ? sqlUpdate : sqlInserir;
            #endregion
            base.Repository.ExecuteNonQuery(sql, p => 
            {
                p.AddWithValue("@RtfTesteEvidenciaID ", evidencia.RtfTesteEvidenciaID);
                p.AddWithValue("@RtfTesteID", evidencia.RtfTesteID);
                p.AddWithValue("@TipoEvidenciaID", evidencia.TipoEvidenciaID);
                p.AddWithValue("@GuidImagem", evidencia.GuidImagem);
                p.AddWithValue("@Descricao", evidencia.Descricao);
                p.AddWithValue("@Ordem", evidencia.Ordem);
                p.AddWithValue("@DataAtualizacao", dataAtualizacao);
                p.AddWithValue("@UsuarioID", this._context.UsuarioID);
            });
        }
        #endregion

        #region CheckList +
        public CheckListDTO ObterCheckList(int solicitacaoID, int checklistID)
        {
            #region SQL +
            const string sql = @"
SELECT 
     CL.CheckListID
    ,CL.Nome
    ,CL.Descricao
    ,CL.DataCriacao
    ,CL.UsuarioCriacaoID
    ,CL.DataAtualizacao
    ,CL.UsuarioAtualizacaoID
    ,CLS.UsuarioID
    ,CLS.UsuarioVerificacaoID
    ,CLS.DataCriacao
    ,CLS.DataAtualizacao
    ,CLS.UsuarioAtualizacaoID
FROM MDS.CheckList CL
LEFT JOIN MDS.CheckListSolicitacao CLS
    ON CLS.CheckListID = CL.CheckListID
WHERE   ISNULL(CLS.SolicitacaoID, @SolicitacaoID) = @SolicitacaoID
    AND CL.CheckListID = @CheckListID";
            #endregion
            return base.Repository.FindOne<CheckListDTO>(sql, p => 
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
                p.AddWithValue("@CheckListID", checklistID);
            });
        }

        public IEnumerable<CheckListGrupoItemDTO> ObterCheckListGrupoItem(int checklistID)
        {
            #region SQL +
            const string sql = @"
SELECT 
     CheckListGrupoItemID
    ,CheckListID
    ,Nome
    ,Descricao
FROM MDS.CheckListGrupoItem
WHERE CheckListID = @CheckListID";
            #endregion
            return base.Repository.FindAll<CheckListGrupoItemDTO>(sql, p => 
            {
                p.AddWithValue("@CheckListID", checklistID);
            });
        }

        public IEnumerable<CheckListItemDTO> ObterCheckListItens(int solicitacaoID, int checklistID)
        {
            #region SQL +
            const string sql = @"
SELECT 
     CLI.CheckListItemID
    ,CLI.CheckListGrupoItemID
    ,CLI.Nome
    ,CLI.Descricao
    ,CLIR.Sim
    ,CLIR.Nao
    ,CLIR.NaoAplicavel
    ,CLIR.Observacao
FROM MDS.CheckListItem CLI
JOIN MDS.CheckListGrupoItem CLGI
    ON CLGI.CheckListGrupoItemID = CLI.CheckListGrupoItemID
LEFT JOIN MDS.CheckListSolicitacao CLS
    ON CLS.CheckListID = CLGI.CheckListID
LEFT JOIN MDS.CheckListItemResposta CLIR
    ON CLIR.CheckListSolicitacaoID = CLS.CheckListSolicitacaoID
WHERE   CLGI.CheckListID = @ChecklistID
    AND ISNULL(CLS.SolicitacaoID, @SolicitacaoID) = @SolicitacaoID";
            #endregion
            return base.Repository.FindAll<CheckListItemDTO>(sql, p =>
            {
                p.AddWithValue("@SolicitacaoID", solicitacaoID);
                p.AddWithValue("@ChecklistID", checklistID);
            });
        }

        public void SalvarCheckList(CheckListDTO checklist)
        {
            using (var tran = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                checklist.CheckListID = this.InserirOuAtualizarCheckList(checklist, DateTime.Now);
                this.SalvarCheckListGruposItens(checklist.GruposItens, checklist.CheckListID);

                var todosItens = checklist.GruposItens.SelectMany(g => g.Itens).Select(it => it.CheckListItemID);
                this.ApagarCheckListItensNaoEncontrados(checklist.CheckListID, todosItens);

                var todosGrupos = checklist.GruposItens.Select(g => g.CheckListGrupoItemID);
                this.ApagarCheckListGrupoItensNaoEncontrados(checklist.CheckListID, todosGrupos);

                tran.Complete();
            }
        }

        private int InserirOuAtualizarCheckList(CheckListDTO checklist, DateTime dataAtualizacao)
        {
            #region SQL +
            const string sqlInsert = @"
INSERT INTO MDS.CheckList 
        (Nome,  Descricao,  DataCriacao,  UsuarioCriacaoID,  DataAtualizacao,  UsuarioAtualizacaoID)
VALUES (@Nome, @Descricao, @DataCriacao, @UsuarioCriacaoID, @DataAtualizacao, @UsuarioAtualizacaoID)

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC MDS.usp_GravarCheckListHistorico @ID

SELECT @ID";

            const string sqlUpdate = @"
UPDATE MDS.CheckList SET
     Nome = @Nome
    ,Descricao = @Descricao
    ,@DataAtualizacao = @DataAtualizacao
    ,UsuarioAtualizacaoID = @UsuarioAtualizacaoID

EXEC MDS.usp_GravarCheckListHistorico @CheckListID

SELECT @CheckListID";

            var sql = checklist.CheckListID > 0 ? sqlUpdate : sqlInsert;
            #endregion
            return base.Repository.ExecuteScalar<int>(sql, p =>
            {
                p.AddWithValue("@CheckListID", checklist.CheckListID);
                p.AddWithValue("@UsuarioCriacaoID", checklist.UsuarioCriacaoID);
                p.AddWithValue("@Nome", checklist.Nome);
                p.AddWithValue("@Descricao", checklist.Descricao);
                p.AddWithValue("@DataCriacao", dataAtualizacao);
                p.AddWithValue("@DataAtualizacao", dataAtualizacao);
                p.AddWithValue("@UsuarioAtualizacaoID", checklist.UsuarioAtualizacaoID);
            });

        }

        private void SalvarCheckListGruposItens(IEnumerable<CheckListGrupoItemDTO> grupos, int checklistID)
        {
            foreach (var grupo in grupos)
            {
                grupo.CheckListGrupoItemID = this.InserirOuAtualizarCheckListGrupoItem(grupo, checklistID);
                this.SalvarCheckListItens(grupo.Itens, grupo.CheckListGrupoItemID);
            }
        }

        private int InserirOuAtualizarCheckListGrupoItem(CheckListGrupoItemDTO grupo, int checklistID)
        {
            #region SQL +
            const string sqlInsert = @"
INSERT INTO MDS.CheckListGrupoItem 
        ( CheckListID,  Nome,  Descricao)
VALUES  (@CheckListID, @Nome, @Descricao)

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC MDS.usp_GravarCheckListGrupoItem @ID

SELECT @ID";

            const string sqlUpdate = @"
UPDATE MDS.CheckListGrupoItem SET
     Nome = @Nome
    ,Descricao = @Descricao
WHERE CheckListGrupoItemID = @CheckListGrupoItemID

EXEC MDS.usp_GravarCheckListGrupoItem @CheckListGrupoItemID

SELECT @CheckListGrupoItemID";

            var sql = grupo.CheckListGrupoItemID > 0 ? sqlUpdate : sqlInsert;
            #endregion

            return base.Repository.ExecuteScalar<int>(sql, p =>
            {
                p.AddWithValue("@CheckListGrupoItemID", grupo.CheckListGrupoItemID);
                p.AddWithValue("@CheckListID", checklistID);
                p.AddWithValue("@Nome", grupo.Nome);
                p.AddWithValue("@Descricao", grupo.Descricao);
            });
        }

        private void SalvarCheckListItens(IEnumerable<CheckListItemDTO> itens, int checklistGrupoID)
        {
            foreach (var item in itens)
            {
                item.CheckListItemID = this.InserirOuAtualizarCheckListItem(item, checklistGrupoID);
            }
        }

        private int InserirOuAtualizarCheckListItem(CheckListItemDTO item, int checklistGrupoID)
        {
            #region SQL +
            const string sqlInsert = @"
INSERT INTO MDS.CheckListItem
        (CheckListGrupoItemID,  Nome,  Descricao)
VALUES (@CheckListGrupoItemID, @Nome, @Descricao)

DECLARE @ID INT = SCOPE_IDENTITY();

EXEC MDS.usp_GravarCheckListItemHistorico @ID

SELECT @ID";

            const string sqlUpdate = @"
UPDATE MDS.CheckListItem SET
     Nome = @Nome
    ,Descricao = @Descricao
WHERE CheckListItemID = @CheckListItemID

EXEC MDS.usp_GravarCheckListItemHistorico @CheckListItemID

SELECT @CheckListItemID";

            var sql = item.CheckListItemID > 0 ? sqlUpdate : sqlInsert;
            #endregion

            return base.Repository.ExecuteScalar<int>(sql, p =>
            {
                p.AddWithValue("@CheckListItemID", item.CheckListItemID);
                p.AddWithValue("@CheckListGrupoItemID", checklistGrupoID);
                p.AddWithValue("@Nome", item.Nome);
                p.AddWithValue("@Descricao", item.Descricao);
            });
        }

        private void ApagarCheckListItensNaoEncontrados(int checklistID, IEnumerable<int> checklistItensIds)
        {
            if (checklistID == 0 || checklistItensIds.IsNullOrEmpty()) return;

            #region SQL +
            const string sql = @"
DELETE CLI
FROM MDS.CheckListItem CLI
JOIN MDS.CheckListGrupoItem CLG
    ON CLG.CheckListGrupoItemID = CLI.CheckListGrupoItemID
WHERE   CLG.CheckListID = @CheckListID
    AND CLI.CheckListItemID NOT IN (@CheckListItensIDs)";
            #endregion
            base.Repository.ExecuteNonQuery(sql, p => 
            {
                p.AddWithValue("@CheckListID", checklistID);
                p.AddWithValues("@CheckListItensIds", checklistItensIds);
            });
        }

        private void ApagarCheckListGrupoItensNaoEncontrados(int checklistId, IEnumerable<int> checklistGrupoItensIds)
        {
            if (checklistId == 0 || checklistGrupoItensIds.IsNullOrEmpty()) return;

            #region SQL +
            const string sql = @"
DELETE CLG
FROM MDS.CheckListGrupoItem CLG
WHERE   CLG.CheckListID = @CheckListID
    AND CLG.CheckListGrupoItemID NOT IN (@CheckListGrupoItensIds)";
            #endregion
            base.Repository.ExecuteNonQuery(sql, p => 
            {
                p.AddWithValue("@CheckListID", checklistId);
                p.AddWithValues("@CheckListGrupoItensIds", checklistGrupoItensIds);
            });
        }

        public List<PaginacaoCheckListDTO> ObterListaCheckLists(int pagina, int tamanhoPagina)
        {
            #region SQL +
            const string sql = @"
SELECT 
     CheckListID
    ,Nome
    ,Descricao
    ,DataCriacao
    ,DataAtualizacao
    ,NomeUsuarioAtualizacao
    ,NomeUsuarioCriacao
FROM (
    SELECT
         ROW_NUMBER() OVER (ORDER BY CL.CheckListID) RowID
        ,CL.CheckListID
        ,CL.Nome
        ,CL.Descricao
        ,CL.DataCriacao
        ,CL.DataAtualizacao
        ,U.Nome NomeUsuarioAtualizacao
        ,U2.Nome NomeUsuarioCriacao
    FROM MDS.CheckList CL
    JOIN MDS.Usuario U
        ON U.UsuarioID = CL.UsuarioAtualizacaoID
    JOIN MDS.Usuario U2
        ON U2.UsuarioID = CL.UsuarioCriacaoID
) T
WHERE   RowID BETWEEN @Offset AND (@Offset + @TamPagina)";
            #endregion
            return base.Repository.FindAll<PaginacaoCheckListDTO>(sql, p => 
            {
                p.AddWithValue("@Offset", (pagina - 1) * tamanhoPagina);
                p.AddWithValue("@TamPagina", tamanhoPagina);
            }).ToList();
        }

        public int ObterQuantidadeTotalCheckLists()
        {
            return base.Repository.ExecuteScalar<int>(@"SELECT COUNT(1) FROM MDS.CheckList");
        }

        #endregion

        #region TODO
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
        #endregion
    }
}
