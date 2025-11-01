namespace System.Media
{
    public class AdpcmDecoder
    {
        public static AdpcmDecoder Create() => new AdpcmDecoder();

        private int[] indexTable = new int[] {
          -1, -1, -1, -1, 2, 4, 6, 8,
          -1, -1, -1, -1, 2, 4, 6, 8
        };

        private int[] stepSizeTable = new int[] {
          7, 8, 9, 10, 11, 12, 13, 14, 16, 17,
          19, 21, 23, 25, 28, 31, 34, 37, 41, 45,
          50, 55, 60, 66, 73, 80, 88, 97, 107, 118,
          130, 143, 157, 173, 190, 209, 230, 253, 279, 307,
          337, 371, 408, 449, 494, 544, 598, 658, 724, 796,
          876, 963, 1060, 1166, 1282, 1411, 1552, 1707, 1878, 2066,
          2272, 2499, 2749, 3024, 3327, 3660, 4026, 4428, 4871, 5358,
          5894, 6484, 7132, 7845, 8630, 9493, 10442, 11487, 12635, 13899,
          15289, 16818, 18500, 20350, 22385, 24623, 27086, 29794, 32767
        };

        int statePrevSample = 0;
        int statePrevIndex = 0;

        /// <summary>
        /// Clamper func, available in .NET Core, not framework.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minvalue"></param>
        /// <param name="maxvalue"></param>
        /// <returns></returns>
        public int Clamp(int value, int minvalue, int maxvalue)
        {
            if (value <= minvalue) return minvalue;
            if (value >= maxvalue) return maxvalue;

            return value;
        }

        /// <summary>
        /// Decodes a single 4 bit adpcm sample to raw 16 bit value.
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        public short DecodeSample(int sample)
        {
            var predSample = statePrevSample;
            var index = statePrevIndex;
            var step = stepSizeTable[index];
            var difference = step >> 3;


            // compute difference and new predicted value
            if ((sample & 0x4) > 0) difference += step;
            if ((sample & 0x2) > 0) difference += (step >> 1);
            if ((sample & 0x1) > 0) difference += (step >> 2);

            // handle sign bit
            predSample += ((sample & 0x8) > 0) ? -difference : difference;

            // find new index value
            index += indexTable[sample];
            index = Clamp(index, 0, 88);

            // clamp output value
            predSample = Clamp(predSample, -32767, 32767);

            statePrevSample = predSample;
            statePrevIndex = index;

            return (short)predSample;
        }

        /// <summary>
        /// Expects a raw byte array, converts to array of int16 samples.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public short[] Decode(byte[] input)
        {
            statePrevSample = 0;
            statePrevIndex = 0;

            var output = new short[input.Length * 2];

            for (int i = 0; i < input.Length; i++)
            {
                output[i * 2] = DecodeSample((input[i] >> 4) & 0xF);
                output[i * 2 + 1] = DecodeSample(input[i] & 0xF);
            }

            return output;
        }

        // ADPCM decoder implementation based on https://github.com/jwzhangjie/Adpcm_Pcm/blob/master/adpcm.c

    }
}
