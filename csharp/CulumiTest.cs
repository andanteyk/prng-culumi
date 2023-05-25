using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace RngLab.Rng.Generators;

public sealed partial class Culumi
{
    public static class Test
    {
        /// <summary>
        /// Runs all tests that based on reproducibility.
        /// </summary>
        public static void RunReproduction()
        {
            ReproductionCtor();
            ReproductionCtor2();
            ReproductionNext();
            ReproductionJump();
            ReproductionSplit();
            ReproductionPrev();
            ReproductionGetState();
            ReproductionNextULongMax();
            ReproductionNextULongMinMax();
            ReproductionNextLongMax();
            ReproductionNextLongMinMax();
            ReproductionNextULong2Max();
            ReproductionNextULong2MinMax();
            ReproductionNextULong2Max1();
            ReproductionNextULong2MinMax1();
            ReproductionNextLong2Max();
            ReproductionNextLong2MinMax();
            ReproductionNextLong2Max1();
            ReproductionNextLong2MinMax1();
            ReproductionNextUIntMax();
            ReproductionNextUIntMinMax();
            ReproductionNextUInt4Max();
            ReproductionNextUInt4MinMax();
            ReproductionNextUInt4Max1();
            ReproductionNextUInt4MinMax1();
            ReproductionNextIntMax();
            ReproductionNextIntMinMax();
            ReproductionNextInt4Max();
            ReproductionNextInt4MinMax();
            ReproductionNextInt4Max1();
            ReproductionNextInt4MinMax1();
            ReproductionNextDouble2();
            ReproductionNextSignedDouble2();
            ReproductionNextDouble2Max();
            ReproductionNextDouble2MinMax();
            ReproductionNextDouble2MinMax1();
            ReproductionNextDouble();
            ReproductionNextSignedDouble();
            ReproductionNextDoubleMax();
            ReproductionNextDoubleMinMax();
            ReproductionNextFloat4();
            ReproductionNextSignedFloat4();
            ReproductionNextFloat4Max();
            ReproductionNextFloat4MinMax();
            ReproductionNextFloat4MinMax1();
            ReproductionNextFloat();
            ReproductionNextSignedFloat();
            ReproductionNextFloatMax();
            ReproductionNextFloatMinMax();
            ReproductionNextBool();
            ReproductionNextBoolInt();
            ReproductionNextBool2();
            ReproductionNextBool2Double1();
            ReproductionNextBool2Long();
            ReproductionNextBool2Long1();
            ReproductionNextBool4();
            ReproductionNextBool4Float1();
            ReproductionNextBool4Int();
            ReproductionNextBool4Int1();
            ReproductionNextBigIntegerMax();
            ReproductionNextBigIntegerMinMax();
            ReproductionIsProbablePrime();
            ReproductionNextProbablePrime();
            ReproductionNextEnum();
            ReproductionNextGuid();
            ReproductionNextUlid();
            ReproductionNextUlidTimestamp();
            ReproductionNextGaussian();
            ReproductionNextExponential();
            ReproductionNextBytes();
            ReproductionNextBytesInt();
            ReproductionFill();
            ReproductionShuffle();
            ReproductionShuffleIterator();
            ReproductionGetItems();
            ReproductionGetItemsSpanInt();
            ReproductionGetItemsArrayInt();
            ReproductionGetItemsIterator();
            ReproductionWeightedChoice();
            ReproductionWeightedChoiceSelector();
            ReproductionWeightedChoiceInt();
            ReproductionWeightedChoiceIntSelector();
            ReproductionWeightedChoiceArray();
            ReproductionWeightedChoiceArraySelector();
            ReproductionWeightedChoiceIterator();
            ReproductionWeightedChoiceIteratorSelector();
            ReproductionNextInsideCircle();
            ReproductionNextOnCircle();
            ReproductionNextInsideSphere();
            ReproductionNextOnSphere();
            ReproductionNextQuaternion();
            ReproductionShared();
        }

        /// <summary>
        /// Runs all tests that based on statistics or implementation details.
        /// It may take some time.
        /// </summary>
        public static void RunCharacteristic()
        {
            CharacteristicCtor();
            CharacteristicCtor2();
            CharacteristicNext();
            CharacteristicJump();
            CharacteristicSplit();
            CharacteristicPrev();
            CharacteristicGetState();
            CharacteristicNextULongMax();
            CharacteristicNextULongMinMax();
            CharacteristicNextLongMax();
            CharacteristicNextLongMinMax();
            CharacteristicNextULong2Max();
            CharacteristicNextULong2MinMax();
            CharacteristicNextULong2Max1();
            CharacteristicNextULong2MinMax1();
            CharacteristicNextLong2Max();
            CharacteristicNextLong2MinMax();
            CharacteristicNextLong2Max1();
            CharacteristicNextLong2MinMax1();
            CharacteristicNextUIntMax();
            CharacteristicNextUIntMinMax();
            CharacteristicNextUInt4Max();
            CharacteristicNextUInt4MinMax();
            CharacteristicNextUInt4Max1();
            CharacteristicNextUInt4MinMax1();
            CharacteristicNextIntMax();
            CharacteristicNextIntMinMax();
            CharacteristicNextInt4Max();
            CharacteristicNextInt4MinMax();
            CharacteristicNextInt4Max1();
            CharacteristicNextInt4MinMax1();
            CharacteristicNextDouble2();
            CharacteristicNextSignedDouble2();
            CharacteristicNextDouble2Max();
            CharacteristicNextDouble2MinMax();
            CharacteristicNextDouble2MinMax1();
            CharacteristicNextDouble();
            CharacteristicNextSignedDouble();
            CharacteristicNextDoubleMax();
            CharacteristicNextDoubleMinMax();
            CharacteristicNextFloat4();
            CharacteristicNextSignedFloat4();
            CharacteristicNextFloat4Max();
            CharacteristicNextFloat4MinMax();
            CharacteristicNextFloat4MinMax1();
            CharacteristicNextFloat();
            CharacteristicNextSignedFloat();
            CharacteristicNextFloatMax();
            CharacteristicNextFloatMinMax();
            CharacteristicNextBool();
            CharacteristicNextBoolInt();
            CharacteristicNextBool2();
            CharacteristicNextBool2Double1();
            CharacteristicNextBool2Long();
            CharacteristicNextBool2Long1();
            CharacteristicNextBool4();
            CharacteristicNextBool4Float1();
            CharacteristicNextBool4Int();
            CharacteristicNextBool4Int1();
            CharacteristicNextBigIntegerMax();
            CharacteristicNextBigIntegerMinMax();
            CharacteristicIsProbablePrime();
            CharacteristicNextProbablePrime();
            CharacteristicNextEnum();
            CharacteristicNextGuid();
            CharacteristicNextUlid();
            CharacteristicNextUlidTimestamp();
            CharacteristicNextGaussian();
            CharacteristicNextExponential();
            CharacteristicNextBytes();
            CharacteristicNextBytesInt();
            CharacteristicFill();
            CharacteristicShuffle();
            CharacteristicShuffleIterator();
            CharacteristicGetItems();
            CharacteristicGetItemsSpanInt();
            CharacteristicGetItemsArrayInt();
            CharacteristicGetItemsIterator();
            CharacteristicWeightedChoice();
            CharacteristicWeightedChoiceSelector();
            CharacteristicWeightedChoiceInt();
            CharacteristicWeightedChoiceIntSelector();
            CharacteristicWeightedChoiceArray();
            CharacteristicWeightedChoiceArraySelector();
            CharacteristicWeightedChoiceArrayInt();
            CharacteristicWeightedChoiceArrayIntSelector();
            CharacteristicWeightedChoiceIterator();
            CharacteristicWeightedChoiceIteratorSelector();
            CharacteristicNextInsideCircle();
            CharacteristicNextOnCircle();
            CharacteristicNextInsideSphere();
            CharacteristicNextOnSphere();
            CharacteristicNextQuaternion();
            CharacteristicShared();
        }

        public static void ReproductionCtor()
        {
            // cannot reproduce ctor   
        }

        public static void CharacteristicCtor()
        {
            var rng1 = new Culumi();
            var rng2 = new Culumi();

            Assert(!(rng1._v0 == Vector128<ulong>.Zero && rng1._v1 == Vector128<ulong>.Zero));
            Assert(!(rng1._v0 == rng2._v0 && rng1._v1 == rng2._v1));
        }

        public static void ReproductionCtor2()
        {
            var rng = CreateStandardSeed();

            Assert((
                Vector128.Create(0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu),
                Vector128.Create(0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u))
                == rng.GetState());

            AssertThrows<ArgumentException>(() => new Culumi(Vector128<ulong>.Zero, Vector128<ulong>.Zero));

            var shouldNotThrow = new Culumi(Vector128<ulong>.Zero, Vector128.Create(0ul, 1));
        }

        public static void CharacteristicCtor2()
        {
            // n/a
        }

        public static void ReproductionNext()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<ulong>[] {
                Vector128.Create(0x3c8a13af1c8ef222u, 0x343035084213f4cbu),
                Vector128.Create(0xd26f9b55e3f3ea59u, 0x4f8166992c65e22au),
                Vector128.Create(0x4cf8d9aefc4cd0beu, 0x39c10e0031960c8bu),
                Vector128.Create(0x158f8ea2f71ebcc4u, 0x78ff49f82cd1f205u),
                Vector128.Create(0xa05818df15148123u, 0x6147390b9b63e49bu),
                Vector128.Create(0xac616aa06ccb5cdcu, 0x0a0b3eb16b978068u),
                Vector128.Create(0x1fb12a559476f296u, 0xc12be6e152a3f1ebu),
                Vector128.Create(0x7cdd34995203a81au, 0x3b8af000008cb180u),
                Vector128.Create(0x6eb426a9606a863du, 0xdc387d569d41248bu),
                Vector128.Create(0x4646ea9ca904f2a8u, 0xffc8edb9d4341955u),
                Vector128.Create(0xc9b8714c7d36a18cu, 0x2ee0ce1152638a8bu),
                Vector128.Create(0x7a3dc7f65d918c6eu, 0xc91e7709b75a2b8cu),
                Vector128.Create(0xd339134ca469ccacu, 0x949872c2fcfc29a8u),
                Vector128.Create(0x0f5af5b7c48dcac9u, 0xa3928345fc6281d0u),
                Vector128.Create(0x288409d4aabb18f9u, 0x66711e336eebbbbcu),
                Vector128.Create(0xd2a0815182d73662u, 0x0989f87d17675c50u),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.Next()).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual((
                Vector128.Create(0x8270ef68535c9084u, 0x003580c13a853756u),
                Vector128.Create(0xe4060d8f930a7b5du, 0x666a332949e0c67eu)),
                rng.GetState());

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<ulong>.Zero, rng.Next());

            rng = CreateOneSeed();
            AssertAreEqual(Vector128.Create(1ul, 1ul), rng.Next());
            AssertAreEqual(Vector128.Create(1ul, 1ul), rng.Next());

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128<ulong>.AllBitsSet, rng.Next());

            rng = CreateNextSeed(Vector128.Create(0x0123456789abcdef, 0xfedcba9876543210));
            AssertAreEqual(Vector128.Create(0x0123456789abcdef, 0xfedcba9876543210), rng.Next());
        }

        public static void CharacteristicNext()
        {
            var rng = CreateStandardSeed();

            var andVector = Vector128<ulong>.AllBitsSet;
            var orVector = Vector128<ulong>.Zero;

            for (int i = 0; i < 64; i++)
            {
                var next = rng.Next();

                andVector &= next;
                orVector |= next;
            }

            AssertAreEqual(Vector128<ulong>.Zero, andVector);
            AssertAreEqual(Vector128<ulong>.AllBitsSet, orVector);
        }

        public static void ReproductionJump()
        {
            var rng = CreateStandardSeed();
            rng.Jump64();

            AssertAreEqual((
                Vector128.Create(0xb4ae34f360f7dd61u, 0x1b6b93a51e284710u),
                Vector128.Create(0x0c5bab58050abf53u, 0x92ed4e9a6e4db9fbu)),
                rng.GetState());

            rng = CreateStandardSeed();
            rng.Jump128();

            AssertAreEqual((
                Vector128.Create(0x12b5566ceafdd0c9u, 0xfb978ee2b429ee53u),
                Vector128.Create(0x902b9e77ba34c2a0u, 0xdd3a2fbb67b23028u)),
                rng.GetState());

            rng = CreateStandardSeed();
            rng.Jump192();

            AssertAreEqual((
                Vector128.Create(0xd6ff1d41eb6dd5afu, 0x67928f8822d06129u),
                Vector128.Create(0x78a5a83d15b6a940u, 0x68d2523f25f972efu)),
                rng.GetState());
        }

        public static void CharacteristicJump()
        {
            // jumps 2^[i]
            var jumpPolynomials = new ulong[256][] {
                new ulong[] { 0x0000000000000002, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000 },
                new ulong[] { 0x0000000000000004, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000 },
                new ulong[] { 0x0000000000000010, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000 },
                new ulong[] { 0x0000000000000100, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000 },
                new ulong[] { 0x0000000000010000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000 },
                new ulong[] { 0x0000000100000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000 },
                new ulong[] { 0x0000000000000000, 0x0000000000000001, 0x0000000000000000, 0x0000000000000000 },
                new ulong[] { 0x0000000000000000, 0x0000000000000000, 0x0000000000000001, 0x0000000000000000 },
                new ulong[] { 0xAE7E866FC76E3FFD, 0xE3AC7FE2A4CF9BAF, 0x0000000000000001, 0x0000000000000000 },
                new ulong[] { 0xFE6B923BC23B6AAC, 0xA7F86AB6E4DB8FFA, 0x4410505541454454, 0x5405445015555404 },
                new ulong[] { 0x7E9EAA02DF1D3E5A, 0xDC3BA6D5DA07915F, 0xF83ED0B1DB3BA6E8, 0x7DEEB9A537B978C3 },
                new ulong[] { 0x3CE09749A1F38153, 0x4F7E65DD3A2F69C4, 0x8F75474A279FB95A, 0x5D5E8FFA3CC832B5 },
                new ulong[] { 0xBA42421550E53BD4, 0x5992C3A947D97288, 0x01D205C9C12947D9, 0x0BD2AF2BF0DA742B },
                new ulong[] { 0x32336F5F861C40F4, 0x400978898620140F, 0x8AC22E5934025B31, 0x9429A8BBFE2B2FF2 },
                new ulong[] { 0xF8C1AAD861C4B53E, 0x70A8B0DDEC427AE2, 0xD3A1873A37628CB0, 0xE0FA3E03D5F4E6D5 },
                new ulong[] { 0xD6433449E2C1CE67, 0x8AEAF213F96367C7, 0x49E215F4AD940C97, 0x94BECCF652075B08 },
                new ulong[] { 0x6C3DDBA8608D2AF9, 0xB1F763EF174C04BD, 0xE938E2C9DD207CA0, 0xD965C0475D9A8948 },
                new ulong[] { 0xF133EBDDE7BC2AFE, 0xE6E7760BFFACEDCC, 0xB74DD758A0EB364D, 0x2337D8EC83B92378 },
                new ulong[] { 0xA375158544AADC98, 0x4C373735F99BE311, 0x7078FC5C120476EC, 0xF63725B4D9E4B2C8 },
                new ulong[] { 0x1E2C9C0F881A76C9, 0x0D7D00DB2C1D2405, 0x76680CEDA97CE5FB, 0x9C22522B8F09F606 },
                new ulong[] { 0x08E14079112166B8, 0x48B4C73F81348178, 0xDC9F037C49E854FC, 0x2AC7DCF4499552A8 },
                new ulong[] { 0xDC3B46364DD221CB, 0x06113B25238B12BB, 0x45595A16A6BA0BBC, 0xCAE7E4167B1D2047 },
                new ulong[] { 0x2CB7DAC74C4A9834, 0xDC23763A26AB2F9D, 0xD227EB1085E89042, 0xB97013CCE093C24C },
                new ulong[] { 0xAB3CB7E4DCFDCF6E, 0x499C1E93085467ED, 0xEFD60630EC620E2A, 0xC186C327736E076E },
                new ulong[] { 0xDA769F499A424402, 0x272318860B804C76, 0xE4ED7540BF5727E5, 0x805165934F68B2C0 },
                new ulong[] { 0x9CCFBC2FDCE812C3, 0x622EE65A5E2EFCCA, 0x753FC91120121E2C, 0x1DC7A5FFE23FBD65 },
                new ulong[] { 0xDF39577C92F41284, 0xF1F0B422C7C4B3C9, 0x40D27C00593E4D11, 0x6C909576E2853A12 },
                new ulong[] { 0x78A9BDFDE55867C6, 0x374DE72A98EE7593, 0xD3359886B1271F20, 0x502793BA351C0EB3 },
                new ulong[] { 0x36AD527FF107850C, 0xB13D6921CE3B5F0F, 0xBD7AF63AD29C5BEF, 0x5BC6967C97BE09B6 },
                new ulong[] { 0x4AABAB84AB20A009, 0x0B2BA4385713B22A, 0x25ACB0D595B614D5, 0x0EF84FA13C3CA4A5 },
                new ulong[] { 0x402C9F29BF8857C8, 0x16999A3CEC2CDBA0, 0x2121E6B10AD58460, 0x96E7F6BAE19E92E1 },
                new ulong[] { 0x61141BDC06B23B7C, 0xFA3645E42D7E8370, 0xC295E83A67E5B98B, 0xF92C96C51552BB8B },
                new ulong[] { 0xB3E3E2D845EDCF6E, 0x6047B599689973BC, 0xDAC892493E80CBCF, 0xD14232DDE5029E51 },
                new ulong[] { 0x2B299472707E876D, 0x4871DD23483CA77E, 0xD18D7D83D7D321FB, 0x91D33D2DE58BC659 },
                new ulong[] { 0x1FEBEC6B656DED48, 0x94F3CCA7E82B09C9, 0xCDDCF14A36CB7AE1, 0x7320EE8A90BD4DF3 },
                new ulong[] { 0x4A730A0341641CE1, 0xC3746A6335FD3DF6, 0x935675374BDFD2D1, 0x359BD8AC2D9DEE8B },
                new ulong[] { 0xF72C12311D0DD0EC, 0x4570C6143509BCEC, 0xA17CDBDA64F0154C, 0x3147D235AEAFC789 },
                new ulong[] { 0x207B69ACE2D9D99E, 0x8C3EB78D9F9D5453, 0xD2B488AAAA6F3BC7, 0xB40FF0BA216EBC39 },
                new ulong[] { 0xDBC773E4DFEF5B5C, 0xB4E93293E2EAD310, 0x63964E37EFF489BD, 0xB560F1A8EBDF5A81 },
                new ulong[] { 0x9FFE3BA3183B6324, 0x96C9EC3AD5D9B611, 0x9B0EE3657AB06910, 0x5A259EEA1B207E2F },
                new ulong[] { 0xEE5305B3351E5F39, 0x5F6A5704C46A646D, 0xAF1176678965B75B, 0xDE83956F3BEF8E48 },
                new ulong[] { 0x6488D30A8523310F, 0x567503F272F0D55E, 0x2613FBCB4346E198, 0xF1976DC2B1FEE880 },
                new ulong[] { 0x241BC565A2A22174, 0xCCBD5133D15FB2A5, 0xF11092F711973BF1, 0xFCDEACA11968B8A1 },
                new ulong[] { 0x8B94EE13CC82FFA0, 0x43AA9C672D92019F, 0xD4C75138E3DA5549, 0x467A2F3E82DBE489 },
                new ulong[] { 0x53F3BBA36EAD69E8, 0xB8F6DEC10C4ED9E7, 0xAD22E3883BB85E1D, 0x68C8E0496ABF23C9 },
                new ulong[] { 0x4A4EE49A87D450BC, 0xD24C633C724893A1, 0x4F6645091F0FA22F, 0x6974F3AF92B458D9 },
                new ulong[] { 0xD25F4F8F0AD33BFD, 0x767B324BC73ED357, 0x86CD5F92B02AF3E6, 0xB370D888901D565D },
                new ulong[] { 0x8780573D048A4856, 0xCCBAC63E3FB0B149, 0xA2E52670A01C05C2, 0x87B704908168301C },
                new ulong[] { 0x5DEB533D54FEC815, 0x6B10100FF57646F9, 0xA1DFEEF74FB34CAB, 0xD8826B526E86DD6F },
                new ulong[] { 0x6F5B19619FDE5796, 0x1F209977191B4ADE, 0x6535BA6F62D1E093, 0xD295D1238DA33FE0 },
                new ulong[] { 0x4893339C99D54241, 0x667DA2CED2E0519E, 0xEEC052196FEB53E5, 0xDBBF5684FC47EC6E },
                new ulong[] { 0x3DAEEAFD6B90A5D0, 0x25E333DB9F9A5431, 0x0F2E7CAC75EFDBC9, 0x17E8EEF14F04A2F0 },
                new ulong[] { 0x10D880DCF41A1E75, 0xE912575E1B866FC4, 0xF851968895FB1B6E, 0xF631D2BA2BBDFD82 },
                new ulong[] { 0x5676B5E7DF74EE45, 0x7EF38F64329C2BBA, 0xECAC3EE883483E29, 0xEF3D9AF74C7E657B },
                new ulong[] { 0x67CD6420DC1C1E63, 0x07E434C62898FAF5, 0x2525ABC9C3FE2BE2, 0x3ACCA7DE49DB932F },
                new ulong[] { 0x1B8AEFF86EE2A460, 0xF78D69993FF32EEF, 0x3221C48812E5ACAA, 0x1291DC14B8F10EBA },
                new ulong[] { 0xB02ADF604FA28024, 0xCA8CF5AD7A6E333B, 0x140CBCAC514A908E, 0xB5AAFA1C93FC78FF },
                new ulong[] { 0x4D4C0CE4C3F9ACDB, 0x02E7E8495A725AFF, 0xB68CCBFFBA0C84EF, 0xC24E46FBEBE6CCCC },
                new ulong[] { 0x92E56C433DBD738C, 0x7BF2941C0130E9D3, 0x7AD1946010EB2BD4, 0x079301B769D39224 },
                new ulong[] { 0x3196141333D1439C, 0x1B713DD7D3EC9CE4, 0x2BEDAB02CDEADF04, 0xB6B05928463E9938 },
                new ulong[] { 0x3BD97D51DAD8B172, 0xBC4777D6E73309E2, 0xD746EF26E2E16884, 0xA51809077972D111 },
                new ulong[] { 0xFEB1F0B1DBAA0D6F, 0xC5607F69BB3314EE, 0x7C108748494839F3, 0x0F7FAA669EBFDC85 },
                new ulong[] { 0x5DC579E1F9089C3B, 0x7B855D74415FE2D0, 0xDB231394515EAAE9, 0xCF3859DE85A33481 },
                new ulong[] { 0xD554E5F6C3AA9503, 0xF4618D98BEB1E57D, 0x998FC93FA749970A, 0x58CA33A93BCD1C04 },
                new ulong[] { 0x5601375EC36230E1, 0x79CF0DE79B070769, 0x51407AE5A16EA33B, 0x708C91D747D77FE3 },
                new ulong[] { 0x51044A42BC89459E, 0x8E3903B13FEB1A13, 0x13718FD907BD7372, 0xE1900A798A59ADD2 },
                new ulong[] { 0x19C0FF37DA8F5CA4, 0x1228A833ED0AD2D8, 0x9B84B38DEC1CE101, 0x0E3FB9A956399253 },
                new ulong[] { 0x89DBFA07EB3B6728, 0x16927C5703085835, 0x59C636A0ADD8A900, 0x25BBD810B84DF45E },
                new ulong[] { 0xF1387E3CED676852, 0x63AFA1805B6E1BCA, 0xDF6D80E1602DC787, 0xE225CD946CF81DD9 },
                new ulong[] { 0x6D1F905C78F03907, 0xC9FEA8D0F8FA6FC7, 0x6E8F3AD276EAE79C, 0xC5FD698D7DE2EE74 },
                new ulong[] { 0x1B2EB424C721B7E0, 0x30E2DA1BAC9E238E, 0xEF4EB99C0D0BF3B0, 0x2364307825E0E221 },
                new ulong[] { 0xFC97738033848E55, 0xB757309D3A0C2F30, 0x71C272691AF304CF, 0x41E57F14786E6AEB },
                new ulong[] { 0x3F33664E64DCCE2F, 0x91EA6F38A86CD28E, 0x488F1C1937774516, 0x767B7A4FA79D17D5 },
                new ulong[] { 0x04FFEC354D0242FA, 0x864676271919B630, 0x01DB3CF9BE166781, 0x5385BF615A91E656 },
                new ulong[] { 0x9DED0075D9791C90, 0x8770ACEBEEACB441, 0xDC4B60BEBAE844E7, 0xF2BBA7C969DA618A },
                new ulong[] { 0xDBB615C6C2663327, 0xE0B8E1365CAB6282, 0xE278D11809811F51, 0x733892C7472FC50C },
                new ulong[] { 0x3216E88C4D2A812B, 0xC05F4ED6BFB556B9, 0x1758040C538FE804, 0x1E8FF398A819A234 },
                new ulong[] { 0x0DFCBCB699D2552D, 0xB8FADE2302384B0D, 0xD93EB4076B167297, 0xB5C2974D339F3E30 },
                new ulong[] { 0x7ACFFCB86C86BA4C, 0xF916B2C151E4FAC0, 0x08E83647AC11E6A9, 0xDBC8990AB542A4AE },
                new ulong[] { 0xAC10FE645C6E4E9E, 0x58D01EF004178AD0, 0xAFB34FED22F61E70, 0xC9DF685D8CC2AF4B },
                new ulong[] { 0xAC6430A3EA4C090C, 0x26AD4D0F5370B698, 0x0D0A77B928C20A0F, 0x41738D2118E82EAA },
                new ulong[] { 0x9095BC699B0AFBAE, 0x69C7987F6C5E41C2, 0x343401AAF2E01B53, 0x2157F11D6550D67D },
                new ulong[] { 0x5D29F47B203FD524, 0xB00A19990FC5E1E9, 0x184E7005690DDBE5, 0xD59965558BCA10C4 },
                new ulong[] { 0xB527FE57B3876924, 0x33C9E3FAA97C125F, 0x92BF56B34F2B25EB, 0x6391F27561F2DC71 },
                new ulong[] { 0x436DE2F62A12A0A7, 0xBE7CB90F47F01B44, 0x6938DF0E3A92C090, 0xB3FFABA701FD7235 },
                new ulong[] { 0x3306F1843D72D4DF, 0x2BEB966E47FFD0B8, 0x0B2716E39266F3C5, 0x7F23A2ACA2C7135D },
                new ulong[] { 0xDF1F9FFF38F04209, 0xDBB6192FC8BF2A38, 0xEE762793FA7EAE65, 0x7EC189F20260459E },
                new ulong[] { 0x8385DFCBBA6F0EB1, 0xA9B7787D990796D2, 0xBEC062ECDC7F822E, 0xDAADD817FF8CF7B3 },
                new ulong[] { 0xC6100642E586FDBC, 0xA9F3841A1A7F3FDD, 0x26D2BE39E0F194BC, 0xFBD07743F4B4C03A },
                new ulong[] { 0x598B23E84461A783, 0xF1B27834CD325ADE, 0x8748A5242F562350, 0x93F5EBF8EDDB37C4 },
                new ulong[] { 0x3AB6A86A30A0E9E6, 0xF194A61EC63E5F20, 0xCA7A50A285A81179, 0xAEA3FF2F94315F38 },
                new ulong[] { 0x857DC233AF37D2AC, 0x888508BBAAE3A12F, 0x0039C07E4371DDC0, 0xFC1AD97F169609A9 },
                new ulong[] { 0x9F4E662B5AC25CA9, 0xBD04B6BBB380EADB, 0x6AA1C05D5AE9E651, 0xE5BF3D3862FC9242 },
                new ulong[] { 0xDBDA3F9DF6CD8CAC, 0x6D80CBA5720C9043, 0x661AAB59BA207639, 0x65B8D2881851DE9B },
                new ulong[] { 0xBDFF2FB9C1A77FA8, 0x4B9C5F0E5EAD8BA6, 0x60A9EA84C64AEC79, 0x4576642FA0937959 },
                new ulong[] { 0x2380138903EDD329, 0x701D7DF8FF8CA9EB, 0x4F944D7F208F6DCC, 0x0F0A0A6D9A3839B7 },
                new ulong[] { 0x1A53EA9A2CF82E35, 0x76CEC32A57E799E2, 0xB4A0E7A38A681DEC, 0xF21F62934074CFD6 },
                new ulong[] { 0xED4172913E2D21B5, 0xC119BCC560062914, 0xC01378BC1D521BE2, 0xB93AA7CAD8DFE146 },
                new ulong[] { 0x41F80F76DC707882, 0xC95746F4F74E340B, 0xE18F4690F0957DEA, 0xEE18C991D9A358D7 },
                new ulong[] { 0xAF7BAA0B924ADF26, 0x0B90DB82607726D2, 0xF8C47E3B4C8599DE, 0x09CB3585CB9400F4 },
                new ulong[] { 0x9A451DBA97102433, 0x5E4592A3BF49995E, 0xD4258F50D360ED10, 0x803FEF2F887D9812 },
                new ulong[] { 0x22168535ED47E402, 0x7A38ADD0135A9C47, 0x82B1C74858AEAB36, 0xC5A1A3BA93EDC816 },
                new ulong[] { 0x9CBA2688B6D6D4F0, 0xA46B9C16D33EFE03, 0x8CF1D8463691C49F, 0x77E70CEFB5204D3A },
                new ulong[] { 0x70F5FDC623574408, 0x178D7C8E0E6564C5, 0x592C2155AC2632E8, 0x5C4F460103EAC42F },
                new ulong[] { 0x9A2CD59E4473FE21, 0x0416265FB8DD9747, 0x65F155DC752E092D, 0x9AE65D19F93800F0 },
                new ulong[] { 0xDAE1E60D945CE232, 0x2952C73D93DFDC9F, 0x463D12EA88B5849C, 0x9F9A92600D7EFA3B },
                new ulong[] { 0xB348FDD4C583417C, 0xAF37E4E8B1DDB7DA, 0x902C2BB0296F712D, 0xD8F87F83EAFD1759 },
                new ulong[] { 0x5B309BB3F77FA78E, 0x247F332167B37305, 0x327AD41C23F277C1, 0x97481ABE1B43736D },
                new ulong[] { 0xDECE9E1666CA36F9, 0x579E308ADE71BCD2, 0xFFA1546446B6F6F6, 0x1CD0C6E3D9BB2680 },
                new ulong[] { 0x43EC9053832EB7DF, 0xC18B3DB799F19580, 0x6C1C157ABC002CE2, 0x10ACAF50BB547387 },
                new ulong[] { 0x02F34D4B37809594, 0x353A9F414B4A75A9, 0x6C63551271297EB0, 0x818B7E8AF4257FB3 },
                new ulong[] { 0xB7DF8D8FD7CFAD0E, 0x34CEC986D2533D46, 0x85C1EA4890A3E231, 0x307ED6F2EF35E082 },
                new ulong[] { 0x54F01FC7EC8E61D8, 0xC1BEFF8BA8ED5868, 0x0726099F18D0A16B, 0x161DE1662B59663D },
                new ulong[] { 0x1B942A5E45DF7567, 0x73B0123D1CCB7119, 0x5779C38DD10E527F, 0x5F36B2A9B27D019C },
                new ulong[] { 0x29D0BCC639E299FF, 0x2A2673F5AAAF33A8, 0x448B94699767F311, 0x46613BE5D551D931 },
                new ulong[] { 0x4AFCCFA7F5D6745E, 0xF38479F58AA84541, 0xB3385ED4D4A12F68, 0x7822ABD7176B2F71 },
                new ulong[] { 0x170EDF2AAF21DDE1, 0x8E7E092076399305, 0x037A97980402E9C3, 0x224F50C832D5D5F2 },
                new ulong[] { 0xF1FB42E9CE41BD8E, 0x64D3FA2CE5E0847E, 0x93E63CE0DF927290, 0x732AB5BCDDA6C928 },
                new ulong[] { 0x740CAA22757F73DC, 0x75DA27E40552A80D, 0x556F65953CC9FA4D, 0xAABEF1BF192CD6C3 },
                new ulong[] { 0x96F6E0F87695E1CF, 0x71C9471C92BB14B5, 0xD9B96BECBEE94794, 0x5DA39A86F0490B59 },
                new ulong[] { 0x8F1729540014A02D, 0xD09164BE8DEE0F47, 0x9469E779C3F622DD, 0x0F70973357AC72B6 },
                new ulong[] { 0xB7CF1C17F5A403A3, 0x23C6226584BB4C6D, 0xBE768506D4B2C388, 0x9F18674C2016C4BD },
                new ulong[] { 0x11D0D4D37C3EB722, 0x85E6FE7C28E2663D, 0xD1185DB19A13CF10, 0x2F0E9B3FC23A98E2 },
                new ulong[] { 0xD98D81BFD01E6EE3, 0xBBED59012A971345, 0x84CA846A0C301173, 0xAD89B7549AD83719 },
                new ulong[] { 0x6E8C202684692382, 0x053FC812B45A42C9, 0xECC3D3FDC00B8CD4, 0x4C07D003B939E41E },
                new ulong[] { 0x2EE039F7C0B3FA8E, 0x359B1A5120DD2B60, 0xDA00673B42B8992D, 0x95A36F27D088EC52 },
                new ulong[] { 0xBA823415731BC41D, 0x1AB17F1075599476, 0x2973D773EB0F54B6, 0x0CA100098D8C4408 },
                new ulong[] { 0x3A7EE0AED5F12ADA, 0x7A792F89A1CEDAEA, 0x21195C502418ED8A, 0x407EE3E2B5B19D51 },
                new ulong[] { 0x6C81827A1CBDFCCF, 0x7E438EDA9627E879, 0x15123909CF74EB17, 0xA7C9C89160D05C3E },
                new ulong[] { 0x727D413C2155543A, 0x8BA79A7FE97ABCB9, 0x1BDF12D9719BCB45, 0x40F9AF496690140E },
                new ulong[] { 0x7A53558623D9D3BA, 0x2F06AC40805631D5, 0x5DEE46DE681153E9, 0x2D9693B692AE6106 },
                new ulong[] { 0x6743D365ABD6BF2D, 0xC95E9C3416D1AE3A, 0x54FB5BB2F0108862, 0x50B6DA2ECAE22EF3 },
                new ulong[] { 0x7A13EEB8E75DEB66, 0x29C522B3692A3664, 0x80B2626C3E0F10DF, 0x6255E1F1D295AECF },
                new ulong[] { 0xDB4BA466224B7989, 0x2C9B6C4DF70731BF, 0x8EC3000A86D94C10, 0x73814B589E7691EB },
                new ulong[] { 0x29AD28E06A31F02D, 0x118761A011FA2FA5, 0xDA3F12A867C7FF37, 0xBA8C3A90AE811814 },
                new ulong[] { 0x9D1B2221EF5A9E4F, 0x086E6B524DA7D6C9, 0xBDBEA6EA1E1023A8, 0x75AFD6025C8F2A1E },
                new ulong[] { 0x874E6C2E55BED7BA, 0x08D75F9F030D9B88, 0xE22FE91C3541F81B, 0x464E7A84163CDA8A },
                new ulong[] { 0x7A7EEB429AB5CB11, 0x1F9C55AC2802A0BF, 0xF3B1230FCDCA4ACE, 0xF67815693B30237B },
                new ulong[] { 0xC76B2895522E838F, 0x4F77824821D198F7, 0xA7BBAF99D347BC84, 0x56450A97F535000C },
                new ulong[] { 0xDBEA7BD2CBCE99C1, 0x01AF64FFC46AACC8, 0x26B72548A6BEE6CF, 0x4064C53B616F8C67 },
                new ulong[] { 0xF315C229F6CABF39, 0xA5D75A5997EC5809, 0xC6EF1D4BFC7562F6, 0xD011B292B440608E },
                new ulong[] { 0xC2438704FB2847AB, 0x47C24CDADEFC569E, 0x61779DC2DB38AEA3, 0xA042F326B8F48DDA },
                new ulong[] { 0xCCFA62AC3919CBD9, 0x6162D7E9E92AE4B7, 0x0A44456D61C5C9BC, 0x87A158E975536C63 },
                new ulong[] { 0x735D9B4284A397A8, 0xAA23B21F5D62273F, 0x36E5CECC57994808, 0x32974559164084BF },
                new ulong[] { 0x8BCB2ABABD5A112C, 0x712826AB35D6DACB, 0xE7071D2AD8FE433B, 0x2D1EB1BD898A0E9B },
                new ulong[] { 0x5FA1814C9EB2CD0C, 0x7F4D0C22C328FB80, 0x9803152A060BC87B, 0x21E8DCD40938E000 },
                new ulong[] { 0xCDF7BFFF9B7F2F99, 0x1AE9184AD2CE5D15, 0x001CD856972B7400, 0x59B696063BE38AA4 },
                new ulong[] { 0xF584D903992066A6, 0x0446C6CA8609CF5A, 0x7C8AE798F522B51A, 0x566371AB43AFA345 },
                new ulong[] { 0xB24686D99F7BB014, 0x64B5991436D33D55, 0xC2035CEDB1F0ABA9, 0xFB7324F9680B9389 },
                new ulong[] { 0x015A2FB0F974D099, 0x8AE0CD216A6C1407, 0xE8C345D2070CFAC3, 0x2D38A68462AA1866 },
                new ulong[] { 0x710B5FBEE557373B, 0xBD0917670790912E, 0xFA066C71076823D0, 0x1F61AFB9280D6DA2 },
                new ulong[] { 0x8AD2423F793D28E9, 0xF482FDF241761C82, 0x7A08FD7A79A5DF86, 0xCA3FF8C05758CA7C },
                new ulong[] { 0x45A7B555AB93EC47, 0x8AE6ED35204CC001, 0xF200C5558FFC5DCC, 0xA30329DD0139C613 },
                new ulong[] { 0xB4836F5217E68EE3, 0x06B73C4478031EBD, 0x0D2226CD7F4787CA, 0x23744F5A65A06767 },
                new ulong[] { 0x6F458919655BD5EE, 0x653BF39E152A43DC, 0x80B5DC391356C2C2, 0x1E2ABF5C5490F9EF },
                new ulong[] { 0x9C95792B6D214B5C, 0x8C568ABDB3438191, 0x1CAC7E143142C0C2, 0xC3CE024EDFABAE04 },
                new ulong[] { 0x19712A7FA2F4D302, 0xE1BD7A9B66B833FF, 0xF0DA6069E3A15D2B, 0x9B0DE109DF6125C4 },
                new ulong[] { 0x9E0AC5B25A8F9F5E, 0xCBFACBF7F99340BA, 0x0A2055EE38AA0389, 0x1DA5ABE285C48AA1 },
                new ulong[] { 0x7E602569FDAB37FB, 0xEDC42BC4D87DA104, 0xCB69D0246EEB0F90, 0x2FEFC7E053992B14 },
                new ulong[] { 0x00B73F28BCDCA281, 0x699BF944A910AE0D, 0x9E73C0CC96F4CD40, 0x9DB847720D9B99C2 },
                new ulong[] { 0x16E14F94E71F552B, 0x4EAC7BFB051E18A3, 0x5AE1968AFF609785, 0x940A27337026FB7C },
                new ulong[] { 0x5E7CC837718D628B, 0x47C1F394DE7099A8, 0xC2437388D6CEC601, 0x9809CC4E4B230C09 },
                new ulong[] { 0x08CFA9213484B8BA, 0x644B4BB5A3EDEA9B, 0xA1A5E6D5E9B0E1E0, 0x46A58775DF1B19AD },
                new ulong[] { 0x75CB66558AECCAF8, 0x0C4907B25C505D9D, 0x534505CA11FF2F2E, 0x2DD3469CDBBDE30F },
                new ulong[] { 0xC14CDC47A54C759F, 0x4E0D9F43769C385A, 0xAE8E026A02141EB6, 0x74D810090985E571 },
                new ulong[] { 0x05A1CF21A4476E26, 0x063A487BCE625F13, 0xE31ABB02B8635FC9, 0x101622D5B908E1C9 },
                new ulong[] { 0x787D2C8B50C82BB1, 0x82493F808F50FA80, 0x77923F6B44F08131, 0x5FA3A052C9C638D0 },
                new ulong[] { 0xE389D0D1FE43DC2B, 0x9ACE44907B62F78F, 0x3458F48A1C97273D, 0x68C5777A4ED54AD6 },
                new ulong[] { 0x4E37E51D0F8689E2, 0x50615C81527A7AAC, 0x4B0D05A7A8E9B56D, 0xDE4980683026A605 },
                new ulong[] { 0x262617AC037EE0E0, 0xC021F2FA5433A63B, 0x9F2736EA3CCBF8E6, 0x54BE7EBBECFD9BD1 },
                new ulong[] { 0xE06EFCC3C4800507, 0xA4CDFFE6C83A0B04, 0x30F76C68B47D0D60, 0x0C4A684D291CD717 },
                new ulong[] { 0xAD7B9EF1B235989A, 0xD8688C38DB98E621, 0x76329B594B38BBED, 0x1BEBAC1241F32101 },
                new ulong[] { 0xB12776200BABD166, 0x2AF07F7A937D415A, 0x4FED8D99562157AC, 0x8593D81F5BED854A },
                new ulong[] { 0xF8E9A2C9CA331057, 0x57435942326E4F45, 0xB37F8039053EC020, 0x2717AFBC65D97A08 },
                new ulong[] { 0x42AD26204A352A54, 0x6DDF6D6E27B7F916, 0x79877E94D73810C1, 0x9EFEAC6D77F9C063 },
                new ulong[] { 0xA044FC155C6A01FF, 0x728622D0AE2DDC07, 0x1FE5312469602653, 0x2269ED728D860E2B },
                new ulong[] { 0xD93CE2FAABB6266A, 0x9D7A27B1119F419C, 0x1FC196975EC34539, 0x97EF5DF9D5FC8EDD },
                new ulong[] { 0x23AB18CA4F7AE85A, 0x91DE25F39D1B9C2B, 0x4F02E53664568E79, 0x396B883E7A04CB6C },
                new ulong[] { 0xBD38F55B3BA11583, 0xCA1C2DAE200EDEF6, 0x2C183376661AD9C0, 0x99D2493B31040E2F },
                new ulong[] { 0xE9625A85D14E4886, 0x66F0D160BA7DB0EC, 0xB95357B0BF39CE19, 0xBEB7E9ABB61DEDAE },
                new ulong[] { 0xB69A61015BB8E7AD, 0xD526FD8BD2F5DF16, 0x4B8352F65C82A9B2, 0xBF288B312ECE2CF8 },
                new ulong[] { 0x4AF96223BA53315C, 0x0DD6ACC8ACDC7A07, 0xBE5D8EEBA43DC67F, 0xFB09A8EAB51ABE3E },
                new ulong[] { 0x05AD084D26396233, 0x7B6C799C06D9B09E, 0x4983E65B20BB8266, 0x0EF8330E1513D596 },
                new ulong[] { 0x97FF7E37828B0F55, 0x4A59E5CA6480D413, 0x228A7C114AB6D17A, 0x8AA267700D4BBBF9 },
                new ulong[] { 0x2407E86DEF2E1455, 0xA115421F7C93F733, 0x935C78329DFCACB1, 0xBDCDDBD2EF56DA0A },
                new ulong[] { 0xD544D8A315D11955, 0x5DC4F882F71712DC, 0x3DFFEBCE3EDC92C9, 0x159F29BEE6D3BAC2 },
                new ulong[] { 0x72FA42D114DB08A9, 0x80D2E67AF64BFD7A, 0x623CEF762B900D8D, 0xF7562B0ADA99532F },
                new ulong[] { 0x1A08E161F3DC4AC6, 0xD921874FFD9AFBF7, 0x0BD42D4748F0926D, 0x00A2D4CA33763E9A },
                new ulong[] { 0x1EB37C084379F4F7, 0xBC48FC1F92143B8D, 0xE44AC7D389EC21AC, 0x642EF87FC685090F },
                new ulong[] { 0x7F85088EFE7FB0B8, 0x64DD96C1594E94DA, 0x19922226D10DAD06, 0x5232B193A4936340 },
                new ulong[] { 0x13A00F0E3B86A1FE, 0xD7E752FDED41EAA0, 0x335994AB61BD8C3D, 0x61F437CDAABD5319 },
                new ulong[] { 0x7C515293F289B9BF, 0x4A57015AB6B992BD, 0x45A760908206CAED, 0xD67C291340E0200F },
                new ulong[] { 0xE03ABAC0D7F32901, 0x176EBE5A39A97EE5, 0x92B41C08DDEE8EAE, 0x9C1C03167238346D },
                new ulong[] { 0x2B0AD09880CF8752, 0x1CAF2F2D26E726F8, 0x158433E50864CDE2, 0x78BD3F34D4DB9EF4 },
                new ulong[] { 0xF9302CAA30401BE5, 0x24C92DAB8E8489DD, 0xACDB6362FCB2C4A7, 0x489E9C0C1ACD7445 },
                new ulong[] { 0x4A2A374490ED544A, 0xE7044CAC82A402FD, 0x6C60B648CA0AA48D, 0x47B4D63AEC87769A },
                new ulong[] { 0xEC89C63B86681FA3, 0x79E8F1DA9B82ABD7, 0xF1D3E07C3C93664F, 0xD1BF86A93A437B56 },
                new ulong[] { 0x82811FDDCA1D7E8B, 0x64FD7594E06EC8C4, 0xBD0C60920E322D1C, 0x1311464822CFA978 },
                new ulong[] { 0x18BE6CDE6AA6413B, 0x702EFA254BC79134, 0x73EA01C97EF26C42, 0x19394B0152015B62 },
                new ulong[] { 0x7D077BD0537499AE, 0xB8EE5E569F75A685, 0x00ACB85E9C72C3E8, 0x7A45F2F5D85FA9EF },
                new ulong[] { 0xB1D990BC408D8E9B, 0x82A8578C00C344EA, 0x25EF2D705049ECE2, 0xC58B38DE21CD8685 },
                new ulong[] { 0x63248E2FC730E608, 0x51780EF1C773B4FA, 0x7A4220DCB761DCE6, 0xEB3F63134EDB78FF },
                new ulong[] { 0x21F87FA401CFB361, 0x1C54E0194D297DE2, 0x3D7CCC880982781F, 0xD1E7C2471AF2B1A1 },
                new ulong[] { 0xB72CE0F8E7D8D1DB, 0x791B21D13761C038, 0x84908CCBF7D3226C, 0xC44DC16188008C2A },
                new ulong[] { 0xD8C1023DC12B5050, 0x290E6D9C3D0A6176, 0xD685DFA5594D5676, 0x879BA60FA597B5E8 },
                new ulong[] { 0x289117A18977DB5D, 0x9EBDAE194FA4C6A2, 0x7EA9B6BEADE4BB74, 0x135352974A41D8FD },
                new ulong[] { 0xB02B27434FC40CE4, 0xFA9D3F97230F8E41, 0x7ABB51C75760F20A, 0xDB156E2FA460727A },
                new ulong[] { 0xECC9D2042F5385A3, 0x788BF06D0B15722C, 0x718A0298DD84DE2A, 0x99F7A949CC527740 },
                new ulong[] { 0xE66C98FEA89561CF, 0xD05F1DF56DD26BD9, 0xCA600539039B7617, 0xA60F7BCB1F209CEC },
                new ulong[] { 0x4E9A5D579B7192F4, 0x1290D8525B50D40D, 0xFFCE9F99A3E59A48, 0x8DD1EAC88141C069 },
                new ulong[] { 0xC07C491DD6CAA309, 0x366ECC4F9481DBEE, 0x6E150A3D9145BF07, 0xB88EDBF260A6CA25 },
                new ulong[] { 0x77FB1C3C4368BB87, 0x7A9F571AA9969402, 0x4DB1B62EF37C3181, 0x79FD76918D02C306 },
                new ulong[] { 0x61F221A930C9D909, 0xC28EA9983E161DC1, 0x26AA62FFE8018BDD, 0xF4844F707A04CCFE },
                new ulong[] { 0x6C5A01117B2B7BB1, 0xF870DAF7AEA53A2F, 0x53A788EBD481F422, 0x4312BFED662D4BD6 },
                new ulong[] { 0xB19DC08927104E18, 0x854F8627C4A61EE0, 0xBD3F6C2F46ADF0F7, 0xFD7C7DFA04AD2C68 },
                new ulong[] { 0x74AC01F93C07CA2D, 0xC9A34E1E2BF8DFD7, 0x4174EC7D70D6B72E, 0x509C8FB73FAB3122 },
                new ulong[] { 0x781A2E9D10D5D6AA, 0xBC73FB3128DB90E8, 0x744BF27DE742A0EF, 0x3CB2605FE6F16686 },
                new ulong[] { 0x62DE5680009F916A, 0x16DA598E19B86305, 0xAB0711A9460CFEE4, 0x9513318651F06BFB },
                new ulong[] { 0x3FC6EC53EFF8BC67, 0x2F5C02A187CD834E, 0xAB707B57FF0E57B6, 0xF16EF28CF7AAD568 },
                new ulong[] { 0xF31241EFD8FA7161, 0xEA8C6B5B6D758EF5, 0x2D67344E8EC1275D, 0x25ED8D6BAB72D6E4 },
                new ulong[] { 0xE0891468447179E5, 0x71B56CEEDF5D5F77, 0x0161E44A6A215BD9, 0x2F32C99F6C9F672F },
                new ulong[] { 0xE1A122516B4419A1, 0xDACC4EBE32D0154B, 0x2C84332A1DE215EC, 0xE860AB7A6E0EFD07 },
                new ulong[] { 0xFE9E8C6937A8848A, 0x3B9B50B6A7D4BC89, 0x04BDB0190CBBA3CE, 0x81CE526D51E6B728 },
                new ulong[] { 0x42AC3ACD6CDA865A, 0xB98F312060055827, 0x5716BBC99F86FB1A, 0x688CDA5443C0BB35 },
                new ulong[] { 0xF7145AB12B917C4E, 0x9E98831A21AB3808, 0xCEC6C4943032AF15, 0x4FB30D099076778C },
                new ulong[] { 0x07B09F9DDFA63A90, 0x54D05BCF2860C6AC, 0xF75FAC3474A29B47, 0x8A8D227DC209FFA4 },
                new ulong[] { 0x4B570AFCDDC99319, 0xE7E4C95258F66360, 0x1FE206A7DDF7D605, 0x8C4CFF922C4E246F },
                new ulong[] { 0x28DD2B751C2AF1BC, 0xD53ED0524D8DCC3B, 0x203FF026AF3E4D8C, 0x93D7A91193121801 },
                new ulong[] { 0xAC50D2AA4C7E99FA, 0x7239E003B7C49203, 0x93A10F6C980C4ED8, 0x4649B964F5B8E2E9 },
                new ulong[] { 0x03C3F013E26BDA1E, 0x10CB74269F50123F, 0x63D13471C7A9CE08, 0x0D862C6A83165877 },
                new ulong[] { 0x7E1912864E6960F2, 0x84016543C5C8495A, 0x48B57A3931F09293, 0x06390CE21FCD194F },
                new ulong[] { 0xAF468103472CD1E9, 0x7BAC0397558FCED1, 0x664B227840DC6DC5, 0xB2AFB72E21768255 },
                new ulong[] { 0xAFCCAEE1561F4B4F, 0x4CEE26D0249D3CC1, 0x942B675CE216CBD0, 0x5EEC54389D0326D9 },
                new ulong[] { 0x4D331DFB59A1B736, 0xFD47984347B29A8B, 0x0E7E74EEFE2D0B9A, 0x7614F03ECBC32560 },
                new ulong[] { 0x2EA6ECE0AA80B7E4, 0x65E010A8BE408540, 0x61F232E97F655DF3, 0x47BFDD1491CF6955 },
                new ulong[] { 0x8D9DD1B21C361F74, 0xB58B2ADC6A57142F, 0x542B71B385C31B97, 0x4490D50F59DB987C },
                new ulong[] { 0x89F0441989B4A257, 0x663BB6DD7D90060A, 0x1441310190BA1B66, 0x25EF63A57F441273 },
                new ulong[] { 0xEFACF451CCA37AEA, 0x2BBDAF67A14FF8CC, 0x1484E4FBF1C01745, 0x19949F56A2344C1F },
                new ulong[] { 0x48088959902A7709, 0x52EF43D3D5419B6E, 0x904E83B67593806B, 0x20F43C6BDF7FD9FD },
                new ulong[] { 0xEB198F9E53B2332F, 0xFD4F4F8C6FB83F60, 0x0C1BD5A63D66BF1A, 0xA9E7307201050E82 },
                new ulong[] { 0x82F0B00C88709EA7, 0xE8E94096A10F0739, 0x11027971168BB78B, 0x174519F8BAD727DE },
                new ulong[] { 0xEDF364DBBE96EC63, 0x4905164F524F676E, 0x5974ADAE0E25E22E, 0xEF084462EFDAA256 },
                new ulong[] { 0x9B42FEB9161360E8, 0xC2EF5FC98BB4B616, 0x606BC8DFA3E903B1, 0x7CA37BBBFF12108B },
                new ulong[] { 0x37AAC71C030B4C54, 0xE7A24206AE61FD33, 0x67A6AD145AF1DD94, 0x6ACA7647BD9A0839 },
                new ulong[] { 0xC463225655BE8B7A, 0x1F8C524DDE1476BD, 0xE456BD5619ADFAE5, 0xCC9B6FE512127D57 },
                new ulong[] { 0x6060FFCE0FE638EA, 0x58080129E006ACFD, 0xCAF4285CCEFDE806, 0x112C83770F1DCFFD },
                new ulong[] { 0x116429582CF46D5E, 0xD9AD53F52C121AC3, 0x2967DAB291BC75C7, 0x116632693ADD97D3 },
                new ulong[] { 0xA9C852D8F5B4DED5, 0xC4D1CEEDD6CCF8FB, 0x820B16071ACF59E2, 0xFC69713EF536C12D },
                new ulong[] { 0x027D35E5D9AB8B45, 0xA6422918EC29C67B, 0x773FE180C3471F8D, 0x2D79AF3CC9FD8524 },
                new ulong[] { 0x7571DFAE66C63298, 0x73127B1A0BC31A9B, 0x8BE3BA932592C641, 0x9CBF36EB503D595E },
                new ulong[] { 0x3D8B749335DD7A22, 0x8AA4FA9C820D3923, 0xB078CEC2F5CEC6D3, 0x3AEFE2805838900C },
                new ulong[] { 0xEAC9DE77DBF71B2A, 0x9F0D3ACDBD50BEAD, 0xB5182F5ABAEA6A8A, 0x7C0D1A48FA8E8B5C },
                new ulong[] { 0xE043E36754D57097, 0xE38ABC1F83E47DF0, 0xE2D78C4071AAA7FB, 0xE34F3CF1179DCDD5 },
                new ulong[] { 0x97C7E3649585359A, 0xA816A6D197ACEB71, 0x4F991CD4FBAFC854, 0x4B26D5A8E48CF853 },
                new ulong[] { 0xE4FBDF1E1522BF1C, 0xBC440A2C5EC2F206, 0x5450DC8495458F4A, 0xAC8EBEFBF07B3152 },
                new ulong[] { 0xBAEBE7B56F132FCB, 0x878DE3FF8F138F05, 0x673BC4B87CBD26DB, 0xFF1BDB2224E21F23 },
            };

            var rng = CreateStandardSeed();
            for (int i = 1; i < jumpPolynomials.Length; i++)
            {
                var clone = new Culumi(rng._v0, rng._v1);

                rng.JumpByPolynomial(jumpPolynomials[i]);

                clone.JumpByPolynomial(jumpPolynomials[i - 1]);
                clone.JumpByPolynomial(jumpPolynomials[i - 1]);

                AssertAreEqual(rng.GetState(), clone.GetState());
            }

            rng.Next();
            AssertAreEqual(CreateStandardSeed().GetState(), rng.GetState());
        }

        public static void ReproductionSplit()
        {
            var rng = CreateStandardSeed();
            var split = rng.Split();

            AssertAreEqual((
                Vector128.Create(0x12b5566ceafdd0c9u, 0xfb978ee2b429ee53u),
                Vector128.Create(0x902b9e77ba34c2a0u, 0xdd3a2fbb67b23028u)),
                rng.GetState());
            AssertAreEqual((
                Vector128.Create(0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu),
                Vector128.Create(0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u)),
                split.GetState());
        }

        public static void CharacteristicSplit()
        {
            var rng1 = CreateStandardSeed();
            var rng2 = rng1.Split();

            int i = 1;
            EasyDiscreteUniformStatTest(() => (i ^= 1) != 0 ? rng2.Next()[0] : rng1.Next()[0], 0, Math.ScaleB(1, 64) - 1);
        }

        public static void ReproductionPrev()
        {
            var rng = CreateStandardSeed();

            rng.Prev();
            AssertAreEqual((
                Vector128.Create(0xe6356c47bcbee54au, 0x6d41f19db72149b2u),
                Vector128.Create(0x71f26f924a773261u, 0x012507ee5ab2ff7eu)),
                rng.GetState());
        }

        public static void CharacteristicPrev()
        {
            var rng = CreateStandardSeed();

            rng.Prev();
            rng.Next();

            AssertAreEqual(rng.GetState(), CreateStandardSeed().GetState());

            rng.Next();
            rng.Prev();

            AssertAreEqual(rng.GetState(), CreateStandardSeed().GetState());
        }

        public static void ReproductionGetState()
        {
            var rng = CreateStandardSeed();

            AssertAreEqual((
               Vector128.Create(0x6c64f673ed93b6ccu, 0x97c703d5f6c9d72bu),
               Vector128.Create(0xdcdfab737aa7a8deu, 0x0aaf5961e4dc5255u)),
               rng.GetState());
        }

        public static void CharacteristicGetState()
        {
            var rng = CreateStandardSeed();
            var state = (rng._v0, rng._v1);

            AssertAreEqual(rng.GetState(), state);
        }

        public static void ReproductionNextULongMax()
        {
            var rng = CreateStandardSeed();

            var expected = new ulong[] {
                742929756236564108,
                2582437806625887441,
                944589432147913550,
                264590458212111614,
                1967718512920242987,
                2115427409610720570,
                388919993874446339,
                1532312845729196261,
                1358538974796822344,
                862428756102311802,
                2475482712046206547,
                1500126843508881534,
                2592095587106811278,
                188438027041781279,
                497203363712199929,
                2584781842912658609,
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextULong(3141592653589793238)).ToArray();
            AssertAreEqual(expected, actual);


            AssertAreEqual(0ul, rng.NextULong(0));
            AssertAreEqual(0ul, rng.NextULong(1));

            rng = CreateZeroSeed();
            AssertAreEqual(0ul, rng.NextULong(3141592653589793238));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(3141592653589793238 - 1ul, rng.NextULong(3141592653589793238));

            // it will go to Fallback
            rng = CreateNextSeed(Vector128.Create(3ul, 1ul));
            AssertAreEqual(0ul, rng.NextULong(0x5555555555555555));
        }

        public static void CharacteristicNextULongMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextULong(3141592653589793238), 0, 3141592653589793238 - 1);
        }

        private static void EasyDiscreteUniformStatTest(Func<double> next, double theoreticalMin, double theoreticalMax)
        {
            double theoreticalAverage = theoreticalMin * 0.5 + theoreticalMax * 0.5;
            double theoreticalVariance = ((double)(theoreticalMax - theoreticalMin + 1) * (theoreticalMax - theoreticalMin + 1) - 1.0) / 12.0;
            double range = theoreticalMax - theoreticalMin;
            double min = theoreticalMax;
            double max = theoreticalMin;
            double average = 0;
            double variance = 0;

            for (int i = 0; i < 1 << 24; i++)
            {
                var value = next();

                min = Math.Min(value, min);
                max = Math.Max(value, max);
                average = average * (i / (i + 1.0)) + value / (i + 1.0);
                variance = variance * (i / (i + 1.0)) + (theoreticalAverage - value) * (theoreticalAverage - value) / (i + 1.0);
            }

            Assert(min >= theoreticalMin, theoreticalMin, min);
            Assert(max <= theoreticalMax, theoreticalMax, max);
            Assert(min <= theoreticalMin + range * 0.01, theoreticalMin, min);
            Assert(max >= theoreticalMax - range * 0.01, theoreticalMax, max);
            Assert(theoreticalAverage - range * 0.1 < average && average < theoreticalAverage + range * 0.1, theoreticalAverage, average);
            Assert(theoreticalVariance - range * range * 0.1 < variance && variance < theoreticalVariance + range * range * 0.1, theoreticalVariance, variance);

            ShouldBe(min <= theoreticalMin + range * 0.000001, theoreticalMin, min);
            ShouldBe(max >= theoreticalMax - range * 0.000001, theoreticalMax, max);
            ShouldBe(theoreticalAverage - range * 0.001 < average && average < theoreticalAverage + range * 0.001, theoreticalAverage, average);
            ShouldBe(theoreticalVariance - range * range * 0.001 < variance && variance < theoreticalVariance + range * range * 0.001, theoreticalVariance, variance);
        }

        public static void ReproductionNextULongMinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new ulong[] {
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
                3066565726263576259
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextULong(2718281828459045235, 3141592653589793238)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(0ul, rng.NextULong(0, 1));
            AssertAreEqual(1ul, rng.NextULong(1, 1));
            AssertThrows<ArgumentException>(() => rng.NextULong(1, 0));

            rng = CreateZeroSeed();
            AssertAreEqual(2718281828459045235ul, rng.NextULong(2718281828459045235, 3141592653589793238));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(3141592653589793238ul - 1, rng.NextULong(2718281828459045235, 3141592653589793238));
        }

        public static void CharacteristicNextULongMinMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextULong(2718281828459045235, 3141592653589793238), 2718281828459045235, 3141592653589793238 - 1);
        }

        public static void ReproductionNextLongMax()
        {
            var rng = CreateStandardSeed();

            var expected = new long[] { 742929756236564108, 2582437806625887441, 944589432147913550, 264590458212111614, 1967718512920242987, 2115427409610720570, 388919993874446339, 1532312845729196261, 1358538974796822344, 862428756102311802, 2475482712046206547, 1500126843508881534, 2592095587106811278, 188438027041781279, 497203363712199929, 2584781842912658609 };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextLong(3141592653589793238)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(0, rng.NextLong(0));
            AssertAreEqual(0, rng.NextLong(1));

            rng = CreateZeroSeed();
            AssertAreEqual(0, rng.NextLong(3141592653589793238));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(3141592653589793238 - 1, rng.NextLong(3141592653589793238));
        }

        public static void CharacteristicNextLongMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextLong(3141592653589793238), 0, 3141592653589793238 - 1);
        }

        public static void ReproductionNextLongMinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new long[] { -1332527658340638899, 2098625731861834835, -956380742036621640, -2224752894178181179, 952016893605002761, 1227531796372258784, -1992846484363974289, 139873869398208052, -184258882427493078, -1109631434304877569, 1899127102166341602, 79838736302745847, 2116639964651922238, -2366796671766943841, -1790870281301029026, 2102997959561740959 };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextLong(-2718281828459045235, 3141592653589793238)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(-1, rng.NextLong(-1, 0));
            AssertAreEqual(-1, rng.NextLong(-1, -1));
            AssertThrows<ArgumentException>(() => rng.NextLong(0, -1));

            rng = CreateZeroSeed();
            AssertAreEqual(-2718281828459045235, rng.NextLong(-2718281828459045235, 3141592653589793238));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(3141592653589793238 - 1, rng.NextLong(-2718281828459045235, 3141592653589793238));
        }

        public static void CharacteristicNextLongMinMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextLong(-2718281828459045235, 3141592653589793238), -2718281828459045235, 3141592653589793238 - 1);
        }

        public static void ReproductionNextULong2Max()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<ulong>[] {
                Vector128.Create(642824413881842228, 640446909418979297u),
                Vector128.Create(2234469753694992629, 975678929893421886u),
                Vector128.Create(817311654274510045, 708749682593666276u),
                Vector128.Create(228938476068752442, 1484859316810116545u),
                Vector128.Create(1830386215220583449, 123257508420232023u),
                Vector128.Create(336515350220624606, 2370570856301814224u),
                Vector128.Create(1175483971234729812, 2702514123101591303u),
                Vector128.Create(746221638051855864, 3138952702220089349u),
                Vector128.Create(2141926218579180290, 575281382351239722u),
                Vector128.Create(2242826206004156195, 1823541151125566792u),
                Vector128.Create(163047129650320114, 2007334316149760735u),
                Vector128.Create(430208183445816279, 1257150847798669689u),
                Vector128.Create(2236497945108127585, 117060502346660430u),
                Vector128.Create(2547391800036822557, 1230191090070468735u),
                Vector128.Create(890109616937845051, 267840945541771076u),
                Vector128.Create(966782828187926018, 1155557803751969213u),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextULong2(Vector128.Create(2718281828459045235u, 3141592653589793238u))).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128<ulong>.Zero, rng.NextULong2(Vector128.Create(0ul, 1ul)));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<ulong>.Zero, rng.NextULong2(Vector128.Create(2718281828459045235u, 3141592653589793238u)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(2718281828459045235u - 1, 3141592653589793238u - 1),
                rng.NextULong2(Vector128.Create(2718281828459045235u, 3141592653589793238u)));

            // it will go to Fallback
            rng = CreateNextSeed(Vector128.Create(3ul, 6ul));
            AssertAreEqual(Vector128.Create(1ul, 2ul), rng.NextULong2(Vector128.Create(0x5555555555555555u, 0x5555555555555555u)));
        }

        public static void CharacteristicNextULong2Max()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextULong2(Vector128.Create(2718281828459045235u, 3141592653589793238u))[0], 0, 2718281828459045235u - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextULong2(Vector128.Create(2718281828459045235u, 3141592653589793238u))[1], 0, 3141592653589793238u - 1);
        }

        public static void ReproductionNextULong2MinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<ulong>[] {
                Vector128.Create(642824413989819995, 640446909432768836u),
                Vector128.Create(2234469753720163440, 975678929905363194u),
                Vector128.Create(817311654373409937, 708749682607079242u),
                Vector128.Create(228938476198263042, 1484859316819250595u),
                Vector128.Create(1702580209196647911, 1193783293223463979u),
                Vector128.Create(1830386215266777115, 123257508436872977u),
                Vector128.Create(336515350344538411, 2370570856306065090u),
                Vector128.Create(1325842852200500069, 730699164888036831u),
                Vector128.Create(1175483971314995428, 2702514123104012070u),
                Vector128.Create(2141926218609165772, 575281382365388537u),
                Vector128.Create(1297993721326801470, 2468101504187667118u),
                Vector128.Create(2242826206028892253, 1823541151132833591u),
                Vector128.Create(163047129783258779, 2007334316156014229u),
                Vector128.Create(430208183564855620, 1257150847809059161u),
                Vector128.Create(2236497945133192878, 117060502363335549u),
                Vector128.Create(2547391800045713283, 1230191090081006844u),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextULong2(Vector128.Create(141421356, 17320508ul), Vector128.Create(2718281828459045235u, 3141592653589793238u))).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128.Create(1ul, 1ul), rng.NextULong2(Vector128.Create(1ul, 1ul), Vector128.Create(1ul, 2ul)));
            AssertThrows<ArgumentException>(() => rng.NextULong2(Vector128.Create(1ul, 1ul), Vector128.Create(0ul, 1ul)));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(141421356, 17320508ul), rng.NextULong2(Vector128.Create(141421356, 17320508ul), Vector128.Create(2718281828459045235u, 3141592653589793238u)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(2718281828459045235u - 1, 3141592653589793238u - 1),
                rng.NextULong2(Vector128.Create(141421356, 17320508ul), Vector128.Create(2718281828459045235u, 3141592653589793238u)));
        }

        public static void CharacteristicNextULong2MinMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextULong2(Vector128.Create(141421356, 17320508ul), Vector128.Create(2718281828459045235u, 3141592653589793238u))[0], 141421356, 2718281828459045235u - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextULong2(Vector128.Create(141421356, 17320508ul), Vector128.Create(2718281828459045235u, 3141592653589793238u))[1], 17320508ul, 3141592653589793238u - 1);
        }

        public static void ReproductionNextULong2Max1()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<ulong>[] {
                Vector128.Create(742929756236564108, 640446909418979297u),
                Vector128.Create(2582437806625887441, 975678929893421886u),
                Vector128.Create(264590458212111614, 1484859316810116545u),
                Vector128.Create(2115427409610720570, 123257508420232023u),
                Vector128.Create(388919993874446339, 2370570856301814224u),
                Vector128.Create(1358538974796822344, 2702514123101591303u),
                Vector128.Create(2475482712046206547, 575281382351239722u),
                Vector128.Create(2592095587106811278, 1823541151125566792u),
                Vector128.Create(188438027041781279, 2007334316149760735u),
                Vector128.Create(497203363712199929, 1257150847798669689u),
                Vector128.Create(2584781842912658609, 117060502346660430u),
                Vector128.Create(2944090373935682535, 1230191090070468735u),
                Vector128.Create(1028724028606988186, 267840945541771076u),
                Vector128.Create(1117337355845003746, 1155557803751969213u),
                Vector128.Create(1486680260422671190, 2262785641788971421u),
                Vector128.Create(1192755674326443027, 1839704557346092644u),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextULong2(3141592653589793238u)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128<ulong>.Zero, rng.NextULong2(0));
            AssertAreEqual(Vector128<ulong>.Zero, rng.NextULong2(1));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<ulong>.Zero, rng.NextULong2(3141592653589793238u));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(3141592653589793238u - 1, 3141592653589793238u - 1),
                rng.NextULong2(3141592653589793238u));

            // it will go to Fallback
            rng = CreateNextSeed(Vector128.Create(3ul, 6ul));
            AssertAreEqual(Vector128.Create(1ul, 2ul), rng.NextULong2(0x5555555555555555u));
        }

        public static void CharacteristicNextULong2Max1()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextULong2(3141592653589793238u)[0], 0, 3141592653589793238u - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextULong2(3141592653589793238u)[1], 0, 3141592653589793238u - 1);
        }

        public static void ReproductionNextULong2MinMax1()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<ulong>[] {
                Vector128.Create(742929756344541875, 640446909531570413u),
                Vector128.Create(2582437806651058253, 975678929990922259u),
                Vector128.Create(944589432246813442, 708749682703182687u),
                Vector128.Create(264590458341622215, 1484859316884695748u),
                Vector128.Create(1967718512973085889, 1193783293300407370u),
                Vector128.Create(388919993998360145, 2370570856336522399u),
                Vector128.Create(1532312845801639305, 730699164983273216u),
                Vector128.Create(862428756204910224, 3138952702220208189u),
                Vector128.Create(2475482712076192029, 575281382466764317u),
                Vector128.Create(2592095587131547336, 1823541151184899955u),
                Vector128.Create(497203363831239270, 1257150847883499374u),
                Vector128.Create(2584781842937723902, 117060502482812212u),
                Vector128.Create(2944090373944573261, 1230191090156512036u),
                Vector128.Create(1028724028702100690, 267840945671135353u),
                Vector128.Create(1117337355936127250, 1155557803841372192u),
                Vector128.Create(1486680260497168421, 2262785641828531636u),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextULong2(141421356, 3141592653589793238u)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128.Create(1ul, 1ul), rng.NextULong2(1ul, 1ul));
            AssertAreEqual(Vector128.Create(1ul, 1ul), rng.NextULong2(1ul, 2ul));
            AssertThrows<ArgumentException>(() => rng.NextULong2(1ul, 0ul));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(141421356, 141421356ul), rng.NextULong2(141421356, 3141592653589793238u));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(3141592653589793238u - 1, 3141592653589793238u - 1),
                rng.NextULong2(141421356, 3141592653589793238u));
        }

        public static void CharacteristicNextULong2MinMax1()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextULong2(141421356, 3141592653589793238u)[0], 141421356, 3141592653589793238u - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextULong2(141421356, 3141592653589793238u)[1], 141421356, 3141592653589793238u - 1);
        }

        public static void ReproductionNextLong2Max()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<long>[] {
                Vector128.Create(642824413881842228, 640446909418979297),
                Vector128.Create(2234469753694992629, 975678929893421886),
                Vector128.Create(817311654274510045, 708749682593666276),
                Vector128.Create(228938476068752442, 1484859316810116545),
                Vector128.Create(1830386215220583449, 123257508420232023),
                Vector128.Create(336515350220624606, 2370570856301814224),
                Vector128.Create(1175483971234729812, 2702514123101591303),
                Vector128.Create(746221638051855864, 3138952702220089349),
                Vector128.Create(2141926218579180290, 575281382351239722),
                Vector128.Create(2242826206004156195, 1823541151125566792),
                Vector128.Create(163047129650320114, 2007334316149760735),
                Vector128.Create(430208183445816279, 1257150847798669689),
                Vector128.Create(2236497945108127585, 117060502346660430),
                Vector128.Create(2547391800036822557, 1230191090070468735),
                Vector128.Create(890109616937845051, 267840945541771076),
                Vector128.Create(966782828187926018, 1155557803751969213),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextLong2(Vector128.Create(2718281828459045235, 3141592653589793238))).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128<long>.Zero, rng.NextLong2(Vector128.Create(0, 1)));
            AssertThrows<ArgumentException>(() => rng.NextLong2(Vector128.Create(-1, 0)));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<long>.Zero, rng.NextLong2(Vector128.Create(2718281828459045235, 3141592653589793238)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(2718281828459045235 - 1, 3141592653589793238 - 1),
                rng.NextLong2(Vector128.Create(2718281828459045235, 3141592653589793238)));

            // it will go to Fallback
            rng = CreateNextSeed(Vector128.Create(3ul, 6ul));
            AssertAreEqual(Vector128.Create(1, 2), rng.NextLong2(Vector128.Create(0x5555555555555555, 0x5555555555555555)));
        }

        public static void CharacteristicNextLong2Max()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextLong2(Vector128.Create(2718281828459045235, 3141592653589793238))[0], 0, 2718281828459045235u - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextLong2(Vector128.Create(2718281828459045235, 3141592653589793238))[1], 0, 3141592653589793238u - 1);
        }

        public static void ReproductionNextLong2MinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<long>[] {
                Vector128.Create(-1432633000695360778, -1860698834751834644),
                Vector128.Create(-1083658519910025145, -1724093288402460685),
                Vector128.Create(-2260404876321540351, -171874019969560147),
                Vector128.Create(942490601982121664, -2895077636749329191),
                Vector128.Create(-2045251128017796023, 1599549059013835211),
                Vector128.Create(-367313885989585610, 2263435592613389368),
                Vector128.Create(-1225838552355333507, 3136312750850385461),
                Vector128.Create(1565570608699315345, -1991029888887313794),
                Vector128.Create(1767370583549267155, 505489648661340346),
                Vector128.Create(-2392187569158405006, 873075978709728232),
                Vector128.Create(-1857865461567412676, -627290957992453860),
                Vector128.Create(1754714061757209936, -2907471648896472378),
                Vector128.Create(-938062594583355133, -2605910762506251086),
                Vector128.Create(-784716172083193198, -830477046085854812),
                Vector128.Create(-654203870033608710, 537816461102392050),
                Vector128.Create(-130093336813404754, -1863936928145027332),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextLong2(Vector128.Create(-2718281828459045235, -3141592653589793238), Vector128.Create(2718281828459045235, 3141592653589793238))).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128.Create(-1, -1), rng.NextLong2(Vector128.Create(-1, -1), Vector128.Create(-1, 0)));
            AssertThrows<ArgumentException>(() => rng.NextLong2(Vector128.Create(-1, -1), Vector128.Create(-2, -1)));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(141421356, 17320508), rng.NextLong2(Vector128.Create(141421356, 17320508), Vector128.Create(2718281828459045235, 3141592653589793238)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(2718281828459045235 - 1, 3141592653589793238 - 1),
                rng.NextLong2(Vector128.Create(141421356, 17320508), Vector128.Create(2718281828459045235, 3141592653589793238)));
        }

        public static void CharacteristicNextLong2MinMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextLong2(Vector128.Create(141421356, 17320508), Vector128.Create(2718281828459045235, 3141592653589793238))[0], 141421356, 2718281828459045235 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextLong2(Vector128.Create(141421356, 17320508), Vector128.Create(2718281828459045235, 3141592653589793238))[1], 17320508, 3141592653589793238 - 1);
        }

        public static void ReproductionNextLong2Max1()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<long>[] {
                Vector128.Create(742929756236564108, 640446909418979297),
                Vector128.Create(2582437806625887441, 975678929893421886),
                Vector128.Create(264590458212111614, 1484859316810116545),
                Vector128.Create(2115427409610720570, 123257508420232023),
                Vector128.Create(388919993874446339, 2370570856301814224),
                Vector128.Create(1358538974796822344, 2702514123101591303),
                Vector128.Create(2475482712046206547, 575281382351239722),
                Vector128.Create(2592095587106811278, 1823541151125566792),
                Vector128.Create(188438027041781279, 2007334316149760735),
                Vector128.Create(497203363712199929, 1257150847798669689),
                Vector128.Create(2584781842912658609, 117060502346660430),
                Vector128.Create(2944090373935682535, 1230191090070468735),
                Vector128.Create(1028724028606988186, 267840945541771076),
                Vector128.Create(1117337355845003746, 1155557803751969213),
                Vector128.Create(1486680260422671190, 2262785641788971421),
                Vector128.Create(1192755674326443027, 1839704557346092644),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextLong2(3141592653589793238)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128<long>.Zero, rng.NextLong2(0));
            AssertAreEqual(Vector128<long>.Zero, rng.NextLong2(1));
            AssertThrows<ArgumentException>(() => rng.NextLong2(-1));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<long>.Zero, rng.NextLong2(3141592653589793238));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(3141592653589793238 - 1, 3141592653589793238 - 1),
                rng.NextLong2(3141592653589793238));

            // it will go to Fallback
            rng = CreateNextSeed(Vector128.Create(3ul, 6ul));
            AssertAreEqual(Vector128.Create(1, 2), rng.NextLong2(0x5555555555555555));
        }

        public static void CharacteristicNextLong2Max1()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextLong2(3141592653589793238)[0], 0, 3141592653589793238 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextLong2(3141592653589793238)[1], 0, 3141592653589793238 - 1);
        }

        public static void ReproductionNextLong2MinMax1()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<long>[] {
                Vector128.Create(-1332527658340638898, -1523684401156379846),
                Vector128.Create(-956380742036621640, -1396282245080489242),
                Vector128.Create(952016893605002761, -491570402647668703),
                Vector128.Create(1227531796372258784, -2488375023868284691),
                Vector128.Create(139873869398208053, -1355340842010264825),
                Vector128.Create(-1109631434304877569, 3136668468512042661),
                Vector128.Create(79838736302745847, 1885359260065842530),
                Vector128.Create(2116639964651922238, 683089207418713979),
                Vector128.Create(-1790870281301029026, -373373692554235189),
                Vector128.Create(2773200345513459857, -423660541860807236),
                Vector128.Create(-799448182914211998, -2218689903250465051),
                Vector128.Create(-634161644426115470, -562870725431068732),
                Vector128.Create(54757416798259242, 1502392620061309198),
                Vector128.Create(-493487174919883945, 713238097519542492),
                Vector128.Create(-2686288349312388111, -1515234782556957915),
                Vector128.Create(-1123676558471514158, 2680457816398943470),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextLong2(-2718281828459045235, 3141592653589793238)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128.Create(-1, -1), rng.NextLong2(-1, -1));
            AssertAreEqual(Vector128.Create(-1, -1), rng.NextLong2(-1, 0));
            AssertThrows<ArgumentException>(() => rng.NextLong2(-1, -2));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(-2718281828459045235, -2718281828459045235),
                rng.NextLong2(-2718281828459045235, 3141592653589793238));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(3141592653589793238 - 1, 3141592653589793238 - 1),
                rng.NextLong2(-2718281828459045235, 3141592653589793238));
        }

        public static void CharacteristicNextLong2MinMax1()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextLong2(-2718281828459045235, 3141592653589793238)[0], -2718281828459045235, 3141592653589793238 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextLong2(-2718281828459045235, 3141592653589793238)[1], -2718281828459045235, 3141592653589793238 - 1);
        }

        public static void ReproductionNextUIntMax()
        {
            var rng = CreateStandardSeed();

            var expected = new uint[] { 742929756, 2582437806, 944589431, 264590458, 1967718512, 2115427409, 388919993, 1532312845, 1358538974, 862428755, 2475482711, 1500126843, 2592095586, 188438027, 497203363, 2584781842 };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextUInt(3141592653)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(0u, rng.NextUInt(0));
            AssertAreEqual(0u, rng.NextUInt(1));

            rng = CreateZeroSeed();
            AssertAreEqual(0ul, rng.NextUInt(3141592653));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(3141592653 - 1ul, rng.NextUInt(3141592653));

            // it will go to Fallback       
            rng = CreateNextSeed(Vector128.Create(0x1111111111111111ul, 1ul));
            AssertAreEqual(0ul, rng.NextUInt(15));
        }

        public static void CharacteristicNextUIntMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt(3141592653), 0, 3141592653 - 1);
        }

        public static void ReproductionNextUIntMinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new uint[] { 2818387170, 3066249880, 2845559605, 2753933810, 2983420131, 3003323022, 2770686471, 2924751821, 2901336831, 2834488946, 3051838321, 2920414950, 3067551208, 2743672725, 2785277008, 3066565725 };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextUInt(2718281828, 3141592653)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(0ul, rng.NextUInt(0, 1));
            AssertAreEqual(1ul, rng.NextUInt(1, 1));
            AssertThrows<ArgumentException>(() => rng.NextUInt(1, 0));

            rng = CreateZeroSeed();
            AssertAreEqual(2718281828, rng.NextUInt(2718281828, 3141592653));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(3141592653 - 1, rng.NextUInt(2718281828, 3141592653));
        }

        public static void CharacteristicNextUIntMinMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt(2718281828, 3141592653), 2718281828, 3141592653 - 1);
        }

        public static void ReproductionNextUInt4Max()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<uint>[] {
                Vector128.Create(33443589, 1932209, 554150517, 810898495u),
                Vector128.Create(42521463, 17070176, 613249900, 608513352u),
                Vector128.Create(88578454, 1426242, 1032928132, 1906924737u),
                Vector128.Create(17507550, 10044855, 2051150607, 1014150411u),
                Vector128.Create(61155740, 6523343, 2338366504, 1929802611u),
                Vector128.Create(111435873, 8471717, 497765656, 1011063092u),
                Vector128.Create(116685297, 11123912, 1577829884, 3104593167u),
                Vector128.Create(22382015, 11551347, 1087757287, 1361203418u),
                Vector128.Create(132530629, 9743283, 1064430195, 3114516752u),
                Vector128.Create(50297852, 2079548, 999853299, 416015286u),
                Vector128.Create(53692869, 7493112, 1591815368, 1391718705u),
                Vector128.Create(772125, 4757197, 558070131, 818628264u),
                Vector128.Create(112942512, 11256435, 813533837, 2388569130u),
                Vector128.Create(68003122, 14096283, 2608961949, 1503594076u),
                Vector128.Create(69308255, 7463508, 39385568, 90417613u),
                Vector128.Create(69898321, 13826117, 1063498249, 563973865u),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextUInt4(Vector128.Create(141421356, 17320508, 2718281828, 3141592653))).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128<uint>.Zero, rng.NextUInt4(Vector128.Create(0u, 1, 0, 1)));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<uint>.Zero, rng.NextUInt4(Vector128.Create(141421356, 17320508, 2718281828, 3141592653)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(141421356 - 1, 17320508 - 1, 2718281828 - 1, 3141592653 - 1),
                rng.NextUInt4(Vector128.Create(141421356, 17320508, 2718281828, 3141592653)));

            // it will go to Fallback
            rng = CreateNextSeed(Vector128.Create(6u, 3u, 12u, 9u).AsUInt64());
            AssertAreEqual(Vector128.Create(1u, 2u, 3u, 4u), rng.NextUInt4(Vector128.Create(0x55555555u, 0x55555555u, 0x55555555u, 0x55555555u)));
        }

        public static void CharacteristicNextUInt4Max()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(Vector128.Create(141421356, 17320508, 2718281828, 3141592653))[0], 0, 141421356 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(Vector128.Create(141421356, 17320508, 2718281828, 3141592653))[1], 0, 17320508 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(Vector128.Create(141421356, 17320508, 2718281828, 3141592653))[2], 0, 2718281828 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(Vector128.Create(141421356, 17320508, 2718281828, 3141592653))[3], 0, 3141592653 - 1);
        }

        public static void ReproductionNextUInt4MinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<uint>[] {
                Vector128.Create(45797570, 4077102, 580445216, 1125165173u),
                Vector128.Create(119130394, 15687383, 866982411, 894986046u),
                Vector128.Create(26728357, 16803449, 1302200741, 899467717u),
                Vector128.Create(100512812, 8749061, 138381238, 1565923199u),
                Vector128.Create(31684811, 11058969, 2059256407, 1301010960u),
                Vector128.Create(70339111, 8028302, 2342982563, 2093198237u),
                Vector128.Create(114866580, 9705103, 524745443, 1298339930u),
                Vector128.Create(75983576, 7862495, 2142620037, 2370281639u),
                Vector128.Create(119515406, 11987622, 1591686640, 3109582112u),
                Vector128.Create(23692506, 13859096, 1748782546, 3103209525u),
                Vector128.Create(36001576, 12355480, 1107568539, 1601267954u),
                Vector128.Create(133547837, 10799432, 1084524877, 3118167619u),
                Vector128.Create(60723499, 4203904, 1020732605, 783527317u),
                Vector128.Create(63730085, 8862900, 1605502197, 1627668613u),
                Vector128.Create(16864124, 6508329, 584317206, 1131852673u),
                Vector128.Create(116200841, 12101674, 836676968, 2490105490u),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextUInt4(
                Vector128.Create(16180339u, 2414213, 33027756, 423606797),
                Vector128.Create(141421356, 17320508, 2718281828, 3141592653))).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128.Create(1u, 1u, 0u, 2u), rng.NextUInt4(Vector128.Create(1u, 1, 0, 2), Vector128.Create(1u, 2u, 1, 2)));
            AssertThrows<ArgumentException>(() => rng.NextUInt4(Vector128.Create(1u, 1u, 1u, 2u), Vector128.Create(0u, 1u, 2u, 3u)));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(16180339u, 2414213, 33027756, 423606797), rng.NextUInt4(
                Vector128.Create(16180339u, 2414213, 33027756, 423606797),
                Vector128.Create(141421356, 17320508, 2718281828, 3141592653)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(141421356 - 1, 17320508 - 1, 2718281828 - 1, 3141592653 - 1), rng.NextUInt4(
                Vector128.Create(16180339u, 2414213, 33027756, 423606797),
                Vector128.Create(141421356, 17320508, 2718281828, 3141592653)));
        }

        public static void CharacteristicNextUInt4MinMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(Vector128.Create(16180339u, 2414213, 33027756, 423606797), Vector128.Create(141421356, 17320508, 2718281828, 3141592653))[0], 16180339u, 141421356 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(Vector128.Create(16180339u, 2414213, 33027756, 423606797), Vector128.Create(141421356, 17320508, 2718281828, 3141592653))[1], 2414213, 17320508 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(Vector128.Create(16180339u, 2414213, 33027756, 423606797), Vector128.Create(141421356, 17320508, 2718281828, 3141592653))[2], 33027756, 2718281828 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(Vector128.Create(16180339u, 2414213, 33027756, 423606797), Vector128.Create(141421356, 17320508, 2718281828, 3141592653))[3], 423606797, 3141592653 - 1);
        }

        public static void ReproductionNextUInt4Max1()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<uint>[] {
                Vector128.Create(742929756, 350464077, 640446909, 810898495u),
                Vector128.Create(944589431, 3096187560, 708749682, 608513352u),
                Vector128.Create(1967718512, 258691691, 1193783292, 1906924737u),
                Vector128.Create(388919993, 1821935231, 2370570855, 1014150411u),
                Vector128.Create(1358538974, 1183203693, 2702514122, 1929802611u),
                Vector128.Create(2475482711, 1536599630, 575281382, 1011063092u),
                Vector128.Create(2592095586, 2017654493, 1823541150, 3104593167u),
                Vector128.Create(497203363, 2095182747, 1257150847, 1361203418u),
                Vector128.Create(2944090373, 1767236090, 1230191089, 3114516752u),
                Vector128.Create(1117337355, 377188360, 1155557803, 416015286u),
                Vector128.Create(1192755674, 1359100215, 1839704556, 1391718705u),
                Vector128.Create(17152326, 862860143, 644976914, 818628264u),
                Vector128.Create(2508951814, 2041691606, 940223306, 2388569130u),
                Vector128.Create(1510649571, 2556782948, 3015248679, 1503594076u),
                Vector128.Create(1539642321, 1353730700, 45518978, 90417613u),
                Vector128.Create(1552750308, 2507780245, 1229114013, 563973865u),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextUInt4(3141592653)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128<uint>.Zero, rng.NextUInt4(0));
            AssertAreEqual(Vector128<uint>.Zero, rng.NextUInt4(1));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<uint>.Zero, rng.NextUInt4(3141592653));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(3141592653 - 1, 3141592653 - 1, 3141592653 - 1, 3141592653 - 1),
                rng.NextUInt4(3141592653));

            // it will go to Fallback
            rng = CreateNextSeed(Vector128.Create(6u, 3u, 12u, 9u).AsUInt64());
            AssertAreEqual(Vector128.Create(1u, 2u, 3u, 4u), rng.NextUInt4(0x55555555u));
        }

        public static void CharacteristicNextUInt4Max1()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(168198401)[0], 0, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(168198401)[1], 0, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(168198401)[2], 0, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(168198401)[3], 0, 168198401 - 1);
        }

        public static void ReproductionNextUInt4MinMax1()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<uint>[] {
                Vector128.Create(2818387170, 2765504766, 2804578219, 2827545551u),
                Vector128.Create(3066249880, 3095214974, 2849748729, 2791696462u),
                Vector128.Create(2845559605, 3135474588, 2813781609, 2800275363u),
                Vector128.Create(2753933810, 3126909170, 2918357742, 2792394457u),
                Vector128.Create(3003323022, 2898179645, 2734890040, 2896191105u),
                Vector128.Create(2770686471, 2963776706, 3037702076, 2854932531u),
                Vector128.Create(2924751821, 2853897195, 2816739171, 2719190596u),
                Vector128.Create(2901336831, 2877711450, 3082429446, 2978311190u),
                Vector128.Create(2834488946, 2997765074, 3141236935, 3069172623u),
                Vector128.Create(2920414950, 2873002842, 3050843747, 3021465353u),
                Vector128.Create(3067551208, 2990148706, 2963993094, 3136607193u),
                Vector128.Create(2785277008, 3000595175, 2887675387, 2901695849u),
                Vector128.Create(3066565725, 2934634458, 2734055030, 2756981287u),
                Vector128.Create(3114980401, 2956406324, 2884042721, 3137944337u),
                Vector128.Create(2856896239, 2808728767, 2754371793, 2929863374u),
                Vector128.Create(2868836355, 2769105704, 2873986332, 2774337400u),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextUInt4(2718281828, 3141592653)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128.Create(1u, 1u, 1u, 1u), rng.NextUInt4(1, 1));
            AssertAreEqual(Vector128.Create(1u, 1u, 1u, 1u), rng.NextUInt4(1, 2));
            AssertThrows<ArgumentException>(() => rng.NextUInt4(1, 0));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(2718281828, 2718281828, 2718281828, 2718281828), rng.NextUInt4(2718281828, 3141592653));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(3141592653 - 1, 3141592653 - 1, 3141592653 - 1, 3141592653 - 1), rng.NextUInt4(2718281828, 3141592653));
        }

        public static void CharacteristicNextUInt4MinMax1()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(1314, 168198401)[0], 1314, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(1314, 168198401)[1], 1314, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(1314, 168198401)[2], 1314, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextUInt4(1314, 168198401)[3], 1314, 168198401 - 1);
        }

        public static void ReproductionNextIntMax()
        {
            var rng = CreateStandardSeed();

            var expected = new int[] { 74292975, 258243780, 94458943, 26459045, 196771851, 211542740, 38891999, 153231284, 135853897, 86242875, 247548270, 150012684, 259209558, 18843802, 49720336, 258478183 };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextInt(314159265)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(0, rng.NextInt(0));
            AssertAreEqual(0, rng.NextInt(1));
            AssertThrows<ArgumentException>(() => rng.NextInt(-1));

            rng = CreateZeroSeed();
            AssertAreEqual(0, rng.NextInt(314159265));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(314159265 - 1, rng.NextInt(314159265));

            // it will go to Fallback  
            rng = CreateNextSeed(Vector128.Create(0x1111111111111111ul, 1ul));
            AssertAreEqual(0, rng.NextInt(15));
        }

        public static void CharacteristicNextIntMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt(314159265), 0, 314159265 - 1);
        }

        public static void ReproductionNextIntMinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new int[] { -133252766, 209862573, -95638074, -222475289, 95201689, 122753179, -199284648, 13987387, -18425888, -110963143, 189912710, 7983873, 211663996, -236679667, -179087028, 210299795 };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextInt(-271828182, 314159265)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(-1, rng.NextInt(-1, 0));
            AssertAreEqual(-1, rng.NextInt(-1, -1));
            AssertThrows<ArgumentException>(() => rng.NextInt(1, -1));

            rng = CreateZeroSeed();
            AssertAreEqual(-271828182, rng.NextInt(-271828182, 314159265));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(314159265 - 1, rng.NextInt(-271828182, 314159265));
        }

        public static void CharacteristicNextIntMinMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt(-271828182, 314159265), -271828182, 314159265 - 1);
        }

        public static void ReproductionNextInt4Max()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<int>[] {
                Vector128.Create(33443589, 1932209, 55415051, 81089849),
                Vector128.Create(116250544, 15422883, 84421202, 54484521),
                Vector128.Create(42521463, 17070176, 61324989, 60851335),
                Vector128.Create(11910755, 16719707, 128478339, 55002536),
                Vector128.Create(88578454, 1426242, 103292812, 190692473),
                Vector128.Create(17507550, 10044855, 205115060, 101415041),
                Vector128.Create(68978312, 5548941, 63224182, 674440),
                Vector128.Create(38822933, 11435549, 271599758, 260412887),
                Vector128.Create(111435873, 8471717, 49776565, 101106309),
                Vector128.Create(67529433, 6330682, 213553957, 225007034),
                Vector128.Create(116685297, 11123912, 157782987, 310459316),
                Vector128.Create(8482691, 13298488, 173685801, 309722738),
                Vector128.Create(22382015, 11551347, 108775728, 136120341),
                Vector128.Create(132530629, 9743283, 106443019, 311451674),
                Vector128.Create(50297852, 2079548, 99985329, 41601528),
                Vector128.Create(66924124, 14198116, 195788880, 188414422),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextInt4(Vector128.Create(141421356, 17320508, 271828182, 314159265))).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128<int>.Zero, rng.NextInt4(Vector128.Create(0, 1, 0, 1)));
            AssertThrows<ArgumentException>(() => rng.NextInt4(Vector128.Create(0, -1, 0, -1)));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<int>.Zero, rng.NextInt4(Vector128.Create(141421356, 17320508, 271828182, 314159265)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(141421356 - 1, 17320508 - 1, 271828182 - 1, 314159265 - 1),
                rng.NextInt4(Vector128.Create(141421356, 17320508, 271828182, 314159265)));

            // it will go to Fallback
            rng = CreateNextSeed(Vector128.Create(6u, 3u, 12u, 9u).AsUInt64());
            AssertAreEqual(Vector128.Create(1, 2, 3, 4), rng.NextInt4(Vector128.Create(0x55555555, 0x55555555, 0x55555555, 0x55555555)));
        }

        public static void CharacteristicNextInt4Max()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(Vector128.Create(141421356, 17320508, 271828182, 314159265))[0], 0, 141421356 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(Vector128.Create(141421356, 17320508, 271828182, 314159265))[1], 0, 17320508 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(Vector128.Create(141421356, 17320508, 271828182, 314159265))[2], 0, 271828182 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(Vector128.Create(141421356, 17320508, 271828182, 314159265))[3], 0, 314159265 - 1);
        }

        public static void ReproductionNextInt4MinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<int>[] {
                Vector128.Create(21089607, -212683, 29120353, -233176829),
                Vector128.Create(113370694, 15158383, 61650818, -295656311),
                Vector128.Create(31206102, 17035284, 35748362, -280704598),
                Vector128.Create(-2906848, 16635965, 111061000, -294439813),
                Vector128.Create(82532577, -789175, 82815373, 24212035),
                Vector128.Create(89942568, 5972610, -21067013, -113538122),
                Vector128.Create(3330290, 9030741, 197009260, -185445509),
                Vector128.Create(51972368, 5018384, 229220590, 29584634),
                Vector128.Create(27084415, 10615276, 271572004, 187942222),
                Vector128.Create(108005166, 7238332, 22796777, -186170529),
                Vector128.Create(59075290, 4798869, 206473504, 104795743),
                Vector128.Create(-6727124, 12737880, 161761274, 303740605),
                Vector128.Create(113488285, 7672131, -21668362, -356159554),
                Vector128.Create(35426813, 1802416, -7036830, -54852504),
                Vector128.Create(39872205, -44808, 79106023, -325910504),
                Vector128.Create(43655653, 6123324, 145494707, -96778038),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextInt4(
                Vector128.Create(-16180339, -2414213, -33027756, -423606797),
                Vector128.Create(141421356, 17320508, 271828182, 314159265))).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128.Create(-1, 0, 1, 2), rng.NextInt4(Vector128.Create(-1, 0, 1, 2), Vector128.Create(0, 0, 2, 2)));
            AssertThrows<ArgumentException>(() => rng.NextInt4(Vector128.Create(-1, 0, 1, 2), Vector128.Create(-1, -1, 1, 1)));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(-16180339, -2414213, -33027756, -423606797), rng.NextInt4(
                Vector128.Create(-16180339, -2414213, -33027756, -423606797),
                Vector128.Create(141421356, 17320508, 271828182, 314159265)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(141421356 - 1, 17320508 - 1, 271828182 - 1, 314159265 - 1), rng.NextInt4(
                Vector128.Create(-16180339, -2414213, -33027756, -423606797),
                Vector128.Create(141421356, 17320508, 271828182, 314159265)));
        }

        public static void CharacteristicNextInt4MinMax()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(Vector128.Create(-16180339, -2414213, -33027756, -423606797), Vector128.Create(141421356, 17320508, 271828182, 314159265))[0], -16180339, 141421356 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(Vector128.Create(-16180339, -2414213, -33027756, -423606797), Vector128.Create(141421356, 17320508, 271828182, 314159265))[1], -2414213, 17320508 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(Vector128.Create(-16180339, -2414213, -33027756, -423606797), Vector128.Create(141421356, 17320508, 271828182, 314159265))[2], -33027756, 271828182 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(Vector128.Create(-16180339, -2414213, -33027756, -423606797), Vector128.Create(141421356, 17320508, 271828182, 314159265))[3], -423606797, 314159265 - 1);
        }

        public static void ReproductionNextInt4Max1()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<int>[] {
                Vector128.Create(74292975, 35046407, 64044690, 81089849),
                Vector128.Create(258243780, 279740165, 97567892, 54484521),
                Vector128.Create(94458943, 309618755, 70874968, 60851335),
                Vector128.Create(26459045, 303261948, 148485931, 55002536),
                Vector128.Create(196771851, 25869169, 119378329, 190692473),
                Vector128.Create(211542740, 133510797, 12325750, 132035006),
                Vector128.Create(153231284, 100646668, 73069916, 674440),
                Vector128.Create(135853897, 118320369, 270251412, 192980260),
                Vector128.Create(247548270, 153659962, 57528138, 101106309),
                Vector128.Create(150012684, 114825885, 246810150, 225007034),
                Vector128.Create(259209558, 201765449, 182354114, 310459316),
                Vector128.Create(18843802, 241207894, 200733431, 309722738),
                Vector128.Create(49720336, 209518274, 125715084, 136120341),
                Vector128.Create(258478184, 160565663, 11706050, 28720725),
                Vector128.Create(102872402, 67125011, 26784094, 157024812),
                Vector128.Create(111733735, 37718835, 115555780, 41601528),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextInt4(314159265)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128<int>.Zero, rng.NextInt4(0));
            AssertAreEqual(Vector128<int>.Zero, rng.NextInt4(1));
            AssertThrows<ArgumentException>(() => rng.NextInt4(-1));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<int>.Zero, rng.NextInt4(314159265));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(314159265 - 1, 314159265 - 1, 314159265 - 1, 314159265 - 1),
                rng.NextInt4(314159265));

            // it will go to Fallback
            rng = CreateNextSeed(Vector128.Create(6u, 3u, 12u, 9u).AsUInt64());
            AssertAreEqual(Vector128.Create(1, 2, 3, 4), rng.NextInt4(0x55555555));
        }

        public static void CharacteristicNextInt4Max1()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(168198401)[0], 0, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(168198401)[1], 0, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(168198401)[2], 0, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(168198401)[3], 0, 168198401 - 1);
        }

        public static void ReproductionNextInt4MinMax1()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<int>[] {
                Vector128.Create(-133252766, -206457661, -152368440, -120574856),
                Vector128.Create(209862572, 249958834, -89839087, -170200603),
                Vector128.Create(-222475289, 293832980, 5136089, -169234372),
                Vector128.Create(95201689, -223575560, -49157040, 83862094),
                Vector128.Create(122753179, -22796369, -248837502, -25549097),
                Vector128.Create(13987387, -84096383, -135534084, -270570179),
                Vector128.Create(-110963143, 115059334, 313666846, 213908513),
                Vector128.Create(7983873, -57648514, 188535925, 147867534),
                Vector128.Create(211663996, 104516028, 68308920, 307257913),
                Vector128.Create(-236679667, 178086285, 102591050, 305884008),
                Vector128.Create(-179087028, 118977032, -37337369, -17928901),
                Vector128.Create(210299795, 27667881, -249993402, -218256678),
                Vector128.Create(277320034, 57806585, -42366054, 309108916),
                Vector128.Create(-63416164, -201472898, -56287073, -194230683),
                Vector128.Create(-49348718, -18321203, 71323809, -12237019),
                Vector128.Create(7143253, 46860176, -152670434, -239331062),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextInt4(-271828182, 314159265)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Vector128.Create(-1, -1, -1, -1), rng.NextInt4(-1, 0));
            AssertThrows<ArgumentException>(() => rng.NextInt4(0, -1));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(-271828182, -271828182, -271828182, -271828182), rng.NextInt4(-271828182, 314159265));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(314159265 - 1, 314159265 - 1, 314159265 - 1, 314159265 - 1), rng.NextInt4(-271828182, 314159265));
        }

        public static void CharacteristicNextInt4MinMax1()
        {
            var rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(-1314, 168198401)[0], -1314, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(-1314, 168198401)[1], -1314, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(-1314, 168198401)[2], -1314, 168198401 - 1);
            rng = CreateStandardSeed();
            EasyDiscreteUniformStatTest(() => rng.NextInt4(-1314, 168198401)[3], -1314, 168198401 - 1);
        }


        public static void ReproductionNextDouble2()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<double>[] {
                Vector128.Create(0.23648188615021204, 0.20386058284391573),
                Vector128.Create(0.8220154843038042, 0.31056824912629777),
                Vector128.Create(0.3006721546374138, 0.22560203079918761),
                Vector128.Create(0.0842217586388142, 0.47264540013276934),
                Vector128.Create(0.6263442558893804, 0.3799930241906533),
                Vector128.Create(0.6733614579832595, 0.03923408347654167),
                Vector128.Create(0.12379707898477554, 0.7545761394600417),
                Vector128.Create(0.48775032752202085, 0.23258876800587092),
                Vector128.Create(0.4324363864438201, 0.8602369629345543),
                Vector128.Create(0.274519599196555, 0.9991596773799788),
                Vector128.Create(0.7879706203213694, 0.18311775133987684),
                Vector128.Create(0.4775052048185612, 0.7856211088868369),
                Vector128.Create(0.8250896513094751, 0.5804511762662378),
                Vector128.Create(0.05998168694036743, 0.6389543577064476),
                Vector128.Create(0.15826474611342822, 0.40016354327865045),
                Vector128.Create(0.822761614227457, 0.03726151517858278),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextDouble2()).ToArray();
            AssertAreEqual(expected, actual);

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<double>.Zero, rng.NextDouble2());

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(0.9999999999999999, 0.9999999999999999), rng.NextDouble2());

            rng = CreateNextSeed(Vector128.Create(0x7fful, 0x800ul));
            AssertAreEqual(Vector128.Create(0.0, 1.1102230246251565E-16), rng.NextDouble2());
        }

        public static void CharacteristicNextDouble2()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextDouble2()[0], 0, 1);
            EasyContinuousUniformStatTest(() => rng.NextDouble2()[1], 0, 1);
        }

        private static void EasyContinuousUniformStatTest(Func<double> next, double theoreticalMin, double theoreticalMax)
        {
            double theoreticalAverage = theoreticalMin * 0.5 + theoreticalMax * 0.5;
            double theoreticalVariance = (double)(theoreticalMax - theoreticalMin) * (theoreticalMax - theoreticalMin) / 12.0;
            double range = theoreticalMax - theoreticalMin;
            double min = theoreticalMax;
            double max = theoreticalMin;
            double average = 0;
            double variance = 0;

            for (int i = 0; i < 1 << 24; i++)
            {
                var value = next();

                min = Math.Min(value, min);
                max = Math.Max(value, max);
                average = average * (i / (i + 1.0)) + value / (i + 1.0);
                variance = variance * (i / (i + 1.0)) + (theoreticalAverage - value) * (theoreticalAverage - value) / (i + 1.0);
            }

            Assert(min >= theoreticalMin, theoreticalMin, min);
            Assert(max < theoreticalMax, theoreticalMax, max);
            Assert(min < theoreticalMin + range * 0.01, theoreticalMin, min);
            Assert(max > theoreticalMax - range * 0.01, theoreticalMax, max);
            Assert(theoreticalAverage - range * 0.1 < average && average < theoreticalAverage + range * 0.1, theoreticalAverage, average);
            Assert(theoreticalVariance - range * range * 0.1 < variance && variance < theoreticalVariance + range * range * 0.1, theoreticalVariance, variance);

            ShouldBe(min < theoreticalMin + range * 0.000001, theoreticalMin, min);
            ShouldBe(max > theoreticalMax - range * 0.000001, theoreticalMax, max);
            ShouldBe(theoreticalAverage - range * 0.001 < average && average < theoreticalAverage + range * 0.001, theoreticalAverage, average);
            ShouldBe(theoreticalVariance - range * range * 0.001 < variance && variance < theoreticalVariance + range * range * 0.001, theoreticalVariance, variance);
        }

        public static void ReproductionNextSignedDouble2()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<double>[] {
                Vector128.Create(0.4729637723004241, 0.40772116568783157),
                Vector128.Create(-0.35596903139239156, 0.6211364982525955),
                Vector128.Create(0.6013443092748276, 0.45120406159837534),
                Vector128.Create(0.16844351727762852, 0.9452908002655387),
                Vector128.Create(-0.7473114882212393, 0.7599860483813067),
                Vector128.Create(-0.6532770840334808, 0.07846816695308334),
                Vector128.Create(0.2475941579695511, -0.4908477210799167),
                Vector128.Create(0.9755006550440417, 0.46517753601174183),
                Vector128.Create(0.8648727728876403, -0.27952607413089126),
                Vector128.Create(0.54903919839311, -0.001680645240042411),
                Vector128.Create(-0.4240587593572611, 0.3662355026797537),
                Vector128.Create(0.9550104096371225, -0.4287577822263262),
                Vector128.Create(-0.3498206973810497, -0.8390976474675245),
                Vector128.Create(0.11996337388073486, -0.7220912845871048),
                Vector128.Create(0.31652949222685645, 0.8003270865573009),
                Vector128.Create(-0.35447677154508594, 0.07452303035716568),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextSignedDouble2()).ToArray();
            AssertAreEqual(expected, actual);

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(0.0, 0.0), rng.NextSignedDouble2());

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(-1.1102230246251565E-16, -1.1102230246251565E-16), rng.NextSignedDouble2());

            rng = CreateNextSeed(Vector128.Create(0x8000000000000000ul, 0x400ul));
            AssertAreEqual(Vector128.Create(-1.0, 1.1102230246251565E-16), rng.NextSignedDouble2());
        }

        public static void CharacteristicNextSignedDouble2()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextSignedDouble2()[0], -1, 1);
            EasyContinuousUniformStatTest(() => rng.NextSignedDouble2()[1], -1, 1);
        }

        public static void ReproductionNextDouble2Max()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<double>[] {
                Vector128.Create(0.6428244138818421, 0.6404469094189791),
                Vector128.Create(2.2344697536949925, 0.9756789298934218),
                Vector128.Create(0.81731165427451, 0.708749682593666),
                Vector128.Create(0.22893847606875226, 1.4848593168101163),
                Vector128.Create(1.702580209143805, 1.1937832932127248),
                Vector128.Create(1.8303862152205832, 0.123257508420232),
                Vector128.Create(0.3365153502206245, 2.370570856301814),
                Vector128.Create(1.325842852128057, 0.7306991648747447),
                Vector128.Create(1.1754839712347296, 2.702514123101591),
                Vector128.Create(0.7462216380518557, 3.1389527022200894),
                Vector128.Create(2.1419262185791803, 0.5752813823512396),
                Vector128.Create(1.2979937212529093, 2.4681015041839536),
                Vector128.Create(2.242826206004156, 1.8235411511255666),
                Vector128.Create(0.16304712965032, 2.0073343161497608),
                Vector128.Create(0.43020818344581624, 1.2571508477986695),
                Vector128.Create(2.2364979451081273, 0.11706050234666024),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextDouble2(Vector128.Create(Math.E, Math.PI))).ToArray();
            AssertAreEqual(expected, actual);



            rng = CreateStandardSeed();
            AssertThrows<ArgumentException>(() => rng.NextDouble2(Vector128.Create(double.NegativeInfinity, 1.0)));
            AssertThrows<ArgumentException>(() => rng.NextDouble2(Vector128.Create(double.MinValue, 1.0)));
            AssertThrows<ArgumentException>(() => rng.NextDouble2(Vector128.Create(-1.0, 1.0)));
            AssertThrows<ArgumentException>(() => rng.NextDouble2(Vector128.Create(-double.Epsilon, 1.0)));
            AssertThrows<ArgumentException>(() => rng.NextDouble2(Vector128.Create(-0.0, 0.0)));
            AssertThrows<ArgumentException>(() => rng.NextDouble2(Vector128.Create(double.NaN, double.NaN)));
            AssertThrows<ArgumentException>(() => rng.NextDouble2(Vector128.Create(double.PositiveInfinity, 1.0)));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(0.0, 0.0), rng.NextDouble2(Vector128.Create(double.Epsilon, +1.0)));
            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(0.0, 0.0), rng.NextDouble2(Vector128.Create(double.MaxValue, double.MaxValue)));

            rng = CreateNextSeed(Vector128.Create(1ul << 63));
            AssertAreEqual(Vector128.Create(0.0, 0.5), rng.NextDouble2(Vector128.Create(double.Epsilon, +1.0)));
            rng = CreateNextSeed(Vector128.Create(1ul << 63));
            AssertAreEqual(Vector128.Create(double.MaxValue / 2), rng.NextDouble2(Vector128.Create(double.MaxValue, double.MaxValue)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(0.0, (~0ul >> 11) * (1.0 / (1ul << 53))), rng.NextDouble2(Vector128.Create(double.Epsilon, +1.0)));
            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(double.MaxValue * ((~0ul >> 11) * (1.0 / (1ul << 53)))), rng.NextDouble2(Vector128.Create(double.MaxValue, double.MaxValue)));
        }

        public static void CharacteristicNextDouble2Max()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextDouble2(Vector128.Create(Math.PI, 299792458.0))[0], 0, Math.PI);
            EasyContinuousUniformStatTest(() => rng.NextDouble2(Vector128.Create(Math.PI, 299792458.0))[1], 0, 299792458.0);
        }

        public static void ReproductionNextDouble2MinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<double>[] {
                Vector128.Create(-0.12069369996794585, -1.860698834751835),
                Vector128.Create(2.0564852379987966, -1.1902347938029496),
                Vector128.Create(0.11798380891192378, -1.7240932884024611),
                Vector128.Create(-0.6868397652924335, -0.1718740199695603),
                Vector128.Create(1.3289244650331853, -0.7540260671643433),
                Vector128.Create(1.5037476732038426, -2.895077636749329),
                Vector128.Create(-0.5396875707946, 1.599549059013835),
                Vector128.Create(0.8135931796500778, -1.6801943238403036),
                Vector128.Create(0.6079203576785496, 2.263435592613389),
                Vector128.Create(0.02074123724841072, 3.136312750850385),
                Vector128.Create(1.9298968389005495, -1.991029888887314),
                Vector128.Create(0.7754989260714705, 1.7946103547781143),
                Vector128.Create(2.067915857313631, 0.5054896486613402),
                Vector128.Create(-0.7769711834093126, 0.8730759787097281),
                Vector128.Create(-0.41152707044075554, -0.6272909579924542),
                Vector128.Create(2.059259559335584, -2.9074716488964727),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextDouble2(Vector128.Create(-1.0, -Math.PI), Vector128.Create(Math.E, Math.PI))).ToArray();
            AssertAreEqual(expected, actual);

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(-1.0, -Math.PI), rng.NextDouble2(Vector128.Create(-1.0, -Math.PI), Vector128.Create(Math.E, Math.PI)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(2.7182818284590446, 3.1415926535897922), rng.NextDouble2(Vector128.Create(-1.0, -Math.PI), Vector128.Create(Math.E, Math.PI)));


            {
                const double eps = double.Epsilon;
                const double h01 = 1.0 * ((~0ul >> 11) * (1.0 / (1ul << 53)));
                const double h02 = double.MaxValue * (1.0 / (1ul << 53));
                const double h50 = double.MaxValue / 2;
                const double h98 = double.MaxValue * ((~0ul >> 11) * (1.0 / (1ul << 53)));
                const double h99 = double.MinValue * (1ul * (1.0 / (1ul << 53))) + double.MaxValue * ((~0ul >> 11) * (1.0 / (1ul << 53)));
                const double r01 = 1.1102230246251565E-16;
                const double r10 = 0.99999999999999978;
                const double r90 = 9.607670647659132E+307;
                const double r91 = 1.2371388209149099E+308;
                const double r92 = 1.7395420178859149E+308;
                const double max = double.MaxValue;
                const double inf = double.PositiveInfinity;
                double qnan = BitConverter.UInt64BitsToDouble(0xfff8ul << 48);
                double snan = BitConverter.UInt64BitsToDouble(0xfff4ul << 48);
                static bool StrictCompare(double x, double y) => BitConverter.DoubleToUInt64Bits(x) == BitConverter.DoubleToUInt64Bits(y);

                var candidate = new double[] { -inf, -max, -1.0, -eps, -0.0, +0.0, +eps, +1.0, +max, +inf, qnan };
                var expectedt0 = new double[] {
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -max, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -max, -1.0, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -max, -1.0, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -max, -1.0, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -max, -1.0, -eps, +0.0, +0.0, snan, snan, snan, snan, snan,
                    snan, -max, -1.0, -eps, +0.0, +0.0, +eps, snan, snan, snan, snan,
                    snan, -r90, -1.0, -eps, +0.0, +0.0, +eps, +1.0, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                };
                var expectedt5 = new double[] {
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h50, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h50, -0.5, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h50, -0.5, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h50, -0.5, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h50, -0.5, +0.0, +0.0, +0.0, snan, snan, snan, snan, snan,
                    snan, -h50, +0.0, +0.5, +0.5, +0.5, +0.5, snan, snan, snan, snan,
                    snan, -r92, +h50, +h50, +h50, +h50, +h50, +h50, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                };
                var expectedt1 = new double[] {
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h02, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h02, -r01, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h02, -r01, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h02, -r01, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h02, -r01, -eps, +0.0, +0.0, snan, snan, snan, snan, snan,
                    snan, -h02, +r10, +h01, +h01, +h01, +h01, snan, snan, snan, snan,
                    snan, +r91, +h98, +h98, +h98, +h98, +h98, +h98, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                };

                double[] CreateActual(ulong next)
                {
                    return candidate.SelectMany(c1 => candidate.Select(c2 => (min: c2, max: c1))).Select(pair =>
                    {
                        var rng = CreateNextSeed(Vector128.Create(next));
                        try
                        {
                            return rng.NextDouble2(Vector128.Create(pair.min), Vector128.Create(pair.max))[0];
                        }
                        catch (ArgumentException)
                        {
                            return snan;
                        }
                    }).ToArray();
                }

                var actualt0 = CreateActual(0);
                var actualt5 = CreateActual(0x8000000000000000ul);
                var actualt1 = CreateActual(~0ul);

                var equalityt0 = expectedt0.Zip(actualt0).Select(pair => StrictCompare(pair.First, pair.Second)).ToArray();
                var equalityt5 = expectedt5.Zip(actualt5).Select(pair => StrictCompare(pair.First, pair.Second)).ToArray();
                var equalityt1 = expectedt1.Zip(actualt1).Select(pair => StrictCompare(pair.First, pair.Second)).ToArray();

                /*
                void DebugPrint(double[] actual, bool[] equality)
                {
                    for (int i = 0; i < actual.Length; i++)
                    {
                        Console.Write((equality[i] ? "\u001b[37m" : "\u001b[91m") + actual[i] switch
                        {
                            +eps => "+eps",
                            +1.0 => "+1.0",
                            +h01 => "+h01",
                            +h02 => "+h02",
                            +h50 => "+h50",
                            +h98 => "+h98",
                            +h99 => "+h99",
                            +max => "+max",
                            +inf => "+inf",
                            -eps => "-eps",
                            -1.0 => "-1.0",
                            -h01 => "-h01",
                            -h02 => "-h02",
                            -h50 => "-h50",
                            -h98 => "-h98",
                            -h99 => "-h99",
                            -max => "-max",
                            -inf => "-inf",
                            _ when StrictCompare(actual[i], qnan) => "qnan",
                            _ when StrictCompare(actual[i], snan) => "snan",
                            _ when StrictCompare(actual[i], +0.0) => "+0.0",
                            _ when StrictCompare(actual[i], -0.0) => "-0.0",
                            < 0 => actual[i].ToString("g1"),
                            _ => "+" + actual[i].ToString("g1"),
                        } + " ");
                        if (i % 11 == 10)
                        {
                            Console.WriteLine();
                        }
                    }
                    Console.WriteLine();
                }

                DebugPrint(actualt0, equalityt0);
                DebugPrint(actualt5, equalityt5);
                DebugPrint(actualt1, equalityt1);
                //*/

                Assert(equalityt0.All(b => b));
                Assert(equalityt5.All(b => b));
                Assert(equalityt1.All(b => b));
            }

        }

        public static void CharacteristicNextDouble2MinMax()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextDouble2(Vector128.Create(-Math.PI, Math.PI), Vector128.Create(Math.PI, 299792458.0))[0], -Math.PI, Math.PI);
            EasyContinuousUniformStatTest(() => rng.NextDouble2(Vector128.Create(-Math.PI, Math.PI), Vector128.Create(Math.PI, 299792458.0))[1], Math.PI, 299792458.0);
        }

        public static void ReproductionNextDouble2MinMax1()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<double>[] {
                Vector128.Create(-1.6557331411166651, -1.860698834751835),
                Vector128.Create(2.0232829596619815, -1.1902347938029496),
                Vector128.Create(-1.2524137892939662, -1.7240932884024611),
                Vector128.Create(-2.61241173716557, -0.1718740199695603),
                Vector128.Create(0.7938443722506926, -0.7540260671643433),
                Vector128.Create(1.0892621656316475, -2.895077636749329),
                Vector128.Create(-2.3637526658409005, 1.599549059013835),
                Vector128.Create(-0.07696696213140072, -1.6801943238403036),
                Vector128.Create(-0.4245147039961491, 2.263435592613389),
                Vector128.Create(-1.4167351413851699, 3.136312750850385),
                Vector128.Create(1.8093727705026197, -1.991029888887314),
                Vector128.Create(-0.14133896657203054, 1.7946103547781143),
                Vector128.Create(2.042598520623829, 0.5054896486613402),
                Vector128.Create(-2.764716599506231, 0.8730759787097281),
                Vector128.Create(-2.147185926165393, -0.6272909579924542),
                Vector128.Create(2.0279710322355236, -2.9074716488964727),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextDouble2(-Math.PI, Math.PI)).ToArray();
            AssertAreEqual(expected, actual);
        }

        public static void CharacteristicNextDouble2MinMax1()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextDouble2(-Math.E, Math.PI)[0], -Math.E, Math.PI);
            EasyContinuousUniformStatTest(() => rng.NextDouble2(-Math.E, Math.PI)[1], -Math.E, Math.PI);
        }

        public static void ReproductionNextDouble()
        {
            var rng = CreateStandardSeed();

            var expected = new double[] {
                0.23648188615021204,
                0.8220154843038042,
                0.3006721546374138,
                0.0842217586388142,
                0.6263442558893804,
                0.6733614579832595,
                0.12379707898477554,
                0.48775032752202085,
                0.4324363864438201,
                0.274519599196555,
                0.7879706203213694,
                0.4775052048185612,
                0.8250896513094751,
                0.05998168694036743,
                0.15826474611342822,
                0.822761614227457,
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextDouble()).ToArray();
            AssertAreEqual(expected, actual);
        }

        public static void CharacteristicNextDouble()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextDouble(), 0, 1);
        }

        public static void ReproductionNextSignedDouble()
        {
            var rng = CreateStandardSeed();

            var expected = new double[] {
                0.4729637723004241,
                -0.35596903139239156,
                0.6013443092748276,
                0.16844351727762852,
                -0.7473114882212393,
                -0.6532770840334808,
                0.2475941579695511,
                0.9755006550440417,
                0.8648727728876403,
                0.54903919839311,
                -0.4240587593572611,
                0.9550104096371225,
                -0.3498206973810497,
                0.11996337388073486,
                0.31652949222685645,
                -0.35447677154508594,
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextSignedDouble()).ToArray();
            AssertAreEqual(expected, actual);
        }

        public static void CharacteristicNextSignedDouble()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextSignedDouble(), -1, 1);
        }

        public static void ReproductionNextDoubleMax()
        {
            var rng = CreateStandardSeed();

            var expected = new double[] {
                0.742929756236564,
                2.582437806625887,
                0.9445894321479135,
                0.2645904582121114,
                1.9677185129202428,
                2.1154274096107204,
                0.3889199938744462,
                1.5323128457291961,
                1.358538974796822,
                0.8624287561023116,
                2.4754827120462064,
                1.5001268435088813,
                2.5920955871068108,
                0.18843802704178114,
                0.49720336371219986,
                2.5847818429126583
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextDouble(Math.PI)).ToArray();
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentException>(() => rng.NextDouble(double.NegativeInfinity));
            AssertThrows<ArgumentException>(() => rng.NextDouble(-1.0));
            AssertThrows<ArgumentException>(() => rng.NextDouble(-0.0));
            AssertThrows<ArgumentException>(() => rng.NextDouble(+0.0));
            AssertThrows<ArgumentException>(() => rng.NextDouble(double.PositiveInfinity));
            AssertThrows<ArgumentException>(() => rng.NextDouble(double.NaN));
        }

        public static void CharacteristicNextDoubleMax()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextDouble(Math.PI), 0, Math.PI);
        }

        public static void ReproductionNextDoubleMinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new double[] {
                -1.6557331411166651,
                2.0232829596619815,
                -1.2524137892939662,
                -2.61241173716557,
                0.7938443722506926,
                1.0892621656316475,
                -2.3637526658409005,
                -0.07696696213140072,
                -0.4245147039961491,
                -1.4167351413851699,
                1.8093727705026197,
                -0.14133896657203054,
                2.042598520623829,
                -2.764716599506231,
                -2.147185926165393,
                2.0279710322355236,
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextDouble(-Math.PI, Math.PI)).ToArray();
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentException>(() => rng.NextDouble(double.NegativeInfinity, 1.0));
            AssertThrows<ArgumentException>(() => rng.NextDouble(+1.0, -1.0));
            AssertThrows<ArgumentException>(() => rng.NextDouble(-0.0, +0.0));
            AssertThrows<ArgumentException>(() => rng.NextDouble(1.0, double.PositiveInfinity));
            AssertThrows<ArgumentException>(() => rng.NextDouble(double.NaN, double.NaN));
        }

        public static void CharacteristicNextDoubleMinMax()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextDouble(-Math.E, Math.PI), -Math.E, Math.PI);
        }


        public static void ReproductionNextFloat4()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<float>[] {
                Vector128.Create(0.11155617f, 0.23648185f, 0.25811696f, 0.20386058f),
                Vector128.Create(0.8904406f, 0.82201546f, 0.17342961f, 0.3105682f),
                Vector128.Create(0.98554707f, 0.3006721f, 0.19369578f, 0.22560203f),
                Vector128.Create(0.9653127f, 0.08422172f, 0.17507851f, 0.47264534f),
                Vector128.Create(0.082344115f, 0.6263442f, 0.60699296f, 0.37999302f),
                Vector128.Create(0.42497802f, 0.6733614f, 0.42028046f, 0.039234042f),
                Vector128.Create(0.57993996f, 0.12379706f, 0.32281405f, 0.7545761f),
                Vector128.Create(0.3203683f, 0.4877503f, 0.0021467805f, 0.23258877f),
                Vector128.Create(0.37662542f, 0.43243635f, 0.6142752f, 0.86023694f),
                Vector128.Create(0.6602317f, 0.27451956f, 0.82891995f, 0.99915963f),
                Vector128.Create(0.48911482f, 0.7879706f, 0.32183135f, 0.18311775f),
                Vector128.Create(0.36550212f, 0.47750515f, 0.7162196f, 0.7856211f),
                Vector128.Create(0.64223933f, 0.82508963f, 0.98822266f, 0.58045113f),
                Vector128.Create(0.7677885f, 0.059981644f, 0.98587805f, 0.63895434f),
                Vector128.Create(0.6669173f, 0.1582647f, 0.43328446f, 0.40016353f),
                Vector128.Create(0.51109636f, 0.8227616f, 0.09142089f, 0.037261486f),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextFloat4()).ToArray();
            AssertAreEqual(expected, actual);

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<float>.Zero, rng.NextFloat4());

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(0.99999994f), rng.NextFloat4());

            rng = CreateNextSeed(Vector128.Create(0xfful, 0x100ul));
            AssertAreEqual(Vector128.Create(0f, 0f, 5.9604645E-08f, 0f), rng.NextFloat4());
        }

        public static void CharacteristicNextFloat4()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextFloat4()[0], 0, 1);
            EasyContinuousUniformStatTest(() => rng.NextFloat4()[1], 0, 1);
            EasyContinuousUniformStatTest(() => rng.NextFloat4()[2], 0, 1);
            EasyContinuousUniformStatTest(() => rng.NextFloat4()[3], 0, 1);
        }

        public static void ReproductionNextSignedFloat4()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<float>[] {
                Vector128.Create(0.22311234f, 0.47296375f, 0.516234f, 0.40772116f),
                Vector128.Create(-0.21911883f, -0.35596907f, 0.34685922f, 0.6211365f),
                Vector128.Create(-0.028905809f, 0.6013443f, 0.38739163f, 0.45120406f),
                Vector128.Create(-0.0693745f, 0.1684435f, 0.35015702f, 0.94529074f),
                Vector128.Create(0.16468823f, -0.74731153f, -0.786014f, 0.75998604f),
                Vector128.Create(0.8499561f, -0.6532771f, 0.8405609f, 0.078468144f),
                Vector128.Create(-0.84012f, 0.24759412f, 0.64562815f, -0.49084777f),
                Vector128.Create(0.6407366f, 0.97550064f, 0.0042936206f, 0.46517754f),
                Vector128.Create(0.75325084f, 0.86487275f, -0.7714495f, -0.2795261f),
                Vector128.Create(-0.6795365f, 0.5490392f, -0.3421601f, -0.0016806722f),
                Vector128.Create(0.9782297f, -0.4240588f, 0.64366275f, 0.3662355f),
                Vector128.Create(0.73100424f, 0.95501035f, -0.56756073f, -0.4287578f),
                Vector128.Create(-0.7155213f, -0.34982073f, -0.023554623f, -0.8390977f),
                Vector128.Create(-0.46442288f, 0.11996335f, -0.02824384f, -0.7220913f),
                Vector128.Create(-0.6661653f, 0.31652945f, 0.866569f, 0.80032706f),
                Vector128.Create(-0.9778073f, -0.3544768f, 0.18284178f, 0.07452297f),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextSignedFloat4()).ToArray();
            AssertAreEqual(expected, actual);

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128<float>.Zero, rng.NextSignedFloat4());

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(-5.9604645E-08f), rng.NextSignedFloat4());

            rng = CreateNextSeed(Vector128.Create(0x00000080_0000007ful, 0x00000100_000000fful));
            AssertAreEqual(Vector128.Create(0f, 5.9604645E-08f, 5.9604645E-08f, 1.1920929E-07f), rng.NextSignedFloat4());
        }

        public static void CharacteristicNextSignedFloat4()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextSignedFloat4()[0], -1, 1);
            EasyContinuousUniformStatTest(() => rng.NextSignedFloat4()[1], -1, 1);
            EasyContinuousUniformStatTest(() => rng.NextSignedFloat4()[2], -1, 1);
            EasyContinuousUniformStatTest(() => rng.NextSignedFloat4()[3], -1, 1);
        }

        public static void ReproductionNextFloat4Max()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<float>[] {
                Vector128.Create(0.3032411f, 0.74292964f, 0.70163465f, 0.6404469f),
                Vector128.Create(2.4204683f, 2.5824378f, 0.47143054f, 0.97567886f),
                Vector128.Create(2.6789947f, 0.9445893f, 0.5265197f, 0.7087497f),
                Vector128.Create(2.623992f, 0.26459035f, 0.47591272f, 1.4848592f),
                Vector128.Create(0.2238345f, 1.9677184f, 1.6499779f, 1.1937833f),
                Vector128.Create(1.15521f, 2.1154273f, 1.1424407f, 0.12325738f),
                Vector128.Create(1.5764402f, 0.38891995f, 0.8774995f, 2.3705707f),
                Vector128.Create(0.8708513f, 1.5323128f, 0.005835554f, 0.7306992f),
                Vector128.Create(1.023774f, 1.3585389f, 1.6697731f, 2.7025142f),
                Vector128.Create(1.7946959f, 0.86242867f, 2.253238f, 3.1389527f),
                Vector128.Create(1.3295519f, 2.4754827f, 0.8748283f, 0.5752814f),
                Vector128.Create(0.9935377f, 1.5001267f, 1.9468867f, 2.4681015f),
                Vector128.Create(1.7457875f, 2.5920956f, 2.6862676f, 1.823541f),
                Vector128.Create(2.0870655f, 0.1884379f, 2.6798942f, 2.0073342f),
                Vector128.Create(1.8128692f, 0.49720323f, 1.1777892f, 1.2571509f),
                Vector128.Create(1.3893039f, 2.584782f, 0.24850774f, 0.117060415f),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextFloat4(Vector128.Create(MathF.E, MathF.PI, MathF.E, MathF.PI))).ToArray();
            AssertAreEqual(expected, actual);



            rng = CreateStandardSeed();
            AssertThrows<ArgumentException>(() => rng.NextFloat4(Vector128.Create(float.NegativeInfinity, 1.0f, 1.0f, 1.0f)));
            AssertThrows<ArgumentException>(() => rng.NextFloat4(Vector128.Create(float.MinValue, 1.0f, 1.0f, 1.0f)));
            AssertThrows<ArgumentException>(() => rng.NextFloat4(Vector128.Create(-1.0f, 1.0f, 1.0f, 1.0f)));
            AssertThrows<ArgumentException>(() => rng.NextFloat4(Vector128.Create(-float.Epsilon, 1.0f, 1.0f, 1.0f)));
            AssertThrows<ArgumentException>(() => rng.NextFloat4(Vector128.Create(-0.0f, 1.0f, 1.0f, 1.0f)));
            AssertThrows<ArgumentException>(() => rng.NextFloat4(Vector128.Create(float.NaN, float.NaN, float.NaN, float.NaN)));
            AssertThrows<ArgumentException>(() => rng.NextFloat4(Vector128.Create(float.PositiveInfinity, 1.0f, 1.0f, 1.0f)));

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(0f), rng.NextFloat4(Vector128.Create(float.Epsilon, +1.0f, MathF.PI, float.MaxValue)));

            rng = CreateNextSeed(Vector128.Create(0x8000000080000000ul));
            AssertAreEqual(Vector128.Create(0f, 0.5f, MathF.PI / 2, float.MaxValue / 2), rng.NextFloat4(Vector128.Create(float.Epsilon, +1.0f, MathF.PI, float.MaxValue)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(0, 0.99999994f, 3.1415925f, 3.4028233E+38f), rng.NextFloat4(Vector128.Create(float.Epsilon, +1.0f, MathF.PI, float.MaxValue)));
        }

        public static void CharacteristicNextFloat4Max()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextFloat4(Vector128.Create(MathF.E, MathF.PI, 100, 299792458.0f))[0], 0, MathF.E);
            EasyContinuousUniformStatTest(() => rng.NextFloat4(Vector128.Create(MathF.E, MathF.PI, 100, 299792458.0f))[1], 0, MathF.PI);
            EasyContinuousUniformStatTest(() => rng.NextFloat4(Vector128.Create(MathF.E, MathF.PI, 100, 299792458.0f))[2], 0, 100);
            EasyContinuousUniformStatTest(() => rng.NextFloat4(Vector128.Create(MathF.E, MathF.PI, 100, 299792458.0f))[3], 0, 299792458.0f);
        }

        public static void ReproductionNextFloat4MinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<float>[] {
                Vector128.Create(2.0801287f, 1.5064478f, 0.8799137f, 0.44632697f),
                Vector128.Create(2.6395872f, 2.7604225f, 0.26312f, 1.7268186f),
                Vector128.Create(2.7079005f, 1.6439172f, 0.41072232f, 0.70722437f),
                Vector128.Create(2.6933665f, 1.1803687f, 0.27512926f, 3.671744f),
                Vector128.Create(2.0591462f, 2.3413742f, 3.4208424f, 2.5599163f),
                Vector128.Create(2.305254f, 2.442066f, 2.0609806f, -1.5291915f),
                Vector128.Create(2.4165602f, 1.2651229f, 1.3511146f, 7.054913f),
                Vector128.Create(2.2301147f, 2.0445626f, -0.9843646f, 0.7910652f),
                Vector128.Create(2.270523f, 1.9261025f, 3.4738803f, 8.322844f),
                Vector128.Create(2.4742324f, 1.5879091f, 5.0371776f, 9.989916f),
                Vector128.Create(2.3513222f, 2.6875122f, 1.3439574f, 0.19741297f),
                Vector128.Create(2.2625334f, 2.0226216f, 4.21636f, 7.427453f),
                Vector128.Create(2.4613087f, 2.767006f, 6.197409f, 4.9654136f),
                Vector128.Create(2.5514884f, 1.1284562f, 6.1803327f, 5.667452f),
                Vector128.Create(2.4790344f, 1.3389385f, 2.1556911f, 2.8019624f),
                Vector128.Create(2.3671112f, 2.7620203f, -0.3341647f, -1.5528622f),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextFloat4(Vector128.Create(2f, 1f, -1f, -2f), Vector128.Create(MathF.E, MathF.PI, MathF.Tau, 10f))).ToArray();
            AssertAreEqual(expected, actual);

            rng = CreateZeroSeed();
            AssertAreEqual(Vector128.Create(2f, 1f, -1f, -2f), rng.NextFloat4(Vector128.Create(2f, 1f, -1f, -2f), Vector128.Create(MathF.E, MathF.PI, MathF.Tau, 10f)));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(Vector128.Create(2.3064668f, 3.1415925f, 6.283185f, 9.999999f), rng.NextFloat4(Vector128.Create(2f, 1f, -1f, -2f), Vector128.Create(MathF.E, MathF.PI, MathF.Tau, 10f)));


            {
                const float eps = float.Epsilon;
                const float h01 = 1.0f * ((~0u >> 8) * (1f / (1u << 24)));
                const float h02 = float.MaxValue * (1f / (1u << 24));
                const float h50 = float.MaxValue / 2;
                const float h98 = float.MaxValue * ((~0u >> 8) * (1f / (1u << 24)));
                const float h99 = float.MinValue * (1u * (1f / (1u << 24))) + float.MaxValue * ((~0u >> 8) * (1f / (1u << 24)));
                const float r01 = 5.96046448E-08f;
                const float r10 = 0.9999999f;
                const float r90 = 1.18932359E+38f;
                const float r91 = 1.1892599E+38f;
                const float r92 = 3.1798346E+38f;
                const float max = float.MaxValue;
                const float inf = float.PositiveInfinity;
                float qnan = BitConverter.UInt32BitsToSingle(0xfff8u << 16);
                float snan = BitConverter.UInt32BitsToSingle(0xfff4u << 16);
                static bool StrictCompare(float x, float y) => BitConverter.SingleToUInt32Bits(x) == BitConverter.SingleToUInt32Bits(y);

                var candidate = new float[] { -inf, -max, -1f, -eps, -0f, +0f, +eps, +1f, +max, +inf, qnan };
                var expectedt0 = new float[] {
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -max, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -max,  -1f, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -max,  -1f, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -max,  -1f, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -max,  -1f, -eps,  +0f,  +0f, snan, snan, snan, snan, snan,
                    snan, -max,  -1f, -eps,  +0f,  +0f, +eps, snan, snan, snan, snan,
                    snan, -r90,  -1f, -eps,  +0f,  +0f, +eps,  +1f, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                };
                var expectedt5 = new float[] {
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h50, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h50, -.5f, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h50, -.5f, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h50, -.5f, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h50, -.5f,  +0f,  +0f,  +0f, snan, snan, snan, snan, snan,
                    snan, -h50,  +0f, 0.5f, 0.5f, 0.5f, 0.5f, snan, snan, snan, snan,
                    snan, +r92, +h50, +h50, +h50, +h50, +h50, +h50, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                };
                var expectedt1 = new float[] {
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h02, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h02, -r01, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h02, -r01, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h02, -r01, -eps, snan, snan, snan, snan, snan, snan, snan,
                    snan, -h02, -r01, -eps, 0.0f, 0.0f, snan, snan, snan, snan, snan,
                    snan, -h02, +r10, +h01, +h01, +h01, +h01, snan, snan, snan, snan,
                    snan, -r91, +h98, +h98, +h98, +h98, +h98, +h98, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                    snan, snan, snan, snan, snan, snan, snan, snan, snan, snan, snan,
                };

                float[] CreateActual(ulong next)
                {
                    return candidate.SelectMany(c1 => candidate.Select(c2 => (min: c2, max: c1))).Select(pair =>
                    {
                        var rng = CreateNextSeed(Vector128.Create(next));
                        try
                        {
                            return rng.NextFloat4(Vector128.Create(pair.min), Vector128.Create(pair.max))[0];
                        }
                        catch (ArgumentException)
                        {
                            return snan;
                        }
                    }).ToArray();
                }

                var actualt0 = CreateActual(0);
                var actualt5 = CreateActual(0x8000000080000000ul);
                var actualt1 = CreateActual(~0ul);

                var equalityt0 = expectedt0.Zip(actualt0).Select(pair => StrictCompare(pair.First, pair.Second)).ToArray();
                var equalityt5 = expectedt5.Zip(actualt5).Select(pair => StrictCompare(pair.First, pair.Second)).ToArray();
                var equalityt1 = expectedt1.Zip(actualt1).Select(pair => StrictCompare(pair.First, pair.Second)).ToArray();

                /*
                void DebugPrint(float[] actual, bool[] equality)
                {
                    for (int i = 0; i < actual.Length; i++)
                    {
                        Console.Write((equality[i] ? "\u001b[37m" : "\u001b[91m") + actual[i] switch
                        {
                            +eps => "+eps",
                            +1f => "+1.0",
                            +h01 => "+h01",
                            +h02 => "+h02",
                            +h50 => "+h50",
                            +h98 => "+h98",
                            +h99 => "+h99",
                            +max => "+max",
                            +inf => "+inf",
                            -eps => "-eps",
                            -1f => "-1.0",
                            -h01 => "-h01",
                            -h02 => "-h02",
                            -h50 => "-h50",
                            -h98 => "-h98",
                            -h99 => "-h99",
                            -max => "-max",
                            -inf => "-inf",
                            _ when StrictCompare(actual[i], qnan) => "qnan",
                            _ when StrictCompare(actual[i], snan) => "snan",
                            _ when StrictCompare(actual[i], +0.0f) => "+0.0",
                            _ when StrictCompare(actual[i], -0.0f) => "-0.0",
                            < 0 => actual[i].ToString("g1"),
                            _ => "+" + actual[i].ToString("g1"),
                        } + " ");
                        if (i % 11 == 10)
                        {
                            Console.WriteLine();
                        }
                    }
                    Console.WriteLine();
                }

                DebugPrint(actualt0, equalityt0);
                DebugPrint(actualt5, equalityt5);
                DebugPrint(actualt1, equalityt1);
                //*/

                Assert(equalityt0.All(b => b));
                Assert(equalityt5.All(b => b));
                Assert(equalityt1.All(b => b));
            }

        }

        public static void CharacteristicNextFloat4MinMax()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextFloat4(Vector128.Create(2f, 1f, -1f, -2f), Vector128.Create(MathF.E, MathF.PI, MathF.Tau, 10f))[0], 2f, MathF.E);
            EasyContinuousUniformStatTest(() => rng.NextFloat4(Vector128.Create(2f, 1f, -1f, -2f), Vector128.Create(MathF.E, MathF.PI, MathF.Tau, 10f))[1], 1f, MathF.PI);
            EasyContinuousUniformStatTest(() => rng.NextFloat4(Vector128.Create(2f, 1f, -1f, -2f), Vector128.Create(MathF.E, MathF.PI, MathF.Tau, 10f))[2], -1f, MathF.Tau);
            EasyContinuousUniformStatTest(() => rng.NextFloat4(Vector128.Create(2f, 1f, -1f, -2f), Vector128.Create(MathF.E, MathF.PI, MathF.Tau, 10f))[3], -2f, 10f);
        }

        public static void ReproductionNextFloat4MinMax1()
        {
            var rng = CreateStandardSeed();

            var expected = new Vector128<float>[] {
                Vector128.Create(-2.4406645f, -1.6557335f, -1.519796f, -1.8606989f),
                Vector128.Create(2.4532106f, 2.023283f, -2.0519023f, -1.190235f),
                Vector128.Create(3.0507822f, -1.2524141f, -1.9245661f, -1.7240933f),
                Vector128.Create(2.9236462f, -2.612412f, -2.041542f, -0.17187439f),
                Vector128.Create(-2.6242094f, 0.79384404f, 0.6722566f, -0.7540261f),
                Vector128.Create(-0.47137702f, 1.089262f, -0.5008927f, -2.895078f),
                Vector128.Create(0.5022776f, -2.3637528f, -1.1132922f, 1.5995488f),
                Vector128.Create(-1.1286594f, -0.07696719f, -3.1281042f, -1.6801944f),
                Vector128.Create(-0.7751854f, -0.42451495f, 0.7180124f, 2.2634356f),
                Vector128.Create(1.0067656f, -1.4167354f, 2.066665f, 3.1363125f),
                Vector128.Create(-0.068393596f, 1.8093727f, -1.1194667f, -1.99103f),
                Vector128.Create(-0.84507513f, -0.14133933f, 1.3585479f, 1.7946104f),
                Vector128.Create(0.8937161f, 2.0425985f, 3.0675936f, 0.50548935f),
                Vector128.Create(1.682565f, -2.7647169f, 3.052862f, 0.8730759f),
                Vector128.Create(1.0487725f, -2.1471863f, -0.41918612f, -0.627291f),
                Vector128.Create(0.06972048f, 2.027971f, -2.5671782f, -2.907472f),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextFloat4(-MathF.PI, MathF.PI)).ToArray();
            AssertAreEqual(expected, actual);
        }

        public static void CharacteristicNextFloat4MinMax1()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextFloat4(-MathF.E, MathF.PI)[0], -MathF.E, MathF.PI);
            EasyContinuousUniformStatTest(() => rng.NextFloat4(-MathF.E, MathF.PI)[1], -MathF.E, MathF.PI);
            EasyContinuousUniformStatTest(() => rng.NextFloat4(-MathF.E, MathF.PI)[2], -MathF.E, MathF.PI);
            EasyContinuousUniformStatTest(() => rng.NextFloat4(-MathF.E, MathF.PI)[3], -MathF.E, MathF.PI);
        }

        public static void ReproductionNextFloat()
        {
            var rng = CreateStandardSeed();

            var expected = new float[] {
                0.11155617f,
                0.8904406f,
                0.98554707f,
                0.9653127f,
                0.082344115f,
                0.42497802f,
                0.57993996f,
                0.3203683f,
                0.37662542f,
                0.6602317f,
                0.48911482f,
                0.36550212f,
                0.64223933f,
                0.7677885f,
                0.6669173f,
                0.51109636f,
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextFloat()).ToArray();
            AssertAreEqual(expected, actual);
        }

        public static void CharacteristicNextFloat()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextFloat(), 0, 1);
        }

        public static void ReproductionNextSignedFloat()
        {
            var rng = CreateStandardSeed();

            var expected = new float[] {
                0.22311234f,
                -0.21911883f,
                -0.028905809f,
                -0.0693745f,
                0.16468823f,
                0.8499561f,
                -0.84012f,
                0.6407366f,
                0.75325084f,
                -0.6795365f,
                0.9782297f,
                0.73100424f,
                -0.7155213f,
                -0.46442288f,
                -0.6661653f,
                -0.9778073f,
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextSignedFloat()).ToArray();
            AssertAreEqual(expected, actual);
        }

        public static void CharacteristicNextSignedFloat()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextSignedFloat(), -1, 1);
        }

        public static void ReproductionNextFloatMax()
        {
            var rng = CreateStandardSeed();

            var expected = new float[] {
                0.35046408f,
                2.7974017f,
                3.0961876f,
                3.0326195f,
                0.25869167f,
                1.3351078f,
                1.8219352f,
                1.0064667f,
                1.1832037f,
                2.0741792f,
                1.5365995f,
                1.1482588f,
                2.0176544f,
                2.4120789f,
                2.0951827f,
                1.6056566f,
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextFloat(MathF.PI)).ToArray();
            AssertAreEqual(expected, actual);


            rng = CreateStandardSeed();
            AssertThrows<ArgumentException>(() => rng.NextFloat(float.NegativeInfinity));
            AssertThrows<ArgumentException>(() => rng.NextFloat(float.MinValue));
            AssertThrows<ArgumentException>(() => rng.NextFloat(-1.0f));
            AssertThrows<ArgumentException>(() => rng.NextFloat(-float.Epsilon));
            AssertThrows<ArgumentException>(() => rng.NextFloat(-0.0f));
            AssertThrows<ArgumentException>(() => rng.NextFloat(float.NaN));
            AssertThrows<ArgumentException>(() => rng.NextFloat(float.PositiveInfinity));

            rng = CreateZeroSeed();
            AssertAreEqual(0f, rng.NextFloat(MathF.PI));

            rng = CreateNextSeed(Vector128.Create(0x8000000080000000ul));
            AssertAreEqual(MathF.PI / 2, rng.NextFloat(MathF.PI));

            rng = CreateAllBitsSetSeed();
            AssertAreEqual(3.1415925f, rng.NextFloat(MathF.PI));
        }

        public static void CharacteristicNextFloatMax()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextFloat(MathF.PI), 0, MathF.PI);
        }

        public static void ReproductionNextFloatMinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new float[] {
                -2.4406645f,
                2.4532106f,
                3.0507822f,
                2.9236462f,
                -2.6242094f,
                -0.47137702f,
                0.5022776f,
                -1.1286594f,
                -0.7751854f,
                1.0067656f,
                -0.068393596f,
                -0.84507513f,
                0.8937161f,
                1.682565f,
                1.0487725f,
                0.06972048f,
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextFloat(-MathF.PI, MathF.PI)).ToArray();
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentException>(() => rng.NextFloat(float.NegativeInfinity, 1.0f));
            AssertThrows<ArgumentException>(() => rng.NextFloat(+1.0f, -1.0f));
            AssertThrows<ArgumentException>(() => rng.NextFloat(-0.0f, +0.0f));
            AssertThrows<ArgumentException>(() => rng.NextFloat(1.0f, float.PositiveInfinity));
            AssertThrows<ArgumentException>(() => rng.NextFloat(float.NaN, float.NaN));
        }

        public static void CharacteristicNextFloatMinMax()
        {
            var rng = CreateStandardSeed();

            EasyContinuousUniformStatTest(() => rng.NextFloat(-MathF.E, MathF.PI), -MathF.E, MathF.PI);
        }


        public static void ReproductionNextBool()
        {
            var rng = CreateStandardSeed();

            var expected = new bool[] {
                true, false, false, true, false, false, true, false, false, false, false, false, false, true, true, false
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBool(0.25)).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual(false, rng.NextBool(0.0));
            AssertAreEqual(true, rng.NextBool(1.0));

            AssertAreEqual(false, rng.NextBool(double.NegativeInfinity));
            AssertAreEqual(true, rng.NextBool(double.PositiveInfinity));
            AssertAreEqual(false, rng.NextBool(double.NaN));
        }

        public static void CharacteristicNextBool()
        {
            var rng = CreateStandardSeed();

            EasyBinomialStatTest(() => rng.NextBool(0.2), 0.2);
        }

        private static void EasyBinomialStatTest(Func<bool> next, double theoreticalProbability)
        {
            const int unitTrial = 16;
            const int entireTrial = 1 << 20;

            double theoreticalExpected = unitTrial * theoreticalProbability;
            double theoreticalVariance = unitTrial * theoreticalProbability * (1 - theoreticalProbability);
            var theoreticalPmf = Enumerable.Range(0, unitTrial + 1).Select(i => Pmf(theoreticalProbability, unitTrial, i) * entireTrial).ToArray();
            static double Pmf(double probability, int n, int k)
            {
                double p = 1;
                for (int i = 1; i <= k; i++)
                {
                    p *= (n - i + 1.0) / i;
                }
                return p * Math.Pow(probability, k) * Math.Pow(1 - probability, n - k);
            }

            var bucket = new int[unitTrial + 1];

            for (int i = 0; i < entireTrial; i++)
            {
                int succeeded = 0;

                for (int t = 0; t < unitTrial; t++)
                {
                    if (next())
                        succeeded++;
                }

                bucket[succeeded]++;
            }

            double expected = bucket.Select((v, i) => (double)v * i).Sum() / entireTrial;
            double variance = bucket.Select((v, i) => v * (i - theoreticalExpected) * (i - theoreticalExpected)).Sum() / entireTrial;

            ShouldBe(theoreticalExpected * 0.99 < expected && expected < theoreticalExpected * 1.01, theoreticalExpected, expected);
            ShouldBe(theoreticalVariance * 0.99 < variance && variance < theoreticalVariance * 1.01, theoreticalVariance, variance);
            Assert(theoreticalExpected * 0.9 < expected && expected < theoreticalExpected * 1.1, theoreticalExpected, expected);
            Assert(theoreticalVariance * 0.9 < variance && variance < theoreticalVariance * 1.1, theoreticalVariance, variance);

            for (int i = 0; i < bucket.Length; i++)
            {
                ShouldBe(theoreticalPmf[i] - entireTrial * 0.001 < bucket[i] && bucket[i] < theoreticalPmf[i] + entireTrial * 0.001, bucket[i], theoreticalPmf[i]);
                Assert(theoreticalPmf[i] - entireTrial * 0.1 < bucket[i] && bucket[i] < theoreticalPmf[i] + entireTrial * 0.1, bucket[i], theoreticalPmf[i]);
            }
        }

        public static void ReproductionNextBoolInt()
        {
            var rng = CreateStandardSeed();

            var expected = new bool[] {
                true, false, true, true, false, false, true, false, false, true, false, false, false, true, true, false
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBool(2, 5)).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual(false, rng.NextBool(0, 5));
            AssertAreEqual(true, rng.NextBool(5, 5));

            AssertThrows<ArgumentException>(() => rng.NextBool(-3, -5));
            AssertAreEqual(false, rng.NextBool(-3, 5));
        }

        public static void CharacteristicNextBoolInt()
        {
            var rng = CreateStandardSeed();

            EasyBinomialStatTest(() => rng.NextBool(2, 5), 2.0 / 5);
        }

        public static void ReproductionNextBool2()
        {
            var rng = CreateStandardSeed();

            var expected = new (bool, bool)[] {
                (true, true),
                (false, true),
                (false, true),
                (true, true),
                (false, true),
                (false, true),
                (true, false),
                (false, true),
                (false, false),
                (false, false),
                (false, true),
                (false, false),
                (false, true),
                (true, true),
                (true, true),
                (false, true),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBool2(Vector128.Create(0.25, 0.75))).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual((false, true), rng.NextBool2(Vector128.Create(0.0, 1.0)));

            AssertAreEqual((false, true), rng.NextBool2(Vector128.Create(double.NegativeInfinity, double.PositiveInfinity)));
            AssertAreEqual((false, false), rng.NextBool2(Vector128.Create(double.NaN, double.NaN)));
        }

        public static void CharacteristicNextBool2()
        {
            var rng = CreateStandardSeed();

            EasyBinomialStatTest(() => rng.NextBool2(Vector128.Create(0.2, 0.8)).Item1, 0.2);
            EasyBinomialStatTest(() => rng.NextBool2(Vector128.Create(0.2, 0.8)).Item2, 0.8);
        }

        public static void ReproductionNextBool2Double1()
        {
            var rng = CreateStandardSeed();

            var expected = new (bool, bool)[] {
                (true, true),
                (false, false),
                (false, true),
                (true, false),
                (false, false),
                (false, true),
                (true, false),
                (false, true),
                (false, false),
                (false, false),
                (false, true),
                (false, false),
                (false, false),
                (true, false),
                (true, false),
                (false, true),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBool2(0.25)).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual((false, false), rng.NextBool2(0.0));
            AssertAreEqual((true, true), rng.NextBool2(1.0));

            AssertAreEqual((false, false), rng.NextBool2(double.NegativeInfinity));
            AssertAreEqual((true, true), rng.NextBool2(double.PositiveInfinity));
            AssertAreEqual((false, false), rng.NextBool2(double.NaN));
        }

        public static void CharacteristicNextBool2Double1()
        {
            var rng = CreateStandardSeed();

            EasyBinomialStatTest(() => rng.NextBool2(Vector128.Create(0.2)).Item1, 0.2);
            EasyBinomialStatTest(() => rng.NextBool2(Vector128.Create(0.2)).Item2, 0.2);
        }

        public static void ReproductionNextBool2Long()
        {
            var rng = CreateStandardSeed();

            var expected = new (bool, bool)[] {
                (false, true),
                (false, true),
                (false, true),
                (true, false),
                (false, false),
                (false, true),
                (true, false),
                (false, true),
                (false, false),
                (false, false),
                (false, true),
                (false, false),
                (false, false),
                (true, false),
                (true, false),
                (false, true),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBool2(Vector128.Create(1L, 2L), Vector128.Create(6L, 6L))).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual((false, false), rng.NextBool2(Vector128.Create(-1L, 0L), Vector128.Create(6L, 6L)));
            AssertAreEqual((true, true), rng.NextBool2(Vector128.Create(6L, 7L), Vector128.Create(6L, 6L)));
            AssertThrows<ArgumentException>(() => rng.NextBool2(Vector128.Create(2L, 3L), Vector128.Create(-6L, 6L)));
        }

        public static void CharacteristicNextBool2Long()
        {
            var rng = CreateStandardSeed();

            EasyBinomialStatTest(() => rng.NextBool2(Vector128.Create(1L, 2L), Vector128.Create(6L, 6L)).Item1, 1.0 / 6);
            EasyBinomialStatTest(() => rng.NextBool2(Vector128.Create(1L, 2L), Vector128.Create(6L, 6L)).Item2, 2.0 / 6);
        }

        public static void ReproductionNextBool2Long1()
        {
            var rng = CreateStandardSeed();

            var expected = new (bool, bool)[] {
                (false, false),
                (false, false),
                (false, false),
                (true, false),
                (false, false),
                (false, true),
                (true, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (false, false),
                (true, false),
                (true, false),
                (false, true),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBool2(1, 6)).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual((false, false), rng.NextBool2(0L, 6L));
            AssertAreEqual((true, true), rng.NextBool2(6L, 6L));
            AssertThrows<ArgumentException>(() => rng.NextBool2(2L, -6L));
        }

        public static void CharacteristicNextBool2Long1()
        {
            var rng = CreateStandardSeed();

            EasyBinomialStatTest(() => rng.NextBool2(1L, 6L).Item1, 1.0 / 6);
            EasyBinomialStatTest(() => rng.NextBool2(1L, 6L).Item2, 1.0 / 6);
        }

        public static void ReproductionNextBool4()
        {
            var rng = CreateStandardSeed();

            var expected = new (bool, bool, bool, bool)[] {
                (true, true, true, true),
                (false, false, true, true),
                (false, true, true, true),
                (false, true, true, true),
                (true, false, true, true),
                (false, false, true, true),
                (false, true, true, true),
                (false, true, true, true),
                (false, true, true, true),
                (false, true, false, false),
                (false, false, true, true),
                (false, true, true, true),
                (false, false, false, true),
                (false, true, false, true),
                (false, true, true, true),
                (false, false, true, true),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBool4(Vector128.Create(0.25f, 0.5f, 0.75f, 0.9f))).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual((false, false, true, true), rng.NextBool4(Vector128.Create(-1.0f, 0.0f, 1.0f, 2.0f)));
            AssertAreEqual((false, true, false, false), rng.NextBool4(Vector128.Create(float.NegativeInfinity, float.PositiveInfinity, float.NaN, -0.0f)));
        }

        public static void CharacteristicNextBool4()
        {
            var rng = CreateStandardSeed();

            EasyBinomialStatTest(() => rng.NextBool4(Vector128.Create(0.25f, 0.5f, 0.75f, 0.9f)).Item1, 0.25);
            EasyBinomialStatTest(() => rng.NextBool4(Vector128.Create(0.25f, 0.5f, 0.75f, 0.9f)).Item2, 0.5);
            EasyBinomialStatTest(() => rng.NextBool4(Vector128.Create(0.25f, 0.5f, 0.75f, 0.9f)).Item3, 0.75);
            EasyBinomialStatTest(() => rng.NextBool4(Vector128.Create(0.25f, 0.5f, 0.75f, 0.9f)).Item4, 0.9);
        }

        public static void ReproductionNextBool4Float1()
        {
            var rng = CreateStandardSeed();

            var expected = new (bool, bool, bool, bool)[] {
                (true, true, false, true),
                (false, false, true, false),
                (false, false, true, true),
                (false, true, true, false),
                (true, false, false, false),
                (false, false, false, true),
                (false, true, false, false),
                (false, false, true, true),
                (false, false, false, false),
                (false, false, false, false),
                (false, false, false, true),
                (false, false, false, false),
                (false, false, false, false),
                (false, true, false, false),
                (false, true, false, false),
                (false, false, true, true),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBool4(0.25f)).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual((false, false, false, false), rng.NextBool4(0.0f));
            AssertAreEqual((true, true, true, true), rng.NextBool4(1.0f));
            AssertAreEqual((false, false, false, false), rng.NextBool4(-1.0f));

            AssertAreEqual((false, false, false, false), rng.NextBool4(float.NegativeInfinity));
            AssertAreEqual((true, true, true, true), rng.NextBool4(float.PositiveInfinity));
            AssertAreEqual((false, false, false, false), rng.NextBool4(float.NaN));
        }

        public static void CharacteristicNextBool4Float1()
        {
            var rng = CreateStandardSeed();

            EasyBinomialStatTest(() => rng.NextBool4(0.25f).Item1, 0.25);
            EasyBinomialStatTest(() => rng.NextBool4(0.25f).Item2, 0.25);
            EasyBinomialStatTest(() => rng.NextBool4(0.25f).Item3, 0.25);
            EasyBinomialStatTest(() => rng.NextBool4(0.25f).Item4, 0.25);
        }

        public static void ReproductionNextBool4Int()
        {
            var rng = CreateStandardSeed();

            var expected = new (bool, bool, bool, bool)[] {
                (false, true, true, true),
                (false, false, true, true),
                (false, false, true, true),
                (true, false, false, true),
                (false, true, false, false),
                (false, false, true, true),
                (true, false, false, true),
                (false, false, true, true),
                (false, false, false, false),
                (false, false, false, false),
                (false, false, true, true),
                (false, false, false, false),
                (false, false, false, false),
                (true, false, false, false),
                (true, false, false, true),
                (false, false, true, true),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBool4(Vector128.Create(1, 2, 3, 4), Vector128.Create(6, 7, 8, 9))).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual((false, false, true, true), rng.NextBool4(Vector128.Create(-1, 0, 6, 7), Vector128.Create(6, 6, 6, 6)));
            AssertThrows<ArgumentException>(() => rng.NextBool4(Vector128.Create(2, 3, 4, 5), Vector128.Create(-6, 6, -8, 8)));
        }

        public static void CharacteristicNextBool4Int()
        {
            var rng = CreateStandardSeed();

            EasyBinomialStatTest(() => rng.NextBool4(Vector128.Create(1, 2, 3, 4), Vector128.Create(6, 6, 6, 6)).Item1, 1.0 / 6);
            EasyBinomialStatTest(() => rng.NextBool4(Vector128.Create(1, 2, 3, 4), Vector128.Create(6, 6, 6, 6)).Item2, 2.0 / 6);
            EasyBinomialStatTest(() => rng.NextBool4(Vector128.Create(1, 2, 3, 4), Vector128.Create(6, 6, 6, 6)).Item3, 3.0 / 6);
            EasyBinomialStatTest(() => rng.NextBool4(Vector128.Create(1, 2, 3, 4), Vector128.Create(6, 6, 6, 6)).Item4, 4.0 / 6);
        }

        public static void ReproductionNextBool4Int1()
        {
            var rng = CreateStandardSeed();

            var expected = new (bool, bool, bool, bool)[] {
                (false, true, false, false),
                (false, false, false, false),
                (false, false, false, false),
                (true, false, false, false),
                (false, true, false, false),
                (false, false, true, false),
                (true, false, false, false),
                (false, false, false, true),
                (false, false, false, false),
                (false, false, false, false),
                (false, false, false, false),
                (false, false, false, false),
                (false, false, false, false),
                (true, false, false, false),
                (true, false, false, false),
                (false, false, true, true),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBool4(1, 6)).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual((false, false, false, false), rng.NextBool4(0, 6));
            AssertAreEqual((true, true, true, true), rng.NextBool4(6, 6));
            AssertAreEqual((false, false, false, false), rng.NextBool4(-1, 6));
            AssertThrows<ArgumentException>(() => rng.NextBool4(6, -6));
        }

        public static void CharacteristicNextBool4Int1()
        {
            var rng = CreateStandardSeed();

            EasyBinomialStatTest(() => rng.NextBool4(1, 6).Item1, 1.0 / 6);
            EasyBinomialStatTest(() => rng.NextBool4(1, 6).Item2, 1.0 / 6);
            EasyBinomialStatTest(() => rng.NextBool4(1, 6).Item3, 1.0 / 6);
            EasyBinomialStatTest(() => rng.NextBool4(1, 6).Item4, 1.0 / 6);
        }


        public static void ReproductionNextBigIntegerMax()
        {
            var rng = CreateStandardSeed();

            var expected = new BigInteger[] {
                BigInteger.Parse("69370161652009727015069241389557412386"),
                BigInteger.Parse("76768393022518041203439069200814756030"),
                BigInteger.Parse("129304925685040947984224600444030452003"),
                BigInteger.Parse("256768954757527134793673402521702822550"),
                BigInteger.Parse("292723469860249774369065267095193421373"),
                BigInteger.Parse("62311741851173155255666726725718811020"),
                BigInteger.Parse("197517300141918255559830767042606779564"),
                BigInteger.Parse("136168597662328587537203249086354626809"),
                BigInteger.Parse("133248444993622624369489550751402418643"),
                BigInteger.Parse("125164522563218574055987688293778878274"),
                BigInteger.Parse("199268043389913934064504537392653261794"),
                BigInteger.Parse("69860830219297055462086478327282920031"),
                BigInteger.Parse("101840514535970212691253025367096507228"),
                BigInteger.Parse("326597388889783015882365375904268971346"),
                BigInteger.Parse("4930399137613651734193092971916679366"),
                BigInteger.Parse("133131781143085305087921744882916012429"),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBigInteger((BigInteger.One << 128) + 1)).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual(BigInteger.Zero, rng.NextBigInteger(BigInteger.One));
            AssertThrows<ArgumentException>(() => rng.NextBigInteger(BigInteger.Zero));
            AssertThrows<ArgumentException>(() => rng.NextBigInteger(BigInteger.MinusOne));
        }

        public static void CharacteristicNextBigIntegerMax()
        {
            var rng = CreateStandardSeed();

            EasyDiscreteUniformStatTest(() => (double)rng.NextBigInteger((BigInteger.One << 128) + 1), 0, Math.ScaleB(1, 128) + 1);
        }

        public static void ReproductionNextBigIntegerMinMax()
        {
            var rng = CreateStandardSeed();

            var expected = new BigInteger[] {
                BigInteger.Parse("69370161652009727015069241389557412385"),
                BigInteger.Parse("-263513973898420422259935538230953455427"),
                BigInteger.Parse("-210977441235897515479150006987737759454"),
                BigInteger.Parse("-83513412163411328669701204910065388907"),
                BigInteger.Parse("-47558897060688689094309340336574790084"),
                BigInteger.Parse("-277970625069765308207707880706049400437"),
                BigInteger.Parse("197517300141918255559830767042606779563"),
                BigInteger.Parse("-204113769258609875926171358345413584648"),
                BigInteger.Parse("-207033921927315839093885056680365792814"),
                BigInteger.Parse("125164522563218574055987688293778878273"),
                BigInteger.Parse("-141014323531024529398870070039114949663"),
                BigInteger.Parse("-270421536701641408001288129104485291426"),
                BigInteger.Parse("-238441852384968250772121582064671704229"),
                BigInteger.Parse("-13684978031155447581009231527499240111"),
                BigInteger.Parse("-335351967783324811729181514459851532091"),
                BigInteger.Parse("133131781143085305087921744882916012428"),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextBigInteger(-((BigInteger.One << 128) + 1), (BigInteger.One << 128) + 1)).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual(BigInteger.MinusOne, rng.NextBigInteger(BigInteger.MinusOne, BigInteger.Zero));
            AssertThrows<ArgumentException>(() => rng.NextBigInteger(BigInteger.Zero, BigInteger.Zero));
            AssertThrows<ArgumentException>(() => rng.NextBigInteger(BigInteger.One, BigInteger.MinusOne));
        }

        public static void CharacteristicNextBigIntegerMinMax()
        {
            var rng = CreateStandardSeed();

            EasyDiscreteUniformStatTest(() => (double)rng.NextBigInteger(-((BigInteger.One << 128) + 1), (BigInteger.One << 128) + 1), -(Math.ScaleB(1, 128) + 1), Math.ScaleB(1, 128) + 1);
        }

        public static void ReproductionIsProbablePrime()
        {
            var rng = CreateStandardSeed();

            AssertAreEqual(true, rng.IsProbablePrime(2));
            AssertAreEqual(true, rng.IsProbablePrime(3));
            AssertAreEqual(true, rng.IsProbablePrime(BigInteger.Parse("7455602825647884208337395736200454918783366342657")));
            AssertAreEqual(false, rng.IsProbablePrime(BigInteger.Parse("108822472788546164744491685567613056567446748240747022331102420559238060181251334487421618822485058215969458333618329123632102718702100848358615938550937209853232229002122926202513875720592569020211207766948345724883024280825027114880973470015948213765825488023083314978827288161581114552644466831")));
        }

        public static void CharacteristicIsProbablePrime()
        {
            var rng = CreateStandardSeed();

            var primes = new BigInteger[] {
                BigInteger.Parse("641"),
                BigInteger.Parse("6700417"),
                BigInteger.Parse("274177"),
                BigInteger.Parse("67280421310721"),
                BigInteger.Parse("59649589127497217"),
                BigInteger.Parse("5704689200685129054721"),
                BigInteger.Parse("1238926361552897"),
                BigInteger.Parse("93461639715357977769163558199606896584051237541638188580280321"),
                BigInteger.Parse("2424833"),
                BigInteger.Parse("7455602825647884208337395736200454918783366342657 "),
                BigInteger.Parse("741640062627530801524787141901937474059940781097519023905821316144415759504705008092818711693940737"),
                BigInteger.Parse("45592577"),
                BigInteger.Parse("6487031809"),
                BigInteger.Parse("4659775785220018543264560743076778192897"),
                BigInteger.Parse("130439874405488189727484768796509903946608530841611892186895295776832416251471863574140227977573104895898783928842923844831149032913798729088601617946094119449010595906710130531906171018354491609619193912488538116080712299672322806217820753127014424577"),
                BigInteger.Parse("319489"),
                BigInteger.Parse("974849"),
                BigInteger.Parse("167988556341760475137"),
                BigInteger.Parse("3560841906445833920513"),
                BigInteger.Parse("173462447179147555430258970864309778377421844723664084649347019061363579192879108857591038330408837177983810868451546421940712978306134189864280826014542758708589243873685563973118948869399158545506611147420216132557017260564139394366945793220968665108959685482705388072645828554151936401912464931182546092879815733057795573358504982279280090942872567591518912118622751714319229788100979251036035496917279912663527358783236647193154777091427745377038294584918917590325110939381322486044298573971650711059244462177542540706913047034664643603491382441723306598834177"),
            };

            foreach (var prime in primes)
            {
                AssertAreEqual(true, rng.IsProbablePrime(prime));
            }
            foreach (var composite in primes.Zip(primes).Select(pair => pair.First * pair.Second))
            {
                AssertAreEqual(false, rng.IsProbablePrime(composite));
            }
        }

        public static void ReproductionNextProbablePrime()
        {
            var rng = CreateStandardSeed();

            var expected = new BigInteger[] {
                BigInteger.Parse("233962957916543515432736314363394474789"),
                BigInteger.Parse("59688187410139169011169849195838845317"),
                BigInteger.Parse("13906527100130758160230359428244586127"),
                BigInteger.Parse("275438277656208936590716066635596879903"),
                BigInteger.Parse("279420538228743862449130602828928210247"),
                BigInteger.Parse("326766856068661451562963686911732250759"),
                BigInteger.Parse("91444360081770926221297092617726474309"),
                BigInteger.Parse("217898698008871217703602013143347113781"),
                BigInteger.Parse("107228330579224272156836919762337938371"),
                BigInteger.Parse("308883146330420825527072590471771820699"),
                BigInteger.Parse("119887563188585772194579190218136651473"),
                BigInteger.Parse("87000005285969053813837091144442762013"),
                BigInteger.Parse("9024352903773531307902117420237070163"),
                BigInteger.Parse("282039139292271532512752514766221509747"),
                BigInteger.Parse("339486805418846214930967703925260991409"),
                BigInteger.Parse("89937724896551931675556927905568734629"),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextProbablePrime((BigInteger.One << 128) + 1)).ToArray();
            AssertAreEqual(expected, actual);

            AssertAreEqual(new BigInteger(2), rng.NextProbablePrime(3));
            AssertThrows<ArgumentException>(() => rng.NextProbablePrime(new BigInteger(2)));
            AssertThrows<ArgumentException>(() => rng.NextProbablePrime(BigInteger.MinusOne));
        }

        public static void CharacteristicNextProbablePrime()
        {
            var rng = CreateStandardSeed();

            for (int i = 0; i < 1024; i++)
            {
                var prime = rng.NextProbablePrime((BigInteger.One << 256) + 1);

                AssertAreEqual(true, rng.IsProbablePrime(prime));

                var number = rng.NextBigInteger(prime);
                AssertAreEqual(number, BigInteger.ModPow(number, prime, prime));
            }
        }


        public static void ReproductionNextEnum()
        {
            var rng = CreateStandardSeed();

            var expected = new DayOfWeek[] {
                DayOfWeek.Monday,
                DayOfWeek.Friday,
                DayOfWeek.Tuesday,
                DayOfWeek.Sunday,
                DayOfWeek.Thursday,
                DayOfWeek.Thursday,
                DayOfWeek.Sunday,
                DayOfWeek.Wednesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Monday,
                DayOfWeek.Friday,
                DayOfWeek.Wednesday,
                DayOfWeek.Friday,
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Friday,
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextEnum<DayOfWeek>()).ToArray();
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentException>(() => rng.NextEnum<EmptyEnum>());
        }

        private enum EmptyEnum { }

        public static void CharacteristicNextEnum()
        {
            var rng = CreateStandardSeed();

            EasyDiscreteUniformStatTest(() => (int)rng.NextEnum<DayOfWeek>(), 0, 6);
        }

        public static void ReproductionNextGuid()
        {
            var rng = CreateStandardSeed();

            var expected = new Guid[] {
                 new Guid("1c8ef222-13af-4c8a-8bf4-134208353034"),
                 new Guid("e3f3ea59-9b55-426f-aae2-652c9966814f"),
                 new Guid("fc4cd0be-d9ae-4cf8-8b0c-9631000ec139"),
                 new Guid("f71ebcc4-8ea2-458f-85f2-d12cf849ff78"),
                 new Guid("15148123-18df-4058-9be4-639b0b394761"),
                 new Guid("6ccb5cdc-6aa0-4c61-a880-976bb13e0b0a"),
                 new Guid("9476f296-2a55-4fb1-abf1-a352e1e62bc1"),
                 new Guid("5203a81a-3499-4cdd-80b1-8c0000f08a3b"),
                 new Guid("606a863d-26a9-4eb4-8b24-419d567d38dc"),
                 new Guid("a904f2a8-ea9c-4646-9519-34d4b9edc8ff"),
                 new Guid("7d36a18c-714c-49b8-8b8a-635211cee02e"),
                 new Guid("5d918c6e-c7f6-4a3d-8c2b-5ab709771ec9"),
                 new Guid("a469ccac-134c-4339-a829-fcfcc2729894"),
                 new Guid("c48dcac9-f5b7-4f5a-9081-62fc458392a3"),
                 new Guid("aabb18f9-09d4-4884-bcbb-eb6e331e7166"),
                 new Guid("82d73662-8151-42a0-905c-67177df88909"),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextGuid()).ToArray();
            AssertAreEqual(expected, actual);
        }

        public static void CharacteristicNextGuid()
        {
            var rng = CreateStandardSeed();

            var and = Vector128.Create(~0ul, ~0ul);
            var or = Vector128.Create(0ul, 0ul);

            for (int i = 0; i < 64; i++)
            {
                var guid = rng.NextGuid();

                var asvector = MemoryMarshal.Cast<Guid, Vector128<ulong>>(MemoryMarshal.CreateSpan(ref guid, 1))[0];
                and &= asvector;
                or |= asvector;
            }

            AssertAreEqual(Vector128.Create(0x40000000_00000000ul, 0x00000000_00000080ul), and);
            AssertAreEqual(Vector128.Create(0x4fffffff_fffffffful, 0xffffffff_ffffffbful), or);
        }

        public static void ReproductionNextUlid()
        {
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var rng = CreateStandardSeed();
            var expected = new Guid[] {
                new Guid(timestamp.ToString("x12") + "1817cbf4134208353034"),
                new Guid(timestamp.ToString("x12") + "f6f22ae2652c9966814f"),
                new Guid(timestamp.ToString("x12") + "68658b0c9631000ec139"),
                new Guid(timestamp.ToString("x12") + "311205f2d12cf849ff78"),
                new Guid(timestamp.ToString("x12") + "84c59be4639b0b394761"),
                new Guid(timestamp.ToString("x12") + "88fc6880976bb13e0b0a"),
                new Guid(timestamp.ToString("x12") + "3b2cebf1a352e1e62bc1"),
                new Guid(timestamp.ToString("x12") + "584080b18c0000f08a3b"),
                new Guid(timestamp.ToString("x12") + "4a298b24419d567d38dc"),
                new Guid(timestamp.ToString("x12") + "62db551934d4b9edc8ff"),
                new Guid(timestamp.ToString("x12") + "ed258b8a635211cee02e"),
                new Guid(timestamp.ToString("x12") + "5ea08c2b5ab709771ec9"),
                new Guid(timestamp.ToString("x12") + "f7a4a829fcfcc2729894"),
                new Guid(timestamp.ToString("x12") + "2bc7d08162fc458392a3"),
                new Guid(timestamp.ToString("x12") + "0c19bcbbeb6e331e7166"),
                new Guid(timestamp.ToString("x12") + "f63d505c67177df88909"),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextUlid()).ToArray();

            ShouldBe(expected.SequenceEqual(actual));
        }

        public static void CharacteristicNextUlid()
        {
            var rng = CreateStandardSeed();

            var and = Vector128.Create(~0ul, ~0ul);
            var or = Vector128.Create(0ul, 0ul);

            for (int i = 0; i < 64; i++)
            {
                var ulid = rng.NextUlid();

                var asvector = MemoryMarshal.Cast<Guid, Vector128<ulong>>(MemoryMarshal.CreateSpan(ref ulid, 1))[0];
                and &= asvector;
                or |= asvector;
            }

            var randomMask = Vector128.Create(0xffff0000_00000000ul, 0xffffffff_fffffffful);
            AssertAreEqual(Vector128.Create(0x00000000_00000000ul, 0x00000000_00000000ul), Vector128.BitwiseAnd(and, randomMask));
            AssertAreEqual(Vector128.Create(0xffff0000_00000000ul, 0xffffffff_fffffffful), Vector128.BitwiseAnd(or, randomMask));
        }

        public static void ReproductionNextUlidTimestamp()
        {
            // 0x0188249DABFB
            var timestamp = new DateTimeOffset(2023, 5, 16, 21, 51, 32, 987, TimeSpan.FromHours(9)).ToUnixTimeMilliseconds();

            var rng = CreateStandardSeed();
            var expected = new Guid[] {
                new Guid("0188249d-abfb-1817-cbf4-134208353034"),
                new Guid("0188249d-abfb-f6f2-2ae2-652c9966814f"),
                new Guid("0188249d-abfb-6865-8b0c-9631000ec139"),
                new Guid("0188249d-abfb-3112-05f2-d12cf849ff78"),
                new Guid("0188249d-abfb-84c5-9be4-639b0b394761"),
                new Guid("0188249d-abfb-88fc-6880-976bb13e0b0a"),
                new Guid("0188249d-abfb-3b2c-ebf1-a352e1e62bc1"),
                new Guid("0188249d-abfb-5840-80b1-8c0000f08a3b"),
                new Guid("0188249d-abfb-4a29-8b24-419d567d38dc"),
                new Guid("0188249d-abfb-62db-5519-34d4b9edc8ff"),
                new Guid("0188249d-abfb-ed25-8b8a-635211cee02e"),
                new Guid("0188249d-abfb-5ea0-8c2b-5ab709771ec9"),
                new Guid("0188249d-abfb-f7a4-a829-fcfcc2729894"),
                new Guid("0188249d-abfb-2bc7-d081-62fc458392a3"),
                new Guid("0188249d-abfb-0c19-bcbb-eb6e331e7166"),
                new Guid("0188249d-abfb-f63d-505c-67177df88909"),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextUlid(timestamp)).ToArray();
            AssertAreEqual(expected, actual);
        }

        public static void CharacteristicNextUlidTimestamp()
        {
            var rng = CreateStandardSeed();

            var timestamp = new DateTimeOffset(2023, 5, 16, 21, 51, 32, 987, TimeSpan.FromHours(9)).ToUnixTimeMilliseconds();

            var and = Vector128.Create(~0ul, ~0ul);
            var or = Vector128.Create(0ul, 0ul);

            for (int i = 0; i < 64; i++)
            {
                var ulid = rng.NextUlid(timestamp);

                var asvector = MemoryMarshal.Cast<Guid, Vector128<ulong>>(MemoryMarshal.CreateSpan(ref ulid, 1))[0];
                and &= asvector;
                or |= asvector;
            }

            AssertAreEqual(Vector128.Create(0x0000abfb_0188249dul, 0x00000000_00000000ul), and);
            AssertAreEqual(Vector128.Create(0xffffabfb_0188249dul, 0xffffffff_fffffffful), or);
        }


        public static void ReproductionNextGaussian()
        {
            var rng = CreateStandardSeed();
            var expected = new double[] {
                1.1105650289206304,
                -0.6427624409003508,
                0.6636729116686806,
                0.1782797109973765,
                -1.7443866195737778,
                -0.5577627496122389,
                0.3418876069098378,
                2.4108561605848555,
                1.7663542139477968,
                0.6915210653988335,
                -0.6138509196099251,
                1.5775267688711463,
                -0.43098523343670414,
                0.12228296430255546,
                0.13355036809726528,
                -0.6161046250879065
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextGaussian()).ToArray();
            AssertAreEqual(expected, actual);
        }

        public static void CharacteristicNextGaussian()
        {
            var rng = CreateStandardSeed();

            EasyGaussianStatTest(() => rng.NextGaussian());
        }

        private static void EasyGaussianStatTest(Func<double> next)
        {
            double theoreticalAverage = 0;
            double theoreticalVariance = 1;
            double min = 0;
            double max = 0;
            double average = 0;
            double variance = 0;

            for (int i = 0; i < 1 << 24; i++)
            {
                var value = next();

                min = Math.Min(value, min);
                max = Math.Max(value, max);
                average = average * (i / (i + 1.0)) + value / (i + 1.0);
                variance = variance * (i / (i + 1.0)) + (theoreticalAverage - value) * (theoreticalAverage - value) / (i + 1.0);
            }

            Assert(min < -4, -4, min);
            Assert(max > 4, 4, max);
            Assert(theoreticalAverage - 0.01 < average && average < theoreticalAverage + 0.01, theoreticalAverage, average);
            Assert(theoreticalVariance - 0.01 < variance && variance < theoreticalVariance + 0.01, theoreticalVariance, variance);

            ShouldBe(min < -5, -5, min);
            ShouldBe(max > 5, 5, max);
            ShouldBe(theoreticalAverage - 0.001 < average && average < theoreticalAverage + 0.001, theoreticalAverage, average);
            ShouldBe(theoreticalVariance - 0.001 < variance && variance < theoreticalVariance + 0.001, theoreticalVariance, variance);
        }

        public static void ReproductionNextExponential()
        {
            var rng = CreateStandardSeed();
            var expected = new double[] {
                0.8214004121756002,
                1.8088052642951093,
                0.28852614252150904,
                0.07540663250852259,
                2.1530136085643345,
                0.42319457363507795,
                0.17267078052545642,
                1.8539159591370373,
                1.1771564710358624,
                0.32819337773014745,
                1.1899986490102077,
                0.9021640486431689,
                0.9507213954831581,
                0.05046814869293399,
                0.029817554477172905,
                1.6957191122376356,
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextExponential()).ToArray();
            AssertAreEqual(expected, actual);
        }

        public static void CharacteristicNextExponential()
        {
            var rng = CreateStandardSeed();

            EasyExponentialStatTest(() => rng.NextExponential());
        }

        private static void EasyExponentialStatTest(Func<double> next)
        {
            double theoreticalAverage = 1;
            double theoreticalVariance = 1;
            double min = 1;
            double max = 1;
            double average = 0;
            double variance = 0;

            for (int i = 0; i < 1 << 24; i++)
            {
                var value = next();

                min = Math.Min(value, min);
                max = Math.Max(value, max);
                average = average * (i / (i + 1.0)) + value / (i + 1.0);
                variance = variance * (i / (i + 1.0)) + (theoreticalAverage - value) * (theoreticalAverage - value) / (i + 1.0);
            }

            Assert(min > 0, 0, min);
            Assert(max > 4, 4, max);
            Assert(theoreticalAverage - 0.01 < average && average < theoreticalAverage + 0.01, theoreticalAverage, average);
            Assert(theoreticalVariance - 0.01 < variance && variance < theoreticalVariance + 0.01, theoreticalVariance, variance);

            ShouldBe(max > 6, 6, max);
        }


        public static void ReproductionNextBytes()
        {
            var rng = CreateStandardSeed();
            var expected = new byte[] {
                0x22, 0xf2, 0x8e, 0x1c, 0xaf, 0x13, 0x8a, 0x3c, 0xcb, 0xf4, 0x13, 0x42, 0x08, 0x35, 0x30, 0x34,
                0x59, 0xea, 0xf3, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };
            var actual = new byte[32];
            rng.NextBytes(actual.AsSpan(..19));

            AssertAreEqual(expected, actual);


            // must not throw
            rng.NextBytes(new byte[0]);
        }

        public static void CharacteristicNextBytes()
        {
            var rng = CreateStandardSeed();

            EasyDiscreteUniformStatTest(() =>
            {
                var buffer = (stackalloc byte[1]);
                rng.NextBytes(buffer);
                return buffer[0];
            }, 0, 255);
        }

        public static void ReproductionNextBytesInt()
        {
            var rng = CreateStandardSeed();
            var expected = new byte[] {
                0x22, 0xf2, 0x8e, 0x1c, 0xaf, 0x13, 0x8a, 0x3c, 0xcb, 0xf4, 0x13, 0x42, 0x08, 0x35, 0x30, 0x34,
                0x59, 0xea, 0xf3, 0xe3, 0x55, 0x9b, 0x6f, 0xd2, 0x2a, 0xe2, 0x65, 0x2c, 0x99, 0x66, 0x81, 0x4f,
            };
            var actual = rng.NextBytes(32);

            AssertAreEqual(expected, actual);


            AssertAreEqual(new byte[0], rng.NextBytes(0));
            AssertThrows<ArgumentException>(() => rng.NextBytes(-1));
        }

        public static void CharacteristicNextBytesInt()
        {
            var rng = CreateStandardSeed();

            EasyDiscreteUniformStatTest(() => rng.NextBytes(1)[0], 0, 255);
        }

        public static void ReproductionFill()
        {
            var rng = CreateStandardSeed();
            var expected = new Vector128<byte>[] {
                Vector128.Create(34, 242, 142, 28, 175, 19, 138, 60, 203, 244, 19, 66, 8, 53, 48, 52),
                Vector128.Create(89, 234, 243, 227, 85, 155, 111, 210, 42, 226, 101, 44, 153, 102, 129, 79),
                Vector128.Create(190, 208, 76, 252, 174, 217, 248, 76, 139, 12, 150, 49, 0, 14, 193, 57),
                Vector128.Create(196, 188, 30, 247, 162, 142, 143, 21, 5, 242, 209, 44, 248, 73, 255, 120),
                Vector128.Create(35, 129, 20, 21, 223, 24, 88, 160, 155, 228, 99, 155, 11, 57, 71, 97),
                Vector128.Create(220, 92, 203, 108, 160, 106, 97, 172, 104, 128, 151, 107, 177, 62, 11, 10),
                Vector128.Create(150, 242, 118, 148, 85, 42, 177, 31, 235, 241, 163, 82, 225, 230, 43, 193),
                Vector128.Create(26, 168, 3, 82, 153, 52, 221, 124, 128, 177, 140, 0, 0, 240, 138, 59),
                Vector128.Create(61, 134, 106, 96, 169, 38, 180, 110, 139, 36, 65, 157, 86, 125, 56, 220),
                Vector128.Create(168, 242, 4, 169, 156, 234, 70, 70, 85, 25, 52, 212, 185, 237, 200, 255),
                Vector128.Create(140, 161, 54, 125, 76, 113, 184, 201, 139, 138, 99, 82, 17, 206, 224, 46),
                Vector128.Create(110, 140, 145, 93, 246, 199, 61, 122, 140, 43, 90, 183, 9, 119, 30, 201),
                Vector128.Create(172, 204, 105, 164, 76, 19, 57, 211, 168, 41, 252, 252, 194, 114, 152, 148),
                Vector128.Create(201, 202, 141, 196, 183, 245, 90, 15, 208, 129, 98, 252, 69, 131, 146, 163),
                Vector128.Create(249, 24, 187, 170, 212, 9, 132, 40, 188, 187, 235, 110, 51, 30, 113, 102),
                Vector128.Create(98, 54, 215, 130, 81, 129, 160, 210, 80, 92, 103, 23, 125, 248, 137, 9),
            };
            var actual = new Vector128<byte>[16];
            rng.Fill(actual.AsSpan());

            AssertAreEqual(expected, actual);


            rng.Fill(actual.AsSpan(0..0));
        }

        public static void CharacteristicFill()
        {
            var rng = CreateStandardSeed();

            EasyDiscreteUniformStatTest(() =>
            {
                var buffer = (stackalloc ushort[1]);
                rng.Fill(buffer);
                return buffer[0];
            }, 0, 65535);
        }


        public static void ReproductionShuffle()
        {
            var rng = CreateStandardSeed();
            var expected = new int[] {
                1, 11, 5, 4, 14, 10, 7, 3, 8, 0, 12, 6, 13, 9, 15, 2
            };
            var actual = Enumerable.Range(0, 16).ToArray();
            rng.Shuffle(actual.AsSpan());

            AssertAreEqual(expected, actual);


            rng.Shuffle(actual.AsSpan(0..0));
        }

        public static void CharacteristicShuffle()
        {
            var rng = CreateStandardSeed();
            int length = 16;
            int trial = 1 << 20;

            var bucket = new int[length][];
            for (int i = 0; i < bucket.Length; i++)
                bucket[i] = new int[length];

            for (int i = 0; i < trial; i++)
            {
                var temp = Enumerable.Range(0, length).ToArray();
                rng.Shuffle(temp.AsSpan());

                for (int k = 0; k < temp.Length; k++)
                {
                    bucket[k][temp[k]]++;
                }
            }


            double theoreticalAverage = (double)trial / length;
            for (int before = 0; before < bucket.Length; before++)
            {
                for (int after = 0; after < bucket.Length; after++)
                {
                    Assert(theoreticalAverage * 0.99 < bucket[before][after] && bucket[before][after] < theoreticalAverage * 1.01,
                        theoreticalAverage, bucket[before][after]);
                }
            }
        }

        public static void ReproductionShuffleIterator()
        {
            var rng = CreateStandardSeed();
            var expected = new int[] {
                1, 11, 5, 4, 14, 10, 7, 3, 8, 0, 12, 6, 13, 9, 15, 2
            };
            var actual = rng.Shuffle(Enumerable.Range(0, 16)).ToArray();

            AssertAreEqual(expected, actual);


            AssertAreEqual(Enumerable.Empty<int>(), rng.Shuffle(Enumerable.Empty<int>()));

            static IEnumerable<int> Thrower()
            {
                throw new InvalidOperationException("must not enumerate until materialized");
#pragma warning disable CS0162
                yield return 0;
#pragma warning restore
            }
            var thrower = Thrower();
            var iter = rng.Shuffle(thrower);
            AssertThrows<InvalidOperationException>(() => iter.ToArray());
        }

        public static void CharacteristicShuffleIterator()
        {
            var rng = CreateStandardSeed();
            int length = 16;
            int trial = 1 << 20;

            var bucket = new int[length][];
            for (int i = 0; i < bucket.Length; i++)
                bucket[i] = new int[length];

            for (int i = 0; i < trial; i++)
            {
                var temp = rng.Shuffle(Enumerable.Range(0, length)).ToArray();

                for (int k = 0; k < temp.Length; k++)
                {
                    bucket[k][temp[k]]++;
                }
            }


            double theoreticalAverage = (double)trial / length;
            for (int before = 0; before < bucket.Length; before++)
            {
                for (int after = 0; after < bucket.Length; after++)
                {
                    Assert(theoreticalAverage * 0.99 < bucket[before][after] && bucket[before][after] < theoreticalAverage * 1.01,
                        theoreticalAverage, bucket[before][after]);
                }
            }
        }

        public static void ReproductionGetItems()
        {
            var rng = CreateStandardSeed();
            var expected = "1343cc248813ea30f1f323452ac02fb8";
            var actual = new char[32];
            rng.GetItems("0123456789abcdef".AsSpan(), actual);

            AssertAreEqual(expected, new string(actual));


            rng.GetItems("0123456789abcdef".AsSpan(), actual.AsSpan(0..0));
            AssertThrows<ArgumentException>(() => rng.GetItems(Span<char>.Empty, actual.AsSpan()));
        }

        public static void CharacteristicGetItems()
        {
            var rng = CreateStandardSeed();

            const int trial = 1 << 20;

            var source = Enumerable.Range(0, 16).ToArray();
            var destination = new int[16];

            var bucket = new int[destination.Length];

            for (int i = 0; i < trial; i++)
            {
                rng.GetItems(source, destination.AsSpan());

                for (int k = 0; k < destination.Length; k++)
                {
                    bucket[destination[k]]++;
                }
            }


            for (int i = 0; i < bucket.Length; i++)
            {
                Assert(trial * 0.99 < bucket[i] && bucket[i] < trial * 1.01, trial, bucket[i]);
            }
        }

        public static void ReproductionGetItemsSpanInt()
        {
            var rng = CreateStandardSeed();
            var expected = "1343cc248813ea30f1f323452ac02fb8";

            AssertAreEqual(expected, new string(rng.GetItems("0123456789abcdef".AsSpan(), 32)));


            rng.GetItems("0123456789abcdef".AsSpan(), 0);
            AssertThrows<ArgumentException>(() => rng.GetItems("0123456789abcdef".AsSpan(), -1));
            AssertThrows<ArgumentException>(() => rng.GetItems<char>(Span<char>.Empty, 32));
        }

        public static void CharacteristicGetItemsSpanInt()
        {
            var rng = CreateStandardSeed();

            const int trial = 1 << 20;

            var source = Enumerable.Range(0, 16).ToArray();

            var bucket = new int[source.Length];

            for (int i = 0; i < trial; i++)
            {
                var destination = rng.GetItems((ReadOnlySpan<int>)source.AsSpan(), 16);

                for (int k = 0; k < destination.Length; k++)
                {
                    bucket[destination[k]]++;
                }
            }


            for (int i = 0; i < bucket.Length; i++)
            {
                Assert(trial * 0.99 < bucket[i] && bucket[i] < trial * 1.01, trial, bucket[i]);
            }
        }

        public static void ReproductionGetItemsArrayInt()
        {
            var rng = CreateStandardSeed();
            var expected = "1343cc248813ea30f1f323452ac02fb8";

            AssertAreEqual(expected, new string(rng.GetItems("0123456789abcdef".ToCharArray(), 32)));


            rng.GetItems("0123456789abcdef".ToCharArray(), 0);
            AssertThrows<ArgumentException>(() => rng.GetItems("0123456789abcdef".ToCharArray(), -1));
            AssertThrows<ArgumentNullException>(() => rng.GetItems((char[])null!, 1));
            AssertThrows<ArgumentException>(() => rng.GetItems(new char[0], 32));
        }

        public static void CharacteristicGetItemsArrayInt()
        {
            var rng = CreateStandardSeed();

            const int trial = 1 << 20;

            var source = Enumerable.Range(0, 16).ToArray();

            var bucket = new int[source.Length];

            for (int i = 0; i < trial; i++)
            {
                var destination = rng.GetItems(source, 16);

                for (int k = 0; k < destination.Length; k++)
                {
                    bucket[destination[k]]++;
                }
            }


            for (int i = 0; i < bucket.Length; i++)
            {
                Assert(trial * 0.99 < bucket[i] && bucket[i] < trial * 1.01, trial, bucket[i]);
            }
        }

        public static void ReproductionGetItemsIterator()
        {
            var rng = CreateStandardSeed();
            var expected = "cggfxdshkwmvskrbmohkyffgaltdzhff";
            var actual = new string(rng.GetItems(Enumerable.Range('a', 26)).Take(32).Select(i => (char)i).ToArray());
            AssertAreEqual(expected, actual);


            AssertThrows<ArgumentException>(() => rng.GetItems(Enumerable.Empty<int>()).ToArray());

            static IEnumerable<int> Thrower()
            {
                throw new InvalidOperationException("must not enumerate until materialized");
#pragma warning disable CS0162
                yield return 0;
#pragma warning restore
            }
            var thrower = Thrower();
            var iter = rng.GetItems(thrower);
            AssertThrows<InvalidOperationException>(() => iter.ToArray());
        }

        public static void CharacteristicGetItemsIterator()
        {
            var rng = CreateStandardSeed();

            const int trial = 1 << 20;

            var source = Enumerable.Range(0, 16).ToArray();

            var bucket = new int[source.Length];

            foreach (var elem in rng.GetItems(source).Take(trial))
            {
                bucket[elem]++;
            }

            for (int i = 0; i < bucket.Length; i++)
            {
                Assert(trial / bucket.Length * 0.99 < bucket[i] && bucket[i] < trial / bucket.Length * 1.01, trial, bucket[i]);
            }
        }

        public static void ReproductionWeightedChoice()
        {
            var rng = CreateStandardSeed();

            string expected = "....oO.o.......O..o..o.....oo.o.";
            var actualArray = new char[32];
            rng.WeightedChoice(".oO".AsSpan(), new double[] { 78.5, 18.5, 3.0 }.AsSpan(), actualArray);
            string actual = new string(actualArray);
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentException>(() => rng.WeightedChoice(ReadOnlySpan<char>.Empty, ReadOnlySpan<double>.Empty, actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".oO".AsSpan(), new double[1] { 100.0 }.AsSpan(), actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".".AsSpan(), new double[] { 0 }.AsSpan(), actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".AsSpan(), new double[] { -1, 2 }.AsSpan(), actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".AsSpan(), new double[] { double.NaN, double.NaN }.AsSpan(), actualArray));

            rng.WeightedChoice(".oO".AsSpan(), new double[] { 78.5, 18.5, 3.0 }.AsSpan(), actualArray.AsSpan(0..0));
            rng.WeightedChoice(".".AsSpan(), new double[] { 1 }.AsSpan(), actualArray);
            AssertAreEqual(new string('.', 32), new string(actualArray));
            rng.WeightedChoice(".o".AsSpan(), new double[] { 1, 0 }.AsSpan(), actualArray);
            AssertAreEqual(new string('.', 32), new string(actualArray));
        }

        public static void CharacteristicWeightedChoice()
        {
            var rng = CreateStandardSeed();
            const int trial = 1 << 20;
            var buffer = new char[16];
            var bucket = new Dictionary<char, int>();

            var source = "abcdefghijklmnopqrstuvwxyz".AsSpan();
            var weights = Enumerable.Range(1, 26).Select(i => (double)i).ToArray().AsSpan();

            for (int i = 0; i < trial; i++)
            {
                rng.WeightedChoice(source, weights, buffer);

                for (int k = 0; k < buffer.Length; k++)
                {
                    if (bucket.ContainsKey(buffer[k]))
                        bucket[buffer[k]]++;
                    else
                        bucket.Add(buffer[k], 1);
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                var expected = trial * buffer.Length * weights[i] / (27 * 13);
                var actual = bucket[source[i]];
                Assert(expected * 0.99 < actual && actual < expected * 1.01,
                    expected, actual);
            }
        }

        public static void ReproductionWeightedChoiceSelector()
        {
            var rng = CreateStandardSeed();

            string expected = "....oO.o.......O..o..o.....oo.o.";
            var actualArray = new char[32];
            rng.WeightedChoice(".oO".AsSpan(), c => c switch { '.' => 78.5, 'o' => 18.5, _ => 3.0 }, actualArray);
            string actual = new string(actualArray);
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentException>(() => rng.WeightedChoice(ReadOnlySpan<char>.Empty, c => c, actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".oO".AsSpan(), (Func<char, double>)null!, actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".".AsSpan(), c => 0, actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".AsSpan(), c => -1, actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".AsSpan(), c => double.NaN, actualArray));

            rng.WeightedChoice(".oO".AsSpan(), c => c switch { '.' => 78.5, 'o' => 18.5, _ => 3.0 }, actualArray.AsSpan(0..0));
            rng.WeightedChoice(".".AsSpan(), c => 1, actualArray);
            AssertAreEqual(new string('.', 32), new string(actualArray));
            rng.WeightedChoice(".o".AsSpan(), c => c switch { '.' => 1, _ => 0 }, actualArray);
            AssertAreEqual(new string('.', 32), new string(actualArray));
        }

        public static void CharacteristicWeightedChoiceSelector()
        {
            var rng = CreateStandardSeed();
            const int trial = 1 << 20;
            var buffer = new char[16];
            var bucket = new Dictionary<char, int>();

            var source = "abcdefghijklmnop".AsSpan();
            static double weightSelector(char c) => c - 'a' + 1;

            for (int i = 0; i < trial; i++)
            {
                rng.WeightedChoice(source, weightSelector, buffer);

                for (int k = 0; k < buffer.Length; k++)
                {
                    if (bucket.ContainsKey(buffer[k]))
                        bucket[buffer[k]]++;
                    else
                        bucket.Add(buffer[k], 1);
                }
            }

            for (int i = 0; i < source.Length; i++)
            {
                var expected = trial * buffer.Length * weightSelector(source[i]) / (17 * 8);
                var actual = bucket[source[i]];
                Assert(expected * 0.99 < actual && actual < expected * 1.01,
                    expected, actual);
            }
        }

        public static void ReproductionWeightedChoiceInt()
        {
            var rng = CreateStandardSeed();

            string expected = "....oO.o.......O..o..o.....oo.o.";
            var actualArray = rng.WeightedChoice(".oO".AsSpan(), new double[] { 78.5, 18.5, 3.0 }.AsSpan(), 32);
            string actual = new string(actualArray);
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentException>(() => rng.WeightedChoice(ReadOnlySpan<char>.Empty, ReadOnlySpan<double>.Empty, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".oO".AsSpan(), new double[1] { 100.0 }.AsSpan(), 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".".AsSpan(), new double[] { 0 }.AsSpan(), 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".AsSpan(), new double[] { -1, 2 }.AsSpan(), 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".AsSpan(), new double[] { double.NaN, double.NaN }.AsSpan(), 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".oO".AsSpan(), new double[] { 78.5, 18.5, 3.0 }.AsSpan(), -1));

            AssertAreEqual(new char[0], rng.WeightedChoice(".oO".AsSpan(), new double[] { 78.5, 18.5, 3.0 }.AsSpan(), 0));
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".".AsSpan(), new double[] { 1 }.AsSpan(), 32)));
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".o".AsSpan(), new double[] { 1, 0 }.AsSpan(), 32)));
        }

        public static void CharacteristicWeightedChoiceInt()
        {
            var rng = CreateStandardSeed();
            const int trial = 1 << 20;
            const int bufferLength = 16;
            var bucket = new Dictionary<char, int>();

            var source = "abcdefghijklmnopqrstuvwxyz".AsSpan();
            var weights = Enumerable.Range(1, 26).Select(i => i / (27.0 * 13)).ToArray().AsSpan();

            for (int i = 0; i < trial; i++)
            {
                var buffer = rng.WeightedChoice(source, weights, bufferLength);

                for (int k = 0; k < buffer.Length; k++)
                {
                    if (bucket.ContainsKey(buffer[k]))
                        bucket[buffer[k]]++;
                    else
                        bucket.Add(buffer[k], 1);
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                var expected = trial * bufferLength * weights[i];
                var actual = bucket[source[i]];
                Assert(expected * 0.99 < actual && actual < expected * 1.01,
                    expected, actual);
            }
        }

        public static void ReproductionWeightedChoiceIntSelector()
        {
            var rng = CreateStandardSeed();

            string expected = "....oO.o.......O..o..o.....oo.o.";
            var actualArray = rng.WeightedChoice(".oO".AsSpan(), c => c switch { '.' => 78.5, 'o' => 18.5, _ => 3.0 }, 32);
            string actual = new string(actualArray);
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentException>(() => rng.WeightedChoice(ReadOnlySpan<char>.Empty, c => c, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".oO".AsSpan(), (Func<char, double>)null!, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".".AsSpan(), c => 0, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".AsSpan(), c => -1, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".AsSpan(), c => double.NaN, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".AsSpan(), c => 1, -1));

            AssertAreEqual(new char[0], rng.WeightedChoice(".oO".AsSpan(), c => c switch { '.' => 78.5, 'o' => 18.5, _ => 3.0 }, 0));
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".".AsSpan(), c => 1, 32)));
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".o".AsSpan(), c => c switch { '.' => 1, _ => 0 }, 32)));
        }

        public static void CharacteristicWeightedChoiceIntSelector()
        {
            var rng = CreateStandardSeed();
            const int trial = 1 << 20;
            const int bufferLength = 16;
            var bucket = new Dictionary<char, int>();

            var source = "0123456789".AsSpan();
            static double weightSelector(char c) => c - '0' + 1;

            for (int i = 0; i < trial; i++)
            {
                var buffer = rng.WeightedChoice(source, weightSelector, bufferLength);

                for (int k = 0; k < buffer.Length; k++)
                {
                    if (bucket.ContainsKey(buffer[k]))
                        bucket[buffer[k]]++;
                    else
                        bucket.Add(buffer[k], 1);
                }
            }

            for (int i = 0; i < source.Length; i++)
            {
                var expected = trial * bufferLength * weightSelector(source[i]) / 55;
                var actual = bucket[source[i]];
                Assert(expected * 0.99 < actual && actual < expected * 1.01,
                    expected, actual);
            }
        }

        public static void ReproductionWeightedChoiceArray()
        {
            var rng = CreateStandardSeed();

            string expected = "....oO.o.......O..o..o.....oo.o.";
            var actualArray = new char[32];
            rng.WeightedChoice(".oO".ToCharArray(), new double[] { 78.5, 18.5, 3.0 }, actualArray);
            string actual = new string(actualArray);
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentException>(() => rng.WeightedChoice(new char[0], new double[0], actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".oO".ToCharArray(), new double[1] { 100.0 }, actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".".ToCharArray(), new double[] { 0 }, actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".ToCharArray(), new double[] { -1, 2 }, actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".ToCharArray(), new double[] { double.NaN, double.NaN }, actualArray));
            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice(null!, new double[] { 78.5, 18.5, 3.0 }, actualArray));
            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice(".oO".ToCharArray(), (double[])null!, actualArray));
            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice(".oO".ToCharArray(), new double[] { 78.5, 18.5, 3.0 }, null!));

            rng.WeightedChoice(".oO".ToCharArray(), new double[] { 78.5, 18.5, 3.0 }, new char[0]);

            rng.WeightedChoice(".".ToCharArray(), new double[] { 1 }, actualArray);
            AssertAreEqual(new string('.', 32), new string(actualArray));
            rng.WeightedChoice(".o".ToCharArray(), new double[] { 1, 0 }, actualArray);
            AssertAreEqual(new string('.', 32), new string(actualArray));
        }

        public static void CharacteristicWeightedChoiceArray()
        {
            var rng = CreateStandardSeed();
            const int trial = 1 << 20;
            var buffer = new char[16];
            var bucket = new Dictionary<char, int>();

            var source = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            var weights = Enumerable.Range(1, 26).Select(i => 10.0 + i).ToArray();

            for (int i = 0; i < trial; i++)
            {
                rng.WeightedChoice(source, weights, buffer);

                for (int k = 0; k < buffer.Length; k++)
                {
                    if (bucket.ContainsKey(buffer[k]))
                        bucket[buffer[k]]++;
                    else
                        bucket.Add(buffer[k], 1);
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                var expected = trial * buffer.Length * weights[i] / (27 * 13 + 260);
                var actual = bucket[source[i]];
                Assert(expected * 0.99 < actual && actual < expected * 1.01,
                    expected, actual);
            }
        }


        public static void ReproductionWeightedChoiceArraySelector()
        {
            var rng = CreateStandardSeed();

            string expected = "....oO.o.......O..o..o.....oo.o.";
            var actualArray = new char[32];
            rng.WeightedChoice(".oO".ToCharArray(), c => c switch { '.' => 78.5, 'o' => 18.5, _ => 3.0 }, actualArray);
            string actual = new string(actualArray);
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice(null!, c => c, actualArray));
            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice(".oO".ToCharArray(), (Func<char, double>)null!, actualArray));
            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice(".oO".ToCharArray(), c => c, null!));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(new char[0], c => c, actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".".ToCharArray(), c => 0, actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".ToCharArray(), c => -1, actualArray));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".ToCharArray(), c => double.NaN, actualArray));

            rng.WeightedChoice(".oO".ToCharArray(), c => c switch { '.' => 78.5, 'o' => 18.5, _ => 3.0 }, new char[0]);
            rng.WeightedChoice(".".ToCharArray(), c => 1, actualArray);
            AssertAreEqual(new string('.', 32), new string(actualArray));
            rng.WeightedChoice(".o".ToCharArray(), c => c switch { '.' => 1, _ => 0 }, actualArray);
            AssertAreEqual(new string('.', 32), new string(actualArray));
        }

        public static void CharacteristicWeightedChoiceArraySelector()
        {
            var rng = CreateStandardSeed();
            const int trial = 1 << 24;
            var buffer = new string[16];
            var bucket = new Dictionary<string, int>();

            var source = new string[] { "bronze", "silver", "gold", "platinum" };
            static double weightSelector(string c) => c switch { "bronze" => 0.90, "silver" => 0.09, "gold" => 0.009, _ => 0.001 };

            for (int i = 0; i < trial; i++)
            {
                rng.WeightedChoice(source, weightSelector, buffer);

                for (int k = 0; k < buffer.Length; k++)
                {
                    if (bucket.ContainsKey(buffer[k]))
                        bucket[buffer[k]]++;
                    else
                        bucket.Add(buffer[k], 1);
                }
            }

            for (int i = 0; i < source.Length; i++)
            {
                var expected = trial * buffer.Length * weightSelector(source[i]);
                var actual = bucket[source[i]];
                Assert(expected * 0.99 < actual && actual < expected * 1.01,
                    expected, actual);
            }
        }

        public static void ReproductionWeightedChoiceArrayInt()
        {
            var rng = CreateStandardSeed();

            string expected = "....oO.o.......O..o..o.....oo.o.";
            var actualArray = rng.WeightedChoice(".oO".ToCharArray(), new double[] { 78.5, 18.5, 3.0 }, 32);
            string actual = new string(actualArray);
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice((char[])null!, new double[1] { 100.0 }, 32));
            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice(".".ToCharArray(), (double[])null!, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(new char[0], new double[0], 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".oO".ToCharArray(), new double[1] { 100.0 }, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".".ToCharArray(), new double[] { 0 }, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".ToCharArray(), new double[] { -1, 2 }, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".ToCharArray(), new double[] { double.NaN, double.NaN }, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".oO".ToCharArray(), new double[] { 78.5, 18.5, 3.0 }, -1));

            AssertAreEqual(new char[0], rng.WeightedChoice(".oO".ToCharArray(), new double[] { 78.5, 18.5, 3.0 }, 0));
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".".ToCharArray(), new double[] { 1 }, 32)));
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".o".ToCharArray(), new double[] { 1, 0 }, 32)));
        }

        public static void CharacteristicWeightedChoiceArrayInt()
        {
            var rng = CreateStandardSeed();
            const int trial = 1 << 20;
            const int bufferLength = 16;
            var bucket = new Dictionary<char, int>();

            var source = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            var weights = Enumerable.Range(1, 26).Select(i => (double)i * i).ToArray();

            for (int i = 0; i < trial; i++)
            {
                var buffer = rng.WeightedChoice(source, weights, bufferLength);

                for (int k = 0; k < buffer.Length; k++)
                {
                    if (bucket.ContainsKey(buffer[k]))
                        bucket[buffer[k]]++;
                    else
                        bucket.Add(buffer[k], 1);
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                var expected = trial * bufferLength * weights[i] / 6201;
                var actual = bucket[source[i]];
                Assert(expected * 0.99 < actual && actual < expected * 1.01,
                    expected, actual);
            }
        }

        public static void ReproductionWeightedChoiceArrayIntSelector()
        {
            var rng = CreateStandardSeed();

            string expected = "....oO.o.......O..o..o.....oo.o.";
            var actualArray = rng.WeightedChoice(".oO".ToCharArray(), c => c switch { '.' => 78.5, 'o' => 18.5, _ => 3.0 }, 32);
            string actual = new string(actualArray);
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice((char[])null!, c => c, 32));
            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice(".oO".ToCharArray(), (Func<char, double>)null!, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(new char[0], c => c, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".".ToCharArray(), c => 0, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".ToCharArray(), c => -1, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".ToCharArray(), c => double.NaN, 32));
            AssertThrows<ArgumentException>(() => rng.WeightedChoice(".o".ToCharArray(), c => 1, -1));

            AssertAreEqual(new char[0], rng.WeightedChoice(".oO".ToCharArray(), c => c switch { '.' => 78.5, 'o' => 18.5, _ => 3.0 }, 0));
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".".ToCharArray(), c => 1, 32)));
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".o".ToCharArray(), c => c switch { '.' => 1, _ => 0 }, 32)));
        }

        public static void CharacteristicWeightedChoiceArrayIntSelector()
        {
            var rng = CreateStandardSeed();
            const int trial = 1 << 20;
            const int bufferLength = 16;
            var bucket = new Dictionary<char, int>();

            var source = "0123456789".ToCharArray();
            static double weightSelector(char c) => c - '0' + 10;

            for (int i = 0; i < trial; i++)
            {
                var buffer = rng.WeightedChoice(source, weightSelector, bufferLength);

                for (int k = 0; k < buffer.Length; k++)
                {
                    if (bucket.ContainsKey(buffer[k]))
                        bucket[buffer[k]]++;
                    else
                        bucket.Add(buffer[k], 1);
                }
            }

            for (int i = 0; i < source.Length; i++)
            {
                var expected = trial * bufferLength * weightSelector(source[i]) / 145;
                var actual = bucket[source[i]];
                Assert(expected * 0.99 < actual && actual < expected * 1.01,
                    expected, actual);
            }
        }

        public static void ReproductionWeightedChoiceIterator()
        {
            var rng = CreateStandardSeed();

            string expected = "....oO.o.......O..o..o.....oo.o.";
            var actualArray = rng.WeightedChoice(".oO", new double[] { 78.5, 18.5, 3.0 }).Take(32).ToArray();
            string actual = new string(actualArray);
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice((char[])null!, new double[1] { 100.0 }));
            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice(".", (double[])null!));

            {
                var iter = rng.WeightedChoice(new char[0], new double[0]);
                AssertThrows<ArgumentException>(() => iter.Take(1).ToArray());
            }
            {
                var iter = rng.WeightedChoice(".oO", new double[1] { 100.0 });
                AssertThrows<ArgumentException>(() => iter.Take(1).ToArray());
            }
            {
                var iter = rng.WeightedChoice(".", new double[] { 0 });
                AssertThrows<ArgumentException>(() => iter.Take(1).ToArray());
            }
            {
                var iter = rng.WeightedChoice(".o", new double[] { -1, 2 });
                AssertThrows<ArgumentException>(() => iter.Take(1).ToArray());
            }
            {
                var iter = rng.WeightedChoice(".o", new double[] { double.NaN, double.NaN });
                AssertThrows<ArgumentException>(() => iter.Take(1).ToArray());
            }

            AssertAreEqual(new char[0], rng.WeightedChoice(".oO".ToCharArray(), new double[] { 78.5, 18.5, 3.0 }).Take(0).ToArray());
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".".ToCharArray(), new double[] { 1 }).Take(32).ToArray()));
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".o".ToCharArray(), new double[] { 1, 0 }).Take(32).ToArray()));
        }

        public static void CharacteristicWeightedChoiceIterator()
        {
            var rng = CreateStandardSeed();
            const int trial = 1 << 24;
            var bucket = new Dictionary<char, int>();

            var source = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var weights = Enumerable.Range(1, 52).Select(i => (i & 0xf) + 1.0).ToArray();

            foreach (var element in rng.WeightedChoice(source, weights).Take(trial))
            {
                if (bucket.ContainsKey(element))
                    bucket[element]++;
                else
                    bucket.Add(element, 1);
            }

            for (int i = 0; i < weights.Length; i++)
            {
                var expected = trial * weights[i] / 422;
                var actual = bucket[source[i]];
                Assert(expected * 0.99 < actual && actual < expected * 1.01,
                    expected, actual);
            }
        }

        public static void ReproductionWeightedChoiceIteratorSelector()
        {
            var rng = CreateStandardSeed();

            string expected = "....oO.o.......O..o..o.....oo.o.";
            var actualArray = rng.WeightedChoice(".oO", c => c switch { '.' => 78.5, 'o' => 18.5, _ => 3.0 }).Take(32).ToArray();
            string actual = new string(actualArray);
            AssertAreEqual(expected, actual);

            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice((char[])null!, c => c));
            AssertThrows<ArgumentNullException>(() => rng.WeightedChoice(".", (Func<char, double>)null!));

            {
                var iter = rng.WeightedChoice(new char[0], c => c);
                AssertThrows<ArgumentException>(() => iter.Take(1).ToArray());
            }
            {
                var iter = rng.WeightedChoice(".", c => 0);
                AssertThrows<ArgumentException>(() => iter.Take(1).ToArray());
            }
            {
                var iter = rng.WeightedChoice(".o", c => -1);
                AssertThrows<ArgumentException>(() => iter.Take(1).ToArray());
            }
            {
                var iter = rng.WeightedChoice(".o", c => double.NaN);
                AssertThrows<ArgumentException>(() => iter.Take(1).ToArray());
            }

            AssertAreEqual(new char[0], rng.WeightedChoice(".oO".ToCharArray(), c => c switch { '.' => 78.5, 'o' => 18.5, _ => 3.0 }).Take(0).ToArray());
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".".ToCharArray(), c => 1).Take(32).ToArray()));
            AssertAreEqual(new string('.', 32), new string(rng.WeightedChoice(".o".ToCharArray(), c => c switch { '.' => 1, _ => 0 }).Take(32).ToArray()));
        }

        public static void CharacteristicWeightedChoiceIteratorSelector()
        {
            var rng = CreateStandardSeed();
            const int trial = 1 << 24;
            var bucket = new Dictionary<char, int>();

            var source = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var weights = Enumerable.Range(1, 52).Select(i => (i & 0xf) + 1.0).ToArray();

            foreach (var element in rng.WeightedChoice(source, weights).Take(trial))
            {
                if (bucket.ContainsKey(element))
                    bucket[element]++;
                else
                    bucket.Add(element, 1);
            }

            for (int i = 0; i < weights.Length; i++)
            {
                var expected = trial * weights[i] / 422;
                var actual = bucket[source[i]];
                Assert(expected * 0.99 < actual && actual < expected * 1.01,
                    expected, actual);
            }
        }


        public static void ReproductionNextInsideCircle()
        {
            var rng = CreateStandardSeed();
            var expected = new Vector2[] {
                new Vector2(0.22311234f, 0.47296375f),
                new Vector2(-0.21911883f, -0.35596907f),
                new Vector2(-0.028905809f, 0.6013443f),
                new Vector2(-0.0693745f, 0.1684435f),
                new Vector2(0.16468823f, -0.74731153f),
                new Vector2(0.8405609f, 0.078468144f),
                new Vector2(-0.84012f, 0.24759412f),
                new Vector2(0.0042936206f, 0.46517754f),
                new Vector2(-0.7714495f, -0.2795261f),
                new Vector2(-0.6795365f, 0.5490392f),
                new Vector2(0.64366275f, 0.3662355f),
                new Vector2(-0.56756073f, -0.4287578f),
                new Vector2(-0.7155213f, -0.34982073f),
                new Vector2(-0.46442288f, 0.11996335f),
                new Vector2(-0.6661653f, 0.31652945f),
                new Vector2(0.18284178f, 0.07452297f),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextInsideCircle()).ToArray();
            AssertAreEqual(expected, actual);

            rng = CreateZeroSeed();
            AssertAreEqual(new Vector2(0f, 0f), rng.NextInsideCircle());
        }

        public static void CharacteristicNextInsideCircle()
        {
            const int trial = 1 << 24;
            var rng = CreateStandardSeed();

            var bucketX = new int[36];
            var bucketL = new int[36];

            double theoreticalAverage = bucketX.Length / 2;
            double theoreticalVarianceX = (bucketX.Length * bucketX.Length) / 12.0;
            double theoreticalVarianceL = (bucketL.Length * bucketL.Length) / 12.0;
            double varianceX = 0;
            double varianceL = 0;

            for (int i = 0; i < trial; i++)
            {
                var vector = rng.NextInsideCircle();
                var vectorLength = vector.Length();

                Assert(vectorLength < 1.0000002f, 1f, vectorLength);
                ShouldBe(vectorLength <= 1f, 1f, vectorLength);

                int indexX = (int)Math.Floor((MathF.Atan2(vector.Y, vector.X) + MathF.PI) * (36f / MathF.Tau)) % bucketX.Length;
                bucketX[indexX]++;
                int indexL = (int)Math.Floor(vectorLength * vectorLength * bucketL.Length) % bucketL.Length;
                bucketL[indexL]++;

                varianceX = varianceX * (i / (i + 1.0)) + (theoreticalAverage - indexX) * (theoreticalAverage - indexX) / (i + 1.0);
                varianceL = varianceL * (i / (i + 1.0)) + (theoreticalAverage - indexL) * (theoreticalAverage - indexL) / (i + 1.0);
            }

            Assert(theoreticalVarianceX * 0.9 < varianceX && varianceX < theoreticalVarianceX * 1.1, theoreticalVarianceX, varianceX);
            ShouldBe(theoreticalVarianceX * 0.99 < varianceX && varianceX < theoreticalVarianceX * 1.01, theoreticalVarianceX, varianceX);
            Assert(theoreticalVarianceL * 0.9 < varianceL && varianceL < theoreticalVarianceL * 1.1, theoreticalVarianceL, varianceL);
            ShouldBe(theoreticalVarianceL * 0.99 < varianceL && varianceL < theoreticalVarianceL * 1.01, theoreticalVarianceL, varianceL);
        }

        public static void ReproductionNextOnCircle()
        {
            var rng = CreateStandardSeed();
            var expected = new Vector2[] {
                new Vector2(0.42664406f, 0.9044196f),
                new Vector2(-0.5242032f, -0.85159326f),
                new Vector2(-0.048013214f, 0.9988467f),
                new Vector2(-0.38082212f, 0.9246483f),
                new Vector2(0.2152104f, -0.97656775f),
                new Vector2(0.9956709f, 0.092948f),
                new Vector2(-0.9592108f, 0.2826917f),
                new Vector2(0.009229676f, 0.9999574f),
                new Vector2(-0.9401846f, -0.3406654f),
                new Vector2(-0.77783895f, 0.6284637f),
                new Vector2(0.86915594f, 0.49453813f),
                new Vector2(-0.79791194f, -0.60277414f),
                new Vector2(-0.89837927f, -0.4392206f),
                new Vector2(-0.96822065f, 0.25009748f),
                new Vector2(-0.9032244f, 0.42916846f),
                new Vector2(0.9260359f, 0.37743533f),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextOnCircle()).ToArray();
            AssertAreEqual(expected, actual);

            rng = CreateZeroSeed();
            AssertAreEqual(new Vector2(0.054783203f, 0.9984983f), rng.NextOnCircle());
        }

        public static void CharacteristicNextOnCircle()
        {
            const int trial = 1 << 24;
            var rng = CreateStandardSeed();

            var bucketX = new int[36];

            double theoreticalAverage = bucketX.Length / 2;
            double theoreticalVariance = (bucketX.Length * bucketX.Length) / 12.0;
            double variance = 0;

            for (int i = 0; i < trial; i++)
            {
                var vector = rng.NextOnCircle();
                var vectorLength = vector.Length();

                Assert(0.999999f <= vectorLength && vectorLength <= 1.000001f, 1f, vectorLength);
                ShouldBe(0.9999998f <= vectorLength && vectorLength <= 1.0000001f, 1f, vectorLength);

                int index = (int)Math.Floor((MathF.Atan2(vector.Y, vector.X) + MathF.PI) * (36f / MathF.Tau));
                bucketX[index]++;

                variance = variance * (i / (i + 1.0)) + (theoreticalAverage - index) * (theoreticalAverage - index) / (i + 1.0);
            }

            Assert(theoreticalVariance * 0.9 < variance && variance < theoreticalVariance * 1.1, theoreticalVariance, variance);
            ShouldBe(theoreticalVariance * 0.99 < variance && variance < theoreticalVariance * 1.01, theoreticalVariance, variance);
        }

        public static void ReproductionNextInsideSphere()
        {
            var rng = CreateStandardSeed();
            var expected = new Vector3[] {
                new Vector3(0.22311234f, 0.47296375f, 0.516234f),
                new Vector3(-0.21911883f, -0.35596907f, 0.34685922f),
                new Vector3(-0.028905809f, 0.6013443f, 0.38739163f),
                new Vector3(-0.0693745f, 0.1684435f, 0.35015702f),
                new Vector3(-0.6795365f, 0.5490392f, -0.3421601f),
                new Vector3(-0.7155213f, -0.34982073f, -0.023554623f),
                new Vector3(-0.46442288f, 0.11996335f, -0.02824384f),
                new Vector3(-0.8749426f, -0.12573391f, -0.017237067f),
                new Vector3(0.24012554f, 0.711319f, 0.26484352f),
                new Vector3(0.5493138f, 0.010919452f, 0.5211549f),
                new Vector3(-0.7002188f, -0.40275168f, -0.47938967f),
                new Vector3(0.14570397f, -0.39056742f, -0.45952886f),
                new Vector3(0.12932706f, 0.79628676f, 0.082844615f),
                new Vector3(-0.5823629f, 0.42519838f, 0.04076886f),
                new Vector3(-0.055678487f, 0.47784686f, 0.63297844f),
                new Vector3(0.8645374f, 0.29777473f, 0.16730535f),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextInsideSphere()).ToArray();
            AssertAreEqual(expected, actual);

            rng = CreateZeroSeed();
            AssertAreEqual(new Vector3(0f, 0f, 0f), rng.NextInsideSphere());
        }

        public static void CharacteristicNextInsideSphere()
        {
            const int trial = 1 << 24;
            var rng = CreateStandardSeed();

            var bucketX = new int[36];
            var bucketY = new int[36];
            var bucketL = new int[36];

            double theoreticalAverage = bucketX.Length / 2;
            double theoreticalVarianceX = (bucketX.Length * bucketX.Length) / 12.0;
            double theoreticalVarianceY = (bucketY.Length * bucketY.Length) / 12.0;
            double theoreticalVarianceL = (bucketL.Length * bucketL.Length) / 12.0;
            double varianceX = 0;
            double varianceY = 0;
            double varianceL = 0;

            for (int i = 0; i < trial; i++)
            {
                var vector = rng.NextInsideSphere();
                var vectorLength = vector.Length();

                Assert(vectorLength < 1.0000002f, 1f, vectorLength);
                ShouldBe(vectorLength <= 1f, 1f, vectorLength);

                int indexX = (int)Math.Floor((MathF.Atan2(vector.Y, vector.X) + MathF.PI) * (36f / MathF.Tau)) % bucketX.Length;
                bucketX[indexX]++;
                int indexY = (int)Math.Floor((MathF.Atan2(vector.Z, vector.Y) + MathF.PI) * (36f / MathF.Tau)) % bucketY.Length;
                bucketY[indexY]++;
                int indexL = (int)Math.Floor(vectorLength * vectorLength * vectorLength * bucketL.Length) % bucketL.Length;
                bucketL[indexL]++;

                varianceX = varianceX * (i / (i + 1.0)) + (theoreticalAverage - indexX) * (theoreticalAverage - indexX) / (i + 1.0);
                varianceY = varianceY * (i / (i + 1.0)) + (theoreticalAverage - indexY) * (theoreticalAverage - indexY) / (i + 1.0);
                varianceL = varianceL * (i / (i + 1.0)) + (theoreticalAverage - indexL) * (theoreticalAverage - indexL) / (i + 1.0);
            }

            Assert(theoreticalVarianceX * 0.9 < varianceX && varianceX < theoreticalVarianceX * 1.1, theoreticalVarianceX, varianceX);
            ShouldBe(theoreticalVarianceX * 0.99 < varianceX && varianceX < theoreticalVarianceX * 1.01, theoreticalVarianceX, varianceX);
            Assert(theoreticalVarianceY * 0.9 < varianceY && varianceY < theoreticalVarianceY * 1.1, theoreticalVarianceY, varianceY);
            ShouldBe(theoreticalVarianceY * 0.99 < varianceY && varianceY < theoreticalVarianceY * 1.01, theoreticalVarianceY, varianceY);
            Assert(theoreticalVarianceL * 0.9 < varianceL && varianceL < theoreticalVarianceL * 1.1, theoreticalVarianceL, varianceL);
            ShouldBe(theoreticalVarianceL * 0.99 < varianceL && varianceL < theoreticalVarianceL * 1.01, theoreticalVarianceL, varianceL);
        }

        public static void ReproductionNextOnSphere()
        {
            var rng = CreateStandardSeed();
            var expected = new Vector3[] {
                new Vector3(0.38034633f, 0.8062755f, 0.45305234f),
                new Vector3(-0.39811498f, -0.6467569f, 0.65054595f),
                new Vector3(-0.046160668f, 0.9603071f, 0.27509904f),
                new Vector3(-0.13642731f, 0.33124986f, 0.93362796f),
                new Vector3(0.21203333f, -0.96215105f, -0.17119348f),
                new Vector3(0.90108764f, 0.08411844f, -0.4253999f),
                new Vector3(-0.8108712f, 0.23897411f, -0.534209f),
                new Vector3(0.007601486f, 0.82355684f, 0.5671829f),
                new Vector3(-0.8819269f, -0.31955636f, -0.3465383f),
                new Vector3(-0.6613341f, 0.53433233f, -0.52642775f),
                new Vector3(0.8650692f, 0.4922128f, -0.09686029f),
                new Vector3(-0.7978552f, -0.60273135f, -0.011916876f),
                new Vector3(-0.8653428f, -0.42306897f, -0.26869047f),
                new Vector3(-0.81501657f, 0.2105239f, 0.53984034f),
                new Vector3(-0.8997256f, 0.427506f, -0.08793414f),
                new Vector3(0.35848466f, 0.1461118f, 0.92203045f),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextOnSphere()).ToArray();
            AssertAreEqual(expected, actual);

            rng = CreateZeroSeed();
            AssertAreEqual(new Vector3(0f, 0f, 1f), rng.NextOnSphere());
        }

        public static void CharacteristicNextOnSphere()
        {
            const int trial = 1 << 24;
            var rng = CreateStandardSeed();

            var bucketX = new int[36];
            var bucketY = new int[36];

            double theoreticalAverage = bucketX.Length / 2;
            double theoreticalVarianceX = (bucketX.Length * bucketX.Length) / 12.0;
            double theoreticalVarianceY = (bucketY.Length * bucketY.Length) / 12.0;
            double varianceX = 0;
            double varianceY = 0;

            for (int i = 0; i < trial; i++)
            {
                var vector = rng.NextOnSphere();
                var vectorLength = vector.Length();

                Assert(0.999999f <= vectorLength && vectorLength <= 1.000001f, 1f, vectorLength);
                ShouldBe(0.9999998f <= vectorLength && vectorLength <= 1.0000001f, 1f, vectorLength);

                int indexX = (int)Math.Floor((MathF.Atan2(vector.Y, vector.X) + MathF.PI) * (36f / MathF.Tau)) % bucketX.Length;
                bucketX[indexX]++;
                int indexY = (int)Math.Floor((MathF.Atan2(vector.Z, vector.Y) + MathF.PI) * (36f / MathF.Tau)) % bucketY.Length;
                bucketY[indexY]++;

                varianceX = varianceX * (i / (i + 1.0)) + (theoreticalAverage - indexX) * (theoreticalAverage - indexX) / (i + 1.0);
                varianceY = varianceY * (i / (i + 1.0)) + (theoreticalAverage - indexY) * (theoreticalAverage - indexY) / (i + 1.0);
            }

            Assert(theoreticalVarianceX * 0.9 < varianceX && varianceX < theoreticalVarianceX * 1.1, theoreticalVarianceX, varianceX);
            ShouldBe(theoreticalVarianceX * 0.99 < varianceX && varianceX < theoreticalVarianceX * 1.01, theoreticalVarianceX, varianceX);
            Assert(theoreticalVarianceY * 0.9 < varianceY && varianceY < theoreticalVarianceY * 1.1, theoreticalVarianceY, varianceY);
            ShouldBe(theoreticalVarianceY * 0.99 < varianceY && varianceY < theoreticalVarianceY * 1.01, theoreticalVarianceY, varianceY);
        }

        public static void ReproductionNextQuaternion()
        {
            var rng = CreateStandardSeed();
            var expected = new Quaternion[] {
                new Quaternion(0.223112345f, 0.47296375f, 0.6689007f, 0.528297246f),
                new Quaternion(-0.219118834f, -0.355969071f, 0.4429192f, 0.793155432f),
                new Quaternion(-0.0289058089f, 0.6013443f, 0.5201347f, 0.605813f),
                new Quaternion(-0.84012f, 0.247594118f, 0.384173065f, -0.2920729f),
                new Quaternion(-0.6795365f, 0.5490392f, -0.486600876f, -0.00239015743f),
                new Quaternion(-0.7155213f, -0.349820733f, -0.0169679038f, -0.6044558f),
                new Quaternion(-0.464422882f, 0.119963348f, -0.0342943445f, -0.8767805f),
                new Quaternion(-0.8749426f, -0.125733912f, -0.0102895545f, 0.4675046f),
                new Quaternion(0.240125537f, 0.711319f, 0.223757923f, 0.6215282f),
                new Quaternion(0.5493138f, 0.0109194517f, 0.6563146f, 0.517094f),
                new Quaternion(0.6159137f, 0.5442455f, 0.554663241f, -0.1295987f),
                new Quaternion(-0.7002188f, -0.402751684f, -0.368496239f, 0.460103482f),
                new Quaternion(0.145703971f, -0.390567422f, -0.574069858f, 0.7047491f),
                new Quaternion(0.129327059f, 0.796286762f, 0.142730251f, 0.573437035f),
                new Quaternion(-0.6613508f, 0.5289816f, -0.5164098f, 0.126942709f),
                new Quaternion(-0.5823629f, 0.425198376f, 0.0412954353f, -0.6916318f),
            };
            var actual = Enumerable.Range(0, 16).Select(_ => rng.NextQuaternion()).ToArray();
            AssertAreEqual(expected, actual);

            rng = CreateZeroSeed();
            AssertAreEqual(new Quaternion(0.038386524f, 0.69964653f, 0.07620768f, 0.7093755f), rng.NextQuaternion());
        }

        public static void CharacteristicNextQuaternion()
        {
            const int trial = 1 << 24;
            var rng = CreateStandardSeed();

            {
                var bucketX = new int[36];
                var bucketY = new int[36];
                var bucketZ = new int[36];

                double theoreticalAverage = bucketX.Length / 2;
                double theoreticalVarianceX = (bucketX.Length * bucketX.Length) / 12.0;
                double theoreticalVarianceY = (bucketY.Length * bucketY.Length) / 12.0;
                double theoreticalVarianceZ = (bucketZ.Length * bucketZ.Length) / 12.0;
                double varianceX = 0;
                double varianceY = 0;
                double varianceZ = 0;

                for (int i = 0; i < trial; i++)
                {
                    var vector = rng.NextQuaternion();
                    var vectorLength = vector.Length();

                    Assert(0.999999f <= vectorLength && vectorLength <= 1.000001f, 1f, vectorLength);
                    ShouldBe(0.9999998f <= vectorLength && vectorLength <= 1.0000001f, 1f, vectorLength);

                    int indexX = (int)Math.Floor((MathF.Atan2(vector.Y, vector.X) + MathF.PI) * (36f / MathF.Tau)) % bucketX.Length;
                    bucketX[indexX]++;
                    int indexY = (int)Math.Floor((MathF.Atan2(vector.Z, vector.Y) + MathF.PI) * (36f / MathF.Tau)) % bucketY.Length;
                    bucketY[indexY]++;
                    int indexZ = (int)Math.Floor((MathF.Atan2(vector.W, vector.Z) + MathF.PI) * (36f / MathF.Tau)) % bucketZ.Length;
                    bucketZ[indexZ]++;

                    varianceX = varianceX * (i / (i + 1.0)) + (theoreticalAverage - indexX) * (theoreticalAverage - indexX) / (i + 1.0);
                    varianceY = varianceY * (i / (i + 1.0)) + (theoreticalAverage - indexY) * (theoreticalAverage - indexY) / (i + 1.0);
                    varianceZ = varianceZ * (i / (i + 1.0)) + (theoreticalAverage - indexZ) * (theoreticalAverage - indexZ) / (i + 1.0);
                }


                Assert(theoreticalVarianceX * 0.9 < varianceX && varianceX < theoreticalVarianceX * 1.1, theoreticalVarianceX, varianceX);
                ShouldBe(theoreticalVarianceX * 0.99 < varianceX && varianceX < theoreticalVarianceX * 1.01, theoreticalVarianceX, varianceX);
                Assert(theoreticalVarianceY * 0.9 < varianceY && varianceY < theoreticalVarianceY * 1.1, theoreticalVarianceY, varianceY);
                ShouldBe(theoreticalVarianceY * 0.99 < varianceY && varianceY < theoreticalVarianceY * 1.01, theoreticalVarianceY, varianceY);
                Assert(theoreticalVarianceZ * 0.9 < varianceZ && varianceZ < theoreticalVarianceZ * 1.1, theoreticalVarianceZ, varianceZ);
                ShouldBe(theoreticalVarianceZ * 0.99 < varianceZ && varianceZ < theoreticalVarianceZ * 1.01, theoreticalVarianceZ, varianceZ);
            }
        }


        public static void ReproductionShared()
        {
            Assert(Shared != null);
        }

        public static void CharacteristicShared()
        {
            var states = Enumerable.Range(0, 4).AsParallel().WithDegreeOfParallelism(4).Select(_ => Shared.GetState()).ToArray();
            ShouldBeEqual(4, states.Distinct().Count());
        }



        // -------- helper function --------

        /// <summary>
        /// Creates the instance with a common (but specific) seed
        /// </summary>
        private static Culumi CreateStandardSeed()
        {
            var seedArray = (stackalloc ulong[4]);
            ulong seed = 401;
            for (int i = 0; i < seedArray.Length; i++)
            {
                seedArray[i] = seed = seed * 6364136223846793005 + 1442695040888963407;
            }

            return new Culumi(Vector128.Create(seedArray[0], seedArray[1]), Vector128.Create(seedArray[2], seedArray[3]));
        }

        /// <summary>
        /// Creates the instance that returns {0, 0} first
        /// </summary>
        private static Culumi CreateZeroSeed()
        {
            return new Culumi(
                Vector128.Create(0x8000000000008000u, 0x8000000000008000u),
                Vector128.Create(0x8000000000000000u, 0x8000000000000000u));
        }

        /// <summary>
        /// Creates the instance that returns {1, 1}, {1, 1} first
        /// </summary>
        /// <returns></returns>
        private static Culumi CreateOneSeed()
        {
            return new Culumi(
               Vector128.Create(0x000122f722f6ffffu, 0x4d75916591654d74u),
               Vector128.Create(0xf1526e846e840eaeu, 0xd947b74cb74dd944u));
        }

        /// <summary>
        /// Creates the instance that returns {~0ul, ~0ul} first
        /// </summary>
        private static Culumi CreateAllBitsSetSeed()
        {
            return new Culumi(
                Vector128.Create(~0ul, ~0ul),
                Vector128.Create(0ul, 0ul));
        }

        /// <summary>
        /// Creates the instance that returns {next} first
        /// </summary>
        private static Culumi CreateNextSeed(Vector128<ulong> next)
        {
            var b = Vector128.Create(0xcccccccccccccccc, 0xcccccccccccccccc);
            var a = Vector128.Shuffle((next - b).AsByte(), Vector128.Create(0x0100030205040706, 0x09080b0a0d0c0f0e).AsByte()).AsUInt64() - b;
            return new Culumi(a, b);
        }


        private static void Assert([DoesNotReturnIf(false)] bool expectedTrue, [CallerMemberName] string? memberName = null, [CallerArgumentExpression(nameof(expectedTrue))] string? expression = null)
        {
            if (!expectedTrue)
            {
                Console.WriteLine($"\u001b[91m[X] {memberName}: assertion failed: {expression}\u001b[0m");

                throw new InvalidOperationException("assertion failed");
            }
        }

        private static void Assert<T>([DoesNotReturnIf(false)] bool expectedTrue, T expected, T actual, [CallerMemberName] string? memberName = null, [CallerArgumentExpression(nameof(expectedTrue))] string? expression = null)
        {
            if (!expectedTrue)
            {
                Console.WriteLine($"\u001b[91m[X] {memberName}: assertion failed: expected <{expected}> but <{actual}>: {expression}\u001b[0m");

                throw new InvalidOperationException("assertion failed");
            }
        }

        private static void AssertAreEqual<T>(T expected, T actual, [CallerMemberName] string? memberName = null, [CallerArgumentExpression(nameof(actual))] string? expression = null)
            where T : IEquatable<T>
        {
            if (!expected.Equals(actual))
            {
                Console.WriteLine($"\u001b[91m[X] {memberName}: assertion failed: expected <{expected}> but <{actual}>: {expression}\u001b[0m");

                throw new InvalidOperationException("assertion failed");
            }
        }

        private static void AssertAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, [CallerMemberName] string? memberName = null, [CallerArgumentExpression(nameof(actual))] string? expression = null)
        {
            if (!expected.SequenceEqual(actual))
            {
                Console.WriteLine($"\u001b[91m[X] {memberName}: assertion failed: expected [{string.Join(", ", expected)}] but [{string.Join(", ", actual)}]: {expression}\u001b[0m");

                throw new InvalidOperationException("assertion failed");
            }
        }

        private static void AssertThrows<T>(Action act, [CallerMemberName] string? memberName = null, [CallerArgumentExpression(nameof(act))] string? expression = null)
            where T : Exception
        {
            try
            {
                act();
            }
            catch (T)
            {
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\u001b[91m[X] {memberName}: assertion failed - thrown is not {typeof(T).Name}: {expression}\u001b[0m");

                throw new InvalidOperationException($"assertion failed. thrown is not {typeof(T).Name}", ex);
            }

            Console.WriteLine($"\u001b[91m[X] {memberName}: assertion failed - no {typeof(T).Name} thrown: {expression}\u001b[0m");

            throw new InvalidOperationException($"assertion failed. no {typeof(T).Name} thrown\u001b[0m");
        }

        private static void ShouldBe(bool expectedTrue, [CallerMemberName] string? memberName = null, [CallerArgumentExpression(nameof(expectedTrue))] string? expression = null)
        {
            if (!expectedTrue)
            {
                Console.WriteLine($"\u001b[93m[!] {memberName}: {expression}\u001b[0m");
            }
        }

        private static void ShouldBe<T>(bool expectedTrue, T expected, T actual, [CallerMemberName] string? memberName = null, [CallerArgumentExpression(nameof(expectedTrue))] string? expression = null)
        {
            if (!expectedTrue)
            {
                Console.WriteLine($"\u001b[93m[!] {memberName}: expected {expected} but {actual}: {expression}\u001b[0m");
            }
        }

        private static void ShouldBeEqual<T>(T expected, T actual, [CallerMemberName] string? memberName = null, [CallerArgumentExpression(nameof(actual))] string? expression = null)
            where T : IEquatable<T>
        {
            if (!expected.Equals(actual))
            {
                Console.WriteLine($"\u001b[93m[!] {memberName}: expected {expected} but {actual}: {expression}\u001b[0m");
            }
        }

    }
}

