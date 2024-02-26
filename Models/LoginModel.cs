using System.ComponentModel.DataAnnotations;

namespace AspnetCoreMvcFull.Models
{
  public class LoginModel
  {
    [Required]
    public string? VCH_USERNAME { get; set; }
    [Required]
    public string? VCH_PASSWORD  { get; set; }
    public string? Status { get; set; }
    public string? Role { get; set; }
  }
  public class LoginModelDetails
  {
    public List<LoginModel>? Data { get; set; }    
  }
}
