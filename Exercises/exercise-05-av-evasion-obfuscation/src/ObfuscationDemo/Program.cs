using System;
using System.Text;

namespace ObfuscationDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // ================================
            // Étape 1 : Donnée claire (exemple)
            // ================================
            // Dans un vrai malware, ceci pourrait être un payload ou une URL C2.
            // Ici, c’est volontairement un simple message inoffensif.
            string clearText = "This is a harmless academic demonstration";

            Console.WriteLine("[+] Texte original :");
            Console.WriteLine(clearText);

            // ==================================
            // Étape 2 : Obfuscation (XOR + Base64)
            // ==================================
            // Objectif pédagogique :
            // Montrer comment on évite une signature statique en ne stockant
            // jamais la donnée en clair dans le binaire final.
            byte key = 0x5A; // clé XOR simple (exemple académique)
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);

            for (int i = 0; i < clearBytes.Length; i++)
            {
                clearBytes[i] ^= key; // XOR = obfuscation basique
            }

            string obfuscated = Convert.ToBase64String(clearBytes);

            Console.WriteLine("\n[+] Donnée obfusquée (stockable dans le binaire) :");
            Console.WriteLine(obfuscated);

            // ==================================
            // Étape 3 : Désobfuscation à l’exécution
            // ==================================
            // Les antivirus basés sur signatures analysent souvent le contenu statique.
            // La donnée n’existe en clair qu’en mémoire, à l’exécution.
            byte[] decodedBytes = Convert.FromBase64String(obfuscated);

            for (int i = 0; i < decodedBytes.Length; i++)
            {
                decodedBytes[i] ^= key; // XOR inverse
            }

            string decodedText = Encoding.UTF8.GetString(decodedBytes);

            Console.WriteLine("\n[+] Texte reconstruit à l’exécution :");
            Console.WriteLine(decodedText);

            // ==================================
            // Étape 4 : Limite volontaire
            // ==================================
            // Ici, on s’arrête volontairement.
            // Aucun chargement dynamique, aucun appel système,
            // aucune exécution de code externe.
            Console.WriteLine("\n[!] Démonstration terminée (sans comportement malveillant).");
        }
    }
}
