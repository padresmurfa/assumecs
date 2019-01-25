using System;

namespace BrokenArrow
{
    public class Assume
    {
        public static void Fail(string message = null)
        {
            throw new AssumptionFailure(message ?? "Assumption failure: Explicit failure was triggered");
        }
        
        public static void Unreachable(string message = null)
        {
            throw new AssumptionFailure(message ?? "Assumption failure: This code was supposed to be unreachable");
        }
        
        public static void IsTrue(bool value, string message = null)
        {
            if (value != true)
            {
                throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{value}' to be true, but was false");
            }
        }
        
        public static void IsTrue(bool? value, string message = null)
        {
            if (value == null || value != true)
            {
                throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{value}' to be true, but was false");
            }
        }
        
        public static void IsFalse(bool? value, string message = null)
        {
            if (value == null || value != false)
            {
                throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{value}' to be false, but was true");
            }
        }

        public static void IsFalse(bool value, string message = null)
        {
            if (value != false)
            {
                throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{value}' to be false, but was true");
            }
        }
        
        public static void AreEqual<T>(IEquatable<T> expected, IEquatable<T> actual, string message = null)
        {
            if (expected != null && actual != null)
            {
                if (!expected.Equals(actual))
                {
                    throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{actual}' to be equal to {actual}");
                }
            }
            else if (expected != null || actual != null)
            {
                throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{actual}' to be equal to {actual}");
            }
        }

        public static void AreEqual<T>(T? expected, T? actual, string message = null)
            where T : struct, IEquatable<T>
        {
            if (expected != null && actual != null)
            {
                if (!expected.Equals(actual))
                {
                    throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{actual}' to be equal to {actual}");
                }
            }
            else if (expected != null || actual != null)
            {
                throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{actual}' to be equal to {actual}");
            }
        }
        
        public static void AreNotEqual<T>(IEquatable<T> expected, IEquatable<T> actual, string message = null)
        {
            if (expected == null && actual == null)
            {
                return;
            }
            
            if (expected != null && actual != null)
            {
                if (expected.Equals(actual))
                {
                    throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{actual}' to not be equal to {actual}");
                }
            }
            else if (expected == null && actual == null)
            {
                throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{actual}' to not be equal to {actual}");
            }
        }
        
        public static void AreNotEqual<T>(T? expected, T? actual, string message = null)
            where T : struct, IEquatable<T>
        {
            if (expected == null && actual == null)
            {
                return;
            }
            
            if (expected != null && actual != null)
            {
                if (expected.Equals(actual))
                {
                    throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{actual}' to not be equal to {actual}");
                }
            }
            else if (expected == null && actual == null)
            {
                throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{actual}' to not be equal to {actual}");
            }
        }
        
        public static void IsNull<T>(Nullable<T> value, string message = null) where T : struct
        {
            if (value.HasValue)
            {
                throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{value}' to be null");
            }
        }
        
        public static void IsNotNull<T>(Nullable<T> value, string message = null) where T : struct
        {
            if (!value.HasValue)
            {
                throw new AssumptionFailure(message ?? $"Assumption failure: Expected '{value}' to not be null");
            }
        }
    }
}
