using AutoMapper;
using Lndr.MdsOnline.DataModel;
using Lndr.MdsOnline.DataModel.Model;
using Lndr.MdsOnline.Web.Helpers;
using Lndr.MdsOnline.Web.Helpers.Extensions;
using Lndr.MdsOnline.Web.Models.Domain;
using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using Lndr.MdsOnline.Web.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lndr.MdsOnline.Services
{
    public class MdsOnlineService : IMdsOnlineService
    {
        private readonly IMdsOnlineRepository _repository;

        private readonly IServiceContext _userContext;

        public MdsOnlineService(IServiceContext serviceContext, IMdsOnlineRepository repository)
        {
            this._repository = repository;
            this._userContext = serviceContext;
        }

        public void UploadArquivo(ArquivoDTO arquivo)
        {
            this._repository.UploadArquivo(arquivo);
        }

        public void ApagarArquivo(string guid)
        {
            this._repository.RemoverArquivo(guid);
        }

        public ArquivoDTO ObterArquivo(string guid)
        {
            return this._repository.ObterArquivo(guid);
        }

        public IEnumerable<SolicitacaoRTUDomain> ObterRTU(int solicitacaoID)
        {
            return this._repository.ObterRTU(solicitacaoID);
        }

        public void SalvarRTU(IEnumerable<SolicitacaoRTUDomain> RTU, int solicitacaoID)
        {
            this._repository.SalvarRTU(RTU, solicitacaoID);
        }

        public IEnumerable<SolicitacaoRTFDTO> ObterRTF(int solicitacaoID)
        {
            var testesDomain = this._repository.ObterTestesRTF(solicitacaoID);
            var evidencias = this._repository.ObterEvidenciasRTF(solicitacaoID);
            var testes = Mapper.Map<List<SolicitacaoRTFDTO>>(testesDomain);

            if (!testes.IsNullOrEmpty() && !evidencias.IsNullOrEmpty())
            {
                testes.ForEach(t => {
                    t.Evidencias = evidencias.Where(e => e.SolicitacaoRTFID == t.SolicitacaoRTFID);
                });
            }
            return testes;
        }

        public void SalvarRTF(IEnumerable<SolicitacaoRTFDTO> RTF, int solicitacaoID)
        {
            this._repository.SalvarRTF(RTF, solicitacaoID);
        }        

        public RtfDTO GetRTF(int solicitacaoID)
        {
            using (var dbContext = new MdsOnlineDbContext(SystemHelper.BDMdsConnectionString))
            {
                dbContext.Database.Log = s => {
                    using (var sw = new StreamWriter("C:\\users\\lndr2.lndr-pc\\desktop\\log.txt", true))
                        sw.Write(s);
                };

                return dbContext.RTF
                    .AsNoTracking()
                    .Where(r => r.SolicitacaoID == solicitacaoID)
                    .Select(r => new RtfDTO
                    {
                        SolicitacaoID          = r.SolicitacaoID,
                        DataAtualizacao        = r.DataAtualizacao,
                        DataCriacao            = r.DataCriacao,
                        UsuarioID              = r.Usuario.UsuarioID,
                        NomeUsuario            = r.Usuario.Nome,
                        UsuarioVerificacaoID   = r.UsuarioVerificacaoID,
                        NomeUsuarioVerificacao = r.UsuarioVerificacao.Nome,
                        Testes                 = r.Testes.Select(t => new RtfTesteDTO
                        {
                            RtfTesteID                  = t.RtfTesteID,
                            CondicaoCenario             = t.CondicaoCenario,
                            DadosEntrada                = t.DadosEntrada,
                            Funcionalidade              = t.Funcionalidade,
                            Observacoes                 = t.Observacoes,
                            Ordem                       = t.Ordem,
                            PreCondicao                 = t.PreCondicao,
                            ResultadoEsperado           = t.ResultadoEsperado,
                            Sequencia                   = t.Sequencia,
                            StatusExecucaoHomologacaoID = t.StatusExecucaoHomologacaoID,
                            Evidencias                  = t.Evidencias.Where(e => e.TipoEvidenciaID == (int)TipoEvidenciaEnum.Sucesso)
                            .Select(e => new RtfTesteEvidenciaDTO
                            {
                                RtfTesteEvidenciaID = e.RtfTesteEvidenciaID,
                                Descricao           = e.Descricao,
                                GuidImagem          = e.Arquivo.Guid.ToString(),
                                Ordem               = e.Ordem,
                                TipoEvidenciaID     = e.TipoEvidenciaID
                            }),
                            Erros = t.Evidencias.Where(e => e.TipoEvidenciaID == (int)TipoEvidenciaEnum.Erro)
                            .Select(e => new RtfTesteEvidenciaDTO
                            {
                                RtfTesteEvidenciaID = e.RtfTesteEvidenciaID,
                                Descricao           = e.Descricao,
                                GuidImagem          = e.Arquivo.Guid.ToString(),
                                Ordem               = e.Ordem,
                                TipoEvidenciaID     = e.TipoEvidenciaID
                            })
                        })
                    })
                    .FirstOrDefault();                
            }
        }

        public void SaveRTF(RtfDTO dto)
        {
            this._repository.SaveRTF(dto);
        }
    }
}