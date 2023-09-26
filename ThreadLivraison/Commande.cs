using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitaires;
using static ThreadLivraison.ConstantesSimulation;

namespace ThreadLivraison
{
    internal class Commande
    {
        private bool _estLivree;
        private Stopwatch _stopwatch;
        public static int tempsLivraisonGlobal = 0;

        public void setEstLivree(bool estLivree)
        {
            _estLivree = estLivree;
            tempsLivraisonGlobal += TempsLivraison;
            _stopwatch.Stop();
            
        }

        public bool getEstLivree()
        {
            return _estLivree;
        }

        public int Numero
        {
            get;
            set;
        }

        public bool EstTraitee
        {
            get;
            set;
        }



        public Position Destination
        {
            get;
            set;
        }

        public int TempsPreparation
        {
            get;
            set;
        }

        public int TempsLivraison
        {
            get
            {
                return (int)(_stopwatch.ElapsedMilliseconds * FACTEUR_ACCELERATION);
            }
        }

        

        public Commande(int numero, Position destination, int tempsPrep, Stopwatch sw)
        {
            Numero = numero;
            Destination = destination;
            TempsPreparation = tempsPrep;
            _stopwatch = sw;
            sw.Start();
            
        }

        public string ToString()
        {
            return $"Commande #{Numero} ==> Temps de préparation : {Utilitaires.Utilitaires.calculerTemps((int)(TempsPreparation * FACTEUR_ACCELERATION))}, Destination : {Destination.ToString()}";
        }
    }
        

   }

