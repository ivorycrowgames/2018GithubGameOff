using System;

namespace IvoryCrow.Extensions
{
    public static class GuidExtensions
    {
        public static bool IsEmpty(Guid guid)
        {
            return guid == Guid.Empty;
        }

        public static string ToB64String(Guid guid)
        {
            return System.Convert.ToBase64String(guid.ToByteArray());
        }

        public static Guid FromB64String(string value)
        {
            return new Guid(System.Convert.FromBase64String(value));
        }
    }
}