using static ThreadLivraison.ConstantesSimulation;




string calculerTemps(int nbMillisecondes)
{
    const int MILLIS_PAR_HEURE = 3600000;
    const int MILLIS_PAR_MINUTE = 6000;
    const int MILLIS_PAR_SECONDE = 1000;

    int ss = (nbMillisecondes/ MILLIS_PAR_SECONDE) % 60;
    int mm = (nbMillisecondes / MILLIS_PAR_MINUTE) % 60;
    int hh = (nbMillisecondes / MILLIS_PAR_HEURE ) % 24;
    return $"{hh}:{mm}:{ss}";  
}
