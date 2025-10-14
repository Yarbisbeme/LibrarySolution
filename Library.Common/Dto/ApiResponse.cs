namespace Library.Common.Dto
{

    /// <summary>
    /// DTO para respuesta estándar de API
    /// </summary>
    /// <typeparam name="T">Tipo de datos contenidos en la respuesta</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo del resultado
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Datos de la respuesta
        /// </summary>
        public T Data { get; set; } = default!;

        /// <summary>
        /// Errores de validación o procesamiento
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Constructor para respuesta exitosa
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Operación exitosa")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Constructor para respuesta con error
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }

}