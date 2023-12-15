using System.Threading.Channels;

namespace PhoneBookWPF.Context
{
    /// <summary>
    /// Класс для хранения jwt-tokena полученнного из при входе в web-API
    /// </summary>
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

        /// <summary>
        /// Событие при смене токена
        /// </summary>
        public static event СhangedToken onСhangedToken;

        public delegate void СhangedToken();
    }
}
