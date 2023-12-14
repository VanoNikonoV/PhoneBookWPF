using System.Threading.Channels;

namespace PhoneBookWPF.Context
{
    public static class AccessForToken
    {
        private static string token = string.Empty;
        public static string Token 
        {   
            get { return token; }
            set 
            { 
                if (token == value) return;
                   token = value;
                   onСhangedToken(); 
            } 
        } 

        public static event СhangedToken onСhangedToken;

        public delegate void СhangedToken();
    }
}
