using Reader.Structures;
using System;
using System.Runtime.InteropServices;

namespace Reader.Imports.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PEB_LDR_DATA
    {
        public uint Length;                  // 0x00
        [MarshalAs(UnmanagedType.U1)]
        public bool Initialized;             // 0x04
        public IntPtr SsHandle;              // 0x08
        public LIST_ENTRY InLoadOrderModuleList;           // 0x10  // Модули в порядке загрузки
        public LIST_ENTRY InMemoryOrderModuleList;         // 0x20  // Модули в памяти
        public LIST_ENTRY InInitializationOrderModuleList; // 0x30  // Модули по инициализации
        public IntPtr EntryInProgress;       // 0x40
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LIST_ENTRY
    {
        public IntPtr Flink; // Указатель на следующий элемент
        public IntPtr Blink; // Указатель на предыдущий элемент
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LDR_DATA_TABLE_ENTRY
    {
        public LIST_ENTRY InLoadOrderLinks;         // LIST_ENTRY для InLoadOrderModuleList
        public LIST_ENTRY InMemoryOrderLinks;       // LIST_ENTRY для InMemoryOrderModuleList
        public LIST_ENTRY InInitializationOrderLinks; // LIST_ENTRY для InInitializationOrderModuleList

        public IntPtr DllBase;                      // Base address модуля
        public IntPtr EntryPoint;                   // Entry point модуля
        public uint SizeOfImage;

        public UNICODE_STRING FullDllName;          // Полный путь к DLL
        public UNICODE_STRING BaseDllName;          // Имя файла DLL

        // остальные поля можно опустить, если нужны только базовые данные
    }
}
