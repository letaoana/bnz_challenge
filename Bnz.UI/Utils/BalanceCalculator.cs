namespace Bnz.UI.Utils
{
    public static class BalanceCalculator
    {
        public static double SubtractFromBalance(string balance, double subtrahend) => double.Parse(balance) - subtrahend;

        public static double AddToBalance(string balance, double addend) => double.Parse(balance) + addend;
    }
}