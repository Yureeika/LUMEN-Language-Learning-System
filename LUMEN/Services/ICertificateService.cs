namespace LUMEN.Services;

public interface ICertificateService
{
    Task<string> GenerateCertificateAsync(string userId, int subjectId, CancellationToken cancellationToken = default);
}