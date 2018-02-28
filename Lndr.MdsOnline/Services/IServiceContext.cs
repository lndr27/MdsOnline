namespace Lndr.MdsOnline.Services
{
    public interface IServiceContext
    {
        int UsuarioID { get; }

        string NomeUsuario { get; } 

        string CodigoUsuario { get; }
    }
}
