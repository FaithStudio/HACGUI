﻿using LibHac;

namespace HACGUI.Utilities
{
    // https://gist.github.com/roblabla/d8358ab058bbe3b00614740dcba4f208
    // hashes came from here
    public class NintendoKeys
    {
        public static readonly byte[]
            // All taken from key_derivation.c in biskeydump (so if sharing these keys are illegal, inform raj too lol)
            DevSpecificAesKeySource = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
            DevSpecificAesKeyCtr = new byte[] { 0x3C, 0xD5, 0x92, 0xEC, 0x68, 0x31, 0x4A, 0x06, 0xD4, 0x1B, 0x0C, 0xD9, 0xF6, 0x2E, 0xD9, 0xE9 },
            DevSpecificAesKeyMask = new byte[] { 0xAC, 0xCA, 0x9A, 0xCA, 0xFF, 0x2E, 0xB9, 0x22, 0xCC, 0x1F, 0x4F, 0xAD, 0xDD, 0x77, 0x21, 0x1E },
            RetailSpecificAesKeySource = new byte[] { 0xE2, 0xD6, 0xB8, 0x7A, 0x11, 0x9C, 0xB8, 0x80, 0xE8, 0x22, 0x88, 0x8A, 0x46, 0xFB, 0xA1, 0x95 },
            PersonalizedAesKeySource = new byte[] { 0x89, 0x61, 0x5E, 0xE0, 0x5C, 0x31, 0xB6, 0x80, 0x5F, 0xE5, 0x8F, 0x3D, 0xA2, 0x4F, 0x7A, 0xA8 },
            BisKekSource = new byte[] { 0x34, 0xC1, 0xA0, 0xC4, 0x82, 0x58, 0xF8, 0xB4, 0xFA, 0x9E, 0x5E, 0x6A, 0xDA, 0xFC, 0x7E, 0x4F },
            DeviceKeySource = new byte[] { 0x4F, 0x02, 0x5F, 0x0E, 0xB6, 0x6D, 0x11, 0x0E, 0xDC, 0x32, 0x7D, 0x41, 0x86, 0xC2, 0xF4, 0x78 },

            // Package 1 keys | Taken from KeyCollection.cpp in Lockpick
            KeyblobMacKeySource = new byte[] { 0x59, 0xC7, 0xFB, 0x6F, 0xBE, 0x9B, 0xBE, 0x87, 0x65, 0x6B, 0x15, 0xC0, 0x53, 0x73, 0x36, 0xA5 },
            MasterKeySource = new byte[] { 0xD8, 0xA2, 0x41, 0x0A, 0xC6, 0xC5, 0x90, 0x01, 0xC6, 0x1D, 0x6A, 0x26, 0x7C, 0x51, 0x3F, 0x3C },

            // TrustZone Keys
            Pkg2KeySource = new byte[] { 0xFB, 0x8B, 0x6A, 0x9C, 0x79, 0x00, 0xC8, 0x49, 0xEF, 0xD2, 0x4D, 0x85, 0x4D, 0x30, 0xA0, 0xC7 },
            TitleKekSource = new byte[] { 0x1E, 0xDC, 0x7B, 0x3B, 0x60, 0xE6, 0xB4, 0xD8, 0x78, 0xB8, 0x17, 0x15, 0x98, 0x5E, 0x62, 0x9B },

            // Package 2 hashes
            KeyAreaKeyApplicationSourceHash = new byte[] { 0x04, 0xad, 0x66, 0x14, 0x3c, 0x72, 0x6b, 0x2a, 0x13, 0x9f, 0xb6, 0xb2, 0x11, 0x28, 0xb4, 0x6f, 0x56, 0xc5, 0x53, 0xb2, 0xb3, 0x88, 0x71, 0x10, 0x30, 0x42, 0x98, 0xd8, 0xd0, 0x09, 0x2d, 0x9e },
            KeyAreaKeyOceanSourceHash = new byte[] { 0xfd, 0x43, 0x40, 0x00, 0xc8, 0xff, 0x2b, 0x26, 0xf8, 0xe9, 0xa9, 0xd2, 0xd2, 0xc1, 0x2f, 0x6b, 0xe5, 0x77, 0x3c, 0xbb, 0x9d, 0xc8, 0x63, 0x00, 0xe1, 0xbd, 0x99, 0xf8, 0xea, 0x33, 0xa4, 0x17 },
            KeyAreaKeySystemSourceHash = new byte[] { 0x1f, 0x17, 0xb1, 0xfd, 0x51, 0xad, 0x1c, 0x23, 0x79, 0xb5, 0x8f, 0x15, 0x2c, 0xa4, 0x91, 0x2e, 0xc2, 0x10, 0x64, 0x41, 0xe5, 0x17, 0x22, 0xf3, 0x87, 0x00, 0xd5, 0x93, 0x7a, 0x11, 0x62, 0xf7, },
            HeaderKekSourceHash = new byte[] { 0x18, 0x88, 0xca, 0xed, 0x55, 0x51, 0xb3, 0xed, 0xe0, 0x14, 0x99, 0xe8, 0x7c, 0xe0, 0xd8, 0x68, 0x27, 0xf8, 0x08, 0x20, 0xef, 0xb2, 0x75, 0x92, 0x10, 0x55, 0xaa, 0x4e, 0x2a, 0xbd, 0xff, 0xc2, },
            HeaderKeySourceHash = new byte[] { 0x8f, 0x78, 0x3e, 0x46, 0x85, 0x2d, 0xf6, 0xbe, 0x0b, 0xa4, 0xe1, 0x92, 0x73, 0xc4, 0xad, 0xba, 0xee, 0x16, 0x38, 0x00, 0x43, 0xe1, 0xb8, 0xc4, 0x18, 0xc4, 0x08, 0x9a, 0x8b, 0xd6, 0x4a, 0xa6, },
            AesKeyGenerationSourceHash = new byte[] { 0xfb, 0xd1, 0x00, 0x56, 0x99, 0x9e, 0xdc, 0x7a, 0xcd, 0xb9, 0x60, 0x98, 0xe4, 0x7e, 0x2c, 0x36, 0x06, 0x23, 0x02, 0x70, 0xd2, 0x32, 0x81, 0xe6, 0x71, 0xf0, 0xf3, 0x89, 0xfc, 0x5b, 0xc5, 0x85, },
            SaveMacKekSourceHash = new byte[] { 0x3D, 0xCB, 0xA1, 0x00, 0xAD, 0x4D, 0xF1, 0x54, 0x7F, 0xE3, 0xC4, 0x79, 0x5C, 0x4B, 0x22, 0x8A, 0xA9, 0x80, 0x38, 0xF0, 0x7A, 0x36, 0xF1, 0xBC, 0x14, 0x8E, 0xEA, 0xF3, 0xDC, 0xD7, 0x50, 0xF4, },
            SaveMacKeySourceHash = new byte[] { 0xB4, 0x7B, 0x60, 0x0B, 0x1A, 0xD3, 0x14, 0xF9, 0x41, 0x14, 0x7D, 0x8B, 0x39, 0x1D, 0x4B, 0x19, 0x87, 0xCC, 0x8C, 0x88, 0x4A, 0xC8, 0x9F, 0xFC, 0x91, 0xCA, 0xE2, 0x21, 0xC5, 0x24, 0x51, 0xF7, },
            SDCardKekSourceHash = new byte[] { 0x6B, 0x2E, 0xD8, 0x77, 0xC2, 0xC5, 0x23, 0x34, 0xAC, 0x51, 0xE5, 0x9A, 0xBF, 0xA7, 0xEC, 0x45, 0x7F, 0x4A, 0x7D, 0x01, 0xE4, 0x62, 0x91, 0xE9, 0xF2, 0xEA, 0xA4, 0x5F, 0x01, 0x1D, 0x24, 0xB7, },
            SDCardSaveKeySourceHash = new byte[] { 0xD4, 0x82, 0x74, 0x35, 0x63, 0xD3, 0xEA, 0x5D, 0xCD, 0xC3, 0xB7, 0x4E, 0x97, 0xC9, 0xAC, 0x8A, 0x34, 0x21, 0x64, 0xFA, 0x04, 0x1A, 0x1D, 0xC8, 0x0F, 0x17, 0xF6, 0xD3, 0x1E, 0x4B, 0xC0, 0x1C, },
            SDCardNcaKeySourceHash = new byte[] { 0x2E, 0x75, 0x1C, 0xEC, 0xF7, 0xD9, 0x3A, 0x2B, 0x95, 0x7B, 0xD5, 0xFF, 0xCB, 0x08, 0x2F, 0xD0, 0x38, 0xCC, 0x28, 0x53, 0x21, 0x9D, 0xD3, 0x09, 0x2C, 0x6D, 0xAB, 0x98, 0x38, 0xF5, 0xA7, 0xCC, },

            // ETicket hashes
            EticketRsaKekSourceHash = new byte[] { 0xB7, 0x1D, 0xB2, 0x71, 0xDC, 0x33, 0x8D, 0xF3, 0x80, 0xAA, 0x2C, 0x43, 0x35, 0xEF, 0x88, 0x73, 0xB1, 0xAF, 0xD4, 0x08, 0xE8, 0x0B, 0x35, 0x82, 0xD8, 0x71, 0x9F, 0xC8, 0x1C, 0x5E, 0x51, 0x1C, },
            EticketRsaKekekSourceHash = new byte[] { 0xE8, 0x96, 0x5A, 0x18, 0x7D, 0x30, 0xE5, 0x78, 0x69, 0xF5, 0x62, 0xD0, 0x43, 0x83, 0xC9, 0x96, 0xDE, 0x48, 0x7B, 0xBA, 0x57, 0x61, 0x36, 0x3D, 0x2D, 0x4D, 0x32, 0x39, 0x18, 0x66, 0xA8, 0x5C },

            // ssl hashes
            SslAesKeyXHash = new byte[] { 0x69, 0xA0, 0x8E, 0x62, 0xE0, 0xAE, 0x50, 0x7B, 0xB5, 0xDA, 0x0E, 0x65, 0x17, 0x9A, 0xE3, 0xBE, 0x05, 0x1F, 0xED, 0x3C, 0x49, 0x94, 0x1D, 0xF4, 0xEF, 0x29, 0x56, 0xD3, 0x6D, 0x30, 0x11, 0x0C, },
            SslRsaKeyYHash = new byte[] { 0x1C, 0x86, 0xF3, 0x63, 0x26, 0x54, 0x17, 0xD4, 0x99, 0x22, 0x9E, 0xB1, 0xC4, 0xAD, 0xC7, 0x47, 0x9B, 0x2A, 0x15, 0xF9, 0x31, 0x26, 0x1F, 0x31, 0xEE, 0x67, 0x76, 0xAE, 0xB4, 0xC7, 0x65, 0x42, };

        // Taken from KeyCollection.cpp in Lockpick
        public static readonly byte[][] TsecRootKeyHashes = new byte[][]
        {
            new byte[] { 0x03, 0x2a, 0xdf, 0x0a, 0x6b, 0xe7, 0xdd, 0x7c, 0x11, 0xa4, 0xfa, 0x5c, 0xd6, 0x4a, 0x15, 0x75, 0xe4, 0x69, 0xb9, 0xda, 0x5d, 0x8b, 0xd5, 0x6a, 0x12, 0xd0, 0xfb, 0xc0, 0xeb, 0x84, 0xe8, 0xe7}
        };

        public static readonly byte[][] MasterKekSources = new byte[][]
        {
            // 00-05 don't exist so empty
            Util.CreateJaggedArray<byte[]>(0x10), // 00 
            Util.CreateJaggedArray<byte[]>(0x10), // 01
            Util.CreateJaggedArray<byte[]>(0x10), // 02
            Util.CreateJaggedArray<byte[]>(0x10), // 03
            Util.CreateJaggedArray<byte[]>(0x10), // 04
            Util.CreateJaggedArray<byte[]>(0x10), // 05
            new byte[] { 0x37, 0x4B, 0x77, 0x29, 0x59, 0xB4, 0x04, 0x30, 0x81, 0xF6, 0xE5, 0x8C, 0x6D, 0x36, 0x17, 0x9A }
        };

        // Taken from key_derivation.c in biskeydump
        public static readonly byte[][] BisKeySources = new byte[][]
        {
            new byte[] {0xF8, 0x3F, 0x38, 0x6E, 0x2C, 0xD2, 0xCA, 0x32, 0xA8, 0x9A, 0xB9, 0xAA, 0x29, 0xBF, 0xC7, 0x48, 0x7D, 0x92, 0xB0, 0x3A, 0xA8, 0xBF, 0xDE, 0xE1, 0xA7, 0x4C, 0x3B, 0x6E, 0x35, 0xCB, 0x71, 0x06},
            new byte[] {0x41, 0x00, 0x30, 0x49, 0xDD, 0xCC, 0xC0, 0x65, 0x64, 0x7A, 0x7E, 0xB4, 0x1E, 0xED, 0x9C, 0x5F, 0x44, 0x42, 0x4E, 0xDA, 0xB4, 0x9D, 0xFC, 0xD9, 0x87, 0x77, 0x24, 0x9A, 0xDC, 0x9F, 0x7C, 0xA4},
            new byte[] {0x52, 0xC2, 0xE9, 0xEB, 0x09, 0xE3, 0xEE, 0x29, 0x32, 0xA1, 0x0C, 0x1F, 0xB6, 0xA0, 0x92, 0x6C, 0x4D, 0x12, 0xE1, 0x4B, 0x2A, 0x47, 0x4C, 0x1C, 0x09, 0xCB, 0x03, 0x59, 0xF0, 0x15, 0xF4, 0xE4 }
        };

        public static readonly byte[][] KeyblobSources = new byte[][]
        {
            new byte[] {0xDF, 0x20, 0x6F, 0x59, 0x44, 0x54, 0xEF, 0xDC, 0x70, 0x74, 0x48, 0x3B, 0x0D, 0xED, 0x9F, 0xD3}, // 1.0.0
            new byte[] {0x0C, 0x25, 0x61, 0x5D, 0x68, 0x4C, 0xEB, 0x42, 0x1C, 0x23, 0x79, 0xEA, 0x82, 0x25, 0x12, 0xAC}, // 3.0.0
            new byte[] {0x33, 0x76, 0x85, 0xEE, 0x88, 0x4A, 0xAE, 0x0A, 0xC2, 0x8A, 0xFD, 0x7D, 0x63, 0xC0, 0x43, 0x3B}, // 3.0.1
            new byte[] {0x2D, 0x1F, 0x48, 0x80, 0xED, 0xEC, 0xED, 0x3E, 0x3C, 0xF2, 0x48, 0xB5, 0x65, 0x7D, 0xF7, 0xBE}, // 4.0.0
            new byte[] {0xBB, 0x5A, 0x01, 0xF9, 0x88, 0xAF, 0xF5, 0xFC, 0x6C, 0xFF, 0x07, 0x9E, 0x13, 0x3C, 0x39, 0x80}, // 5.0.0
            new byte[] {0xD8, 0xCC, 0xE1, 0x26, 0x6A, 0x35, 0x3F, 0xCC, 0x20, 0xF3, 0x2D, 0x3B, 0x51, 0x7D, 0xE9, 0xC0}, // 6.0.0
        };

        // Taken from smc_user.c in Atmosphere
        public static readonly byte[][] KekSeeds = new byte[][]
        {
            new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
            new byte[] {0xA2, 0xAB, 0xBF, 0x9C, 0x92, 0x2F, 0xBB, 0xE3, 0x78, 0x79, 0x9B, 0xC0, 0xCC, 0xEA, 0xA5, 0x74},
            new byte[] {0x57, 0xE2, 0xD9, 0x45, 0xE4, 0x92, 0xF4, 0xFD, 0xC3, 0xF9, 0x86, 0x38, 0x89, 0x78, 0x9F, 0x3C},
            new byte[] {0xE5, 0x4D, 0x9A, 0x02, 0xF0, 0x4F, 0x5F, 0xA8, 0xAD, 0x76, 0x0A, 0xF6, 0x32, 0x95, 0x59, 0xBB},
            new byte[] {0x59, 0xD9, 0x31, 0xF4, 0xA7, 0x97, 0xB8, 0x14, 0x40, 0xD6, 0xA2, 0x60, 0x2B, 0xED, 0x15, 0x31},
            new byte[] {0xFD, 0x6A, 0x25, 0xE5, 0xD8, 0x38, 0x7F, 0x91, 0x49, 0xDA, 0xF8, 0x59, 0xA8, 0x28, 0xE6, 0x75},
            new byte[] {0x89, 0x96, 0x43, 0x9A, 0x7C, 0xD5, 0x59, 0x55, 0x24, 0xD5, 0x24, 0x18, 0xAB, 0x6C, 0x04, 0x61}
        };

        public static readonly byte[][] KekMasks = new byte[][] {
            new byte[] {0x4D, 0x87, 0x09, 0x86, 0xC4, 0x5D, 0x20, 0x72, 0x2F, 0xBA, 0x10, 0x53, 0xDA, 0x92, 0xE8, 0xA9},
            new byte[] {0x25, 0x03, 0x31, 0xFB, 0x25, 0x26, 0x0B, 0x79, 0x8C, 0x80, 0xD2, 0x69, 0x98, 0xE2, 0x22, 0x77},
            new byte[] {0x76, 0x14, 0x1D, 0x34, 0x93, 0x2D, 0xE1, 0x84, 0x24, 0x7B, 0x66, 0x65, 0x55, 0x04, 0x65, 0x81},
            new byte[] {0xAF, 0x3D, 0xB7, 0xF3, 0x08, 0xA2, 0xD8, 0xA2, 0x08, 0xCA, 0x18, 0xA8, 0x69, 0x46, 0xC9, 0x0B}
        };
    }
}
