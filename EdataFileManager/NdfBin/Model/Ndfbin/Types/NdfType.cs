﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdataFileManager.NdfBin.Model.Ndfbin.Types
{
    public enum NdfType : uint
    {
        Boolean = 0x00000000,
        Int32 = 0x00000002,
        UInt32 = 0x00000003,
        Float32 = 0x00000005,
        TableString = 0x00000007,

        Reference = 0x00000009,

        ObjectReference = 0xBBBBBBBB,

        DescriptorId = 26,
        Unknown8Byte = 29,


        Vector = 0x0000000b,

        Color32 = 0x0000000d,

        TableStringFile = 0x0000001C,

        Unknown = 0xFFFFFFFF
    }
}