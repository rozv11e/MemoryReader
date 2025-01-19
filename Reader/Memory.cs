using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using WinAPI;

namespace Reader
{
    public class Memory
    {
        private readonly Process process;
        public Memory(Process process)
        {
            this.process = process;
        }

        public IntPtr GetModuleBase(string module)
        {
            foreach (ProcessModule item in process.Modules)
            {
                if (item.ModuleName == module)
                {
                    return item.BaseAddress;
                }
            }
            return IntPtr.Zero;
        }

        public byte[] ReadRaw(IntPtr MemoryAddress, uint Len)
        {
            return ReadProcessMemory(MemoryAddress, Len);
        }

        public string ReadString(IntPtr MemoryAddress)
        {
            string text = "";
            byte[] array = new byte[1];
            array = ReadProcessMemory(MemoryAddress, 1u);
            while (array[0] != 0)
            {
                text += (char)array[0];
                MemoryAddress = IntPtr.Add(MemoryAddress, 1); // Use IntPtr.Add instead of casting to int
                array = ReadProcessMemory(MemoryAddress, 1u);
            }

            return text;
        }

        public string ReadString(IntPtr MemoryAddress, uint Len)
        {
            string text = "";
            byte[] array = ReadProcessMemory(MemoryAddress, Len);
            for (int i = 0; i > Len; i++)
            {
                text += (char)array[i];
            }

            return text;
        }

        public float ReadFloat(IntPtr Address)
        {
            byte[] value = ReadProcessMemory(Address, 4u);
            return BitConverter.ToSingle(value, 0);
        }

        public uint ReadUInt32(IntPtr MemoryAddress)
        {
            byte[] value = ReadProcessMemory(MemoryAddress, 4u);
            return BitConverter.ToUInt32(value, 0);
        }

        public ulong ReadUInt64(IntPtr MemoryAddress)
        {
            byte[] value = ReadProcessMemory(MemoryAddress, 8u);
            return BitConverter.ToUInt64(value, 0);
        }

        public int ReadInt32(IntPtr MemoryAddress)
        {
            byte[] value = ReadProcessMemory(MemoryAddress, 4u);
            return BitConverter.ToInt32(value, 0);
        }

        public long ReadInt64(IntPtr MemoryAddress)
        {
            byte[] value = ReadProcessMemory(MemoryAddress, 8u);
            return BitConverter.ToInt64(value, 0);
        }

        public ulong ReadULong(IntPtr MemoryAddress)
        {
            byte[] value = ReadProcessMemory(MemoryAddress, 8u);
            return BitConverter.ToUInt64(value, 0);
        }

        public long ReadLong(IntPtr MemoryAddress)
        {
            byte[] value = ReadProcessMemory(MemoryAddress, 8u);
            return BitConverter.ToInt64(value, 0);
        }

        public Vector3 ReadVec3(IntPtr MemoryAddress)
        {
            float x = ReadFloat(MemoryAddress);
            float y = ReadFloat(MemoryAddress + 4);
            float z = ReadFloat(MemoryAddress + 8);

            return new Vector3(x, y, z);
        }

        public byte[] ReadBytes(IntPtr addy, uint bytes)
        {
            var array = ReadProcessMemory(addy, bytes);
            return array;
        }
        public float[] ReadMatrix(IntPtr address)
        {
            byte[] array = ReadBytes(address, 64);
            float[] array2 = new float[array.Length];
            array2[0] = BitConverter.ToSingle(array, 0);
            array2[1] = BitConverter.ToSingle(array, 4);
            array2[2] = BitConverter.ToSingle(array, 8);
            array2[3] = BitConverter.ToSingle(array, 12);
            array2[4] = BitConverter.ToSingle(array, 16);
            array2[5] = BitConverter.ToSingle(array, 20);
            array2[6] = BitConverter.ToSingle(array, 24);
            array2[7] = BitConverter.ToSingle(array, 28);
            array2[8] = BitConverter.ToSingle(array, 32);
            array2[9] = BitConverter.ToSingle(array, 36);
            array2[10] = BitConverter.ToSingle(array, 40);
            array2[11] = BitConverter.ToSingle(array, 44);
            array2[12] = BitConverter.ToSingle(array, 48);
            array2[13] = BitConverter.ToSingle(array, 52);
            array2[14] = BitConverter.ToSingle(array, 56);
            array2[15] = BitConverter.ToSingle(array, 60);
            return array2;
        }

        public IntPtr[] ReadArray<T>(IntPtr address, uint count)
        {
            byte[] buffer = new byte[count * Marshal.SizeOf(typeof(T))];

            var pArray = ReadProcessMemory(address, (uint)buffer.Length);

            IntPtr[] intptrArray = new IntPtr[pArray.Length / IntPtr.Size];

            for (int i = 0; i < intptrArray.Length; i++)
            {
                intptrArray[i] = (IntPtr)BitConverter.ToInt64(pArray, i * IntPtr.Size);
            }

            return intptrArray;
        }

        public byte ReadByte(IntPtr address)
        {
            byte[] buffer = ReadProcessMemory(address, 1);
            return buffer[0];
        }

        public short ReadShort(IntPtr address)
        {
            var value = ReadProcessMemory(address, 2u);
            return BitConverter.ToInt16(value, 0);
        }

        public char ReadChar(IntPtr address)
        {
            var value = ReadProcessMemory(address, 1u);
            return BitConverter.ToChar(value, 0);
        }

        //public IntPtr ReadPointer(IntPtr MemoryAddress)
        //{
        //    byte[] value = ReadProcessMemory(MemoryAddress, 8u);

        //    if (value.Length != 8)
        //    {
        //        return IntPtr.Zero;
        //    }

        //    var longValue = BitConverter.ToInt64(value, 0);
        //    return (IntPtr)longValue;
        //}

        public IntPtr ReadPointer(IntPtr address)
        {
            var value = ReadProcessMemory(address, 4u);
            return (IntPtr)BitConverter.ToInt32(value, 0);
        }

        public byte[] ReadProcessMemory(IntPtr MemoryAddress, uint bytesToRead)
        {
            byte[] array = new byte[bytesToRead];
            if (process == null)
            {
                return array;
            }

            WinImports.ReadProcessMemory(process.Handle, MemoryAddress, array, bytesToRead, out var _);
            return array;
        }

        // Write

        public bool WriteFloat(IntPtr address, float value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value));
        }

        public bool WriteBytes(IntPtr address, byte[] newbytes)
        {
            return WinImports.WriteProcessMemory(process.Handle, address, newbytes, (uint)newbytes.Length, out var _);
        }
    }
}

//[MethodImpl(MethodImplOptions.AggressiveInlining)]
//public unsafe bool Read<T>(IntPtr address, out T value) where T : unmanaged
//{
//    int size = sizeof(T);

//    fixed (byte* pBuffer = new byte[size])
//    {
//        if (WinImports.ReadProcessMemory(Process.GetCurrentProcess().Handle, address, pBuffer, size, out _))
//        {
//            value = *(T*)pBuffer;
//            return true;
//        }
//    }

//    value = default;
//    return false;
//}


//public IntPtr ProcessHwnd = WinImports.OpenProcess((uint)WinImports.ProcessAccessFlags.VirtualMemoryRead, false, (uint)process.Id);