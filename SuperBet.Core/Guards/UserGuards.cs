using Ardalis.GuardClauses;
using SuperBet.Core.Models;
using System.Text.RegularExpressions;

namespace SuperBet.Core.Guards
{
    public static class UserGuards
    {
        public static void DuplicateEmail(this IGuardClause guardClause, string email, IEnumerable<User> users)
        {
            if (users.Any(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase))) 
                throw new ArgumentException($"User with email '{email}' already exists.");
        }

        public static void InvalidUsername(this IGuardClause guardClause, string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be empty.");
            
            if (username.Length < 3)
                throw new ArgumentException("Username must be longer than 3 characters.");
            
            if (username.Length > 32)
                throw new ArgumentException("Username must not be longer than 32 characters.");
        }

        public static void InvalidAge(this IGuardClause guardClause, int age)
        {
            if (age < 18)
                throw new ArgumentException("You must be at least 18!"); 
        }

        public static void InvalidEmail(this IGuardClause guard, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email address cannot be empty.");

            const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
                throw new ArgumentException("Invalid email address format.");

            if (email.Length > 254)
                throw new ArgumentException("Email address cannot exceed 254 characters.");
        }

        public static void InvalidPassword(this IGuardClause guard, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long.");

            if (password.Length > 64)
                throw new ArgumentException("Password cannot exceed 64 characters.");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                throw new ArgumentException("Password must contain at least one uppercase letter.");

            if (!Regex.IsMatch(password, @"[a-z]"))
                throw new ArgumentException("Password must contain at least one lowercase letter.");

            if (!Regex.IsMatch(password, @"[0-9]"))
                throw new ArgumentException("Password must contain at least one number.");
        }

        public static void PasswordsDoNotMatch(this IGuardClause guard, string password, string confirmPassword)
        {
            if (password != confirmPassword)
                throw new ArgumentException("Passwords do not match.");
        }
    }
}
