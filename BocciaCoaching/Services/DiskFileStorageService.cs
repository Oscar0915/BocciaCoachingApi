using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace BocciaCoaching.Services
{
    public class DiskFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DiskFileStorageService> _logger;

        private static readonly string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

        public DiskFileStorageService(IWebHostEnvironment env, ILogger<DiskFileStorageService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public Task DeleteFileAsync(string relativePath)
        {
            try
            {
                var cleaned = relativePath.TrimStart('/', '\\');
                var fullPath = Path.Combine(_env.WebRootPath, cleaned);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar archivo {path}", relativePath);
            }

            return Task.CompletedTask;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("El archivo es inválido");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                throw new ArgumentException("Tipo de archivo no permitido");

            var webRoot = _env.WebRootPath ?? "wwwroot";
            var targetFolder = Path.Combine(webRoot, "images", subFolder);
            Directory.CreateDirectory(targetFolder);

            // Generar nombre único
            var fileName = GenerateFileName(ext);
            var relativePath = Path.Combine("images", subFolder, fileName).Replace(Path.DirectorySeparatorChar, '/');
            var fullPath = Path.Combine(targetFolder, fileName);

            // Guardar
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/" + relativePath; // ruta relativa con slash inicial
        }

        private string GenerateFileName(string ext)
        {
            return $"{Guid.NewGuid():N}{ext}";
        }
    }
}


