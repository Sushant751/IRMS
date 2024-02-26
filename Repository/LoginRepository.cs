using AspnetCoreMvcFull.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AspnetCoreMvcFull.Repository
{
  public class LoginRepository :BaseRepository, ILoginRepository
  {
    public LoginRepository(IConfiguration configuration):base(configuration)
    {
          
    }
    public LoginModelDetails Login(LoginModel login)
    {
      LoginModelDetails loginModelDetails = new();
      try
      {
        using (var con = ConnectionDB())
        {
          var param = new DynamicParameters();
          //param.Add("@P_action", "DM");
          param.Add("@P_Username", login.VCH_USERNAME);
          param.Add("@P_Password", login.VCH_PASSWORD);
          var LoginData = con.QueryMultiple("USP_USER_LOGIN", param, commandType: CommandType.StoredProcedure);
          loginModelDetails.Data = LoginData.Read<LoginModel>().ToList();
          return loginModelDetails;
        }

      }
      catch (Exception ex)
      {
        throw;
      }
    }
  }
}


