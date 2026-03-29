namespace software_API.Services
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }

    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        public bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }

    public interface IEmailService
    {
        Task SendAsync(string email, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendAsync(string email, string subject, string message)
        {
            try
            {
                // TODO: Implement actual email sending using SMTP
                _logger.LogInformation($"Email sent to {email}: {subject}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                throw;
            }
        }
    }

    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file, string folder);
        Task DeleteAsync(string filePath);
        bool IsValidImage(IFormFile file);
    }

    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly string _uploadPath;

        public FileService(ILogger<FileService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _uploadPath = configuration.GetValue<string>("UploadPath") ?? "wwwroot/uploads";
        }

        public async Task<string> UploadAsync(IFormFile file, string folder)
        {
            try
            {
                if (!IsValidImage(file))
                    throw new ArgumentException("????? ??? ?? ???? ????");

                var uploadDir = Path.Combine(_uploadPath, folder);
                Directory.CreateDirectory(uploadDir);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return $"/uploads/{folder}/{fileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                throw;
            }
        }

        public async Task DeleteAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine("wwwroot", filePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file");
                throw;
            }
        }

        public bool IsValidImage(IFormFile file)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            return validExtensions.Contains(extension) && file.Length > 0 && file.Length < 5 * 1024 * 1024; // 5MB max
        }
    }
}
