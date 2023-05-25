/* Culumi256 v1 - pseudorandom number generator

To the extent possible under law, the author has waived all copyright
and related or neighboring rights to this software.
See: https://creativecommons.org/publicdomain/zero/1.0/
*/

namespace RngLab.Rng.Generators;

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;

/// <summary>
/// Culumi pseudorandom number generator
/// </summary>
public sealed partial class Culumi
{
    /// <summary>
    /// Internal state
    /// </summary>
    private Vector128<ulong> _v0, _v1;

    /// <summary>
    /// Initializes the internal state using CSPRNG.
    /// It is highly recommended to initialize with this method.
    /// </summary>
    public Culumi()
    {
        var buffer = (stackalloc Vector128<ulong>[2]);
        do
        {
            RandomNumberGenerator.Fill(MemoryMarshal.AsBytes(buffer));
        } while (buffer[0] == Vector128<ulong>.Zero && buffer[1] == Vector128<ulong>.Zero);

        _v0 = buffer[0];
        _v1 = buffer[1];
    }

    /// <summary>
    /// Initializes the internal state with user-specified states.
    /// Useful when deserializing.
    /// </summary>
    /// <exception cref="ArgumentException">v0 and v1 are all zero.</exception>
    public Culumi(Vector128<ulong> v0, Vector128<ulong> v1)
    {
        if (v0 == Vector128<ulong>.Zero && v1 == Vector128<ulong>.Zero)
            ThrowArgumentZero();

        _v0 = v0;
        _v1 = v1;
    }

    /// <summary>
    /// Generates a random number.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<ulong> Next()
    {
        Vector128<ulong> v0 = _v0, v1 = _v1;
        var result = Vector128.AsUInt64(Vector128.Shuffle(Vector128.AsByte(v0 + v1), Vector128.Create(0x0100030205040706, 0x09080b0a0d0c0f0e).AsByte())) + v1;

        Vector128<ulong> clmul;
        if (Pclmulqdq.IsSupported)      // This branch will be eliminated by JIT
            clmul = Pclmulqdq.CarrylessMultiply(v0, Vector128.Create(0xBBC1B31A6451A582, 0), 0x00);
        else if (System.Runtime.Intrinsics.Arm.Aes.IsSupported)
            clmul = System.Runtime.Intrinsics.Arm.Aes.PolynomialMultiplyWideningLower(Vector64.Create(v0[0]), Vector64.Create(0xBBC1B31A6451A582));
        else
            clmul = CarrylessMultiplyFallback(v0, Vector128.Create(0xBBC1B31A6451A582, 0), 0x00);

        _v0 = Vector128.Shuffle(v0 ^ v1, Vector128.Create(1ul, 0));
        _v1 = v0 ^ clmul;

        return result;
    }

    /// <summary>
    /// If your environment doesn't support <see cref="Pclmulqdq"/>, you can use this instead.
    /// However, it will be very slow.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<ulong> CarrylessMultiplyFallback(Vector128<ulong> a, Vector128<ulong> b, byte control)
    {
        ulong x = (control & 0x1) != 0 ? a[1] : a[0];
        ulong y = (control & 0x10) != 0 ? b[1] : b[0];

        ulong lo = 0;
        ulong hi = 0;

        do
        {
            hi ^= Math.BigMul(x, y & (0 - y), out var tlo);
            lo ^= tlo;
            y &= y - 1;
        } while (y != 0);

        return Vector128.Create(lo, hi);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector128<ulong> NextEmbedded(ref Vector128<ulong> _v0, ref Vector128<ulong> _v1)
    {
        Vector128<ulong> v0 = _v0, v1 = _v1;
        var result = Vector128.AsUInt64(Vector128.Shuffle(Vector128.AsByte(v0 + v1), Vector128.Create(0x0100030205040706, 0x09080b0a0d0c0f0e).AsByte())) + v1;

        Vector128<ulong> clmul;
        if (Pclmulqdq.IsSupported)
            clmul = Pclmulqdq.CarrylessMultiply(v0, Vector128.Create(0xBBC1B31A6451A582, 0), 0x00);
        else if (System.Runtime.Intrinsics.Arm.Aes.IsSupported)
            clmul = System.Runtime.Intrinsics.Arm.Aes.PolynomialMultiplyWideningLower(Vector64.Create(v0[0]), Vector64.Create(0xBBC1B31A6451A582));
        else
            clmul = CarrylessMultiplyFallback(v0, Vector128.Create(0xBBC1B31A6451A582, 0), 0x00);

        _v0 = Vector128.Shuffle(v0 ^ v1, Vector128.Create(1ul, 0));
        _v1 = v0 ^ clmul;

        return result;
    }


    private void JumpByPolynomial(ReadOnlySpan<ulong> polynomial)
    {
        Vector128<ulong> v0 = _v0, v1 = _v1;

        var jump0 = Vector128<ulong>.Zero;
        var jump1 = Vector128<ulong>.Zero;

        for (int i = 0; i < polynomial.Length; i++)
        {
            for (int b = 0; b < 64; b++)
            {
                if ((polynomial[i] & 1ul << b) != 0)
                {
                    jump0 ^= v0;
                    jump1 ^= v1;
                }
                NextEmbedded(ref v0, ref v1);
            }
        }

        _v0 = jump0;
        _v1 = jump1;
    }

    /// <summary>
    /// Equivalent to 2^64 next() calls.
    /// </summary>
    public void Jump64()
    {
        JumpByPolynomial(stackalloc ulong[] { 0x5601375EC36230E1, 0x79CF0DE79B070769, 0x51407AE5A16EA33B, 0x708C91D747D77FE3 });
    }

    /// <summary>
    /// Equivalent to 2^128 next() calls.
    /// </summary>
    public void Jump128()
    {
        JumpByPolynomial(stackalloc ulong[] { 0x6C81827A1CBDFCCF, 0x7E438EDA9627E879, 0x15123909CF74EB17, 0xA7C9C89160D05C3E });
    }

    /// <summary>
    /// Equivalent to 2^192 next() calls.
    /// </summary>
    public void Jump192()
    {
        JumpByPolynomial(stackalloc ulong[] { 0xE03ABAC0D7F32901, 0x176EBE5A39A97EE5, 0x92B41C08DDEE8EAE, 0x9C1C03167238346D });
    }

    /// <summary>
    /// Creates a new <see cref="Culumi"/> instance from the current state.
    /// The sequence of child instances is uncorrelated with the parent.
    /// Useful when parallelizing.
    /// </summary>
    /// <remarks>
    /// DON'T call <see cref="Split"/> from child instances.
    /// Correlation occurs between parent and child.
    /// </remarks>
    public Culumi Split()
    {
        var copy = new Culumi(_v0, _v1);
        Jump128();
        return copy;
    }

    /// <summary>
    /// Inverse function of <see cref="Next"/>.
    /// </summary>
    public void Prev()
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Vector128<ulong> CarrylessMultiply(Vector128<ulong> a, ulong b)
        {
            if (Pclmulqdq.IsSupported)
                return Pclmulqdq.CarrylessMultiply(a, Vector128.Create(b, 0), 0x00);
            else if (System.Runtime.Intrinsics.Arm.Aes.IsSupported)
                return System.Runtime.Intrinsics.Arm.Aes.PolynomialMultiplyWideningLower(Vector64.Create(a[0]), Vector64.Create(b));
            else
                return CarrylessMultiplyFallback(a, Vector128.Create(b, 0), 0x00);
        }


        var p0l = CarrylessMultiply(_v1, 0x4D12E2CABE3FB47F);
        var pcl = CarrylessMultiply(p0l, 0xBBC1B31A6451A582);

        var p0 = _v1 ^ pcl;
        var p1 = p0 ^ Vector128.Shuffle(_v0, Vector128.Create(1ul, 0ul));

        _v0 = p0;
        _v1 = p1;
    }


    /// <summary>
    /// Gets the internal state.
    /// Useful when serializing.
    /// </summary>
    public (Vector128<ulong>, Vector128<ulong>) GetState()
        => (_v0, _v1);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ulong NextRange(ulong range)
    {
        var r = Next();
        ulong hi = Math.BigMul(r[0], range, out ulong lo);

        if (lo < 0ul - range)
            return hi;

        ulong thi = Math.BigMul(r[1], range, out ulong tlo);
        ulong sum = lo + thi;
        ulong carry = sum < thi ? 1ul : 0ul;

        if (sum != ~0ul)
            return hi + carry;

        return hi + Fallback(this, range, tlo);


        [MethodImpl(MethodImplOptions.NoInlining)]
        static ulong Fallback(Culumi _rng, ulong range, ulong lo)
        {
            while (true)
            {
                var r = _rng.Next();

                ulong thi = Math.BigMul(r[0], range, out ulong tlo);
                ulong sum = lo + thi;
                ulong carry = sum < thi ? 1ul : 0ul;

                if (sum != ~0ul)
                    return carry;

                lo = tlo;


                thi = Math.BigMul(r[1], range, out tlo);
                sum = lo + thi;
                carry = sum < thi ? 1ul : 0ul;

                if (sum != ~0ul)
                    return carry;

                lo = tlo;
            }
        }
    }

    /// <summary>
    /// Generates a random number in the range [0, max) .
    /// </summary>
    public ulong NextULong(ulong maxExclusive)
    {
        return NextRange(maxExclusive);
    }

    /// <summary>
    /// Generates a random number in the range [min, max) .
    /// </summary>
    public ulong NextULong(ulong minInclusive, ulong maxExclusive)
    {
        if (minInclusive > maxExclusive)
            ThrowMinMax();

        return minInclusive + NextRange(maxExclusive - minInclusive);
    }

    /// <summary>
    /// Generates a random number in the range [0, max) .
    /// </summary>
    public long NextLong(long maxExclusive)
    {
        if (maxExclusive < 0)
            ThrowMaxMinus();

        return (long)NextRange((ulong)maxExclusive);
    }

    /// <summary>
    /// Generates a random number in the range [min, max) .
    /// </summary>
    public long NextLong(long minInclusive, long maxExclusive)
    {
        if (minInclusive > maxExclusive)
            ThrowMinMax();

        return minInclusive + (long)NextRange((ulong)(maxExclusive - minInclusive));
    }

    /// <summary>
    /// Generates 2 random numbers in the range [0, max) .
    /// </summary>
    public Vector128<ulong> NextULong2(Vector128<ulong> maxExclusive2)
    {
        var r = Next();

        var hi = Vector128.Create(
            Math.BigMul(r[0], maxExclusive2[0], out ulong lo0),
            Math.BigMul(r[1], maxExclusive2[1], out ulong lo1));
        var lo = Vector128.Create(lo0, lo1);

        if (Vector128.LessThanOrEqualAll(lo, Vector128.Negate(maxExclusive2)))
            return hi;

        r = Next();
        var thi = Vector128.Create(
            Math.BigMul(r[0], maxExclusive2[0], out lo0),
            Math.BigMul(r[1], maxExclusive2[1], out lo1));
        var tlo = Vector128.Create(lo0, lo1);

        var sum = lo + thi;
        var carry = Vector128.Negate(Vector128.LessThan(sum, thi));
        hi += carry;

        var mask = Vector128.Equals(sum, Vector128<ulong>.AllBitsSet);
        if (Vector128.ExtractMostSignificantBits(mask) == 0)
            return hi;

        return hi + Fallback(this, maxExclusive2, mask, tlo);

        [MethodImpl(MethodImplOptions.NoInlining)]
        static Vector128<ulong> Fallback(Culumi _rng, Vector128<ulong> max, Vector128<ulong> mask, Vector128<ulong> lo)
        {
            while (true)
            {
                var r = _rng.Next();
                var thi = Vector128.Create(
                    Math.BigMul(r[0], max[0], out ulong lo0),
                    Math.BigMul(r[1], max[1], out ulong lo1));
                var tlo = Vector128.Create(lo0, lo1);

                var sum = lo + thi;
                var carry = Vector128.BitwiseAnd(Vector128.Negate(Vector128.LessThan(sum, thi)), mask);
                mask = Vector128.BitwiseAnd(Vector128.Equals(sum, Vector128<ulong>.AllBitsSet), mask);

                if (Vector128.ExtractMostSignificantBits(mask) == 0)
                    return carry;

                lo = tlo;
            }
        }
    }

    /// <summary>
    /// Generates 2 random numbers in the range [min, max) .
    /// </summary>
    public Vector128<ulong> NextULong2(Vector128<ulong> minInclusive2, Vector128<ulong> maxExclusive2)
    {
        if (Vector128.GreaterThanAny(minInclusive2, maxExclusive2))
            ThrowMinMax();

        return minInclusive2 + NextULong2(maxExclusive2 - minInclusive2);
    }

    /// <summary>
    /// Generates 2 random numbers in the range [0, max) .
    /// </summary>
    public Vector128<ulong> NextULong2(ulong maxExclusive)
    {
        return NextULong2(Vector128.Create(maxExclusive, maxExclusive));
    }

    /// <summary>
    /// Generates 2 random numbers in the range [min, max) .
    /// </summary>
    public Vector128<ulong> NextULong2(ulong minInclusive, ulong maxExclusive)
    {
        if (minInclusive > maxExclusive)
            ThrowMinMax();

        var min2 = Vector128.Create(minInclusive, minInclusive);
        var max2 = Vector128.Create(maxExclusive, maxExclusive);

        return min2 + NextULong2(max2 - min2);
    }

    /// <summary>
    /// Generates 2 random numbers in the range [0, max) .
    /// </summary>
    public Vector128<long> NextLong2(Vector128<long> maxExclusive2)
    {
        if (Vector128.LessThanAny(maxExclusive2, Vector128<long>.Zero))
            ThrowMaxMinus();

        return NextULong2(maxExclusive2.AsUInt64()).AsInt64();
    }

    /// <summary>
    /// Generates 2 random numbers in the range [min, max) .
    /// </summary>
    public Vector128<long> NextLong2(Vector128<long> minInclusive2, Vector128<long> maxExclusive2)
    {
        if (Vector128.GreaterThanAny(minInclusive2, maxExclusive2))
            ThrowMinMax();

        return minInclusive2 + NextULong2((maxExclusive2 - minInclusive2).AsUInt64()).AsInt64();
    }

    /// <summary>
    /// Generates 2 random numbers in the range [0, max) .
    /// </summary>
    public Vector128<long> NextLong2(long maxExclusive)
    {
        if (maxExclusive < 0)
            ThrowMaxMinus();

        return NextULong2(Vector128.Create(maxExclusive, maxExclusive).AsUInt64()).AsInt64();
    }

    /// <summary>
    /// Generates 2 random numbers in the range [min, max) .
    /// </summary>
    public Vector128<long> NextLong2(long minInclusive, long maxExclusive)
    {
        if (minInclusive > maxExclusive)
            ThrowMinMax();

        var min2 = Vector128.Create(minInclusive, minInclusive);
        var max2 = Vector128.Create(maxExclusive, maxExclusive);

        return min2 + NextULong2((max2 - min2).AsUInt64()).AsInt64();
    }

    /// <summary>
    /// Generates a random number in the range [0, max) .
    /// </summary>
    public uint NextUInt(uint maxExclusive)
    {
        return (uint)NextRange(maxExclusive);
    }

    /// <summary>
    /// Generates a random number in the range [min, max) .
    /// </summary>
    public uint NextUInt(uint minInclusive, uint maxExclusive)
    {
        if (minInclusive > maxExclusive)
            ThrowMinMax();

        return minInclusive + (uint)NextRange(maxExclusive - minInclusive);
    }

    /// <summary>
    /// Generates 4 random numbers in the range [0, max) .
    /// </summary>
    public Vector128<uint> NextUInt4(Vector128<uint> maxExclusive4)
    {
        var r = Next().AsUInt32();

        var mulFromLo = Sse2.Multiply(Vector128.Shuffle(r, Vector128.Create(1u, 0, 3, 2)), maxExclusive4).AsUInt32();
        var mulFromHi = Sse2.Multiply(r, Vector128.Shuffle(maxExclusive4, Vector128.Create(1u, 0, 3, 2))).AsUInt32();

        mulFromLo = Vector128.Shuffle(mulFromLo.AsUInt32(), Vector128.Create(0u, 2, 1, 3));
        mulFromHi = Vector128.Shuffle(mulFromHi.AsUInt32(), Vector128.Create(0u, 2, 1, 3));

        var mulLo = Sse2.UnpackLow(mulFromLo, mulFromHi);
        var mulHi = Sse2.UnpackHigh(mulFromLo, mulFromHi);

        var failedMask = Vector128.GreaterThan(mulLo, Vector128.Negate(maxExclusive4));
        if (Vector128.ExtractMostSignificantBits(failedMask) == 0)
            return mulHi;

        while (true)
        {
            r = Next().AsUInt32();

            mulFromLo = Sse2.Multiply(Vector128.Shuffle(r, Vector128.Create(1u, 0, 3, 2)), maxExclusive4).AsUInt32();
            mulFromHi = Sse2.Multiply(r, Vector128.Shuffle(maxExclusive4, Vector128.Create(1u, 0, 3, 2))).AsUInt32();

            mulFromLo = Vector128.Shuffle(mulFromLo.AsUInt32(), Vector128.Create(0u, 2, 1, 3));
            mulFromHi = Vector128.Shuffle(mulFromHi.AsUInt32(), Vector128.Create(0u, 2, 1, 3));

            var mulTLo = Sse2.UnpackLow(mulFromLo, mulFromHi);
            var mulTHi = Sse2.UnpackHigh(mulFromLo, mulFromHi);

            var sum = Vector128.Add(mulLo, mulTHi);
            var carry = Vector128.Negate(Vector128.LessThan(sum, mulTHi));

            mulHi = Vector128.Add(mulHi, Vector128.BitwiseAnd(failedMask, carry));

            failedMask = Vector128.Equals(Vector128.BitwiseAnd(sum, failedMask), Vector128<uint>.AllBitsSet);
            if (Vector128.ExtractMostSignificantBits(failedMask) == 0)
            {
                return mulHi;
            }

            mulLo = mulTLo;
        }
    }

    /// <summary>
    /// Generates 4 random numbers in the range [0, max) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<uint> NextUInt4(uint maxExclusive)
    {
        return NextUInt4(Vector128.Create(maxExclusive, maxExclusive, maxExclusive, maxExclusive));
    }

    /// <summary>
    /// Generates 4 random numbers in the range [min, max) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<uint> NextUInt4(Vector128<uint> minInclusive4, Vector128<uint> maxExclusive4)
    {
        if (Vector128.GreaterThanAny(minInclusive4, maxExclusive4))
            ThrowMinMax();

        return Vector128.Add(minInclusive4, NextUInt4(Vector128.Subtract(maxExclusive4, minInclusive4)));
    }

    /// <summary>
    /// Generates 4 random numbers in the range [min, max) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<uint> NextUInt4(uint minInclusive, uint maxExclusive)
    {
        if (minInclusive > maxExclusive)
            ThrowMinMax();

        var min4 = Vector128.Create(minInclusive, minInclusive, minInclusive, minInclusive);
        var max4 = Vector128.Create(maxExclusive, maxExclusive, maxExclusive, maxExclusive);

        return Vector128.Add(min4, NextUInt4(Vector128.Subtract(max4, min4)));
    }

    /// <summary>
    /// Generates a random number in the range [0, max) .
    /// </summary>
    public int NextInt(int maxExclusive)
    {
        if (maxExclusive < 0)
            ThrowMaxMinus();

        return (int)NextRange((ulong)maxExclusive);
    }

    /// <summary>
    /// Generates a random number in the range [min, max) .
    /// </summary>
    public int NextInt(int minInclusive, int maxExclusive)
    {
        if (minInclusive > maxExclusive)
            ThrowMinMax();

        return minInclusive + (int)NextRange((ulong)(maxExclusive - minInclusive));
    }

    /// <summary>
    /// Generates 4 random numbers in the range [0, max) .
    /// </summary>
    public Vector128<int> NextInt4(Vector128<int> maxExclusive4)
    {
        if (Vector128.LessThanAny(maxExclusive4, Vector128<int>.Zero))
            ThrowMaxMinus();

        return NextUInt4(maxExclusive4.AsUInt32()).AsInt32();
    }

    /// <summary>
    /// Generates 4 random numbers in the range [0, max) .
    /// </summary>
    public Vector128<int> NextInt4(int maxExclusive)
    {
        if (maxExclusive < 0)
            ThrowMaxMinus();

        return NextUInt4(Vector128.Create((uint)maxExclusive, (uint)maxExclusive, (uint)maxExclusive, (uint)maxExclusive)).AsInt32();
    }

    /// <summary>
    /// Generates 4 random numbers in the range [min, max) .
    /// </summary>
    public Vector128<int> NextInt4(Vector128<int> minInclusive4, Vector128<int> maxExclusive4)
    {
        if (Vector128.LessThanAny(maxExclusive4, minInclusive4))
            ThrowMinMax();

        return Vector128.Add(minInclusive4, NextUInt4(Vector128.Subtract(maxExclusive4, minInclusive4).AsUInt32()).AsInt32());
    }

    /// <summary>
    /// Generates 4 random numbers in the range [min, max) .
    /// </summary>
    public Vector128<int> NextInt4(int minInclusive, int maxExclusive)
    {
        if (maxExclusive < minInclusive)
            ThrowMinMax();

        var min4 = Vector128.Create(minInclusive, minInclusive, minInclusive, minInclusive);
        var max4 = Vector128.Create(maxExclusive, maxExclusive, maxExclusive, maxExclusive);

        return Vector128.Add(min4, NextUInt4(Vector128.Subtract(max4, min4).AsUInt32()).AsInt32());
    }


    /// <summary>
    /// Generates 2 random numbers in the range [0, 1) .
    /// </summary>
    public Vector128<double> NextDouble2()
    {
        var zeroone = Vector128.ConvertToDouble(Vector128.ShiftRightLogical(Next(), 11))
            * Vector128.Create(1.0 / (1ul << 53), 1.0 / (1ul << 53));
        return zeroone;
    }

    /// <summary>
    /// Generates 2 random numbers in the range [-1, 1) .
    /// </summary>
    public Vector128<double> NextSignedDouble2()
    {
        var zeroone = Vector128.ConvertToDouble(Vector128.ShiftRightArithmetic(Vector128.AsInt64(Next()), 10))
            * Vector128.Create(1.0 / (1ul << 53), 1.0 / (1ul << 53));
        return zeroone;
    }

    /// <summary>
    /// Generates 2 random numbers in the range [0, max) .
    /// </summary>
    public Vector128<double> NextDouble2(Vector128<double> max2)
    {
        if (!Vector128.LessThanAll(Vector128<double>.Zero, max2))
            ThrowMaxMinus();

        var zeroone = Vector128.ConvertToDouble(Vector128.ShiftRightLogical(Next(), 11))
            * Vector128.Create(1.0 / (1ul << 53));
        var result = zeroone * max2;

        if (!Vector128.LessThanAll(result, max2))
            result = Fallback(this, result, max2);

        return result;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static Vector128<double> Fallback(Culumi rng, in Vector128<double> prevResult, in Vector128<double> max2)
        {
            Vector128<double> result = prevResult;

            if (Vector128.EqualsAny(max2, Vector128.Create(double.PositiveInfinity)))
                ThrowMaxIsNotFinite();

            var successMask = Vector128.LessThan(result, max2);

            while (Vector128.ExtractMostSignificantBits(successMask) != 0x3)
            {
                var zeroone = Vector128.ConvertToDouble(Vector128.ShiftRightLogical(rng.Next(), 11))
                    * Vector128.Create(1.0 / (1ul << 53), 1.0 / (1ul << 53));

                var newResult = zeroone * max2;

                var updateFlag = Vector128.OnesComplement(Vector128.BitwiseOr(Vector128.GreaterThanOrEqual(newResult, max2), successMask));
                result = Vector128.Xor(Vector128.BitwiseAnd(newResult, updateFlag), Vector128.AndNot(result, updateFlag));
                successMask = Vector128.BitwiseOr(successMask, updateFlag);
            }
            return result;
        }
    }

    /// <summary>
    /// Generates 2 random numbers in the range [0, max) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<double> NextDouble2(double max)
        => NextDouble2(Vector128.Create(max, max));

    /// <summary>
    /// Generates 2 random numbers in the range [min, max) .
    /// </summary>
    public Vector128<double> NextDouble2(Vector128<double> min2, Vector128<double> max2)
    {
        if (!Vector128.LessThanAll(min2, max2))
            ThrowMinMax();

        var zeroone = Vector128.ConvertToDouble(Vector128.ShiftRightLogical(Next(), 11))
            * Vector128.Create(1.0 / (1ul << 53), 1.0 / (1ul << 53));

        var result = Fma.IsSupported ?
            Fma.MultiplyAdd(zeroone, max2 - min2, min2) :
            Vector128.Create(
                Math.FusedMultiplyAdd(zeroone[0], max2[0] - min2[0], min2[0]),
                Math.FusedMultiplyAdd(zeroone[1], max2[1] - min2[1], min2[1]));

        if (!Vector128.LessThanAll(result, max2))
            result = Fallback(this, result, min2, max2);

        return result;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static Vector128<double> Fallback(Culumi rng, in Vector128<double> prevResult, in Vector128<double> min2, in Vector128<double> max2)
        {
            Vector128<double> result = prevResult;

            if (Vector128.EqualsAny(min2, Vector128.Create(double.NegativeInfinity)) ||
                Vector128.EqualsAny(max2, Vector128.Create(double.PositiveInfinity)))
                ThrowMinMax();

            var successMask = Vector128.LessThan(result, max2);

            while (Vector128.ExtractMostSignificantBits(successMask) != 0x3)
            {
                var zeroone = Vector128.ConvertToDouble(Vector128.ShiftRightLogical(rng.Next(), 11))
                    * Vector128.Create(1.0 / (1ul << 53), 1.0 / (1ul << 53));

                var newResult = Fma.IsSupported ?
                    Fma.MultiplyAdd(Vector128.Create(1.0, 1.0) - zeroone, min2, zeroone * max2) :
                    Vector128.Create(
                        Math.FusedMultiplyAdd(1.0 - zeroone[0], min2[0], zeroone[0] * max2[0]),
                        Math.FusedMultiplyAdd(1.0 - zeroone[1], min2[1], zeroone[1] * max2[1]));

                var updateFlag = Vector128.OnesComplement(Vector128.BitwiseOr(Vector128.GreaterThanOrEqual(newResult, max2), successMask));
                result = Vector128.Xor(Vector128.BitwiseAnd(newResult, updateFlag), Vector128.AndNot(result, updateFlag));
                successMask = Vector128.BitwiseOr(successMask, updateFlag);
            }
            return result;
        }
    }

    /// <summary>
    /// Generates 2 random numbers in the range [min, max) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<double> NextDouble2(double min, double max)
        => NextDouble2(Vector128.Create(min, min), Vector128.Create(max, max));

    /// <summary>
    /// Generates a random number in the range [0, 1) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double NextDouble()
        => NextDouble2()[0];

    /// <summary>
    /// Generates a random number in the range [-1, 1) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double NextSignedDouble()
        => NextSignedDouble2()[0];

    /// <summary>
    /// Generates a random number in the range [0, max) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double NextDouble(double max)
        => NextDouble2(max)[0];

    /// <summary>
    /// Generates a random number in the range [min, max) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double NextDouble(double min, double max)
        => NextDouble2(min, max)[0];


    /// <summary>
    /// Generates 4 random numbers in the range [0, 1) .
    /// </summary>
    public Vector128<float> NextFloat4()
    {
        var zeroone = Vector128.ConvertToSingle(Vector128.ShiftRightLogical(Next().AsUInt32(), 8))
            * Vector128.Create(1f / (1u << 24), 1f / (1u << 24), 1f / (1u << 24), 1f / (1u << 24));
        return zeroone;
    }

    /// <summary>
    /// Generates 4 random numbers in the range [-1, 1) .
    /// </summary>
    public Vector128<float> NextSignedFloat4()
    {
        var zeroone = Vector128.ConvertToSingle(Vector128.ShiftRightArithmetic(Next().AsInt32(), 7))
            * Vector128.Create(1f / (1u << 24), 1f / (1u << 24), 1f / (1u << 24), 1f / (1u << 24));
        return zeroone;
    }

    /// <summary>
    /// Generates 4 random numbers in the range [0, max) .
    /// </summary>
    public Vector128<float> NextFloat4(Vector128<float> max4)
    {
        if (!Vector128.LessThanAll(Vector128<float>.Zero, max4))
            ThrowMaxMinus();

        var zeroone = Vector128.ConvertToSingle(Vector128.ShiftRightLogical(Next().AsUInt32(), 8))
            * Vector128.Create(1f / (1u << 24));
        var result = zeroone * max4;

        if (!Vector128.LessThanAll(result, max4))
            result = Fallback(this, result, max4);

        return result;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static Vector128<float> Fallback(Culumi rng, in Vector128<float> prevResult, Vector128<float> max4)
        {
            Vector128<float> result = prevResult;

            if (Vector128.EqualsAny(max4, Vector128.Create(float.PositiveInfinity)))
                ThrowMaxIsNotFinite();

            var successMask = Vector128.LessThan(result, max4);

            while (Vector128.ExtractMostSignificantBits(successMask) != 0xf)
            {
                var zeroone = Vector128.ConvertToSingle(Vector128.ShiftRightLogical(rng.Next().AsUInt32(), 8))
                    * Vector128.Create(1f / (1u << 24));

                var newResult = zeroone * max4;

                var updateFlag = Vector128.OnesComplement(Vector128.BitwiseOr(Vector128.GreaterThanOrEqual(newResult, max4), successMask));
                result = Vector128.Xor(Vector128.BitwiseAnd(newResult, updateFlag), Vector128.AndNot(result, updateFlag));
                successMask = Vector128.BitwiseOr(successMask, updateFlag);
            };
            return result;
        }
    }

    /// <summary>
    /// Generates 4 random numbers in the range [0, max) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<float> NextFloat4(float max)
        => NextFloat4(Vector128.Create(max, max, max, max));

    /// <summary>
    /// Generates 4 random numbers in the range [min, max) .
    /// </summary>
    public Vector128<float> NextFloat4(Vector128<float> min4, Vector128<float> max4)
    {
        if (!Vector128.LessThanAll(min4, max4))
            ThrowMinMax();

        var zeroone = Vector128.ConvertToSingle(Vector128.ShiftRightLogical(Next().AsUInt32(), 8))
            * Vector128.Create(1f / (1u << 24));
        var result = Fma.IsSupported ?
            Fma.MultiplyAdd(zeroone, max4 - min4, min4) :
            Vector128.Create(
                MathF.FusedMultiplyAdd(zeroone[0], max4[0] - min4[0], min4[0]),
                MathF.FusedMultiplyAdd(zeroone[1], max4[1] - min4[1], min4[1]),
                MathF.FusedMultiplyAdd(zeroone[2], max4[2] - min4[2], min4[2]),
                MathF.FusedMultiplyAdd(zeroone[3], max4[3] - min4[3], min4[3]));

        if (!Vector128.LessThanAll(result, max4))
            result = Fallback(this, result, min4, max4);

        return result;

        [MethodImpl(MethodImplOptions.NoInlining)]
        static Vector128<float> Fallback(Culumi rng, in Vector128<float> prevResult, Vector128<float> min4, Vector128<float> max4)
        {
            Vector128<float> result = prevResult;

            if (Vector128.EqualsAny(min4, Vector128.Create(float.NegativeInfinity)) ||
                Vector128.EqualsAny(max4, Vector128.Create(float.PositiveInfinity)))
                ThrowMinMax();

            var successMask = Vector128.LessThan(result, max4);

            while (Vector128.ExtractMostSignificantBits(successMask) != 0xf)
            {
                var zeroone = Vector128.ConvertToSingle(Vector128.ShiftRightLogical(rng.Next().AsUInt32(), 8))
                    * Vector128.Create(1f / (1u << 24));

                var newResult = Fma.IsSupported ?
                    Fma.MultiplyAdd(Vector128.Create(1.0f) - zeroone, min4, zeroone * max4) :
                    Vector128.Create(
                        MathF.FusedMultiplyAdd(1.0f - zeroone[0], min4[0], zeroone[0] * max4[0]),
                        MathF.FusedMultiplyAdd(1.0f - zeroone[1], min4[1], zeroone[1] * max4[1]),
                        MathF.FusedMultiplyAdd(1.0f - zeroone[2], min4[2], zeroone[2] * max4[2]),
                        MathF.FusedMultiplyAdd(1.0f - zeroone[3], min4[3], zeroone[3] * max4[3]));

                var updateFlag = Vector128.OnesComplement(Vector128.BitwiseOr(Vector128.GreaterThanOrEqual(newResult, max4), successMask));
                result = Vector128.Xor(Vector128.BitwiseAnd(newResult, updateFlag), Vector128.AndNot(result, updateFlag));
                successMask = Vector128.BitwiseOr(successMask, updateFlag);
            }
            return result;
        }
    }

    /// <summary>
    /// Generates 4 random numbers in the range [min, max) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector128<float> NextFloat4(float min, float max)
        => NextFloat4(Vector128.Create(min, min, min, min), Vector128.Create(max, max, max, max));

    /// <summary>
    /// Generates a random number in the range [0, 1) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float NextFloat()
        => NextFloat4()[0];

    /// <summary>
    /// Generates a random number in the range [-1, 1) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float NextSignedFloat()
        => NextSignedFloat4()[0];

    /// <summary>
    /// Generates a random number in the range [0, max) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float NextFloat(float max)
        => NextFloat4(Vector128.Create(max, max, max, max))[0];

    /// <summary>
    /// Generates a random number in the range [min, max) .
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float NextFloat(float min, float max)
        => NextFloat4(Vector128.Create(min, min, min, min), Vector128.Create(max, max, max, max))[0];


    /// <summary>
    /// Generates a random number that is true with <paramref name="probability"/>.
    /// </summary>
    public bool NextBool(double probability)
        => (Next()[0] >> 11) * (1.0 / (1ul << 53)) < probability;

    /// <summary>
    /// Generates a random number that is true with (<paramref name="numerator"/> / <paramref name="denominator"/>).
    /// </summary>
    public bool NextBool(int numerator, int denominator)
        => NextInt(denominator) < numerator;

    /// <summary>
    /// Generates 2 random numbers that is true with <paramref name="probability2"/>.
    /// </summary>
    public (bool, bool) NextBool2(Vector128<double> probability2)
    {
        var vec = Vector128.LessThan(NextDouble2(), probability2);
        return (vec[0] != 0, vec[1] != 0);
    }

    /// <summary>
    /// Generates 2 random numbers that is true with <paramref name="probability"/>.
    /// </summary>
    public (bool, bool) NextBool2(double probability)
    {
        var vec = Vector128.LessThan(NextDouble2(), Vector128.Create(probability));
        return (vec[0] != 0, vec[1] != 0);
    }

    /// <summary>
    /// Generates 2 random numbers that is true with (<paramref name="numerator"/> / <paramref name="denominator"/>).
    /// </summary>
    public (bool, bool) NextBool2(Vector128<long> numerator, Vector128<long> denominator)
    {
        var flag = Vector128.LessThan(NextLong2(denominator), numerator);
        return (flag[0] != 0, flag[1] != 0);
    }

    /// <summary>
    /// Generates 2 random numbers that is true with (<paramref name="numerator"/> / <paramref name="denominator"/>).
    /// </summary>
    public (bool, bool) NextBool2(long numerator, long denominator)
    {
        var flag = Vector128.LessThan(NextLong2(denominator), Vector128.Create(numerator));
        return (flag[0] != 0, flag[1] != 0);
    }

    /// <summary>
    /// Generates 4 random numbers that is true with <paramref name="probability4"/>.
    /// </summary>
    public (bool, bool, bool, bool) NextBool4(Vector128<float> probability4)
    {
        var vec = Vector128.LessThan(NextFloat4(), probability4);
        return (vec[0] != 0, vec[1] != 0, vec[2] != 0, vec[3] != 0);
    }

    /// <summary>
    /// Generates 4 random numbers that is true with <paramref name="probability"/>.
    /// </summary>
    public (bool, bool, bool, bool) NextBool4(float probability)
    {
        var vec = Vector128.LessThan(NextFloat4(), Vector128.Create(probability));
        return (vec[0] != 0, vec[1] != 0, vec[2] != 0, vec[3] != 0);
    }

    /// <summary>
    /// Generates 4 random numbers that is true with (<paramref name="numerator"/> / <paramref name="denominator"/>).
    /// </summary>
    public (bool, bool, bool, bool) NextBool4(Vector128<int> numerator, Vector128<int> denominator)
    {
        var flag = Vector128.LessThan(NextInt4(denominator), numerator);
        return (flag[0] != 0, flag[1] != 0, flag[2] != 0, flag[3] != 0);
    }

    /// <summary>
    /// Generates 4 random numbers that is true with (<paramref name="numerator"/> / <paramref name="denominator"/>).
    /// </summary>
    public (bool, bool, bool, bool) NextBool4(int numerator, int denominator)
    {
        var flag = Vector128.LessThan(NextInt4(denominator), Vector128.Create(numerator));
        return (flag[0] != 0, flag[1] != 0, flag[2] != 0, flag[3] != 0);
    }


    /// <summary>
    /// Generates a random number in the range [0, max) .
    /// </summary>
    public BigInteger NextBigInteger(BigInteger maxExclusive)
    {
        if (maxExclusive <= 0)
            ThrowMaxMinus();

        int actualByteLength = maxExclusive.GetByteCount(true);
        int vectorLength = (actualByteLength + 15) >> 4;
        Vector128<ulong>[]? maxArray = null;
        Vector128<ulong>[]? resultArray = null;

        var maxSpan = vectorLength <= 64 ?
            stackalloc Vector128<ulong>[vectorLength] :
            (maxArray = ArrayPool<Vector128<ulong>>.Shared.Rent(vectorLength)).AsSpan(..vectorLength);
        var resultSpan = vectorLength <= 64 ?
            stackalloc Vector128<ulong>[vectorLength] :
            (resultArray = ArrayPool<Vector128<ulong>>.Shared.Rent(vectorLength)).AsSpan(..vectorLength);

        maxSpan[^1] = Vector128<ulong>.Zero;
        maxExclusive.TryWriteBytes(MemoryMarshal.AsBytes(maxSpan), out _, true);
        var mostSignificantMask = maxSpan[^1][1] == 0 ?
            Vector128.Create(BitOperations.RoundUpToPowerOf2(maxSpan[^1][0]) - 1, 0ul) :
            Vector128.Create(~0ul, BitOperations.RoundUpToPowerOf2(maxSpan[^1][1]) - 1);

        while (true)
        {
            for (int i = 0; i < resultSpan.Length; i++)
            {
                resultSpan[i] = Next();
            }
            resultSpan[^1] &= mostSignificantMask;


            static bool IsLessThan(ReadOnlySpan<Vector128<ulong>> left, ReadOnlySpan<Vector128<ulong>> right)
            {
                for (int i = left.Length - 1; i >= 0; i--)
                {
                    var lessThan = Vector128.LessThan(left[i], right[i]);
                    if (lessThan[1] != 0)
                        return true;

                    var greaterThan = Vector128.GreaterThan(left[i], right[i]);
                    if (greaterThan[1] != 0)
                        return false;

                    if (lessThan[0] != 0)
                        return true;

                    if (greaterThan[0] != 0)
                        return false;
                }
                return false;
            }

            if (IsLessThan(resultSpan, maxSpan))
            {
                var result = new BigInteger(MemoryMarshal.AsBytes(resultSpan), true);

                if (maxArray != null)
                    ArrayPool<Vector128<ulong>>.Shared.Return(maxArray);
                if (resultArray != null)
                    ArrayPool<Vector128<ulong>>.Shared.Return(resultArray);

                return result;
            }
        }
    }

    /// <summary>
    /// Generates a random number in the range [min, max) .
    /// </summary>
    public BigInteger NextBigInteger(BigInteger minInclusive, BigInteger maxExclusive)
    {
        if (minInclusive >= maxExclusive)
            ThrowMinMax();

        return minInclusive + NextBigInteger(maxExclusive - minInclusive);
    }

    private bool IsProbablePrime(BigInteger n)
    {
        if (n <= 1)
            return false;

        const int accuracy = 32;

        if (n == 2)
            return true;
        if (n.IsEven)
            return false;

        var nMinus1 = n - 1;
        var d = nMinus1;
        int trailingZeroes = (int)BigInteger.TrailingZeroCount(d);
        d >>= trailingZeroes;


        for (int trial = 0; trial < accuracy; trial++)
        {
            BigInteger a;
            do
            {
                a = NextBigInteger(n);
            } while (a < 2);

            var b = BigInteger.ModPow(a, d, n);

            if (b % n is BigInteger firstMod && (firstMod == 1 || firstMod == nMinus1))
                continue;

            int i;
            for (i = 1; i < trailingZeroes; i++)
            {
                b = BigInteger.ModPow(b, 2, n);

                if (b == nMinus1)
                    break;
            }
            if (i >= trailingZeroes)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Generates a random probable prime in the range [2, max).
    /// </summary>
    /// <remarks>
    /// It returns almost certainly prime, but it may return a composite number with probability less than 2^-64.
    /// </remarks>
    public BigInteger NextProbablePrime(BigInteger maxExclusive)
    {
        if (maxExclusive <= 2)
            ThrowMaxMinus();

        while (true)
        {
            var candidate = NextBigInteger(maxExclusive);

            if (candidate.IsEven)
                candidate++;

            if (candidate == BigInteger.One)
                return new BigInteger(2);

            if (IsProbablePrime(candidate))
                return candidate;
        }
    }


    /// <summary>
    /// Generates a random enum.
    /// </summary>
    public TEnum NextEnum<TEnum>()
        where TEnum : struct, Enum
    {
        var values = EnumCache<TEnum>.Values;
        if (values.Length == 0)
            ThrowEnumIsEmpty();

        return values[NextInt(0, values.Length)];
    }

    private static class EnumCache<T>
        where T : struct, Enum
    {
        public static readonly T[] Values;

        static EnumCache()
        {
            Values = Enum.GetValues<T>();
        }
    }

    /// <summary>
    /// Generates a random <see cref="Guid"/> (UUIDv4).
    /// </summary>
    /// <remarks>
    /// Note: This is faster than <see cref="Guid.NewGuid"/> but not cryptographically secure. 
    /// DON'T use for security sensitive purposes.
    /// </remarks>
    public Guid NextGuid()
    {
        var buffer = Next();
        buffer = Vector128.BitwiseOr(Vector128.AndNot(buffer, Vector128.Create(0xf000ul << 48, 0xc0ul)), Vector128.Create(0x4000ul << 48, 0x80ul));
        return MemoryMarshal.Cast<Vector128<ulong>, Guid>(MemoryMarshal.CreateSpan(ref buffer, 1))[0];
    }

    /// <summary>
    /// Generates a random Ulid.
    /// </summary>
    /// <seealso cref="https://github.com/ulid/spec"/>
    public Guid NextUlid()
    {
        var buffer = Next();
        ulong timestamp = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds();
        buffer = Vector128.Xor(Vector128.AndNot(buffer, Vector128.Create(0xfffffffffffful, 0)),
            Vector128.Create(timestamp >> 16 ^ timestamp << 32, 0));
        return MemoryMarshal.Cast<Vector128<ulong>, Guid>(MemoryMarshal.CreateSpan(ref buffer, 1))[0];
    }

    /// <summary>
    /// Generates a random Ulid with specific timestamp.
    /// </summary>
    /// <seealso cref="https://github.com/ulid/spec"/>
    public Guid NextUlid(long unixTimeMilliseconds)
    {
        var buffer = Next();
        ulong timestamp = (ulong)unixTimeMilliseconds;
        buffer = Vector128.Xor(Vector128.AndNot(buffer, Vector128.Create(0xfffffffffffful, 0)),
            Vector128.Create(timestamp >> 16 ^ timestamp << 32, 0));
        return MemoryMarshal.Cast<Vector128<ulong>, Guid>(MemoryMarshal.CreateSpan(ref buffer, 1))[0];
    }


    private static readonly double[] NextGaussianX = { 3.942166282539769E-19, 3.7204945004118776E-19, 3.5827024480628514E-19, 3.4807476236540124E-19, 3.3990177171882035E-19, 3.3303778360340052E-19, 3.2709438817617477E-19, 3.2183577132495033E-19, 3.1710758541840374E-19, 3.1280307407034012E-19, 3.088452065580397E-19, 3.0517650624107304E-19, 3.0175290292584557E-19, 2.9853983440705282E-19, 2.9550967462801758E-19, 2.9263997988491624E-19, 2.8991225869977438E-19, 2.8731108780226257E-19, 2.84823463271013E-19, 2.8243831535194356E-19, 2.8014613964727E-19, 2.7793871261807768E-19, 2.7580886921411178E-19, 2.7375032698308729E-19, 2.7175754543391022E-19, 2.698256124753846E-19, 2.6795015188771481E-19, 2.6612724730440009E-19, 2.6435337927976614E-19, 2.6262537282028419E-19, 2.6094035335224123E-19, 2.5929570954330983E-19, 2.5768906173214707E-19, 2.5611823497719588E-19, 2.5458123593393342E-19, 2.5307623292372444E-19, 2.5160153867798386E-19, 2.5015559533646177E-19, 2.4873696135403144E-19, 2.4734430003079192E-19, 2.4597636942892717E-19, 2.446320134791244E-19, 2.4331015411139196E-19, 2.4200978427132945E-19, 2.4072996170445874E-19, 2.3946980340903342E-19, 2.3822848067252665E-19, 2.3700521461931791E-19, 2.3579927220741325E-19, 2.3460996262069967E-19, 2.334366340105445E-19, 2.3227867054673835E-19, 2.311354897430376E-19, 2.3000654002704233E-19, 2.28891298527976E-19, 2.2778926905921892E-19, 2.2669998027527317E-19, 2.2562298398527411E-19, 2.2455785360727256E-19, 2.2350418274933907E-19, 2.2246158390513289E-19, 2.2142968725296249E-19, 2.2040813954857555E-19, 2.1939660310297606E-19, 2.1839475483749618E-19, 2.1740228540916853E-19, 2.1641889840016519E-19, 2.1544430956570613E-19, 2.1447824613540345E-19, 2.1352044616350571E-19, 2.1257065792395107E-19, 2.1162863934653125E-19, 2.1069415749082023E-19, 2.0976698805483467E-19, 2.0884691491567363E-19, 2.0793372969963634E-19, 2.0702723137954107E-19, 2.0612722589717129E-19, 2.0523352580895635E-19, 2.0434594995315797E-19, 2.0346432313698151E-19, 2.0258847584216418E-19, 2.0171824394771313E-19, 2.0085346846857531E-19, 1.9999399530912018E-19, 1.9913967503040587E-19, 1.9829036263028147E-19, 1.9744591733545175E-19, 1.9660620240469855E-19, 1.9577108494251483E-19, 1.9494043572246305E-19, 1.9411412901962158E-19, 1.9329204245152932E-19, 1.9247405682708166E-19, 1.9166005600287069E-19, 1.9084992674649821E-19, 1.9004355860642335E-19, 1.892408437879372E-19, 1.8844167703488431E-19, 1.8764595551677744E-19, 1.8685357872097447E-19, 1.8606444834960931E-19, 1.852784682209879E-19, 1.8449554417517925E-19, 1.8371558398354866E-19, 1.8293849726199562E-19, 1.8216419538767388E-19, 1.8139259141898443E-19, 1.8062360001864449E-19, 1.7985713737964738E-19, 1.790931211539384E-19, 1.7833147038364195E-19, 1.7757210543468423E-19, 1.768149479326639E-19, 1.7605992070083135E-19, 1.7530694770004407E-19, 1.7455595397057215E-19, 1.7380686557563472E-19, 1.7305960954655261E-19, 1.7231411382940904E-19, 1.7157030723311375E-19, 1.7082811937877138E-19, 1.7008748065025788E-19, 1.6934832214591352E-19, 1.6861057563126349E-19, 1.6787417349268046E-19, 1.6713904869190636E-19, 1.6640513472135291E-19, 1.656723655601024E-19, 1.6494067563053264E-19, 1.6420999975549112E-19, 1.6348027311594529E-19, 1.6275143120903658E-19, 1.6202340980646725E-19, 1.6129614491314931E-19, 1.6056957272604589E-19, 1.5984362959313479E-19, 1.5911825197242491E-19, 1.5839337639095552E-19, 1.57668939403708E-19, 1.5694487755235887E-19, 1.5622112732380259E-19, 1.5549762510837067E-19, 1.5477430715767269E-19, 1.5405110954198328E-19, 1.5332796810709686E-19, 1.5260481843056971E-19, 1.5188159577726681E-19, 1.5115823505412761E-19, 1.5043467076406196E-19, 1.4971083695888395E-19, 1.4898666719118714E-19, 1.4826209446506113E-19, 1.4753705118554368E-19, 1.468114691066983E-19, 1.4608527927820112E-19, 1.4535841199031451E-19, 1.4463079671711862E-19, 1.4390236205786415E-19, 1.4317303567630175E-19, 1.4244274423783481E-19, 1.4171141334433217E-19, 1.4097896746642792E-19, 1.4024532987312287E-19, 1.3951042255849034E-19, 1.3877416616527576E-19, 1.3803647990516385E-19, 1.3729728147547174E-19, 1.3655648697200824E-19, 1.3581401079782068E-19, 1.35069765567529E-19, 1.3432366200692418E-19, 1.3357560884748263E-19, 1.3282551271542044E-19, 1.3207327801488085E-19, 1.3131880680481522E-19, 1.3056199866908071E-19, 1.2980275057923783E-19, 1.2904095674948603E-19, 1.2827650848312722E-19, 1.2750929400989208E-19, 1.2673919831340477E-19, 1.2596610294799505E-19, 1.2518988584399369E-19, 1.2441042110056516E-19, 1.2362757876504158E-19, 1.2284122459762067E-19, 1.2205121982017847E-19, 1.2125742084782237E-19, 1.2045967900166969E-19, 1.1965784020118015E-19, 1.188517446341955E-19, 1.1804122640264086E-19, 1.1722611314162059E-19, 1.1640622560939106E-19, 1.1558137724540872E-19, 1.1475137369333182E-19, 1.1391601228549045E-19, 1.1307508148492589E-19, 1.1222836028063025E-19, 1.1137561753107903E-19, 1.1051661125053528E-19, 1.0965108783189755E-19, 1.0877878119905372E-19, 1.0789941188076654E-19, 1.0701268599703639E-19, 1.0611829414763283E-19, 1.0521591019102927E-19, 1.043051899002755E-19, 1.033857694803547E-19, 1.0245726392923698E-19, 1.0151926522209309E-19, 1.0057134029488234E-19, 9.96130287996728E-20, 9.864384059945989E-20, 9.76632529647558E-20, 9.6670707427623454E-20, 9.5665606240866658E-20, 9.46473083804332E-20, 9.3615125017323484E-20, 9.256831437088727E-20, 9.1506075837638762E-20, 9.04275432677257E-20, 8.9331777233763668E-20, 8.8217756102327859E-20, 8.7084365674892307E-20, 8.5930387109612138E-20, 8.4754482764244337E-20, 8.3555179508462331E-20, 8.2330848933585352E-20, 8.1079683729129829E-20, 7.9799669284133852E-20, 7.8488549286072733E-20, 7.714378370093468E-20, 7.5762496979467554E-20, 7.4341413578485329E-20, 7.2876776807378419E-20, 7.1364245443525362E-20, 6.9798760240761054E-20, 6.8174368944799042E-20, 6.6483992986198527E-20, 6.4719110345162767E-20, 6.2869314813103686E-20, 6.0921687548281251E-20, 5.8859873575576818E-20, 5.6662675116090981E-20, 5.4301813630894559E-20, 5.17381717444942E-20, 4.8915031722398539E-20, 4.5744741890755295E-20, 4.2078802568583392E-20, 3.7625986722404749E-20, 3.16285898058819E-20, 0 };
    private static readonly double[] NextGaussianY = { 1.4598410796621224E-22, 3.0066613427945036E-22, 4.6129728815105761E-22, 6.2663350049236694E-22, 7.9594524761883951E-22, 9.6874655021707428E-22, 1.1446877002379678E-21, 1.3235036304379412E-21, 1.5049857692053373E-21, 1.688965300071954E-21, 1.8753025382711875E-21, 2.0638798423695436E-21, 2.2545966913644952E-21, 2.4473661518802054E-21, 2.6421122727763804E-21, 2.8387681187880171E-21, 3.0372742567457558E-21, 3.2375775699986863E-21, 3.4396303157949074E-21, 3.6433893657998091E-21, 3.8488155868912591E-21, 4.0558733309493069E-21, 4.2645300104283891E-21, 4.4747557422305367E-21, 4.686523046535586E-21, 4.8998065902775521E-21, 5.1145829672105745E-21, 5.3308305082046459E-21, 5.5485291167032029E-21, 5.767660125269071E-21, 5.9882061699178717E-21, 6.2101510795442477E-21, 6.4334797782257449E-21, 6.6581781985714183E-21, 6.8842332045893437E-21, 7.1116325227957336E-21, 7.3403646804903347E-21, 7.5704189502886659E-21, 7.8017853001379984E-21, 8.0344543481570242E-21, 8.2684173217333329E-21, 8.5036660203915217E-21, 8.7401927820109687E-21, 8.9779904520282081E-21, 9.217052355306156E-21, 9.4573722703928955E-21, 9.69894440592696E-21, 9.9417633789758589E-21, 1.0185824195119829E-20, 1.0431122230114781E-20, 1.0677653212987408E-20, 1.0925413210432012E-20, 1.1174398612392903E-20, 1.1424606118728722E-20, 1.1676032726866311E-20, 1.1928675720361041E-20, 1.2182532658289385E-20, 1.2437601365406799E-20, 1.2693879923010687E-20, 1.2951366660454158E-20, 1.3210060147261467E-20, 1.3469959185800735E-20, 1.3731062804473641E-20, 1.3993370251385593E-20, 1.4256880988463133E-20, 1.4521594685988372E-20, 1.4787511217522905E-20, 1.505463065519617E-20, 1.5322953265335221E-20, 1.5592479504415048E-20, 1.5863210015310325E-20, 1.6135145623830982E-20, 1.6408287335525598E-20, 1.6682636332737935E-20, 1.6958193971903124E-20, 1.7234961781071113E-20, 1.7512941457646084E-20, 1.779213486633149E-20, 1.807254403727107E-20, 1.8354171164377274E-20, 1.8637018603838942E-20, 1.8921088872801E-20, 1.9206384648209465E-20, 1.9492908765815639E-20, 1.9780664219333854E-20, 2.006965415974783E-20, 2.0359881894760853E-20, 2.0651350888385693E-20, 2.0944064760670542E-20, 2.1238027287557472E-20, 2.1533242400870493E-20, 2.1829714188430486E-20, 2.2127446894294606E-20, 2.2426444919118282E-20, 2.2726712820637813E-20, 2.3028255314272291E-20, 2.3331077273843579E-20, 2.3635183732413304E-20, 2.394057988323637E-20, 2.4247271080830292E-20, 2.4555262842160345E-20, 2.486456084794038E-20, 2.5175170944049631E-20, 2.5487099143065938E-20, 2.5800351625916006E-20, 2.61149347436437E-20, 2.6430855019297335E-20, 2.6748119149937423E-20, 2.7066734008766265E-20, 2.7386706647381211E-20, 2.7708044298153576E-20, 2.8030754376735287E-20, 2.8354844484695771E-20, 2.8680322412291655E-20, 2.9007196141372144E-20, 2.9335473848423237E-20, 2.9665163907754E-20, 2.999627489482863E-20, 3.0328815589748068E-20, 3.0662794980885293E-20, 3.0998222268678766E-20, 3.1335106869588609E-20, 3.1673458420220558E-20, 3.2013286781622988E-20, 3.2354602043762618E-20, 3.2697414530184806E-20, 3.304173480286495E-20, 3.3387573667257349E-20, 3.3734942177548944E-20, 3.4083851642125214E-20, 3.4434313629256261E-20, 3.4786339973011394E-20, 3.5139942779411176E-20, 3.5495134432826177E-20, 3.585192760263246E-20, 3.6210335250134172E-20, 3.657037063576439E-20, 3.6932047326575888E-20, 3.7295379204034264E-20, 3.7660380472126407E-20, 3.802706566579829E-20, 3.8395449659736661E-20, 3.8765547677510173E-20, 3.9137375301086418E-20, 3.9510948480742178E-20, 3.9886283545385442E-20, 4.0263397213308578E-20, 4.0642306603393553E-20, 4.1023029246790973E-20, 4.140558309909645E-20, 4.1789986553048823E-20, 4.2176258451776819E-20, 4.2564418102621753E-20, 4.2954485291566191E-20, 4.3346480298300112E-20, 4.3740423911958146E-20, 4.4136337447563716E-20, 4.4534242763218286E-20, 4.493416227807625E-20, 4.5336118991149031E-20, 4.5740136500984466E-20, 4.6146239026271279E-20, 4.6554451427421133E-20, 4.6964799229185082E-20, 4.7377308644364926E-20, 4.7792006598684163E-20, 4.8208920756888113E-20, 4.8628079550147814E-20, 4.9049512204847647E-20, 4.9473248772842596E-20, 4.9899320163277668E-20, 5.0327758176068971E-20, 5.0758595537153414E-20, 5.11918659356227E-20, 5.1627604062866077E-20, 5.2065845653856434E-20, 5.2506627530725224E-20, 5.2949987648783478E-20, 5.339596514515945E-20, 5.3844600390237606E-20, 5.42959350420994E-20, 5.47500121041839E-20, 5.5206875986405109E-20, 5.5666572569983857E-20, 5.6129149276275828E-20, 5.6594655139902512E-20, 5.70631408865206E-20, 5.7534659015596954E-20, 5.8009263888591254E-20, 5.8487011822987619E-20, 5.8967961192659828E-20, 5.9452172535103507E-20, 5.9939708666122642E-20, 6.0430634802618953E-20, 6.0925018694200555E-20, 6.1422930764402872E-20, 6.1924444262401555E-20, 6.2429635426193963E-20, 6.2938583658336226E-20, 6.3451371715447575E-20, 6.3968085912834963E-20, 6.4488816345752724E-20, 6.5013657128995346E-20, 6.5542706656731714E-20, 6.6076067884730729E-20, 6.6613848637404208E-20, 6.7156161942412992E-20, 6.770312639595058E-20, 6.825486656224642E-20, 6.8811513411327837E-20, 6.9373204799659693E-20, 6.9940085998959121E-20, 7.0512310279279515E-20, 7.1090039553397179E-20, 7.1673445090644808E-20, 7.22627083096558E-20, 7.2858021661057338E-20, 7.3459589613035812E-20, 7.4067629754967565E-20, 7.4682374037052829E-20, 7.5304070167226678E-20, 7.5932983190698559E-20, 7.6569397282483766E-20, 7.721361778948769E-20, 7.7865973566417028E-20, 7.8526819659456767E-20, 7.9196540403850572E-20, 7.987555301703798E-20, 8.0564311788901642E-20, 8.1263312996426188E-20, 8.1973100703706316E-20, 8.2694273652634046E-20, 8.3427493508836792E-20, 8.4173494807453416E-20, 8.4933097052832066E-20, 8.57072195782309E-20, 8.64968999859307E-20, 8.7303317295655339E-20, 8.8127821378859516E-20, 8.8971970928196666E-20, 8.9837583239314076E-20, 9.0726800697869543E-20, 9.1642181484063544E-20, 9.2586826406702765E-20, 9.3564561480278864E-20, 9.4580210012636175E-20, 9.564001555085037E-20, 9.6752334770503142E-20, 9.7928851697808831E-20, 9.9186905857531331E-20, 1.0055456271343398E-19, 1.0208407377305566E-19, 1.0390360993240711E-19, 1.0842021724855044E-19 };
    private static readonly ulong[] NextGaussianAliasBox = { 0xc6d1bcb1d7c5c8fb, 0xc58e65fed90c18f7, 0xbdfe727f7b5100f4, 0xdf559b5c3fd230f1, 0xfd37a81820b170ef, 0xda845732a9b27003, 0xc18bb1c5c69448f3, 0xaea2e0fe0c50f0f5, 0x9fc6849420a6c8f6, 0x93c2f42cbec160f7, 0x89d49e0c64c588f8, 0x8178f3a67d05e0f9, 0x7a554ee0b04510f9, 0x7428aefe15e7f4fa, 0x6ec330fc6a9140fb, 0x6a00bca9d20520fb, 0x65c592cf1a7894fb, 0x61fc00beb1018800, 0x5e92cd0eec7fc800, 0x5b7c1cb2569b60fc, 0x58aca85e4418b0fc, 0x561b28c74e477cfc, 0x53bfe910a63250fc, 0x5194745ca09590fd, 0x4f9356ee453d70fd, 0x4db7eda0d49b90fd, 0x4bfe400809666cfd, 0x4a62e28fed3b58fd, 0x48e2deb6dce1f0fd, 0x477b9ff6890330fd, 0x462ae44f1e19e8fd, 0x44eeafaec37ed8fd, 0x43c54198bbb850fd, 0x42ad0c96e746f4fd, 0x41a4af1f120398fd, 0x40aaed9f2142d0fd, 0x3fbead7e1c2650fd, 0x3edef0e17972b0fd, 0x3e0ad317806c02fd, 0x3d418587825728fd, 0x3c824d1301ff92fd, 0x3bcc7fd4ba1b34fd, 0x3b1f832e92fdb8fd, 0x3a7aca1973b674fd, 0x39ddd3aedfad24fd, 0x394829e0df9906fd, 0x38b9605bcc8b1afd, 0x3831138b23cd28fd, 0x37aee7bbd833a6fd, 0x37328858ffdef4fd, 0x36bba73f647326fd, 0x3649fc239c9b0cfd, 0x35dd440a3ca3e4fd, 0x357540cd98fcdcfd, 0x3511b8b1b6f68cfd, 0x34b27603192b44fd, 0x345746bed152d2fd, 0x33fffc451e8a3afd, 0x33ac6b11ce6374fd, 0x335c6a7d782972fd, 0x330fd4834a7bf0fd, 0x32c6858cd99e9efd, 0x32805c436b10a6fd, 0x323d39647de4dafd, 0x31fcff9b1cbc2efd, 0x31bf935c0afec2fd, 0x3184dac54d663cfd, 0x314cbd80aea4b6fd, 0x311724a8b10300fd, 0x30e3faaf5c30f4fd, 0x30b32b47f4eed4fd, 0x3084a3519d16c8fd, 0x305850c4757e66fd, 0x302e229fe376a2fd, 0x300608da4510a6fd, 0x2fdff451a1855afd, 0x2fbbd6be63f1eefd, 0x2f99a2a64bd204fd, 0x2f794b507d5784fd, 0x2f5ac4babd80a4fd, 0x2f3e038f49a680fd, 0x2f22fd1b9810c4fd, 0x2f09a74772a6d0fd, 0x2ef1f88d34f902fd, 0x2edbe7f1c515c6fd, 0x2ec76cfe738d88fd, 0x2eb47fba0c19dafd, 0x2ea318a28e748efd, 0x2e9330a894d4cafd, 0x2e84c1291fc1cafd, 0x2e77c3e97a4dcefd, 0x2e6c3312903184fd, 0x2e62092c7d15f2fd, 0x2e59411b71ded4fd, 0x2e51d61b49fe3efd, 0x2e4bc3bccd1166fd, 0x2e4705e25f1ef2fd, 0x2e4398bd39fd94fd, 0x2e4178cab1778afd, 0x2e40a2d1fb9f2cfd, 0x2e4113e198c4c8fd, 0x2e42c94de1bd5afd, 0x2e45c0ae0c8020fd, 0x2e49f7dbb2dc20fd, 0x2e4f6cf035308efd, 0x2e561e43fc4bbefd, 0x2e5e0a6cbd8db0fd, 0x2e67303c4fe530fd, 0x2e718ebfe48e86fd, 0x2e7d253f029ffafd, 0x2e89f33a94dd24fd, 0x2e97f86c0ae26afd, 0x2ea734c573754cfd, 0x2eb7a8703ea7dafd, 0x2ec953cd280342fd, 0x2edc377436e01cfd, 0x2ef0543416c832fd, 0x2f05ab123d28a2fd, 0x2f1c3d4b2da8d4fd, 0x2f340c5207034afd, 0x2f4d19d1325b6efd, 0x2f6767aadb615afd, 0x2f82f7f87920a0fd, 0x2f9fcd0c79737afd, 0x2fbde971df0bb2fd, 0x2fdd4fedcdd7bafd, 0x2ffe037f772576fd, 0x30200761cc76dafd, 0x30435f0c42b708fd, 0x30680e33921a30fd, 0x308e18cbac79aafd, 0x30b5830850abb8fd, 0x30de515f4a0566fd, 0x310888894f49a6fd, 0x31342d84491134fd, 0x31614594c1269afd, 0x318fd6486c0d4cfd, 0x31bfe57779f8b4fd, 0x31f17947e4e340fd, 0x3224982ee3906cfd, 0x325948f43c8910fd, 0x328f92b4cc68f2fd, 0x32c77ce56f2f8efd, 0x33010f562b3bd6fd, 0x333c5235b88b54fd, 0x33794e1501d94afd, 0x33b80beac83996fd, 0x33f89517e43358fd, 0x343af36b92a066fd, 0x347f3127c9566cfd, 0x34c55906684954fd, 0x350d763e286330fd, 0x3557948851df6efd, 0x35a3c02639f2e2fd, 0x35f205e7cfddc8fd, 0x3642733226acc4fd, 0x3695160663fbc8fd, 0x36e9fd09463002fd, 0x3741378b39d708fd, 0x379ad590b4f468fd, 0x37f6e7db2e059cfd, 0x38557ff35d637cfd, 0x38b6b03277c0eefd, 0x391a8bce450fdefd, 0x398126e4252d20fd, 0x39ea9685db8464fd, 0x3a56f0c70e33d8fd, 0x3ac64ccb521042fd, 0x3b38c2d5fabeb2fd, 0x3bae6c5a0bd7defd, 0x3c27640c403414fd, 0x3ca3c5f5b1b4f2fd, 0x3d23af886fd6bcfd, 0x3da73fb5118a5afd, 0x3e2e97023a62d6fd, 0x3eb9d7a61501d0fd, 0x3f4925a1214e68fd, 0x3fdca6dc15b5f4fd, 0x40748346fbe7a8fd, 0x4110e4fb856bb8fd, 0x41b1f8627a85d0fd, 0x4257ec5b004f08fd, 0x4302f26661d574fd, 0x43b33ed7116e40fd, 0x4469090393bba4fd, 0x45248b7deb8264fd, 0x45e6045012d9e0fd, 0x46adb53da6aed4fd, 0x477be40bd0fa20fd, 0x4850dacf591c1cfd, 0x492ce842a0fd70fd, 0x4a1060233adb10fd, 0x4afb9b98ffb8dcfd, 0x4beef9a70adef0fd, 0x4ceadfa81a5098fd, 0x4defb9d72c7494fd, 0x4efdfbe72fe4d0fd, 0x501621a9608c34fd, 0x5138afc6a7a9f0fd, 0x5266348c2347b8fc, 0x539f48cf4aad78fc, 0x54e490eb262980fc, 0x5636bddb69fe9cfc, 0x57968e77cf0548fc, 0x5904d0d6975054fc, 0x5a8263d9629960fc, 0x5c1038ebfe230400, 0x5daf55fbfd149000, 0x5f60d7b2ef00d400, 0x6125f3fa86025800, 0x62fffcdb2c4b1800, 0x64f063bf243f3800, 0x66f8bd2da9e8d8fb, 0x691ac5123b4ba8fb, 0x6b5863a76bef24fb, 0x6db3b324abb4d8fb, 0x702f0650b62f28fa, 0x72ccf025b46f0cfa, 0x75904cbbd8328cfa, 0x787c4bbbb09d30fa, 0x7b947ca99e2e4cf9, 0x7edcdd6e5388c4f9, 0x8259eb9b2bd398f8, 0x8610b907d7b2c0f8, 0x8a070493b6c2f001, 0x8e435809cf16f001, 0x92cd2c7476d78801, 0x97ad168ac8e140f7, 0x9cecfd6d8baee0f7, 0xa2985e97c21140f6, 0xa8bca2e6cb4158f5, 0xaf6989f6a2190002, 0xb6b1b2fd462b6002, 0xbeab4d13c9c930f4, 0xc770fcdd4cdad0f3, 0xd1230b71f7ef48f2, 0xdbe8fb696740b003, 0xe7f3aeac31d898f1, 0xf5805d6770e7f0f0, 0xfffffffffd71c0ef, 0xfdeb966ea72228ef, 0xf1fe2559fa5188f0, 0xf7bdcf2ab32e5003, 0xe4368349dbba40f2, 0xfcccff52b1bd70f3, 0xcf0a381011797802, 0xce3286a74622e0f5, 0xae75adb6e22950f6, 0xd722c4a089ebd801, 0xc9dc5f7b7d2d80f8, 0xd7c9bdcbf93010f9, 0xcfef53b6a4d3c8fa, 0xb786b6325d47c000, 0xb06b5777e76e98fc, 0x00000000000000fd, 0x00000000000000fd };

    /// <summary>
    /// Generates a random number from a Gaussian (normal) distribution.
    /// </summary>
    public double NextGaussian()
    {
        const int TableSize = 256;
        const int IMax = 253;

        var uu = Next();
        long u1 = (long)uu[0];
        int tableX = (int)(u1 & (TableSize - 1));

        if (tableX < IMax)
        {
            return u1 * NextGaussianX[tableX];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static double Fallback(Culumi self, Vector128<ulong> u)
        {
            const int InflectionIndex = 204;
            const ulong MaxConvexRate = 0x3efb83be6450cc00;
            const ulong MaxConcaveRate = 0x151b6b6b7cd81f00;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static double SampleX(double x, int tableY) => Math.FusedMultiplyAdd(x, NextGaussianX[tableY - 1] - NextGaussianX[tableY], NextGaussianX[tableY] * (1ul << 63));
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static double SampleY(double y, int tableY) => Math.FusedMultiplyAdd(y, NextGaussianY[tableY - 1] - NextGaussianY[tableY], NextGaussianY[tableY] * (1ul << 63));

            long u1 = (long)u[0];
            ulong u2 = u[1];
            int tableAlias = (int)(u2 & (TableSize - 1));
            int tableY = (u2 >> 8) < (NextGaussianAliasBox[tableAlias] >> 8) ? tableAlias : (int)(NextGaussianAliasBox[tableAlias] & (TableSize - 1));

            if (tableY == 0)
            {
                double x0 = NextGaussianX[0] * (1ul << 63);
                double s, t;
                do
                {
                    s = self.NextExponential() / x0;
                    t = self.NextExponential();
                } while (s * s > t + t);
                return (x0 + s) * (u1 < 0 ? -1 : 1);
            }
            else if (tableY < InflectionIndex)
            {
                var uu = Vector128.ShiftRightLogical(self.Next(), 1);
                ulong ux = uu[0];
                ulong uy = uu[1];

                double tx;

                for (; ; uu = Vector128.ShiftRightLogical(self.Next(), 1), ux = uu[0], uy = uu[1])
                {
                    if (uy < ux)
                    {
                        ux = (1ul << 63) - 1 - ux;
                        uy = (1ul << 63) - 1 - uy;
                    }

                    if (uy >= ux + MaxConcaveRate)
                    {
                        tx = SampleX(ux, tableY);
                        break;
                    }

                    tx = SampleX(ux, tableY);
                    double ty = SampleY(uy, tableY);

                    if (ty <= Math.Exp(-0.5 * tx * tx))
                    {
                        break;
                    }
                }
                return tx * (u1 < 0 ? -1 : 1);
            }
            else if (tableY == InflectionIndex)
            {
                var uu = Vector128.ShiftRightLogical(self.Next(), 1);
                ulong ux = uu[0];
                ulong uy = uu[1];

                double tx;

                for (; ; uu = Vector128.ShiftRightLogical(self.Next(), 1), ux = uu[0], uy = uu[1])
                {
                    if (uy >= ux + MaxConcaveRate)
                    {
                        tx = SampleX(ux, tableY);
                        break;
                    }

                    if (uy + MaxConvexRate <= ux)
                    {
                        continue;
                    }

                    tx = SampleX(ux, tableY);
                    double ty = SampleY(uy, tableY);

                    if (ty <= Math.Exp(-0.5 * tx * tx))
                    {
                        break;
                    }
                }
                return tx * (u1 < 0 ? -1 : 1);
            }
            else
            {
                var uu = Vector128.ShiftRightLogical(self.Next(), 1);
                ulong ux = uu[0];
                ulong uy = uu[1];

                double tx;

                for (; ; uu = Vector128.ShiftRightLogical(self.Next(), 1), ux = uu[0], uy = uu[1])
                {
                    if (uy >= ux)
                    {
                        tx = SampleX(ux, tableY);
                        break;
                    }

                    if (uy + MaxConvexRate <= ux)
                    {
                        continue;
                    }

                    tx = SampleX(ux, tableY);
                    double ty = SampleY(uy, tableY);

                    if (ty <= Math.Exp(-tx * tx / 2))
                    {
                        break;
                    }
                }
                return tx * (u1 < 0 ? -1 : 1);
            }
        }
        return Fallback(this, uu);
    }

    private static readonly double[] NextExponentialX = { 4.10331203376756E-19, 3.6986866175804254E-19, 3.4566656688958066E-19, 3.2823679410482584E-19, 3.1456269979909514E-19, 3.03286120648027E-19, 2.9367638051868817E-19, 2.8529425264268634E-19, 2.7785472845811339E-19, 2.7116219451872107E-19, 2.6507648848254508E-19, 2.5949369628854142E-19, 2.5433461308999266E-19, 2.4953746469398331E-19, 2.4505312947224859E-19, 2.408418950532468E-19, 2.3687119326822434E-19, 2.3311397903598484E-19, 2.2954754508892096E-19, 2.2615263895329144E-19, 2.2291279408177043E-19, 2.1981381563184253E-19, 2.1684337983553293E-19, 2.1399071809234915E-19, 2.11246365135325E-19, 2.0860195626732108E-19, 2.0605006261232858E-19, 2.0358405612934665E-19, 2.0119799815503497E-19, 1.9888654671438722E-19, 1.9664487892667293E-19, 1.9446862564655205E-19, 1.9235381609360234E-19, 1.902968306909011E-19, 1.8829436069272401E-19, 1.8634337346015125E-19, 1.8444108246124117E-19, 1.8258492124400068E-19, 1.807725207664377E-19, 1.7900168957659048E-19, 1.7727039642266747E-19, 1.7557675494392153E-19, 1.739190101501551E-19, 1.7229552644453697E-19, 1.7070477698281687E-19, 1.691453341937061E-19, 1.6761586131144529E-19, 1.6611510479342966E-19, 1.6464188751402265E-19, 1.6319510264101051E-19, 1.6177370811405434E-19, 1.6037672165540421E-19, 1.5900321625239331E-19, 1.5765231605910447E-19, 1.5632319267132589E-19, 1.5501506173467129E-19, 1.5372717985068675E-19, 1.5245884175002801E-19, 1.5120937770547304E-19, 1.4997815116072294E-19, 1.4876455655371315E-19, 1.4756801731556631E-19, 1.4638798402842153E-19, 1.45223932727213E-19, 1.4407536333208373E-19, 1.4294179819953481E-19, 1.4182278078165824E-19, 1.4071787438389916E-19, 1.3962666101276579E-19, 1.3854874030576456E-19, 1.3748372853660133E-19, 1.3643125768936716E-19, 1.3539097459603044E-19, 1.3436254013209542E-19, 1.3334562846576738E-19, 1.3233992635639463E-19, 1.3134513249834234E-19, 1.3036095690679895E-19, 1.2938712034232588E-19, 1.28423353771241E-19, 1.2746939785917756E-19, 1.2652500249538757E-19, 1.2558992634556372E-19, 1.246639364311392E-19, 1.2374680773319347E-19, 1.2283832281924351E-19, 1.2193827149133938E-19, 1.210464504540078E-19, 1.2016266300070284E-19, 1.192867187175259E-19, 1.1841843320307338E-19, 1.1755762780335641E-19, 1.1670412936081657E-19, 1.1585776997653414E-19, 1.1501838678479183E-19, 1.1418582173921758E-19, 1.1335992140978604E-19, 1.1254053679000983E-19, 1.117275231136981E-19, 1.1092073968070402E-19, 1.1012004969112224E-19, 1.0932532008743433E-19, 1.085364214041337E-19, 1.077532276243935E-19, 1.06975616043369E-19, 1.0620346713775331E-19, 1.0543666444122949E-19, 1.0467509442548528E-19, 1.0391864638647763E-19, 1.0316721233565366E-19, 1.0242068689585317E-19, 1.0167896720163442E-19, 1.0094195280378056E-19, 1.0020954557775858E-19, 9.9481649635916368E-20, 9.8758171243215533E-20, 9.8039018736309816E-20, 9.7324102445789393E-20, 9.6613334621421653E-20, 9.5906629360228306E-20, 9.5203902537247469E-20, 9.4505071738837578E-20, 9.3810056198387454E-20, 9.31187767343039E-20, 9.2431155690154966E-20, 9.1747116876852892E-20, 9.1066585516766538E-20, 9.03894881896586E-20, 8.9715752780347442E-20, 8.9045308427998322E-20, 8.83780854769529E-20, 8.7714015429009779E-20, 8.7053030897072713E-20, 8.6395065560086262E-20, 8.5740054119181881E-20, 8.5087932254960365E-20, 8.44386365858392E-20, 8.3792104627395537E-20, 8.3148274752638179E-20, 8.2507086153143368E-20, 8.1868478800991455E-20, 8.1232393411442874E-20, 8.0598771406293152E-20, 7.9967554877848161E-20, 7.9338686553461617E-20, 7.8712109760577806E-20, 7.808776839222306E-20, 7.7465606872890166E-20, 7.6845570124760031E-20, 7.6227603534205167E-20, 7.5611652918519362E-20, 7.4997664492817877E-20, 7.438558483705183E-20, 7.3775360863079942E-20, 7.31669397817399E-20, 7.2560269069860586E-20, 7.1955296437155025E-20, 7.13519697929326E-20, 7.0750237212566977E-20, 7.015004690365451E-20, 6.95513471717952E-20, 6.8954086385926047E-20, 6.8358212943133333E-20, 6.7763675232867276E-20, 6.7170421600478691E-20, 6.6578400309993487E-20, 6.59875595060358E-20, 6.5397847174806131E-20, 6.4809211104014845E-20, 6.4221598841665558E-20, 6.3634957653576154E-20, 6.3049234479517678E-20, 6.2464375887843187E-20, 6.188032802846975E-20, 6.129703658406669E-20, 6.0714446719292248E-20, 6.0132503027908849E-20, 5.9551149477593744E-20, 5.8970329352247147E-20, 5.83899851915836E-20, 5.7810058727774453E-20, 5.723049081888938E-20, 5.6651221378862847E-20, 5.607218930368675E-20, 5.5493332393503676E-20, 5.4914587270244651E-20, 5.4335889290421791E-20, 5.3757172452648771E-20, 5.3178369299420046E-20, 5.2599410812633142E-20, 5.2020226302285734E-20, 5.1440743287720515E-20, 5.0860887370724853E-20, 5.0280582099717818E-20, 4.9699748824173369E-20, 4.9118306538333753E-20, 4.8536171713160077E-20, 4.7953258115345353E-20, 4.7369476612077128E-20, 4.6784734960079542E-20, 4.6198937577284764E-20, 4.561198529527826E-20, 4.5023775090426467E-20, 4.4434199791323838E-20, 4.3843147759883749E-20, 4.3250502543035532E-20, 4.2656142491570618E-20, 4.2059940342192631E-20, 4.1461762758256734E-20, 4.0861469824017271E-20, 4.0258914486419624E-20, 3.9653941937549631E-20, 3.9046388929762231E-20, 3.8436083014214539E-20, 3.7822841691982579E-20, 3.7206471465089576E-20, 3.658676677254669E-20, 3.5963508793815561E-20, 3.5336464098833411E-20, 3.47053831197502E-20, 3.4069998414628236E-20, 3.3430022687305141E-20, 3.2785146520105071E-20, 3.2135035766684288E-20, 3.14793285404618E-20, 3.0817631719071592E-20, 3.0149516866075868E-20, 2.9474515446425115E-20, 2.8792113179942989E-20, 2.8101743334798716E-20, 2.7402778706749675E-20, 2.6694521954501665E-20, 2.5976193858994976E-20, 2.5246918933169189E-20, 2.4505707611314757E-20, 2.3751433966683067E-20, 2.298280750063274E-20, 2.2198336948998792E-20, 2.1396283151074303E-20, 2.0574596636715019E-20, 1.9730833381303158E-20, 1.886203856570086E-20, 1.79645820431022E-20, 1.7033918345550304E-20, 1.6064223820782044E-20, 1.5047823458200018E-20, 1.3974234727799188E-20, 1.2828456524359345E-20, 1.1587604878401979E-20, 1.0213347614125671E-20, 8.63088516510677E-21, 1.8691277361760884E-21, 0 };
    private static readonly double[] NextExponentialY = { 2.7976027475557515E-23, 5.9012549913509879E-23, 9.2222116933672047E-23, 1.2719515233348412E-22, 1.6368847155753889E-22, 2.0153866066352558E-22, 2.4062739159746744E-22, 2.8086457448290812E-22, 3.2217910270220888E-22, 3.6451331171730957E-22, 4.0781944228160054E-22, 4.520572684174019E-22, 4.9719244243198682E-22, 5.4319530229844609E-22, 5.9003998877305356E-22, 6.3770377674155041E-22, 6.8616655881885455E-22, 7.3541043971875075E-22, 7.8541941287201191E-22, 8.3617909921871784E-22, 8.8767653375151537E-22, 9.39899989255219E-22, 9.928388293916143E-22, 1.0464833852026511E-21, 1.1008248504979007E-21, 1.1558551926152977E-21, 1.2115670758062617E-21, 1.2679537950710331E-21, 1.3250092187085155E-21, 1.3827277381830083E-21, 1.4411042241734185E-21, 1.5001339878773739E-21, 1.5598127468065068E-21, 1.6201365944400758E-21, 1.6811019732093431E-21, 1.74270565037044E-21, 1.8049446963929617E-21, 1.8678164655485767E-21, 1.931318578430991E-21, 1.9954489061776306E-21, 2.0602055561959357E-21, 2.125586859224434E-21, 2.1915913575816748E-21, 2.2582177944755211E-21, 2.3254651042617279E-21, 2.3933324035547874E-21, 2.4618189831059857E-21, 2.5309243003739369E-21, 2.6006479727217234E-21, 2.6709897711824342E-21, 2.7419496147415344E-21, 2.8135275650903038E-21, 2.8857238218095836E-21, 2.9585387179475211E-21, 3.0319727159588386E-21, 3.1060264039765707E-21, 3.1807004923902052E-21, 3.2559958107068082E-21, 3.331913304674072E-21, 3.4084540336463018E-21, 3.4856191681762061E-21, 3.5634099878170283E-21, 3.6418278791210032E-21, 3.7208743338214959E-21, 3.8005509471873057E-21, 3.8808594165387593E-21, 3.9618015399161166E-21, 4.0433792148917293E-21, 4.1255944375181539E-21, 4.2084493014051493E-21, 4.2919459969191414E-21, 4.37608681049931E-21, 4.4608741240850228E-21, 4.5463104146498117E-21, 4.6323982538375496E-21, 4.7191403076969E-21, 4.8065393365105021E-21, 4.894598194715693E-21, 4.983319830913927E-21, 5.0727072879663022E-21, 5.1627637031729616E-21, 5.25349230853432E-21, 5.344896431092389E-21, 5.4369794933506531E-21, 5.5297450137711825E-21, 5.623196607347895E-21, 5.7173379862550425E-21, 5.8121729605702182E-21, 5.9077054390713131E-21, 6.0039394301070831E-21, 6.1008790425410963E-21, 6.1985284867690043E-21, 6.2968920758092644E-21, 6.3959742264675574E-21, 6.4957794605752805E-21, 6.5963124063026928E-21, 6.6975777995473823E-21, 6.7995804853988689E-21, 6.9023254196803455E-21, 7.0058176705686239E-21, 7.1100624202935653E-21, 7.2150649669183374E-21, 7.3208307262020838E-21, 7.427365233546625E-21, 7.5346741460290269E-21, 7.64276324452201E-21, 7.7516384359042979E-21, 7.8613057553631861E-21, 7.9717713687917565E-21, 8.0830415752833376E-21, 8.1951228097259628E-21, 8.308021645499782E-21, 8.4217447972805243E-21, 8.5362991239523415E-21, 8.6516916316335227E-21, 8.7679294768187883E-21, 8.8850199696421055E-21, 9.00297057726413E-21, 9.1217889273886856E-21, 9.24148281191289E-21, 9.3620601907158E-21, 9.4835291955907153E-21, 9.6058981343265841E-21, 9.7291754949442317E-21, 9.8533699500934251E-21, 9.9784903616171675E-21, 1.010454578528994E-20, 1.0231545475736935E-20, 1.0359498891541785E-20, 1.0488415700550663E-20, 1.0618305785381053E-20, 1.0749179249143974E-20, 1.0881046421388921E-20, 1.1013917864281284E-20, 1.1147804379022596E-20, 1.1282717012524507E-20, 1.1418667064347987E-20, 1.155566609391999E-20, 1.1693725928040416E-20, 1.1832858668693039E-20, 1.197307670117479E-20, 1.2114392702558688E-20, 1.2256819650506589E-20, 1.2400370832448864E-20, 1.2545059855149203E-20, 1.2690900654673779E-20, 1.2837907506785232E-20, 1.2986095037783149E-20, 1.3135478235814109E-20, 1.3286072462675743E-20, 1.3437893466140902E-20, 1.3590957392829556E-20, 1.3745280801657969E-20, 1.3900880677896509E-20, 1.4057774447869568E-20, 1.4215979994333246E-20, 1.43755156725689E-20, 1.4536400327233133E-20, 1.4698653310007725E-20, 1.486229449809581E-20, 1.5027344313614035E-20, 1.5193823743933803E-20, 1.5361754363028524E-20, 1.5531158353887937E-20, 1.570205853206498E-20, 1.5874478370425466E-20, 1.604844202517616E-20, 1.6223974363252439E-20, 1.6401100991152985E-20, 1.6579848285315668E-20, 1.6760243424136097E-20, 1.6942314421738426E-20, 1.7126090163616655E-20, 1.7311600444274307E-20, 1.7498876007000826E-20, 1.7687948585934524E-20, 1.7878850950574509E-20, 1.8071616952917891E-20, 1.8266281577413691E-20, 1.8462880993941777E-20, 1.8661452614043478E-20, 1.8862035150651046E-20, 1.9064668681585509E-20, 1.9269394717117605E-20, 1.9476256271913919E-20, 1.9685297941721188E-20, 1.9896565985175711E-20, 2.011010841116287E-20, 2.0325975072194052E-20, 2.054421776431546E-20, 2.0764890334116344E-20, 2.0988048793463279E-20, 2.1213751442653714E-20, 2.144205900275679E-20, 2.1673034757993714E-20, 2.1906744709105117E-20, 2.2143257738760407E-20, 2.2382645790186164E-20, 2.2624984060329141E-20, 2.2870351209027197E-20, 2.3118829585841496E-20, 2.3370505476409172E-20, 2.3625469370411695E-20, 2.3883816253525597E-20, 2.4145645926034935E-20, 2.441106335114639E-20, 2.4680179036466914E-20, 2.4953109452591E-20, 2.522997749331276E-20, 2.5510912982642653E-20, 2.579605323458912E-20, 2.6085543672584608E-20, 2.6379538516522633E-20, 2.667820154666292E-20, 2.6981706955199747E-20, 2.7290240298129617E-20, 2.760399956226778E-20, 2.7923196364936903E-20, 2.8248057307096873E-20, 2.8578825504645344E-20, 2.8915762327478304E-20, 2.9259149381897149E-20, 2.9609290779395838E-20, 2.9966515744169338E-20, 3.0331181623398431E-20, 3.0703677379217488E-20, 3.108442766024987E-20, 3.1473897575051851E-20, 3.1872598321607185E-20, 3.228109386876898E-20, 3.2700008940944537E-20, 3.313003863165466E-20, 3.35719600725733E-20, 3.4026646723650843E-20, 3.4495086044066494E-20, 3.4978401579282243E-20, 3.5477880897439209E-20, 3.5995011394472534E-20, 3.6531526869552723E-20, 3.7089469133133434E-20, 3.767127106708655E-20, 3.8279871085571472E-20, 3.8918874931706407E-20, 3.9592791337014738E-20, 4.0307387768676638E-20, 4.1070251384909018E-20, 4.1891722989140241E-20, 4.2786564624839063E-20, 4.3777229834795032E-20, 4.490119402885342E-20, 4.6231235710575519E-20, 5.2372836842253038E-20, 5.4210108624275222E-20 };
    private static readonly ulong[] NextExponentialAliasBox = { 0xd6701309607c00fa, 0xb706635304f880f7, 0xe707a22b605bc0f4, 0xbb6690d043b840f2, 0xf7f0e20d4e6640ef, 0xf861ac912e35c0ed, 0xfc733e1db4d4c0eb, 0xea7c6f626bb600ed, 0xd6530fc94de280ef, 0xc606ebbf928680f0, 0xb88e65112bba80f2, 0xad3835f893548003, 0xa3894f25d92c00f4, 0x9b2975b2fba90002, 0x93d7ac60134580f5, 0x8d62f853f5de00f6, 0x87a5b38769c500f7, 0x82826dc545e100f7, 0x7de1c92824e08001, 0x79b0fa0dfb6380f8, 0x75e0b41799ab00f8, 0x72646198d58f80f9, 0x6f318ee74f9080f9, 0x6c3f7a79c27800fa, 0x6986bf9802fe00fa, 0x670114812b3700fa, 0x64a91705bf5e0000, 0x627a2400936a0000, 0x60703714cfdd0000, 0x5e87d0c784300000, 0x5cbde1897c370000, 0x5b0fb89f5ef300fc, 0x597af618aa1c00fc, 0x57fd7f361fa200fc, 0x569574c494a800fc, 0x55412b0c19c600fc, 0x53ff2307c87b00fb, 0x52ce04aae61d00fb, 0x51ac9a033e0500fb, 0x5099cb12df2700fb, 0x4f949a42401e00fb, 0x4e9c2151ca6c00fb, 0x4daf8eb65fa700fb, 0x4cce23501edc00fb, 0x4bf7306d94af00fb, 0x4b2a160fe10200fb, 0x4a6641665f8200fb, 0x49ab2b79c02900fb, 0x48f858000d6900fb, 0x484d5453dc2f00fb, 0x47a9b689eb5c00fb, 0x470d1ca1578900fb, 0x46772bcaa7a700fb, 0x45e78fc30cfc00fb, 0x455dfa4133f600fb, 0x44da22716a1d00fb, 0x445bc47f6c1d00fb, 0x43e2a12c2a3c00fb, 0x436e7d6e025600fb, 0x42ff221a76ac00fb, 0x42945b981cb100fb, 0x422df997f96c00fb, 0x41cbced566df00fb, 0x416db0dbf94f00fb, 0x411377d273ad00fb, 0x40bcfe4aa2cd00fb, 0x406a2115621200fb, 0x401abf1a58d000fb, 0x3fceb93355b300fb, 0x3f85f20a882200fb, 0x3f404dfbb77d00fb, 0x3efdb2f7ead100fb, 0x3ebe086b4eb700fb, 0x3e8137254cfd00fb, 0x3e472942828d00fb, 0x3e0fca184dd500fb, 0x3ddb062229e100fb, 0x3da8caf036b700fb, 0x3d790717509200fb, 0x3d4baa2226ba00fb, 0x3d20a4838a6e00fb, 0x3cf7e789b8f400fb, 0x3cd165528b6100fb, 0x3cad10c0885500fb, 0x3c8add70c62800fb, 0x3c6abfb16baf00fb, 0x3c4cac7900e800fb, 0x3c30995e3f4700fb, 0x3c167c90877b00fb, 0x3bfe4cd0d0fe00fb, 0x3be8016b2aa700fb, 0x3bd39230969400fb, 0x3bc0f7716a7100fb, 0x3bb029f812b300fb, 0x3ba123041a5f00fb, 0x3b93dc45b02800fb, 0x3b884fd96bd900fb, 0x3b7e78445a4900fb, 0x3b765070640100fb, 0x3b6fd3a8e46e00fb, 0x3b6afd979e3d00fb, 0x3b67ca41c8f100fb, 0x3b663605700e00fb, 0x3b663d970db500fb, 0x3b67ddff35dc00fb, 0x3b6b1498a5ce00fb, 0x3b6fdf0e54a800fb, 0x3b763b59c45000fb, 0x3b7e27c18cfc00fb, 0x3b87a2d7edea00fb, 0x3b92ab79a32a00fb, 0x3b9f40ccd1d000fb, 0x3bad6240241200fb, 0x3bbd0f8a075200fb, 0x3bce48a7fd3800fb, 0x3be10dde233400fb, 0x3bf55fb6da5c00fb, 0x3c0b3f02826000fb, 0x3c22acd7549400fb, 0x3c3baa9178a600fb, 0x3c5639d320a400fb, 0x3c725c84c0ea00fb, 0x3c9014d583c000fb, 0x3caf653bc01a00fb, 0x3cd05075a4ce00fb, 0x3cf2d989f35000fb, 0x3d1703c8e9ae00fb, 0x3d3cd2cd4b1000fb, 0x3d644a7d78bc00fb, 0x3d8d6f0cd39200fb, 0x3db844fd17c800fb, 0x3de4d11ff60a00fb, 0x3e131898d0e600fb, 0x3e4320de958c00fb, 0x3e74efbdccb200fb, 0x3ea88b5ac4ee00fb, 0x3eddfa33fd8600fb, 0x3f154324a35a00fb, 0x3f4e6d675ce600fb, 0x3f898099348600fb, 0x3fc684bcada400fb, 0x4005823d350200fb, 0x404681f2a8f400fb, 0x40898d25245600fb, 0x40cead91243000fb, 0x4115ed6bbef200fb, 0x415f576762da00fb, 0x41aaf6b8aa3000fb, 0x41f8d71b961c00fb, 0x424904d92de200fb, 0x429b8ccd588600fb, 0x42f07c6d351600fb, 0x4347e1cdc8fe00fb, 0x43a1cbab21f800fb, 0x43fe496feeae00fb, 0x445d6b3d957000fb, 0x44bf41f4c4e400fb, 0x4523df3ea8ca00fb, 0x458b5596ac7e00fb, 0x45f5b854e36400fb, 0x46631bb91c4e00fb, 0x46d394f6bcdc00fb, 0x47473a416e3c00fb, 0x47be22da94ec00fb, 0x4838671fba7000fb, 0x48b6209a002a00fb, 0x49376a0ea4be00fb, 0x49bc5f90997000fb, 0x4a451e938a8600fb, 0x4ad1c6000bb400fb, 0x4b627649699800fb, 0x4bf75184fef000fb, 0x4c907b834ce200fb, 0x4d2e19eaf70c00fb, 0x4dd05455bee000fb, 0x4e77546fd2dc00fb, 0x4f234619799800fb, 0x4fd4578b6d8a00fb, 0x508ab97e16ca00fb, 0x51469f540fb600fb, 0x52083f47eafe00fb, 0x52cfd29e0ba000fb, 0x539d95da7f5c00fb, 0x5471c8fb8e1a00fb, 0x554cafb93d9000fc, 0x562e91ca95aa00fc, 0x5717bb30e5b000fc, 0x58087c89f87800fc, 0x59012b69cb7000fc, 0x5a0222bca07000fc, 0x5b0bc3325da800fc, 0x5c1e73b4533000fc, 0x5d3aa1e671700000, 0x5e60c2b562e00000, 0x5f9152f31ac80000, 0x60ccd8035e740000, 0x6213e09a7b7c0000, 0x6367059046fc0000, 0x64c6eaca1ab40000, 0x6634403e83ec00fa, 0x67afc316678800fa, 0x693a3eef3f3c00fa, 0x6ad48f430f4800fa, 0x6c7fa0fb666400fa, 0x6e3c743590e800f9, 0x700c1e3edef400f9, 0x71efcbd1b44400f9, 0x73e8c39d0cac00f9, 0x75f86921c3c400f8, 0x78203ff3c96c00f8, 0x7a61ef6ee04000f8, 0x7cbf46f2a9740001, 0x7f3a42bc810c0001, 0x81d5117b70fc0001, 0x84921abea46c00f7, 0x87740667b2a400f7, 0x8a7dc550a9e000f6, 0x8db29b62b57c00f6, 0x91162b665fa400f5, 0x94ac84e89cb800f5, 0x987a34a5a9680002, 0x9c845807104800f4, 0xa0d0b466b73800f4, 0xa565d2f88b8400f3, 0xaa4b227a9e2800f3, 0xaf89201e3d480003, 0xb529898ac11400f2, 0xbb379a6f82dc00f1, 0xc1c058e4de2000f0, 0xc8d2f4f36d940004, 0xd081411ddbd800ef, 0xd8e04befb33800ee, 0xe20925a810280005, 0xec19e192689000ed, 0xf736e941b9c800ec, 0xfffffffffffcc0eb, 0xf84e32105543c006, 0xefc4d596acd3c0ec, 0xf04f089008e8c005, 0xe51b75a76848c0ee, 0xd9f6ddd4a2914004, 0xf4c648847f92c0f0, 0xc13cd60fe90ec0f1, 0xc2e92e87e11bc003, 0xb4ec402cc0ffc0f3, 0xa05bdee4bcda0002, 0xbf28b27170a080f5, 0xa64cb7a7255e80f6, 0xa99e6bb6d07f8001, 0xe7ae9dfdc03100f8, 0xf8596f91c66500f9, 0xd717854fe71f00fc, 0xd8d13e002ab80000, 0x00000000000000fb, 0x00000000000000fb, 0x00000000000000fb };

    /// <summary>
    /// Generates a random number from a exponential distribution.
    /// </summary>
    public double NextExponential()
    {
        const int TableSize = 256;
        const int IMax = 252;

        var uu = Next();
        ulong u1 = uu[0];
        int tableX = (int)(u1 & (TableSize - 1));

        if (tableX < IMax)
        {
            return u1 * NextExponentialX[tableX];
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static double Fallback(Culumi self, Vector128<ulong> u)
        {
            const ulong MaxConcaveRate = 0x17b3cab860ee7800;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static double SampleX(double x, int tableY) => Math.FusedMultiplyAdd(x, NextExponentialX[tableY - 1] - NextExponentialX[tableY], NextExponentialX[tableY] * (2.0 * (1ul << 63)));
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static double SampleY(double y, int tableY) => Math.FusedMultiplyAdd(y, NextExponentialY[tableY - 1] - NextExponentialY[tableY], NextExponentialY[tableY] * (2.0 * (1ul << 63)));

            double tail = 0;

            while (true)
            {
                ulong u2 = u[1];
                int tableAlias = (int)(u2 & (TableSize - 1));
                int tableY = (u2 >> 8) < (NextExponentialAliasBox[tableAlias] >> 8) ? tableAlias : (int)(NextExponentialAliasBox[tableAlias] & (TableSize - 1));

                if (tableY == 0)
                {
                    double x0 = NextExponentialX[0] * (2.0 * (1ul << 63));
                    tail += x0;

                    u = self.Next();
                    long u1 = (long)u[0];
                    int tableX = (int)(u1 & (TableSize - 1));

                    if (tableX < IMax)
                    {
                        return tail + u1 * NextExponentialX[tableX];
                    }
                }
                else
                {
                    var uu = self.Next();
                    ulong ux = uu[0];
                    ulong uy = uu[1];

                    double tx;

                    for (; ; uu = self.Next(), ux = uu[0], uy = uu[1])
                    {
                        if (uy < ux)
                        {
                            ux = ~ux;
                            uy = ~uy;
                        }

                        if (uy >= ux + MaxConcaveRate)
                        {
                            tx = SampleX(ux, tableY);
                            break;
                        }

                        tx = SampleX(ux, tableY);
                        double ty = SampleY(uy, tableY);

                        if (ty <= Math.Exp(-tx))
                        {
                            break;
                        }
                    }
                    return tail + tx;
                }
            }
        }
        return Fallback(this, uu);
    }


    /// <summary>
    /// Fills the elements of the specified span with random numbers.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void NextBytes(Span<byte> bytes)
        => Fill(bytes);

    /// <summary>
    /// Generates bytes filled with random numbers.
    /// </summary>
    public byte[] NextBytes(int length)
    {
        if (length < 0)
            ThrowLengthMinus();

        var result = new byte[length];
        Fill(result.AsSpan());
        return result;
    }

    /// <summary>
    /// Fills the elements of the specified span with random numbers.
    /// </summary>
    public void Fill<TElement>(Span<TElement> span)
        where TElement : struct
    {
        Vector128<ulong> v0 = _v0, v1 = _v1;

        var vectors = MemoryMarshal.Cast<TElement, Vector128<ulong>>(span);
        ref var vectorsHead = ref MemoryMarshal.GetReference(vectors);
        ref var vectorsTail = ref Unsafe.Add(ref vectorsHead, vectors.Length);

        while (Unsafe.IsAddressLessThan(ref vectorsHead, ref vectorsTail))
        {
            vectorsHead = NextEmbedded(ref v0, ref v1);
            vectorsHead = ref Unsafe.Add(ref vectorsHead, 1);
        }

        ref var spanTail = ref Unsafe.As<TElement, Vector128<ulong>>(
            ref Unsafe.Add(ref MemoryMarshal.GetReference(span), span.Length));

        if (Unsafe.IsAddressLessThan(ref vectorsTail, ref spanTail))
        {
            ref byte restHead = ref Unsafe.As<Vector128<ulong>, byte>(ref vectorsTail);
            ref byte restTail = ref Unsafe.As<Vector128<ulong>, byte>(ref spanTail);
            var last = NextEmbedded(ref v0, ref v1);
            ref byte lastHead = ref Unsafe.As<Vector128<ulong>, byte>(ref last);

            while (Unsafe.IsAddressLessThan(ref restHead, ref restTail))
            {
                restHead = lastHead;
                restHead = ref Unsafe.Add(ref restHead, 1);
                lastHead = ref Unsafe.Add(ref lastHead, 1);
            }
        }

        _v0 = v0;
        _v1 = v1;
    }

    /// <summary>
    /// Shuffles the order of <paramref name="span"/>.
    /// </summary>
    public void Shuffle<T>(Span<T> span)
    {
        Vector128<ulong> v0 = _v0, v1 = _v1;

        ulong factorial = 2432902008176640000;        // 20!
        int prev = 1;
        int next = Math.Min(20, span.Length);

        Vector128<ulong> rVector = Vector128<ulong>.Zero;
        int rIndex = 1;

        while (true)
        {
            ulong r = (rIndex ^= 1) == 0 ? (rVector = NextEmbedded(ref v0, ref v1))[rIndex] : rVector[rIndex];

            ulong rlo = r * factorial;
            while (rlo > 0ul - factorial)
            {
                ulong thi = Math.BigMul((rIndex ^= 1) == 0 ? (rVector = NextEmbedded(ref v0, ref v1))[rIndex] : rVector[rIndex],
                    factorial, out ulong tlo);
                ulong sum = rlo + thi;
                ulong carry = sum < thi ? 1ul : 0ul;

                if (sum != ~0ul)
                {
                    r += carry;
                    break;
                }

                rlo = tlo;
            }

            for (int k = prev; k < next; k++)
            {
                int j = (int)Math.BigMul(r, (ulong)k + 1, out r);

                ref var refJ = ref Unsafe.Add(ref MemoryMarshal.GetReference(span), j);
                ref var refK = ref Unsafe.Add(ref MemoryMarshal.GetReference(span), k);
                var swap = refJ;
                refJ = refK;
                refK = swap;
            }

            if (next >= span.Length)
                break;

            prev = next;
            factorial = 1;
            while (next < span.Length && Math.BigMul(factorial, (ulong)next + 1, out var newFactorial) == 0)
            {
                factorial = newFactorial;
                next++;
            }
        }

        _v0 = v0;
        _v1 = v1;
    }

    /// <summary>
    /// Shuffles the order of <paramref name="source"/>.
    /// </summary>
    public ShuffleIterator<TElement> Shuffle<TElement>(IEnumerable<TElement> source)
    {
        return new ShuffleIterator<TElement>(this, source);
    }

    public sealed class ShuffleIterator<TElement> : ICollection<TElement>, IEnumerator<TElement>
    {
        private readonly Culumi _rng;
        private readonly IEnumerable<TElement> _source;
        private TElement[]? _elements;
        private int _index;
        private int _length;

        private const int NotInitialized = int.MinValue;
        private const int Disposed = -2;

        internal ShuffleIterator(Culumi rng, IEnumerable<TElement> source)
        {
            _rng = rng;
            _source = source;
            _index = NotInitialized;
        }

        TElement Current => _elements![_index];
        TElement IEnumerator<TElement>.Current => Current;
        object? IEnumerator.Current => Current;

        int ICollection<TElement>.Count
        {
            get
            {
                if (_index == NotInitialized)
                    return _source.Count();

                return _length;
            }
        }
        bool ICollection<TElement>.IsReadOnly => true;

        ShuffleIterator<TElement> GetEnumerator()
        {
            if (_index == NotInitialized)
                return this;

            return new ShuffleIterator<TElement>(_rng, _source);
        }
        IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
            => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public bool MoveNext()
        {
            if ((uint)++_index < (uint)_length)
                return true;

            if (--_index == NotInitialized)
            {
                InitializeShuffling();
                return (uint)_index < (uint)_length;
            }

            return false;
        }
        bool IEnumerator.MoveNext() => MoveNext();

        private void InitializeShuffling()
        {
            if (_source.TryGetNonEnumeratedCount(out int length))
            {
                _elements = ArrayPool<TElement>.Shared.Rent(length);

                if (_source is ICollection<TElement> sourceCollectionGeneric)
                {
                    sourceCollectionGeneric.CopyTo(_elements, 0);
                }
                else if (_source is ICollection sourceCollection)
                {
                    sourceCollection.CopyTo(_elements, 0);
                }
                else
                {
                    int i = 0;
                    foreach (var element in _source)
                    {
                        _elements[i++] = element;
                    }
                }
            }
            else
            {
                length = 16;
                _elements = ArrayPool<TElement>.Shared.Rent(length);

                int i = 0;
                foreach (var element in _source)
                {
                    if (i >= _elements.Length)
                    {
                        length <<= 1;
                        if (length < 0)
                            length = Array.MaxLength;

                        var prev = _elements;
                        _elements = ArrayPool<TElement>.Shared.Rent(length);
                        prev.CopyTo(_elements, 0);
                        ArrayPool<TElement>.Shared.Return(prev, true);
                    }

                    _elements[i++] = element;
                }
                length = i;
            }

            _rng.Shuffle(_elements.AsSpan(..length));

            _index = 0;
            _length = length;
        }

        void IEnumerator.Reset() => throw new NotSupportedException();

        void ICollection<TElement>.Add(TElement item) => throw new NotSupportedException();

        void ICollection<TElement>.Clear() => throw new NotSupportedException();

        bool ICollection<TElement>.Contains(TElement item)
        {
            if (_index == NotInitialized)
            {
                return _source.Contains(item);
            }

            return _elements!.Contains(item);
        }

        void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
        {
            if (_index == NotInitialized)
            {
                InitializeShuffling();
                _elements.AsSpan(.._length).CopyTo(array.AsSpan(arrayIndex..));
                ArrayPool<TElement>.Shared.Return(_elements!, true);
                _elements = null;
                _index = NotInitialized;
                return;
            }

            for (int i = 0; i < _length; i++)
            {
                array[arrayIndex + i] = _elements![i];
            }
        }

        bool ICollection<TElement>.Remove(TElement item) => throw new NotSupportedException();

        private void Dispose(bool disposing)
        {
            if (_elements != null)
            {
                ArrayPool<TElement>.Shared.Return(_elements, true);
                _elements = null;
            }
            _index = Disposed;
        }

        ~ShuffleIterator()
        {
            Dispose(disposing: false);
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }


    /// <summary>
    /// Sets a randomly chosen element from <paramref name="source"/> to <paramref name="destination"/>.
    /// </summary>
    public void GetItems<TElement>(ReadOnlySpan<TElement> source, Span<TElement> destination)
    {
        if (source.IsEmpty)
            ThrowSourceIsEmpty();

        Vector128<ulong> v0 = _v0, v1 = _v1;

        // assumes that b is (x, x, x, x)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static (Vector128<uint> lo, Vector128<uint> hi) Multiply(in Vector128<uint> a, in Vector128<uint> b)
        {
            var mulFromLo = Sse2.Multiply(a, b).AsUInt32();
            var mulFromHi = Sse2.Multiply(Vector128.Shuffle(a, Vector128.Create(1u, 0, 3, 2)), b).AsUInt32();

            mulFromLo = Vector128.Shuffle(mulFromLo.AsUInt32(), Vector128.Create(0u, 2, 1, 3));
            mulFromHi = Vector128.Shuffle(mulFromHi.AsUInt32(), Vector128.Create(0u, 2, 1, 3));

            var mulLo = Sse2.UnpackLow(mulFromLo, mulFromHi);
            var mulHi = Sse2.UnpackHigh(mulFromLo, mulFromHi);

            return (mulLo, mulHi);
        }


        int quantity;
        uint power = 1;
        uint length = (uint)source.Length;
        int estimatedQuantity = 64 / BitOperations.Log2((ulong)length * length);

        quantity = estimatedQuantity == 0 ? int.MaxValue : estimatedQuantity;
        {
            uint powerpower = length;
            while (estimatedQuantity > 0)
            {
                if ((estimatedQuantity & 1) != 0)
                {
                    power *= powerpower;
                }
                powerpower *= powerpower;
                estimatedQuantity >>= 1;
            }
        }

        var powerVector = Vector128.Create(power, power, power, power);
        var lengthVector = Vector128.Create(length, length, length, length);
        Vector128<uint> rVector = Vector128<uint>.Zero;
        Vector128<uint> rHi = Vector128<uint>.Zero;


        int remains = 0;
        for (int i = 0; i < destination.Length; i++)
        {
            if ((i & 3) == 0)
            {
                if (remains <= 0)
                {
                    rVector = NextEmbedded(ref v0, ref v1).AsUInt32();

                    var rlo = Sse41.MultiplyLow(rVector, powerVector);
                    var failedMask = Vector128.GreaterThan(rlo, Vector128.Negate(powerVector));
                    while (Vector128.ExtractMostSignificantBits(failedMask) != 0)
                    {
                        var (tlo, thi) = Multiply(NextEmbedded(ref v0, ref v1).AsUInt32(), powerVector);
                        var sum = Vector128.Add(rlo, thi);
                        var carry = Vector128.Negate(Vector128.LessThan(sum, thi));

                        rVector = Vector128.Add(rVector, Vector128.BitwiseAnd(failedMask, carry));
                        failedMask = Vector128.Equals(Vector128.BitwiseAnd(sum, failedMask), Vector128<uint>.AllBitsSet);

                        if (Vector128.ExtractMostSignificantBits(failedMask) == 0)
                        {
                            break;
                        }

                        rlo = tlo;
                    }

                    remains = quantity;
                }

                (rVector, rHi) = Multiply(rVector, lengthVector);
                remains--;
            }

            destination[i] = source[(int)rHi[i & 3]];
        }

        _v0 = v0;
        _v1 = v1;
    }

    /// <summary>
    /// Generates an array with randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public TElement[] GetItems<TElement>(ReadOnlySpan<TElement> source, int length)
    {
        if (length < 0)
            ThrowLengthMinus();

        var result = new TElement[length];
        GetItems(source, result.AsSpan());
        return result;
    }

    /// <summary>
    /// Generates an array with randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public TElement[] GetItems<TElement>(TElement[] source, int length)
    {
        if (source == null)
            ThrowArgumentNull(nameof(source));

        return GetItems<TElement>(source.AsSpan(), length);
    }

    /// <summary>
    /// Creates an iterator (<see cref="IEnumerable{TElement}"/>) that returns randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public GetItemsIterator<TElement> GetItems<TElement>(IEnumerable<TElement> source)
    {
        return new GetItemsIterator<TElement>(this, source);
    }

    public sealed class GetItemsIterator<TElement> : IEnumerable<TElement>, IEnumerator<TElement>
    {
        private readonly Culumi _rng;
        private readonly IEnumerable<TElement> _source;

        private TElement[]? _elements;
        private int _currentIndex;
        private int _length;

        private int _quantity;
        private int _remains;
        private uint _power;
        private Vector128<uint> _entropy;
        private Vector128<uint> _entropyHi;

        internal GetItemsIterator(Culumi rng, IEnumerable<TElement> source)
        {
            _rng = rng;
            _source = source;
        }


        public TElement Current => _elements![_currentIndex];
        TElement IEnumerator<TElement>.Current => Current;
        object? IEnumerator.Current => Current;

        public GetItemsIterator<TElement> GetEnumerator()
        {
            if (_elements == null)
                return this;

            return new GetItemsIterator<TElement>(_rng, _source);
        }

        IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
            => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public bool MoveNext()
        {
            if (_elements == null)
                InitializeElements();

            if ((_remains & 3) == 0)
            {
                if (_remains <= 0)
                {
                    var powerVector = Vector128.Create(_power, _power, _power, _power);

                    _entropy = _rng.Next().AsUInt32();

                    var rlo = Sse41.MultiplyLow(_entropy, powerVector);
                    var failedMask = Vector128.GreaterThan(rlo, Vector128.Negate(powerVector));
                    while (Vector128.ExtractMostSignificantBits(failedMask) != 0)
                    {
                        var (tlo, thi) = Multiply(_rng.Next().AsUInt32(), powerVector);
                        var sum = Vector128.Add(rlo, thi);
                        var carry = Vector128.Negate(Vector128.LessThan(sum, thi));

                        _entropy = Vector128.Add(_entropy, Vector128.BitwiseAnd(failedMask, carry));
                        failedMask = Vector128.Equals(Vector128.BitwiseAnd(sum, failedMask), Vector128<uint>.AllBitsSet);

                        if (Vector128.ExtractMostSignificantBits(failedMask) == 0)
                        {
                            break;
                        }

                        rlo = tlo;
                    }

                    _remains = _quantity * 4;
                }

                (_entropy, _entropyHi) = Multiply(_entropy, Vector128.Create((uint)_length, (uint)_length, (uint)_length, (uint)_length));
            }

            _currentIndex = (int)_entropyHi[3 - (--_remains & 3)];

            return true;
        }
        bool IEnumerator.MoveNext()
            => MoveNext();

        private void InitializeElements()
        {
            (_elements, _length) = ToArrayPool(_source);

            if (_length == 0)
                ThrowSourceIsEmpty();

            int quantity;
            uint power = 1;
            int estimatedQuantity = 64 / BitOperations.Log2((ulong)_length * (ulong)_length);

            quantity = estimatedQuantity == 0 ? int.MaxValue / 4 : estimatedQuantity;
            {
                uint powerpower = (uint)_length;
                while (estimatedQuantity > 0)
                {
                    if ((estimatedQuantity & 1) != 0)
                    {
                        power *= powerpower;
                    }
                    powerpower *= powerpower;
                    estimatedQuantity >>= 1;
                }
            }

            _quantity = quantity;
            _remains = 0;
            _power = power;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (T[] array, int length) ToArrayPool<T>(IEnumerable<T> source)
        {
            T[] array;
            if (source.TryGetNonEnumeratedCount(out int length))
            {
                array = ArrayPool<T>.Shared.Rent(length);

                if (source is ICollection<T> sourceCollectionGeneric)
                {
                    sourceCollectionGeneric.CopyTo(array, 0);
                }
                else if (source is ICollection sourceCollection)
                {
                    sourceCollection.CopyTo(array, 0);
                }
                else
                {
                    int i = 0;
                    foreach (var element in source)
                    {
                        array[i++] = element;
                    }
                }
            }
            else
            {
                length = 16;
                array = ArrayPool<T>.Shared.Rent(length);

                int i = 0;
                foreach (var element in source)
                {
                    if (i >= array.Length)
                    {
                        length <<= 1;
                        if (length < 0)
                            length = Array.MaxLength;

                        var prev = array;
                        array = ArrayPool<T>.Shared.Rent(length);
                        prev.CopyTo(array, 0);
                        ArrayPool<T>.Shared.Return(prev, true);
                    }

                    array[i++] = element;
                }
                length = i;
            }
            return (array, length);
        }

        // assumes that b is (x, x, x, x)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (Vector128<uint> lo, Vector128<uint> hi) Multiply(in Vector128<uint> a, in Vector128<uint> b)
        {
            var mulFromLo = Sse2.Multiply(a, b).AsUInt32();
            var mulFromHi = Sse2.Multiply(Vector128.Shuffle(a, Vector128.Create(1u, 0, 3, 2)), b).AsUInt32();

            mulFromLo = Vector128.Shuffle(mulFromLo.AsUInt32(), Vector128.Create(0u, 2, 1, 3));
            mulFromHi = Vector128.Shuffle(mulFromHi.AsUInt32(), Vector128.Create(0u, 2, 1, 3));

            var mulLo = Sse2.UnpackLow(mulFromLo, mulFromHi);
            var mulHi = Sse2.UnpackHigh(mulFromLo, mulFromHi);

            return (mulLo, mulHi);
        }

        void IEnumerator.Reset() => throw new NotSupportedException();

        private void Dispose(bool disposing)
        {
            if (_elements != null)
            {
                ArrayPool<TElement>.Shared.Return(_elements);
                _elements = null;
            }
        }

        ~GetItemsIterator()
        {
            Dispose(disposing: false);
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (int, double) NextIntDoubleEmbedded(ref Vector128<ulong> v0, ref Vector128<ulong> v1, int intMax, double doubleMax)
    {
        int intResult;
        double doubleResult;

        var u = NextEmbedded(ref v0, ref v1);

        do
        {
            ulong range = (ulong)intMax;
            ulong hi = Math.BigMul(u[0], range, out ulong lo);
            if (lo < 0ul - range)
            {
                intResult = (int)hi;
                break;
            }
            while (true)
            {
                var r = NextEmbedded(ref v0, ref v1);

                ulong thi = Math.BigMul(r[0], range, out ulong tlo);
                ulong sum = lo + thi;
                ulong carry = sum < thi ? 1ul : 0ul;

                if (sum != ~0ul)
                {
                    intResult = (int)(hi + carry);
                    break;
                }
                lo = tlo;


                thi = Math.BigMul(r[1], range, out tlo);
                sum = lo + thi;
                carry = sum < thi ? 1ul : 0ul;

                if (sum != ~0ul)
                {
                    intResult = (int)(hi + carry);
                    break;
                }

                lo = tlo;
            }
        } while (false);

        {
            double zeroone = (u[1] >> 11) * (1.0 / (1ul << 53));
            doubleResult = zeroone * doubleMax;

            while (doubleResult >= doubleMax)
            {
                var r = NextEmbedded(ref v0, ref v1);

                var zeroone2 = Vector128.ConvertToDouble(Vector128.ShiftRightLogical(r, 11))
                    * Vector128.Create(1.0 / (1ul << 53), 1.0 / (1ul << 53));
                var result2 = zeroone2 * Vector128.Create(doubleMax, doubleMax);
                if (result2[0] < doubleMax)
                {
                    doubleResult = result2[0];
                    break;
                }
                doubleResult = result2[1];
            }
        }

        return (intResult, doubleResult);
    }

    /// <summary>
    /// Set the <paramref name="destination"/> to a weighted randomly chosen element from the <paramref name="source"/>.
    /// </summary>
    public void WeightedChoice<TElement>(ReadOnlySpan<TElement> source, ReadOnlySpan<double> weights, Span<TElement> destination)
    {
        if (source.Length == 0)
            ThrowSourceIsEmpty();

        if (weights.Length != source.Length)
            ThrowWeightsMismatch();

        Vector128<ulong> v0 = _v0, v1 = _v1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Vector128<double> NextDouble2Embedded(ref Vector128<ulong> v0, ref Vector128<ulong> v1, Vector128<double> max)
        {
            var u = NextEmbedded(ref v0, ref v1);
            var zeroone = Vector128.ConvertToDouble(Vector128.ShiftRightLogical(u, 11)) * (1.0 / (1ul << 53));
            var multiplied = zeroone * max;
            if (Vector128.LessThanAll(multiplied, max))
                return multiplied;

            [MethodImpl(MethodImplOptions.NoInlining)]
            static Vector128<double> Fallback(ref Vector128<ulong> v0, ref Vector128<ulong> v1, Vector128<double> max)
            {
                Vector128<double> multiplied;
                do
                {
                    var u = NextEmbedded(ref v0, ref v1);
                    var zeroone = Vector128.ConvertToDouble(Vector128.ShiftRightLogical(u, 11)) * (1.0 / (1ul << 53));
                    multiplied = zeroone * max;
                } while (Vector128.GreaterThanOrEqualAny(multiplied, max));
                return multiplied;
            }
            return Fallback(ref v0, ref v1, max);
        }

        if (source.Length * 2 > destination.Length * BitOperations.Log2((uint)destination.Length))
        {
            double[]? cumulativeSumArray = null;
            var cumulativeSum = source.Length <= 128
                ? stackalloc double[source.Length]
                : (cumulativeSumArray = ArrayPool<double>.Shared.Rent(source.Length)).AsSpan(..source.Length);

            double sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                if (weights[i] < 0)
                    ThrowWeightMinus();

                sum = cumulativeSum[i] = sum + weights[i];
            }
            if (!(sum > 0))
                ThrowSumOfWeightsZero();

            int dlength = destination.Length & ~1;
            for (int d = 0; d < dlength; d += 2)
            {
                var r = NextDouble2Embedded(ref v0, ref v1, Vector128.Create(sum));

                int s0 = cumulativeSum.BinarySearch(r[0]);
                destination[d + 0] = source[s0 < 0 ? ~s0 : s0];

                int s1 = cumulativeSum.BinarySearch(r[1]);
                destination[d + 1] = source[s1 < 0 ? ~s1 : s1];
            }
            if (dlength != destination.Length)
            {
                var r = NextDouble2Embedded(ref v0, ref v1, Vector128.Create(sum));

                int s0 = cumulativeSum.BinarySearch(r[0]);
                destination[^1] = source[s0 < 0 ? ~s0 : s0];
            }

            if (cumulativeSumArray != null)
                ArrayPool<double>.Shared.Return(cumulativeSumArray);

            _v0 = v0;
            _v1 = v1;
            return;
        }


        int[]? sortedArray = null;
        var sorted = source.Length <= 256
            ? stackalloc int[source.Length]
            : (sortedArray = ArrayPool<int>.Shared.Rent(source.Length)).AsSpan(..source.Length);

        double[]? firstWeightsArray = null;
        var firstWeights = source.Length <= 128
            ? stackalloc double[source.Length]
            : (firstWeightsArray = ArrayPool<double>.Shared.Rent(source.Length)).AsSpan(..source.Length);

        int[]? secondIndexesArray = null;
        var secondIndexes = source.Length <= 256
            ? stackalloc int[source.Length]
            : (secondIndexesArray = ArrayPool<int>.Shared.Rent(source.Length)).AsSpan(..source.Length);

        double averageOfWeights = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            if (weights[i] < 0)
                ThrowWeightMinus();

            firstWeights[i] = weights[i];
            averageOfWeights += weights[i];
        }
        if (!(averageOfWeights > 0))
            ThrowSumOfWeightsZero();
        averageOfWeights /= weights.Length;


        {
            int small = 0;
            int large = source.Length - 1;

            for (int i = 0; i < firstWeights.Length; i++)
            {
                if (firstWeights[i] < averageOfWeights)
                {
                    sorted[small] = i;
                    small++;
                }
                else
                {
                    sorted[large] = i;
                    large--;
                }
            }
        }

        {
            int small = 0;
            int large = source.Length - 1;
            while (small < large)
            {
                secondIndexes[sorted[small]] = sorted[large];
                firstWeights[sorted[large]] -= averageOfWeights - firstWeights[sorted[small]];

                int cmp = firstWeights[sorted[large]].CompareTo(averageOfWeights);

                if (cmp > 0)
                {
                    small++;
                }
                else if (cmp < 0)
                {
                    (sorted[small], sorted[large]) = (sorted[large], sorted[small]);
                    large--;
                }
                else
                {
                    small++;
                    firstWeights[sorted[large]] = averageOfWeights;
                    secondIndexes[sorted[large]] = sorted[large];
                    large--;
                }
            }
        }

        for (int d = 0; d < destination.Length; d++)
        {
            (int s, double w) = NextIntDoubleEmbedded(ref v0, ref v1, source.Length, averageOfWeights);
            if (w >= firstWeights[s])
                s = secondIndexes[s];

            destination[d] = source[s];
        }

        if (sortedArray != null)
            ArrayPool<int>.Shared.Return(sortedArray);
        if (firstWeightsArray != null)
            ArrayPool<double>.Shared.Return(firstWeightsArray);
        if (secondIndexesArray != null)
            ArrayPool<int>.Shared.Return(secondIndexesArray);

        _v0 = v0;
        _v1 = v1;
    }

    /// <summary>
    /// Set the <paramref name="destination"/> to a weighted randomly chosen element from the <paramref name="source"/>.
    /// </summary>
    public void WeightedChoice<TElement>(ReadOnlySpan<TElement> source, Func<TElement, double> weightSelector, Span<TElement> destination)
    {
        if (weightSelector == null)
            ThrowArgumentNull(nameof(weightSelector));

        double[]? weightsArray = null;
        var weights = source.Length <= 128
            ? stackalloc double[source.Length]
            : (weightsArray = ArrayPool<double>.Shared.Rent(source.Length)).AsSpan(..source.Length);

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = weightSelector(source[i]);
        }

        WeightedChoice(source, weights, destination);

        if (weightsArray != null)
            ArrayPool<double>.Shared.Return(weightsArray);
    }

    /// <summary>
    /// Generates an array with weighted randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public TElement[] WeightedChoice<TElement>(ReadOnlySpan<TElement> source, ReadOnlySpan<double> weights, int outputLength)
    {
        if (outputLength < 0)
            ThrowLengthMinus();

        var result = new TElement[outputLength];
        WeightedChoice(source, weights, result.AsSpan());
        return result;
    }

    /// <summary>
    /// Generates an array with weighted randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public TElement[] WeightedChoice<TElement>(ReadOnlySpan<TElement> source, Func<TElement, double> weightSelector, int outputLength)
    {
        if (weightSelector == null)
            ThrowArgumentNull(nameof(weightSelector));
        if (outputLength < 0)
            ThrowLengthMinus();

        var result = new TElement[outputLength];
        WeightedChoice(source, weightSelector, result.AsSpan());
        return result;
    }

    /// <summary>
    /// Set the <paramref name="destination"/> to a weighted randomly chosen element from the <paramref name="source"/>.
    /// </summary>
    public void WeightedChoice<TElement>(TElement[] source, double[] weights, TElement[] destination)
    {
        if (source == null)
            ThrowArgumentNull(nameof(source));
        if (weights == null)
            ThrowArgumentNull(nameof(weights));
        if (destination == null)
            ThrowArgumentNull(nameof(destination));

        WeightedChoice(source.AsSpan(), weights.AsSpan(), destination.AsSpan());
    }

    /// <summary>
    /// Set the <paramref name="destination"/> to a weighted randomly chosen element from the <paramref name="source"/>.
    /// </summary>
    public void WeightedChoice<TElement>(TElement[] source, Func<TElement, double> weightSelector, TElement[] destination)
    {
        if (source == null)
            ThrowArgumentNull(nameof(source));
        if (weightSelector == null)
            ThrowArgumentNull(nameof(weightSelector));
        if (destination == null)
            ThrowArgumentNull(nameof(destination));

        WeightedChoice(source.AsSpan(), weightSelector, destination.AsSpan());
    }

    /// <summary>
    /// Generates an array with weighted randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public TElement[] WeightedChoice<TElement>(TElement[] source, double[] weights, int outputLength)
    {
        if (source == null)
            ThrowArgumentNull(nameof(source));
        if (weights == null)
            ThrowArgumentNull(nameof(weights));
        if (outputLength < 0)
            ThrowLengthMinus();

        return WeightedChoice<TElement>(source.AsSpan(), weights.AsSpan(), outputLength);
    }

    /// <summary>
    /// Generates an array with weighted randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public TElement[] WeightedChoice<TElement>(TElement[] source, Func<TElement, double> weightSelector, int outputLength)
    {
        if (source == null)
            ThrowArgumentNull(nameof(source));
        if (weightSelector == null)
            ThrowArgumentNull(nameof(weightSelector));
        if (outputLength < 0)
            ThrowLengthMinus();

        return WeightedChoice(source.AsSpan(), weightSelector, outputLength);
    }

    /// <summary>
    /// Creates an iterator (<see cref="IEnumerable{TElement}"/>) that returns weighted randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public WeightedChoiceIterator<TElement> WeightedChoice<TElement>(IEnumerable<TElement> source, IEnumerable<double> weights)
    {
        if (source == null)
            ThrowArgumentNull(nameof(source));
        if (weights == null)
            ThrowArgumentNull(nameof(weights));

        return new WeightedChoiceIterator<TElement>(this, source, weights);
    }

    /// <summary>
    /// Creates an iterator (<see cref="IEnumerable{TElement}"/>) that returns weighted randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public WeightedChoiceIterator<TElement> WeightedChoice<TElement>(IEnumerable<TElement> source, Func<TElement, double> weightSelector)
    {
        if (source == null)
            ThrowArgumentNull(nameof(source));
        if (weightSelector == null)
            ThrowArgumentNull(nameof(weightSelector));

        return new WeightedChoiceIterator<TElement>(this, source, weightSelector);
    }

    public sealed class WeightedChoiceIterator<TElement> : IEnumerable<TElement>, IEnumerator<TElement>
    {
        private readonly Culumi _rng;
        private readonly IEnumerable<TElement> _source;
        private readonly Func<TElement, double>? _weightSelector;
        private readonly IEnumerable<double>? _weights;

        private TElement[]? _elements;
        private double[]? _firstWeights;
        private int[]? _secondIndexes;
        private int _currentIndex;
        private int _length;

        internal WeightedChoiceIterator(Culumi rng, IEnumerable<TElement> source, Func<TElement, double> weightSelector)
        {
            _rng = rng;
            _source = source;
            _weightSelector = weightSelector;
            _weights = null;
        }

        internal WeightedChoiceIterator(Culumi rng, IEnumerable<TElement> source, IEnumerable<double> weights)
        {
            _rng = rng;
            _source = source;
            _weightSelector = null;
            _weights = weights;
        }

        public WeightedChoiceIterator<TElement> GetEnumerator()
        {
            if (_elements == null)
                return this;

            if (_weightSelector != null)
                return new WeightedChoiceIterator<TElement>(_rng, _source, _weightSelector);
            else
                return new WeightedChoiceIterator<TElement>(_rng, _source, _weights!);
        }
        IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
            => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();


        TElement Current => _elements![_currentIndex];
        TElement IEnumerator<TElement>.Current => Current;

        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_elements == null)
            {
                InitializeBox();
            }

            (int s, double w) = NextIntDoubleEmbedded(ref _rng._v0, ref _rng._v1, _length, 1);

            if (w >= Indirect(_firstWeights.AsSpan(), s))
                s = Indirect(_secondIndexes.AsSpan(), s);

            _currentIndex = s;
            return true;
        }
        bool IEnumerator.MoveNext() => MoveNext();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ref T Indirect<T>(Span<T> span, int i) => ref Unsafe.Add(ref MemoryMarshal.GetReference(span), i);

        private void InitializeBox()
        {
            (_elements, int length) = ToArrayPool(_source);
            var elements = _elements.AsSpan(..length);
            _length = length;


            int[]? sortedArray = null;
            var sorted = length <= 256
                ? stackalloc int[length]
                : (sortedArray = ArrayPool<int>.Shared.Rent(length)).AsSpan(..length);

            _firstWeights = ArrayPool<double>.Shared.Rent(length);
            var firstWeights = _firstWeights.AsSpan(..length);

            _secondIndexes = ArrayPool<int>.Shared.Rent(length);
            var secondIndexes = _secondIndexes.AsSpan(..length);

            double averageOfWeights = 0;
            if (_weightSelector != null)
            {
                for (int i = 0; i < length; i++)
                {
                    Indirect(firstWeights, i) = _weightSelector(Indirect(elements, i));
                }
            }
            else
            {
                if (_weights is ICollection<double> weightsCollectionGeneric)
                {
                    if (weightsCollectionGeneric.Count != length)
                        ThrowWeightsMismatch();

                    weightsCollectionGeneric.CopyTo(_firstWeights, 0);
                }
                else if (_weights is ICollection weightsCollection)
                {
                    if (weightsCollection.Count != length)
                        ThrowWeightsMismatch();

                    weightsCollection.CopyTo(_firstWeights, 0);
                }
                else
                {
                    int i = 0;
                    foreach (var weight in _weights!)
                    {
                        Indirect(firstWeights, i) = weight;

                        if (++i >= length)
                            break;
                    }
                    if (i < length)
                        ThrowWeightsMismatch();
                }
            }

            for (int i = 0; i < length; i++)
            {
                double weight = Indirect(firstWeights, i);
                if (weight < 0)
                    ThrowWeightMinus();

                averageOfWeights += weight;
            }
            if (!(averageOfWeights > 0))
                ThrowSumOfWeightsZero();
            averageOfWeights = length / averageOfWeights;

            for (int i = 0; i < length; i++)
            {
                Indirect(firstWeights, i) *= averageOfWeights;
            }

            {
                int small = 0;
                int large = length - 1;

                for (int i = 0; i < firstWeights.Length; i++)
                {
                    if (firstWeights[i] < 1)
                    {
                        Indirect(sorted, small) = i;
                        small++;
                    }
                    else
                    {
                        Indirect(sorted, large) = i;
                        large--;
                    }
                }
            }

            {
                int small = 0;
                int large = length - 1;
                while (small < large)
                {
                    Indirect(secondIndexes, Indirect(sorted, small)) = Indirect(sorted, large);
                    Indirect(firstWeights, Indirect(sorted, large)) -= 1 - Indirect(firstWeights, Indirect(sorted, small));

                    int cmp = Indirect(firstWeights, Indirect(sorted, large)).CompareTo(1);

                    if (cmp > 0)
                    {
                        small++;
                    }
                    else if (cmp < 0)
                    {
                        var swap = Indirect(sorted, small);
                        Indirect(sorted, small) = Indirect(sorted, large);
                        Indirect(sorted, large) = swap;
                        large--;
                    }
                    else
                    {
                        small++;
                        Indirect(firstWeights, Indirect(sorted, large)) = 1;
                        Indirect(secondIndexes, Indirect(sorted, large)) = Indirect(sorted, large);
                        large--;
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (T[] array, int length) ToArrayPool<T>(IEnumerable<T> source)
        {
            T[] array;
            if (source.TryGetNonEnumeratedCount(out int length))
            {
                array = ArrayPool<T>.Shared.Rent(length);

                if (source is ICollection<T> sourceCollectionGeneric)
                {
                    sourceCollectionGeneric.CopyTo(array, 0);
                }
                else if (source is ICollection sourceCollection)
                {
                    sourceCollection.CopyTo(array, 0);
                }
                else
                {
                    int i = 0;
                    foreach (var element in source)
                    {
                        array[i++] = element;
                    }
                }
            }
            else
            {
                length = 16;
                array = ArrayPool<T>.Shared.Rent(length);

                int i = 0;
                foreach (var element in source)
                {
                    if (i >= array.Length)
                    {
                        length <<= 1;
                        if (length < 0)
                            length = Array.MaxLength;

                        var prev = array;
                        array = ArrayPool<T>.Shared.Rent(length);
                        prev.CopyTo(array, 0);
                        ArrayPool<T>.Shared.Return(prev, true);
                    }

                    array[i++] = element;
                }
                length = i;
            }
            return (array, length);
        }

        void IEnumerator.Reset() => throw new NotSupportedException();

        private void Dispose(bool disposing)
        {
            if (_elements != null)
            {
                ArrayPool<TElement>.Shared.Return(_elements);
                _elements = null;
            }
            if (_firstWeights != null)
            {
                ArrayPool<double>.Shared.Return(_firstWeights);
                _firstWeights = null;
            }
            if (_secondIndexes != null)
            {
                ArrayPool<int>.Shared.Return(_secondIndexes);
                _secondIndexes = null;
            }
        }

        ~WeightedChoiceIterator()
        {
            Dispose(disposing: false);
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }


    /// <summary>
    /// Generates a vector to a random point within the unit circle.
    /// </summary>
    public Vector2 NextInsideCircle()
    {
        while (true)
        {
            var floats = NextSignedFloat4();
            var multiplied = floats * floats;

            if (multiplied[0] + multiplied[1] <= 1)
                return new Vector2(floats[0], floats[1]);

            if (multiplied[2] + multiplied[3] <= 1)
                return new Vector2(floats[2], floats[3]);
        }
    }

    /// <summary>
    /// Generates a vector to a random point on the unit circle.
    /// </summary>
    public Vector2 NextOnCircle()
    {
        while (true)
        {
            var floats = NextSignedFloat4();
            var multiplied = floats * floats;

            float s = multiplied[0] + multiplied[1];
            if (0 < s && s <= 1)
            {
                float radius = 1 / MathF.Sqrt(s);
                return new Vector2(floats[0] * radius, floats[1] * radius);
            }

            s = multiplied[2] + multiplied[3];
            if (0 < s && s <= 1)
            {
                float radius = 1 / MathF.Sqrt(s);
                return new Vector2(floats[2] * radius, floats[3] * radius);
            }
        }
    }

    /// <summary>
    /// Generates a vector to a random point within the unit sphere.
    /// </summary>
    public Vector3 NextInsideSphere()
    {
        while (true)
        {
            var floats = NextSignedFloat4();
            var multiplied = floats * floats;

            if (multiplied[0] + multiplied[1] + multiplied[2] <= 1)
            {
                return new Vector3(floats[0], floats[1], floats[2]);
            }
        }
    }

    /// <summary>
    /// Generates a vector to a random point on the unit sphere.
    /// </summary>
    public Vector3 NextOnSphere()
    {
        while (true)
        {
            var floats = NextSignedFloat4();
            var multiplied = floats * floats;

            if (multiplied[0] + multiplied[1] <= 1)
            {
                var t = MathF.Sqrt(1 - (multiplied[0] + multiplied[1]));
                return new Vector3(2 * floats[0] * t, 2 * floats[1] * t, 1 - 2 * (multiplied[0] + multiplied[1]));
            }

            if (multiplied[2] + multiplied[3] <= 1)
            {
                var t = MathF.Sqrt(1 - (multiplied[2] + multiplied[3]));
                return new Vector3(2 * floats[2] * t, 2 * floats[3] * t, 1 - 2 * (multiplied[2] + multiplied[3]));
            }
        }
    }

    /// <summary>
    /// Generates a random <see cref="Quaternion"/>.
    /// </summary>
    public Quaternion NextQuaternion()
    {
        Vector128<float> floats;

        float v0, v1;
        do
        {
            floats = NextSignedFloat4();
            var multiplied = floats * floats;

            v0 = multiplied[0] + multiplied[1];
            v1 = multiplied[2] + multiplied[3];
        } while (v0 > 1 || v1 > 1 || v0 == 0 || v1 == 0);

        float s = MathF.Sqrt((1 - v0) / v1);

        return new Quaternion(floats[0], floats[1], s * floats[2], s * floats[3]);
    }


    [ThreadStatic]
    private static Culumi? _sharedInstance;

    /// <summary>
    /// Gets a thread-static instance of <see cref="Culumi"/>.
    /// </summary>
    public static Culumi Shared => _sharedInstance ??= new Culumi();



    [DoesNotReturn]
    private static void ThrowArgumentZero() => throw new ArgumentException("state must not be all zero");
    [DoesNotReturn]
    private static void ThrowMaxMinus() => throw new ArgumentException("max must be positive");
    [DoesNotReturn]
    private static void ThrowMinMax() => throw new ArgumentException("min must be smaller than max");
    [DoesNotReturn]
    private static void ThrowMaxIsNotFinite() => throw new ArgumentException("max must be finite");
    [DoesNotReturn]
    private static void ThrowEnumIsEmpty() => throw new ArgumentException("enum has no members");
    [DoesNotReturn]
    private static void ThrowSourceIsEmpty() => throw new ArgumentException("source is empty");
    [DoesNotReturn]
    private static void ThrowWeightsMismatch() => throw new ArgumentException("source.Length must be equal to weights.Length");
    [DoesNotReturn]
    private static void ThrowWeightMinus() => throw new ArgumentException("weight must be positive");
    [DoesNotReturn]
    private static void ThrowSumOfWeightsZero() => throw new ArgumentException("sum of weights must be positive");
    [DoesNotReturn]
    private static void ThrowArgumentNull(string name) => throw new ArgumentNullException(name);
    [DoesNotReturn]
    private static void ThrowLengthMinus() => throw new ArgumentNullException("length must be positive");
}

public static class CulumiEnumerableExtension
{
    /// <summary>
    /// Shuffles the order of <paramref name="source"/>.
    /// </summary>
    public static Culumi.ShuffleIterator<TElement> Shuffle<TElement>(this IEnumerable<TElement> source)
        => Culumi.Shared.Shuffle(source);

    /// <summary>
    /// Shuffles the order of <paramref name="source"/>.
    /// </summary>
    public static Culumi.ShuffleIterator<TElement> Shuffle<TElement>(this IEnumerable<TElement> source, Culumi rng)
        => rng.Shuffle(source);


    /// <summary>
    /// Creates an iterator (<see cref="IEnumerable{TElement}"/>) that returns randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public static Culumi.GetItemsIterator<TElement> GetItems<TElement>(this IEnumerable<TElement> source)
        => Culumi.Shared.GetItems(source);

    /// <summary>
    /// Creates an iterator (<see cref="IEnumerable{TElement}"/>) that returns randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public static Culumi.GetItemsIterator<TElement> GetItems<TElement>(this IEnumerable<TElement> source, Culumi rng)
        => rng.GetItems(source);


    /// <summary>
    /// Creates an iterator (<see cref="IEnumerable{TElement}"/>) that returns weighted randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public static Culumi.WeightedChoiceIterator<TElement> WeightedChoice<TElement>(this IEnumerable<TElement> source, IEnumerable<double> weights)
        => Culumi.Shared.WeightedChoice(source, weights);

    /// <summary>
    /// Creates an iterator (<see cref="IEnumerable{TElement}"/>) that returns weighted randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public static Culumi.WeightedChoiceIterator<TElement> WeightedChoice<TElement>(this IEnumerable<TElement> source, Func<TElement, double> weightSelector)
        => Culumi.Shared.WeightedChoice(source, weightSelector);

    /// <summary>
    /// Creates an iterator (<see cref="IEnumerable{TElement}"/>) that returns weighted randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public static Culumi.WeightedChoiceIterator<TElement> WeightedChoice<TElement>(this IEnumerable<TElement> source, Culumi rng, IEnumerable<double> weights)
        => rng.WeightedChoice(source, weights);

    /// <summary>
    /// Creates an iterator (<see cref="IEnumerable{TElement}"/>) that returns weighted randomly chosen elements from <paramref name="source"/>.
    /// </summary>
    public static Culumi.WeightedChoiceIterator<TElement> WeightedChoice<TElement>(this IEnumerable<TElement> source, Culumi rng, Func<TElement, double> weightSelector)
        => rng.WeightedChoice(source, weightSelector);
}
