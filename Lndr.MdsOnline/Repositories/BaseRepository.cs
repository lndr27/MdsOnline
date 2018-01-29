using Lndr.MdsOnline.Helpers.DataAccess;
using System.Configuration;

namespace Lndr.MdsOnline.Repositories
{
    public class BaseRepository
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