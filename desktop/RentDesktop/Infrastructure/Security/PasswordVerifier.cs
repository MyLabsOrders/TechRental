using System.Linq;

namespace RentDesktop.Infrastructure.Security
{
    internal class PasswordVerifier
    {
        public const int STRONG_PASSWORD_MIN_LEGTH = 6;
        public const int STRONG_PASSWORD_MIN_UNIQUE_CHARS_COUNT = 1;

        public static string STRONG_PASSWORD_REQUIREMENTS = "Пароль должен содержать минимум " +
            $"{STRONG_PASSWORD_MIN_LEGTH} символов, минимум {STRONG_PASSWORD_MIN_UNIQUE_CHARS_COUNT} " +
            $"уникальный(-ых) символ(-ов), а также должен содержать хотя бы один символ в нижнем " +
            $"регистре, хотя бы один символ в верхнем регистре, хотя бы одну цифру и хотя бы один " +
            $"символ, не являющийся цифрой и буквой.";

        public PasswordVerifier(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public int Length => Text.Length;
        public int UniqueCharsCount => Text.ToCharArray().Distinct().Count();

        public bool ContainsLowerCase => Text.Any(t => char.IsLower(t));
        public bool ContainsUpperCase => Text.Any(t => char.IsUpper(t));
        public bool ContainsDigit => Text.Any(t => char.IsDigit(t));
        public bool ContainsNonAlphanumeric => Text.Any(t => !char.IsLetterOrDigit(t));

        public bool IsStrong()
        {
            return Length >= STRONG_PASSWORD_MIN_LEGTH
                && UniqueCharsCount >= STRONG_PASSWORD_MIN_UNIQUE_CHARS_COUNT
                && ContainsLowerCase
                && ContainsUpperCase
                && ContainsDigit
                && ContainsNonAlphanumeric;
        }
    }
}
