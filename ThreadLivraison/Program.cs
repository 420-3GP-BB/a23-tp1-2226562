using ThreadLivraison;
using static ThreadLivraison.ConstantesSimulation;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Collections;
using Utilitaires;

Random random = new Random();
int delaiCommande = random.Next(DELAI_MINIMUM_COMMANDE, DELAI_MAXIMUM_COMMANDE);
ConcurrentQueue<Commande> lesCommandes = new ConcurrentQueue<Commande>();
CommandeLivraison commandesPrepares = new CommandeLivraison();
const int QUARANTE_CINQ_MINUTES_EN_MILI = 2700000;
int livraisonsHorsDelai = 0;
object LOCK = new object();


Stopwatch chronoTravail = new Stopwatch();
chronoTravail.Start();

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

for (int i = 0; i < NOMBRE_LIVREURS; i++)
{
    livreurs[i] = new Thread(new ParameterizedThreadStart(livrerCommande));
}

for (int i = 0; i < NOMBRE_LIVREURS; i++)
{
    livreurs[i].Start(i);    
}

afficherStats();

void livrerCommande(object? valeurObjet)
{
    
    int dureeDistance = 0;
    int valeur = (int)valeurObjet;
    int distanceRetour = 0;
    List<Commande> livraisons = new List<Commande>();
    while (!verifierCuisiniersMorts() || !commandesPrepares.estVide())
    {
        livraisons = commandesPrepares.obtenirCommande();
        if (livraisons.Count == 0)
        {
            Thread.Sleep(1);
        }
        else
        {
            Console.WriteLine($"Le livreur #{(valeur + 1)} va livré les commandes : ");
            afficherCommandeLivree(livraisons);
            for (int i = 0; i < livraisons.Count; i++)
            {
                Console.WriteLine($"Le livreur #{(valeur + 1)} va livre la commande : #{livraisons[i].Numero}");
                if (i == 0)
                {
                    dureeDistance = (Utilitaires.Utilitaires.calculerPosition(livraisons[i].Destination.X, livraisons[i].Destination.Y) * TEMPS_DEPLACEMENT) + TEMPS_PAIEMENT;
                }
                else
                {
                    dureeDistance = (Math.Abs(livraisons[i].Destination.X - livraisons[i].Destination.X) + Math.Abs(livraisons[0].Destination.Y - livraisons[i].Destination.Y) * TEMPS_DEPLACEMENT) + TEMPS_PAIEMENT;
                }
                Thread.Sleep((int)(dureeDistance));
                livraisons[i].setEstLivree(true);
                Console.WriteLine($"Le livreur #{(valeur + 1)} a livré la commande : #{livraisons[i].Numero}. Temps : {Utilitaires.Utilitaires.calculerTemps(livraisons[i].TempsLivraison)}");
                if (livraisons[i].TempsLivraison > QUARANTE_CINQ_MINUTES_EN_MILI)
                {
                    livraisonsHorsDelai++;
                }
                if(i == livraisons.Count - 1)
                {
                    distanceRetour = (Utilitaires.Utilitaires.calculerPosition(livraisons[livraisons.Count - 1].Destination.X, livraisons[livraisons.Count - 1].Destination.Y) * TEMPS_DEPLACEMENT);
                }
            }
            for (int k = 0; k < livraisons.Count; k++)
            {
                livraisons.Remove(livraisons[k]);
            }

            Thread.Sleep((int)(distanceRetour));

            Console.WriteLine($"Le livreur #{(valeur + 1)} est de retour à la pizzeria");
        }
    }
}
    
   

void creerCommandes()
{

    for (int i = 0; i < NOMBRE_COMMANDES; i++)
    {
        int tempsPreparation = random.Next(TEMPS_MINIMUM_PREPARATION, TEMPS_MAXIMUM_PREPARATION);
        int positionX = random.Next(-10, 11);
        int positionY = random.Next(-10, 11);
        Thread.Sleep((int)(delaiCommande));
        Commande nouvelleCommande = new Commande(i + 1, new Position(positionX, positionY), tempsPreparation, new Stopwatch());
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
            Console.WriteLine($"Le cuisinier #{((int)(valeur) + 1)} commence la commande #{uneCommande.Numero} {Utilitaires.Utilitaires.calculerTemps((int)(uneCommande.TempsPreparation * FACTEUR_ACCELERATION))}");
            Thread.Sleep((int)(uneCommande.TempsPreparation));
            Console.WriteLine($"Le cuisinier #{((int)(valeur) + 1)} a terminé la commande #{uneCommande.Numero}");
            commandesPrepares.Ajouter(uneCommande);
        }
        else
        {
            Thread.Sleep(1);
        }


    }

}
void afficherCommandeLivree(List<Commande> cmd)
{
    foreach (Commande c in cmd)
    {
        Console.WriteLine($"           #{c.Numero} à {c.Destination}");
    }
}

void afficherStats()
{
    while (true)
    {
        if (verifierLivreursMorts())
        {
            Console.WriteLine($"Temps d'attente moyen : {calculerTempsAttenteMoyen()}");
            Console.WriteLine($"Temps d'exécution : {Utilitaires.Utilitaires.calculerTemps((int)((chronoTravail.ElapsedMilliseconds) * FACTEUR_ACCELERATION))}");
            Console.WriteLine($"Nombre de commandes hors délai : {livraisonsHorsDelai}");
            break;
        }
    }
}

string calculerTempsAttenteMoyen()
{
    return Utilitaires.Utilitaires.calculerTemps((int)((Commande.tempsLivraisonGlobal / NOMBRE_COMMANDES)));
}



bool verifierCuisiniersMorts()
{
    foreach(Thread cuis in cuisiniers)
    {
        if (cuis.IsAlive)
        {
            return false;
        }
    }
    return true;
}

bool verifierLivreursMorts()
{
    foreach(Thread liv in livreurs)
    {
        if(liv.IsAlive)
        {
            return false;
        }
    }
    return true;
}

