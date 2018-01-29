using Lndr.MdsOnline.Helpers.Extensions;
using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Models.DTO;
using System.Collections.Generic;
using System.Data;
using System.Transactions;

namespace Lndr.MdsOnline.Repositories
{
    public class MdsOnlineRepository : BaseRepository, IMdsOnlineRepository
    {
        public void SalvarDocumento(DocumentoDTO documento)
        {
            using (var tran = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                this.InserirOuAtualizarListaGrupoItemCheckList(documento.GruposItensChecklist);

                this.InserirOuAtualizarListaDocumentoTopico(documento.Topicos);

                this.GravarHistoricoAlteracaoDocumento(documento.DocumentoID);

                tran.Complete();
            }
        }


        private void InserirOuAtualizarDocumento(DocumentoDomain documento)
        {
            if (documento.DocumentoID == 0)
            {
                documento.DocumentoID =  this.InserirDocumento(documento);
            }
            else
            {
                this.AtualizarDocumento(documento);
            }
        }

        private int InserirDocumento(DocumentoDomain documento)
        {
            const string sql = @"
INSERT INTO dbo.Documento (Nome, Descricao, TipoDocumentoID, DataCriacao, DataAtualizacao, UsuarioAtualizacaoID)
SELECT @Nome, @Descricao, @TipoDocumentoID, @DataCriacao, @DataAtualizacao, @UsuarioAtualizacaoID
SELECT SCOPE_IDENTITY()";
            return Repository.ExecuteScalar<int>(sql, p =>
            {
                p.AddWithValue("@Nome", documento.Nome);
                p.AddWithValue("@Descricao", documento.Descricao);
                p.AddWithValue("@TipoDocumentoID", documento.TipoDocumentoID);
                p.AddWithValue("@DataCriacao", documento.DataCriacao);
                p.AddWithValue("@DataAtualizacao", documento.DataAtualizacao);
                p.AddWithValue("@UsuarioAtualizacaoID", documento.UsuarioAtualizacaoID);
            });
        }

        private void AtualizarDocumento(DocumentoDomain documento)
        {
            const string sql = @"
UPDATE dbo.Documento SET
     Nome = @Nome
    ,Descricao = @Descricao
    ,TipoDocumentoID = @TipoDocumentoID
    ,DataAtualizacao = @DataAtualizacao
    ,UsuarioAtualizacaoID @UsuarioAtualizacaoID
WHERE DocumentoID = @DocumentoID
";
            Repository.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@Nome", documento.Nome);
                p.AddWithValue("@Descricao", documento.Descricao);
                p.AddWithValue("@TipoDocumentoID", documento.TipoDocumentoID);
                p.AddWithValue("@DataAtualizacao", documento.DataAtualizacao);
                p.AddWithValue("@UsuarioAtualizacaoID", documento.UsuarioAtualizacaoID);
                p.AddWithValue("@DocumentoID", documento.DocumentoID);
            });
        }


        private void InserirOuAtualizarListaGrupoItemCheckList(IEnumerable<DocumentoGrupoItemCheckListDTO> grupos)
        {
            if (grupos.IsNullOrEmpty()) return;

            foreach (var grupo in grupos)
            {
                this.InserirOuAtualizarGrupoItemCheckList(grupo);
            }
        }

        private void InserirOuAtualizarGrupoItemCheckList(DocumentoGrupoItemCheckListDTO grupoItemCheckList)
        {
            if (grupoItemCheckList.DocumentoGrupoItemCheckListID == 0)
            {
                grupoItemCheckList.DocumentoGrupoItemCheckListID =  this.InserirGrupoItemCheckList(grupoItemCheckList);
                
                if (!grupoItemCheckList.ItensChecklist.IsNullOrEmpty())
                {
                    foreach (var item in grupoItemCheckList.ItensChecklist)
                    {
                        item.DocumentoGrupoItemCheckListID = grupoItemCheckList.DocumentoGrupoItemCheckListID;
                        this.InserirOuAtualizarDocumentoItemCheckList(item);
                    }
                }
            }
            else
            {
                this.AtualizarGrupoItemCheckList(grupoItemCheckList);
            }
        }

        private int InserirGrupoItemCheckList(DocumentoGrupoItemCheckListDomain grupoItemCheckList)
        {
            const string sql = @"
INSERT INTO dbo.DocumentoGrupoItemCheckList (DocumentoID, Nome)
VALUE (@DocumentoID, @Nome)
SELECT SCOPE_IDENTITY()";
            return Repository.ExecuteScalar<int>(sql, p => {
                p.AddWithValue("@DocumentoID", grupoItemCheckList.DocumentoID);
                p.AddWithValue("@Nome", grupoItemCheckList.Nome);
            });
        }

        private void AtualizarGrupoItemCheckList(DocumentoGrupoItemCheckListDomain grupoItemCheckList)
        {
            const string sql = @"
UPDATE dbo.DocumentoGrupoItemCheckList SET
    Nome = @Nome
WHERE DocumentoGrupoItemCheckListID = @DocumentoGrupoItemCheckListID";
            Repository.ExecuteNonQuery(sql, p => {
                p.AddWithValue("@DocumentoGrupoItemCheckListID", grupoItemCheckList.DocumentoGrupoItemCheckListID);
                p.AddWithValue("@Nome", grupoItemCheckList.Nome);
            });
        }


        private void InserirOuAtualizarDocumentoItemCheckList(DocumentoItemCheckListDomain itemCheckList)
        {
            if (itemCheckList.DocumentoItemCheckListID == 0)
            {
                itemCheckList.DocumentoItemCheckListID = this.InserirDocumentoItemCheckList(itemCheckList);
            }
            else
            {
                this.AtualizarDocumentoItemCheckList(itemCheckList);
            }
        }

        private int InserirDocumentoItemCheckList(DocumentoItemCheckListDomain itemCheckList)
        {
            const string sql = @"
INSERT INTO dbo.DocumentoItemCheckList (DocumentoGrupoItemCheckListID, Texto)
VALUES (@DocumentoGrupoItemCheckListID, @Texto)
SELECT SCOPE_IDENTITY()";
            return Repository.ExecuteScalar<int>(sql, p => {
                p.AddWithValue("@DocumentoGrupoItemCheckListID", itemCheckList.DocumentoGrupoItemCheckListID);
                p.AddWithValue("@Texto", itemCheckList.Texto);
            });
        }

        private void AtualizarDocumentoItemCheckList(DocumentoItemCheckListDomain itemCheckList)
        {
            const string sql = @"
UPDATE dbo.DocumentoItemCheckList SET 
    Texto = @Texto
WHERE DocumentoItemCheckListID = @DocumentoItemCheckListID";
            Repository.ExecuteNonQuery(sql, p => {
                p.AddWithValue("@Texto", itemCheckList.Texto);
                p.AddWithValue("@DocumentoItemCheckListID", itemCheckList.DocumentoItemCheckListID);                
            });
        }


        private void InserirOuAtualizarListaDocumentoTopico(IEnumerable<DocumentoTopicoDomain> topicos)
        {
            if (topicos.IsNullOrEmpty()) return;

            foreach (var topico in topicos)
            {
                this.InserirOuAtualizarDocumentoTopico(topico);
            }
        }

        private void InserirOuAtualizarDocumentoTopico(DocumentoTopicoDomain documentoTopico)
        {
            if (documentoTopico.DocumentoTopicoID == 0)
            {
                documentoTopico.DocumentoTopicoID = this.InserirDocumentoTopico(documentoTopico);
            }
            else
            {
                this.AtualizarDocumentoTopico(documentoTopico);
            }
        }

        private int InserirDocumentoTopico(DocumentoTopicoDomain documentoTopico)
        {
            const string sql = @"
INSERT INTO dbo.DocumentoTopico (DocumentoID, Nome)
VALUE (@DocumentoID, @Nome)";
            return Repository.ExecuteScalar<int>(sql, p => {
                p.AddWithValue("@DocumentoID", documentoTopico.DocumentoID);
                p.AddWithValue("@Nome", documentoTopico.Nome);
            });
        }

        private void AtualizarDocumentoTopico(DocumentoTopicoDomain documentoTopico)
        {
            const string sql = @"
UPDATE dbo.DocumentoTopico  SET 
    Nome = @Nome
WHERE DocumentoTopicoID = @DocumentoTopicoID";
            Repository.ExecuteNonQuery(sql, p => {
                p.AddWithValue("@DocumentoTopicoID", documentoTopico.DocumentoTopicoID);
                p.AddWithValue("@Nome", documentoTopico.Nome);
            });
        }


        private void GravarHistoricoAlteracaoDocumento(int documentoID)
        {
            Repository.ExecuteNonQuery("dbo.GravarHistoricoAlteracaoDocumento", p =>
            {
                p.SetCommandType(CommandType.StoredProcedure);
                p.AddWithValue("@DocumentoID", documentoID);
            });
        }
    }
}