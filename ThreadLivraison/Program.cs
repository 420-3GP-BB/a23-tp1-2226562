using ThreadLivraison;
using static ThreadLivraison.ConstantesSimulation;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Collections;

Random random = new Random();
int delaiCommande = random.Next(DELAI_MINIMUM_COMMANDE, DELAI_MAXIMUM_COMMANDE);
ConcurrentQueue <Commande> lesCommandes = new ConcurrentQueue<Commande>();
ConcurrentQueue<Commande> lesCommandesPrepare = new ConcurrentQueue<Commande>();
List<Commande> commandesPrepareTableau = new List<Commande>();
ConcurrentQueue<Commande> lesCommandesLivre = new ConcurrentQueue<Commande>();

Stopwatch sw = new Stopwatch();
string tempsTravail; 


//sw.Start();
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

for(int i=0; i<NOMBRE_LIVREURS; i++)
{
    livreurs[i] = new Thread(new ParameterizedThreadStart(livrerCommande));
}

for(int i=0; i < NOMBRE_LIVREURS; i++)
{
    livreurs[i].Start(i);
}

//Console.WriteLine($"Temps de travail : {Utilitaires.Utilitaires.calculerTemps((int)((sw.ElapsedMilliseconds) * FACTEUR_ACCELERATION))}");

void livrerCommande(object? valeur)
{
    int distance = 0;
    int dureeDistance = 0;
    Commande[] commandeLivree = new Commande[1];
    while (cuisiniers[0].IsAlive || ! lesCommandesPrepare.IsEmpty)
    {
        //
        Commande uneCommande;
        if (lesCommandesPrepare.TryDequeue(out uneCommande))
        {
            remplirTableauCommandeLivree(commandeLivree,uneCommande);
            Console.WriteLine($"Le livreur #{((int)(valeur) + 1)} va livré les commandes : ");
            afficherCommandeLivree(commandeLivree);
            distance = calculerDistance(uneCommande.Destination.X, uneCommande.Destination.Y);
            dureeDistance = (calculerDureeDistance(0, distance) * TEMPS_DEPLACEMENT) + TEMPS_PAIEMENT;
            for (int i = 0; i < commandeLivree.Length; i++)
            {
                Console.WriteLine($"Le livreur #{((int)(valeur) + 1)} va livre la commande : #{commandeLivree[i].Numero}");
                Thread.Sleep(dureeDistance);
                commandeLivree[i].EstLivrée = true;
                Console.WriteLine($"Le livreur #{((int)(valeur) + 1)} a livré la commande : #{commandeLivree[i].Numero}. Temps : {commandeLivree[i].TempsLivraison}");
                commandesPrepareTableau.Remove(uneCommande);
            }
            Thread.Sleep(dureeDistance);

            Console.WriteLine($"Le livreur #{((int)(valeur) + 1)} est de retour à la pizzeria");
        }
        else
        {
            Thread.Sleep(1);
        }
    }
}



void creerCommandes()
{

    for (int i = 0; i < NOMBRE_COMMANDES; i++)
    {
        int tempsPreparation = random.Next(TEMPS_MINIMUM_PREPARATION, TEMPS_MAXIMUM_PREPARATION);
        int tempsLivraison = 1000;
        int positionX = random.Next(-10, 11);
        int positionY = random.Next(-10, 11);

        Thread.Sleep(delaiCommande);
        Commande nouvelleCommande = new Commande(i + 1, new Position(positionX, positionY), tempsPreparation, tempsLivraison, false ,false, new Stopwatch());
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
            commandesPrepareTableau.Add(uneCommande);
        }
        else
        {
            Console.WriteLine("Aucune commande à traiter");
            Thread.Sleep(1000);
        }

        
    }
    
}



int calculerDistance(int x, int y)
{
    return Math.Abs(x) + Math.Abs(y);
}

int calculerDureeDistance(int depart, int arrive)
{
    return (arrive - depart);
}


void remplirTableauCommandeLivree(Commande[] tabCmd,Commande cmd)
{
    tabCmd[0] = cmd;
}

static void afficherCommandeLivree(Commande[] cmd)
{
    foreach(Commande c in cmd)
    {
        Console.WriteLine($"{c.Numero} à {c.Destination}");
    }
}






