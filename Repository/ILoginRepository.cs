using AspnetCoreMvcFull.Models;

namespace AspnetCoreMvcFull.Repository
{
  public interface ILoginRepository
  {
    public LoginModelDetails Login(LoginModel login);
  }
}
