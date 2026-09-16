using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services;

public class FirebaseAuthenticationService : IFirebaseAuthenticationService
{
    private const string FirebaseAppName = "BocciaCoaching";
    private static readonly object FirebaseAppLock = new();
    private readonly IConfiguration _configuration;

    public FirebaseAuthenticationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<FirebaseIdentity> VerifyIdTokenAsync(string idToken)
    {
        if (string.IsNullOrWhiteSpace(idToken))
            throw new ArgumentException("El token de Firebase es requerido", nameof(idToken));

        var token = await GetFirebaseAuth().VerifyIdTokenAsync(idToken, checkRevoked: true);

        if (!token.Claims.TryGetValue("email", out var emailValue) ||
            emailValue is not string email ||
            string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("El token de Firebase no contiene un email.", nameof(idToken));
        }

        var emailVerified = token.Claims.TryGetValue("email_verified", out var verifiedValue) &&
                            verifiedValue is bool verified &&
                            verified;

        return new FirebaseIdentity(email.Trim(), emailVerified);
    }

    private FirebaseAuth GetFirebaseAuth()
    {
        lock (FirebaseAppLock)
        {
            try
            {
                return FirebaseAuth.GetAuth(FirebaseApp.GetInstance(FirebaseAppName));
            }
            catch (ArgumentException)
            {
                var app = FirebaseApp.Create(new AppOptions { Credential = ResolveCredential() }, FirebaseAppName);
                return FirebaseAuth.GetAuth(app);
            }
        }
    }

    private GoogleCredential ResolveCredential()
    {
        // Prioridad 1: JSON de credenciales embebido en una variable de entorno
        // (usado en despliegues como Render, donde no se puede subir el archivo al repo).
        var credentialsJson = _configuration["Firebase:CredentialsJson"]
                               ?? Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS_JSON");
        if (!string.IsNullOrWhiteSpace(credentialsJson))
        {
            return GoogleCredential.FromJson(credentialsJson);
        }

        // Prioridad 2: ruta a un archivo de credenciales en disco (uso local).
        var configuredCredentialPath = _configuration["Firebase:CredentialsPath"];
        if (!string.IsNullOrWhiteSpace(configuredCredentialPath))
        {
            if (!File.Exists(configuredCredentialPath))
            {
                throw new InvalidOperationException(
                    "No se encontró el archivo de credenciales configurado para Firebase.");
            }

            return GoogleCredential.FromFile(configuredCredentialPath);
        }

        // Prioridad 3: Application Default Credentials del entorno.
        return GoogleCredential.GetApplicationDefault();
    }
}
