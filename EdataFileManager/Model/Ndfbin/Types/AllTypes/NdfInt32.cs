﻿using System;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfInt32 : NdfFlatValueWrapper
    {
        public NdfInt32(int value, long offset)
            : base(NdfType.Int32, value, offset)
        {

        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            try
            {
                return BitConverter.GetBytes(Convert.ToInt32(Value));
            }
            catch (Exception e)
            {
                valid = false;
                return new byte[0];
            }
        }
    }
}
