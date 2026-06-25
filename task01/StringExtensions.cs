namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        string lower = input.ToLower();
        string clean = "";
        foreach (char c in lower)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            if (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))
            {
                clean += c;
            }
        }

        char[] cMassiv = clean.ToCharArray();
        Array.Reverse(cMassiv);
        string revers = new string(cMassiv);

        return clean == revers;
    }
}
