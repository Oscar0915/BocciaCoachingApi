namespace BocciaCoaching.Services.Interfaces;

public interface IFirebaseAuthenticationService
{
    Task<FirebaseIdentity> VerifyIdTokenAsync(string idToken);
}

public sealed record FirebaseIdentity(string Email, bool EmailVerified);
