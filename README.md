# progettoVGD
- **Alessio update 28/03/22 commit "Effetti sonori e musica di background"**  
Mancano da aggiungere alcune clip audio e mancano da fare le hitbox del vicecapo.
Ora è presente:

	- Audio dei passi del player
	- Audio per il roll del player
	- Audio per l'attacco del player
	- Audio quando il player viene colpito
	- Audio per la morte del player
	- Urlo del player quando muore
	- Animazione di morte del player
	- Tre nuove tracce di background:
		- Una standard che parte subito
		- Una quando parte il combattimento con il vicecapo
		- Una per il combattimento con il boss finale  
**Le Musiche e le clip audio sono prese da Dark Souls e Dark Souls 3.. Sono quindi sicuramente soggette a copyright**

- **Alessio update 22/03/22 commit "Hitbox scheletri-player"**  
Gli scheletri hanno lo stesso meccanisco di collision detection del player per gli attacchi.
Adesso attaccano e si comportano allo stesso modo di prima, con la differenza che il player il danno lo riceve quando
effettivamente ha ricevuto il calcio da uno scheletro e non solo a stargli vicino. Seguirà lo sviluppo di una
AI per i nemici.

- **Alessio update 21/03/22 commit "Hitbox player-scheletri"**  
Perfezionato in modo sostanziale il meccanismo di collisione per gli attacchi del player. In particolare ci sono 3 box
collider che complessivamente coprono il braccio destro e la spada sel player. Si possono vedere selezionando il
player nella scena. Quando uno di quei 3 collider entra in contatto con la capsula di uno scheletro questo subirà danno.
Spiegherò meglio un'altra volta. L'attacco da parte del player è quasi sempre consistente con l'animazione e la posizione
rispetto al target, ma qualche rara volta capita che l'attacco sembra aver colpito lo scheletro ma in realtà non è
avvenuta nessuna collisione.. farò qualche altro test e vedrò se riesco a risolvere senza perdere troppo tempo.
Al momento gli scheletri non attaccano con successo il player e non arrecano quindi danno.. work in progress...

- **Alessio update 20/03/22 commit "Attacco semplice"**
Aggiustata la capsula del player. Migliorata notevolmente la meccanica di attacco, resa piu responsiva e coerente 
con la sua animazione. Codice aggiunto da commentare e riordinare. Seguiranno altre animazioni di attacco e la
possibilità di concatenazione di più attacchi. Successivamente sarà aggiornato il sistema di collision detection per
gli attacchi, e quindi il take damage.

- **Alessio update 18/03/22 commit "Roll"**
Il roll così dovrebbe andare bene.. mancherebbe solo da modificare la capsula del character controller durante
l'animazione come faceva prima. Ho iniziato a ordinare un po lo script PlayerController che era molto incasinato.

- **Alessio update 17/03/22 commit "Roll quasi finito"**
Ho quasi finito di implementare il roll con tutti i particolari

- **Alessio update 15/03/22 commit "Animator humanoid per il player"**  
Creato un nuovo animator per il player con rig di tipo humanoid e un blend tree 2D. Tutte le animazioni
ora sono per rig di tipo humanoid. Ci sono le animazioni per ogni direzione di movimento e per il roll,
manca l'animazione di attacco che aggiungerò presto. Avvolte il player sbarella, tipo quando scavalca un
ostacolo o è in pendenze ripide. Ciò è dovuto alla capsula del character controller troppo grande, per i 
problemi con gli scheletri. Fixerò questo problema appena finisco il player.

- **Elena update 14/03/22**  
Creato un game manager dove tenere traccia delle info del player, tipo lo status delle quest e il numero di mele. 
Iniziato a implementare i bottoni si/no della signora delle mele. A seconda di quale premi cambia il dialogo, controlla 
anche se hai abbastanza mele, e segna i flag del progresso della quest nel game manager. Manca 1) spawnare le mele 
(e implementare le funzioni per raccoglierle?) 2) dare la spada quando completi la quest. Comunque tutta sta roba 
della quest delle mele sta nel dialogue manager, che è molto brutto ma onestamente non so come gestirlo da file esterno 
e non ho sbatti, se qualcuno vuole provare e ci riesce sarebbe meglio :D

- **Alessio update 13/03/22 commit "fix dialog system"**  
Semplice fix del sistema di dialogo riguardo al fatto che delle volte lo stesso dialogo veniva avviato più volte in 
contemporanea

- **Alessio update 12/03/22 commit "Small Update"**  
Piccolo aggiornamento in cui ho ordinato e commentato gli script che gestiscono il dialog system, e qualche parte dello
script "PlayerController".
Corretti un paio di bug, aggiustata la capsula del character controller e modificato il sistema di ground check del player.
In particolare adesso viene controllata solo una parte della capsula, precisamente un area intorno ai piedi. Si può 
capire bene selezionando il player nel motore grafico, in cui si puo notare una sfera intorno ai piedi, che puo essere di 
colore verde trasparente o rossa trasparente, a seconda del risultato del check a runtime.

- **Alessio Update 11/03/22 commit "Dialog system"**  
Adesso si può iniziare un dialogo con un npc quando questo entra nel field of view (script FieldOfView) del player.
Le frasi pronuciate dall' npc non si aggiornano da sole ma bisogna premere un tasto qualsiasi, che permette anche di 
skippare "l'animazione" e terminare il dialogo. 
Tutto viene gestito dal gameObject "DialogueManager", a cui è attaccato lo script "DialogueManager"
Devo ancora ordinare e commentare il codice che ho scritto.  
Per aggiungere npc con dialoghi bisogna:
	- Attaccare al gameObject dell'npc lo script "DialogTrigger" presente in Assets -> 2 Scripts -> Dialogue, scritto 
	da Elena e leggermente modificato da me.
 	- Assegnare nell'ispector del gameObject dell'npc le frasi e l'Action Display e l'Action Text, implementati, credo, 
	da Andrea.
  	- Assegnare al gameObject dell'npc il layer "Npc Layer"

- **Alessio update 09/03/22 commit "work in progress 2"**  
Fix di un bug strano nel FieldOfView e altre piccole correzioni.. 

- **Alessio update 07/03/22 commit "work in progress"**  
Aggiunte alcune cose sul player controller ma ancora da terminare. Aggiunto il campo visivo sul player ma ancora 
inutilizzato. 

- **Andrea update 06/03/22**  
Quando ci si avvicina ad un NPC si può inziare a parlare con E. Lo script si chiama NpcDialogue e le frasi possono essere 
inserite direttamente dall'inspector. Quando si avvia un discorso non si può andare avanti, bisogna ascolatrlo tutto, non 
è frustrante ed è la soluzione migliore io che abbia trovato.
Il discorso viene avviato quando si è abbastanza vicini e il puntatore del mouse (che viene messo al centro dello schermo 
in automatico se si clicca) punta all'NPC, la camera è sistemata per permettere questo.
Sono presenti due NPC, uno all'inzio e uno alla casa sulla destra.  

- **Alessio update 05/03/22 Commit: "Third person camera e movimento laterale"**   
Ho aggiunto il movimento laterale tramite tasti a e s. Mancano però le animazioni per la camminata laterale
e il controllo sul movimento obliquo, cioé quando si ha sia input verticale che orizzontale. 
Lo farò appena riesco. Ho aggiunto un sistema di camera piu avanzato che permette maggiore personalizzazione. 
Funziona tutto bene a meno delle cose che ho scritto prima. 

- **Elena update 03/03/2022**  
Ho fatto un dialogue system rudimentale, è bruttino ma funziona :)
premi F quando sei vicino a un NPC per parlare, premi F di nuovo per avanzare il dialogo, quando finisce la conversazione 
si chiude per ora c'è erika in T-pose davanti al cavaliere se volete provarlo (non è la stessa erika che passeggia nel 
villaggio) potete cambiare le frasi dall'editor sul npc nei campi del dialogue trigger
ho seguito questo tutorial https://www.youtube.com/watch?v=_nRzoTzeyxU
c'è solo il testo ed è bruttino da vedere per adesso, bisogna mettere il testo dentro un box bianco magari

- **Alessio update 01/03/22**   
Ho cambiato molte cose nel playerController.. sto cercando di rendere il movimento del player fluido e 
godibile il più possibile. Non ho commentato il codice che ho scritto perché non ho fatto in tempo. 
Appena ci rimetto mano lo faccio e cerco di finire il sistema di movimento del player.. poi ci concentriamo
sulle animazioni. Al momento quindi il movimento del player non è molto intuitivo e non è possibile fare
il roll.
 
- **Alessio update 28/02/22**  
Ho effetuato alcune modifiche nello script playerController, in cui modifico il controllo del player.
Ho anche aggiunto dei TODO per ricordarmi le prossime cose da fare e del perché vanno fatte. Le modifiche 
che ho fatto sono commentate e motivate.
