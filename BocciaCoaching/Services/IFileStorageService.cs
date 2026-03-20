using Microsoft.AspNetCore.Http;

namespace BocciaCoaching.Services
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Guarda un archivo y devuelve la ruta relativa donde quedó almacenado
        /// </summary>
        Task<string> SaveFileAsync(IFormFile file, string subFolder);

        /// <summary>
        /// Elimina un archivo por su ruta relativa
        /// </summary>
        Task DeleteFileAsync(string relativePath);
    }
}

