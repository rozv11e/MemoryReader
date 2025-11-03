using static WinDeepMem.Imports.WinApi;
using System.Diagnostics;
using System.Text;
namespace WinDeepMem
{
    public class Injector
    {
        private readonly string _pathToDll;
        private readonly Process _process;
        private readonly Memory mem;
        public Injector(Process targetProcess, string pathToDll)
        {
            _process = targetProcess;
            _pathToDll = pathToDll;
            mem = new Memory(targetProcess);
        }

        public bool Inject()
        {


            var hProcess = _process.Handle;
            if (hProcess == null)
            {
                Console.WriteLine("[Error] Could't get handle to process");
                return false;
            }

            if (string.IsNullOrEmpty(_pathToDll))
            {
                Console.WriteLine("[Error] Path string is empty");
                return false;
            }

            byte[] path = Encoding.Unicode.GetBytes(_pathToDll + "\0");
            //byte[] pathBytes = Encoding.Unicode.GetBytes(_pathToDll + "\0");
            //uint size = (uint)(_pathToDll.Length + 1) * 2; // Unicode = * 2
            uint size = (uint)path.Length; // Размер уже с терминатором

            // Allocate memory in remote process
            var pDllPath = VirtualAllocEx(hProcess,
                IntPtr.Zero,
                size,
                (uint)MemoryAllocationType.MEM_COMMIT,
                (uint)MemoryProtectionType.PAGE_READWRITE);
            
            if (pDllPath == IntPtr.Zero)
            {
                Console.WriteLine("Could not write to memory in remote process");
                return false;
            }

            Thread.Sleep(500);

            Console.WriteLine("[Debug] pDllPath: 0x" + pDllPath.ToString("X"));

            // for LoadLibraryW (UTF-16) - Unicode || LoadLibraryA (ANSI) - Default
            //byte[] path = Encoding.Unicode.GetBytes(_pathToDll);
            mem.WriteBytes(pDllPath, path);
            Thread.Sleep(500);

            // Check if array isn't empty
            var bytesToRead = mem.ReadBytes(pDllPath, (uint)path.Length);
            Console.WriteLine("[Debug] bytesToRead: " + BitConverter.ToString(bytesToRead));


            var hModule = GetModuleHandle("kernel32.dll");
            if (hModule == nint.Zero)
            {
                Console.WriteLine("[Error] Could't get handle to kernel32.dll");
                return false;
            }

            var ploadlib = GetProcAddress(hModule, "LoadLibraryW");
            if (ploadlib == nint.Zero)
            {
                Console.WriteLine("[Error] Couldn't get pointer to LoadLibraryW");
                return false;
            }

            Console.WriteLine("[Debug] pLoadLibraryA: 0x" + ploadlib.ToString("X"));
            Thread.Sleep(500);

            IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, ploadlib, pDllPath, 0, out _);
            if (hThread == IntPtr.Zero)
            {
                Console.WriteLine("[Error] phThread: 0x" + hThread.ToString("X"));
                VirtualFreeEx(hProcess, pDllPath, 0, MemoryFreeType.MEM_RELEASE);
                return false;
            }

            Console.WriteLine("[Debug] hThread: 0x" + hThread);

            // Wait thread finish
            WaitForSingleObject(hThread, INFINITE);

            //uint exitCode;

            //if (!GetExitCodeThread(hThread, out exitCode))
            //{
            //    Console.WriteLine("[Error] Could not get thread exit code.");
            //    return false;
            //}

            //if (exitCode == 0)
            //{
            //    Console.WriteLine("[Error] Call to LoadLibraryW in remote process failed. DLL must have exited non-gracefully.");
            //    return false;
            //}

            var error = GetLastError();
            Console.WriteLine("[Debug] LastError: " + error);

            //VirtualFreeEx(handle, pDllPath, 0, MemoryFreeType.MEM_RELEASE);

            return true;
        }

        // TODO: Unload
    }
}
