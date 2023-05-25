# `Culumi` Pseudorandom Number Generator

I propose a new LFSR-based pseudorandom number generator using SIMD.

## Minimal implementation (C#)

```cs
// Internal state
private Vector128<ulong> _v0, _v1;

// Generates a 128-bit random number
public Vector128<ulong> Next()
{
    Vector128<ulong> v0 = _v0, v1 = _v1;
    var result = Vector128.AsUInt64(Vector128.Shuffle(Vector128.AsByte(v0 + v1),
        Vector128.Create(0x0100030205040706, 0x09080b0a0d0c0f0e).AsByte())) + v1;

    Vector128<ulong> clmul = Pclmulqdq.CarrylessMultiply(v0, Vector128.Create(0xbbc1b31a6451a582, 0), 0x00);

    _v0 = Vector128.Shuffle(v0 ^ v1, Vector128.Create(1ul, 0));
    _v1 = v0 ^ clmul;

    return result;
}
```

Detailed implementations:

* [C source](c/culumi.c) (requires Intel SSE4/PCLMUL/FMA, gcc/clang/MSVC compatible)
* [C# source](csharp/Culumi.cs) (.NET 7)

## Features

1. Using **SIMD**, outputs 128-bit random numbers at once.
    * Uses the [`CLMUL`](https://en.wikipedia.org/wiki/CLMUL_instruction_set) instruction to generate high-speed, high-quality random numbers.
    * In addition to speeding up by SIMD itself, it also speeds up processing after generation.
1. **Period** is 2^256 - 1. This can be proven mathematically.
1. **Equidistribution**: The output is 1-dimensionally equidistributed with 128-bit precision.
1. **Memory usage** is minimal.
    * `Culumi` only uses 256 bits == 32 bytes.
1. **Quality**: Passes many strong randomness tests.
    * [PractRand](http://pracrand.sourceforge.net/) v0.94 expanded extra (`-tf 2 -te 1`) 32 TB: no anomalies in 2417 test result(s)
    * [Hamming-weight dependencies](http://prng.di.unimi.it/hwd.php) (hwd) 1e+15 bytes p = 0.496
    * [TestU01](http://simul.iro.umontreal.ca/testu01/tu01.html) v1.2.3 BigCrush: p = [1e-7, 1 - 1e-7] through 100 runs, no systematic failures
    * [gjrand](http://gjrand.sourceforge.net/) v4.3.0.0
        * testunif 10TB (`--ten-tera`): Overall summary one sided P-value P = 0.172 : ok
        * testfunif 1TB (`--tera`): 10 out of 10 tests ok.
        * palladiumbell 1TB (`--tera`): one sided P value P = 0.84
1. **Parallelization** is easy with the jump function.
    * An operation equivalent to 2^128 or 2^192 `next()` calls is possible in constant time.
    * Virtually 2^128 non-overlapping streams of length 2^128 are available.
1. **Ease of use**: Pre-implemented useful functions in C#.
    * Generating GUID/[ULID](https://github.com/ulid/spec), shuffling, weighted selection, etc.
1. Culumi is **NOT** cryptographically secure.
    * DON'T use for cryptography or security-sensitive purposes.
    * From three consecutive 128-bit outputs, it was possible to restore the internal state in about a few hours using [Z3 solver](https://github.com/Z3Prover/z3).

## Comparison

A comparison with major PRNGs is shown below. (Fastest to slowest.)

|                                                                                                                 Name |      Period | Equidist. |       Bits | Failed Test | Stream |     Predict. | ns/KiB |
|---------------------------------------------------------------------------------------------------------------------:|------------:|----------:|-----------:|------------:|-------:|-------------:|-------:|
|                                                                                                           **Culumi** |   2^256 - 1 |   1 @ 128 |        256 |           - |   jump |     easy(Z3) |  58.68 |
|                                                                     [Shioi](https://github.com/andanteyk/prng-shioi) |   2^128 - 1 |    1 @ 64 |        128 |           - |   jump |      unknown |  81.97 |
|             [SplitMix](https://docs.oracle.com/en/java/javase/17/docs/api/java.base/java/util/SplittableRandom.html) |        2^64 |    1 @ 64 |        128 |           - | stream |   easy(math) |  91.80 |
|                                                                   [Seiran](https://github.com/andanteyk/prng-seiran) |   2^128 - 1 |    1 @ 64 |        128 |           - |   jump |     easy(Z3) |  97.26 |
|                                                                           [SFC64](https://pracrand.sourceforge.net/) |      > 2^64 |         0 |        256 |           - |      - |      unknown |  97.80 |
| [xoroshiro128++](https://docs.oracle.com/en/java/javase/17/docs/api/java.base/java/util/random/package-summary.html) |   2^128 - 1 |    1 @ 64 |        128 |           - |   jump |     easy(Z3) | 113.53 |
|                              [xoshiro256**](https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-7.0) |   2^256 - 1 |    4 @ 64 |        256 |           - |   jump | easy(matrix) | 137.79 |
|                                             [Lehmer128](https://en.wikipedia.org/wiki/Linear_congruential_generator) |       2^128 |    1 @ 64 |        128 |        TMFn |      - |    difficult | 140.67 |
|                                                                      [Shishua](https://github.com/espadrine/shishua) |      > 2^64 |         0 |       2304 |           - |      - |      unknown | 220.97 |
|                                                     [xorshift](https://docs.unity3d.com/ScriptReference/Random.html) |   2^128 - 1 |    4 @ 32 |        128 |       BRank | (jump) | easy(matrix) | 242.48 |
|                             [PCG64DXSM](https://numpy.org/doc/stable/reference/random/bit_generators/pcg64dxsm.html) |       2^128 |    1 @ 64 |        256 |           - | stream |      unknown | 308.30 |
|                                             [MT19937_32](https://en.wikipedia.org/wiki/Mersenne_Twister#Application) | 2^19937 - 1 |  623 @ 32 |      20000 |       BRank | (jump) |        heavy | 547.58 |
|                                          [SFMT19937](http://www.math.sci.hiroshima-u.ac.jp/m-mat/MT/SFMT/index.html) | 2^19937 - 1 | 155 @ 128 |      20000 |       BRank | (jump) |        heavy | 953.13 |

Note:  
* Equidist. : `x @ y` is meant `x`-dimensionally equidistributed with `y`-bit precision.

A comparison with `System.Random` is shown below.

|                               Method | Culumi (ns) | System (ns) | Speed x |
|-------------------------------------:|------------:|------------:|--------:|
|                              `new()` |      65.625 |     66.8184 |    1.02 |
|                          `new(seed)` |      3.9224 |    289.8158 |   73.89 |
|                             `Next()` |      0.9371 |      1.6193 |    1.73 |
|                          `Next(401)` |      0.9073 |      2.0801 |    2.29 |
|                     `Next(168, 401)` |      0.9559 |      0.9679 |    1.01 |
|                     `NextInt64(401)` |      0.9441 |      2.0898 |    2.21 |
|                `NextInt64(168, 401)` |       0.992 |      2.0943 |    2.11 |
|                  `NextBytes([1024])` |     59.2181 |     98.7463 |    1.67 |
|                       `NextSingle()` |      1.0112 |      1.8821 |    1.86 |
|                       `NextDouble()` |      1.0821 |      1.8491 |    1.71 |
|       `GetItems(char[16], char[32])` |     29.7815 |     77.1647 |    2.59 |
|                  `Shuffle(char[16])` |     19.2451 |     55.1565 |    2.87 |
|             `NextBigInteger(2^64-1)` |     32.5591 |         N/A |       - |
|          `NextProbablePrime(2^64-1)` |  247,684.12 |         N/A |       - |
|                         `NextInt4()` |      1.6784 |   * 10.4402 |    6.22 |
|                       `NextULong2()` |      2.2805 |    * 5.1849 |    2.27 |
|                      `NextDouble2()` |      1.0592 |    * 4.7126 |    4.45 |
|                       `NextFloat4()` |      1.1355 |    * 9.4965 |    8.36 |
|                     `NextBool(0.25)` |      1.0455 |    * 2.1795 |    2.08 |
|                     `NextGaussian()` |      1.4223 |         N/A |       - |
|                  `NextExponential()` |      6.0921 |         N/A |       - |
|                         `NextGuid()` |      0.9709 |  ** 38.8405 |   40.00 |
|                         `NextUlid()` |     39.2406 |         N/A |       - |
| `WeightedChoice(char[16], char[32])` |    441.1448 |         N/A |       - |
|                           `Jump64()` |    189.3694 |         N/A |       - |
|                          `Jump128()` |    187.7919 |         N/A |       - |
|                          `Jump192()` |    188.8253 |         N/A |       - |
|                             `Prev()` |      1.5549 |         N/A |       - |
|                 `NextInsideCircle()` |      4.5625 |         N/A |       - |
|                 `NextInsideSphere()` |     14.3452 |         N/A |       - |
|                     `NextOnCircle()` |      5.0074 |         N/A |       - |
|                     `NextOnSphere()` |      5.3887 |         N/A |       - |
|                   `NextQuaternion()` |     10.4413 |         N/A |       - |

Note: `*` is emulated by straightforward implementation, `**` is `Guid.NewGuid()`.

The measurement environment is:

```
BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1555/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-12700F, 1 CPU, 20 logical and 12 physical cores
.NET SDK=8.0.100-preview.3.23178.7
  [Host]   : .NET 8.0.0 (8.0.23.17408), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.0 (8.0.23.17408), X64 RyuJIT AVX2
```

## License

Public Domain (CC0)  
https://creativecommons.org/publicdomain/zero/1.0/

