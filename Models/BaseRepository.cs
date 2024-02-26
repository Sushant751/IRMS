using System.Data;
using System.Data.SqlClient;

namespace AspnetCoreMvcFull.Models
{
  public class BaseRepository
  {
    private readonly IConfiguration _configuration;
    public BaseRepository(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    public IDbConnection ConnectionDB()
    {
      return new SqlConnection(_configuration.GetConnectionString("DefaultCon"));

    }
  }
}
