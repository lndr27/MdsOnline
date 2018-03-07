using Lndr.MdsOnline.Web.Helpers.DataAccess;
using System.Configuration;

namespace Lndr.MdsOnline.Web.Repositories
{
    public abstract class BaseRepository
    {
        private SqlRepository _repository;

        protected SqlRepository Repository
        {
            get
            {
                _repository = _repository ?? new SqlRepository(ConfigurationManager.ConnectionStrings["BDMdsOnline"].ConnectionString);
                return _repository;
            }
        }
    }
}