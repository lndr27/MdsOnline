using AutoMapper;
using Lndr.MdsOnline.DataModel.Model;
using Lndr.MdsOnline.Services;
using Lndr.MdsOnline.Web.Helpers;
using Lndr.MdsOnline.Web.Helpers.Extensions;
using Lndr.MdsOnline.Web.Models.Domain;
using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        #region Novo RTF
        public void SaveRTF(RtfDTO dto)
        {
            var dataAtualizacao = DateTime.Now;

            using (var db = new MdsOnlineDbContext(SystemHelper.BDMdsConnectionString))
            {
                var rtfBase = db.RTF.Where(r => r.SolicitacaoID == dto.SolicitacaoID).FirstOrDefault();
                rtfBase.DataAtualizacao = dataAtualizacao;
                rtfBase.Historico.Add(Mapper.Map<RtfHistorico>(rtfBase));

                
                this.RemoverEvidenciasNaoEncontradas(dto, db, dataAtualizacao);
                this.ApagarRtfTestes(dto, db);

                // Incluir Testes
                // Incluir Historico dos Testes
                // Incluir Historico das Evidencias
                this.IncluirRtfTestes(dto, db, dataAtualizacao);

                db.SaveChanges();
            }
        }

        private void ApagarRtfTestes(RtfDTO dto, MdsOnlineDbContext db)
        {
            if (dto.Testes.IsNullOrEmpty()) return;

            var testesParaManter = dto.Testes.Where(t => t.RtfTesteID > 0).Select(t => t.RtfTesteID).ToList();
            var testesNaoEncontradosNaBase = db.RtfTeste.Where(r => !testesParaManter.Contains(r.RtfID) && r.RtfID == dto.RtfID).ToList();
            db.RtfTeste.RemoveRange(testesNaoEncontradosNaBase);
        }

        private void IncluirRtfTestes(RtfDTO dto, MdsOnlineDbContext db, DateTime data)
        {
            var todosTestesParaInserir = dto.Testes.Where(t => t.RtfTesteID == 0)
            .Select(t => {

                var evidencias = new List<RtfTesteEvidenciaDTO>();
                if (!t.Evidencias.IsNullOrEmpty())
                {
                    evidencias.AddRange(t.Evidencias);
                }
                if (!t.Erros.IsNullOrEmpty())
                {
                    evidencias.AddRange(t.Erros);
                }

                return new RtfTeste
                {
                    CondicaoCenario             = t.CondicaoCenario,
                    PreCondicao                 = t.PreCondicao,
                    DadosEntrada                = t.DadosEntrada,
                    DataAtualizacao             = data,
                    Funcionalidade              = t.Funcionalidade,
                    Observacoes                 = t.Observacoes,
                    Ordem                       = t.Ordem,
                    ResultadoEsperado           = t.ResultadoEsperado,
                    Sequencia                   = t.Sequencia,
                    StatusExecucaoHomologacaoID = t.StatusExecucaoHomologacaoID,
                    UsuarioID                   = this._context.UsuarioID,
                    RtfID                       = dto.RtfID,
                    Evidencias                  = evidencias.Select(e =>
                    {
                        // Recupear arquivo e retira flag de rascunho
                        var arquivo = db.Arquivo.Where(a => a.Guid.ToString() == e.GuidImagem).FirstOrDefault();
                        arquivo.IsRascunho = false;
                        db.Entry(arquivo).State = EntityState.Modified;

                        return new RtfTesteEvidencia
                        {
                            DataAtualizacao = data,
                            Descricao       = e.Descricao,
                            Ordem           = e.Ordem,
                            TipoEvidenciaID = e.TipoEvidenciaID,
                            UsuarioID       = this._context.UsuarioID,
                            ArquivoID       = arquivo.ArquivoID
                        };
                    }).ToList()
                };
            }).ToList();
            db.RtfTeste.AddRange(todosTestesParaInserir);

            // Salva Historico dos testes e evidencias
            var historico = Mapper.Map<List<RtfTesteHistorico>>(todosTestesParaInserir);
            db.RtfTesteHistorico.AddRange(historico);
        }

        private void AtualizarTestes(RtfDTO dto, MdsOnlineDbContext db, DateTime data)
        {
            var testesParaAtualizar = dto.Testes.Where(t => t.RtfTesteID > 0);
            var testesBase = db.RtfTeste.Where(t => testesParaAtualizar.Any(t2 => t2.RtfTesteID == t.RtfTesteID)).ToList();

            foreach (var teste in testesParaAtualizar)
            {
                var testeBase                         = testesBase.FirstOrDefault(t => t.RtfTesteID == teste.RtfTesteID);
                testeBase.Sequencia                   = teste.Sequencia;
                testeBase.Funcionalidade              = teste.Funcionalidade;
                testeBase.CondicaoCenario             = teste.CondicaoCenario;
                testeBase.PreCondicao                 = teste.PreCondicao;
                testeBase.DadosEntrada                = teste.DadosEntrada;
                testeBase.ResultadoEsperado           = teste.ResultadoEsperado;
                testeBase.Observacoes                 = teste.Observacoes;
                testeBase.StatusExecucaoHomologacaoID = teste.StatusExecucaoHomologacaoID;
                testeBase.Ordem                       = teste.Ordem;
                testeBase.DataAtualizacao             = data;
                testeBase.UsuarioID                   = this._context.UsuarioID;

                var evidenciasNovas = teste.Evidencias.Where(e => e.RtfTesteEvidenciaID == 0).Select(e =>
                {
                    var arquivo = db.Arquivo.Where(a => a.Guid.ToString() == e.GuidImagem).FirstOrDefault();
                    arquivo.IsRascunho = false;
                    db.Entry(arquivo).State = EntityState.Modified;

                    return new RtfTesteEvidencia
                    {
                        RtfTesteID      = testeBase.RtfTesteID,
                        DataAtualizacao = data,
                        Descricao       = e.Descricao,
                        Ordem           = e.Ordem,
                        TipoEvidenciaID = e.TipoEvidenciaID,
                        UsuarioID       = this._context.UsuarioID,
                        ArquivoID       = arquivo.ArquivoID
                    };
                }).ToList();
            }

        }

        private void RemoverEvidenciasNaoEncontradas(RtfDTO dto, MdsOnlineDbContext db, DateTime data)
        {
            if (dto.Testes.IsNullOrEmpty()) return;

            var evidencias = dto.Testes.SelectMany(t => t.Evidencias ?? new List<RtfTesteEvidenciaDTO>()).ToList();

            if (evidencias.IsNullOrEmpty()) return;

            var idsEvidencias = evidencias.Select(e => e.RtfTesteEvidenciaID);
            var testesParaRemover = db.RtfTesteEvidencia.Where(r => idsEvidencias.Contains(r.RtfTesteEvidenciaID)).ToList();
            var arquivosParaRemover = testesParaRemover.Select(t => t.Arquivo).ToList();

            db.RtfTesteEvidencia.RemoveRange(testesParaRemover);
            db.Arquivo.RemoveRange(arquivosParaRemover);
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

