/* Culumi256 v1 - pseudorandom number generator

To the extent possible under law, the author has waived all copyright
and related or neighboring rights to this software.
See: https://creativecommons.org/publicdomain/zero/1.0/

To compile:
gcc -Wall -O3 culumi.c -msse4 -mpclmul -mfma

for unity,
#define BUILD_FOR_UNITY_DLL
*/

#include <inttypes.h>
#include <time.h>
#include <stdio.h>

#if __clang__ || __GNUC__
#include <x86intrin.h>
#define _mm_setr_epi64x(v0, v1) _mm_set_epi64x((v1), (v0))
static inline uint64_t Multiply128(uint64_t x, uint64_t y, uint64_t *hi)
{
    unsigned __int128 mul = (unsigned __int128)x * y;
    *hi = (uint64_t)(mul >> 64);
    return (uint64_t)mul;
}

#elif _MSC_VER
#define _CRT_RAND_S
#include <stdlib.h>
#include <intrin.h>
static inline uint64_t Multiply128(uint64_t x, uint64_t y, uint64_t *hi)
{
    return _umul128(x, y, hi);
}

#endif

#if defined(BUILD_FOR_UNITY_DLL) && defined(_WIN32)
#define DLLEXPORT __declspec(dllexport)
#else
#define DLLEXPORT
#endif

// Internal state
typedef struct
{
    __m128i s[2];
} culumi_t;

// Initializes internal state.
DLLEXPORT void init(culumi_t *rng)
{
#if __clang__ || __GNUC__
    {
        FILE *random = fopen("/dev/urandom", "rb");
        if (random == NULL)
        {
            goto fallback;
        }
        do
        {
            if (fread(rng->s, 1, sizeof(rng->s), random) != sizeof(rng->s))
            {
                goto fallback;
            }
        } while (_mm_testz_si128(rng->s[0], rng->s[0]) && _mm_testz_si128(rng->s[1], rng->s[1]));
        fclose(random);

        return;
    }
#elif _MSC_VER
    {
        uint32_t *ss = (uint32_t *)rng->s;
        do
        {
            for (int i = 0; i < 8; i++)
            {
                if (rand_s(ss + i))
                {
                    goto fallback;
                }
            }
        } while (_mm_testz_si128(rng->s[0], rng->s[0]) && _mm_testz_si128(rng->s[1], rng->s[1]));

        return;
    }
#endif

    {
    fallback:;
        // This method SHOULD NOT be used as it lacks entropy
        uint64_t seed = time(NULL) | (uint64_t)clock() << 32;

        uint64_t a = seed = seed * 6364136223846793005 + 1442695040888963407,
                 b = seed = seed * 6364136223846793005 + 1442695040888963407,
                 c = seed = seed * 6364136223846793005 + 1442695040888963407,
                 d = seed = seed * 6364136223846793005 + 1442695040888963407;

        rng->s[0] = _mm_setr_epi64x(a, b);
        rng->s[1] = _mm_setr_epi64x(c, d);
    }
}

// Initializes internal state with user-specified values.
DLLEXPORT void initState(culumi_t *rng, uint64_t a, uint64_t b, uint64_t c, uint64_t d)
{
    if (a == 0 && b == 0 && c == 0 && d == 0)
    {
        fprintf(stderr, "state must not be all zero\n");
        exit(-1);
    }

    rng->s[0] = _mm_setr_epi64x(a, b);
    rng->s[1] = _mm_setr_epi64x(c, d);
}

// Generates 128-bit random numbers.
// note: _mm_setr_epi64x is (lo, hi)
DLLEXPORT inline __m128i next(culumi_t *rng)
{
    __m128i v0 = rng->s[0];
    __m128i v1 = rng->s[1];

    __m128i result = _mm_add_epi64(
        _mm_shuffle_epi8(
            _mm_add_epi64(v0, v1),
            _mm_setr_epi64x(0x0100030205040706, 0x09080b0a0d0c0f0e)),
        v1);

    __m128i clmul = _mm_clmulepi64_si128(v0, _mm_setr_epi64x(0xbbc1b31a6451a582, 0), 0);

    __m128i n0 = _mm_shuffle_epi32(_mm_xor_si128(v0, v1), 0x4e);
    __m128i n1 = _mm_xor_si128(v0, clmul);

    rng->s[0] = n0;
    rng->s[1] = n1;

    return result;
}

static inline void jump(culumi_t *rng, const uint64_t jumppoly[])
{
    __m128i v0 = _mm_setzero_si128();
    __m128i v1 = _mm_setzero_si128();

    for (int i = 0; i < 4; i++)
    {
        for (int b = 0; b < 64; b++)
        {
            if ((jumppoly[i] & (uint64_t)1 << b) != 0)
            {
                v0 = _mm_xor_si128(v0, rng->s[0]);
                v1 = _mm_xor_si128(v1, rng->s[1]);
            }
            next(rng);
        }
    }

    rng->s[0] = v0;
    rng->s[1] = v1;
}

// Equivalent to 2^64 calls of next().
DLLEXPORT void jump64(culumi_t *rng)
{
    uint64_t jumppoly[4] = {0x5601375EC36230E1, 0x79CF0DE79B070769, 0x51407AE5A16EA33B, 0x708C91D747D77FE3};
    jump(rng, jumppoly);
}

// Equivalent to 2^128 calls of next().
DLLEXPORT void jump128(culumi_t *rng)
{
    uint64_t jumppoly[4] = {0x6C81827A1CBDFCCF, 0x7E438EDA9627E879, 0x15123909CF74EB17, 0xA7C9C89160D05C3E};
    jump(rng, jumppoly);
}

// Equivalent to 2^192 calls of next().
DLLEXPORT void jump192(culumi_t *rng)
{
    uint64_t jumppoly[4] = {0xE03ABAC0D7F32901, 0x176EBE5A39A97EE5, 0x92B41C08DDEE8EAE, 0x9C1C03167238346D};
    jump(rng, jumppoly);
}

// Generates a random number in the range [0, range)
DLLEXPORT uint64_t nextRange(culumi_t *rng, uint64_t range)
{
    __m128i value = next(rng);

    uint64_t hi;
    uint64_t lo = Multiply128(_mm_extract_epi64(value, 0), range, &hi);

    if (lo < 0 - range)
        return hi;

    uint64_t thi;
    uint64_t tlo = Multiply128(_mm_extract_epi64(value, 1), range, &thi);
    uint64_t sum = lo + thi;
    uint64_t carry = sum < thi;

    if (sum != ~(uint64_t)0)
        return hi + carry;

    while (1)
    {
        value = next(rng);

        tlo = Multiply128(_mm_extract_epi64(value, 0), range, &thi);
        sum = lo + thi;
        carry = sum < thi;

        if (sum != ~(uint64_t)0)
            return hi + carry;

        lo = tlo;

        tlo = Multiply128(_mm_extract_epi64(value, 1), range, &thi);
        sum = lo + thi;
        carry = sum < thi;

        if (sum != ~(uint64_t)0)
            return hi + carry;

        lo = tlo;
    }
}

// Generates a uint64_t random number in the range [min, max)
DLLEXPORT uint64_t nextULong(culumi_t *rng, uint64_t min, uint64_t max)
{
    return min + nextRange(rng, max - min);
}

// Generates a int64_t random number in the range [min, max)
DLLEXPORT int64_t nextLong(culumi_t *rng, int64_t min, int64_t max)
{
    return min + nextRange(rng, max - min);
}

// Generates a int32_t random number in the range [min, max)
DLLEXPORT int32_t nextInt(culumi_t *rng, int32_t min, int32_t max)
{
    return (int32_t)nextLong(rng, min, max);
}

// Generates 4 uint32_t random numbers in the range [0, range)
DLLEXPORT __m128i nextUIntRange4(culumi_t *rng, __m128i range4)
{
    __m128i allBitsSet = _mm_cmpeq_epi32(_mm_setzero_si128(), _mm_setzero_si128());

    __m128i r = next(rng);
    __m128i mulFromLo = _mm_mul_epu32(_mm_shuffle_epi32(r, 0xb1), range4);
    __m128i mulFromHi = _mm_mul_epu32(r, _mm_shuffle_epi32(range4, 0xb1));

    mulFromLo = _mm_shuffle_epi32(mulFromLo, 0xd8);
    mulFromHi = _mm_shuffle_epi32(mulFromHi, 0xd8);

    __m128i mulLo = _mm_unpacklo_epi32(mulFromLo, mulFromHi);
    __m128i mulHi = _mm_unpackhi_epi32(mulFromLo, mulFromHi);

    // -range4 < mulLo
    __m128i failedMask = _mm_xor_si128(_mm_cmpeq_epi32(_mm_min_epu32(_mm_sign_epi32(range4, allBitsSet), mulLo), mulLo), allBitsSet);
    if (_mm_movemask_epi8(_mm_cmpeq_epi32(failedMask, _mm_setzero_si128())) == 0xffff)
        return mulHi;

    while (1)
    {
        r = next(rng);

        mulFromLo = _mm_mul_epu32(_mm_shuffle_epi32(r, 0xb1), range4);
        mulFromHi = _mm_mul_epu32(r, _mm_shuffle_epi32(range4, 0xb1));

        mulFromLo = _mm_shuffle_epi32(mulFromLo, 0xd8);
        mulFromHi = _mm_shuffle_epi32(mulFromHi, 0xd8);

        __m128i mulTLo = _mm_unpacklo_epi32(mulFromLo, mulFromHi);
        __m128i mulTHi = _mm_unpackhi_epi32(mulFromLo, mulFromHi);

        __m128i sum = _mm_add_epi32(mulLo, mulTHi);
        // -(sum < mulTHi)
        __m128i carry = _mm_sign_epi32(_mm_xor_si128(_mm_cmpeq_epi32(_mm_min_epu32(sum, mulTHi), mulTHi), allBitsSet), allBitsSet);

        mulHi = _mm_add_epi32(mulHi, _mm_and_si128(failedMask, carry));

        failedMask = _mm_cmpeq_epi32(_mm_and_si128(sum, failedMask), allBitsSet);
        if (_mm_movemask_epi8(_mm_cmpeq_epi32(failedMask, _mm_setzero_si128())) == 0xffff)
            return mulHi;
        else
            mulLo = mulTLo;
    }
}

// Generates 4 uint32_t random numbers in the range [min, max)
DLLEXPORT __m128i nextUInt4(culumi_t *rng, __m128i min4, __m128i max4)
{
    return _mm_add_epi32(min4, nextUIntRange4(rng, _mm_sub_epi32(max4, min4)));
}

// Generates 4 int32_t random numbers in the range [min, max)
DLLEXPORT __m128i nextInt4(culumi_t *rng, __m128i min4, __m128i max4)
{
    return _mm_add_epi32(min4, nextUIntRange4(rng, _mm_sub_epi32(max4, min4)));
}

// Generates 2 double random numbers in the range [min, max)
DLLEXPORT __m128d nextDouble2(culumi_t *rng, __m128d min2, __m128d max2)
{
    __m128d result;
    __m128d successMask;
    {
        __m128i shifted = _mm_srli_epi64(next(rng), 11);
        __m128i lower = _mm_or_si128(_mm_and_si128(shifted, _mm_set1_epi64x(0xffffffff)), _mm_set1_epi64x(0x4330000000000000));
        __m128i upper = _mm_or_si128(_mm_srli_epi64(shifted, 32), _mm_set1_epi64x(0x4530000000000000));
        __m128d converted = _mm_add_pd(_mm_sub_pd(_mm_castsi128_pd(upper), _mm_castsi128_pd(_mm_set1_epi64x(0x4530000000100000))), _mm_castsi128_pd(lower));
        __m128d zeroone = _mm_mul_pd(converted, _mm_set1_pd(0x1.0p-53));

        result = _mm_fmadd_pd(zeroone, _mm_sub_pd(max2, min2), min2);
        successMask = _mm_cmplt_pd(result, max2);
        if (_mm_movemask_pd(successMask) == 3)
            return result;
    }

    do
    {
        __m128i shifted = _mm_srli_epi64(next(rng), 11);
        __m128i lower = _mm_or_si128(_mm_and_si128(shifted, _mm_set1_epi64x(0xffffffff)), _mm_set1_epi64x(0x4330000000000000));
        __m128i upper = _mm_or_si128(_mm_srli_epi64(shifted, 32), _mm_set1_epi64x(0x4530000000000000));
        __m128d converted = _mm_add_pd(_mm_sub_pd(_mm_castsi128_pd(upper), _mm_castsi128_pd(_mm_set1_epi64x(0x4530000000100000))), _mm_castsi128_pd(lower));
        __m128d zeroone = _mm_mul_pd(converted, _mm_set1_pd(0x1.0p-53));

        __m128d newResult = _mm_fmadd_pd(_mm_sub_pd(_mm_set1_pd(1), zeroone), min2, _mm_mul_pd(zeroone, min2));

        __m128d updateFlag = _mm_xor_pd(_mm_cmpeq_pd(_mm_setzero_pd(), _mm_setzero_pd()),
                                        _mm_or_pd(_mm_cmpge_pd(newResult, max2), successMask));
        result = _mm_xor_pd(_mm_and_pd(newResult, updateFlag), _mm_andnot_pd(result, updateFlag));
        successMask = _mm_or_pd(successMask, updateFlag);
    } while (_mm_movemask_pd(successMask) != 3);

    return result;
}

// Generates 4 float random numbers in the range [min, max)
DLLEXPORT __m128 nextFloat4(culumi_t *rng, __m128 min4, __m128 max4)
{
    __m128 result;
    __m128 successMask;

    {
        __m128 zeroone = _mm_mul_ps(_mm_cvtepi32_ps(_mm_srli_epi32(next(rng), 8)), _mm_set1_ps(0x1.0p-24f));
        result = _mm_fmadd_ps(zeroone, _mm_sub_ps(max4, min4), min4);
        successMask = _mm_cmplt_ps(result, max4);

        if (_mm_movemask_ps(successMask) == 15)
            return result;
    }

    do
    {
        __m128 zeroone = _mm_mul_ps(_mm_cvtepi32_ps(_mm_srli_epi32(next(rng), 8)), _mm_set1_ps(0x1.0p-24f));
        __m128 newResult = _mm_fmadd_ps(_mm_sub_ps(_mm_set1_ps(1), zeroone), min4, _mm_mul_ps(zeroone, max4));
        successMask = _mm_cmplt_ps(result, max4);

        __m128 updateFlag = _mm_xor_ps(_mm_cmpeq_ps(_mm_setzero_ps(), _mm_setzero_ps()),
                                       _mm_or_ps(_mm_cmpge_ps(newResult, max4), successMask));
        result = _mm_xor_ps(_mm_and_ps(newResult, updateFlag), _mm_andnot_ps(result, updateFlag));
        successMask = _mm_or_ps(successMask, updateFlag);
    } while (_mm_movemask_ps(successMask) != 15);

    return result;
}

// Fills the specified byte sequence with random numbers
DLLEXPORT void nextBytes(culumi_t *rng, uint8_t *bytes, size_t length)
{
    __m128i *words = (__m128i *)bytes;
    size_t wordLength = length >> 4;
    for (size_t i = 0; i < wordLength; i++)
    {
        *words++ = next(rng);
    }

    size_t remains = length & 0xf;
    if (remains > 0)
    {
        uint8_t *tail = (uint8_t *)words;
        uint8_t last[16];
        *(__m128i *)last = next(rng);

        for (size_t i = 0; i < remains; i++)
            *tail++ = last[i];
    }
}

DLLEXPORT void prev(culumi_t *rng)
{
    __m128i p0l = _mm_clmulepi64_si128(rng->s[1], _mm_setr_epi64x(0x4D12E2CABE3FB47F, 0), 0x00);
    __m128i pcl = _mm_clmulepi64_si128(p0l, _mm_setr_epi64x(0xBBC1B31A6451A582, 0), 0x00);

    __m128i p0 = _mm_xor_si128(rng->s[1], pcl);
    __m128i p1 = _mm_xor_si128(p0, _mm_shuffle_epi32(rng->s[0], 0x4e));

    rng->s[0] = p0;
    rng->s[1] = p1;
}

#if BUILD_FOR_UNITY_DLL

DLLEXPORT void nextForUnity(culumi_t *rng, __m128i *output)
{
    *output = next(rng);
}

DLLEXPORT void nextUInt4ForUnity(culumi_t *rng, __m128i *min4, __m128i *max4, __m128i *output)
{
    *output = nextUInt4(rng, *min4, *max4);
}

DLLEXPORT void nextInt4ForUnity(culumi_t *rng, __m128i *min4, __m128i *max4, __m128i *output)
{
    *output = nextInt4(rng, *min4, *max4);
}

DLLEXPORT void nextDouble2ForUnity(culumi_t *rng, __m128d *min2, __m128d *max2, __m128d *output)
{
    *output = nextDouble2(rng, *min2, *max2);
}

DLLEXPORT void nextFloat4ForUnity(culumi_t *rng, __m128 *min4, __m128 *max4, __m128 *output)
{
    *output = nextFloat4(rng, *min4, *max4);
}
#endif

// ----------------------------
// The test code is shown below
// ----------------------------

#define ARE_EQUAL(v0, v1) _mm_test_all_ones(_mm_cmpeq_epi8((v0), (v1)))
#define ARE_EQUAL_PD(v0, v1) (_mm_movemask_pd(_mm_cmp_pd((v0), (v1), _CMP_EQ_OS)) == 0x3)
#define ARE_EQUAL_PS(v0, v1) (_mm_movemask_ps(_mm_cmp_ps((v0), (v1), _CMP_EQ_OS)) == 0xf)

#define ASSERT_STATE(state0, state1)        \
    ASSERT(ARE_EQUAL(rng.s[0], (state0)) && \
           ARE_EQUAL(rng.s[1], (state1)))

#define ASSERT(pred)              \
    if (!(pred))                  \
    {                             \
        puts("assertion failed"); \
        return -1;                \
    }
#define PRINT_STATE(title) printf(title " %016" PRIx64 " %016" PRIx64 " %016" PRIx64 " %016" PRIx64 "\n",             \
                                  (uint64_t)_mm_extract_epi64(rng.s[0], 0), (uint64_t)_mm_extract_epi64(rng.s[0], 1), \
                                  (uint64_t)_mm_extract_epi64(rng.s[1], 0), (uint64_t)_mm_extract_epi64(rng.s[1], 1))

int main(int argc, char **argv)
{
    culumi_t rng;

    initState(&rng, 0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu, 0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u);
    PRINT_STATE("init :");
    ASSERT_STATE(_mm_setr_epi64x(0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu),
                 _mm_setr_epi64x(0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u));

    printf("next :\n");
    {
        const __m128i answers[] = {
            _mm_setr_epi64x(0x3c8a13af1c8ef222u, 0x343035084213f4cbu),
            _mm_setr_epi64x(0xd26f9b55e3f3ea59u, 0x4f8166992c65e22au),
            _mm_setr_epi64x(0x4cf8d9aefc4cd0beu, 0x39c10e0031960c8bu),
            _mm_setr_epi64x(0x158f8ea2f71ebcc4u, 0x78ff49f82cd1f205u),
            _mm_setr_epi64x(0xa05818df15148123u, 0x6147390b9b63e49bu),
            _mm_setr_epi64x(0xac616aa06ccb5cdcu, 0x0a0b3eb16b978068u),
            _mm_setr_epi64x(0x1fb12a559476f296u, 0xc12be6e152a3f1ebu),
            _mm_setr_epi64x(0x7cdd34995203a81au, 0x3b8af000008cb180u),
            _mm_setr_epi64x(0x6eb426a9606a863du, 0xdc387d569d41248bu),
            _mm_setr_epi64x(0x4646ea9ca904f2a8u, 0xffc8edb9d4341955u),
            _mm_setr_epi64x(0xc9b8714c7d36a18cu, 0x2ee0ce1152638a8bu),
            _mm_setr_epi64x(0x7a3dc7f65d918c6eu, 0xc91e7709b75a2b8cu),
            _mm_setr_epi64x(0xd339134ca469ccacu, 0x949872c2fcfc29a8u),
            _mm_setr_epi64x(0x0f5af5b7c48dcac9u, 0xa3928345fc6281d0u),
            _mm_setr_epi64x(0x288409d4aabb18f9u, 0x66711e336eebbbbcu),
            _mm_setr_epi64x(0xd2a0815182d73662u, 0x0989f87d17675c50u),
        };

        for (int i = 0; i < 16; i++)
        {
            __m128i value = next(&rng);
            printf("       %016" PRIx64 " %016" PRIx64 " \n",
                   (uint64_t)_mm_extract_epi64(value, 0), (uint64_t)_mm_extract_epi64(value, 1));
            ASSERT(ARE_EQUAL(value, answers[i]));
        }
    }
    puts("");

    initState(&rng, 0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu, 0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u);
    jump64(&rng);
    PRINT_STATE("jp 64:");
    ASSERT_STATE(_mm_setr_epi64x(0xb4ae34f360f7dd61u, 0x1b6b93a51e284710u),
                 _mm_setr_epi64x(0x0c5bab58050abf53u, 0x92ed4e9a6e4db9fbu));

    initState(&rng, 0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu, 0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u);
    jump128(&rng);
    PRINT_STATE("jp128:");
    ASSERT_STATE(_mm_setr_epi64x(0x12b5566ceafdd0c9u, 0xfb978ee2b429ee53u),
                 _mm_setr_epi64x(0x902b9e77ba34c2a0u, 0xdd3a2fbb67b23028u));

    initState(&rng, 0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu, 0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u);
    jump192(&rng);
    PRINT_STATE("jp192:");
    ASSERT_STATE(_mm_setr_epi64x(0xd6ff1d41eb6dd5afu, 0x67928f8822d06129u),
                 _mm_setr_epi64x(0x78a5a83d15b6a940u, 0x68d2523f25f972efu));

    {
        printf("\nnulo : \n");
        initState(&rng, 0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu, 0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u);

        const uint64_t answers[16] = {
            2818387170813767114,
            3066249881389940047,
            2845559606332448740,
            2753933810602404407,
            2983420132235483212,
            3003323022849182355,
            2770686472112866968,
            2924751822060184470,
            2901336832021137767,
            2834488946509501173,
            3051838321926071492,
            2920414950715017221,
            3067551209561700318,
            2743672725850506399,
            2785277008725428884,
            3066565726263576259};

        for (int i = 0; i < 16; i++)
        {
            uint64_t value = nextULong(&rng, 2718281828459045235, 3141592653589793238);
            printf("       %" PRId64 "\n", value);
            ASSERT(answers[i] == value);
        }

        puts("");
        PRINT_STATE("nu64 :");
    }

    {
        printf("\nnui4 : \n");
        initState(&rng, 0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu, 0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u);

        const __m128i answers[16] = {
            _mm_setr_epi32(45797570, 4077102, 580445216, 1125165173u),
            _mm_setr_epi32(119130394, 15687383, 866982411, 894986046u),
            _mm_setr_epi32(26728357, 16803449, 1302200741, 899467717u),
            _mm_setr_epi32(100512812, 8749061, 138381238, 1565923199u),
            _mm_setr_epi32(31684811, 11058969, 2059256407, 1301010960u),
            _mm_setr_epi32(70339111, 8028302, 2342982563u, 2093198237u),
            _mm_setr_epi32(114866580, 9705103, 524745443, 1298339930u),
            _mm_setr_epi32(75983576, 7862495, 2142620037, 2370281639u),
            _mm_setr_epi32(119515406, 11987622, 1591686640, 3109582112u),
            _mm_setr_epi32(23692506, 13859096, 1748782546, 3103209525u),
            _mm_setr_epi32(36001576, 12355480, 1107568539, 1601267954u),
            _mm_setr_epi32(133547837, 10799432, 1084524877, 3118167619u),
            _mm_setr_epi32(60723499, 4203904, 1020732605, 783527317u),
            _mm_setr_epi32(63730085, 8862900, 1605502197, 1627668613u),
            _mm_setr_epi32(16864124, 6508329, 584317206, 1131852673u),
            _mm_setr_epi32(116200841, 12101674, 836676968, 2490105490u)};

        for (int i = 0; i < 16; i++)
        {
            __m128i value = nextUInt4(&rng, _mm_setr_epi32(16180339u, 2414213, 33027756, 423606797), _mm_setr_epi32(141421356, 17320508, 2718281828u, 3141592653u));
            printf("       %" PRId32 " %" PRId32 " %" PRId32 " %" PRId32 "\n",
                   _mm_extract_epi32(value, 0), _mm_extract_epi32(value, 1), _mm_extract_epi32(value, 2), _mm_extract_epi32(value, 3));
            ASSERT(_mm_movemask_epi8(_mm_cmpeq_epi32(answers[i], value)) == 0xffff);
        }

        PRINT_STATE("nui4 :");
    }

    {
        printf("\nnsi4 : \n");
        initState(&rng, 0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu, 0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u);

        const __m128i answers[16] = {
            _mm_setr_epi32(21089607, -212683, 29120353, -233176829),
            _mm_setr_epi32(113370694, 15158383, 61650818, -295656311),
            _mm_setr_epi32(31206102, 17035284, 35748362, -280704598),
            _mm_setr_epi32(-2906848, 16635965, 111061000, -294439813),
            _mm_setr_epi32(82532577, -789175, 82815373, 24212035),
            _mm_setr_epi32(89942568, 5972610, -21067013, -113538122),
            _mm_setr_epi32(3330290, 9030741, 197009260, -185445509),
            _mm_setr_epi32(51972368, 5018384, 229220590, 29584634),
            _mm_setr_epi32(27084415, 10615276, 271572004, 187942222),
            _mm_setr_epi32(108005166, 7238332, 22796777, -186170529),
            _mm_setr_epi32(59075290, 4798869, 206473504, 104795743),
            _mm_setr_epi32(-6727124, 12737880, 161761274, 303740605),
            _mm_setr_epi32(113488285, 7672131, -21668362, -356159554),
            _mm_setr_epi32(35426813, 1802416, -7036830, -54852504),
            _mm_setr_epi32(39872205, -44808, 79106023, -325910504),
            _mm_setr_epi32(43655653, 6123324, 145494707, -96778038),
        };

        for (int i = 0; i < 16; i++)
        {
            __m128i value = nextInt4(&rng, _mm_setr_epi32(-16180339, -2414213, -33027756, -423606797), _mm_setr_epi32(141421356, 17320508, 271828182, 314159265));
            printf("       %" PRId32 " %" PRId32 " %" PRId32 " %" PRId32 "\n",
                   _mm_extract_epi32(value, 0), _mm_extract_epi32(value, 1), _mm_extract_epi32(value, 2), _mm_extract_epi32(value, 3));
            ASSERT(_mm_movemask_epi8(_mm_cmpeq_epi32(answers[i], value)) == 0xffff);
        }

        PRINT_STATE("nsi4 :");
    }

    {
        printf("ndbl :\n");
        initState(&rng, 0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu, 0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u);

        const __m128d answers[16] = {
            _mm_setr_pd(-0.12069369996794585, -1.860698834751835),
            _mm_setr_pd(2.0564852379987966, -1.1902347938029496),
            _mm_setr_pd(0.11798380891192378, -1.7240932884024611),
            _mm_setr_pd(-0.6868397652924335, -0.1718740199695603),
            _mm_setr_pd(1.3289244650331853, -0.7540260671643433),
            _mm_setr_pd(1.5037476732038426, -2.895077636749329),
            _mm_setr_pd(-0.5396875707946, 1.599549059013835),
            _mm_setr_pd(0.8135931796500778, -1.6801943238403036),
            _mm_setr_pd(0.6079203576785496, 2.263435592613389),
            _mm_setr_pd(0.02074123724841072, 3.136312750850385),
            _mm_setr_pd(1.9298968389005495, -1.991029888887314),
            _mm_setr_pd(0.7754989260714705, 1.7946103547781143),
            _mm_setr_pd(2.067915857313631, 0.5054896486613402),
            _mm_setr_pd(-0.7769711834093126, 0.8730759787097281),
            _mm_setr_pd(-0.41152707044075554, -0.6272909579924542),
            _mm_setr_pd(2.059259559335584, -2.9074716488964727)};

        for (int i = 0; i < 16; i++)
        {
            __m128d value = nextDouble2(&rng, _mm_setr_pd(-1.0, -3.14159265358979323846), _mm_setr_pd(2.7182818284590452354, 3.14159265358979323846));

            double lo, hi;
            _mm_storel_pd(&lo, value);
            _mm_storeh_pd(&hi, value);

            printf("       %.16g %.16g\n", lo, hi);

            ASSERT(ARE_EQUAL_PD(answers[i], value));
        }

        PRINT_STATE("ndbl :");
    }

    {
        printf("nflt :\n");
        initState(&rng, 0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu, 0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u);

        const __m128 answers[16] = {
            _mm_setr_ps(2.0801287f, 1.5064478f, 0.8799137f, 0.44632697f),
            _mm_setr_ps(2.6395872f, 2.7604225f, 0.26312f, 1.7268186f),
            _mm_setr_ps(2.7079005f, 1.6439172f, 0.41072232f, 0.70722437f),
            _mm_setr_ps(2.6933665f, 1.1803687f, 0.27512926f, 3.671744f),
            _mm_setr_ps(2.0591462f, 2.3413742f, 3.4208424f, 2.5599163f),
            _mm_setr_ps(2.305254f, 2.442066f, 2.0609806f, -1.5291915f),
            _mm_setr_ps(2.4165602f, 1.2651229f, 1.3511146f, 7.054913f),
            _mm_setr_ps(2.2301147f, 2.0445626f, -0.9843646f, 0.7910652f),
            _mm_setr_ps(2.270523f, 1.9261025f, 3.4738803f, 8.322844f),
            _mm_setr_ps(2.4742324f, 1.5879091f, 5.0371776f, 9.989916f),
            _mm_setr_ps(2.3513222f, 2.6875122f, 1.3439574f, 0.19741297f),
            _mm_setr_ps(2.2625334f, 2.0226216f, 4.21636f, 7.427453f),
            _mm_setr_ps(2.4613087f, 2.767006f, 6.197409f, 4.9654136f),
            _mm_setr_ps(2.5514884f, 1.1284562f, 6.1803327f, 5.667452f),
            _mm_setr_ps(2.4790344f, 1.3389385f, 2.1556911f, 2.8019624f),
            _mm_setr_ps(2.3671112f, 2.7620203f, -0.3341647f, -1.5528622f),
        };

        for (int i = 0; i < 16; i++)
        {
            __m128 value = nextFloat4(&rng, _mm_setr_ps(2, 1, -1, -2), _mm_setr_ps(2.71828183f, 3.14159265f, 6.283185307f, 10.0f));
            uint8_t values[4 * 4];
            ((int32_t *)values)[0] = _mm_extract_ps(value, 0);
            ((int32_t *)values)[1] = _mm_extract_ps(value, 1);
            ((int32_t *)values)[2] = _mm_extract_ps(value, 2);
            ((int32_t *)values)[3] = _mm_extract_ps(value, 3);

            printf("       %.7g %.7g %.7g %.7g\n", ((float *)values)[0], ((float *)values)[1], ((float *)values)[2], ((float *)values)[3]);

            ASSERT(ARE_EQUAL_PS(answers[i], value));
        }

        PRINT_STATE("nflt :");
    }

    {
        printf("nbyte: ");
        initState(&rng, 0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu, 0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u);

        const uint8_t answers[32] = {
            0x22, 0xf2, 0x8e, 0x1c, 0xaf, 0x13, 0x8a, 0x3c, 0xcb, 0xf4, 0x13, 0x42, 0x08, 0x35, 0x30, 0x34,
            0x59, 0xea, 0xf3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
        uint8_t values[32] = {0};
        nextBytes(&rng, values, 19);

        for (int i = 0; i < 32; i++)
        {
            printf("%02x ", values[i]);
            ASSERT(answers[i] == values[i]);
        }

        puts("");
        PRINT_STATE("nbyte:");
    }

    puts("\nAll tests were passed.");

    init(&rng);
    PRINT_STATE("csprn:");

    return 0;
}
