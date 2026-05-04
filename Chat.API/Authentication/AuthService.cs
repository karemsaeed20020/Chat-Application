using Chat.API.Abstractions;
using Chat.API.DTOs.Authentication;
using Chat.API.Entities;
using Chat.API.Errors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace Chat.API.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtProvider _jwtProvider;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _refreshTokenExpiryDays = 14;
        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IJwtProvider jwtProvider, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtProvider = jwtProvider;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<AuthResponse>> LoginAsync(_LoginRequest request, CancellationToken cancellationToken = default)
        {
            // Check Email
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result.Failure<AuthResponse>(UserError.InvalidCredential);
            // Check Password
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
            if (result.Succeeded)
            {
                //send roles of user to token for front end 
                var userRoles = await _userManager.GetRolesAsync(user);
                // Generate JWT Token
                var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRoles);
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresIn = refreshTokenExpiration
                });
                await _userManager.UpdateAsync(user);
                var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);
                return Result.Success(response);
            }
            var error =
             result.IsLockedOut
            ? UserError.UserLockOut
            : result.IsNotAllowed
            ? UserError.EmailNotConfirmed
            : UserError.InvalidCredential;

            return Result.Failure<AuthResponse>(error);
        }

        // Register a new user
        public async Task<Result> RegisterAsync(_RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var email = await _userManager.FindByEmailAsync(request.Email);
            if (email != null)
                return Result.Failure(UserError.DuplicateUser);

            var user = new User()
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                // Generate code to send it in email to user to confirmed
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // Start Send Email
                await SendEmailConfirmation(user, code);

                return Result.Success();

            }
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status409Conflict));
        }

        // Start Send Email
        private async Task SendEmailConfirmation(User user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            var TempPath = $"{Directory.GetCurrentDirectory()}/Templates/EmailConfirmation.html";
            StreamReader streamReader = new StreamReader(TempPath);
            var body = streamReader.ReadToEnd();
            streamReader.Close();
            body = body
            .Replace("[name]", $"{user.FirstName} {user.LastName}")
            .Replace("[action_url]", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}");
            await _emailSender.SendEmailAsync(user.Email!, "Confirm your email", body);
            await Task.CompletedTask;
        }

        private async Task SendResetPassword(User user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            var TempPath = $"{Directory.GetCurrentDirectory()}/Templates/ForgetPassword.html";
            StreamReader streamReader = new StreamReader(TempPath);
            var body = streamReader.ReadToEnd();
            streamReader.Close();
            body = body
                .Replace("{{name}}", $"{user}")
                .Replace("{{action_url}}", $"{origin}/auth/ForgetPassword?email={user.Email}&code={code}");

            await _emailSender.SendEmailAsync(user.Email!, "Reset your password", body);
            await Task.CompletedTask;
        }

        // Generate Refresh Token
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        // Confirm Email
        public async Task<Result> ConfirmEmailAsync(_ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UseId);
            if (user is null)
            {
                return Result.Failure(UserError.UserNotFound);
            }
            if (user.EmailConfirmed)
            {
                return Result.Failure(UserError.DuplicateConfirmed);
            }
            var code = request.Code;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (Exception ex)
            {
                return Result.Failure(UserError.InvalidCode);
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Result.Success();
            }
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> SendResetPasswordAsync(_ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result.Success(); // Don't reveal that the email does not exist
            if (!user.EmailConfirmed)
                return Result.Failure(UserError.EmailNotConfirmed);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await SendResetPassword(user, code);
            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(_ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || !user.EmailConfirmed)
                return Result.Failure(UserError.InvalidCode);
            IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
            }
            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
        }

        public async Task<Result> ResendConfirmationEmailAsync(_ResendConfirmationEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Success();
            if (user.EmailConfirmed)
                return Result.Failure(UserError.DuplicateConfirmed);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await SendEmailConfirmation(user, code);
            return Result.Success();
        }

        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure<AuthResponse>(UserError.InvalidRefreshToken);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure<AuthResponse>(UserError.InvalidRefreshToken);

            if (user.LockoutEnd > DateTime.UtcNow)
                return Result.Failure<AuthResponse>(UserError.UserLockOut);


            var userRefreshToken = user.RefreshTokens.SingleOrDefault(r => r.Token == refreshToken);
            if (userRefreshToken is null)
                return Result.Failure<AuthResponse>(UserError.InvalidRefreshToken);
            userRefreshToken.RevokedIn = DateTime.UtcNow;

            //Regenerate new token
            var userRoles = await _userManager.GetRolesAsync(user);
            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, userRoles);

            //generate new refresh taken
            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresIn = refreshTokenExpiration
            });
            await _userManager.UpdateAsync(user);

            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);
            return Result.Success(response);

        }

        public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure(UserError.InvalidRefreshToken_Token);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure(UserError.InvalidRefreshToken_Token);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken);
            if (userRefreshToken is null)
                return Result.Failure(UserError.InvalidRefreshToken_Token);

            userRefreshToken.RevokedIn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return Result.Success();
        }
    }
}
