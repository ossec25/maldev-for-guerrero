using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ProcessHollowingDemo
{
    class Program
    {
        // Import API Windows nécessaires
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetThreadContext(IntPtr hThread, ref CONTEXT lpContext);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetThreadContext(IntPtr hThread, ref CONTEXT lpContext);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern bool CreateProcess(
            string lpApplicationName,
   	    string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint ResumeThread(IntPtr hThread);

        // Constantes
        const uint PROCESS_ALL_ACCESS = 0x001F0FFF;
        const uint CREATE_SUSPENDED = 0x00000004;

        // Flags pour CONTEXT
        const uint CONTEXT_CONTROL = 0x00100001;
        const uint CONTEXT_INTEGER = 0x00010002;
        const uint CONTEXT_FULL = CONTEXT_CONTROL | CONTEXT_INTEGER;

        // Structures Windows

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            public uint cb;
            public string? lpReserved;
            public string? lpDesktop;
            public string? lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public ushort wShowWindow;
            public ushort cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        // CONTEXT structure pour x64 (simplifiée mais correcte)
        [StructLayout(LayoutKind.Sequential)]
        public struct CONTEXT
        {
            public ulong P1Home;
            public ulong P2Home;
            public ulong P3Home;
            public ulong P4Home;
            public ulong P5Home;
            public ulong P6Home;

            public uint ContextFlags;
            public uint MxCsr;

            public ushort SegCs;
            public ushort SegDs;
            public ushort SegEs;
            public ushort SegFs;
            public ushort SegGs;
            public ushort SegSs;
            public uint EFlags;

            public ulong Dr0;
            public ulong Dr1;
            public ulong Dr2;
            public ulong Dr3;
            public ulong Dr6;
            public ulong Dr7;

            public ulong Rax;
            public ulong Rcx;
            public ulong Rdx;
            public ulong Rbx;
            public ulong Rsp;
            public ulong Rbp;
            public ulong Rsi;
            public ulong Rdi;
            public ulong R8;
            public ulong R9;
            public ulong R10;
            public ulong R11;
            public ulong R12;
            public ulong R13;
            public ulong R14;
            public ulong R15;

            public ulong Rip;

            // There are more fields for floating points etc, but omitted here for brevity
        }

        static void Main(string[] args)
        {
            Console.WriteLine("[+] Début Process Hollowing");

            // Préparation des structures
            STARTUPINFO si = new STARTUPINFO();
            si.cb = (uint)Marshal.SizeOf(typeof(STARTUPINFO));
            PROCESS_INFORMATION pi;

            // Création du processus Notepad en mode suspendu
            bool result = CreateProcess(
                null,
                "C:\\Windows\\System32\\notepad.exe",
                IntPtr.Zero,
                IntPtr.Zero,
                false,
                CREATE_SUSPENDED,
                IntPtr.Zero,
                null,
                ref si,
                out pi);

            if (!result)
            {
                Console.WriteLine($"Erreur création processus : {Marshal.GetLastWin32Error()}");
                return;
            }

            Console.WriteLine($"[+] Processus créé en état SUSPENDED");
            Console.WriteLine($"    PID  : {pi.dwProcessId}");
            Console.WriteLine($"    TID  : {pi.dwThreadId}");

            // Préparer la structure CONTEXT pour récupérer le contexte thread
            CONTEXT context = new CONTEXT();
            context.ContextFlags = CONTEXT_FULL;

            // Récupérer le contexte du thread principal (suspendu)
            bool ctxResult = GetThreadContext(pi.hThread, ref context);
            if (!ctxResult)
            {
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine($"Erreur lors de la récupération du contexte du thread. Code erreur : {error}");
                return;
            }

            Console.WriteLine("[+] Contexte Thread récupéré");
            Console.WriteLine($"    RIP : 0x{context.Rip:X}");
            Console.WriteLine($"    RDX (PEB) : 0x{context.Rdx:X}");

            // Ici tu pourras modifier context.Rip ou d'autres registres selon besoin

            // Exemple (inutile ici mais à titre d’illustration) :
            // context.Rip = nouvelleAdresse;

            // Appliquer le contexte modifié (optionnel)
            // bool setCtxResult = SetThreadContext(pi.hThread, ref context);
            // if (!setCtxResult)
            // {
            //     int errSet = Marshal.GetLastWin32Error();
            //     Console.WriteLine($"Erreur SetThreadContext : {errSet}");
            //     return;
            // }

            // Reprendre le thread principal
            uint resumeResult = ResumeThread(pi.hThread);
            if (resumeResult == unchecked((uint)-1))
            {
                Console.WriteLine("Erreur lors du ResumeThread");
                return;
            }

            Console.WriteLine("[+] Thread repris → Notepad démarre normalement");

// À partir de ce point, les étapes suivantes du process hollowing
// (lecture mémoire distante, modification de l'image, redirection RIP)
// ne sont PAS implémentées volontairement.
// Elles sont décrites uniquement de manière théorique dans le write-up
// pour des raisons de sécurité et de cadre académique.

        }
    }
}
