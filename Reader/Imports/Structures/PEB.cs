using System;
using System.Runtime.InteropServices;

namespace Reader.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PEB
    {
        public byte InheritedAddressSpace;        // 0x000
        public byte ReadImageFileExecOptions;     // 0x001
        public byte BeingDebugged;                // 0x002
        public byte BitField;                     // 0x003
        public IntPtr Mutant;                     // 0x008
        public IntPtr ImageBaseAddress;           // 0x010
        public IntPtr Ldr;                        // 0x018 -> PEB_LDR_DATA
        public IntPtr ProcessParameters;          // 0x020 -> RTL_USER_PROCESS_PARAMETERS
        public IntPtr SubSystemData;              // 0x028
        public IntPtr ProcessHeap;                // 0x030
        public IntPtr FastPebLock;                // 0x038
        public IntPtr AtlThunkSListPtr;           // 0x040
        public IntPtr IFEOKey;                    // 0x048
        public uint CrossProcessFlags;            // 0x050
        public uint Padding1;                     // 0x054
        public IntPtr KernelCallbackTable;        // 0x058
        public uint SystemReserved;               // 0x060
        public uint AtlThunkSListPtr32;           // 0x064
        public uint NtGlobalFlag;                 // 0x068
        public IntPtr CriticalSectionTimeout;     // 0x070
        public IntPtr HeapSegmentReserve;         // 0x078
        public IntPtr HeapSegmentCommit;          // 0x080
        public IntPtr HeapDeCommitTotalFreeThreshold; // 0x088
        public IntPtr HeapDeCommitFreeBlockThreshold; // 0x090
        public uint NumberOfHeaps;                // 0x098
        public uint MaximumNumberOfHeaps;         // 0x09C
        public IntPtr ProcessHeaps;               // 0x0A0
        public IntPtr GdiSharedHandleTable;       // 0x0A8
        public IntPtr ProcessStarterHelper;       // 0x0B0
        public IntPtr GdiDCAttributeList;         // 0x0B8
        public IntPtr LoaderLock;                 // 0x0C0
        public uint OSMajorVersion;               // 0x0C8
        public uint OSMinorVersion;               // 0x0CC
        public ushort OSBuildNumber;              // 0x0D0
        public ushort OSCSDVersion;               // 0x0D2
        public uint OSPlatformId;                 // 0x0D4
        public uint ImageSubsystem;               // 0x0D8
        public uint ImageSubsystemMajorVersion;   // 0x0DC
        public uint ImageSubsystemMinorVersion;   // 0x0E0
        public IntPtr ActiveProcessAffinityMask;  // 0x0E8
        public uint GdiHandleBuffer0;             // 0x0F0 (начало массива GdiHandleBuffer[60])
                                                  // ... далее остальные поля по необходимости
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct RTL_USER_PROCESS_PARAMETERS
    {
        public uint MaximumLength;
        public uint Length;

        public uint Flags;
        public uint DebugFlags;

        public IntPtr ConsoleHandle;
        public uint ConsoleFlags;
        public IntPtr StandartInput;
        public IntPtr StandartOutput;
        public IntPtr StandardError;

        public CURDIR CurrentDirectory;
        public UNICODE_STRING DllPath;
        public UNICODE_STRING ImagePathName;
        public UNICODE_STRING CommandLine;
        public IntPtr Environment;

        public uint StartingX;
        public uint StartingY;
        public uint CountX;
        public uint CountY;
        public uint CountCharsX;
        public uint CountCharsY;
        public uint FillAttribute;

        public uint WindowFlags;
        public uint ShowWindowFlags;
        public UNICODE_STRING WindowTitle;
        public UNICODE_STRING DesktopInfo;
        public UNICODE_STRING ShellInfo;
        public UNICODE_STRING RuntimeData;

        public unsafe fixed byte CurrentDirectories[26 * 24]; // если структура 24 байта

        public UIntPtr EnvironmentSize;
        public UIntPtr EnvironmentVersion;

        public IntPtr PackageDependencyData;
        public uint ProcessGroupId;
        public uint LoaderThreads;
        public UNICODE_STRING RedirectionDllName;
        public UNICODE_STRING HeapPartitionName;
        public IntPtr DefaultThreadpoolCpuSetMasks;
        public uint DefaultThreadpoolCpuSetMaskCount;
        public uint DefaultThreadpoolThreadMaximum;
        public uint HeapMemoryTypeMask;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UNICODE_STRING
    {
        public ushort Length;
        public ushort MaximumLength;
        public IntPtr Buffer; // указатель на WCHAR*
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CURDIR
    {
        public UNICODE_STRING DosPath; // путь к текущему каталогу
        public IntPtr Handle;           // HANDLE к каталогу
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RTL_DRIVE_LETTER_CURDIR
    {
        public ushort Flags;
        public ushort Length;
        public uint TimeStamp;
        public UNICODE_STRING DosPath;
    }

}
