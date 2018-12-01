using System;
using System.Text;

namespace IvoryCrow.Utilities
{
    public static class Throw
    {
        public static void Error(string message)
        {
            throw new ArgumentNullException(message);
        }

        public static void IfNull<T>(T var, string message)
        {
            If((var == null), new ArgumentNullException(message));
        }

        public static void IfNotNull<T>(T var, string message)
        {
            If((var != null), new ArgumentNullException(message));
        }

        public static void IfTrue(bool condition, string message)
        {
            If(condition, new ArgumentException(message));
        }

        public static void IfFalse(bool condition, string message)
        {
            If(!(condition), new ArgumentException(message));
        }

        public static void IfNullOrEmpty(string str, string message)
        {
            If(string.IsNullOrEmpty(str), new ArgumentException(message));
        }

        public static void IfNullOrEmpty(Guid guid, string message)
        {
            If(guid.Equals(Guid.Empty), new ArgumentException(message));
        }

        private static void If<ExceptionType>(bool condition, ExceptionType exception) where ExceptionType : Exception
        {
            if (condition)
            {
                throw exception;
            }
        }
    }
}
