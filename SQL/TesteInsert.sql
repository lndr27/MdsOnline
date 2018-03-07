SELECT *
FROM dbo.Rtf

select * from dbo.usuario

BEGIN TRAN --ROLLBACK --COMMIT

insert into dbo.usuario (nome, codigo, email, datacriacao, dataatualizacao)
values ('Leandro Almeida', 'leandro.almeida', 'lndr27@hotmail.com', getdate(), getdate())

DECLARE @usuarioID INT = SCOPE_IDENTITY();

insert into dbo.rtf (SolicitacaoID, DataCriacao, DataAtualizacao, usuarioid, UsuarioVerificacaoID)
values (1, getdate(), getdate(), @usuarioID, @usuarioID)


DECLARE @RtfID INT = SCOPE_IDENTITY();

insert into dbo.RtfTeste (rtfId, Sequencia, StatusExecucaoHomologacaoID, DataAtualizacao, UsuarioID, Ordem)
VALUES (@RtfId, 'RGN 1', 1, getdate(), @usuarioID, 0)

DECLARE @RtFTesteID INT = SCOPE_IDENTITY();

insert into dbo.Arquivo (guid, nome, extensao, ContentType, TamanhoKB, DataUpload, UsuarioID)
values (newid(), 'teste.jpg', 'jpg', 'image/jpg', 123, getdate(), @usuarioID);

declare @arquivoID INT = SCOPE_IDENTITY();

insert into dbo.RtfTesteEvidencia (RtfTesteID, DataAtualizacao, UsuarioID, ArquivoID, ORdem, TipoEvidenciaID)
values (@rtfTesteID, Getdate(), @usuarioId, @arquivoID, 0, 1);


select *
from dbo.rtfHistorico