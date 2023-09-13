using ThreadLivraison;
using static ThreadLivraison.ConstantesSimulation;
using System.Threading;
using System.Collections.Concurrent;

Random random = new Random();
int delaiCommande = random.Next(DELAI_MINIMUM_COMMANDE, DELAI_MAXIMUM_COMMANDE);
ConcurrentQueue <Commande> lesCommandes = new ConcurrentQueue<Commande>();

Thread threadCommande = new Thread(new ThreadStart(creerCommandes));
threadCommande.Start();


Thread[] cuisiniers = new Thread[NOMBRE_CUISINIERS];
Console.WriteLine(threadCommande.IsAlive);


for(int i = 0; i < NOMBRE_CUISINIERS; i++)
{
    cuisiniers[i] = new Thread(new ParameterizedThreadStart (traiterCommande));
}


for (int i = 0; i<NOMBRE_CUISINIERS; i++)
{
    cuisiniers[i].Start(i);
}


Thread[] livreurs = new Thread[NOMBRE_LIVREURS];



void creerCommandes()
{

    for (int i = 0; i < NOMBRE_COMMANDES; i++)
    {
        int tempsPreparation = random.Next(TEMPS_MINIMUM_PREPARATION, TEMPS_MAXIMUM_PREPARATION);
        int tempsLivraison = 1000;
        int positionX = random.Next(-10, 11);
        int positionY = random.Next(-10, 11);

        Thread.Sleep(delaiCommande);
        Commande nouvelleCommande = new Commande(i + 1, new Position(positionX, positionY), tempsPreparation, tempsLivraison, false);
        Console.WriteLine(nouvelleCommande.ToString());
        lesCommandes.Enqueue(nouvelleCommande);
       
    }
}

void traiterCommande(object? valeur)
{
    
    Console.WriteLine("Siuuu");
    foreach (Commande cmd in lesCommandes)
    {
        if(cmd.EstTraitée == false)
        {
            Console.WriteLine($"Le cuisinier #{((int)(valeur)+1)} traite la commande #{cmd.Numero}");
            Thread.Sleep (cmd.TempsPreparation);
            Console.WriteLine($"Le cuisinier #{((int)(valeur) + 1)} a traité la commande #{cmd.Numero}");
            cmd.EstTraitée = true;
        }
        
    }

}






