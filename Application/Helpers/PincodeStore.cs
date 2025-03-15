using Application.Commands.Auth;

namespace Application.Helpers;

public class PincodeStore
{
    private readonly Dictionary<string, string> _pincodeMap = [];
    private readonly Dictionary<string, ValidateSignupCommand> _validateUserMap = [];

    public void AddPincode(string key, string pincode)
    {
        lock (_pincodeMap)
        {
            _pincodeMap[key] = pincode;
        }
    }

    public string GetPincode(string key)
    {
        lock (_pincodeMap)
        {
            return _pincodeMap[key];
        }
    }

    public void AddValidateUser(string key, ValidateSignupCommand user)
    {
        lock (_validateUserMap)
        {
            _validateUserMap[key] = user;
        }
    }

    public ValidateSignupCommand GetValidateUser(string key)
    {
        lock (_validateUserMap)
        {
            return _validateUserMap[key];
        }
    }

    public static string GeneratePincode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
