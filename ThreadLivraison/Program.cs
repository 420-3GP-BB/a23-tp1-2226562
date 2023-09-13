using ThreadLivraison;
using static ThreadLivraison.ConstantesSimulation;
using System.Threading;

Random random = new Random();
int delaiCommande = random.Next(DELAI_MINIMUM_COMMANDE) + DELAI_MAXIMUM_COMMANDE;

Commande[] tabCommande = new Commande[NOMBRE_COMMANDES];
Thread threadCommande = new Thread(creerCommandes);
threadCommande.Start();


void creerCommandes()
{

    for (int i = 0; i < tabCommande.Length; i++)
    {
        int tempsPreparation = random.Next(TEMPS_MINIMUM_PREPARATION, TEMPS_MAXIMUM_PREPARATION);
        int tempsLivraison = 1000;
        int positionX = random.Next(-10, 11);
        int positionY = random.Next(-10, 11);

        tabCommande[i] = new Commande(i + 1, new Position(positionX, positionY), tempsPreparation, tempsLivraison);
        Console.WriteLine(tabCommande[i].ToString());

        Thread.Sleep(100);

    }
}






