using System;
using System.Runtime.InteropServices;

namespace ProcessHollowingDemo
{
    class Program
    {
        // =========================
        // IMPORT DES API WINDOWS
        // =========================

        // Récupère le contexte d'un thread (registres CPU)
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetThreadContext(IntPtr hThread, ref CONTEXT lpContext);

        // Permettrait de modifier le contexte du thread (NON UTILISÉ volontairement)
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetThreadContext(IntPtr hThread, ref CONTEXT lpContext);

        // Création d'un processus (ici Notepad) avec des flags spécifiques
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
            out PROCESS_INFORMATION lpProcessInformation
        );

        // Reprend l'exécution d'un thread suspendu
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint ResumeThread(IntPtr hThread);

        // =========================
        // CONSTANTES
        // =========================

        const uint CREATE_SUSPENDED = 0x00000004;

        // Flags pour définir quels registres récupérer
        // CONTEXT_FULL permet d'obtenir :
        // - les registres de contrôle (RIP, RSP)
        // - les registres généraux (RAX, RBX, RDX, etc.)
        const uint CONTEXT_CONTROL = 0x00100001;
        const uint CONTEXT_INTEGER = 0x00010002;
        const uint CONTEXT_FULL = CONTEXT_CONTROL | CONTEXT_INTEGER;

        // =========================
        // STRUCTURES WINDOWS
        // =========================

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

        // Structure CONTEXT x64 (partielle mais suffisante ici)
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
            public ulong Rdx; // Contient un pointeur vers le PEB
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

            public ulong Rip; // Instruction suivante à exécuter
        }

        static void Main(string[] args)
        {
            Console.WriteLine("[+] Début analyse Process Hollowing");

            STARTUPINFO si = new STARTUPINFO();
            si.cb = (uint)Marshal.SizeOf(typeof(STARTUPINFO));

            PROCESS_INFORMATION pi;

            // =========================
            // 1️⃣ Création du processus suspendu
            // =========================

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
                out pi
            );

            if (!result)
            {
                Console.WriteLine($"Erreur CreateProcess : {Marshal.GetLastWin32Error()}");
                return;
            }

            Console.WriteLine("[+] Processus créé en état SUSPENDED");
            Console.WriteLine($"    PID : {pi.dwProcessId}");
            Console.WriteLine($"    TID : {pi.dwThreadId}");

            // =========================
            // 2️⃣ Récupération du contexte du thread principal
            // =========================

            CONTEXT context = new CONTEXT();
            context.ContextFlags = CONTEXT_FULL;

            bool ctxResult = GetThreadContext(pi.hThread, ref context);
            if (!ctxResult)
            {
                Console.WriteLine($"Erreur GetThreadContext : {Marshal.GetLastWin32Error()}");
                return;
            }

            Console.WriteLine("[+] Contexte du thread récupéré");
            Console.WriteLine($"    RIP : 0x{context.Rip:X}");
            Console.WriteLine($"    RDX : 0x{context.Rdx:X}");
            Console.WriteLine("    → RDX pointe vers le PEB (Process Environment Block)");

            // Aucune modification du contexte n'est effectuée volontairement
            // SetThreadContext n'est PAS utilisé dans ce projet

            // =========================
            // 3️⃣ Reprise du thread
            // =========================

            uint resumeResult = ResumeThread(pi.hThread);
            if (resumeResult == unchecked((uint)-1))
            {
                Console.WriteLine("Erreur ResumeThread");
                return;
            }

            Console.WriteLine("[+] Thread repris → Notepad démarre normalement");

            // Les étapes offensives classiques du process hollowing
            // (lecture mémoire, remplacement d'image, redirection RIP)
            // sont volontairement NON implémentées dans ce projet.
        }
    }
}
