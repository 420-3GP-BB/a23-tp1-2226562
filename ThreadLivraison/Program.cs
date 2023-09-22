using ThreadLivraison;
using static ThreadLivraison.ConstantesSimulation;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Collections;

Random random = new Random();
int delaiCommande = random.Next(DELAI_MINIMUM_COMMANDE, DELAI_MAXIMUM_COMMANDE);
ConcurrentQueue<Commande> lesCommandes = new ConcurrentQueue<Commande>();
ConcurrentQueue<Commande> lesCommandesPrepareFile = new ConcurrentQueue<Commande>();
List<Commande> lesCommandesPrepareListe = new List<Commande>();
List<Commande> lesCommandesLivre = new List<Commande>();

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

for (int i = 0; i < NOMBRE_LIVREURS; i++)
{
    livreurs[i] = new Thread(new ParameterizedThreadStart(livrerCommande));
}

for (int i = 0; i < NOMBRE_LIVREURS; i++)
{
    livreurs[i].Start(i);
}

//Console.WriteLine($"Temps de travail : {Utilitaires.Utilitaires.calculerTemps((int)((sw.ElapsedMilliseconds) * FACTEUR_ACCELERATION))}");

void livrerCommande(object? valeurObjet)
{
    int distance = 0;
    int dureeDistance = 0;
    int valeur = (int)valeurObjet;
    //List<Commande> commandeLivree = new List<Commande>();
    while (cuisiniers[0].IsAlive || !lesCommandesPrepareFile.IsEmpty)
    {
        //
        Commande uneCommande;
        if (lesCommandesPrepareFile.TryDequeue(out uneCommande))
        {
            if(uneCommande.EstLivrée == false)
            {
                remplirTableauCommandeLivree(uneCommande);

                Console.WriteLine($"Le livreur #{(valeur + 1)} va livré les commandes : ");
                afficherCommandeLivree(lesCommandesLivre);

                
                for (int i = 0; i < lesCommandesLivre.Count; i++)
                {
                    Console.WriteLine($"Le livreur #{(valeur + 1)} va livre la commande : #{lesCommandesLivre.ElementAt(i).Numero}");
                    dureeDistance = (calculerPosition(lesCommandesLivre.ElementAt(i).Destination.X, lesCommandesLivre.ElementAt(i).Destination.Y) * TEMPS_DEPLACEMENT) + TEMPS_PAIEMENT;

                    Thread.Sleep(dureeDistance);
                    //lesCommandesLivre.ElementAt(valeur).EstLivrée = true;
                    Console.WriteLine($"Le livreur #{(valeur + 1)} a livré la commande : #{lesCommandesLivre.ElementAt(i).Numero}. Temps : {lesCommandesLivre.ElementAt(i).TempsLivraison}");
                    lesCommandesPrepareListe.Remove(uneCommande);
                }
                Thread.Sleep(dureeDistance);

                Console.WriteLine($"Le livreur #{(valeur + 1)} est de retour à la pizzeria");
            }
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
        Commande nouvelleCommande = new Commande(i + 1, new Position(positionX, positionY), tempsPreparation, tempsLivraison, false, false, new Stopwatch());
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
            lesCommandesPrepareFile.Enqueue(uneCommande);
            lesCommandesPrepareListe.Add(uneCommande);
        }
        else
        {
            Console.WriteLine("Aucune commande à traiter");
            Thread.Sleep(1000);
        }


    }

}



int calculerPosition(int x, int y)
{
    return Math.Abs(x) + Math.Abs(y);
}

int calculerDistance(int depart, int arrive)
{
    return (arrive - depart);
}


void Ajouter(List<Commande> tabCmd, Commande cmd)
{
    tabCmd.Add(cmd);
}

void remplirTableauCommandeLivree(Commande uneCommande){
    List<Commande> livraison = new List<Commande>();
    Ajouter(livraison, uneCommande);
    
   
    for(int i=0; i<lesCommandesPrepareListe.Count;i++)
    {
        int depart;
        int arrive;
        if(i == 0)
        {
            depart = 0;
            arrive = calculerPosition(uneCommande.Destination.X, uneCommande.Destination.Y);
        }
        else
        {
            depart = calculerPosition(uneCommande.Destination.X, uneCommande.Destination.Y);
            arrive = calculerPosition(lesCommandesPrepareListe.ElementAt(i).Destination.X, lesCommandesPrepareListe.ElementAt(i).Destination.X);
            if ((calculerDistance(depart, arrive) < 10) && livraison.Count < 5)
            {
                Ajouter(lesCommandesLivre, lesCommandesPrepareListe.ElementAt(i));
                lesCommandesPrepareListe.ElementAt(i).EstLivrée = true;
                lesCommandesPrepareListe.Remove(lesCommandesPrepareListe.ElementAt(i));
            }


        }
        
        
    }

    
}

static void afficherCommandeLivree(List<Commande> cmd)
{
    if (cmd.Count == 0)
    { Console.WriteLine("liste vide"); }

    foreach (Commande c in cmd)
    {
        Console.WriteLine($"{c.Numero} à {c.Destination}");
    }
}