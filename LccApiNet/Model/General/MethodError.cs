using Ardalis.SmartEnum;

namespace LccApiNet.Model.General
{
    /// <summary>
    /// Possible API method errors enumeration
    /// </summary>
    public class MethodError : SmartEnum<MethodError>
    {
        public MethodError(string name, int value) : base(name, value) { }


        /// <summary>
        /// Occures when API key was not provided or was wrong
        /// </summary>
        public static readonly MethodError WrongApiKey = new MethodError("WrongApiKeyError", 1);

        /// <summary>
        /// Occures when provided access tokens was invalid
        /// during the call of the method that requires access token to execute
        /// </summary>
        public static readonly MethodError WrongAccessToken = new MethodError("WrongAccessTokenError", 2);

        /// <summary>
        /// Occures when access tokens was not provided
        /// during the call of the method that requires access token to execute
        /// </summary>
        public static readonly MethodError AccessTokenNotProvided = new MethodError("AccessTokenNotProvidedError", 3);

        /// <summary>
        /// Occures when wrong email, nickname or password was used for authorization
        /// </summary>
        public static readonly MethodError WrongNicknameEmailOrPassword = new MethodError("WrongNicknameEmailOrPasswordError", 4);

        /// <summary>
        /// Occures when API method that requires json parameters was provided
        /// with body that contains json syntax errors
        /// </summary>
        public static readonly MethodError JsonParsingError = new MethodError("JsonParsingError", 5);

        /// <summary>
        /// Occures when API method that requires some parameters
        /// was not provided with them
        /// </summary>
        public static MethodError MissingMethodParameters = new MethodError("MissingMethodParametersError", 5);

        /// <summary>
        /// Occures when the API method parameter is invalid
        /// </summary>
        public static readonly MethodError InvalidMethodParameter = new MethodError("InvalidMethodParameterError", 6);
    }
}
