namespace LccApiNet.Enums.Safety
{
    /// <summary>
    /// Possible API method errors enumeration
    /// </summary>
    public class MethodError : SafetyEnum<MethodError>
    {
        static MethodError()
        {
            Init();
        }

        /// <summary>
        /// Occures when API key was not provided or was wrong
        /// </summary>
        [SafetyEnumValue("WrongApiKeyError")]
        public static MethodError WrongApiKey { get; private set; } = null!;

        /// <summary>
        /// Occures when provided access tokens was invalid
        /// during the call of the method that requires access token to execute
        /// </summary>
        [SafetyEnumValue("WrongAccessTokenError")]
        public static MethodError WrongAccessToken { get; private set; } = null!;

        /// <summary>
        /// Occures when access tokens was not provided
        /// during the call of the method that requires access token to execute
        /// </summary>
        [SafetyEnumValue("AccessTokenNotProvidedError")]
        public static MethodError AccessTokenNotProvided { get; private set; } = null!;
        
        /// <summary>
        /// Occures when wrong email, nickname or password was used for authorization
        /// </summary>
        [SafetyEnumValue("WrongNicknameEmailOrPasswordError")]
        public static MethodError WrongNicknameEmailOrPassword { get; private set; } = null!;

        /// <summary>
        /// Occures when API method that requires json parameters was provided
        /// with body that contains json syntax errors
        /// </summary>
        [SafetyEnumValue("JsonParsingError")]
        public static MethodError JsonParsing { get; private set; } = null!;

        /// <summary>
        /// Occures when API method that requires some parameters
        /// was not provided with them
        /// </summary>
        [SafetyEnumValue("MissingMethodParametersError")]
        public static MethodError MissingMethodParameters { get; private set; } = null!;

        /// <summary>
        /// Occures when the API method parameter is invalid
        /// </summary>
        [SafetyEnumValue("InvalidMethodParameterError")]
        public static MethodError InvalidMethodParameter { get; private set; } = null!;
    }
}
