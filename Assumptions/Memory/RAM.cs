using System;

namespace Assumptions.Memory
{
    public class RAM : IComparable<RAM>, IEquatable<RAM>, IComparable
    {
        private UInt64 bytes;

        public RAM(UInt64 bytes = 0)
        {
            this.bytes = bytes;
        }

        public RAM(int bytes = 0)
            : this((UInt64) bytes)
        {
            if (bytes < 0)
            {
                throw new ArgumentException(nameof(bytes));
            }
        }

        public RAM(long bytes = 0)
            : this((UInt64) bytes)
        {
            if (bytes < 0)
            {
                throw new ArgumentException(nameof(bytes));
            }
        }

        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is RAM))
            {
                return -1;
            }

            return CompareTo((RAM) obj);
        }
        
        public int CompareTo(RAM other)
        {
            return bytes.CompareTo(other.bytes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RAM) obj);
        }

        public override int GetHashCode()
        {
            return bytes.GetHashCode();
        }

        private const UInt64 kilo = (UInt64) (1L << 10);
        private const UInt64 mega = (UInt64) (1L << 20);
        private const UInt64 giga = (UInt64) (1L << 30);
        private const UInt64 tera = (UInt64) (1L << 40);
        private const UInt64 peta = (UInt64) (1L << 50);

        public static RAM FromBytes(UInt64 bytes)
        {
            return new RAM(bytes);
        }

        public static RAM FromKilobytes(UInt64 kilobytes)
        {
            return new RAM(kilobytes * kilo);
        }

        public static RAM FromMegabytes(UInt64 megabytes)
        {
            return new RAM(megabytes * mega);
        }

        public static RAM FromGigabytes(UInt64 gigabytes)
        {
            return new RAM(gigabytes * giga);
        }

        public static RAM FromTerabytes(UInt64 terabytes)
        {
            return new RAM(terabytes * tera);
        }

        public static RAM FromPetabytes(UInt64 petabytes)
        {
            return new RAM(petabytes * peta);
        }

        public UInt64 ToInt()
        {
            return bytes;
        }

        public static implicit operator RAM(int bytes)
        {
            return new RAM(bytes);
        }

        public static implicit operator RAM(uint bytes)
        {
            return new RAM(bytes);
        }

        public static implicit operator RAM(long bytes)
        {
            return new RAM(bytes);
        }

        public static implicit operator RAM(ulong bytes)
        {
            return new RAM(bytes);
        }

        public static implicit operator UInt64(RAM bytes)
        {
            return bytes.bytes;
        }

        public static RAM operator +(RAM b, RAM c)
        {
            return new RAM(b.bytes + c.bytes);
        }

        public static RAM operator -(RAM b, RAM c)
        {
            return new RAM(b.bytes - c.bytes);
        }

        public static bool operator ==(RAM lhs, RAM rhs)
        {
            return lhs.bytes == rhs.bytes;
        }

        public static bool operator !=(RAM lhs, RAM rhs)
        {
            return lhs.bytes != rhs.bytes;
        }

        public static bool operator <(RAM lhs, RAM rhs)
        {
            return lhs.bytes < rhs.bytes;
        }

        public static bool operator >(RAM lhs, RAM rhs)
        {
            return lhs.bytes > rhs.bytes;
        }

        public static bool operator <=(RAM lhs, RAM rhs)
        {
            return lhs.bytes <= rhs.bytes;
        }

        public static bool operator >=(RAM lhs, RAM rhs)
        {
            return lhs.bytes >= rhs.bytes;
        }

        public string ToKilobytes()
        {
            if (bytes >= kilo && (bytes % kilo) != 0L)
            {
                return $"{bytes / kilo} Kb";
            }

            return null;
        }

        public string ToMegabytes()
        {
            if (bytes >= mega && (bytes % mega) != 0L)
            {
                return $"{bytes / mega} Mb";
            }

            return null;
        }

        public string ToGigabytes()
        {
            if (bytes >= giga && (bytes % giga) != 0L)
            {
                return $"{bytes / giga} Gb";
            }

            return null;
        }

        public string ToTerabytes()
        {
            if (bytes >= tera && (bytes % tera) != 0L)
            {
                return $"{bytes / tera} Tb";
            }

            return null;
        }

        public string ToPetabytes()
        {
            if (bytes >= peta && (bytes % peta) != 0L)
            {
                return $"{bytes / peta} Pb";
            }

            return null;
        }

        public override string ToString()
        {
            return ToPetabytes() ??
                   ToTerabytes() ?? ToGigabytes() ?? ToMegabytes() ?? ToKilobytes() ?? $"{bytes} bytes";
        }


        public bool Equals(RAM other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return bytes == other.bytes;
        }
    }
}