﻿using System;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfDouble : NdfFlatValueWrapper
    {
        public NdfDouble(double value, long offset)
            : base(NdfType.Float64, value, offset)
        {

        }

        public new double Value
        {
            get { return (double)base.Value; }
            set { base.Value = value; OnPropertyChanged("Value"); }
        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            try
            {
                return BitConverter.GetBytes(Convert.ToDouble(Value));
            }
            catch (Exception e)
            {
                valid = false;
                return new byte[0];
            }
        }

        public override string ToString()
        {
            return string.Format("{0:0.###################################}", Value);
        }
    }
}
