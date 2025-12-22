using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PersistenceC2Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("[+] Démarrage du programme (binaire 06)");

            // ============================
            // 1. SIMULATION DE PERSISTANCE
            // ============================

            /*
             * Dans un vrai malware, la persistance consisterait à
             * s'inscrire dans le système (registry, service, tâche planifiée).
             *
             * Ici, on SIMULE ce concept de façon inoffensive :
             * - on crée un simple fichier "marqueur"
             * - il sert uniquement à démontrer la logique
             */

            string demoDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PersistenceC2Demo"
            );

            string markerFile = Path.Combine(demoDir, "marker.txt");

            if (!Directory.Exists(demoDir))
            {
                Directory.CreateDirectory(demoDir);
                Console.WriteLine("[+] Dossier de démonstration créé");
            }

            if (!File.Exists(markerFile))
            {
                File.WriteAllText(markerFile, "Persistence simulation marker");
                Console.WriteLine("[+] Premier lancement détecté");
                Console.WriteLine("    (Une vraie persistance serait installée ici)");
            }
            else
            {
                Console.WriteLine("[+] Marqueur trouvé : exécution déjà observée");
            }

            // ============================
            // 2. SIMULATION DE C2
            // ============================

            /*
             * Un C2 réel impliquerait :
             * - un serveur distant
             * - des ordres reçus
             * - une exécution conditionnelle
             *
             * Ici :
             * - simple requête HTTP locale
             * - aucune commande
             * - aucune action distante
             */

            Console.WriteLine("[+] Tentative de communication C2 (simulation locale)");

            try
            {
                using HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(3);

                // Adresse volontairement locale et inoffensive
                string url = "http://127.0.0.1:8080/ping";

                HttpResponseMessage response = await client.GetAsync(url);

                Console.WriteLine($"[+] Réponse reçue : {response.StatusCode}");
                Console.WriteLine("[+] Aucune commande traitée (simulation uniquement)");
            }
            catch
            {
                Console.WriteLine("[!] Aucun serveur C2 disponible (comportement attendu)");
            }

            Console.WriteLine("[!] Fin du programme – aucune action persistante ou malveillante");
        }
    }
}
