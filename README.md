# progettoVGD

Alessio update 09/03/22 commit "work in progress 2" Fix di un bug strano nel FieldOfView e altre piccole correzioni.. 

Alessio update 07/03/22 commit "work in progress" Aggiunte alcune cose sul player controller ma ancora da terminare. Aggiunto il campo visivo sul player ma ancora inutilizzato. 

Andrea update 06/03/22
Quando ci si avvicina ad un NPC si può inziare a parlare con E. Lo script si chiama NpcDialogue e le frasi possono essere inserite direttamente dall'inspector. Quando si avvia un discorso non si può andare avanti, bisogna ascolatrlo tutto, non è frustrante ed è la soluzione migliore io che abbia trovato.
Il discorso viene avviato quando si è abbastanza vicini e il puntatore del mouse (che viene messo al centro dello schermo in automatico se si clicca) punta all'NPC, la camera è sistemata per permettere questo.
Sono presenti due NPC, uno all'inzio e uno alla casa sulla destra.  

Alessio update 05/03/22 Commit: "Third person camera e movimento laterale"
Ho aggiunto il movimento laterale tramite tasti a e s. Mancano però le animazioni per la camminata laterale
e il controllo sul movimento obliquo, cioé quando si ha sia input verticale che orizzontale. 
Lo farò appena riesco. Ho aggiunto un sistema di camera piu avanzato che permette maggiore personalizzazione. 
Funziona tutto bene a meno delle cose che ho scritto prima. 


Elena update 03/03/2022
Ho fatto un dialogue system rudimentale, è bruttino ma funziona :)
premi F quando sei vicino a un NPC per parlare, premi F di nuovo per avanzare il dialogo, quando finisce la conversazione si chiude
per ora c'è erika in T-pose davanti al cavaliere se volete provarlo (non è la stessa erika che passeggia nel villaggio)
potete cambiare le frasi dall'editor sul npc nei campi del dialogue trigger
ho seguito questo tutorial https://www.youtube.com/watch?v=_nRzoTzeyxU
c'è solo il testo ed è bruttino da vedere per adesso, bisogna mettere il testo dentro un box bianco magari


Alessio update 01/03/22
Ho cambiato molte cose nel playerController.. sto cercando di rendere il movimento del player fluido e 
godibile il più possibile. Non ho commentato il codice che ho scritto perché non ho fatto in tempo. 
Appena ci rimetto mano lo faccio e cerco di finire il sistema di movimento del player.. poi ci concentriamo
sulle animazioni. Al momento quindi il movimento del player non è molto intuitivo e non è possibile fare
il roll.


Alessio update 28/02/22
Ho effetuato alcune modifiche nello script playerController, in cui modifico il controllo del player.
Ho anche aggiunto dei TODO per ricordarmi le prossime cose da fare e del perché vanno fatte. Le modifiche 
che ho fatto sono commentate e motivate.


26/02/22
TODO List:
-Animazione attacco con spada
-Parata e/o roll
-Generare gli npc
-Creare la scena foresta 
-Oggetti raccoglibili
-Interfaccia grafica dei menu
-animazione caduta libera
