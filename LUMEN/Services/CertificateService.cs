namespace LUMEN.Services;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.EntityFrameworkCore;
using QRCoder;

public class CertificateService(ApplicationDbContext context) : ICertificateService
{
    public async Task<string> GenerateCertificateAsync(string userId, int subjectId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync([userId], cancellationToken);
        var subject = await context.Subjects.FindAsync([subjectId], cancellationToken);

        if (user == null || subject == null) throw new InvalidOperationException("User or Subject not found.");

        var certCode = $"CERT-{userId}-{subjectId}-{DateTime.UtcNow:yyyyMMddHHmmss}";

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(certCode, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeImage = qrCode.GetGraphic(20);

        var filePath = Path.Combine("wwwroot", "certificates", $"{certCode}.png");
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        await File.WriteAllBytesAsync(filePath, qrCodeImage, cancellationToken);

        var certificate = new Certificate { UserId = userId, SubjectId = subjectId, CertificateUrl = $"/certificates/{certCode}.png", CertificateCode = certCode, IssuedAt = DateTime.UtcNow };
        context.Certificates.Add(certificate);
        await context.SaveChangesAsync(cancellationToken);

        return certificate.CertificateUrl;
    }
}