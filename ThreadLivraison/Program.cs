using ThreadLivraison;
using static ThreadLivraison.ConstantesSimulation;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;

Random random = new Random();
int delaiCommande = random.Next(DELAI_MINIMUM_COMMANDE, DELAI_MAXIMUM_COMMANDE);
ConcurrentQueue <Commande> lesCommandes = new ConcurrentQueue<Commande>();
ConcurrentQueue<Commande> lesCommandesPrepare = new ConcurrentQueue<Commande>();
ConcurrentQueue<Commande> lesCommandesLivre = new ConcurrentQueue<Commande>();

Stopwatch sw = new Stopwatch();
string tempsTravail; 

sw.Start();
Console.WriteLine("La pizzeria est ouverte");

Thread threadCommande = new Thread(new ThreadStart(creerCommandes));
threadCommande.Start();


Thread[] cuisiniers = new Thread[NOMBRE_CUISINIERS];
for (int i = 0; i < NOMBRE_CUISINIERS; i++)
{
    cuisiniers[i] = new Thread(new ParameterizedThreadStart(traiterCommande));
}

for (int i = 0; i < NOMBRE_CUISINIERS; i++)
{
    cuisiniers[i].Start(i);
}

Thread[] livreurs = new Thread[NOMBRE_LIVREURS];

//Console.WriteLine($"Temps de travail : {Utilitaires.Utilitaires.calculerTemps((int)((sw.ElapsedMilliseconds) * FACTEUR_ACCELERATION))}");

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
    while (threadCommande.IsAlive == true || lesCommandes.IsEmpty == false)
    {
        Commande uneCommande;
        int tempsPreparation = 0;

        if (lesCommandes.TryDequeue(out uneCommande))
        {
            tempsPreparation = uneCommande.TempsPreparation;
            Thread.Sleep(tempsPreparation);
            Console.WriteLine($"Le cuisinier #{((int)(valeur) + 1)} commence la commande #{uneCommande.Numero} {Utilitaires.Utilitaires.calculerTemps((int)(uneCommande.TempsPreparation * FACTEUR_ACCELERATION))}");
            Thread.Sleep(uneCommande.TempsPreparation);
            Console.WriteLine($"Le cuisinier #{((int)(valeur) + 1)} a terminé la commande #{uneCommande.Numero}");
            lesCommandesPrepare.Enqueue(uneCommande);
        }
        else
        {
            Console.WriteLine("Aucune commande à traiter");
            Thread.Sleep(1000);
        }

        
    }
    
}

static void afficherCommande(ConcurrentQueue<Commande> commandes)
{
    Console.WriteLine("Les commandes prepare sont : ");
    foreach(Commande c in commandes)
    {
        Console.WriteLine(c.ToString());
    }
}






