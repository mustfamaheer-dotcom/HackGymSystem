namespace Gym.Shared.Common;

public static class Guard
{
    public static void AgainstNull<T>(T value, string parameterName)
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);
    }

    public static void AgainstEmptyString(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{parameterName} cannot be empty.", parameterName);
    }

    public static void AgainstNegativeOrZero(int value, string parameterName)
    {
        if (value <= 0)
            throw new ArgumentException($"{parameterName} must be greater than zero.", parameterName);
    }

    public static void AgainstNegative(decimal value, string parameterName)
    {
        if (value < 0)
            throw new ArgumentException($"{parameterName} cannot be negative.", parameterName);
    }

    public static void AgainstInvalidEmail(string? email, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(email)) return;
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email)
                throw new ArgumentException($"{parameterName} is not a valid email.", parameterName);
        }
        catch
        {
            throw new ArgumentException($"{parameterName} is not a valid email.", parameterName);
        }
    }
}
