using ThreadLivraison;
using static ThreadLivraison.ConstantesSimulation;
using System.Threading;
using System.Collections.Concurrent;

Random random = new Random();
int delaiCommande = random.Next(DELAI_MINIMUM_COMMANDE, DELAI_MAXIMUM_COMMANDE);
ConcurrentQueue <Commande> lesCommandes = new ConcurrentQueue<Commande>();
Thread threadCommande = new Thread(new ThreadStart(creerCommandes));
threadCommande.Start();


void creerCommandes()
{

    for (int i = 0; i < NOMBRE_COMMANDES; i++)
    {
        int tempsPreparation = random.Next(TEMPS_MINIMUM_PREPARATION, TEMPS_MAXIMUM_PREPARATION);
        int tempsLivraison = 1000;
        int positionX = random.Next(-10, 11);
        int positionY = random.Next(-10, 11);

        Thread.Sleep(delaiCommande);
        Commande nouvelleCommande = new Commande(i + 1, new Position(positionX, positionY), tempsPreparation, tempsLivraison);
        Console.WriteLine(nouvelleCommande.ToString());
        lesCommandes.Enqueue(nouvelleCommande);
       
    }
}






