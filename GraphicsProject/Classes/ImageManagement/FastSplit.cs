using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Classes.ImageManagement
{
    class FastSplit
    {
        public FastSplit(int bufferSize)
        {
            _buffer = new string[bufferSize];
        }
        public string[] Results => _buffer;

        private string[] _buffer;

        public int SafeSplit(string value, char separator)
        {
            int resultIndex = 0;
            int startIndex = 0;

            int length = value.Length;

            for (int i = 0; i < length; i++)
            {
                if (value[i] != separator) continue;

                if (_buffer.Length == resultIndex)
                {
                    Array.Resize(ref _buffer, _buffer.Length * 2);
                }

                _buffer[resultIndex++] = value.Substring(startIndex, i - startIndex);
                startIndex = i + 1;
            }

            if (_buffer.Length == resultIndex)
            {
                Array.Resize(ref _buffer, _buffer.Length * 2);
            }

            _buffer[resultIndex] = value.Substring(startIndex, value.Length - startIndex);

            return resultIndex + 1;
        }
    }
}

