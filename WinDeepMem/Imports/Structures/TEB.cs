using System.Runtime.InteropServices;

namespace WinDeepMem.Imports.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TEB
    {
        public NtTib NtTib;                   // 0x000 — Thread Information Block (стандартное начало TEB)
        public IntPtr EnvironmentPointer;     // 0x038
        public CLIENT_ID ClientId;            // 0x040 (UniqueProcess, UniqueThread)
        public IntPtr ActiveRpcHandle;        // 0x050
        public IntPtr ThreadLocalStoragePointer; // 0x058
        public IntPtr ProcessEnvironmentBlock;   // 0x060 → PEB
                                                 // дальше идёт куча служебных данных
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NtTib
    {
        public IntPtr ExceptionList;      // 0x000 — список SEH
        public IntPtr StackBase;          // 0x008
        public IntPtr StackLimit;         // 0x010
        public IntPtr SubSystemTib;       // 0x018
        public IntPtr FiberData;          // 0x020
        public IntPtr ArbitraryUserPointer; // 0x028
        public IntPtr Self;               // 0x030 → Указатель на сам TEB
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct CLIENT_ID
    {
        public IntPtr UniqueProcess;   // PID
        public IntPtr UniqueThread;    // TID
    }


}
