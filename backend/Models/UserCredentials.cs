
namespace backend.Models
{

//////////////////////////////////////////////////////////////////////////
/// <summary>
/// This represents user credentials model class
/// </summary>
/// <remarks>
/// constructor has arguments username and password
/// </remarks>
//////////////////////////////////////////////////////////////////////////

 public class userCredentials
 {
  public userCredentials(string username, string password)
  {
   this.userName = username;
   this.passWord = password;
  }
  public string userName { get; set; }
  public string passWord { get; set; }
 }
}